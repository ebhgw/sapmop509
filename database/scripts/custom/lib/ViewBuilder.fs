/*  Run BSCM Job
    
	custom/lib/ViewBuilder.fs
	Version 1.0
	E. Bomitali

	Updates:
	Ver    	Author          Description
	1.0    	E. Bomitali    Initial Release
	
	Tested with NOC 5.0
	
	Run a bscm job on viewDName within some try...catch

*/

var ViewBuilder = (function() {

	var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.viewbuilder');

	var _build = function(viewDName) {
		var viewElement = null;
		try {
			viewElement = formula.Root.findElement( viewDName );
			viewElement.perform( session, "ViewBuilder|Run", [], [] );
		} catch( excp ) {
			_logger.error("ViewBuilder.build error on " + viewDName + ": " + excp );
		}
	}
	
	return {
		build:_build
	}
})();