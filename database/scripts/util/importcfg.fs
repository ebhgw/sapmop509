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
// Contact the Managed Objects Server meta element and tell it to import the configuration.
// 
// NOTE: this script can be modified to operate remotely, but currently it
// assumes it is running on the Managed Objects Server.
//

function getout()
{
    formula.logout( this.session )
    java.lang.System.exit( 0 )
}

function importConfig( filename, removeDeleted )
{
	 // We don't need the client automation processor panel
	 java.lang.System.setProperty('client.automation.processor.panel.disable','true')
	 // Perform login
    load( 'util/login' )
    if( login() )
    {
        // Find the server element.
        var element = formula.Root.findElement( 'formulaServer=Server/root=Administration' )
        if( element )
        {
            // Opening file stream.
            var fis = new java.io.FileInputStream( filename )
            var ins = new Packages.com.mosol.util.CORBA.RemoteInputStreamAdapter( fis )
            var insRef = formula.util.ORB.init().object_to_string( ins.getReference( 'all' ) )

            // writeln( 'Found target element: ', element )
            writeln( 'Located server, importing configuration...' )
            var Result = element.perform( session, 'Config|ImportSilent', [], [ insRef, new java.io.File( filename ).getAbsolutePath(), removeDeleted ] )
            java.lang.Thread.sleep( 1000 )
            if( Result )
               writeln( '\nResult of import operation:\n\n', Result )

            // Done
            fis.close()
        }
        else
            writeln( 'Could not resolve argument: ' + who )
        getout()
    }
}

if( args.length == 1 || args.length == 2 ) {
    importConfig( args[0], args.length == 2 ? args[1] : 'false' )
} else if( args.length >= 4 ) {
    try {
      formula.util.importConfig( args[0], args[1], args[2], args[3], args.length > 4 ? args[4] : 'false' )
      } catch(e) {
        if( e.getClass().getName().equals("com.mosol.acl.NotFound")) {
        	writeln("User name or password is invalid.")
        } else {
          throw e
        }
      }
} else {
    writeln()
    writeln( 'NetIQ Operations Center(tm) (c) 2014, NetIQ Corporation.' )
    writeln()
    writeln( 'Usage 1:' )
    writeln( '    importcfg filename [true|false (merge/delete)]' )
    writeln( '    (prompts user for userid and password and imports into running Server configuration)' )
    writeln()
    writeln( 'Usage 2:' )
    writeln( '    importcfg filename configurationName userid password [true|false (merge/delete)]' )
    writeln( '    (imports into named configuration--server should not be active with this configuration)' )
    writeln()
    writeln( 'Examples:' )
    writeln( '    importcfg allData.config.xml' )
    writeln( '    importcfg Organizations.config.xml default admin formula' )
}



// @internal importcfg.fs -408cd81
