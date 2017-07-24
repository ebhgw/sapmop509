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
// Script to rename a host to another host in Eve configuration

ORB = Packages.com.mosol.util.CORBA.ORB
ORB.init( [], null )

function renameHost( configFile, oldHost, newHost )
{
    // Open the configuration.
    var ifile = new java.io.FileInputStream( configFile )
    var input = new java.io.ObjectInputStream( ifile )
    var manager = input.readObject()
    input.close()

    // Dump current hosts.
    writeln( "Current hosts: " )
    var hosts = manager.getActiveConfig().hosts()
    for( var i = 0 ; i < hosts.length ; ++i )
        writeln( "    Found host: " + hosts[i].name() )

    // Dump current alarm server host.
    write( "The Alarm Server is currently using host:" )
    var as = manager.getActiveConfig().alarmServer();
    var asHost = as.host();
        writeln( " " +  asHost.name())

    // Look through the active configuration for the target host.
    var changed = false
    var cfg = ORB.deref( manager.activeConfig )
    var host


    try
    {
       host = cfg.findHost( oldHost )
    }
    catch( Exception )
    {
       if (oldHost.equals(asHost.name())) {
          host = asHost;
       }
       else {
          writeln( "Could not find old host: " + oldHost, ": ", Exception )
          return
       }
    }


    try
    {
       writeln("Old Host:"+host.name()+", New Host:"+newHost)
       writeln("The hosts has these ports:")
       var ports = host.portNames();
       for(var i = 0; i<ports.length; ++i)
          writeln("- "+ports[i])
       
       cfg.renameHost( host, newHost )
       changed = true
       writeln( "Host renamed" )
    }
    catch( Exception )
    {
       writeln( "Could not rename old host: " + oldHost, ": ", Exception )
       return
    }


    // Did we change the configuration?
    if( changed )
    {
        // Check the work.
        writeln( "Configured hosts: " )
        hosts = manager.activeConfig.hosts
        for( var i = 0 ; i < hosts.length ; ++i )
            writeln( "    Found host: " + hosts[i].name() )

        // Rewrite config.
        var ofile = new java.io.FileOutputStream( configFile )
        var output = new java.io.ObjectOutputStream( ofile )
        output.writeObject( manager )
        output.close()
        writeln( "Configuration rewritten." )
    }
    else
        writeln( "Configuration was not changed." )
}


if( args.length != 3 ) {
    writeln( "Usage: renameHost configFile oldHost newHost" )
    for (var i=0;i<args.length;i++)
       writeln("arg["+i+"]:"+args[i]);
}
else
    renameHost( args[0], args[1], args[2] )

// Why do we need to do this?
java.lang.System.exit( 0 )
// @internal renamehost.fs 4hie7hg
