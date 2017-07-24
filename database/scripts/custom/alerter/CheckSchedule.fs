/*

 Script SendCheckOnAlarmEmail.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Check Bdi Schedule and then send email

 */

(function () {
    load('custom/lib/Mailer.fs');
    load('custom/lib/Orgs.fs');
    load('custom/lib/underscore.js')
	load('custom/lib/moment.js');
	
	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.alerter.checkalarmlast");

	var dateTooOld = function (dn, datetimeAttr, threshold) {
		var ele = Orgs.findElement(dn);
		// get latest alarm
		
		var checkDate = moment(ele[datetimeAttr]);
		var now = moment(new Date());
		_logger.debug('Checking at ' + now.format('DD-MM-YYYY HH:mm:ss'));
		_logger.debug('Last schedule was ' + checkDate.format('DD-MM-YYYY HH:mm:ss'));
		var diff = now.diff(checkDate, 'seconds');
		if (now.diff(checkDate, 'seconds') > threshold) {
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
	
	// then check last alarm date to check if something got wrong
	
	dn = 'AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements';
	checkRes = dateTooOld(dn, 'LastExportDt', 1200);
	if (checkRes) {
		// schedule maybe stopped
		_logger.info('Sending email stop detected. Difference is ' + checkRes.diff + ' seconds, threshold is ' + checkRes.threshold);
		msg = new Packages.java.lang.StringBuffer();
		msg.append("Report generated " +  checkRes.now.format('DD-MM-YYYY HH:mm:ss') + ' on ' + 	Packages.java.net.InetAddress.getLocalHost().getHostName() + '||'); 
		msg.append('Alarm date too old detected. Last date was ' + checkRes.checkDate.format('DD-MM-YYYY HH:mm:ss'));
		Mailer.send('evelino.bomitali@hogwart.it,david.cacioli@hogwart.it,marcello.melzi@hogwart.it', 'Alarm date too old', msg);
	} else {
		_logger.debug('Last update within threshold');
	}
	
})()