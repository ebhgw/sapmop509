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

///////////////////////////////////////////////////////////////////////////////////
// @title Send element via paging gateway
//
// @define element
// @param pager The paging id (optional, can use element)
// @param message The paging message (optional, can use element)
// @param server The snpp server address (optional)
// @param port The snpp server port (optional)
//
// Note:    we assume "element" contains the element
//          that caused this script to be run.
//
//          This element must contain the "Pager"
//          property, or we don't know what to do!
//
// Use:
//          The page function pages a given pagerid
//          at pagemart.net.  If another host is
//          desired, the third parameter is the host
//          name (optional).  Additionally, the port
//          can be supplied with an optional fourth
//          parameter.
//

// Check the element.
if( this.element )
{
    // Parse pager.
    var _pager = ""
    if( this.element.Pager && this.element.Pager != "" )
        _pager = this.element.Pager
    if( this.pager )
        _pager = this.pager

    // Parse message.
    var _message = "Element " + this.element.name + " has a problem"
    if( this.message )
        _message = this.message

    // Parse server and port.
    var _server = ""
    var _port = ""
    if( this.server )
        _server = this.server
    if( this.port )
        _port = this.port

    // Check the pager.
    if( _pager != "" && _message != "" )

        // Send a simple message.
        page( _pager, _message, _server, _port )

    else
        throwError( "Pager contact or message is missing" )
}
else
    throwError( "No element to retrieve pager to contact" )
