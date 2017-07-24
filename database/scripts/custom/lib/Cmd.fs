/*
  Script Cmd.fs
  
  Author: E. Bomitali - Hogwart 2012
  Tested with versions: 5.0

  Wrap code to launch an external command file

*/

function Cmd () {}

Cmd.execute = function (cmd, logger) {
    var stream, output;
	
	try {
		logger.debug('cmd is ' + cmd);
		stream = formula.util.captureOutputStream( cmd );
		output = new java.lang.String( formula.util.toByteArray( stream ) );
		logger.debug('res is ' + output);
	} catch (excp) {
		logger.error('Error executing ' + cmd + ' : ' + excp);
	} 
	return output;
}
