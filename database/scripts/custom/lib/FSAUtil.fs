/*
  Script FSAUtil.fs
  
  Author: NessPRO Italy - 2009
  Tested with versions: 3.5, 4.0

  Collection of utilities for handling alarms with FormulaScript Adapter
*/


////////////////////////////////////////////////////////////////////////////////
function initializeConstants()
// Initialize all constant variables
////////////////////////////////////////////////////////////////////////////////
{
 fsautil.log = formula.log.getInstance("NessPRO.fsautil");
 fsautil.listenPortPropertyName = "EventListenPort"
 fsautil.defaultListenHost      = "localhost"
 fsautil.defaultListenPort      = 54321
}
// End of function initializeConstants


////////////////////////////////////////////////////////////////////////////////
function field(parmKey, parmValue)
// Return a key/value pair as a FSA field
// Es. field("Key", "Value") returns the string 'Key="Value";'
// _BUT_ if key="class" returns only 'Value;'
////////////////////////////////////////////////////////////////////////////////
{
  if (parmKey == "class")
     return parmValue + ";"
  else
     return parmKey + "=\"" + parmValue + "\";"
}
// End of function field

////////////////////////////////////////////////////////////////////////////////
function numericField(parmKey, parmValue)
// Return a key/value pair as a FSA field
// Es. field("Key", "Value") returns the string 'Key=Value;'
// _BUT_ if key="class" returns only 'Value;'
////////////////////////////////////////////////////////////////////////////////
{
  if (parmKey == "class")
     return parmValue + ";"
  else
     return parmKey + "=" + parmValue + ";"
}
// End of function field

////////////////////////////////////////////////////////////////////////////////
function addEnvelope(parmString, parmClass)
// Return a full formatted string ready to be sent to FSA
// parmClass is added only if present. If not, you must have added by a
// previously invoked <field("class", "something")>
////////////////////////////////////////////////////////////////////////////////
{
  var myAlarm = parmString
  var l = myAlarm.length - 1
  if (myAlarm.charAt(l) == ";")
  {
     myAlarm = myAlarm.substr(0, l)
  }

  if (parmClass != null && parmClass != "")
     myAlarm = parmClass + ";" + myAlarm

  return "### EVENT ###\n" + myAlarm + "\nEND\n### END EVENT ###\nPROCESSED\n"
}
// End of function addEnvelope


////////////////////////////////////////////////////////////////////////////////
function closeOldAlarms(parmElementDname, parmAgeInSecs, parmAlarmField, parmFlags)
// Close old FSA alarms starting from parmElementDname
// Alarms are closed only if older than parmAgeInSecs seconds
// The optional parmAlarmField is used as alarm's timestamp (default=date)
// The optional parmFlags is a bitmask used for many purposes (default=0)
// - bit 0x1   : 0=send the original alarm timestamp
//               1=do not send the original alarm timestamp, so that the
//                 current date/time is assumed
// - bit 0x10  : 0=send the original severity
//               1=send severity=OK
// - other bits: reserved for future use
////////////////////////////////////////////////////////////////////////////////
{
  if (typeof(parmAlarmField) == "undefined" || parmAlarmField == "")
     parmAlarmField = "date"
  if (typeof(parmFlags) == "undefined")
     parmFlags = 0

  var myInvokation = "closedOldAlarms('" + parmElementDname + "', " +
                                           parmAgeInSecs    + ", '" +
                                           parmAlarmField + "', " +
                                           parmFlags + ")"
  fsautil.log.debug("Invoked " + myInvokation)

  try
  {
    var myElement = formula.Root.findElement(parmElementDname)
    if (myElement == null)
       throw "Element not found"

    parmFlags = parseInt(parmFlags)
    var _flag_DoNotSendOriginalAlarmTimestamp = ((parmFlags & 0x1) != 0)
    var _flag_SendSeverityOK                  = ((parmFlags & 0x2) != 0)

    var nLoop = 0
    var nClosed = 0
    var doTheLoop = true

    var mySocket = new java.net.Socket(fsautil.defaultListenHost, getAdapterPortFromElement(myElement))
    var myStream = new java.io.DataOutputStream(mySocket.getOutputStream())

    var maxTS_msec = new Date().valueOf() - 1000 * parmAgeInSecs

    // The loop into the alarm array may be executed more than once
    // as new alarms may come into the Element while the loop is running
    while (doTheLoop)
    {
      doTheLoop = false

      // The following test is to avoid infinite loops. This may happen for two reasons:
      // 1 - Some alarms underwent a reprocessing due to a change in hierarchy definition:
      //     this is a known bug for FSA (reprocessed alarms no longer correlate, so never close)
      // 2 - New alarms are feeded too quickly so closeOldAlarms() cannot keep pace
      if (nLoop == 10)
      {
         fsautil.log.warn(myInvokation + " forcefully stopped after " + nLoop + " loops. " +
                          "This Element seems too busy at the moment, or some alarms don't want to close!")
         break
      }

      fsautil.log.debug("Beginning loop #" + nLoop + ", there are " + myElement.alarms.length + " alarms")

      // 1 - Looping backwards as the array shortens while the alarms get closed
      // 2 - The 'if (typeof() == "undefined"' are to avoid to deal with null objects
      // 3 - The Try/Catch is to detect any "out of bound" attempts
      for (i = myElement.alarms.length - 1; i >= 0; i--)
      {
          try
          {
            // The array may shorten more quickly than this loop, so I have to check
            // the index loop every time
            if (i >= myElement.alarms.length)
               i = myElement.alarms.length - 1

            var myAlarm = myElement.alarms[i]
            if (typeof(myAlarm) == "undefined")
            {
               doTheLoop = true
               continue
            }

            var myTS = myAlarm[parmAlarmField]
            if (typeof(myTS) == "undefined")
            {
               doTheLoop = true
               continue
            }

            if (myTS.valueOf() < maxTS_msec && myAlarm.status != "CLOSED")
            {
               // The Alarm is assembled from the original fields, but for the 'status' which is changed to CLOSED
               var myString = state.fsautil.field("status", "CLOSED")
               for (myPropKey in myAlarm.properties)
               {
                   myPropKey += ""
                   var myPropVal = myAlarm[myPropKey] + ""
                   switch(myPropKey)
                   {
                     case "severity":
                       myString += state.fsautil.field(myPropKey, _flag_SendSeverityOK ? "OK" : myPropVal)
                       break
                     case "status": case "Status": case "class": case "Class": case "ID": case "element": case "Date/Time": case "Description":
                       break
                     default:
                       if (!(myPropKey == parmAlarmField && _flag_DoNotSendOriginalAlarmTimestamp))
                          myString += state.fsautil.field(myPropKey, myPropVal)
                   }
               }
               myString  = addEnvelope(myString, myAlarm.Class)
               if (fsautil.log.isDebugEnabled())
                  fsautil.log.debug("Sending event (" + nLoop + "/" + i + "):\n" + myString)
               myStream.writeBytes(myString)
               nClosed++
               doTheLoop = true
            }
          }
          catch(e)
          {
            doTheLoop = true
            break
          }
      }
      nLoop++
    }

    myStream.flush();
    myStream.close();
    delete myStream;
    mySocket.close();
    delete mySocket;

    fsautil.log.info("Closed " + nClosed + " old alarms from " + parmElementDname)

    return nClosed
  }
  catch(e)
  {
    fsautil.log.error("Exception in " + myInvokation + ": " + e.toString())
    return -1
  }
}
// End of function closeOldAlarms


////////////////////////////////////////////////////////////////////////////////
function getAdapterPortFromElement(parmElement)
// Return the EventListenPort property value for the parmElement
// parmElement must be an Element Object, not a dname
// To get the Element Object from its dname, please use the standard Formula API
// formula.Root.findElement(Dname) before calling this function
////////////////////////////////////////////////////////////////////////////////
{
  try
  {
    return parseInt(parmElement.getAdapter().getProperty(fsautil.listenPortPropertyName,
                                                         fsautil.defaultListenPort))
  }
  catch(e)
  {
    fsautil.log.error("Exception in getAdapterPortFromElement('" +
                      parmElement + "'): " + e.toString())
    return fsautil.defaultListenPort
  }
}
// End of function getAdapterPortFromElement

//////////////////////////////////  M A I N  ///////////////////////////////////

var fsautil = new Object;
initializeConstants();

state.fsautil = fsautil;

state.fsautil.field                     = field
state.fsautil.numericField                     = numericField
state.fsautil.addEnvelope               = addEnvelope
state.fsautil.closeOldAlarms            = closeOldAlarms
state.fsautil.getAdapterPortFromElement = getAdapterPortFromElement
