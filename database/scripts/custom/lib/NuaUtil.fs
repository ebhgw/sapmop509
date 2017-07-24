/*
  Script NuaUtil.fs
  
  Author: E. Bomitali - Hogwart
  
  Collection of utilities for handling alarms with NOC Universal Adapter
  A valid message has the following structure
  
### EVENT ###
Database_Monitor;
hostname=”server45.mosol.com”;
process=”oracle”;
instance=”ORCL”;
application=”people soft”;
testing_id=”12345”;
originating_event_id=”34532”;
msg=”Instance is no longer active, tnsping failed”;
severity=”CRITICAL”;
END
### END EVENT ###
PROCESSED

Starting with the Header "### EVENT ###", then the class and a list of pairs (property name, values), eventually the footer (END to PROCESSED)
  
*/

function NuaUtil () {
}

// Helper to format messagges to FSA adapter.

/* Return a key/value pair according to NUA required formatting
   Es. field("Key", "Value") returns the string 'Key="Value";'
   BUT if key="class" returns only 'Value;'
   as we should define a class for the message.
*/
NuaUtil.prepField = function(parmKey, parmValue)
{
  if (parmKey == "class")
     return parmValue + ";\r\n"
  else
     return parmKey + "=\"" + parmValue + "\";\r\n"
}

/* Return a key/value pair according to NUA required formatting
   Being numeric we do not put quotes
   Es. field("Key", Value) returns the string 'Key=Value;'
   BUT if key="class" returns only 'Value;'
   as we should define a class for the message.
*/
NuaUtil.prepNumericField = function(parmKey, parmValue)
{
  if (parmKey == "class")
     return parmValue + ";\r\n"
  else
     return parmKey + "=" + parmValue + ";\r\n"
}

// Given an object, loop on user properties/values and returns a prep string
NuaUtil.prepObject = function (pObj) {

	var res = ""
	for(var name in pObj) {
		//writeln('name is ' + name);
		res = res + NuaUtil.prepField(name, pObj[name]);
	}
	return res;
}

/* 
Return a fully formatted string ready to be sent to Nua (header, fields, footer)
parmClass is added only if present. If not, you must have added by a
previously invoked <field("class", "something")>
*/
NuaUtil.addEnvelope = function(parmString, parmClass)
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


