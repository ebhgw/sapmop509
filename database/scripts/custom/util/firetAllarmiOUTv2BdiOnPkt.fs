/*  Send schedule event to tAllarmiOUTv2
    
        custom/
        Version 1.0
        E. Bomitali

        Updates:
        Ver    	Author          Description
        1.0    	E. Bomitali    Initial Release
		
		Tested with NOC 5.0

*/

(function () {

	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
	var opElement = null;
	load('util/login.fs');
	login('localhost',8080,'admin','formula');

	try {
		opElement = formula.Root.findElement( 'tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements' );
	} catch( excp ) {
		_logger.error( "Element " + viewDName + "not found: " + excp );
	}
	if( opElement ) {
		try {
			var ret = opElement.perform( session, "_Adapter|Run tAllarmiOUTv2", [], [] );
		} catch( excp ) {
			return false;
		}
	}
})();