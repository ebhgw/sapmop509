/*

 Script OrgUtilv2.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0
 
 [_Element|Remove non contrib mark]
command=
context=element
description=_Element|Remove non contrib mark
operation=load('custom/operation/RemoveNonContributing.fs');
permission=manage
target=namematch:.*
type=serverscript

 */
 
 (function () {
 load('custom/lib/Orgs.Util.fs');
 
 Orgs.Util.removeNonContributingMark(element);
 
 })();