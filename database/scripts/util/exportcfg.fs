// Copyright � 2014 NetIQ Corporation. All Rights Reserved.
// USE AND REDISTRIBUTION OF THIS WORK IS SUBJECT TO THE DEVELOPER LICENSE
// AGREEMENT OR OTHER AGREEMENT THROUGH WHICH NETIQ, INC. MAKES THE WORK AVAILABLE.
// THIS WORK MAY NOT BE ADAPTED WITHOUT NETIQ'S PRIOR WRITTEN CONSENT.
// NETIQ PROVIDES THE WORK "AS IS," WITHOUT ANY EXPRESS OR IMPLIED WARRANTY,
// INCLUDING WITHOUT LIMITATION THE IMPLIED WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT. NETIQ, THE AUTHORS OF THE WORK,
// AND THE OWNERS OF COPYRIGHT IN THE WORK ARE NOT LIABLE FOR ANY CLAIM, DAMAGES,
// OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM,
// OUT OF, OR IN CONNECTION WITH THE WORK OR THE USE OR OTHER DEALINGS IN THE WORK.

//////////////////////////////////////////////////////////////////////////////////////////
// Contact the Managed Objects Server meta element and tell it to export the configuration.
// 
// NOTE: this script can be modified to operate remotely, but currently it
// assumes it is running on the Managed Objects Server.
//

function getout()
{
    formula.logout( this.session )
    java.lang.System.exit( 0 )
}

function exportConfig( filename, host, port, userid, password, dname, exportRelationships, exportedRelatedElements, propertyFilter, excludeFilter )
{
    load( 'util/login' )
    var loggedIn
    if( host != undefined && host != '' && port != undefined && port != '' && 
        userid != undefined && userid != '' && password != undefined && password != '' )
       loggedIn = login( host, port, userid, password )
    else
       loggedIn = login()
    if( loggedIn )
    {
        // Find the server element.
        var element = formula.Root.findElement( 'formulaServer=Server/root=Administration' )
        if( element )
        {
            // Create remote stream.
            var fos = new java.io.BufferedOutputStream( new java.io.FileOutputStream( filename ) )
            var os = new Packages.com.mosol.util.CORBA.RemoteOutputStreamAdapter( fos )
            var osRef = formula.util.ORB.init().object_to_string( os.getReference( 'all' ) )

            var args = [ dname, osRef, new java.io.File( filename ).getAbsolutePath() ]
            if( exportRelationships != undefined )
            {
               writeln( 'Using exportRelationships: ' + exportRelationships )
               args.push( exportRelationships )
            }
            if( exportedRelatedElements != undefined )
            {
               writeln( 'Using exportedRelatedElements: ' + exportedRelatedElements )
               args.push( exportedRelatedElements )
            }
            if( propertyFilter != undefined )
            {
               writeln( 'Using propertyFilter: ' + propertyFilter )
               args.push( propertyFilter )
            }
            
            if( excludeFilter != undefined )
            {
               writeln( 'Using excludeFilter: ' + excludeFilter )
               args.push( excludeFilter )
            }
            
            // writeln( 'Found target element: ', element )
            writeln( 'Located server, exporting configuration...' )
            var Result = element.perform( session, 'Config|ExportSilent', [], args )
            java.lang.Thread.sleep( 1000 )
            if( Result )
               writeln( '\nResult of export operation:\n\n', Result )

            // Done
            fos.close()
        }
        else
            writeln( 'Could not resolve argument: ' + who )
        getout()
    }
}

if( args.length >= 5 && args[0] == '-cold' )
{
   var root = null
   var exportRelationships = true
   var exportRelatedElements = true
   var pattern = null
   var excludePattern = null
   if( args[5] != undefined )
      root = args[5]
   if( args[6] != undefined )
      exportRelationships = args[6] == 'true' 
   if( args[7] != undefined )
      exportRelatedElements = args[7] == 'true' 
   if( args[8] != undefined )
      pattern = args[8]
   if( args[9] != undefined )
      excludePattern = args[9]  
   formula.util.exportConfig( args[1], args[2], args[3], args[4], root, exportRelationships, exportRelatedElements, pattern, excludePattern )
}
else if( args.length > 2 )
{
   exportConfig( args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9])
}
else if( args.length == 2 )
{
   exportConfig( args[1], null, null, null, null, args[0])
  
}
else
{
    writeln()
    writeln( 'NetIQ Operations Center(tm) (c) 2014, NetIQ Corporation.' )
    writeln()
    writeln( 'Usage 1:' )
    writeln( '    exportcfg -cold filename configurationName userid password dname [ exportRelationships ] [ exportedRelatedElements ] [ propertyFilter ] [ excludeFilter ]' )
    writeln()
    writeln( 'Example:' )
    writeln( '    exportcfg -cold all.config.xml MyDB admin formula root=Organizations' )
    writeln()
    writeln( 'Note: this example does not contact the server, but instead attempts to ' )
    writeln( '      connect to the named configuration for use in off-line backup while ' )
    writeln( '      the server is down.' )
    writeln()
    writeln( 'Usage 2:' )
    writeln( '    exportcfg dname filename' )
    writeln()
    writeln( 'Examples:' )
    writeln( '    exportcfg "" allData.config.xml' )
    writeln( '    exportcfg root=Organizations Organizations.config.xml' )
    writeln()
    writeln( 'Usage 3:' )
    writeln( '    exportcfg filename hostname port userid password dname [ exportRelationships ] [ exportedRelatedElements ] [ propertyFilter ] [ excludeFilter ]' )
    writeln()
    writeln( 'Example:' )
    writeln( '    exportcfg all.config.xml localhost 80 admin formula root=Organizations' )
    writeln()
    writeln( 'Note: this example exports via a running server for use in on-line backup while ' )
    writeln( '      the server is up.' )
    writeln()
}


// @internal exportcfg.fs 8ebc2jg
