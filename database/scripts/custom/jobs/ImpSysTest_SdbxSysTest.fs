// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("jobs");
_logger.info('Imp SandboxSystemTest job starting');

	load('custom/jobs/ImpBaselineCmd.fs');

	try {
		ImpBaselineCmd.processDataOnDb ();
		ImpBaselineCmd.runImpModelloBdi();
		ImpBaselineCmd.buildSdbxSysTest();
		
	} catch (excp) {
		_logger.error('Error ImpSdbxSysTest : ' + excp);
		formula.log.error('Error ImpSdbxSysTest : ' + excp);
	}

	_logger.info('Imp SandboxSystemTest completed');
	return true;
}) ();
