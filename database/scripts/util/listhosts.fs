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

//////////////////////////////////////////////////////////////////////
// Script to get hosts in an Eve configuration


ORB = Packages.com.mosol.util.CORBA.ORB
EVE = Packages.com.mosol.ORB.Eve

ORB.init( [], null )

function exportHostNames( configFile )
{
    // Open the configuration.
    var ifile = new java.io.FileInputStream( configFile )
    var input = new java.io.ObjectInputStream( ifile )
    var manager = input.readObject()
    input.close()

    // Dump current Hosts.
    writeln( "Current hosts: " )
    var hosts = manager.getActiveConfig().hosts();
    for( var i = 0 ; i < hosts.length ; ++i ) {
        var hostName = hosts[i].name();
        writeln( "    Found host: " + hostName)
        var ports = hosts[i].ports();
        for (var ii=0; ii<ports.length; ++ii) {
           var portName = ports[ii].name();
           writeln( "      - Found port: " + portName)
        }
    }
    writeln( "Current fan-out hosts: " )
    var fanHosts = manager.getActiveConfig().fanOutHosts();
    for( var i = 0 ; i < fanHosts.length ; ++i ) {
        var fanHostName = fanHosts[i].name();
        writeln( "    Found host: " +  fanHostName)
    }
    
    
    
    // Dump current Alarm Server host
    writeln( "Current Alarm Server Properties: " )
    var as = manager.getActiveConfig().alarmServer();
    var asHost = as.host();
        writeln( "    Found alarm server host: " +  asHost.name())

}


if( args.length != 1 ) {
    writeln( "Usage: exportHostNames configFile" )
    for (var i=0;i<args.length;i++)
       writeln("arg["+i+"]:"+args[i]);
}
else
    exportHostNames( args[0] )

java.lang.System.exit( 0 )
// @internal listhosts.fs -dd5b2e1
