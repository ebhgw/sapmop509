// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.sdbx.refreshstaging");

    load('custom/sdbx/Sdbxutil.fs')
    var envr = SdbxUtil.getEnvironment(element);
    _logger.info('Build ' + envr + ' (from Generational Models) starting');

    load('custom/util/Properties.fs');
    var imp_baseline_prop = Properties.loadCustomConfig('ImpBaseline');

    load('custom/modello/impbaseline/Baseliner.fs');
    var blnr = Baseliner(imp_baseline_prop);

    try {
        _logger.info('Refresh Staging for ' + envr);

        blnr.buildSdbxEnvironment(envr,false);
    } catch (excp) {
        _logger.error('Error building SystemTest : ' + excp);
    }

    _logger.info('Refresh Staging ' + envr + ' completed');
}) ();