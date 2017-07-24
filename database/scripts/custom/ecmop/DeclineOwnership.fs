(function () {

	load('custom/ecmop/EcmOpUtil.fs');
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.declineownership');
    var myAlarms = alarms;
    var currUser = state.DNamer.getUserFromDname(user.DName);
	var success = false;
	
	// Log startup
	if (myAlarms.length > 0) {
		_logger.info( "ECM::Decline Ownership invoked by " + currUser)
	} else {
		_logger.info( "ECM::Decline Ownership invoked by " + currUser + ' with no selected alarms(?), returning');
		return;
	}
    var message = '';
	var res = null;

	try {
		var ancmd = 'DeclineOwnership';
		res = EcmOpUtil.doEnq(ancmd, currUser, myAlarms);
		
		if (res.success) {
			success = true;
			message = 'ECM::Decline Ownership enqueued successfully';
		} else {
			message = 'ECM::Decline Ownership ' + res.message;
		}
	} catch (excp) {
		failOnError = true;
		_logger.error('ECM::Decline Ownership operation error ' + excp);
		message = 'ECM::Decline Ownership operation error ' + excp;
	}

    _logger.info(message);
	// only if something gone wrong
	if (!success) {
		session.sendMessage( message );
	}
}());