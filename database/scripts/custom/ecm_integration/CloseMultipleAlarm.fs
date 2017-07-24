// requires EcmCommander
load('custom/ecm_integration/EcmCommander.fs');

(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.ecm_integration");
    var myAlarms = alarms;
    var currUser = ecmCommander.getUserFromDname(user.DName);
    var op = 'CLOSE', message = '', title = '', res, resSum = 0, alarm;

    _logger.info('User ' + currUser + ' started ECM|Close Event (multiple)');

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
        message = 'ECM|Close Event closed all events'
    else {
        message = 'ECM|Close Event some close failed. See log';
		session.sendMessage( message );
	}
    _logger.info(message);
    return message;
})();
