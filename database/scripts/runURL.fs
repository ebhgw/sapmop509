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

// @ o p t 1
// @debug off

function runURL(URLstr) {
        retval="";
        try {
                ReturnInfo=null;
                SetCookie=null;
                URLConn=null;
                FormulaUserName=null;
                FormulaPassword=null;
                JSESSIONID=null;
                
                url=new java.net.URL( URLstr );
                
                try {
                        URLConn=url.openConnection();
                        gotRequestProperties=URLConn.getRequestProperties();
                        formula.log.debug ( "doURL gotRequestProperties: " + URLstr + " " + gotRequestProperties );
                } catch (Exception) {
                        formula.log.debug ( "doURL HttpURLConn.getContent: " + URLstr + " " + ReturnInfo );
                }
                
                try {
                        bytes=URLConn.getContent();
                        ReturnInfo = new java.lang.String( bytes );
                        
                        gotHeaderFields=URLConn.getHeaderFields();
                        formula.log.debug ( "doURL gotHeaderFields: " + URLstr + " " + gotHeaderFields );
                        
                        SetCookie=URLConn.getHeaderField("Set-Cookie");
                        formula.log.debug ( "doURL SetCookie: " + SetCookie );
                                                
                        gotHeaderFields=URLConn.getHeaderFields();
                        formula.log.debug( "doURL gotHeaderFields: " + URLstr + " " + gotHeaderFields );
                } catch (Exception) {
                        formula.log.warn( "doURL HttpURLConn.getContent: " + URLstr + " " + Exception );
                }
                                
                
        } catch (Exception) {
                formula.log.warn ( "doURL " + URLstr + " " + Exception );
        }
        
        return retval;
        
}

