(function () {
	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
	var cmdDir = java.lang.System.getProperty("formula.home") + '\\..\\..\\tlnd\\jobs\\'
	var ok = false;

	_logger.info('Export Mapping Nomi Servizi, Servizi, Acronimi to csv');
	state.ModelExporter.doExport();
	
	_logger.info('Launching ExpAcronimi tlnd job');
	cmdExpMap = cmdDir + 'ExpAcronimi\\ExpAcronimi_run.bat';
	stream = formula.util.captureOutputStream( cmdExpMap );
	output = new java.lang.String( formula.util.toByteArray( stream ) );
	_logger.debug(output);	
	var pattSuccess = /::OK::/im;
	res = pattSuccess.exec(output);
	
	if (res == null) {
		_logger.info('ExpAcronimi tlnd job failed');
		session.sendMessage('Export acronimi failed, see log');
		return 1;
	}
	
	_logger.info('Launching ExpServizi tlnd job');
	cmdExpMap = cmdDir + 'ExpServizi\\ExpServizi_run.bat';
	stream = formula.util.captureOutputStream( cmdExpMap );
	output = new java.lang.String( formula.util.toByteArray( stream ) );
	_logger.debug(output);	
	var pattSuccess = /::OK::/im;
	res = pattSuccess.exec(output);
	
	if (res == null) {
		_logger.info('ExpServizi tlnd job failed');
		session.sendMessage('Export servizi NOC failed, see log');
		return 1;
	}
	
	_logger.info('ExpModelloNOC tlnd job ok');
	session.sendMessage('Export modello NOC completed');
	return 0;
})();