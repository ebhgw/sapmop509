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


// Load the login routine.
load( 'util/login' )

// Login.
login( 'localhost', '80', 'admin', 'formula' )

// Load the organization creation code.
load('util/orgs')

// Delete our elements from previous run
writeln( 'Removing previous elements' )
deleteElement( 'org=Complex+Test/root=Organizations' )
deleteElement( 'org=Simple+Test/root=Organizations' )

// Create an element with a random name, sample children, just contact info.
writeln( 'Creating simple element' )
createElement2( 'Simple Test',                                  // name
                'root=Organizations',                           // root
                'root=Administration;root=Elements',            // source element list
                'org',                                          // class name (must be 'org' for root organizations
                'Joe Bob' )                                     // contact

// Create an organization, with matching of all defined adapters, full contact information, and other stuff
writeln( 'Creating complex element' )
createElement3( 'Complex Test',                                 // name
                'root=Organizations',                           // root
                'adapters=Adapters/root=Administration',                                             // source element list
                'org',                                          // class name (must be 'org' for root organizations
                'Joe Bob',                                      // contact
                'The Company Joe Runs',
                '1234 ShutTheDoor Blvd.',
                '(900) 555 1234',
                '(900) 555 4321',
                '(900) 555 5678',
                'joe@joebob.com',
                'http://www.com',
                true,
                '',
                '.*adap.*/adapters=Adapters/root=Administration' )

// Modify the last org we created; let's change the (wrong) address and email address.
writeln( 'Wait for 10 seconds, then Modifying complex element' )
java.lang.Thread.sleep (10000) 

var lastElementInfo = getElement( 'org=Complex+Test/root=Organizations' )
lastElementInfo.props.Address = '4321 OpenTheDoor Road'
lastElementInfo.props.Email = 'joe@bob.com'
modifyElement( lastElementInfo )

// Now, remove that simple element we did.
writeln( 'Removing simple element' )
var result = deleteElement( 'org=Simple+Test/root=Organizations' )
if( result != '' )
   writeln( 'Could not remove simple element: ' + result )

// Done
writeln( 'Done' )
// @internal testorgs.fs c152bca
