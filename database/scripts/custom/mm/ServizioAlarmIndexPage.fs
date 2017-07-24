/*

 Script ServizioAlarmIndexPage.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 script used in the page

 */

var ServizioAlarmIndexPageFunc = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.mm.servizio");

    // compute dname as
    // Servizio=Produzione_Phone+Banking+Tech+View/LivelloAmbiente=Produzione/Contatori=Contatori/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements
    var _getTgtDn = function (ele) {
        var dn = null, canonical = '';
		if (typeof ele.target_override != 'undefined' && ele.target_override != null && ele.target_override.length > 0) {
			dn = ele.target_override;
		} else if (ele.name == 'INFRA SYS - Tech View') {
			// special case due to key for bdi
            dn = 'Servizio=Produzione%2FSystemTest_INFRA+SYS+-+Tech+View/LivelloAmbiente=Produzione/Contatori=Contatori/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements';
        } else {
			if (typeof ele.alt_name != 'undefined' && ele.alt_name != '') {
				canonical = ele.alt_name;
			} else {
				canonical = ele.name;
			}
            dn = 'Servizio=' + formula.util.encodeURL(ele.Ambiente) + '_' + formula.util.encodeURL(canonical)+ '/LivelloAmbiente='
                + formula.util.encodeURL(ele.Ambiente) + '/Contatori=Contatori/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements'
        }
        return dn;
    }

    var _getNumeroOpen = function (ele) {
        var dn = _getTgtDn(ele)
        _logger.debug('_getNumeroOpen use dn ' + dn);
        var tgt = state.Orgs.findElement(dn);
        _logger.debug(tgt?"_getNumeroOpen: Element found":"_getNumeroOpen: Element not found");
        return tgt?tgt.NumeroOpen:0
    }

    var _getNumeroACKorASS = function (ele) {
        var tgt = state.Orgs.findElement(_getTgtDn(ele));
        return tgt?tgt.NumeroACKorASS:0
    }

    var _getSeverity = function (ele) {
        var tgt = state.Orgs.findElement(_getTgtDn(ele));
        return tgt?tgt.MinSeverity:0
    }

    var _getNumeroTicket = function (ele) {
        var tgt = state.Orgs.findElement(_getTgtDn(ele));
        return tgt?tgt.NumeroTicket:0
    }

    return {
        getNumeroOpen:_getNumeroOpen,
        getNumeroACKorASS:_getNumeroACKorASS,
        getSeverity:_getSeverity,
        getNumeroTicket:_getNumeroTicket,
		getTarget:_getTgtDn
    }

})()