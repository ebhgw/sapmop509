// requires EcmCommander
load('custom/ecm_integration/EcmCommander.fs');

(function () {
var logger = Packages.org.apache.log4j.Logger.getLogger("Dev.EcmIntegration");
logger.info('Starting ECM|Open Ticket Remedy Event operation');
var op = 'TICKET', message = '', title = '';
var currUser = ecmCommander.getUserFromDname(user.DName);
var alarm = alarms[0];
logger.info('User ' + currUser + ' started ECM|Open Ticket Remedy Event operation on ' + alarm.event_handle);
var res = ecmCommander.executeCmd(alarm.CELLA_ORIGINE,alarm.event_handle,currUser,op);
logger.debug('ECM|Open Ticket Remedy Event operation ended with result ' + res);
title = op + " on " + alarm.event_handle;
if (res == 0) 
	message = 'Result Ok'
else
	message = 'error, return code ' + res + ':' + ecmCommander.getErrorLevelMsg(res);
logger.info('OpenTicketRemedy ' + message);
logger.debug ('Dashboard, sending to user ' + currUser + ' Operation ' + title + ', ' + message); 
Packages.it.hogwart.liferay.ws.InsertAlert.insertAlert(currUser, title, message, "javascript:alert('" + title + " result: " + message + "')");
logger.debug("Sent message to Dashbaord");
})();
