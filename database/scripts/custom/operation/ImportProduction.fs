// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("jobs");
_logger.info('Import Production starting');

	load('custom/jobs/ImpBaselineCmd.fs');

	try {
		ImpBaselineCmd.processDataOnDb ();
		ImpBaselineCmd.runImpModelloBdi();
		ImpBaselineCmd.buildProduction();
		
	} catch (excp) {
		_logger.error('Error importing Production: ' + excp);
	}

	_logger.info('Import Production completed');
	return true;
}) ();
