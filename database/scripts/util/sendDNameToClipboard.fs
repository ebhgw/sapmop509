//	sendDNameToClipboard.fs
//
//	Created By: Ray Parker, based on code written by Tobin Isenberg
//	Date: 2002 Dec 10
//
//	VERSION INFORMATION
//		Date: 2002 Dec 10
//		Version: 1.0
//		Notes: Original Implementation
//
//
//	Purpose
//		The purpose of this script is to give a right-click operation to
//		anyone that will send the DName of the selected object and write
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
//		1) copy sendDNameToClipboard.fs to formula/database/scripts/util/sendDNameToClipBoard.fs
//		2) create a right-click operation as portrayed below...
//
//			Name: Send DName To Clipboard
//			Menu text: Send DName To Clipboard
//			Context: Object
//			Match by: Name match
//					: .*
//			Permission: View (or higher if you wish)
//			Type: Client script
//			Operation: load( "util/sendDNameToClipboard.fs" );
//
//

// main()



// variable to hold the alarm information
var results = ' ';

results = element.getDName()

// Here is the magic of dumping the data into the clipboard
var selection = new java.awt.datatransfer.StringSelection( results );
var clip = java.awt.datatransfer.Clipboard;
clip = java.awt.Toolkit.getDefaultToolkit().getSystemClipboard();
clip.setContents( selection, null );

// eof sendDNameToClipboard.fs
