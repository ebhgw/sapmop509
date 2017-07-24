(function () {
	if (alarms.length > 1) {
		session.sendMessage('Operation is supported for only one alarm at a time');
		return;
	}
	load('custom/ecm_soap/EcmOpUtil.fs');
	
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecm_soap.showlastop');
	_logger.debug("ShowLastOp starting");
	var myAlarm = alarms[0];
	var mcid = myAlarm.mc_ueid +'';
	var message = '';
	var res = null;
	var eo = null;
	_logger.debug('ShowLastOp found mc_ueid ' + mcid);
	try {
		res = EcmOpUtil.doQuLastopByMcueid(mcid);
		_logger.debug('Returning from doQuLastopByMcueid');
		if (res == null) {
			_logger.debug('ShowLastOp no guid found for mcid');
		} else {
			_logger.debug('ShowLastOp res.success ' + res.success + ' res.guid ' + res.guid);
		}
		//_logger.debug('ShowLastOp got eo (toString) ' + res.ecmop===null?"null":res.ecmop.toString());
		if (res != null && res.success && res.ecmop !== null) {
			message = 'ECM::Show last operation\n' + res.ecmop.opexecSummary();
		} else if (res != null && res.success && res.ecmop === null ) {
			message = 'ShowLastOp, no operation on history';
		} else if (res != null) {
			// error while querying, message from captured exception
			message = 'ECM::Show last operation ' + res.message;
		} else {
			// error while querying, message from captured exception
			message = 'ECM::Show last operation no guid found';
		}
	
	} catch (excp) {
		_logger.error('Op:Show Last Operation got error ' + excp);
		message = 'ECM::Show last operation error ' + excp;
	}
	_logger.info('ShowLastop ' + message);
	session.sendMessage(message)
	_logger.info("Show Last Operation end");
}());