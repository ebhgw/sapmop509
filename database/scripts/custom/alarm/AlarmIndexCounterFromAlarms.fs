/*
 Script AlarmIndexCounterFromAlarms.fs

 Author: Bomitali Evelino - Hogwart s.r.l.
 Tested with versions: 5.0

 Count alarms indexes using element.alarms
 open
 ack or ass
 with ticket
 condition
 Calculate open and update as side effects other indexes
 */

var AlarmIndexCounterFromAlarms = (function () {
	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.alarm");
	var _countIndexes = function (ele) {
		var calc_condition = 5;
		var open = 0;
		var tkt = 0;
		var ack_or_ass = 0;
		var alarms = ele.alarms;
		for (var i = 0; i < alarms.length; i++) {
			if (alarms[i].hasTicket == 1)
				tkt++;
			var status = alarms[i].status.toUpperCase();
			if (status === 'OPEN')
				open++;
			if (status === 'ACK' || status === 'ASSIGNED')
				ack_or_ass++
			if (alarms[i].severityN < calc_condition)
				calc_condition = alarms[i].severityN;
		}
		//ele.ack_ass_helper = ack_ass;
	   //ele.alarm_with_ticket_helper = tkt;
	   //ele.calc_condition_helper = calc_condition;
	   //ele.open = open.toFixed(0)+'';
	   ele.ack_or_ass=ack_or_ass.toFixed(0)+'';
	   ele.with_ticket=tkt.toFixed(0)+'';
	   ele.calc_condition=calc_condition.toFixed(0)+'';
	   // debug ele.updated = new Date() + '';
	   _logger.debug('Alarm index on ' + ele.DName + '\r\nopen: ' + open + ', tkt ' + tkt + ', ack_or_ass ' + ack_or_ass + '\r\ncondition ' + calc_condition);
	   return open.toFixed(0)+'';
	}
return {
        countIndexes:_countIndexes
    }
})();