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

// FormulaScript Servlet Example
//
// Servlets written in FormulaScript are invoked in the service() method
// of the servlet.  The following contextual information is available to
// FormulaScript servlets:
//
// Name                 Value
// ====                 =====
// server               Managed Objects server object
// session              Managed Objects session object
// request              HttpServletRequest
// response             HttpServletResponse
// servlet              HttpServlet

// Set the HTTP response

response.setStatus( response.SC_OK )
var now = new java.util.Date();
response.setDateHeader( "Expires", now.getTime() );
response.setContentType( "text/html" );

// Write the HTML document dynamically

var p = response.getOutputStream();
p.println( '<html><head><title>Testing</title></head><body>' );
p.println( '<h1>Testing</h1>' );
var elementString = request.getParameter( 'element' );

if ( elementString != null )
{
   p.println( '<p>Element is "' + formula.util.decodeURL( elementString ) + '"' );
   
   var element = formula.Root.findElement( formula.util.decodeURL( elementString ) );
   
   p.println( '<p>Element\'s simple name is ' + element.getName() );
   p.println( '<p>Element\'s condition is '   + element.getCondition().value() );
}                

p.println( '<p>Your full name is probably ' + session.user().fullName() );

p.println( '<p>You can log out now by clicking <A HREF="/Script?script=@servlets%2Flogin.fs">here</A>.' );

// Complete the dynamically generated HTML document

p.println( '</body></html>' );
p.flush();
p.close();

// end of servlet

// @internal dump.fs -a5b7l61
