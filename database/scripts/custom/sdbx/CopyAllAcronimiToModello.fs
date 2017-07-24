//@debug off
/*
	If element does not exists, copy, ow add new elements
 */
//
//	[Sdbx|Copy All Acronimi To Modello]
//	context=element
//	description=Sdbx|Copy All Acronimi To Modello
//	operation=load('custom/operation/CopyAllAcronimiToModello.fs');\nsession.sendMessage('Copy All Acronimi To Modell completed');
//	permission=manage
//	target=dnamematch:^gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations
//	type=serverscript
//

(function () {
load('custom/orgs/TreeAligner.fs');
load('custom/lib/underscore.js');
load('custom/lib/Orgs.fs');

var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.sdbx");
_logger.info('CopyAllAcronimiToModello starting ' + 'using sdbx ' + element.parent.parent.DName);

// called by an operation with match on gen_folder=Acronimi element
var stagingAcronimiDn = element.DName;
var sdbxRootDn = element.parent.parent.Dname;
var pdn = state.DNamer.parseDn(stagingAcronimiDn);

var modelloAcronimiDn = 'gen_folder=Acronimi/gen_folder=Modello/' + sdbxRootDn;
var modelloServiziDn = 'gen_folder=Acronimi/gen_folder=Modello/' + sdbxRootDn;
var stagingAcronimiDn = element.DName;
_logger.debug('Copying Acronimi\n\tfrom ' + stagingAcronimiDn + '\n\tto ' + modelloAcronimiDn);

var acronimiEle = element
// use children as Children does not cope with // elements
var acroList = element.children;

// copy every single acronimo to preserve modification on modello
_.each(acroList, function (acroEle) {
	var tgtDn = state.DNamer.parseDn(acroEle.DName).fullName + '/' + modelloAcronimiDn;
	var tgtEle = state.Orgs.findElement(tgtDn);
	_logger.debug('Looping on ' + acroEle.DName);
	
	if (tgtEle) {
		_logger.debug('Target found, aligning source ' + acroEle + '\n\tfrom source ' + tgtDn);
		TreeAligner.alignToSource(acroEle,tgtEle);
	} else {
		_logger.debug('Target not found, copying ' + acroEle + '\n\tto ' + tgtDn);
		Orgs.copy(acroEle.DName, modelloAcronimiDn);
	}
});

// add links to servizi towards acronimi within Modello
_logger.debug('Building view on ' + modelloServiziDn);
state.ViewBuilder.build(modelloServiziDn);
_logger.info('Copy All Acronimi completed');
// safe to avoid undefined error
return true;
})();
