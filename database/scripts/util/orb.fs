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

function usage( msg )
{
   writeln( msg )
   writeln( '\nUsage: orb [options]' )
   writeln( '\nOptions:\n' )
   writeln( '\t-command [stop|status]' )
   writeln( '\t-host hostname' )
   writeln( '\t-port port' )
   writeln( '\t-name name' )
   return 0
}

function main()
{
   var command = 'status'
   var host = 'localhost'
   var port = 1574      // PatrolORB
   var name = 'ORB'
   
   // Parse arguments.
   if( args.length > 0 )
   {
      for( var i = 0; i < args.length; ++i )
        if( args[i].indexOf( '-' ) == 0 ) // option
        {
          if( args[i] == '-command' && i < args.length - 1 )
            command = args[++i]
          else if( args[i] == '-host' && i < args.length - 1 )
            host = args[++i]
          else if( args[i] == '-port' && i < args.length - 1 )
            port = args[++i]
          else if( args[i] == '-name' && i < args.length - 1 )
            name = args[++i]
          else
            return usage( 'Unexpected argument: ' + args[i] )
        }
   }
   else
      return usage( 'No arguments specified' )
   
   // Initialize the orb.
   var ORB = formula.util.ORB
   var orb = ORB.init()
   
   // Get the ior of the orb.
   var objKey = new java.lang.String( name ).getBytes()
   var ior = ORB.makeIOR( host, port, objKey, formula.util.ManageableHelper.id() )
   
   // Narrow to the server.
   var manageable = formula.util.ManageableHelper.narrow( orb.string_to_object( ior ) )
   
   if( command == 'stop' )
      for( var i = 0; i < 10; ++i )
      {
         var done = false
         try
         {
            writeln( 'Stop: ', manageable.manageStop() )
            writeln( 'AYT:  ', manageable.manageAreYouThere() )
            java.lang.Thread.sleep( 500 )
            done = true
         }
         catch( Exception )
         {
            if( ! done )
               writeln( Exception )
         }
      }
   else
      writeln( 'Status: ', manageable.manageStatus() )
   ORB.shutdown()
}

main()
// @internal orb.fs -289g3j4
