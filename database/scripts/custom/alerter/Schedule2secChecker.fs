/*

 Script Schedule2secChecker.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 When Adpater Runtime Information is enabled, we may find an element for the schedule
 that reports the last completed schedule datetime.
 It is easy therefore to check if it is over the threshold
 At the moment 510 is on a second schedule. Test before moving

 */

var Schedule2secChecker = (function () {
	load('custom/lib/moment.js');
	
	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.alerter.checkschedule");
	//              EBI-Element=2_seconds/EBI-Element=Completed/EBI-Element=Adapter+Runtime+Information/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements
	var _elementDn='EBI-Element=2_seconds/EBI-Element=Completed/EBI-Element=Adapter+Runtime+Information/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements';
	var _prop='lastCompletedSchedule';
	var _threshold=300; // 5 min
	var _emaillist='evelino.bomitali@hogwart.it,CONTROLLO.SERVIZI.CRITICI@intesasanpaolo.com';
	
	//dn is the element where the attribute lastCompletedSchedule is found
	var lastCompletedScheduleTooOld = function () {
		var res = null;
		try {
			var ele = formula.Root.findElement(_elementDn);
			// get latest alarm
			_logger.debug('Looking for ' + _prop + ' on dn ' + _elementDn + ' value is >' + ele[_prop] + '<')
			var checkDate = moment(ele[_prop] + '');
			_logger.debug('Read date is ' + checkDate.format('YYYY-MM-DD HH:mm:ss'));
			var now = moment(new Date());
			_logger.debug('lastCompletedScheduleTooOld lastCompletedSchedule date is ' + checkDate.format('YYYY-MM-DD HH:mm:ss') + ', now ' + now.format('YYYY-MM-DD HH:mm:ss') );
			var diff = now.diff(checkDate, 'seconds');

			_logger.info('lastCompletedScheduleTooOld diff ' + diff + ', read ' + checkDate.format('YYYY-MM-DD HH:mm:ss:SSSS') + ' now ' + now.format('YYYY-MM-DD HH:mm:ss:SSSS'));
			if (diff > _threshold && diff < 6000) {
				res = {
					now: now,
					checkDate: checkDate,
					diff: diff,
					checkElementDn: _elementDn,
					threshold: _threshold
				}
			} else {
				res = null;
			}
		} catch (excp) {
			_logger.error('lastCompletedScheduleTooOld error : ' + excp);
		}
		return res;
	}
	
	// _check(testMode) -- testMode boolean
	var _docheck = function () {
	
		var testMode = false;
		if (arguments.length > 0) {
			testMode = arguments[0] + '' == 'true'?true:false;
		}
		var tooOld = null;

		_logger.debug('_docheck called with testMode ' + testMode);
		try {
			tooOld = lastCompletedScheduleTooOld();
			
			if (tooOld) {
				_logger.info('_docheck on element ' + tooOld.checkElementDn + ' diff '+ tooOld.diff + ' threshold ' + tooOld.threshold );
				if (testMode) {
					_logger.info('Check failed, ' + tooOld.diff + ' > ' + tooOld.threshold 
					+ ' exec _action on ' + tooOld.checkElementDn);
				// too old
				} else {
					_action(tooOld);
				}
			}
		} catch (excp) {
			_logger.error('_check excp ' + excp);
		}
	}
	
	var _action = function (check) {
		load('custom/lib/Mailer.fs');
		_logger.debug('Schedule2secChecker check too old, sending email');
		Mailer.send(_emaillist, "Possibile blocco adapter allarmi", 
		"A seguito del controllo effettuato il " + check.now.format('YYYY-MM-DD HH:mm:ss') +
		" � stato verificato che l'ultimo aggiornamento � avvenuto il " + check.checkDate.format('YYYY-MM-DD HH:mm:ss') +
		" rilevando un ritardo di " + check.diff + " secondi");
	}
	
	return {
		run:_docheck
	}
	
})()