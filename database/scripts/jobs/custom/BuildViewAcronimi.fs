// Job per Production/Acronimi bscm
var logger = Packages.org.apache.log4j.Logger.getLogger("Dev");

if (server.isCoordinator())
{
	logger.info('Build view Acronimi');
	state.ViewBuilder.build('gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations');
}
else
{
	logger.info('BSCM build skipped as server is not cluster coordinator');
}

