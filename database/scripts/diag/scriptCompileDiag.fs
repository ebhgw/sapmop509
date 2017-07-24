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

//////////////////////////////////////////////////////////////
// Utility functions for script compile diagnostics.
//////////////////////////////////////////////////////////////

// Log categories:
//
// Script compiler diagnostics log category: Diag.Script.Compiler
//    -- Default log level: INFO. Set to DEBUG for more detail.
// Script classloader diagnostics log category: Diag.Script.ClassLoader
//    -- Default log level: INFO. Set to DEBUG for more detail.
// ---------------

// Properties:
//
// Diag.Script.Compiler.Script.Truncate.Size.Debug
//    -- When script compile diagnostic is enabled, the script compiled will be logged, but truncated if
//       too long. This property is the truncation length if the "Diag.Script.Compiler" log level is set
//       to DEBUG. Default value: 2048. Does not log the script if value is 0. Set to 0 if you do not want
//       the script contents logged for any reason.
// Diag.Script.Compiler.Script.Truncate.Size.Info
//    -- When script compile diagnostic is enabled, the script compiled will be logged, but truncated if
//       too long. This property is the truncation length if the "Diag.Script.Compiler" log level is set
//       to INFO. Default value: 128. Does not log the script if value is 0. Set to 0 if you do not want
//       the script contents logged for any reason.
// ---------------

// Sample log output if compiler diagnostics is enabled and "Diag.Script.Compiler" log level is INFO:
//
// ... INFO Diag.Script.Compiler - Script context compiled a script; @opt 0 (Compiled to class); script name "Operation 'OPS|SERVER|LOG|HI3'"; result class "org.mozilla.javascript.gen.c10"; crc match count "3"; total class compiled count "6"; total class compiled ms "12" <<<Script>>>formula.log.info ( "HI" )<<</Script>>>
//
// Where:
// @opt - shows the script optimization level. 0 and higher are compiled with increasing optimization. < 0 is an interpreted script.
// script name - the name for the script.
// result class - what class the script was compiled to.
// crc match count - a CRC is taken of the script text and counts kept. Does not mean exactly the same script, but can give an estimate of the
//                   number of times a script with the same CRC value was compiled. With most scripts the CRC would be unique enough. Disabling
//                   compiler diagnostics clears the counts.
// total class compiled count - total number of script compiles. Counts both compiled to class and compiled to interepreted mode.
// total class compiled ms - accumulated time in ms for script compiles. Includes both compiled to class and compiled to intrepreted mode.
// the script
// a stack trace (if the log level is DEBUG)

// ... INFO Diag.Script.ClassLoader - Script class loader defined a class; class name "org.mozilla.javascript.gen.c19"; class byte length "2626"; result class "org.mozilla.javascript.gen.c19"; total class defined count "15"; total class defined ms "3"
//
// Where:
// class name - the name of the class loaded
// class byte length - class length in bytes
// result class - the class loaded by the classloader
// total class defined count - number of classes defined
// total class defined ms - accumulated time in ms for class definition by the class loader.
// a stack trace (if the log level is DEBUG)
//
// NOTE: The script classloader log messages usually preceed the corresponding script compile log message as the compile log
//       message is logged after the compile is completed. There may be more than one classloader log message related to a
//       script compile if multiple classes were generated as a result.
//
// ---------------

var SCRIPT_DIAG = Packages.com.mosol.Formula.Script.ScriptEcmaDiagnosticListener

// Available calls:
//
// SCRIPT_DIAG.setScriptTruncateSizeDebug( size )
// SCRIPT_DIAG.setScriptTruncateSizeInfo( size )
// SCRIPT_DIAG.setScriptCompilerLogLevel( logLevel )
// SCRIPT_DIAG.setScriptClassLoaderLogLevel( logLevel )
// SCRIPT_DIAG.enable()
// SCRIPT_DIAG.disable()

// Additional helper functions:

// Function to enable compile diagnostics.
//
// You can leave arguments off from right to left. Unspecified arguments will not be changed/set.
// Argument: scriptTruncateSizeDebug: Same as and see notes for property "Diag.Script.Compiler.Script.Truncate.Size.Debug"
// Argument: scriptTruncateSizeInfo: Same as and see notes for property "Diag.Script.Compiler.Script.Truncate.Size.Info"
// Argument: scriptCompilerLogLevel: Log level for category "Diag.Script.Compiler".
//    -- Example values: Packages.org.apache.log4j.Level.DEBUG, Packages.org.apache.log4j.Level.INFO
// Argument: scriptClassLoaderLogLevel: Log level for category "Diag.Script.ClassLoader".
//    -- Example values: Packages.org.apache.log4j.Level.DEBUG, Packages.org.apache.log4j.Level.INFO
// Example: scriptCompileDiagEnable( 2048, 128, Packages.org.apache.log4j.Level.DEBUG, Packages.org.apache.log4j.Level.DEBUG )
//
function scriptCompileDiagEnable( scriptTruncateSizeDebug, scriptTruncateSizeInfo, scriptCompilerLogLevel, scriptClassLoaderLogLevel )
{
   // Set debug log level script truncate length if given.
   if ( scriptTruncateSizeDebug && null != scriptTruncateSizeDebug )
   {
      SCRIPT_DIAG.setScriptTruncateSizeDebug( scriptTruncateSizeDebug )
   }
   // Set info log level script truncate length if given.
   if ( scriptTruncateSizeInfo && null != scriptTruncateSizeInfo )
   {
      SCRIPT_DIAG.setScriptTruncateSizeInfo( scriptTruncateSizeInfo )
   }
   // Set script compiler log level if given.
   if ( scriptCompilerLogLevel && null != scriptCompilerLogLevel )
   {
      SCRIPT_DIAG.setScriptCompilerLogLevel( scriptCompilerLogLevel )
   }
   // Set script classloader log level if given.
   if ( scriptClassLoaderLogLevel && null != scriptClassLoaderLogLevel )
   {
      SCRIPT_DIAG.setScriptClassLoaderLogLevel( scriptClassLoaderLogLevel )
   }
   // Enable script diagnostics.
   SCRIPT_DIAG.enable()
}


// Function to enable compile diagnostics.
//
function scriptCompileDiagDisable()
{
   // Disable script diagnostics.
   SCRIPT_DIAG.disable()
}

// @internal scriptCompileDiag.fs 9c6lai2
