/*
  Script Cmd.fs
  
  Author: E. Bomitali - Hogwart 2012
  Tested with versions: 5.0

  Wrap code to launch an external command file

*/


var runCmd = (function Cmd () {
	var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.runCmd');

	var execute = function (cmd) {
		var stream, output;
		try {
			_logger.debug('cmd is ' + cmd);
			stream = formula.util.captureOutputStream( cmd );
			output = new java.lang.String( formula.util.toByteArray( stream ) );
			_logger.debug('cmd output is\n' + output + '\n>>>>>>>>>>>>>>>\n');
		} catch (excp) {
			_logger.error('Error executing ' + cmd + ' : ' + excp);
		} 
		return output;
	}
	
	return {
		execute:execute
	}
})();
