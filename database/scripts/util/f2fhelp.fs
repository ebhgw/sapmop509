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
// adapter() takes a Managed objects server host, port, adapter name, and
// command as arguments.  It will start/stop/status the named
// adapter on the named server.
//
// IF we are running within the Managed Objects Server, AND we are being
// asked to start a F2F adapter in the targetted server, THEN we
// Assume that the targetted Managed Objects server is us, we set the
// adapter's ServerIOR property to be our server's IOR.
//
// Example usage as element operation:
//
// [Start Remote Formula Adapter on fishbowl]
// description=Start F2F on fishbowl
// target=dname:root=Elements
// context=element
// permission=define
// type=serverscript
// operation=load( "util/f2fhelp" ) ; adapter( 'fishhead', 8080, 'Managed Objects on tailbone', 'start' )
//

function adapter( serverHost, serverPort, adapterKey, command )
{
    var result = ''

    // Get the Managed Objects server reference, and ask for a list of adapters.
    var formulaServer = null
    if (serverHost.equals('localhost')) {
	    formulaServer = server
    } else {
	    formulaServer = getServer( serverHost, serverPort )
    }
    var adapters = formulaServer.adapters()

    // Iterate the adapters, looking for the target.
    var i;
    for( i = 0 ; i < adapters.length ; ++i )
        if (adapters[i].key() == adapterKey)
            try
            {
                var adapter = adapters[i]

                if (command == 'start')
                {
                    // If we are running in a Managed Objects server, and we are
                    // trying to start an adapter to a Managed Objects server...

                    if( this.session && adapter.adapterClass().id() == 'formula' )
                    {
                        // ASSUME that this F2F adapter is pointed at us
                        // and set its Server property

                        adapter.putObjectProperty( 'Server', session.getServer().getReference() )
                    }

                    // Start the adapter.
                    result = adapter.manageStart()
                }

                else if (command == 'stop')
                    result = adapter.manageStop()

                else if (command == 'status')
                    result = adapter.manageStatus()

                else
                    result = 'Unknown command: ' + command

                result = adapterKey+ ': ' + command + ': ' + result
                break;
            }
            catch( Exception )
            {
                throw 'Unexpected error examining adapters: ' + Exception
            }

    if (i == adapters.length)
        throw 'Did not find the adapter named ' + adapterKey

    return result
}

function getServer( serverHost, serverPort ) {

    var result;
    var br

    try
    {
        // Open a stream to the host/port combination.
        //
        // Note: change http to https for a secured connection here.
        //
        var url = new java.net.URL( "http://" + serverHost + ":" + serverPort + "/Reference?version=5.6.0.96787" )
        var br = new java.io.BufferedReader( new java.io.InputStreamReader( url.openStream() ) )

        // Read the ior.
        var ior = br.readLine()

        // Turn the ior into the server reference.
        result = formula.util.ServerHelper.narrow( formula.util.ORB.init().string_to_object( ior ) );
    }
    catch( Exception )
    {
       throw 'Unexpected error getting server reference: ' + Exception
    }
    finally
    {
        if( br )
             try{ br.close() } catch( Anything ) {}
    }

    return result
}

// @internal f2fhelp.fs 1083hl5
