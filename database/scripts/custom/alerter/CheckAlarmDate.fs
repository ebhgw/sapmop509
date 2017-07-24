/*

 Script SendCheckOnAlarmEmail.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Check Bdi Schedule and then send email

 */

var checkAlarmDate = (function () {
    load('custom/lib/Mailer.fs');
    load('custom/lib/Orgs.fs');
    load('custom/lib/underscore.js')
	load('custom/lib/moment.js');
	
	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.checkschedule");

	var dateTooOld = function (dn, datetimeAttr, threshold) {
		var ele = Orgs.findElement(dn);
		// get latest alarm
		
		var checkDate = moment(ele[datetimeAttr]);
		var now = moment(new Date());
		_logger.info('Checking at ' + now.format('DD-MM-YYYY HH:mm:ss'));
		_logger.info('Last schedule was ' + checkDate.format('DD-MM-YYYY HH:mm:ss'));
		var diff = now.diff(checkDate, 'seconds');
		_logger.info('Diff is  ' + diff);
		if (diff > threshold) {
			return {
				now: now,
				checkDate: checkDate,
				diff: diff,
				checkElementDn: dn,
				threshold: threshold
			}
		} else {
			return null;
		}
	}
	
	var _checkLastScheduleCompleted = function () {
		try {
		// Completed, assume only one schedule that is RunOnce
		var completedDn = 'EBI-Element=Completed/EBI-Element=Adapter+Runtime+Information/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements';		
		var checkCompleted = dateTooOld(completedDn, 'LastCompletedDt', 600);
		_logger.info("checkCompleted is " + checkCompleted);
		if (checkCompleted) {
			// schedule maybe stopped
			_logger.info('Sending email because now - LastCompletedDt is over threshold. Difference is ' + checkCompleted.diff + ' seconds, threshold is ' + checkCompleted.threshold);
			msg = new Packages.java.lang.StringBuffer();
			msg.append("Report generated " +  checkCompleted.now.format('DD-MM-YYYY HH:mm:ss') + ' on ' + 	Packages.java.net.InetAddress.getLocalHost().getHostName() + '||'); 
			msg.append('Bdi schedule stop detected. Last run was ' + checkCompleted.checkDate.format('DD-MM-YYYY HH:mm:ss')  + '|');
			msg.append('Possible stop of bdi schedule');
			Mailer.send('evelino.bomitali@hogwart.it,david.cacioli@hogwart.it,marcello.melzi@hogwart.it,alessandro.trebbi@hogwart.it', 'Last schedule too old', msg);
		}
		} catch (excp) {
			_logger.error('_checkLastScheduleCompleted excp ' + excp);
		}
	}

	var _checkLastExport = function () {
	try {
		var allarmiSuElementiDn = 'AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements';
		checkRes = dateTooOld(allarmiSuElementiDn, 'LastExportDt', 1800);
		if (checkRes) {
			// schedule maybe stopped
			_logger.info('Sending email because of now - LastExportDt is over threshold. Difference is ' + checkRes.diff + ' seconds, threshold is ' + checkRes.threshold);
			msg = new Packages.java.lang.StringBuffer();
			msg.append("Test: Report generated " +  checkRes.now.format('DD-MM-YYYY HH:mm:ss') + ' on ' + 	Packages.java.net.InetAddress.getLocalHost().getHostName() + '||'); 
			msg.append('Alarm ExportDt too old detected. Last was ' + checkRes.checkDate.format('DD-MM-YYYY HH:mm:ss') + '|');
			msg.append('Possible stop of ecm alarm feed');
			Mailer.send('evelino.bomitali@hogwart.it,david.cacioli@hogwart.it,marcello.melzi@hogwart.it,alessandro.trebbi@hogwart.it', 'Last ExportDt date (alarm update) too old', msg);
		} else {
			_logger.info('Last update within threshold');
		}
				} catch (excp) {
			_logger.error('_checkLastScheduleCompleted excp ' + excp);
		}
	}
	
	return {
		checkLastScheduleCompleted:_checkLastScheduleCompleted,
		checkLastExport:_checkLastExport
	}
	
})()