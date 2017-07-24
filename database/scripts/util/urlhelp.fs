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
// Name;        getURLContents()
// Description: Get the contents of a given url string.
// Example:     getURLContents( 'http://www.somesite.com/someuri' )
//
function getURLContents( urlString )
{
	var Result = 'ERROR'
    var debug = false
	var stream

	try
	{
	   // Make the url and open the stream.
	   var url = new java.net.URL( urlString )
	   var stream = url.openStream()

	   // Stream is open, now lets pull data from it
       var bytes = formula.util.toByteArray( stream )
	   Result = new java.lang.String( bytes )

	   // Print whatever was grabbed from the stream
	   if( debug ) info( Result )
	}
	catch( Exception )
	{
	   if( debug ) writeln( 'Error getting stream contents: ' + Exception )
	}
	finally
	{
	   // Close the stream, if it had been opened above
	   if( stream )
		  stream.close()
	}

	return Result
}


// Test driver; should print out url of Managed Objects web site
//
// To run:  fscript -f urlhelp.fs -A test
if( args && args.length > 0 && args[0] == 'test' )
{
   var contents = getURLContents( 'http://www.managedobjects.com' )
   writeln( 'URL content: ' + contents )
}
// @internal urlhelp.fs -aj6h962
