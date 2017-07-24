// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("jobs");
_logger.info('Import SandboxSystemTest job starting');

	load('custom/jobs/ImpBaselineCmd.fs');

	try {
		ImpBaselineCmd.processDataOnDb ();
		ImpBaselineCmd.runImpModelloBdi();
		ImpBaselineCmd.buildSdbxSysTest();
	} catch (excp) {
		_logger.error('Error on Import SandboxSystemTest : ' + excp);
	}

	_logger.info('Import SandboxSystemTest completed');
	return true;
}) ();
