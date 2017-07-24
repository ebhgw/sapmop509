//	sendElementPropertiesToClipboard.fs
//
//	VERSION INFORMATION
//		Version: 1.01
//
//		2005May03 - 1.00 Created by The Unknown Scripter
//		2006Sep07 - 1.01 Comments and cleanup - Ray Parker
//
//
//	Purpose
//		The purpose of this script is to give a right-click operation to
//		anyone that will extract the properties of the selected object and write
//		them to the system clipboard.  Once in the system clipboard, they can then
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
//		1) copy sendElementPropertiesToClipboard.fs to formula/database/scripts/custom/sendElementPropertiesToClipboard.fs
//		2) create a right-click operation as portrayed below...
//
//			Name: Send Element Properties To Clipboard
//			Menu text: Send Element Properties To Clipboard
//			Context: Object
//			Match by: Name match
//					: .*
//			Permission: View (or higher if you wish)
//			Type: Client script
//			Operation: @custom/sendElementPropertiesToClipboard.fs
//
//

// Get a Carriage Return/LineFeed into a variable for later use
var cr = new java.lang.String( new java.lang.Character( 0x0d ) ) + java.lang.String( new java.lang.Character( 0x0a ) );

var results = "Properties for element " + element.name + cr + cr;
results += "DName: " + element.dname + cr;
results += Date() + cr + cr + cr;

for( p in element.properties )
{
	results += p + cr;
	results += ">>>" + element[p] + "<<<" + cr + cr;
}	

var selection = new java.awt.datatransfer.StringSelection( results );
var clip = java.awt.datatransfer.Clipboard;
clip = java.awt.Toolkit.getDefaultToolkit().getSystemClipboard();
clip.setContents( selection, null );

// eof sendElementPropertiesToClipboard.fs