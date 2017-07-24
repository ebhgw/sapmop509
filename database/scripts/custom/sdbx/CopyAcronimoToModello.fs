//@debug off
/*

 */
//
//	[Sdbx|Copy Acronimo To Modello]
//	context=element
//	description=Sdbx|Copy Acronimo To Modello
//	operation=load('custom/operation/CopyAcronimoToModello.fs');\nsession.sendMessage('Copy Acronimo To Modello completed');
//	permission=manage
//	target=dnamematch:^Acronimo=[^/]*/gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations
//	type=serverscript
//
// The operation will be called on an Acronimo and in a specific position under the tree, Sdbx<Env>/Staging/Acronimi/<acronimo>

 (function () {
 
load('custom/lib/Orgs.fs');
load('custom/orgs/TreeAligner.fs');

var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.sdbx");
_logger.info('CopyAcronimoToModello op starting');

var sdbxRootDn = element.parent.parent.parent.Dname;
var pdn = state.DNamer.parseDn(element.DName);

var modelloAcronimiDn = 'gen_folder=Acronimi/gen_folder=Modello/' + sdbxRootDn;
var stagingAcronimiDn = pdn.parent;
_logger.debug('Copying Acronimo ' + pdn.name + '\n\tfrom ' + stagingAcronimiDn + '\n\tto ' + modelloAcronimiDn);

// se esiste copia, altrimenti allinea
tgtEle = state.Orgs.findElement(pdn.fullName + '/' + modelloAcronimiDn);
if (!tgtEle) {
	_logger.debug('Target not found, copying ' + element.DName + '\n\tto ' + modelloAcronimiDn);
	Orgs.copy(element.DName, modelloAcronimiDn);
} else {
	_logger.debug('Target found, aligning source ' + element.DName + '\n\tfrom source ' + tgtEle.DName);
	// source this element, target acronimo under Modello/Acronimi
	TreeAligner.alignToSource(element,tgtEle);
}
_logger.info('CopyAcronimoToModello op completed');
})();