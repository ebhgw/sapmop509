// requires EcmCommander
load('custom/ecm_integration/EcmCommander.fs');

(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger('Dev.EcmIntegration');
_logger.debug('Decline ownershipt with ecm_integration load');
var op = 'DECLINE', message = '', title = '';
var currUser = ecmCommander.getUserFromDname(user.DName);
var alarm = alarms[0];
_logger.info('User ' + currUser + ' started ECM|Decline Event operation on ' + alarm.event_handle);
var res = ecmCommander.executeCmd(alarm.CELLA_ORIGINE,alarm.event_handle,currUser,op);
_logger.debug('ECM|Decline Event operation ended with result ' + res);
title = op + " on " + alarm.event_handle;
if (res == 0) 
	message = 'Result Ok'
else
	message = 'error, return code ' + res + ':' + ecmCommander.getErrorLevelMsg(res);
_logger.info('DeclineOwnership ' + message);
_logger.debug ('Dashboard, sending to user ' + currUser + ' Operation ' + title + ', ' + message); 
Packages.it.hogwart.liferay.ws.InsertAlert.insertAlert(currUser, title, message, "javascript:alert('" + title + " result: " + message + "')");
_logger.debug("Sent message to Dashboard");
})();