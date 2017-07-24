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

//////////////////////////////////////////////////////////////////////////////////////////
// A utility to send a message to a user or group of users from a Managed Objects server.
//

function getout()
{
    formula.logout( this.session )
    java.lang.System.exit( 0 )
}

function findTargetElement( who )
{
   var element
   if( who.match( /Group:/ ) )
       element = formula.Root.findElement( 'group=' + formula.util.encodeURL( who.substring( 6 ) ) + '/groups=Groups/security=Security/root=Administration' )
   else if( who.match( /:/ ) )
       element = formula.Root.findElement( 'session=' + formula.util.encodeURL( '[' + who + ']' ) + '/sessions=Sessions/formulaServer=Server/root=Administration' )
   else
       element = formula.Root.findElement( 'user=' + formula.util.encodeURL( who ) + '/users=Users/security=Security/root=Administration' )

   if( element && element.perform )
      return element
   else
      return null
}

function wall( who, message )
{
    load( 'util/login' )
    if( login() )
    {
        // Were we passed a group name?
        var element = findTargetElement( who )
        if( element )
        {
            writeln( 'Found target element: ', element )
            var Result = element.perform( 'SendMessage', message )
            java.lang.Thread.sleep( 1000 )
            if( Result )
               writeln( '\nResult of wall operation:\n\n', Result )
        }
        else
            writeln( 'Could not resolve argument: ' + who )
        getout()
    }
}

if( args.length == 2 )
    wall( args[0], args[1] )
else
{
    writeln()
    writeln( 'NetIQ Operations Center(tm) (c) 2014, NetIQ Corporation.' )
    writeln()
    writeln( 'Usage:' )
    writeln( '    moswall [ username | Group:groupname | username:sessionid ] [ message ]' )
    writeln()
    writeln( 'Examples:' )
    writeln( '    moswall joeuser "Hi there!"' )
    writeln( '    moswall Group:users "System is coming down in 2 minutes..."' )
    writeln( '    moswall admin:42 "Check the router status now!"' )
}

// @internal wall.fs d2hbad0
