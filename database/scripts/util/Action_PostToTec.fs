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
// @title Post event message to T/EC
//
// @param server The hostname of the TEC server
// @param port The port of the TEC server (optional; defaults to 0)
// @param message The message to send to TEC
// @param cls The event class (optional; defaults to LogFile_Base)
// @param source The event source (optional; defaults to Formula)
// @param severity The event severity (optional; defaults to WARNING)
//

// Set our logging category
formula.logCategory = 'Automation.PostToTec'

var _server = ""
if( this.server )
    _server = this.server
else
   throw "You must supply a TEC server to send to"

var _message = ""
if( this.message )
    _message = this.message
else
   throw "You must supply a message to send"

// Parse message parameters.
var _cls = "LogFile_Base"
if( this.cls && this.cls != "" )
    _cls = this.cls

var _source = "Formula"
if( this.source && this.source != "" )
    _source = this.source

var _severity = "WARNING"
if( this.severity && this.severity != "" )
    _severity = this.severity

var _port = 0
if( this.port && this.port > 0 )
   _port = this.port

// Construct the message.
var p = new formula.util.Postemsg( _server, _cls, _source )
p.setSeverity( _severity )
if( _port > 0 )
   p.setPort( _port )

// Send the messge.
try
{
   formula.log.info( 'Sending message ' + _message + ' to ' + _server + ' on port ' + _port )
   p.sendMessage( _message )
}
catch( Exception )
{
   formula.log.error( 'Unable to send message; error was: ' + Exception )
   formula.log.info( 'Message to send was (host:' + _server + '/' + _port + ', class: ' + _cls + ', source:' + _source + '):\n' + _message )
}


