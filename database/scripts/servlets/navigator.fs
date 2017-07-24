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
 * FormulaScript Servlet for navigating around elements
 *
 * Servlets written in FormulaScript are invoked in the service() method
 * of the servlet.  The following contextual information is available to
 * FormulaScript servlets:
 *
 * Name                 Value
 * ====                 =====
 * server               Managed Objects server object
 * session              the currently logged in session
 * request              HttpServletRequest
 * response             HttpServletResponse
 * servlet              HttpServlet
 * HttpServletRequest   HttpServletRequest class
 * HttpServletResponse  HttpServletResponse class
 * Util                 MOS general utility class
 *
 * This servlet expects the following parameters:
 *
 * script  - this FormulaScript file
 * element - distinguished name of the element to represent
 * refresh - whether to auto-refresh the page: 'on' or 'off'
 * rate    - number of seconds to wait before refreshing page
 * front   - whether to bring the page to the top on load
 *
 * @internal
 */

// Setup global variables

var default_rate    = 300;

// should we auto-refresh?
var refresh = (request.getParameter( 'refresh' ) != null) ? request.getParameter( 'refresh' ) : 'off';

// number of seconds to wait before automatically refreshing the page
var rate = (request.getParameter( 'rate' ) != null) ? request.getParameter( 'rate' ) : default_rate;

// make our Managed Objects session last 30 seconds longer than our refresh rate
request.getSession().setMaxInactiveInterval( rate + 30 );

// should we bring to front on refresh?
var front = (request.getParameter( 'front' ) != null) ? request.getParameter( 'front' ) : 'off';

var condNames;
condNames = new Array();
condNames[0] = 'UNKNOWN';
condNames[1] = 'CRITICAL';
condNames[2] = 'MAJOR';
condNames[3] = 'MINOR';
condNames[4] = 'INFORMATIONAL';
condNames[5] = 'OK';
condNames[6] = 'INITIAL';
condNames[7] = 'IDLE';
condNames[8] = 'ACTIVE';
condNames[9] = 'BUSY';

var condColors = new Array();
condColors[0] = '#C0C0C0';      // lightGray
condColors[1] = '#FF0000';      // red
condColors[2] = '#FFC800';      // orange
condColors[3] = '#FFFF00';      // yellow
// condColors[4] = '#00FFFF';      // cyan REAL COLOR
condColors[4] = '#00FF00';      // green
condColors[5] = '#00FF00';      // green
condColors[6] = '#A67100';      // 166, 113, 0
condColors[7] = '#00FF00';      // green
condColors[8] = '#00FFFF';      // cyan
condColors[9] = '#00FFFF';      // cyan

// Markup map....to map our funny client-side markup language to html
var markupMap =
    [
    [ /\[newline\]/gi         , "<br>" ],
    [ /\[bgcolor=white\]/gi   , "" ],
    [ /\[bgcolor=default\]/gi , "" ],
    [ /\[color=default\]/gi   , "<FONT COLOR=\"#000000\">" ],
    [ /\[color=black\]/gi     , "<FONT COLOR=\"#000000\">" ],
    [ /\[color=lightgrey\]/gi , "<FONT COLOR=\"#C0C0C0\">" ],
    [ /\[color=lightgray\]/gi , "<FONT COLOR=\"#C0C0C0\">" ],
    [ /\[color=gray\]/gi      , "<FONT COLOR=\"#808080\">" ],
    [ /\[color=grey\]/gi      , "<FONT COLOR=\"#808080\">" ],
    [ /\[color=darkgray\]/gi  , "<FONT COLOR=\"#404040\">" ],
    [ /\[color=darkgrey\]/gi  , "<FONT COLOR=\"#404040\">" ],
    [ /\[color=white\]/gi     , "<FONT COLOR=\"#FFFFFF\">" ],
    [ /\[color=red\]/gi       , "<FONT COLOR=\"#FF0000\">" ],
    [ /\[color=green\]/gi     , "<FONT COLOR=\"#00FF00\">" ],
    [ /\[color=blue\]/gi      , "<FONT COLOR=\"#0000FF\">" ],
    [ /\[color=yellow\]/gi    , "<FONT COLOR=\"#FFFF00\">" ],
    [ /\[color=cyan\]/gi      , "<FONT COLOR=\"#00FFFF\">" ],
    [ /\[color=pink\]/gi      , "<FONT COLOR=\"#FF00FF\">" ],
    [ /\[color=orange\]/gi    , "<FONT COLOR=\"#FF8020\">" ]
    ]

// Set the HTTP response

response.setStatus( HttpServletResponse.SC_OK )
var now = new Date();
response.setDateHeader( "Expires", now.getTime() );
response.setContentType( "text/html" );

// Write the HTML document dynamically

var p = response.getOutputStream();

// Get the "element" parameter passed to the servlet.
// This is a URL-encoded distinguished name for an
// element.
var elementString = request.getParameter( 'element' );

if (elementString == null)
   elementString = 'root=Elements';

// Find the named element object, and print a site map for it.
var element = null;
try {
   element = session.findElementImpl( elementString );
   if( ! element )
      throw 'Element could not be found'
} catch ( ex ) {  
      var str = beginPage( null, 'Exception for Element' );
      str += '<FONT FACE="Verdana,Arial">';
      str += '<TABLE BORDER=1 WIDTH=100%><TR BGCOLOR=RED><TD ALIGN=LEFT>';
      str += 'Sorry, for element "' + elementString + '": ' + ex;
      str += '</TD></TR></TABLE>';
      str += endPage( null );
      p.println( str );
      element = null;
}

if ( element ) {
   // Render the element
   p.println( elementMap( element ) );
}

p.flush();
p.close();

// END OF SERVLET


// FUNCTIONS

/**
 * elementMap
 *
 * Take an element that refers to anything and produce an HTML page from it.
 *
 * @param element the element
 * @return a string that contains an HTML page
 */
function elementMap( element ) {

   // Find the element's children

   var children = getChildren( element );

   // Build a sorted list of children
   
   var children_array = new Array();
   var i = 0;
   for (var e in children)
      children_array[i++] = e;
   children_array = children_array.sort();

   // Render the mapping of presentation servers to con servers

   var str = beginPage( element, 'Details of ' );

   str += '<FONT FACE="Verdana,Arial">\n';

   var elementClassName = element.getElementClass().getName();

   var img = '<IMG WIDTH=32 HEIGHT=32 ALT="'+element.getName()+' ('+elementClassName+')" SRC="/icons?uri=' + formula.util.encodeURL( element.getDName() ) + '">\n';

   str += '<TABLE BORDER=1 WIDTH=100%><TR BGCOLOR=' + condToColor( element.getCondition() ) + '>\n';
   var now = '\n<SCRIPT language="JavaScript">\nnow = new Date();\ndocument.write(now.toLocaleString());\n</SCRIPT>\n';
   str += '<TD COLSPAN=1 ALIGN=CENTER>' + img + '</TD>\n';
   var parent = element.getNameParent();
   var parentLink = '<A HREF="' + elementToURI( parent ) + '">' + parent.getName() + '</A>';
   str += '<TD COLSPAN='+children_array.length+' ALIGN=CENTER><FONT SIZE=+2>' + element.getName() + ' (' + parentLink + ')</FONT><BR><FONT SIZE=-1>'+now+'</FONT></TD></TR>\n';

   for (var i = 0; i < children_array.length; i++) {
      var child = children[children_array[i]];
      str += '\n  <TR>';
      str += elementToIcon( child );
      str += '<TD ALIGN=LEFT>';
      str += getNotes( child ) + '</TD>';
      str += '</TR>\n';
   }

   str += '\n</TABLE>';

   str += endPage( element );

   return str;
}

/**
 * elementToIcon
 *
 * Convert a Managed Objects element into an HTML fragment that depicts the
 * element iconically.  Moving the mouse over the icon will show
 * current status information, and clicking on the icon will show
 * greater detail of the element.
 *
 * @param       element the element object
 * @return      the HTML text to depict this element
 */
function elementToIcon( element ) {

   var name  = element.getName();
   var link  = elementToURI( element );
   // TODO:
   // why is this alt attr in the IMG tag?  Was messing this up!
   // ALT="'+getNotes( element )+'"
   var img   = '<IMG width=16 height=16 SRC="/icons?uri=' + formula.util.encodeURL( element.getDName() ) + '">';

   str = '<TD ALIGN=CENTER BGCOLOR=' + condToColor( element.getCondition() ) + '>'+img+'<A HREF="'+link+'">';
    
   str += name;
   
   str += '</A></TD>';

   return str;
}

/**
 * elementToURI
 *
 * Convert an element into a URI that can be used to view the given
 * element.
 */
function elementToURI( element ) {
   var str = '/Script?script=@servlets%2Fnavigator.fs';
   str += '&front=' + front;
   str += '&refresh=' + refresh;
   str += '&rate=' + rate;
   str += '&element=' + java.net.URLEncoder.encode( element.getDName() );
   return str;
}

/**
 * getNotes
 *
 * Return the notes for the given element, in readable form.  The strategy is this:
 *
 * - If the element has no children, return its actual notes property.  But if
 *   the notes property is not printable, just shows its status.
 * - If the element has children, build a string that has the notes of each of its
 *   children that appear to be performance metrics.
 */
function getNotes( element ) {
   var children = getChildren( element );

   var text = '';
   for (var e in children) {
      var child = children[e];

      if (child.getElementClass().getName() == 'patrolParm')
      {
         if (text.length > 0) text += '\n';
         text += getNotes( child );
      }
   }

   // Get the notes text.
   if (text == '')
      text += Packages.com.mosol.Formula.Client.engine.ElementUI.getNotes( element.notes() )

   // Get rid of unnecessary precision on numeric values

   if (text.indexOf( '.0' ) == text.length-2)
      text = text.substring(0,text.length-2)

   // Make textual translations.
   for( var i = 0 ; i < markupMap.length ; ++i )
      text = text.replace( markupMap[i][0] , markupMap[i][1] )
   
   return text;
}

/**
 * getChildren
 *
 * Return the children of an element in a JavaScript object
 *
 * NOTE: This implementation could have a problem if a given child had
 * the same name as another, but was of a different element class.  Beware.
 */
function getChildren( element ) {
   var Result = new Object();

   var children = session.findChildrenElements( element, Packages.com.mosol.ORB.Formula.RelationKind.NAM );
   if (children != null)
   {
      for (var e = children.elements(); e.hasMoreElements();)
      {
         var child = e.nextElement();
         Result[child.getName()] = child;
      }
   }

   return Result;
}

/**
 * beginPage
 *
 * Return an HTML section that begins all generated web pages.
 */
function beginPage( element, intro ) {

   var title = intro + ( element ? element.getName() : "" );

   var str = '<HTML>\n<HEAD>\n';

   if (refresh == 'on')
      str += '<META HTTP-EQUIV="Refresh" CONTENT="'+rate+'">';

   str += '<TITLE>'+title+'</TITLE>\n</HEAD>\n<BODY>\n';

   if (front == 'on')
      str += '<SCRIPT language="JavaScript">window.focus();</SCRIPT>\n';

   return str;
}

/**
 * Complete the contents of the generated web page
 */
function endPage( element ) {
   var str = '<TABLE WIDTH=100%><TR><TD VALIGN=BOTTOM ALIGN=CENTER>\n';

   if (element != null) {
      str += '<FORM NAME="settings">';
      str += '<INPUT NAME="script"  TYPE="hidden"   VALUE="@servlets/navigator.fs">';
      str += '<INPUT NAME="element" TYPE="hidden"   VALUE="' + element.getDName() + '">';
      str += '<INPUT NAME="refresh" TYPE="checkbox" ' + ((refresh == 'on') ? 'CHECKED' : '') + '>Update every ';
      str += '<INPUT NAME="rate"    TYPE="text"     SIZE=5 MAXLENGTH=5 VALUE="'+rate+'"> seconds, and ';
      str += '<INPUT NAME="front"   TYPE="checkbox" ' + ((front == 'on') ? 'CHECKED' : '') + '> bring to front. ';
      str += '<INPUT                TYPE="submit" VALUE="Set">';
      str += '</FORM></TD>\n\n';
   }

   if (session != null)
      str += '<TD VALIGN=CENTER><A HREF="/Script?script=@servlets%2Flogin.fs">Logout '+session.user().fullName()+'</A></TD>';
   
   str += '<TD><IMG ALIGN=RIGHT ALT="Your Logo Here" SRC="/images/YourLogo.gif"></TD></TR></TABLE>\n';

   str += '</BODY>\n</HTML>';

   return str;
}

/**
 * Convert a Managed Objects ElementCondition to its English name.
 */
function condToName( cond ) {
   return condNames[ cond.value() ];
}

/**
 * condToColor
 *
 * Convert a Managed Objects ElementCondition to its corresponding canonical color
 * representation in #RRGGBB format.
 */
function condToColor( cond ) {
   return condColors[ cond.value() ];
}

/**
 *
 * condToFont
 *
 * Convert a Managed Objects ElementCondition to its corresponding HTML FONT tag.
 */
function condToFont( cond ) {
   return '<FONT COLOR=' + condToColor( cond ) + '>';
}

/**
 */
function usage() {

   var str = '<HTML><HEAD><TITLE>navigator.fs - usage</TITLE></HEAD><BODY>';

   str += '<H1>Usage for the navigator.fs servlet:</H1>';

   str += '<PRE>/Script?script=servlets%2fnavigator.fs\n';
   str += '    &element=&lt;distinguished name of element to render&gt;\n';
   str += '    &refresh=&lt;on or off -- whether to automatically refresh page&gt;\n';
   str += '    &rate=&lt;number of seconds to wait before refreshing if refresh=on&gt;\n';
   str += '    &front=&lt;on or off -- bring browser to front on refresh&gt;\n</PRE>';
   str += '<P>Make sure to URL-encode your parameters!';
   str += '</BODY></HTML>';

   return str;
}

// END OF FUNCTIONS

// @internal navigator.fs e21l72j
