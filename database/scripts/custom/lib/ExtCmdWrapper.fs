/*
  Script ExtCmdWarpper.fs
  
  Author: E. Bomitali - Hogwart - 2013
  Tested with versions: 5

  Launch external command and collect result
*/


function ExtCmdWrapper () {
}

/* 
	cmd is a string containing the command to be executed, i.e. with a path in Unix separator format, that is "/" comprehensive of arguments
	resCatcher is a regexp used to parse output, if match is ok return success ow failure. Example /job success/im
*/ 
ExtCmdWrapper.run = function (cmd) {
	var strem, output;
	stream = formula.util.captureOutputStream( cmd );
	output = new java.lang.String( formula.util.toByteArray( stream ) );
	formula.log.info("ExtCmdWrapper output\n>>>>>\n" + output + "\n>>>>>\n");
	
	return output;
}

ExtCmdWrapper.runWithFilteredRes = function (cmd, filteringCallback) {
	var strem, output, res;
	stream = formula.util.captureOutputStream( cmd );
	output = new java.lang.String( formula.util.toByteArray( stream ) );
	//formula.log.info("ExtCmdWrapper output\n>>>>>\n" + output + "\n>>>>>\n");
	res = filteringCallback(output);
	return res;
}