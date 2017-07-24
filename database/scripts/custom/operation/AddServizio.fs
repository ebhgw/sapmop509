//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [Sdbx|Add Servizio]
// description=Sdbx|Add Servizio
// context=element
// target=dname:root=Organizations
// permission=manage
// type=serverscript
// operation=load( "custom/operation/AddServizio.fs" );
//


(function () {
// Set logging
var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.sdbx');
load('custom/lib/Orgs.fs');

// Log startup
_logger.info( "Starting Add Servizio to sdbx script")

// Create our object which will be the "callback" from the client
var callback =
{
	clicked: false,
	
    addServizio: function( servizio )
    {
		servizio = servizio.toUpperCase();
		if (this.clicked) {
			_logger.info('Add Servizio: skipping click on OK button');
			return '';
		} else {
			_logger.info('Add Servizio: adding Servizio >' + servizio + '<');
			this.clicked = true;
			res = 0;
			var res = Orgs.createElement('Servizio', servizio, element.DName);
			if (res != null) 
				message = 'Add Servizio OK, added ' + servizio;
			else
				message = 'Add Servizio error, see log';
			_logger.info(message);
			session.sendMessage( message )

			return message;
		}
    },

    cancel: function()
    {
		if (this.clicked) {
			_logger.info('cancel: skipping click on Cancel button');
		} else {
			this.clicked = true;
			message = "Add Servizio cancelled by user, nothing done"
			_logger.info( message )
			session.sendMessage( message )
		}
		return '';
    }
}

// We're going to send this callback and a prompt script to the client, which will
// then cause execution to this script through remote proxy.

var clientTicketScript = "\
    // @opt -1 \
    // @debug off \
    load('custom/operation/AddServizioClientScript.fs')\
"

//
// Now, send the dialog script to the user who invoked this script
//
// Note: this script is sent to variables, called "callback" and "elementName"
//       The callback is wrapped via a remote proxy using the formula.util.makeRemote()
//       function.  The element name is simply the element name of the element
//       the user initiated this ticket for.
//
session.invokeScript( 'Enter Servizio',
                      clientTicketScript,
                      [ formula.util.makeRemote( callback ), element.name ],
                      [ 'callback', 'elementName' ] )

_logger.info( "Add Servizio completed!" )
})();

