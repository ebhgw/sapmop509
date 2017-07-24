//	sendDNameToClipboard.fs
//
//	Created By: E. Bomitali, based on code written by Ray Parker and Tobin Isenberg
//	Date: 2012 Jul 10
//
//	VERSION INFORMATION
//		Date: 2012 Jul 10
//		Version: 1.0
//		Notes: Original Implementation
//
//
//	Purpose
//		The purpose of this script is to give a right-click operation to
//		anyone that will collect alarm properties and write
//		it to the system clipboard.  Once in the system clipboard, it can then
//		be pasted anywhere else desired.
//
//	Notes
//		none
//
//	Required Changes
//		none
//
//	Optional Changes
//
//	Implementation
//		1) copy sendAlarmPropertiesToLog.fs to ../scripts/custom/operation/sendAlarmPropertiesToClipBoard.fs
//		2) create a right-click operation as portrayed below...
/*

*/


// main()

(function () {

var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.alarmlog");

// variable to hold the alarm information
var results = '';

var myAlarm = element.alarms[0];

results = "=== Dump of alarm: ID=" + myAlarm + "- List of Properties ===\n\n";
for (p in myAlarm.properties)
{
    results += p + ": >" + myAlarm[p] + "<\n";
}

_logger.info(results);

})();
