//@debug off
/*

	Delete element in Modello/Acronimi if existent, then copy
	ow should implement a TreeAligner
 */
 
//
//	[Sdbx|Sync Servizio To Modello]
//	command=
//	context=element
//	description=Sdbx|Sync Servizio To Modello
//	operation=load('custom/sdbx/SyncServizioToModello.fs');\nsession.sendMessage('Sync Servizio To Modello completed');
//	permission=manage
//	target=dnamematch:^Servizio=[^/]*/gen_folder=Servizi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations
//	type=serverscript
 
 //

(function () {
var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.sdbx");
var sdbxRootDn = element.parent.parent.parent.Dname;
_logger.info('running Sync Servizio To Modello on Sdbx ' + sdbxRootDn);
load('custom/lib/Orgs.fs');

var sdbxRootDn = element.parent.parent.parent.Dname;
var pdn = state.DNamer.parseDn(element.DName);

var modelloServiziDn = 'gen_folder=Servizi/gen_folder=Modello/' + sdbxRootDn;
var stagingServiziDn = pdn.parent;
_logger.debug('Syncing Servizio ' + pdn.name + '\n\tfrom ' + stagingServiziDn + '\n\tto ' + modelloServiziDn);

var newEle = state.Orgs.findElement(pdn.fullName + '/' + modelloServiziDn);
_logger.info('Syncing to ' + pdn.fullName + '/' + modelloServiziDn);
if(newEle) {
	Orgs.deleteElement(newEle);
}
// copy element Servizio, will copy also children (that is Acronimo)
_logger.debug('Copying ' + element.DName + ' under ' + modelloServiziDn);
var copied = Orgs.copy(element.DName, modelloServiziDn);
var newEle = state.Orgs.findElement(pdn.fullName + '/' + modelloServiziDn);
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
newEle.walk(visitor);

_logger.debug('Sync Servizio To Modello - Connecting Servizi 2 Acronimi');
state.ViewBuilder.build(modelloServiziDn);
_logger.info('SyncServizio To Modello completed');

})();
