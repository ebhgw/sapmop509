(function () {

	load('custom/ecmop/EcmOpUtil.fs');
	var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.takeownership');
    var myAlarms = alarms;
    var currUser = state.DNamer.getUserFromDname(user.DName);
	var success = false;
	
	// Log startup
	if (myAlarms.length > 0) {
		_logger.info( "ECM::Take Ownership invoked by " + currUser)
	} else {
		_logger.info( "ECM::Take Ownership invoked by " + currUser + ' with no selected alarms(?), returning');
		return;
	}
    var message = '';
	var res = null;

	try {
		var ancmd = 'TakeOwnership';
		res = EcmOpUtil.doEnq(ancmd, currUser, myAlarms);
		
		if (res.success) {
			success = true;
			message = 'ECM::Take Ownership request enqueued successfully';
		} else {
			message = 'ECM::Take Ownership ' + res.message;
		}
	} catch (excp) {
		failOnError = true;
		_logger.error('ECM::Take Ownership error ' + excp);
		message = 'ECM::Take Ownership ' + excp;
	}
    _logger.info(message);
    // only if something gone wrong
	if (!success) {
		session.sendMessage( message );
	}
}());