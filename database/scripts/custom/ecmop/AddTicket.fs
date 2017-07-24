//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [ECM|Add Ticket]
// description=ECM::Add Ticket
// context=alarm
// target=dname:root=Organizations
// permission=manage
// type=serverscript
// operation=load( "custom/ecmop/AddTicket.fs" );
//



// wrapping to avoid polluting the global env (and to force garbage collecting)
(function () {
	load('custom/ecmop/EcmOpUtil.fs');
// Set logging
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.addticket');
    var myAlarms = alarms
    var currUser = state.DNamer.getUserFromDname(user.DName);
    // Log startup
	if (myAlarms.length > 0) {
		_logger.info( "ECM::Add Ticket invoked by " + currUser)
	} else {
		_logger.info( "ECM::Add Ticket invoked by " + currUser + ' with no selected alarms(?), returning');
		return;
	}

    function isNumeric(num){
        return !isNaN(num)
    }

// Create our object which will be the "callback" from the client
    var callback =
    {
        clicked: false,
        addTicket: function( aTicket )
        {
			var message = '', res;
			var success = false;
			try {
				if (this.clicked) {
					_logger.debug('ECM|Add Ticket: double click on Ok button, skipping second click');
					return '';
					// ticket = '' in order to reset it
				} else if (isNumeric(aTicket)) {
					this.clicked = true;
					_logger.debug('ECM::Add Ticket adding/resetting ticket >' + aTicket + '< to ' + myAlarms.length + ' alarms');
					var ancmd = "AddTicket";
					res = EcmOpUtil.doEnq(ancmd, currUser, myAlarms, aTicket);

					if (res.success && aTicket.length > 0) {
						success = true;
						message = 'ECM::Add Ticket request for adding tkt ' + aTicket + ' enqueued successfully';
					} else if (res.success && aTicket.length == 0) {
						success = true;
						message = 'ECM::Add Ticket request for clearing tkt enqueued successfully';
					} else {
						message = 'ECM::Add Ticket ' + res.message;
					}

				} else {
					_logger.info('ECM::Add Ticket ticket ' + aTicket + ' is not numeric, skipping');
					message = 'ECM::Add Ticket invalid ticket number. No alarm modified';
				}
			} catch (excp) {
				_logger.error('ECM::Add Ticket, callback error ' + excp);
				message = 'ECM::Add Ticket error ' + excp;
			}
			_logger.info ( message );
            // only if something gone wrong
			if (!success) {
				session.sendMessage( message );
			}
            return message;

        },

        cancel: function()
        {
			formula.log.info('callback cancel');
            if (this.clicked) {
                _logger.info('ECM::Add Ticket: double click, Cancel button, skipping');
            } else {
                this.clicked = true;
                _logger.info( "ECM::Add Ticket operation cancelled by user" )
                session.sendMessage( 'No ticket added/cleared' )
            }
            return '';
        }
    }

// We're going to send this callback and a prompt script to the client, which will
// then cause execution to this script through remote proxy.

    var clientScript = "\
    // @opt -1 \
    // @debug off \
    load('custom/ecmop/AddTicketClientScript.fs')\
"

//
// Now, send the dialog script to the user who invoked this script
//
// Note: this script is sent to variables, called "callback" and "elementName"
//       The callback is wrapped via a remote proxy using the formula.util.makeRemote()
//       function.  The element name is simply the element name of the element
//       the user initiated this ticket for.
//

    _logger.debug( "Sending dialog script to client" )
    session.invokeScript( 'Enter Trouble Ticket',
        clientScript,
        [ formula.util.makeRemote( callback ), element.name ],
        [ 'callback', 'elementName' ] )
    _logger.debug( "Done!" )

}).call(this);