var buildkoper = function () {

	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.be_koper");
	
	var dn1 = 'ClasseCompInfra=BACKUP+INFR./Acronimo=BE_KOPER/PortletView=Banca+Koper+-+Tech+View/gen_folder=Grafici/gen_folder=Banca+Koper/gen_folder=Produzione/Community=Banche+Estere/gen_folder=Views/Dashboard=Dashboard/root=Organizations';
	var dn2 = 'ClasseCompInfra=DATABASE/Acronimo=BE_KOPER/PortletView=Banca+Koper+-+Tech+View/gen_folder=Grafici/gen_folder=Banca+Koper/gen_folder=Produzione/Community=Banche+Estere/gen_folder=Views/Dashboard=Dashboard/root=Organizations';
	var dn3 = 'ClasseCompInfra=HOST+OVM/Acronimo=BE_KOPER/PortletView=Banca+Koper+-+Tech+View/gen_folder=Grafici/gen_folder=Banca+Koper/gen_folder=Produzione/Community=Banche+Estere/gen_folder=Views/Dashboard=Dashboard/root=Organizations';
	var dn4 = 'ClasseCompInfra=HOST+VMWARE/Acronimo=BE_KOPER/PortletView=Banca+Koper+-+Tech+View/gen_folder=Grafici/gen_folder=Banca+Koper/gen_folder=Produzione/Community=Banche+Estere/gen_folder=Views/Dashboard=Dashboard/root=Organizations';

	var viewElement;
	
	try {
		viewElement = formula.Root.findElement( dn1 );
		viewElement.perform( session, "ViewBuilder|Run", [], [] );

		viewElement = formula.Root.findElement( dn2 );
		viewElement.perform( session, "ViewBuilder|Run", [], [] );

		viewElement = formula.Root.findElement( dn3 );
		viewElement.perform( session, "ViewBuilder|Run", [], [] );

		viewElement = formula.Root.findElement( dn4 );
		viewElement.perform( session, "ViewBuilder|Run", [], [] );

	} catch( excp ) {
		formula.log.error("ViewBuilder.build error on " + viewDName + ": " + excp );
	}

};