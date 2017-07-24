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
// A utility to create a remote container server instance.
//


var formulaHome = java.lang.System.getProperty('formula.home')

///////////////////////////////////////////////////////////////////////////////////
// Class aliases
///////////////////////////////////////////////////////////////////////////////////
com = Packages.com
java = Packages.java
File = Packages.java.io.File
Util = Packages.com.mosol.util.Util


///////////////////////////////////////////////////////////////////////////////////
// Identification
///////////////////////////////////////////////////////////////////////////////////

writeln( "" )
writeln( "Remote Container Initialization Utility - Managed Objects v4(r) Copyright (c) 2014 NetIQ Corporation. All Rights Reserved." )
writeln( "" )

///////////////////////////////////////////////////////////////////////////////////
// Main
///////////////////////////////////////////////////////////////////////////////////
try
{
   if (formulaHome == null)
   { 
      throw new java.lang.Exception("Missing Property 'formula.home'")
   }

   var container = null

   if (args.length < 1 || args[0] == null)
   {
      throw new java.lang.Exception("Missing Remote Container Name")
   }

   container = new java.lang.String( args[0] ).trim()
   if ( ! container.startsWith( "Container" ) )
   {
      container = "Container" + container
   }

   writeln( "Initializing " + container + " ..." )
   writeln( "" )

   // Create ini file
   var iniDir = new File( formulaHome + File.separator + "config" + File.separator + "template" + File.separator )
   var iniTemplate = new File( iniDir, "DefaultContainer.ini" )
   var iniContainer = new File( iniDir, container + ".ini" )

   if ( iniContainer.exists() )
   {
      writeln( "Remote Container ini file already exists; skipping ini file creation : " + iniContainer )
      writeln( "" )
   }
   else
   {
      if ( ! iniTemplate.exists() )
      {
         throw new java.lang.Exception( "Remote Container ini template file was not found for ini file : " + iniContainer + "; template file : " + iniTemplate + " was missing" )
      }

      Util.copyStream( new java.io.FileInputStream( iniTemplate ), new java.io.FileOutputStream( iniContainer ) );

      if ( ! iniContainer.exists() )
      {
         throw new java.lang.Exception( "Could not create ini file for Remote Container : " + iniContainer )
      }

      writeln( "A new ini file was created for the Remote Container so that the Remote Container" )
      writeln( "can be managed by the Managed Objects daemon : " + iniContainer )
      writeln( "" )
      writeln( "The next time the Managed Objects Customizer is run, the Remote Container's" )
      writeln( "ini file above will be copied into the daemon.ini file.  The mosstart and" )
      writeln( "mosstop commands will then be available to start and stop the Remote Container" )
      writeln( "(i.e. mosstart " + container + ", mosstatus " + container + ", mosstop " + container + ", etc)." )
      writeln( "" )

   }

   writeln( "Remote Container initialization completed" )
   writeln( "" )   

}
catch (Exception) {
   writeln( "" )
   writeln( 'ERROR: ', Exception)
   writeln( "" )
}

