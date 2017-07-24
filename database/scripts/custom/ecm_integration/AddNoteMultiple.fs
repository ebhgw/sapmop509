//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [ECM|Add Ticket]
// description=ECM|Add Ticket Multiple
// context=alarm
// target=dname:root=Organizations
// permission=manage
// type=serverscript
// operation=load( "custom/ecm_integration/AddTicket.fs" );
//

load('custom/ecm_integration/EcmCommander.fs');
load('custom/lib/underscore.js');

// wrapping to avoid polluting the global env (and to force garbage collecting)
(function () {
// Set logging
var _logger = Packages.org.apache.log4j.Logger.getLogger('fs');

// Log startup
_logger.info( "Starting Add Note to multiple alarms" )
var op = 'ADD2TICKET'
var myAlarms = alarms
var currUser = ecmCommander.getUserFromDname(user.DName);
var title = '', message = '', myAlarm;

// Create our object which will be the "callback" from the client
var callback =
{
	clicked: false,
	
    addNote: function( aNote )
    {
		if (this.clicked) {
			_logger.debug('addNote: skipping double OK click');
			return '';
		} else {
			this.clicked = true;
			var res = 0, resAll = 0, message = '';
			_logger.debug('ECM|addNote multiple: adding ' + aNote);
			try {
				for (var i = 0; i< myAlarms.length; i++) {
						res = 0;
						myAlarm = myAlarms[i];
						res = ecmCommander.executeAddNote(myAlarm.CELLA_ORIGINE,myAlarm.mc_ueid,currUser,aNote);
						title = "Add Note" + " on " + myAlarm.mc_ueid;
						if (res == 0) 
							message = 'ECM|AddNote multiple on mc_ueid ' +  myAlarm.mc_ueid + ':OK';
						else
							message = 'ECM|AddNote multiple on mc_ueid ' +  myAlarm.mc_ueid + ' return code ' + res;

						_logger.info(message);
						resAll = resAll + res;
					};
			} catch (excp) {
				_logger.error('Got error ' + excp)
			}
			if (resAll == 0)
				message = 'ECM|Add note multiple success on all alarm(s)';
			else {
				message = 'ECM|Add note multiple failed on some alarm(s), ';
				session.sendMessage( message );
			}
			_logger.info(message);

			return message;
		}
    },

    cancel: function()
    {
		if (this.clicked) {
			_logger.info('ECM|Add Ticket: double click, Cancel button, skipping');
		} else {
			this.clicked = true;
			_logger.info( "ECM|Add Ticket operation cancelled by user" )
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
    load('custom/ecm_integration/AddNoteClientScript.fs')\
"

//
// Now, send the dialog script to the user who invoked this script
//
// Note: this script is sent to variables, called "callback" and "elementName"
//       The callback is wrapped via a remote proxy using the formula.util.makeRemote()
//       function.  The element name is simply the element name of the element
//       the user initiated this ticket for.
//

_logger.info( "Sending dialog script to client" )
session.invokeScript( 'Enter Trouble Ticket',
                      clientScript,
                      [ formula.util.makeRemote( callback ), element.name ],
                      [ 'callback', 'elementName' ] )
_logger.info( "Done!" )

}).call(this);