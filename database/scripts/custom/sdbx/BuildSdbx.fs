// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.sdbx.buildsdbx");
	
	load('custom/sdbx/Sdbxutil.fs')
	var envr = SdbxUtil.getEnvironment(element);
	_logger.info('Build ' + envr + ' (from Generational Models) starting');

    load('custom/util/Properties.fs');
    var imp_baseline_prop = Properties.loadCustomConfig('ImpBaseline');
	load('custom/lib/AdaptersUtil.fs');
	load('custom/modello/impbaseline/Baseliner.fs');
    var blnr = Baseliner(imp_baseline_prop);

	try {
			_logger.info('Building Sandbox for ' + envr);
			AdaptersUtil.restartAdapter('ImpModello=Adapter%3A+ImpModello/root=Elements');
			AdaptersUtil.runBdiSchedule('RunOnce', 'Adapter: ImpModello');
			_logger.info('Adapter ImpModello restarted');
			blnr.buildSdbxEnvironment(envr);
            _logger.info('Built Sdbx');
	} catch (excp) {
		_logger.error('Error building SystemTest : ' + excp);
	}

	_logger.info('Building Sdbx ' + envr + ' completed');
}) ();