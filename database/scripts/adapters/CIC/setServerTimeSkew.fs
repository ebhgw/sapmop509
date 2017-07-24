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


// Import some classes we need.
var Util = Packages.com.mosol.util.Util
var Protocol = Packages.com.mosol.util.Protocol
var skewLog = formula.log.getInstance( 'Adapter.Netcool.' + adapter.getKey() + '.CalcSkew' )

// Check parameters.
if( ! this.adapter )
    throwError( 'Adapter parameter required' )

//
// Our date formats and the command constant.
// Some 'date' utilities on unix could use different formatting.
//
var unixDateFormat = '+%m/%d/%Y %H:%M:%S'
var javaDateFormat = 'MM/dd/yyyy HH:mm:ss'
var cmd = "date '" + unixDateFormat + "'"

//
// By default, the remote account information is used
// from the adapter.  Replace if desired.
//
var host   = adapter.getPropertyString( 'ObjectServerHost', null )
var user   = adapter.getPropertyString( 'ObjectServerAccount', null )
var passwd = adapter.getPropertyString( 'ObjectServerPassword', null )
if( ! host )
    throwError( 'Could not get host property' )
if( ! user )
    throwError( 'Could not get user property' )
if( ! passwd )
    throwError( 'Could not get passwd property' )

// Get the remote time.
skewLog.debug( 'performing rexec with user ' + user + ' against host ' + host )
skewLog.debug( 'rexec command: ' + cmd )

try {
  var input = Protocol.rexec( host, user, passwd, cmd )
  if( ! input )
      throwError( 'No result from rexec' )
  var result = new java.lang.String( Util.toByteArray( input ) )
  input.close()
}
catch (exception) {
  skewLog.error( "Remote server doesn't support remote process execution.  Please open port 512 on " + host + " to use this script." )
  throw exception
}

result = Util.searchAndReplace( result, '\n', '' )
result = Util.searchAndReplace( result, '\r', '' )
skewLog.debug( 'rexec result: ' + result )

// Parse the date.
var dateFormatter = new java.text.SimpleDateFormat( javaDateFormat )
var parsedDate = dateFormatter.parse( result )
if( ! parsedDate )
    throwError( 'Could not parse date' )
skewLog.debug( 'parsed server date/time: ' + parsedDate )

// Set the skew.
var now = new java.util.Date()
var skew = now.getTime() - parsedDate.getTime()
skewLog.debug( 'got the skew: ' + skew )
adapter.setServerTimeSkew( skew )
// @internal setServerTimeSkew.fs a8b4b9j
