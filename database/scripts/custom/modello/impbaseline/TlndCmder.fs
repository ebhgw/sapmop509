/*
 Script ImpBaselineCmd.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 // cmds to build baseline

 */



// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit point
var TlndCmder = function (cfgprop) {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.impbaseline.tlndcmder");

    load('custom/util/Properties.fs');

    var _checkBaseline = function () {
        var bc = null;
        try {
            bc = _getBaselineCounters();
            _logger.debug('_checkBaseline counters from getBaselineCounters, jobCompleted ' + bc.jobCompleted + ', baseline ' + bc.baseline + ', servizi ' + bc.servizi)

            if (bc.jobCompleted == false) {
                bc.baselineOk=false;
            } else if (bc.jobCompleted && (bc.baseline == 0 || bc.servizi == 0)) {
                bc.baselineOk=false;
            } else {

                _logger.info('checkBaseline reading counters');
                sc = _readSavedCounters();
                _logger.info('_checkBaseline saved counters baseline ' + sc.baseline + ' servizi ' + sc.servizi);
                // se il numero di nuovi elementi � minore del 90% di quelli vecchi, check false
                if (bc.baseline > (sc.baseline * 0.9) && bc.servizi > (sc.servizi * 0.9)) {
                    bc.baselineOk=true;
                } else {
                    _logger.error ('checkBaseline: suspect number of elements, not rebuilding baseline');
                    _logger.error ('checkBaseline counters ' + bc.baseline + ' baseline, ' + bc.servizi + ' servizi\nsaved counters ' + bc.baseline + ' baseline, ' + bc.servizi + ' servizi');
                    bc.baselineOk=false;
                }
                _logger.debug('checkBaseline writing counters');
                _persistCounters(bc);
            }
        } catch (excp) {
            _logger.error('checkBaseline got error ' + excp)
        }

        _logger.info('_checkBaseline returning baselineOk ' + bc.baselineOk + ', baseline ' + bc.baseline + ', servizi ' + bc.servizi)
        return bc;
        // have to check statistics
        // 1. load old counters
        // 2. check new counters against old
        // 3. persiste new counters
    }

    // Launch jobs on the db in order to populate the tables
    // - noc_ref_acronimi
    // - noc_ref_servizi
    var _processDataOnDb = function () {
        try {
            _logger.info('processDataOnDb starting');

            var tlndJobDir = cfgprop.get("custom.tlnd.basedir") + '/jobs/'
            _logger.debug('_processDataOnDb talend job dir ' + tlndJobDir);

            var cmdExpMap, cmdImpBaseline, cmdImpServizi, cmdExecRes, output, res, stop = false;

            // -------------------------------------------------------------------------
            // Export Servizi Maps. Esporta su file e poi un tlnd job lo carica sul db
            _logger.debug('_processDataOnDb Export Mapping Nomi Servizi to csv');
            state.ModelExporter.doExport();
            _logger.debug('_processDataOnDb Launching ExpMapNomiServizi tlnd job');
            cmdExpMap = tlndJobDir + 'ExpMapNomiServizi/ExpMapNomiServizi_run.bat';
            _logger.debug('processDataOnDb launching ' + cmdExpMap);
            cmdExecRes = server.executeExternalProcess( [cmdExpMap], [] );

            if ( cmdExecRes.hasErrors() ) {
                _logger.error("processDataOnDb got error launching ExpMapNomiServizi_run.bat >" + cmdExecRes.getErrorsAsSingleString() + "<");
            }
            _logger.debug('processDataOnDb ExpMapNomiServizio output ' +  cmdExecRes.getOutputAsSingleString());
            _logger.info('processDataOnDb ExpMapNomiServizio completed');

            // -------------------------------------------------------------------------
            // Import Baseline, copy data from ECM sources, add mapping from Servizio Ecm to Servizio NOC
            cmdImpBaseline = tlndJobDir + 'ImpBaseline/ImpBaseline_run.bat';
            _logger.debug('processDataOnDb launching ' + cmdImpBaseline);
            cmdExecRes = server.executeExternalProcess([cmdImpBaseline], []);
            output = cmdExecRes.getOutputAsSingleString();
            if (cmdExecRes.hasErrors()) {
                _logger.error("getBaselineCounters got error launching ImpBaseline_run.bat >" + cmdExecRes.getErrorsAsSingleString() + "<");
            }
            output = cmdExecRes.getOutputAsSingleString();
            _logger.debug('processDataOnDb ImpBaseline output ' +  cmdExecRes.getOutputAsSingleString());
            var pattSuccess = /::OK::/im;
            res = pattSuccess.exec(output);

            if (res == null) {
                _logger.error('ImpBaseline tlnd job failed');
                return;
            }
            _logger.info('processDataOnDb ImpBaseline (Acronimo to Risorsa) completed');


            // If there are no data in import table, tlnd job says stop import
            // as there is nothing to do
            var pattProceed = /stop import/im;
            res = pattProceed.exec(output);
            if (res != null) {
                _logger.info('ImpBaseline tlnd job stopped as no data was found');
                return;
            }

            // -------------------------------------------------------------------------
            // Import Servizi
            _logger.debug('Launching ImpServizi tlnd job');
            cmdImpServizi = tlndJobDir + 'ImpServizi/ImpServizi_run.bat';
            _logger.debug('processDataOnDb launching ' + cmdImpServizi);
            cmdExecRes = server.executeExternalProcess([cmdImpServizi], [] );
            if (cmdExecRes.hasErrors()) {
                _logger.error("processDataOnDb getBaselineCounters got error launching ImpServizi_run.bat. Output >" + cmdExecRes.getErrorsAsSingleString() + "<");
            }
            output = cmdExecRes.getOutputAsSingleString();
            _logger.debug('processDataOnDb ImpServizi output ' +  cmdExecRes.getOutputAsSingleString());

            // check if job ok, if job ok check if data was found
            // whenever an empty table was found, should stop job to keep old data
            res = pattSuccess.exec(output);
            if (res == null) {
                _logger.info('processDataOnDb ImpServizi tlnd job failed');
                return;
            }
            _logger.info('processDataOnDb ImpServizi (Servizi to Acronimo) completed');
            var pattProceed = /stop import/im;
            res = pattProceed.exec(output);
            if (res != null) {
                _logger.info('ImpBaseline tlnd job stopped as no data was found');
                return
            }
        } catch (excp) {
            _logger.error('_processDataOnDb excp: ' + excp);
        }
        _logger.info('processDataOnDb returning');
        return
    }

    return {
        //checkBaseline: _checkBaseline,
        //persistCounters:_persistCounters,
        processDataOnDb:_processDataOnDb
        //readSavedCounters:_readSavedCounters
    }
};
