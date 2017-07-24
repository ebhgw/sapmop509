// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("jobs");
_logger.info('Import SystemTest starting');

	load('custom/jobs/ImpBaselineCmd.fs');

	try {
		ImpBaselineCmd.processDataOnDb ();
		ImpBaselineCmd.runImpModelloBdi();
		ImpBaselineCmd.buildSysTest();
		
	} catch (excp) {
		_logger.error('Error importing SystemTest : ' + excp);
	}

	_logger.info('Import SystemTest  completed');
	return true;
}) ();
