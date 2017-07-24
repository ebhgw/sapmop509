/*
 Script ImpBaselineCmd.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0
 
// cmds to build baseline

*/

load('custom/lib/underscore.js');

// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit point
var ImpBaselineCmd = (function () {
	load('custom/orgs/TreeAligner.fs');
	load('custom/lib/Orgs.fs');
	load('custom/lib/ViewBuilder.fs');
	
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.impbaselinecmd");

    // mapping structure (ServizioGlobalView grouping and mapping ServizioNomeNoc - ServizioNomeEcm) is defined
    // manually, so we add the Ambiente property in case user didn't do.
    var setEnv4MapppingStructure = function () {

        var buildEnvSetterVisitor = function (environment) {
            return {
                count: 0,
                visit: function ( child )
                {
                    clazz = child.elementClassname + '';
                    if (clazz == 'Servizio' || clazz == 'Acronimo' || clazz == 'AcronimoOpVw') {
                        child['Ambiente'] = environment;
                        this.count++;
                    }
                }
            }
        }

        // Update structure used for mapping Ecm Services to NOC Services setting environment
        // property that will be used by bscm
        var prodVisitor = buildEnvSetterVisitor('Produzione');
        var prodRoot = Orgs.findElement('Ambiente=Produzione/gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services');
        prodRoot.walk(prodVisitor);
        var prodsystestVisitor = buildEnvSetterVisitor('Produzione/SystemTest');
        var prodsystestRoot = Orgs.findElement('Ambiente=Produzione%2FSystemTest/gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services');
        prodsystestRoot.walk(prodsystestVisitor);
        var systestVisitor = buildEnvSetterVisitor('SystemTest');
        var systestRoot = formula.Root.findElement('Ambiente=SystemTest/gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services');
        systestRoot.walk(systestVisitor);
    }

    // This function perform all the computation needed in order to generate
    // an updated noc_ref_acronimi and noc_ref_servizi table
    var _processDataOnDb = function () {
        try {
            _logger.info('ImpBaseline job starting');

            // Should end with '\\'
            // original var cmdDir = java.lang.System.getProperty("formula.home") + '..\\tlnd\\jobs\\'
            var cmdDir = java.lang.System.getProperty("formula.home") + '\\..\\..\\tlnd\\jobs\\'

            var cmdExpMap, cmdImpBaseline, cmdImpServizi, stream, output, res, stop = false;

            // set the environment on mapping structure, will be used later when bscm
            // copies properties

            setEnv4MapppingStructure();

            // Export Servizi Maps
            // Firstly export mapping between Servizi names as known in ECM and Servizi names as shown in NOC
            _logger.info('Export Mapping Nomi Servizi to csv');
            state.ModelExporter.doExport();
            _logger.info('Launching ExpMapNomiServizi tlnd job');
            cmdExpMap = cmdDir + 'ExpMapNomiServizi\\ExpMapNomiServizi_run.bat';
            stream = formula.util.captureOutputStream( cmdExpMap );
            output = new java.lang.String( formula.util.toByteArray( stream ) );
            _logger.debug(output);

            // Import Baseline
            // Copy data from ECM sources, add mapping from Servizio Ecm to Servizio NOC
            _logger.info('Launching ImpBaseline tlnd job');
            cmdImpBaseline = cmdDir + 'ImpBaseline\\ImpBaseline_run.bat';
            stream = formula.util.captureOutputStream( cmdImpBaseline );
            output = new java.lang.String( formula.util.toByteArray( stream ) );
            _logger.debug(output);
            var pattSuccess = /::OK::/im;
            res = pattSuccess.exec(output);

            if (res == null) {
                _logger.info('ImpBaseline tlnd job failed');
                return;
            }
            _logger.info('ImpBaseline tlnd job ok');


            // If there are no data in import table, tlnd job says stop import
            // as there is nothing to do
            var pattProceed = /stop import/im;
            res = pattProceed.exec(output);
            if (res != null) {
                _logger.info('ImpBaseline tlnd job stopped as no data was found');
                return;
            }

            _logger.info('Launching ImpServizi tlnd job');
            cmdImpServizi = cmdDir + 'ImpServizi\\ImpServizi_run.bat';
            stream = formula.util.captureOutputStream( cmdImpServizi );
            output = new java.lang.String( formula.util.toByteArray( stream ) );
            _logger.info(output);

            // check if job ok, if job ok check if data was found
            // whenever an empty table was found, should stop job to keep old data
            res = pattSuccess.exec(output);
            if (res == null) {
                _logger.info('ImpServizi tlnd job failed');
                return;
            }
            _logger.info('ImpServizi tlnd job ok');
            var pattProceed = /stop import/im;
            res = pattProceed.exec(output);
            if (res != null) {
                _logger.info('ImpBaseline tlnd job stopped as no data was found');
                return
            }
        } catch (excp) {
            _logger.error('ImpBaseline excp: ' + excp);
        }
    }

    // generate the event in order to trigger RunOnce schedule, that is fire query to db and update
    // the structure un Elements with data from updated tables
    var _runImpModelloBdi = function () {
        // Now rebuild the structure under Generational Models, the structure will be used as a template for Production
        _logger.info('Send bdi event to ImpModello');
        load( 'util/bdievent' );
        fireBDIEvent( 'RunOnce' , 'Adapter: ImpModello' );
        // wait 5 seconds
        Packages.java.lang.Thread.sleep(10000);
    }


    // Building Production structure
    // 1. generate Generational Models/Baseline+Alarmed Elements both for Acronimi and Servizi (with Delete-Before-Execute)
    // 2. align Service Models/Production to Generational Models/Baseline+Alarmed Elements removing extra elements
    //    and adding new ones
    // 3. link Acronimo elements in Servizi structure to ClasseCompInfra elements (children to Acronimi) in Acronimi structure
    var _buildProduction = function () {
        // Baseline+Alarmed Elements
        _logger.info('Env:Production, building view Baseline+Alarmed Elements/Acronimi');
        ViewBuilder.build('gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services');
        _logger.info('Env:Production, building view Baseline+Alarmed Elements/Servizi');
        ViewBuilder.build('gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services');

        // align Production/Acronimi
        _logger.info('Env:Production, align Production/Acronimi');
        templateDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
        targetDn = 'gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'
        _logger.info('Production aligning ' + targetDn + ' to template ' + templateDn);
        TreeAligner.alignToTemplate(templateDn,targetDn)

        //align Production/Servizi
        _logger.info('Env:Production, align Production/Servizi');
        var templateDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
        var targetDn = 'gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'
        _logger.info('Production aligning Servizi ' + targetDn + ' to template ' + templateDn);
        TreeAligner.alignToTemplate(templateDn,targetDn)

        // Link Servizi 2 Acronimi structures
        _logger.info('Env:Production, link (bscm) Servizi to Acronimi structure');
        ViewBuilder.build('gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations');
    }

    // helper function, collect acronimo found within the dn subtree
    var _collectAcroFullname = function (dn) {
        var visitor = {
            acroList: [],
            count: 0,
            visit: function ( ele ) {
                var clazz = ele.elementClassname + '';
                if (clazz == 'Acronimo') {
                    visitor.acroList.push('Acronimo=' + ele.Name);
                    visitor.count++;
                }
            }
        }
        var treeRoot = Orgs.findElement(dn);
        if (treeRoot)
            treeRoot.walk(visitor);
        return visitor.acroList;
    }

    // Building SystemTest
    // 1. generate Generational Models/Baseline+AlarmedElement_SystemTest both for Acronimi and Servizi (with Delete-Before-Execute)
    // 2. align Service Models/SystemTest to Generational Models/Baseline+AlarmedElement_SystemTest removing extra elements
    //    and adding new ones
    // 3. link Acronimo elements in Servizi structure to ClasseCompInfra elements (children to Acronimi) in Acronimi structure
    var _buildSysTest = function (rebuildGenMod) {
		rebuildGenMod = rebuildGenMod || true;
		
		if (rebuildGenMod) {
			// Baseline+AlarmedElement_SystemTest
			_logger.info('Env:SystemTest, building view Baseline+Alarmed Elements(SystemTest)/Acronimi');
			ViewBuilder.build('gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services');
			_logger.info('Env:SystemTest, building view Baseline+Alarmed Elements/Servizi');
			ViewBuilder.build('gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services');
		}
        // align Production/Acronimi
        // should cope with acronimo that should go only in sdbxsystemtest/staging and should not be included in systemtest
        _logger.info('Env:SystemTest, align SystemTest/Acronimi');

        // compute list of acronimos to be excluded
        var baselineSrvzDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services';
        var acroList = _collectAcroFullname(baselineSrvzDn);
		_logger.info('Env:SystemTest, collectAcroFullname on baseline completed');
        var sdbxStagingAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations';
        var sbxAcroList = _collectAcroFullname(sdbxStagingAcroDn);
		_logger.info('Env:SystemTest, collectAcroFullname on sdbxStaging completed');
        var diffList = _.difference(sbxAcroList, acroList);
		_logger.info('Env:SystemTest, difference');
        //_logger.debug('buildSystemTest: exclude list -------->');
        //_.each(diffList, function(str) { _logger.debug(str)});
        //_logger.debug('<--------')

        //align Acronimi, copy only acronimi linked to a Servizio
        var baselineAcroDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services';
        var modelloAcroDn = 'gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations';
        var modelloAcro = Orgs.findElement(modelloAcroDn);
		_logger.info('ImpBaselineCmd.buildSysTest: ' + 'deleteFirstLevNotInTemplate');
        // take acronimi from baseline excluding the one defined only in staging
		_logger.info('Env:SystemTest, deleteFirstLevNotInTemplate');
        TreeAligner.deleteFirstLevNotInTemplate(baselineAcroDn, modelloAcroDn, diffList);
		_logger.info('ImpBaselineCmd.buildSysTest: ' + 'addFirstLev4Template');
        TreeAligner.addFirstLev4Template(baselineAcroDn, modelloAcroDn, diffList);
        // align structure for each acronimo
		_logger.info('ImpBaselineCmd.buildSysTest: looping on children acronimi');
        _.each(modelloAcro.Children, function(dn) {
            var pdn = state.DNamer.parseDn(dn);
            var templateDn = 'Acronimo='+ formula.util.encodeURL(pdn.encodedName) + '/' + baselineAcroDn;
			_logger.debug('ImpBaselineCmd.buildSysTest: checking ' + templateDn);
            var template = Orgs.findElement(templateDn);
            if (template) {
                _logger.info('Aligning ' + dn + ' to ' + template);
                TreeAligner.alignToTemplate(templateDn,dn);
            } else {
                _logger.warn('Not found in baseline: ' + templateDn);
            }
        })

        //align SystemTest/Servizi
        var baselineSrvzDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services';
        var modelloSrvzDn = 'gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations';
        _logger.info('buildSystemTest step Servizi, aligning ' + modelloSrvzDn + ' to base ' + baselineSrvzDn);
        TreeAligner.alignToTemplate(baselineSrvzDn,modelloSrvzDn);
        _logger.info('buildSystemTest step Servizi, completed');

        // Link Servizi 2 Acronimi
        _logger.info('Env:SystemTest, link (bscm) Servizi to Acronimi structure');
        ViewBuilder.build('gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations');
		
    }

    // Building SdbSystemTest/Staging
    // 1. build a baseline for acronimi without alarmed elements under Generational Models/Structure/Modello/SystemTest/Acronimi, bscm with delete-before-execute
    // As the Sdbx purpouse is to experiment we do not want elements generated dinamically
    // so we have to generate a structure different from Generational Models/Baseline+AlarmedElement_SystemTest
    // 2. align SdbxSystemTest/Staging/Acronimi to baseline
    var _buildSdbxSysTest = function (rebuildGenMod) {
	
		rebuildGenMod = rebuildGenMod || true;
		
		if (rebuildGenMod) {
			// Modello/SystemTest/Baseline/Acronimi
			_logger.info('Env:SandboxSystemTest, building view Generational Models/Structure/Modello/SystemTest/Baseline/Acronimi');
			ViewBuilder.build(	'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=SystemTest/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services');
		}

        // SdbxSystemTest/Staging/Acronimi
        _logger.info('Env:SandboxSystemTest, align SandboxSystemTest/Staging/Acronimi');
        var stagingAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations';
        var baselineDn = 'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=SystemTest/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
        var stagingAcro = Orgs.findElement(stagingAcroDn);
        var baseline = Orgs.findElement(baselineDn);

        // if found continue
        if (stagingAcro && baseline) {
            _logger.info('Looping on Sdbx/Staging/Acronimo elements');
            _.each(stagingAcro.Children, function(dn) {
                var pdn = state.DNamer.parseDn(dn);
                var acroBaselineDn = 'Acronimo='+ formula.util.encodeURL(pdn.encodedName) + '/' + baselineDn;
                var acroBaseline = Orgs.findElement(acroBaselineDn);
                if (acroBaseline) {
                    _logger.info('Aligning ' + dn + ' to ' + acroBaselineDn);
                    TreeAligner.alignToTemplate(acroBaselineDn,dn);
                } else {
                    _logger.warn('Not found in baseline: ' + acroBaselineDn);
                }
            })
			_logger.info('Looping on Sdbx/Staging/Acronimo elements completed');
        } else {
            _logger.warn('Not found Modello or Baseline root element');
        }

        // Link Servizi 2 Acronimi
        _logger.info('Env:SandboxSystemTest, link (bscm) Servizi to Acronimi structure');
		var viewElement = formula.Root.findElement( 'gen_folder=Servizi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations');
		viewElement.perform( session, "ViewBuilder|Run", [], [] );
		_logger.info('Env:SandboxSystemTest, link (bscm) Servizi to Acronimi structure completed');

    }

    return {
        processDataOnDb:_processDataOnDb,
        runImpModelloBdi:_runImpModelloBdi,
        buildProduction:_buildProduction,
        buildSysTest:_buildSysTest,
        buildSdbxSysTest:_buildSdbxSysTest
    }
})();
