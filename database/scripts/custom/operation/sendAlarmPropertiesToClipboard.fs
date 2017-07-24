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
//		1) copy sendAlarmPropertiesToClipboard.fs to ../scripts/custom/operation/sendAlarmPropertiesToClipBoard.fs
//		2) create a right-click operation as portrayed below...
/*

 [_Alarm|Copy Properties to Clipboard]
 command=
 context=alarm
 description=_Alarm|Copy Properties to Clipboard
 operation=load('custom/operation/sendAlarmPropertiesToClipboard.fs');
 permission=view
 target=dnamematch:.*
 type=clientscript

*/


// main()

// user
	var getUserFromDname = function (parUser) {
		var patt = /user=(\w*)\/.*/
		var res = patt.exec(parUser);
		if (res == null) {
			return 'user not found';
		} else {
			return res[1];
		}
	}

var currUser = getUserFromDname(user.DName);

// variable to hold the alarm information
var results = '';

var myAlarm = alarms[0];

results = "=== Dump of alarm: ID=" + myAlarm + "- List of Properties ===\n";
results = results + '    Taken from' + currUser + ' at ' + new Date() + '\n';
for (p in myAlarm.properties)
{
    results += p + ": >" + myAlarm[p] + "<\n";
}

// Here is the magic of dumping the data into the clipboard
var selection = new java.awt.datatransfer.StringSelection( results );
var clip = java.awt.datatransfer.Clipboard;
clip = java.awt.Toolkit.getDefaultToolkit().getSystemClipboard();
clip.setContents( selection, null );

// eof sendDNameToClipboard.fs
