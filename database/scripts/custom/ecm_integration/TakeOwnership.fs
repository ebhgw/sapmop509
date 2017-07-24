// requires EcmCommander
load('custom/ecm_integration/EcmCommander.fs');

(function () {
var logger = Packages.org.apache.log4j.Logger.getLogger("Dev.EcmIntegration");
var op = 'ASSIGN', message = '', title = '';
//logger.debug('User dname = ' + user.DName);
var currUser = ecmCommander.getUserFromDname(user.DName);
var alarm = alarms[0];
logger.info('User ' + currUser + ' started ECM|Take Ownership/Assign Event operation on ' + alarm.event_handle);
var res = ecmCommander.executeCmd(alarm.CELLA_ORIGINE,alarm.event_handle,currUser,op);
logger.debug('ECM|Take Ownership/Assign Event operation ended with result ' + res);
title = op + " on " + alarm.event_handle;
if (res == 0) 
	message = 'Result Ok'
else
	message = 'error, return code ' + res + ':' + ecmCommander.getErrorLevelMsg(res);
logger.info('TakeOwnership ' + message);
logger.debug ('Dashboard, sending to user ' + currUser + ' Operation ' + title + ', ' + message); 
Packages.it.hogwart.liferay.ws.InsertAlert.insertAlert(currUser, title, message, "javascript:alert('" + title + " result: " + message + "')");
logger.debug("Sent message to Dashbaord");
})();
