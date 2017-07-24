// requires EcmCommander
load('custom/ecm_integration/EcmCommander.fs');

(function () {
var logger = Packages.org.apache.log4j.Logger.getLogger("Dev.EcmIntegration");
var op = 'ACTIONLIST', message = '', title = '';
var alarm = alarms[0];
var currUser = ecmCommander.getUserFromDname(user.DName);
logger.debug('User ' + currUser + ' started ECM|Action List operation on ' + alarm.event_handle);
var title = op + " on " + alarm.event_handle;
var message = 'http://10.2.230.242:4094/NocActionListsReader.aspx?sourceCell=' + alarm.CELLA_ORIGINE + '&eventHandle=' + alarm.event_handle
logger.debug('Dashboard, sending to user ' + currUser + ', title:' + title + ', message:' + message); 
Packages.it.hogwart.liferay.ws.InsertAlert.insertAlert(currUser, title, message, "javascript:window.open('" + message + "');void (0);");
logger.debug("Sent message to Dashbaord");
})();
