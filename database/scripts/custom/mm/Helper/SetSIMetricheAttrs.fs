/*
 Script SetSIMetricheAttr.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Set some attrs to default values
 */

(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.mm.helper");

    var setVisitor =
    {
            count: 0,
			elename: '',
			parentname: '',
 
            visit: function ( ele )
            {
				var clazz, elename, eleparentname;
				clazz = ele.elementClassName + '';
				if (clazz == 'SIMetriche') {
					elename = ele.name
					parentname = ele.parent.name;
					
					if (parentname == 'Active Directory') {
						ele.acronimo = 'INFRA-AD';
					}
					if (parentname == 'DHCP') {
						ele.acronimo = 'INFRA-DHCP';
					}
					if (parentname == 'DNS') {
						ele.acronimo = 'INFRA-DNS';
					}
					ele.label = ele.name;
					ele.servizio = parentname;
					ele.canonical_name = ele.name;
					setVisitor.count++;
				}
				if (clazz == 'SIServizio') {
					elename = ele.name
					if (elename == 'Active Directory') {
						canonical_name = 'INFRA-AD';
					}
					if (elename == 'DNS') {
						canonical_name = 'INFRA-DNS';
					}
					if (elename == 'DHCP') {
						canonical_name = 'INFRA-DHCP';
					}
        	}
    }

	element.walk(setVisitor);
	_logger.info("Set " + setVisitor.count + " element");
	
})();