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


/**
 * FormulaScript Servlet for logging in to a Managed Objects server.
 *
 * NOTE: This file is shipped with the Managed Objects product, so be very careful if
 *       you make changes, and keep a current backup, because this file will
 *       be replaced if you re-install the product.
 *
 * This servlet script has two roles.  First, if the "username" and "password"
 * parameters are present, the script will attempt to create a session in
 * Managed Objects using this authentication information.  If it succeeds, it will
 * set a SessionID cookie in the response header and present the intended
 * page as passed in the "uri" parameter, if present.  If it fails,
 * it will present the same login page, but with an error message as to
 * why the login attempt failed.
 *
 * If the "username" or "password" parameters are not present, it will present
 * an HTML form to request the user to login.
 */

// Setup global variables

// Set the HTTP response

response.setStatus( response.SC_OK )
response.setContentType( "text/html" );

// Determine if we should prompt for a login, or process a login.

var uri = request.getParameter( 'uri' );

if (uri == null)
   uri = request.getRequestURI() + '?' + request.getQueryString();

if (session != null)
   servlet.logout( request );

if (request.getParameter( "username" ) == null ||
    request.getParameter( "password" ) == null)
{
   promptForLogin( '' );
}
else
{
   processLogin();
}

// FUNCTIONS

function processLogin() {

   var username = request.getParameter( 'username' );
   var password = request.getParameter( 'password' );
   
   var message = servlet.login( username, password, request, response );
   
   if (message == '')
      redirect();
   else
      promptForLogin( message );
}

function promptForLogin( message ) {
   var p = response.getOutputStream();
   
   if (message == '')
      message = '&nbsp;';
   
   p.println( '<HTML><HEAD><TITLE>Welcome to Managed Objects</TITLE></HEAD><BODY BGCOLOR="#C0C0C0">' );
   p.println( '<P ALIGN=CENTER>Welcome to...<BR><IMG SRC=/images/formula-trans.gif></P><P ALIGN=CENTER>' );
   
   p.println( '<FORM NAME=login METHOD=POST ACTION="/Script">' );
   p.println( '  <INPUT TYPE=HIDDEN NAME=script VALUE="@servlets/login.fs">' );
   p.println( '  <INPUT TYPE=HIDDEN NAME=uri VALUE="'+uri+'">' );
   p.println( '  <TABLE ALIGN=CENTER>' );
   p.println( '  <TR><TD ALIGN=CENTER COLSPAN=2><FONT SIZE=-1 FACE="Verdana,Arial,Helvetica"><FONT COLOR="#FF0000">'+message+'</FONT><BR>Please enter your user name and password.<BR>&nbsp;</FONT></TD></TR>' );
   p.println( '  <TR><TD ALIGN=RIGHT><FONT SIZE=-1 FACE="Verdana,Arial,Helvetica">User Name:</FONT></TD><TD><INPUT TYPE=TEXT NAME=username></TD></TR>' );
   p.println( '  <TR><TD ALIGN=RIGHT><FONT SIZE=-1 FACE="Verdana,Arial,Helvetica">Password:</FONT></TD><TD><INPUT TYPE=PASSWORD NAME=password></TD></TR>' );
   p.println( '  <TR><TD ALIGN=CENTER COLSPAN=2><INPUT TYPE=SUBMIT VALUE="OK"> <INPUT TYPE=RESET VALUE="Reset">' );
   p.println( '  </TABLE>' );
   p.println( '</FORM>' );
   p.println( '</BODY></HTML>' );
   
   p.flush();
   p.close();
}

function redirect() {

   response.setStatus( response.SC_MOVED_TEMPORARILY )
   response.setHeader( 'Location', uri );

   
   var p = response.getOutputStream();

   p.println( '<HTML><HEAD><META HTTP-EQUIV="Refresh" CONTENT="3;' + uri + '"></HEAD><BODY>' );

   p.println( '<P>Thank you for logging in.  Your browser should be ' );
   p.println( 'automatically redirected to the desired web page.  If this does ' );
   p.println( 'not appear to happen, please click <A HREF="' + uri + '">here.</A>' );
   p.println( '<= /BODY></HTML>' );
   p.println( '<= /BODY></HTML>' ); 

   p.flush();
   p.close();

   /**/
}

// END OF FUNCTIONS

// @internal login.fs 64hih2h
