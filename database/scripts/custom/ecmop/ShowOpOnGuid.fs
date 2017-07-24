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

// wrapping to avoid polluting the global env (and to force garbage collecting)
(function () {
	load('custom/ecm_soap/EcmOpUtil.fs');
// Set logging
    var _logger = Packages.org.apache.log4j.Logger.getLogger('hog.ecm.op');
    var endpt = 'http://10.2.230.242:2007/GetNocOperationsResults.ashx';
    var javaheo = Packages.hog.ecm.op;
    // Log startup
    _logger.debug( "Starting Add Note" )
    var myAlarms = alarms
    var currUser = state.DNamer.getUserFromDname(user.DName);

// Create our object which will be the "callback" from the client
    var callback =
    {
        clicked:false,

        askGuid:function (guid) {
			var eo = null;
            if (this.clicked) {
                _logger.debug('ECM::Add Note: skipping double OK click');
                return '';
            } else {
                this.clicked = true;
                var message = '';
                _logger.debug('Show Op on Guid ' + guid);

                try {
					_logger.debug('Calling EcmOpUtil.doQuByGuid for');
					eo = EcmOpUtil.doQuByGuid(guid, endpt);
                } catch (excp) {
                    _logger.error('Got error ' + excp);
                }
                session.sendMessage(eo.toString());
                return eo;
            }
        },

        cancel:function () {
            if (this.clicked) {
                _logger.info('Show Op on Guid, double click, skipping');
            } else {
                this.clicked = true;
                _logger.info('Show Op on Guid cancelled by user');
                session.sendMessage('Cancelled by user');
            }
            return '';
        }
    };

// We're going to send this callback and a prompt script to the client, which will
// then cause execution to this script through remote proxy.

    var clientScript = "\
    // @opt -1 \
    // @debug off \
    load('custom/ecm_soap/ShowOpOnGuidClientScript.fs')\
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
    session.invokeScript( 'Enter Guid',
        clientScript,
        [ formula.util.makeRemote( callback ), element.name ],
        [ 'callback', 'elementName' ] )
    _logger.info( "Done!" )

}).call(this);