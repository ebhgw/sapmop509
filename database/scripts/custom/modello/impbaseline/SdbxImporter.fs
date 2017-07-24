// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.importbaseline.impsdbx");

    load('custom/sdbx/Sdbxutil.fs')
    var envr = SdbxUtil.getEnvironment(element);
    _logger.info('Build ' + envr + ' (from Generational Models) starting');
	
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

    load('custom/util/Properties.fs');
    var imp_baseline_prop = Properties.loadCustomConfig('ImpBaseline');
    load('custom/lib/AdaptersUtil.fs');
    load('custom/modello/impbaseline/TlndCmder.fs');
    var tcmdr = TlndCmder(imp_baseline_prop);
    load('custom/modello/impbaseline/Baseliner.fs');
    var blnr = Baseliner(imp_baseline_prop);

    try {

        _logger.info('Building Sandbox for ' + envr);
        initStructure();
        _logger.info('Loading data into db tables');
        tcmdr.processDataOnDb ();
        _logger.info('Restart adapter');
        AdaptersUtil.restartAdapter('ImpModello=Adapter%3A+ImpModello/root=Elements');
        _logger.info('Adapter ImpModello restarted');

        blnr.buildSdbxEnvironment(envr);
        _logger.info('Built Sdbx');
    } catch (excp) {
        _logger.error('Error building SystemTest : ' + excp);
    }

    _logger.info('Import Sdbx ' + envr + ' completed');
    return true;
})();
