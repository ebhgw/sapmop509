(function () {

	load('custom/ecmop/EcmOpUtil.fs');
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.openticket');
    var myAlarms = alarms;
    var currUser = state.DNamer.getUserFromDname(user.DName);
	var success = false;
	
	// Log startup
	if (myAlarms.length > 0) {
		_logger.info( "ECM::Open Ticket invoked by " + currUser)
	} else {
		_logger.info( "ECM::Open Ticket invoked by " + currUser + ' with no selected alarms(?), returning');
		return;
	}
    var message = '';
	var res = null;

	try {
		var ancmd = 'OpenTicket';
		res = EcmOpUtil.doEnq(ancmd, currUser, myAlarms);
		
		if (res.success) {
			success = true;
			message = 'ECM::Open Ticket request enqueued successfully';
		} else {
			message = 'ECM::Open Ticket ' + res.message;
		}
	} catch (excp) {
		failOnError = true;
		_logger.error('ECM::Open Ticket operation error ' + excp);
		message = 'ECM::Open Ticket operation error ' + excp;
	}

    _logger.info(message);
	 // only if something gone wrong
	if (!success) {
		session.sendMessage( message );
	}
}());