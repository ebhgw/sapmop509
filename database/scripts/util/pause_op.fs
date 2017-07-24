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
// [Start Alarm Pause Check Operation]
// description=Alarm Pause Checker|Start
// context=Enterprise
// target=name:Enterprise
// permission=define
// type=clientscript
// operation=// @debug off \nload( "util/pause_op.fs" );
//
// [Stop Alarm Pause Check Operation]
// description=Alarm Pause Checker|Stop
// context=Enterprise
// target=name:Enterprise
// permission=define
// type=clientscript
// operation=// @debug off \nload( "util/pauseStop_op.fs" );
//
/////////////////////////////////////////////////////////////////////////////

// Startup parameters
var nMinutesToWait = 8;   // Time interval between checks
//var MINUTES = 1000 * 60;
var MINUTES = 1000;

alert("Starting Pause Operation...")
// Log output.
formula.logCategory = "AlarmPauseCheck"

Thread = Packages.java.lang.Thread;


var FOREVER = java.lang.Long.MAX_VALUE;
var alarmPauseTimer = null;
function startAlarmPauseTimer(evt, nMinutesToWait) {
   // kill any previous timer
   killAlarmPauseTimer();
   // now start a new one...
   var alarmPauseTimer = new JavaAdapter( Thread, {
       run : function() {
          try {
             while (true) {
                Thread.sleep(nMinutesToWait * MINUTES);
                if (evt != null) {
                   if (evt.getSource().isPaused())
                      alert("Alarm view is currently in pause mode.")
                   else {
                      delete state.alarmPauseTimer
                      break; // we need to stop
                   }
                }
                else {
                   if (appFrame.isAlarmPanelPaused())
                      alert("Alarm view is currently in pause mode.")
                   else {
                      delete state.alarmPauseTimer
                      break; // we need to stop
                   }
                }
             }
          }
          catch (exception) {
            //ignore
          }
       }
    } )
    state.alarmPauseTimer = alarmPauseTimer
    alarmPauseTimer.start();
}

function killAlarmPauseTimer() {
   if( state.alarmPauseTimer ) {
      try {
         state.alarmPauseTimer.interrupt();
      }
      catch (ignore) {
      }
      delete state.alarmPauseTimer
   }
}



// Setup the state check.
if( state.pauseop )
   throw "Alarm Pause Checker already active"
state.pauseop = 1

formula.log.info( "Starting Alarm Pause Checker..." );

var ThisPointer = this
// set up pause listener
var pauseMonitor = new java.beans.PropertyChangeSupport( this );
// Start our monitor listener.
var listener = new java.beans.PropertyChangeListener() {
   propertyChange : function( evt ) {
      if ( evt.getNewValue().equalsIgnoreCase("AlarmsPaused") ) {
         // start the timer
         ThisPointer.startAlarmPauseTimer(evt, nMinutesToWait);
      }
      else if ( evt.getNewValue().equalsIgnoreCase("AlarmsResumed") ) {
         // stop the timer
         ThisPointer.killAlarmPauseTimer();
      }                                                   
   }
}
pauseMonitor.addPropertyChangeListener( listener )
state.pauseMonitor = pauseMonitor

appFrame.listenForAlarmPause(pauseMonitor)

// first check the immediate state to see if paused and start a timer if so...
if (appFrame.isAlarmPanelPaused()) {
   startAlarmPauseTimer(null, nMinutesToWait);
}

// sleep for ever
java.lang.Thread.sleep( FOREVER )

// oops...must be time to die...
appFrame.removeAlarmPauseMonitor()
delete state.alarmPauseTimer
delete state.pauseMonitor
delete state.pauseop


formula.log.info( "Alarm Pause Checker Operation stopped." )
// @internal pause_op.fs 417c2g2
