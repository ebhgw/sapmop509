/*

 Script HashTicket.fs

 Author: Bomitali Evelino - Hogwart s.r.l.
 Tested with versions: 5.0

 Allow to count ticket associated to an element

 */

load('custom/lib/Hashtable.fs');

var logger = Packages.org.apache.log4j.Logger.getLogger("Dev.TicketCounter");

function TicketCounter () {
    this.tickets = new Hashtable();
}

TicketCounter.prototype.clear = function () {
    this.tickets.clear();
}

TicketCounter.prototype.put = function (key) {
    this.tickets.put(key,1);
}

TicketCounter.prototype.size = function () {
    this.tickets.size();
}

// currEle is the element for which we count alarms with a distinct ticket number
TicketCounter.prototype.countTicket = function (currEle) {
    logger.debug('Counting ticket on ' + currEle);
    var res = 0, tn = '', myAlarms, msg = '', step;

	try {
		step =1;
		myAlarms = currEle.alarms;
	    for (i=0; i<myAlarms.length;i++) {
			step = 2;
			tn = myAlarms[i]['Ticket_Num'] +'';
			if (tn != '' && ! this.tickets.containsKey(tn)) {
				step = 3;
				this.tickets.put(tn,1);
			}
		}
	} catch (excp) {
		msg = 'On element ' + currEle.dname + ' on step ' + step + ' error : ' + excp;
		formula.log.error(msg);
		logger.error(msg);
	}
    
    delete myAlarms;
    return this.tickets.size();
}

// currEle is the element for which we count alarms with a distinct ticket number
TicketCounter.prototype.countTicketDebug = function (currEle) {
    logger.debug('Counting ticket on ' + currEle);
    var res = 0, tn = '', myAlarms, step = 1, msg = '', tmp;

	try {
		myAlarms = currEle.alarms;
		step=2;
	    for (i=0; i<myAlarms.length;i++) {
			step=3;
			logger.info('# of alarms ' + myAlarms.length);
			step=6;
			logger.info('Ticket_Num >' + myAlarms[i]['Ticket_Num'] +'<');
			step = 8;
			if (typeof myAlarms[i]['Ticket_Num'] != 'undefined' && myAlarms[i]['Ticket_Num']) {
				step=100;
				tn = myAlarms[i]['Ticket_Num'] + '';
				step=105;
				logger.debug('ticket on alarms[' + i + '] is string >' + tn + '<');
				if (tn != '' && ! this.tickets.containsKey(tn)) {
					step=106;
					logger.debug('Putting >' + tn + '< in hash');
					this.tickets.put(tn,1);
				}
				step=107;
			}
		}
	} catch (excp) {
		msg = 'On element ' + currEle.dname + ' on step ' + step + ' error : ' + excp;
		formula.log.error(msg);
		logger.error(msg);
	}
    
    delete myAlarms;
    return this.tickets.size();
}