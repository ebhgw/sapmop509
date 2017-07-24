// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("jobs");
_logger.info('Generate Sandbox SystemTest starting');

	load('custom/jobs/ImpBaselineCmd.fs');

	try {
		ImpBaselineCmd.processDataOnDb ();
		ImpBaselineCmd.runImpModelloBdi();
		ImpBaselineCmd.buildSdbxSysTest();
		
	} catch (excp) {
		_logger.error('Error importing Sandbox SystemTest : ' + excp);
	}

	_logger.info('Generate Sandbox SystemTest  completed');
	return true;
})();
