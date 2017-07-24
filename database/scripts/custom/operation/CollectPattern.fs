//	CollectPattern.fs
//
//	Created By: E. Bomitali
//	Date: 2014 Feb 12
//
//	VERSION INFORMATION
//		Date: 2014 Feb 12
//		Version: 1.0
//		Notes: Original Implementation
//
//
//	Purpose
//		
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
/*
	{
		"root": "<root dname",
		"date": "<timestamp>",
		"matches": [
			{ "dname": "<...>",
			  "match": "<...>"
			},
			...
			{ 
			  "dname": "<...>",
			  "match": "<...>"
			}
		]
	}
	
*/
/*

[_Element|Collect Pattern to file]
command=
context=element
description=_Element|Collect Pattern to file
operation=load('custom/operation/CollectPattern.fs');
permission=view
target=dnamematch:.*
type=serverscript

*/


// main()

(function () {
	load('custom/lib/MatchMngr.fs');	
	MatchMngr.dumpPatternAsJson(element);

})();

