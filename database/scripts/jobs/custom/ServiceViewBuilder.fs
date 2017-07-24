// Job per Service2Acro bscm
//var logger = Packages.org.apache.log4j.Logger.getLogger("Dev");

function buildbscm(viewDName)
{
	var viewElement = null;
	try {
		viewElement = formula.Root.findElement( viewDName );
	} catch( Exception ) {
		formula.log.error( "ERROR - caught exception while finding view " +
		viewDName + ": " + Exception );
		return false;
	}
	if( viewElement == null ) {
		formula.log.error("ERROR - view " + viewDName + " not found" );
		return false;
	}
	try {
		var ret = viewElement.perform( session, "ViewBuilder|Run", [], [] );
	} catch( Exception ) {
		return false;
	}
}

if (server.isCoordinator())
{
	formula.log.info('Build bscm: Service2Acro start');
	buildbscm('gen_folder=Service2Acro/ISP_FolderBase=Production/root=Organizations');
	formula.log.info('Build bscm: Service2Acro end');
}
else
{
	formula.log.debug('BSCM build skipped as server is not cluster coordinator');
}
