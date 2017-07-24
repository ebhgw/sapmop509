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

	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.fireAllarmiOUTv2");
	var opElement = null;
	load('util/login.fs');
	var hostname = Packages.java.net.InetAddress.getLocalHost().getHostName();
	// l'hostname è anche registrato in formula.web.server.host=SVAPMOT048
	// per cui si potrebbe ottenere con Packages.java.lang.System.getProperty( "formula.web.server.host")
	login(hostname,8080,'admin','formula');

	try {
		opElement = formula.Root.findElement( 'tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements' );
	} catch( excp ) {
		_logger.error( "Element " + viewDName + "not found: " + excp );
	}
	if( opElement ) {
		try {
			_logger.debug("Launch Operation Run tAllarmiOUTv2");
			var ret = opElement.perform( session, "_Adapter|Run tAllarmiOUTv2", [], [] );
		} catch( excp ) {
			return false;
		}
	}
	formula.logout( session );
	java.lang.System.exit( 0 );
})();