//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [ECM|Close Event]
// description=ECM::Close Event
// context=alarm
// target=dname:root=Organizations
// permission=manage
// type=serverscript
// operation=load( "custom/ecmop/CloseEvent.fs" );
//

(function () {

	load('custom/ecmop/EcmOpUtil.fs');
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.closeevent');
    var myAlarms = alarms;
    var currUser = state.DNamer.getUserFromDname(user.DName);
	var success = false;
	
	// Log startup
	if (myAlarms.length > 0) {
		_logger.info( "ECM::Close Event invoked by " + currUser)
	} else {
		_logger.info( "ECM::Close Event invoked by " + currUser + ' with no selected alarms(?), returning');
		return;
	}
	
    var message = '';
	var res = null;

	try {
		var ancmd = 'CloseEvent';
		res = EcmOpUtil.doEnq(ancmd, currUser, myAlarms);
		
		if (res.success) {
			success = true;
			message = 'ECM::Close Event request enqueued successfully';
		} else {
			message = 'ECM::Close Event ' + res.message;
		}
	} catch (excp) {
		failOnError = true;
		_logger.error('ECM::Close Event operation error ' + excp);
		message = 'ECM::Close Event operation error ' + excp;
	}
    _logger.info(message);
    // only if something gone wrong
	if (!success) {
		session.sendMessage( message );
	}
}());
