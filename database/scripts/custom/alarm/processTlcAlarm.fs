/*

	Hogwart srl
	Copyright (C) 2013

	Check the message field to define if a tlc device is active or standby
	
*/

(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.alarm");
    
    var myAlarm = alarm;
    var message = myAlarm.Message
    var pattStandby = /stato standby$/i
    var pattActive = /stato active$/i
    var res = false

    _logger.debug('Processing alarm ' + alarm.ID);
    // assume a default status 'active'
    try {
        res = pattStandby.test(message)
        _logger.debug('Field message >' + message + '<')
        _logger.debug('Pattern matched ' + res);
        if (res) {
            alarm.tlcStatus = 'standby'
        } else {
            res = pattActive.test(message)
            if (res) {
                alarm.tlcStatus = 'active'
            }
        }
    } catch (excp) {
        formula.log.error('processTlcAlarm error ' + excp);
        _logger.error('processTlcAlarm error ' + excp);
    }
    
    delete myAlarm;
    if (!res) {
       return true;
    } else {
       return false;
    }
})();