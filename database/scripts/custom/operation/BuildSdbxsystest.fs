// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.op_sdbx_buildsdbxsystest");
_logger.info('Build SystemTest (from Generational Models) starting');

	load('custom/jobs/ImpBaselineCmd.fs');

	try {
		ImpBaselineCmd.runImpModelloBdi();
		ImpBaselineCmd.buildSdbxSysTest();
	} catch (excp) {
		_logger.error('Error building SystemTest : ' + excp);
	}

	_logger.info('Building SystemTest completed');
}) ();