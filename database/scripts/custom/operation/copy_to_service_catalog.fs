// copy a structure to Service Model (should be applied only to element of class Risorsa in order to have the whole path
load('custom/orgs/StructureGenerator.fs');

var svcPathInfo = new Object();

svcPathInfo.nomeServizioGV 			= element.parent.parent.parent.parent.parent.parent.name;
svcPathInfo.nomeServizio				= element.parent.parent.parent.parent.parent.name;
svcPathInfo.nomeSSA					= element.parent.parent.parent.parent.name;
svcPathInfo.nomeAcronimo			= element.parent.parent.parent.name;
svcPathInfo.nomeClasseCompInfra 	= element.parent.parent.name;
svcPathInfo.nomeCompInfra 			= element.parent.name;
svcPathInfo.nomeRisorsa 				= element.name;

svcPathInfo.nomeServizioGV 	= svcPathInfo.nomeServizioGV.toUpperCase();
svcPathInfo.nomeServizio 		= svcPathInfo.nomeServizio.toUpperCase();
svcPathInfo.nomeServizioOrig 	= svcPathInfo.nomeServizioOrig.toUpperCase();
svcPathInfo.nomeSSA     		= svcPathInfo.nomeSSA.toUpperCase();
svcPathInfo.nomeAcronimo 	= svcPathInfo.nomeAcronimo.toUpperCase();
svcPathInfo.nomeClasseCompInfra = svcPathInfo.nomeClasseCompInfra.toUpperCase();
svcPathInfo.nomeCompInfra 	= svcPathInfo.nome CompInfra.toUpperCase();
svcPathInfo.nomeRisorsa 		= svcPathInfo.nomeRisorsa.toLowerCase();
svcPathInfo.nomeServer 		= svcPathInfo.nomeServer.toLowerCase();
svcPathInfo.chiaveAllarme 	= svcPathInfo.ChiaveAllarme.toLowerCase();

copy2ServiceCatalog(svcPathInfo);