//	CodaHost.fs
//
//	Created By: E. Bomitali
//	Date: 2014 Feb 12
//
//	VERSION INFORMATION
//		Date: 2014 Feb 13
//		Version: 1.0
//		Notes: Original Implementation
//
//
//	Purpose
//  Utils for CodaHost class elemest
//  Build match on [Pattern-LDAP]:(&(objectClass=Risorsa)(cn=*DATAGRAM.ZXI.UR000003))/gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations
//  where nomeRisorsaCoda is a property of target object
//
//	Notes
//		none
//
//	Required Changes
//		none
//
//	Optional Changes
//

var CodaOpen = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
    load('custom/lib/MatchMngr.fs');
    load('custom/lib/underscore.js')

	
    var _setMatchUnderCode = function(ele) {
        var clazz = ele.elementClassName + "";
        var eleCurrentMatch = ele.Matches;
            var eleCalcMatch = '[Pattern-LDAP]:(&(objectClass=Risorsa)(cn=*' + ele.name + '))/gen_folder=Code/ISP_FolderBase=Production/root=Organizations';
            _logger.info('Processing ' + ele.DName);
            _logger.info('Current Match    :' + eleCurrentMatch);
            _logger.info('Calculated Match :' + eleCalcMatch);
            MatchMngr.set(ele, eleCalcMatch);
    }


    // For smarts_instance=Rxx elements
    var setMatchesForChildren = function() {
        var codeFolder = formula.Root.findElement('gen_folder=Code/ISP_FolderBase=Production/root=Organizations, loop on children');
        var codeRisorsaList = codeFolder.children;
        _.each(codeRisorsaList, function (ele) { setMatchForCodaRisorsa(ele) });
    }
	
	return {
		setMatchUnderCode:_setMatchUnderCode
	}
})();
