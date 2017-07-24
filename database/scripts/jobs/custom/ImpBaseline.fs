// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit poing
(function () {
	var _logger = Packages.org.apache.log4j.Logger.getLogger("jobs");
	_logger.info('ImpBaseline job starting -- run only if coordinator');

	if (!server.isCoordinator()) {
		_logger.debug('ImpBaseline skipped as server is not cluster coordinator');
		return;
	} else {
		load('custom/modello/impbaseline/Importer.fs');
	}
	_logger.info('ImpBaseline job end');
	return true;
}) ();
