//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//

(function () {
// Set logging
var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.utentidata');
load('custom/lib/DNamer.fs');

// Log startup
_logger.info( "Starting collecting users data" );


var listUtenti = function () {
	var users_element = formula.Root.findElement('users=Users/security=Security/root=Administration');
	var user_list = users_element.children;
	_logger.debug('Found ' + user_list.length + ' children');
	
	var u, l = user_list.length, res = '"User";"Fullname";"Group"\n';
	for (var i = 0; i < l; i++) {
		u = user_list[i];
		_logger.debug('processing ' + u.DName);
		var username = DNamer.getUserFromDname(u);
		var fullname = u.fullname;
		var groups = u.groups;
		res = res + '"' + username + '";"' + fullname + '";"' + groups + '"\n'; 
		_logger.debug('Found ' + res); 
		
		};
		return res; 
}

// Create our object which will be the "callback" from the client
var callback =
{
    getUtentiData: function()
    {
		return listUtenti()+'';
    }
}

// We're going to send this callback and a prompt script to the client, which will
// then cause execution to this script through remote proxy.

var clientScript = "\
    // @opt -1 \
    // @debug off \
    load('custom/operation/collectDataClient.fs')\
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
session.invokeScript( 'Choose file',
                      clientScript,
                      [ formula.util.makeRemote( callback ), element.name, 'utenti' ],
                      [ 'callback', 'elementName', 'data2collect' ] )
_logger.info( "Done!" )

}).call(this);