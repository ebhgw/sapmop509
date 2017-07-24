// Sample utility script to check alarms on a tree of Service Model (group) elements and remove invalid alarms.
//
// For each group element, find any alarms where either the primary affected element can't be found or the
// primary affected element doesn't know about the alarm id. If the alarm is invalid, remove it from the group
// and send an alarm refresh event to non-group alarm listeners if any alarms were removed from the group.
//
// Usage notes:
// - Meant to find and remove orphan alarms where a primary affected element was removed or
//   no longer has an alarm with the same alarm id. Not a total alarm synchronizer.
// - Best used in a lightly or inactive system without lots of alarm activity.
// - Uses some memory overhead for the seenElements, knownGoodAlarms, and knownBadAlarms
//   collections and cloning of elementMaps and alarm sets.

// @debug off

// true: writeln debug info; false: do not writeln debug info
var debugWriteln = false

// Package shortcuts
var Group = Packages.com.mosol.Adapter.Formula.Group
var Alarmful = Packages.com.mosol.Formula.Common.Alarmful
var AlarmRefreshEvent = Packages.com.mosol.Formula.Server.event.AlarmRefreshEvent

// Reflected method from ElementImpl to get real time alarm listeners
var getRealTimeAlarmListenersMethod = null
// Reflected field in class ChannelHolder to get the channel
var channelHolderChannelField = null
// Reflected field in class Group to get the elementMap
var elementMapField = null

// -----------------------------------------------------------------
// function: fireAlarmRefreshToNonGroups
//
// Function to fire an alarm refresh event to all non-group element alarm listeners that
// are listening to the given group element. Notifies non group listeners that a groups
// alarm set was refreshed.
// TODO: Maybe no need to fire if the alarm set contents did not really change?

function fireAlarmRefreshToNonGroups( group, writelnPrefix )
{
   // Get the alarm listeners for this element
   var listeners = getRealTimeAlarmListenersMethod.invoke( group, [ "" ] )
   if ( listeners.size() > 0 )
   {
      // Collect the alarm listeners that are not group elements into a set (so de-dup them).
      var listenerSet = new java.util.HashSet()
      for ( var iter = listeners.iterator(); iter.hasNext(); )
      {
         var l = iter.next()
         // Check if the listener is not a group element and if so add it to the set.
         if ( ! ( l instanceof Group ) )
         {
            if ( debugWriteln ) writeln( writelnPrefix + '@FAR1a: ' + group.getName() + " [" + group.id() + "]; " + l )
            // Add this alarm listener as it is not a group element
            listenerSet.add( l )
         }
      }
      // If collected non group elements into the set, iterate over them
      if ( listenerSet.size() > 0 )
      {
         // Get the alarms for this group
         var alarms = group.getAlarms()
         var alarmsRefreshedEvent = new AlarmRefreshEvent( group, alarms )
         for ( var iter = listenerSet.iterator(); iter.hasNext(); )
         {
            var l = iter.next()
            if ( debugWriteln ) writeln( writelnPrefix + '@FAR2a: ' + group.getName() + " [" + group.id() + "]; " + l + "; " + alarms.length )
            // Fire the alarm refresh event on this non-group listener
            l.alarmsRefreshed( alarmsRefreshedEvent )
         }        
      }
   }
}

// -----------------------------------------------------------------
// function: checkAlarmSetsDeep
//
// Function to traverse a tree of group elements (name and linked relationship children)
// and check the alarm sets on the group element for invalid alarms. Any invalid alarms
// are removed and a refresh event is sent to non-group listeners.
// TODO: Maybe can optimize alarms into buckets of primary affected element so can reduce
//       scanning alarm sets more often?

// Keep some stats across groups for a debug writeln
var countGroupsTotal = 0
var countAlarmSetsTotal = 0
var countAlarmsTotal = 0
var countAlarmsBadTotal = 0

// Track any seen elements so don't visit them more than once.
var seenElements = new java.util.HashSet()

// Track any known good alarms so don't visit them more than once.
var knownGoodAlarms = new java.util.HashSet()

// Track any known bad alarms so don't visit them more than once.
var knownBadAlarms = new java.util.HashSet()

function checkAlarmSetsDeep( el, depth )
{
   // For debug writeln indenting
   var writelnPrefix = ""
   if ( debugWriteln ) for ( var spc = 0; spc < depth; spc ++ ) writelnPrefix += "  "
   
   // Make sure didn't see this element already
   if ( seenElements.contains( el.id() ) )
   {
      if ( debugWriteln ) writeln( writelnPrefix + "@CASD1a: " + el.getName() + ": " + el.id() )
      return // stop walking as seen this element already
   }
   if ( debugWriteln ) writeln( writelnPrefix + "@CASD1b: " + el.getName() + ": " + el.id() )
   seenElements.add( el.id() )
   countGroupsTotal ++
   
   // Traverse group children deep
   var children = el.getAllChildren( false /* no discover */ )
   if ( null != children && children.length > 0 )
   {
      for ( var i = 0; i < children.length; i ++ )
      {
         var child = children[ i ]
         // Check if this is a Group element
         if ( child instanceof Group )
         {
            // Recurse to check alarm sets deep on Group elements
            checkAlarmSetsDeep( child, depth + 1 )
         }
      }
   }
   // Did depth first traversal, now can check the groups alarm sets for invalid alarms.
   {
      // Get the element map from the group element
      var elementMap = elementMapField.get( el )
      if ( null == elementMap )
      {
         if ( debugWriteln ) writeln( writelnPrefix + "@CASD2a: " + el.getName() + ": " + el.id() )
      }
      else
      {
         // Keep some stats for this group for a debug writeln
         var countAlarmSets = 0
         var countAlarms = 0
         var countAlarmsBad = 0
         
         // Gather alarms.
         elementMap = elementMap.clone()
         // For each channel holder in the element map
         for ( var iterElementMap = elementMap.begin(); ! iterElementMap.atEnd(); iterElementMap.advance() )
         {
            var holder = iterElementMap.key()
            var channelHolderChannel = channelHolderChannelField.get( holder )
            // Check if is the real-time channel
            if ( Alarmful.AlarmUtil.match( channelHolderChannel, Alarmful.CHANNEL_REALTIME ) )
            {
               // Is the real-time channel
               countAlarmSets ++
               countAlarmSetsTotal ++
               
               // Get the alarm set from the element map
               var alarmsOrig = iterElementMap.value()
               var alarms = alarmsOrig.clone()
               // Track ServerAlarms to be deleted
               var deleteAlarms = new java.util.ArrayList()

               // For each ServerAlarm in the alarm set            
               for ( var enumer = alarms.elements(); enumer.hasMoreElements(); )
               {
                  countAlarms ++
                  countAlarmsTotal ++
                  var serverAlarm = enumer.nextElement()
                  var alarmUniqueId = serverAlarm.getId() + ":" + serverAlarm.getElement()

                  // If we know this alarm is good, can skip it.
                  if ( ! knownGoodAlarms.contains( alarmUniqueId ) )
                  {
                     var alarmOk = false
                     
                     // If we know this alarm is bad, can remove it without checking again.
                     if ( ! knownBadAlarms.contains( alarmUniqueId ) )
                     {
                        // Haven't checked validity of this alarm yet.
                        var alarm = serverAlarm.getAlarm( false )
                  
                        // Check if this alarm appears to be valid (primary affected element
                        // is resolvable and primary affected element knows of the alarm id).
                        try
                        {
                           // Check the alarm
                           var id = alarm.id
                           var primaryAffectedElement = alarm.element
                           if ( null != primaryAffectedElement )
                           {
                              // We get all alarms here and scan for the one we want. This avoids some adapters
                              // that for getAlarms( int [] ) will return the alarm if it is anywhere in the
                              // adapter and not necessarily with the primary affected element.
                              var elementAlarms = primaryAffectedElement.getAlarms()
                              if ( null != elementAlarms && elementAlarms.length > 0 )
                              {
                                 // Scan alarms until we find the one we want
                                 for ( var scan = 0; scan < elementAlarms.length; scan ++ )
                                 {
                                    var elementAlarm = elementAlarms[ scan ]
                                    if ( null != elementAlarm && elementAlarm.id == id && elementAlarm.element == primaryAffectedElement )
                                    {
                                       // This alarm seems ok as has an alarm of the given alarm id and primary affected element
                                       alarmOk = true;
                                       break;
                                    }
                                 }
                              }
                           }
                        }
                        catch( e ) { }
                     }
                     if ( alarmOk )
                     {
                        // We now know this alarm is good, so no need to check it again.
                        knownGoodAlarms.add( alarmUniqueId )
                     }
                     else
                     {
                        // This alarm is invalid and needs to be removed.
                        countAlarmsBad ++
                        countAlarmsBadTotal ++
                        deleteAlarms.add( serverAlarm )
                        // We now know this alarm is bad, so no need to check it again.
                        knownBadAlarms.add( alarmUniqueId )
                     }
                  }
               }
               // Check if there are ServerAlarms to be removed from the alarm set.
               if ( deleteAlarms.size() > 0 )
               {
                  // Iterate over each ServerAlarm to be removed.
                  for ( var iterDeleteAlarms = deleteAlarms.iterator(); iterDeleteAlarms.hasNext(); )
                  {
                     var serverAlarm = iterDeleteAlarms.next()
                     // Remove the alarm from the alarm set
                     alarmsOrig.remove( serverAlarm )
                  }
               }
            }
         }
         // Check if need to fire an alarm refresh for this group.
         if ( countAlarmsBad > 0 )
         {
            // Some alarms were removed from this group, so fire alarm refresh to non-group listeners.
            fireAlarmRefreshToNonGroups( el, writelnPrefix )
         }
         if ( debugWriteln ) writeln( writelnPrefix + "@CASD3a; " + el.getName() + ": " + el.id() + ": Finished checking alarms; Removed: " + countAlarmsBad + " alarms; total alarms scanned across " + countAlarmSets + " alarm set(s): " + countAlarms )
      }
   }
}
      
// -----------------------------------------------------------------
// Non-function inline code

if ( ! ( element instanceof Group ) ||
     ! ( element.getDName().endsWith( "/root=Organizations" ) ) )
{
   session.sendMessage( "Operation must be performed on a Service Model element." )
}
else
{
   // Reflect to get non-public methods and fields
   getRealTimeAlarmListenersMethod = java.lang.Class.forName( "com.mosol.Formula.Server.ElementImpl" ).getDeclaredMethod( "getAlarmListeners", [ java.lang.Class.forName( "java.lang.String" ) ] )
   channelHolderChannelField = java.lang.Class.forName( "com.mosol.Adapter.Formula.Group$ChannelHolder" ).getDeclaredField( "channel" )
   elementMapField = java.lang.Class.forName( "com.mosol.Adapter.Formula.Group" ).getDeclaredField( "elementMap" )
   
   // Check reflected the methods and fields ok
   if ( null == getRealTimeAlarmListenersMethod )
   {
      session.sendMessage( "Could not determine class ElementImpl getAlarmListeners( String ) method." )
   }
   else if ( null == channelHolderChannelField )
   {
      session.sendMessage( "Could not determine class ChannelHolder channel field." )
   }
   else if ( null == elementMapField )
   {
      session.sendMessage( "Could not determine class Group elementMap field." )
   }
   else
   {
      // Set the reflected methods and fields as accessible.
      getRealTimeAlarmListenersMethod.setAccessible( true )
      channelHolderChannelField.setAccessible( true )
      elementMapField.setAccessible( true )
      
      // Perform the group element alarms set check deep.
      checkAlarmSetsDeep( element, 0 )

      var msg = "Finished checking alarms deep; Removed " + countAlarmsBadTotal + " alarms; total alarms scanned across " + countGroupsTotal + " group(s) and " + countAlarmSetsTotal + " alarm set(s): " + countAlarmsTotal + ".\nNumber of known good alarms: " + knownGoodAlarms.size() + "; number of known bad alarms: " + knownBadAlarms.size() + "."
      if ( debugWriteln ) writeln( "@IC1a; " + element.getName() + ": " + element.id() + ": " + msg )
      session.sendMessage( msg )
   }
}

// Clean up used vars

getRealTimeAlarmListenersMethod = null
channelHolderChannelField = null
elementMapField = null
seenElements.clear()
knownGoodAlarms.clear()
knownBadAlarms.clear()
seenElements = null
knownGoodAlarms = null
knownBadAlarms = null


// @internal forceGroupRemoveOrphanAlarmsDeep.fs -d397iad
