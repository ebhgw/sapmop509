// requires EcmCommander
load('custom/ecm_integration/EcmCommander.fs');

(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecm_integration');
    var myAlarms = alarms;
    var op = 'DECLINE', message = '', title = '', res, resSum = 0, alarm;
    var currUser = ecmCommander.getUserFromDname(user.DName);
    _logger.info('User ' + currUser + ' started ECM|Take Decline Ownership (multiple)');

    for (var i = 0; i< myAlarms.length; i++) {
        try {
            alarm = myAlarms[i];
            res = ecmCommander.executeCmd(alarm.CELLA_ORIGINE,alarm.event_handle,currUser,op);
            resSum = resSum + res;

            title = currUser + ' ' + op + " on " + alarm.event_handle;
            if (res == 0)
                message = 'Result Ok'
            else
                message = 'error, return code ' + res + ':' + ecmCommander.getErrorLevelMsg(res);
            _logger.info(title + ' : ' + message);
            Packages.it.hogwart.liferay.ws.InsertAlert.insertAlert(currUser, title, message, "javascript:alert('" + title + " result: " + message + "')");

        } catch (excp) {
            _logger.error('Got error ' + excp)
        }
    }


    if (resSum == 0)
        message = 'ECM|Decline Ownership took all events'
    else {
        message = 'ECM|Decline Ownership failed on some events. See log';
		session.sendMessage( message );
	}
    _logger.info(message);
    return message;
})();