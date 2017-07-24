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

// @opt 9

/*  Show impacted.

	ShowImpacted.fs
    Version 1.2
	Managed Objects Engineering

	Updates:
    - 1.2 Kurt Westerfeld 10/31/2001
        tweaked for scripting upgrade (extensible services)
    - 1.1 Kurt Westerfeld 2/12/2001
        tweaked for version 2.0
	- 1.0 Tobin Isenberg 10/19/2000 (and alot of help from Kurt!!!)
		implemented the 1.0 version of the utility.  When you right click
		on an object on the Elements tab, this will determine which 
		organization objects that contains this element.  Keep in mind that
		it deals directly with the fully qualified DName.  In otherwords,
		if the same host is under multiple trees, it will not show up as
		a child of an Organization unless you pick the correct one.


Operations.ini definition:

[Show Impacted]
description=Show Impacted...
context=element
target=namematch:.*
permission=view
type=serverscript
operation=@util/showImpacted

*/


function showNamePath( element )
{
   if( element.dname == 'root=Organizations' || element.dname == '' )
       return element.name

   var parent = element.parent
   if( ! parent )
       return element.name
   else
      return showNamePath( parent ) + '/' + element.name
}

function showRelated( element )
{
    var Result

    // Get the relationships for this element.
    var relationships = element.relationships
    if( relationships.length == 0 )
       Result = "This object is not related to any of the Business Views"
    else
    {
       Result = 'Objects impacted by ' + element.name + ':\n\n'

       // For each of my parents
       for( var i = 0; i < relationships.length; i++ )
           Result += '        ' + showNamePath( relationships[i] ) + '\n'
    }

    return Result

}

session.sendMessage( showRelated( element ) )

// @internal showimpacted.fs clkhl4k
