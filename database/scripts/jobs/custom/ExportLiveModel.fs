// run only if isCoordinator
// 1. write file (on a local directory, does not need to be coordinator
// 2. upload to db tables (only if coordinator, to avoid multiple writers)

var logger = Packages.org.apache.log4j.Logger.getLogger("Dev.ExportModel");
logger.debug('ExportLiveModel job starting -- run only if coordinator');
if (server.isCoordinator())
{
    try {
	logger.info('ExportLiveModel job starting');

	// write file to E:\appliation\noc00\NovellOperationsCenter\tlnd\data\export_*.csv
	// Should end with '\\'
	// original var cmdDir = java.lang.System.getProperty("formula.home") + '..\\tlnd\\jobs\\'
	var cmdDir = java.lang.System.getProperty("formula.home") + '\\..\\..\\tlnd\\jobs\\'
	
	var cmdExpMap, cmdExpAcronim, cmdExpServizi, stream, output;

	logger.info('Exporting Live Model');
	state.ModelExporter.exportAcronimi();
	state.ModelExporter.exportServizi();

	cmdExpAcronimi = cmdDir + 'ExpAcronimi\\ExpAcronimi_run.bat';
	stream = formula.util.captureOutputStream( cmdExpAcronimi );
	output = new java.lang.String( formula.util.toByteArray( stream ) );
	logger.info(output);

	cmdExpServizi = cmdDir + 'ExpServizi\\ExpServizi_run.bat';
	stream = formula.util.captureOutputStream( cmdExpServizi );
	output = new java.lang.String( formula.util.toByteArray( stream ) );
	logger.info(output);

	} catch (excp) {
		logger.error('Error exporting Live Model : ' + excp);
	}

	logger.info('ExportLiveModel job ended');
}
else
{
	logger.debug('ExportLiveModel job skipped as server is not cluster coordinator');
}
