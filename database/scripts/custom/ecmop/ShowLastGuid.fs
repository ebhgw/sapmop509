(function () {
	if (alarms.length > 1) {
		session.sendMessage('Operation is supported for only one alarm at a time');
		return;
	}
	var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecm_soap');    
	_logger.info("Show Last Operation start");
	load('custom/ecm_soap/EcmOpUtil.fs');
	var message = '';
	_logger.info("Show Last Operation start");
	var myAlarm = alarms[0];
	var mcid = myAlarm.mc_ueid +'';
	_logger.info('ShowLastGuid found mc_ueid ' + mcid);
	var eo = null;
	try {
		res = EcmOpUtil.quLastGuidByMcueid(mcid);
		_logger.info('Op:ShowLastGuid got ' + res.success);
		if (res.success) {
			message = res.message;
		} else {
			message = 'ShowLastGuid, guid not found';
		}
	
	} catch (excp) {
		_logger.error('Op:ShowLastGuid got error ' + excp);
		message = 'ShowLastGuid error ' + excp;
	}
	session.sendMessage( message )
	_logger.info("Show Last Operation end");
})();