//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
//	[Sdbx|Add Acronimo]
//	context=element
//	description=Sdbx|Add Acronimo
//	operation=load( "custom/sdbx/AddAcronimo.fs" );
//	permission=manage
//	target=dnamematch:(^gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations)|(Servizio=[^/]*/gen_folder=Servizi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations)
//	type=serverscript
//


(function () {
// Set logging
var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.sdbx');
load('custom/lib/Orgs.fs');

// Log startup
_logger.info( "Starting Add Acronimo to sdbx script")

// Create our object which will be the "callback" from the client
var callback =
{
	clicked: false,
	
    addAcronimo: function( acronimo )
    {
		var acro = null;
		
		if (this.clicked) {
			_logger.info('Add Acronimo: skipping click on OK button');
			return '';
		} else {
			
			this.clicked = true;
			acronimo = acronimo.toUpperCase();
			_logger.info('Add Acronimo: adding acronimo >' + acronimo + '<');
			// check length for ACRONIMO
			var res = null;
			acro = state.Orgs.findElement('Acronimo='+acronimo+'/' + element.DName);
			if (acro != null) {
				message = 'Add Acronimi, acronimo not added as already present'
			} else {
				acro = Orgs.createElement('Acronimo', acronimo, element.DName);
				if (acro != null) 
					message = 'Add Acronimo OK, added ' + acronimo;
				else
					message = 'Add Acronimo error, see log';
			}
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
			message = "Add Acronimo cancelled by user, nothing done"
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
    load('custom/sdbx/AddAcronimoClientScript.fs')\
"

//
// Now, send the dialog script to the user who invoked this script
//
// Note: this script is sent to variables, called "callback" and "elementName"
//       The callback is wrapped via a remote proxy using the formula.util.makeRemote()
//       function.  The element name is simply the element name of the element
//       the user initiated this ticket for.
//
session.invokeScript( 'Enter Acronimo',
                      clientTicketScript,
                      [ formula.util.makeRemote( callback ), element.name ],
                      [ 'callback', 'elementName' ] )

_logger.info( "Add Acronimo completed!" )
})();

