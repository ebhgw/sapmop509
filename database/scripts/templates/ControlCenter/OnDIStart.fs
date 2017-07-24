
// Hook for Control Center BDI adapter to start BSCM job

// location of element with Control Center SCM job
var jobElemDname ="org=NetIQ%28r%29+AppManager+Control+Center/root=Organizations";
var delay = adapter.getPropertyInt("SCM Job Delay",10);

// A separate thread group for running SCM jobs, condition clear jobs, etc.
var jobThreadGroup = Packages.com.mosol.util.Util.getTopLevelThreadGroup( 'Control Center Jobs' );

// Alarm listener to trigger jobs based on status alarms from BDI
var alarmListener = new Packages.com.mosol.Formula.Server.event.AlarmListener()
{
   alarmCreated: function(evt)
   {
      var map = new Packages.com.mosol.Formula.ViewBuilder.server.action.AlarmAttrMap(evt.getAlarm());
      var desc = map.get('Description');
      if(desc.match(".*Structure_Schedule.*Run completed.*") != null)
      {
         handleStructureQueryCompleted();
      }
      else if(desc.match(".*State_Query_Schedule.*Run completed.*") != null)
      {
         handleStateQueryCompleted();
      }

   },
   alarmDeleted: function(evt)
   {
   },
   alarmsRefreshed: function(evt)
   {
   },
   alarmUpdated: function(evt)
   {
   },
   alarmOperationPerformed: function(evt)
   {
   }
}


adapter.getRootElement().addAlarmListener(alarmListener,null,false);
formula.log.info("Alarm listener added to adapter");


// Add a root element condition listener to know when the adapter stops (given the root element will be destroyed).
var rootElementConditionListener = new Packages.com.mosol.Formula.Server.event.ElementConditionListener()
{
   elementDestroyed: function(evt)
   {
      // When the root element is destroyed, start the thread to clear conditions.
      startStateClearJob()
   },
   elementConditionChanged: function(evt)
   {
      // nil
   },
   elementPropertyChanged: function(evt)
   {
      // nil
   },
}
var rootElement = adapter.getRootElement()
if ( null != rootElement )
{
   rootElement.addElementConditionListener(rootElementConditionListener)
   formula.log.info("Root element condition listener added to adapter");
}
// Initialize state vars
initializeStateVars()

function initializeStateVars()
{
   state.isRunningCCStateClears = false;
}

function handleStructureQueryCompleted() 
{
        if(isSCMJobRunning()) 
        {
         formula.log.info("Previous Control Center SCM job is still running. Skipping this run.");
         return;
        }
        startSCMJob();
}

function startSCMJob() 
{
   var runnable = new java.lang.Runnable()
   {
      run: function()
      {
         formula.log.info("Starting Control Center SCM Job in "+ delay + " seconds");
         try 
         {
            java.lang.Thread.sleep(delay * 1000);
            runSCMJob()
         } 
         catch (e)
         {
            if ( null != adapter && ( adapter.isStopped() || adapter.isStopping() ) )
            {
               if ( formula.log.isDebugEnabled() )
               {
                  if ( e instanceof java.lang.InterruptedException )
                  {
                     formula.log.debug( "Exiting sleep during stop of Control Center adapter" )
                  }
                  else
                  {
                     formula.log.debug( "Ignoring exception during stop of Control Center adapter", e )
                  }
               }
            }
            else
            {
               formula.log.error("Unexpected error starting SCM job", e);
            }
         }
      }
   }
   startDaemonThread(runnable, 'Control Center Start SCM Job');
}

function runSCMJob()
{
   var runnable = new java.lang.Runnable()
   {
      run: function()
      {
         try 
         {
            var jobElem = getBSCMRoot();
            var session = server.getAutomationSession();
            jobElem.perform(session,"ViewBuilder|Run", [], [] );
         } 
         catch (e)
         {
            formula.log.error("Unexpected error running SCM job", e);
         }
      }
   }
   // Run the SCM job in a separate thread and thread group, so that
   // stopping the BDI Control Center will not interrupt it.
   startDaemonJob(runnable, 'Control Center Run SCM Job');
}


function handleStateQueryCompleted() 
{
        if(isSCMJobRunning()) 
        {
         trace("Control Center SCM job is running. Suppressing state updates");
         return;
        }
        if(isStateUpdateJobRunning())
        {
         formula.log.info("Previous Control Center state update job is still running. Skipping this run.");
         return;
        }
        startStateUpdateJob();
}

function startStateUpdateJob()
{
   var runnable = new java.lang.Runnable()
   {
      run: function()
      {
         state.isRunningCCStateUpdates = true;
         try
         {
            trace("Processing Control Center state updates");
            Packages.com.mosol.Formula.Templates.NQCCDB.ElementConditionManager.assignConditions(getBSCMRoot(), getSeverityDataElem());
            trace("Control Center state updates completed");
         } 
         catch (e)
         {
            formula.log.error("Unexpected error performing state update", e);
         }
         finally
         {
            state.isRunningCCStateUpdates = false;
         }
      }
   }
   startDaemonThread(runnable, 'Control Center Start State Update Job');
}

function isStateUpdateJobRunning() 
{
   // state.isRunningCCStateUpdates is set and cleared in the startStateUpdateJob method.
   return state.isRunningCCStateUpdates == true;
}

function isSCMJobRunning() 
{
   // state.ccCache is set in the predefinition script and cleared at the end of the postdefnition script
   return state.ccCache && state.ccCache != null;
}

function startStateClearJob()
{
   // Skip if condition state clear is already running.
   if ( true == state.isRunningCCStateClears )
   {
      formula.log.info("Previous Control Center condition state clear is already running. Skipping run.");
   }
   else
   {
      // State clear is not running already.
      var runnable = new java.lang.Runnable()
      {
         run: function()
         {
            state.isRunningCCStateClears = true;
            try
            {
               trace("Condition state clear starting.");
               // Retry a few times, but not forever.
               for ( var retry = 0; retry < 60; retry ++ )
               {
                  if ( null != adapter && ( adapter.isStarted() || adapter.isStarting() ) )
                  {
                     // The adapter was restarted. Skip the condition state clear.
                     formula.log.info("Condition state clear skipped due to adapter restart.");
                     break;
                  }
                  else if ( isStateUpdateJobRunning() || isSCMJobRunning() )
                  {
                     // The CC state update or SCM thread is running already. Wait a bit then retry.
                     var msg = "Condition state clear waiting for other threads to end before starting."
                     if ( 0 == retry )
                     {
                        // Only log this message as info on first try.
                        formula.log.info( msg );
                     }
                     else
                     {
                        trace( msg )
                     }
                     if ( ! sleep( 5, "Exception during condition state clear waiting to start." ) )
                     {
                        break;
                     }
                  }
                  else
                  {
                     // Process the condition state clears.
                     processConditionStateClears()
                     formula.log.info("Condition state clear completed.");
                     // Done retry loop.
                     break;
                 }
              } 
           }
           finally
           {
              state.isRunningCCStateClears = false;
           }
        }
      }
      startDaemonJob(runnable, 'Control Center Start State Clear Job');
  }
}

function processConditionStateClears()
{
   try
   {
      trace("Processing Control Center condition state clears");
      Packages.com.mosol.Formula.Templates.NQCCDB.ElementConditionManager.clearConditions(getBSCMRoot());
      trace("Control Center condition state clears completed");
   }
   catch (e)
   {
      formula.log.error("Unexpected error performing condition state clear", e);
   }
}

function sleep(secs, exceptionMessage)
{
   try 
   {
      java.lang.Thread.sleep(secs * 1000);
      return true;
   }
   catch (e)
   {
      if ( formula.log.isDebugEnabled() )
      {
         formula.log.debug( exceptionMessage, e);
      }
      return false;
   }
}

// Start a thread in the same thread group as the BDI Control Center Adapter,
// which if stopped will interrupt this thread.
function startDaemonThread( runnable, name )
{
   var thread = new java.lang.Thread(runnable, name);
   startThread( thread );
}

// Start a thread in a different thread group as the BDI Control Center Adapter,
// which if stopped will not interrupt this thread.
function startDaemonJob( runnable, name )
{
   var thread = new java.lang.Thread(jobThreadGroup, runnable, name);
   startThread( thread );
}

function startThread( thread )
{
   thread.setDaemon(true);
   thread.setPriority(java.lang.Thread.NORM_PRIORITY);
   thread.start();
}

function getSeverityDataElem() 
{
   return adapter.getRootElement().findChildByKey("SeverityData=SeverityData",Packages.com.mosol.ORB.Formula.RelationKind.NAM);
}

function getBSCMRoot() 
{
   return formula.Root.findElement(jobElemDname);
}

function trace(msg)
{
   formula.log.debug(msg);
}
  
// @internal OnDIStart.fs 9f6a5k9
