// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.impbaseline.importer");

    load('custom/util/Properties.fs');
    var imp_baseline_prop = Properties.loadCustomConfig('ImpBaseline');

    load('custom/lib/OrgsFinder.fs');
    // On Generational Models/Structure we define manually the mapping from Servizio Ecm (as
    // known in ECM) to Servizio NOC (as known in NOC)
    // This relies on the Ambiente property that should be set ow import won't work
    // This script sets Ambiente to the correct value and should be called before import
    var initStructure = function () {

        var envSetterVisitor = function (environment) {
            return {
                count:0,
                visit:function (child) {
                    clazz = child.elementClassname + '';
                    if (clazz == 'Servizio') {
                        child['Ambiente'] = environment;
                        this.count++;
                    }
                }
            }
        }

        var envRootDn = 'Ambiente=Produzione/gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services';
        var prodRoot = OrgsFinder.findElement(envRootDn);
        if (prodRoot) {
            prodRoot.walk(envSetterVisitor('Produzione'));
        } else {
            _logger.warn('initStructure: not found ' + envRootDn);
        }

        envRootDn = 'Ambiente=Produzione%2FSystemTest/gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services';
        var prodsystestRoot = OrgsFinder.findElement(envRootDn);
        if (prodsystestRoot) {
            prodsystestRoot.walk(envSetterVisitor('Produzione/SystemTest'));
        } else {
            _logger.warn('initStructure: not found ' + envRootDn);
        }

        envRootDn = 'Ambiente=SystemTest/gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services';
        var systestRoot = OrgsFinder.findElement(envRootDn);
        if (systestRoot) {
            systestRoot.walk(envSetterVisitor('SystemTest'));
        } else {
            _logger.warn('initStructure: not found ' + envRootDn);
        }
    }

    load('custom/lib/AdaptersUtil.fs');
    load('custom/modello/impbaseline/TlndCmder.fs');
    var tcmdr = TlndCmder(imp_baseline_prop);
    load('custom/modello/impbaseline/Baseliner.fs');
    var blnr = Baseliner(imp_baseline_prop);

    try {
        var res;
        /*
        _logger.info('Checking baseline');
        res = TlndCmder.checkBaseline ();
        _logger.info('Found ' + res.baseline + ' baseline elements, ' + res.servizi + ' servizi elements');
        if (res.baselineOk) {
        */
            
			initStructure();
            _logger.info('Loading data into db tables');
            tcmdr.processDataOnDb ();
			
            _logger.info('Restart adapter');
            AdaptersUtil.restartAdapter('ImpModello=Adapter%3A+ImpModello/root=Elements');
			AdaptersUtil.runBdiSchedule('RunOnce', 'Adapter: ImpModello');
            
			_logger.info('Building production');
            blnr.buildProduction();
            _logger.info('Building systest');
            blnr.buildSysTest();
            _logger.info('Built modello');
			
			// Costruzione viste specifiche
			load('custom/modello/beview/buildkoper.fs');
			buildkoper();
			_logger.info('Built view koper');

        /*
        } else {
            _logger.warn('Import Baseline stopped because baseline check failed');
        }
        */

    } catch (excp) {
        _logger.error('Importer error : ' + excp);
        formula.log.error('Importer error : ' + excp);
    }

    _logger.info('Importer completed');
    return true;
}) ();