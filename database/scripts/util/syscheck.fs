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

/////////////////////////////////////////////////////////////////////////////
// Operation definitions to add to administration element:
//
// [Start System Checker]
// description=System Checker|Start
// context=element
// target=name:Administration
// permission=define
// type=serverscript
// operation=// @debug off \nload( "util/syscheck.fs" );
//
// [Stop System Checker]
// description=System Checker|Stop
// context=element
// target=name:Administration
// permission=define
// type=serverscript
// operation=// @debug off \nload( "util/syscheckstop.fs" );
//
/////////////////////////////////////////////////////////////////////////////

// Startup parameters
var nSecondsToWait = 60;        // Time interval between checks
var fDoGC = false               // Do synchronous garbage collection
var fDoSessions = false         // Do session output
var fDoGCPauseWarnings = true   // Do warnings on gc pause

// Log output.
formula.logCategory = "SysCheck"

// Some convenient shortcuts.
var thisRunTime = java.lang.Runtime.getRuntime();
var UString = formula.util.UString
var numberFormatter = java.text.NumberFormat.getInstance()
numberFormatter.applyPattern( '#,##0.0' )

// Memory check globals.
var nMemoryAtStart = -1;
var nMemoryLast = -1;
var nMemoryCurrent = -1;
var nMemoryMax = -1;
var nMemoryMin = -1;
var nMemoryTotal = -1;
var nMemoryTotalLast = -1;
var nMemoryTotalStart = -1;
var nCount = 0;
var nMaximum = 0;
function checkMemory()
{
    nCount++ ;
   
   // dump memory stats

   nMemoryCurrent = thisRunTime.freeMemory();
   nMemoryTotal = thisRunTime.totalMemory();

   if ( nMemoryAtStart == -1 )
   {
      nMemoryTotalStart = nMemoryTotal;
      nMemoryTotalLast = nMemoryTotalStart;
      nMemoryAtStart = nMemoryCurrent;
      nMemoryLast = nMemoryAtStart;
      nMemoryMax = nMemoryAtStart;
      nMemoryMin = nMemoryAtStart;
   }

    if ( fDoGC && nCount == 10 )
   {
      nCount = 0 ;
        formula.log.info("Collecting garbage..." );
      java.lang.System.gc() ;
      java.lang.System.runFinalization() ;

   }
   
   if ( nMemoryCurrent > nMemoryMax )
   {
      nMemoryMax = nMemoryCurrent;
   

        formula.log.info("New Maximum Free Memory: " + nMemoryMax );
   }
   else if ( nMemoryCurrent < nMemoryMin )
   {
      nMemoryMin = nMemoryCurrent;
   

        formula.log.info("New Minimum Free Memory: " + nMemoryMin );
   }

   if ( nMemoryTotal != nMemoryTotalLast )
   {
        formula.log.info("Available memory changed was " +
         nMemoryTotalLast +
         ", now " +
         nMemoryTotal +
         " (Change=" +
         (nMemoryTotal-nMemoryTotalLast) +
         ")" );

      nMemoryTotalLast = nMemoryTotal;
   }

   // Determine max memory setting
   if( nMaximum == 0 )
   {
      nMaximum = '(unknown)'
      if( thisRunTime.maxMemory )
         nMaximum = numberFormatter.format( thisRunTime.maxMemory() / 1024 )
   }

   // write the string
    var elements = 'n/a'
    var elementClasses = 'n/a'
    var sessions = 'n/a'
    var automations = 'n/a'
    var recalcCount = 'n/a'
    var recalcTotalTime = 'n/a'
    var recalcAvg = 'n/a'
    var recalcHighest = 'n/a'
    var queueSize = 'n/a'
    if( this.server )
    {
       var serverMetaElement = formula.Root.findElement( 'formulaServer=Server/root=Administration' )
       elements = serverMetaElement[ 'Total Elements' ]
       elementClasses = serverMetaElement[ 'Total Element Classes' ]
       sessions = serverMetaElement[ 'Sessions' ]
       automations = serverMetaElement[ 'Total Automation Events' ]
       try
       {
          recalcCount = formula.Elements.adapter.recalcEngine.getRecalcCount()
          recalcTotalTime = formula.Elements.adapter.recalcEngine.getRecalcTotalTime()
          recalcAvg = parseInt( recalcTotalTime / recalcCount )
          recalcHighest = formula.Elements.adapter.recalcEngine.getRecalcHighest()
       }
       catch( ignore ) {}
       try
       {
          var warehouseMetaElement = formula.Root.findElement( 'dataWarehouse=Data+Warehouse/root=Administration' )
          queueSize = warehouseMetaElement[ 'Queue Size' ]
       }
       catch( ignore ) {}
    }
    formula.log.info( "Server Statistics:"+
          "\n > Sessions: " +
          sessions +
          "\n > Elements: " +
          elements +
          "\n > Element Classes: " +
          elementClasses +
          "\n > Automation Events Queued: " +
          automations +
          "\n > Total calculations: " +
          recalcCount +
          "\n > Total calulation time: " +
          recalcTotalTime + 'ms ' +
          "\n > Average calculation time: " +
          recalcAvg + 'ms ' +
          "\n > Highest calculation time: " +
          recalcHighest + 'ms ' )

    formula.log.info( "Memory Statistics:\n"+
        " > Maximum: " +
        nMaximum +
        " KB\n > Total:   " +
        numberFormatter.format( nMemoryTotal / 1024 ) +
        " KB\n > Used:    " +
        numberFormatter.format( ( nMemoryTotal - nMemoryCurrent ) / 1024 ) +
        " KB\n > Free:    " +
        numberFormatter.format( nMemoryCurrent / 1024 ) +
        " KB\n > Diff:    " +
        numberFormatter.format( ( nMemoryCurrent - nMemoryLast ) / 1024 ) +
        " KB\n > Overall: " +
        numberFormatter.format( ( nMemoryCurrent - nMemoryAtStart ) / 1024 ) +
        " KB/" + numberFormatter.format( ( nMemoryTotal - nMemoryTotalStart ) / 1024 ) + ' KB' );

    formula.log.info( "Warehouse Statistics:\n"+
        " > Queue Size: " +
        queueSize )

   // set last to this current for next loop
   nMemoryLast = nMemoryCurrent;
}

function checkUString()
{
   formula.log.info( "UString Statistics:\n" +
                           " > " + UString.getCalls() + " calls\n" +
                           " > " + UString.getHits() + " hits\n" +
                           " > " + UString.getStrings() + " strings\n" +
                           " > " + UString.getMisses() + " misses\n" +
                           " > " + UString.getBytes() + " bytes" )
}

function checkSessions()
{
   var sessions= server.getSessions()
   formula.log.info( "Session Statistics" )
   for( var i = 0; i < sessions.length; ++i )
      try
      {
         sessions[i].dumpState( formula.log )
      }
      catch( Exception )
      {
         formula.log.info( "Session " + i + ", " + sessions[i] + " could not get details: " + Exception )
      }
}

// Setup the state check.
if( state.syscheck )
   throw "System checker already active"
state.syscheck = 1

formula.log.info( "System checker starts..." );
while( state.syscheck )
{
    // Do checks
    checkMemory()
    checkUString()
    if( fDoSessions )
       checkSessions()

    // sleep for period; we sample to see if gc kicks us into a long wait.
    var lasttime = java.lang.System.currentTimeMillis()
    for( var i = 0; i < nSecondsToWait; ++i )
    {
       java.lang.Thread.sleep( 1000 )
       var thistime = java.lang.System.currentTimeMillis()
       if( fDoGCPauseWarnings && ( thistime - lasttime ) > 3000 )
       {
          formula.log.warn( 'System checker was suspended; suspect garbage collector (GC) is taking too long' )
          formula.log.warn( 'Pause took ' + ( ( thistime - lasttime ) / 1000 ) + ' seconds' )
       }
       lasttime = thistime
    }
}

delete state.syscheck

formula.log.info( "System checker stopped." )

// @internal syscheck.fs -a3g9c37
