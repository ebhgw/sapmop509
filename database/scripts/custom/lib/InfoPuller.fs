/*

 Script DNamer.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Utilities to handle dnames

 */
 
 var InfoPuller = (function () {
 
	var ALARMSCOUNTDATACORRELATE = 'Adapter: AlarmsCountData';
	
	var _getCorrelatedValue(ele, name, prop) {
		var res = {
			found:false,
			value:0
		}
		try {
			res.value = ele["Adapter: AlarmsCountData.OpenTicketNum"];
			_logger.debug('Found correlated');
		} catch {
			_logger.debug('Correlated not found')
		}
		return res;
	}
	
	var _getThroughDname () {
	
	}
	
	var getAlarmsCountData (ele, prop) {
		var res, dn = '', source = '';
		var clazz = ele.elementClassname + ''
		_logger.debug('Getting ' + prop + ' for ' + ele.DName);
		
		var res = _getCorrelatedValue(ele, ALARMSCOUNTDATACORRELATE, prop) ;
		if (!res.found)
		try {
			if(element[ALARMSCOUNTDATACORRELATE]){
				_logger.debug('Found correlated')
				res = ele["Adapter: AlarmsCountData.OpenTicketNum"]
			} else if (clazz=='Acronimo'||clazz=='AcronimoOpVw'){
				_logger.debug('Pulling from dname with class ' + clazz);
				// example AcronimoData=Produzione_CISC0/Acronimi=Acronimi/gen_folder=Produzione/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements
				dn='AcronimoData=' +formula.util.encodeURL(element.Ambiente)+ '_' + formula.util.encodeURL(ele.name) +'/Acronimi=Acronimi/gen_folder='+formula.util.encodeURL(element.Ambiente)+'/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
				_logger.debug('finding ' + dn);
				source = state.Orgs.findElement(dn);
			} else if (clazz=='Servizio'){
				dn='ServizioData=' + formula.util.encodeURL(element.Ambiente) + '_'+formula.util.encodeURL(element.name)+'/Servizi=Servizi/gen_folder='+formula.util.encodeURL(element.Ambiente)+'/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';

		var el=formula.Root.findElement(dn);
		el.OpenTicketNum
		} catch (excp) {
			_logger.error('While accessing ' + ele.DName + ' got ' + excp);
		}
		return res;
}

} )();