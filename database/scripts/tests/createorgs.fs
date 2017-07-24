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

// 
//     Create Organizations Dynamically While Managed Objects Server is Running
// 
// This script can be used to test the functionality of the orgs.fs script, and
// as a model for building similar scripts for customer use.
// 
// The createorgs.fs script will drive orgs.fs to create 2 test organizations,
// which can be verified by examining the Organizations in the Managed Objects Browser
// or the Elements.ini file.
// 
// To use:
// 
// 1. Modify the code below these comments in this script.fs to supply the correct
//    login userid and password.
// 
// 2. Run createorgs.fs from the command line, using the Managed Objects "fscript" command:
// 
//    x:\ManagedObjects\database\scripts\util> fscript -f createorgs.fs
//
//    NetIQ Operations Center(r) Copyright 2014 NetIQ Corporation.
//
//    Org delete OK
//    Org delete OK
//    Org create OK
//    Org create OK
//    Shutting down....
//
//    The first time the script is run, the "delete" messages may fail; this indicates
//    the objects to be created were not located.  These messages should be ignored.
// 
// 3. Login to the Managed Objects Browser, and examine the two organizations created. There
//    should be a "Dynamic Organization" organization, that has one child organization
//    called "Child Organization", which should have two elements assigned to it.
//  
// 


// Login; we can hard code login values if we wish, as follows:
load( 'util/login' )
login( 'localhost', 80, 'admin', 'formula' )


// Load the org builder script.
load( 'util/orgs' )


// Function to show the result of creation.
function showResult( verb, value )
{
   if( value != '' )
      writeln( 'Could not ' + verb + ' org: ' + value )
   else
      writeln( 'Org ' + verb + ' OK' )
}

// Cleanup orgs from last run
showResult( 'delete', deleteElement( 'org=Child+Organization/org=Dynamic+Organization/root=Organizations' ) )
showResult( 'delete', deleteElement( 'org=Dynamic+Organization/root=Organizations' ) )

//
// Create a top-level org, using createElement(). This org will have empty 
// children ("new Array()") for now.
//
// createElement() uses an object that must have the following properties:
//
//     name, parent, class
//
// The object passed to createElement() uses an object called "props" that has
// any of the following properties, with "Children" required:
//
//     Children, Contact, Company, Address, Phone, Fax, Pager, Email, Graphic, DisplaySourceElements
//
// The first 3 properties listed are required.
//
var topOrg      = new Object
topOrg.name     = 'Dynamic Organization'
topOrg.parent   = 'root=Organizations'
topOrg.clazz    = 'org'
topOrg.props = new Object
topOrg.props.Children = []
topOrg.props.Contact  = 'your contact information here'
topOrg.props.Company  = 'your company name here'

showResult( 'create', createElement( topOrg ) )


//
// Create a child org of the parent, this time using createElement2(). This will 
// have some children (specified as dnames) that are assumed to already exist.
//
// createElement2() takes variable arguments in the order:
//
//     name, parent, children, clazz, contact, company, address, phone, fax, pager, email, graphic, displaySourceElements
//
// Only the first 4 arguments are required.
//
var children = new Array()
children[0]  = 'unicenter=My+WorldView/root=Elements'
children[1]  = 'tec=My+tec/root=Elements'

showResult( 'create', createElement2( 'Child Organization',
                                      'org=Dynamic+Organization/root=Organizations',
                                      children,
                                      'org',
                                      '',
                                      'your company name here' ) )


// Clean getaway.
writeln( 'Shutting down....' )
formula.logout( session )
delete session
java.lang.Thread.sleep( 5000 )
java.lang.System.exit( 0 )
