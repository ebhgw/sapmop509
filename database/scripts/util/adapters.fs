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
// A utility to interactively query and manage Managed Objects adapters.
//

function adapters()
{
    var doLogin = this.session == undefined || this.session == null

    // Do the interactive login.
    if( doLogin ) load( 'util/login' )
    if( ! doLogin || login() )
    {
        // Get a nice input stream.
        var inStream = this['in']
        if( inStream == undefined )
            inStream = java.lang.System['in']
        var input = new java.io.BufferedReader( new java.io.InputStreamReader( inStream ) )

        // Get adapters.
        var adapters = formula.server.adapters()

        while( true )
        {
            // Show adapters.
            writeln( '\n\nAdapters: ' )
            for( var i = 0 ; i < adapters.length ; ++i )
            {
                var adapter = adapters[i]
                var status = ''
                try
                {
                    status = adapter.manageStatus()
                }
                catch( Exception )
                {
                    status = "Exception: " + Exception
                }
                writeln( '   ', i, ' ', adapter.key(), ': ', status )
            }

            // Ask for an adapter.
            write( '\nAdapter: ' )
            var command = new String( input.readLine() )
            if( ! command || command == "" )
                continue
            if( command == 'q' )
                break
            if( command >= 0 && command < adapters.length )
            {
                // Do the command query.
                var adapter = adapters[ command ], autostart = ''
                if( adapter.startOnStartup() )
                    autostart = ' (autostart)'
                writeln( '\n', adapter.key(), autostart )
                write( 'Command: (q)uery, star(t), sto(p) (a)autostart: ' )
                command = new String( input.readLine() )
                writeln( )

                try
                {
                    // Query
                    if( command == 'q' || command == 'query' )
                        writeln( "Result: ", adapter.manageStatus() )

                    // Start.
                    else if( command == 't' || command == 'start' )
                        writeln( "Result: ", adapter.manageStart() )

                    // Stop.
                    else if( command == 'p' || command == 'stop' )
                        writeln( "Result: ", adapter.manageStop() )

                    // Autostart.
                    else if( command == 'a' || command == 'autostart' )
                        adapter.startOnStartup( ! adapter.startOnStartup() )

                    // Invalid.
                    else
                        writeln( 'Invalid command: ', command )
                }
                catch( Exception )
                {
                    writeln( "Exception: " + Exception )
                }
            }
        }

        // Shutdown.
        if( doLogin ) formula.logout( this.session )
    }
}

adapters()
// @internal adapters.fs -cb42k1j
