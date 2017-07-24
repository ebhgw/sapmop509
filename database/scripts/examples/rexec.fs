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
//      rexec.fs
//      Created By: Tobin Isenberg & Kurt Westerfeld
//                  Managed Objects
//      Date: 9/6/2001
//
//      Version 3.0     (Tested on 3.1)
//
//      Notes: re-implemented for Managed Objects v3.1 1/16/03
//
//
//      Purpose
//              The purpose of this script is to allow an operation to bounce back and forth
//              between being a clientscript and a serverscript.  In some cases you may want
//              to have a script that prompts the user for some parameters and then start
//              a script on the server with the parameter.  Or another case would be that you
//              need to prompt the user for some information then use jdbc drivers on the server
//              to update a database.  In both of these cases, the script needs to be able to
//              run in both contexts.  Additionally, the purpose of this script is for reference
//              when implementing customization within your environment.
//
//      Notes
//              This script starts out as a server script, invokes a script on the client and sends
//              it some variables, the client script prompts the user for input and then returns
//              the user inputted variables back to the server script.  The server script then
//              executes the cmd /c dir (+parms entered), grabs the output of the stdout and stderr
//              and then sends the results back to the client again.
//
//      Additional Notes
//              This can be changed to run other commandline utilities, hit databases, or
//              other processing types of tasks.
//
//      Limitations
//              Very large results are not appropriate for an "info" box and may not be transported
//              back and forth betweent the client and server properly.  You may see Marshalling errors
//              if the results of the process are too large or contain abnormal ascii data.
//
//      Required Changes
//              If the Managed Objects Server is on NT, there are no required changes, if not the fragment
//              below will need to be changed to "ls " instead.
//
//                      (next line uncommented out on purpose!)
                        var myParm = "cmd /c dir ";
//
//      Optional Changes
//              The line above can be changed to run just about anything.  If you change it to
//              c:/winnt/system32/eventvwr.exe it will launch the Event Viewer on an Managed Objects NT
//              server, it will not be able to return execution results since eventvwr.exe is
//              a windows based application.
//
//      Implementation
//              1) Within Managed Objects v4 go to Administration, Server, Operation Definition
//              2) right-click on Operation Definition and choose "Create Operation"
//              3) Use the following settings...
//                      Name: rexec
//                      Menu Text: rexec
//                      Context: Element
//                      Match by: Distinguished name (leave box below empty)
//                      Permission: Define
//                      Type: Server script
//                      Operation: @examples/rexec
//              4) right-click on an element and choose "rexec"
//
//              The operations.ini fragment should appear like this:
//
//                      [rexec]
//                      description=rexec
//                      target=dname:
//                      context=element
//                      permission=define
//                      type=serverscript
//                      operation=@examples/rexec
//                      command=
//
//
//      Expected Results
//              The rexec.fs should start on the server.  It will take the contents of "myParm"
//              and send it to the client and launch a client script, which it formats dynamically.
//              The client script will launch on the originating session and ask the user for
//              parameters to be passed to the "dir" command.
//
//              The "myParm" and whatever the user enters will be packaged together and sent
//              back to the rexec.fs running on the server and re-enter within the "callback"
//              section of the code.  rexec.fs will then execute the command requested, grab
//              the output if any and then issue an "info" dialog box back at the client with
//              the results of the command that was run.
//
//

// this function gets the stdout and stderr on the running process
function getStreamContent( stream, name )
{
    var Result

    if( stream )
    {
        var bytes = formula.util.toByteArray( stream );
        Result = new java.lang.String( bytes ) ;
        stream.close()

    }
    else
    {
        Result = "Could not open " + name;
    }

    return Result
}

// This is the re-entry point from the client script
var callback =
{
   rexec: function( ExecAtServerCommand )
   {
      // try and run command
      var process;
      try
      {
         // execute the process
         process = java.lang.Runtime.getRuntime().exec( ExecAtServerCommand );
      }
      catch( Exception )
      {
         formula.log.error( "Error executing: " + ExecAtServerCommand + " because of Exception", Exception );
      }

      var completionResult = "Operation Performed";
      if( ! process )
      {
         completionResult = "Failed to start process";
      }
      else
      {
         // grab stdout
         var output = getStreamContent( process.getInputStream(), 'standard output' );

         // grab stderr
         var error = getStreamContent( process.getErrorStream(), 'standard error' );

         completionResult = output;

         // if stderr has something add it to the results
         if( error && error != '' )
            completionResult += '\n\nErrors: ' + error;
      }

      // build a results string and show it to the user

      // send results to the client
      session.invokeScript( 'Display Result', "info( theResults )", [completionResult], ['theResults'] );
   },

   cancel: function()
   {
      session.sendMessage( 'You cancelled the remote execute command' )
   }
}

// We're going to send this callback and a prompt script to the client, which will
// then cause execution to this script through remote proxy.

var rexecClientScript = "\
    // @opt -1 \
    // @debug off \
    var result = prompt( 'Enter parameters for ' + myParm + ':', \
                         'Execute Remote Command (leave blank to cancel operation)' ) \
    if( ! result ) \
        callback.cancel() \
    else \
        callback.rexec( myParm + ' ' + result )\
"

// invoke the client script and pass parm array
session.invokeScript( 'Remote Execute Client Script',
                      rexecClientScript,
                      [ formula.util.makeRemote( callback ), myParm ],
                      [ 'callback', 'myParm' ] )

// EOF() rexec.fs
