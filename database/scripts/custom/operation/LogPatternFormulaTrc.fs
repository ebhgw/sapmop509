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

[_Element|Pattern|Log to formula.trc]
command=
context=element
description=_Element|Pattern|Log to formula.trc
operation=load('custom/operation/LogPatternFormulaTrc.fs');
permission=view
target=dnamematch:.*
type=serverscript

*/


// main()

(function () {
	var match = element.getMatchExpression();
    var script = element.getScript();
	formula.log.info('Element ' + element.DName + '\n\rmatch: ' + match  + '\n\rscript: ' + script);
})();

