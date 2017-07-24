/*

 Script RisorsaAlarmIndexPage.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 script used in the page

 */

var SIMetricheAlarmIndexPageFunc = (function () {

	// define Orgs
    //load('custom/lib/Orgs.fs')
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.mm.simetriche");
	
	// compute dname as
	// Servizio=Produzione_Phone+Banking+Tech+View/LivelloAmbiente=Produzione/Contatori=Contatori/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements
    var _getTgtDn = function (ele) {
		var res = '';
		try {
		var acro = ele.acronimo.toLowerCase();
		acro = formula.util.encodeURL(acro)
		res = 'LivelloAffectedElement' + '=' + acro + '_' + formula.util.encodeURL(ele.label.toLowerCase()) + '/' + 'gen_folder=Allarmi/gen_folder=Produzione/gen_folder=AllarmiServiziInfrastrutturali/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements'
		} catch (excp) {
			_logger.error('_getTgtDn error ' + excp);
		}
		return res;
	}

    var _getNumeroOpen = function (ele) {
		var dn = _getTgtDn(ele)
		_logger.debug('_getNumeroOpen using dn ' + dn + ' ' + new Date());
        var tgt = state.Orgs.findElement(dn);
        _logger.debug(tgt?"_getNumeroOpen: Element found":"_getNumeroOpen: Element not found");
        return tgt?tgt.NumeroOpen:0
    }

    var _getNumeroACKorASS = function (ele) {
        var tgt = state.Orgs.findElement(_getTgtDn(ele));
        return tgt?tgt.NumeroACKorASS:0
    }

    var _getMinSeverity = function (ele) {
        var tgt = state.Orgs.findElement(_getTgtDn(ele));
        return tgt?tgt.minSeverityN:0
    }

    var _getNumeroTicket = function (ele) {
        var tgt = state.Orgs.findElement(_getTgtDn(ele));
        return tgt?tgt.NumeroTicket:0
    }

    return {
        getNumeroOpen:_getNumeroOpen,
        getNumeroACKorASS:_getNumeroACKorASS,
        getMinSeverity:_getMinSeverity,
        getNumeroTicket:_getNumeroTicket
    }

})()