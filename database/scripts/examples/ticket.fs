//
// Copyright (c) 2014 NetIQ Corporation.  All Rights Reserved.
//
// THIS WORK IS SUBJECT TO U.S. AND INTERNATIONAL COPYRIGHT LAWS AND TREATIES.  IT MAY NOT BE USED, COPIED, 
// DISTRIBUTED, DISCLOSED, ADAPTED, PERFORMED, DISPLAYED, COLLECTED, COMPILED, OR LINKED WITHOUT NETIQ'S
// PRIOR WRITTEN CONSENT. USE OR EXPLOITATION OF THIS WORK WITHOUT AUTHORIZATION COULD SUBJECT THE 
// PERPETRATOR TO CRIMINAL AND CIVIL LIABILITY.
//
// NETIQ PROVIDES THE WORK "AS IS," WITHOUT ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING WITHOUT THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT. NETIQ,
// THE AUTHORS OF THE WORK, AND THE OWNERS OF COPYRIGHT IN THE WORK ARE NOT LIABLE FOR ANY CLAIM,
// DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF,
// OR IN CONNECTION WITH THE WORK OR THE USE OR OTHER DEALINGS IN THE WORK.
//
/////////////////////////////////////////////////////////////////////////////
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [Enter Trouble Ticket]
// description=Trouble Ticket...
// context=element
// target=dname:root=Elements
// permission=manage
// type=serverscript
// operation=// @debug off \nload( "examples/ticket.fs" );
//

// Set our log category
formula.setLogCategory( "Ticket" )

// Log startup
formula.log.info( "Starting trouble ticket script" )

// Create our object which will be the "callback" from the client
// Note: this is standard javascript syntax for creating an object with named properties/values
var callback =
{
    setTicketInfo: function( reason )
    {
       // We'll log what the user did, to simulate connecting to the ticketing system.
       formula.log.info( "User created trouble ticket:" )
       formula.log.info( "   Element: " + element )
       formula.log.info( "   Reason:  " + reason )

       // Let's notify the user that we did what was asked.
       session.sendMessage( 'Trouble ticket created for ' + element + '.  Content:\n\n' + reason )
    },

    cancel: function()
    {
       formula.log.info( "User cancelled trouble ticket creation" )
       session.sendMessage( 'Trouble ticket cancelled for ' + element + '.' )
    }
}

// We're going to send this callback and a prompt script to the client, which will
// then cause execution to this script through remote proxy.

var clientTicketScript = "\
    // @opt -1 \
    // @debug off \
    var result = prompt( 'Enter trouble ticket information for ' + elementName + ':', \
                         'Trouble Ticket' ) \
    if( ! result ) \
        callback.cancel() \
    else \
        callback.setTicketInfo( result )\
"

//
// Now, send the dialog script to the user who invoked this script
//
// Note: this script is sent to variables, called "callback" and "elementName"
//       The callback is wrapped via a remote proxy using the formula.util.makeRemote()
//       function.  The element name is simply the element name of the element
//       the user initiated this ticket for.
//
formula.log.info( "Sending dialog script to client" )
session.invokeScript( 'Enter Trouble Ticket',
                      clientTicketScript,
                      [ formula.util.makeRemote( callback ), element.name ],
                      [ 'callback', 'elementName' ] )
formula.log.info( "Done!" )

