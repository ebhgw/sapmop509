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


function showElementProperties()
{
    // Create the frame.
    var f = new java.awt.Frame()
    f.setTitle( "Properties of: " + element.name )
    f.setSize( 400, 300 )
    formula.util.center( f )

    // Create the close handler
    var closeTheFrame = new java.awt.event.WindowListener() {
        windowClosing: function(event)
        {
            event.getSource().dispose()
        }
    }
    f.addWindowListener( closeTheFrame )

    // Add the list.
    var list = new java.awt.List()
    f.add( list, java.awt.BorderLayout.CENTER )

    // Add the properties
    for( var p in element.properties )
        list.add( p + ":  " + element[p] )

    // Show the frame.
    f.show()
}

if( ! this.element )
    throw "Element required"
showElementProperties()
