//@debug off
/*

 */
 
//
//	[Sdbx|Sync All Servizi To Modello]
//	context=element
//	description=Sdbx|Sync All Servizi To Modello
//	operation=load('custom/operation/SyncServizi.fs');\nsession.sendMessage('Sync Servizi To Modello completed');
//	permission=manage
//	target=dnamematch:^gen_folder=Servizi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations
//	type=serverscript
//

(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.sdbx");
var sdbxRootDn = element.parent.parent.Dname;
_logger.info('Sync Servizi started on Sandbox ' + sdbxRootDn);
load('custom/util/OrgsCopier.fs');
load('custom/lib/underscore.js');
load('custom/lib/Orgs.fs');

var stagingAcronimiDn = element.DName;
var pdn = state.DNamer.parseDn(stagingAcronimiDn);

var modelloServiziDn = 'gen_folder=Servizi/gen_folder=Modello/' + sdbxRootDn;
var stagingServiziDn = 'gen_folder=Servizi/gen_folder=Staging/' + sdbxRootDn;
_logger.debug('Syncing staging ' + stagingServiziDn + '\n\t\tto modello ' + modelloServiziDn);
var modelloServiziEle = formula.Root.findElement(modelloServiziDn);
// use children as Children does not cope with // elements
var srvChildren = modelloServiziEle.children;
_.each(srvChildren, function(ele){Orgs.deleteElement(ele)});
var actor = OrgsCopier();
//actor.test();
actor.copySubTree(stagingServiziDn, modelloServiziDn);

// var modelloServiziRoot = state.Orgs.findElement(modelloServiziDn);
var visitor =
{
count: 0,
clazz: '',
emptyAr: new Array(),
        
visit: function ( child )
{
	clazz = child.elementClassname + '';
	if (clazz == 'Acronimo') {
		child["SourceElements"] = visitor.emptyAr;
        child["Children"] = visitor.emptyAr;
		visitor.count++;
	}
}
}

if (modelloServiziEle) {
	modelloServiziEle.walk ( visitor );
	_logger.debug('reset sources from ' + visitor.count + ' acronimi');
}

_logger.info('SyncServizi - Connecting Servizi 2 Acronimi');
state.ViewBuilder.build(modelloServiziDn);
_logger.info('SyncServizi completed');

})();