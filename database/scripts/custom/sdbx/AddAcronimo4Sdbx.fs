//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [Sdbx|Add Acronimo]
// description=Sdbx|Add Acronimo
// context=element
// target=dname:root=Organizations
// permission=manage
// type=serverscript
// operation=load( "custom/operation/AddAcronimo.fs" );
//


(function () {
// Set logging
var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.sdbx');
load('custom/lib/Orgs.fs');

// Log startup
_logger.info( "Starting Add Acronimo to sdbx script")

var acronimoInBaseline = true;

var checkAcronimoInSysTestBaseline = function (acroName) {
	var res = false
	_logger.info('Checking ' + 'Acronimo=' + formula.util.encodeURL(acroName)
        + '/gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services');
    var ele = Orgs.findElement('Acronimo=' + formula.util.encodeURL(acroName)
        + '/gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services')
    if (ele) {
        res = true;
    }
	_logger.info('checkAcronimoInSysTestBaseline result ' + res);
	return res;
}

var ask2SendEmail = function (acroDn) {
	var ele = state.Orgs.findElement(acroDn);
	_logger.info('Found element : ' + ele);
	ele.perform(session, 'Sdbx|Send Add Acronimo Email', [], []);
}

// Create our object which will be the "callback" from the client
var callbackCreate =
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
			acronimoInBaseline = checkAcronimoInSysTestBaseline(acronimo);
			
			_logger.info('addAcronimo message:' + message);
			session.sendMessage( message )
			
			if (!acronimoInBaseline) {
				_logger.info('Sending email request for ' + acro.DName);
				ask2SendEmail(acro.DName);
			}
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
    load('custom/operation/AddAcronimoClientScript.fs')\
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
                      [ formula.util.makeRemote( callbackCreate ), element.name ],
                      [ 'callback', 'elementName' ] )

_logger.info( "Add Acronimo completed!" )
})();

