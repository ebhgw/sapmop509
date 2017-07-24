//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [ECM|Add Ticket]
// description=ECM|Add Ticket
// context=alarm
// target=dname:root=Organizations
// permission=manage
// type=serverscript
// operation=load( "custom/ecm_integration/AddTicket.fs" );
//

// wrapping to avoid polluting the global env (and to force garbage collecting)
(function () {
	load('custom/ecmop/EcmOpUtil.fs');
// Set logging
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.addnote');
    var myAlarms = alarms
    var currUser = state.DNamer.getUserFromDname(user.DName);
	// Log startup
	if (myAlarms.length > 0) {
		_logger.info( "ECM::Add Note invoked by " + currUser)
	} else {
		_logger.info( "ECM::Add Note invoked by " + currUser + ' with no selected alarms(?), returning');
		return;
	}

// Create our object which will be the "callback" from the client
    var callback =
    {
        clicked:false,

        addNote:function (aNote) 
		{
			var message = '', res;
			var success = false;
			try {
				if (this.clicked) {
					message = 'ECM::Add Note: skipping double OK click';
					_logger.debug(message);
				} else if (aNote.length >0 ){
					this.clicked = true;
					_logger.debug('ECM::Add note:' + aNote + '< from callback');
					
						_logger.debug('Calling EcmOpUtil.doEnq for ' + 'AddNote');
						res = EcmOpUtil.doEnq("AddNote", currUser, myAlarms, aNote);

						if (res.success) {
							success = true;
							message = 'ECM::Add Note adding ' + aNote.substring(0, 12) + '... enqueued successfully';
						} else {
							message = 'ECM::Add Note ' + res.message;
						}
				} else {
					message = 'ECM::Add Note with empty note, skipping';
				}
			} catch (excp) {
				_logger.error('ECM::Add Note operation error ' + excp);
				message = 'ECM::Add Note operation error ' + excp;
            }
			_logger.info(message);
            // only if something gone wrong
			if (!success) {
				session.sendMessage( message );
			}
            return message;
        },

        cancel:function () {
            if (this.clicked) {
                _logger.info('Test|ECM|Add Note: double click, Cancel button, skipping');
            } else {
                this.clicked = true;
                _logger.info("Test|ECM|Add Note operation cancelled by user");
                session.sendMessage('No note added');
            }
            return '';
        }
    };

// We're going to send this callback and a prompt script to the client, which will
// then cause execution to this script through remote proxy.

    var clientScript = "\
    // @opt -1 \
    // @debug off \
    load('custom/ecmop/AddNoteClientScript.fs')\
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
    session.invokeScript( 'Add Note',
        clientScript,
        [ formula.util.makeRemote( callback ), element.name ],
        [ 'callback', 'elementName' ] )
    _logger.debug( "Done!" )

}).call(this);