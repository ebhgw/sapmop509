/*
  Script checkSvcMdlStruct.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  Test used in hierarchy file to propagate immediately newly generated adapter elements
*/

var now = new Date();
var devLogger = Packages.org.apache.log4j.Logger.getLogger("Dev");
devLogger.debug("checkSvcMdlStruct Starting " + now);

//if (typeof state.OrgUtil == undefined){
if (typeof state.OrgUtil == 'undefined'){
    load('custom/lib/OrgUtil.fs');
}

// start from root
var pathRoot = 'folder_application=Services/ISP_FolderBase=Production/root=Organizations';
//Other path used during tests
//var pathRoot = 'gen_folder=Services/gen_folder=testBSCM/gen_folder=test/root=Generational+Models/root=Services'
//var pathRoot = 'gen_folder=testFSA/root=Generational+Models/root=Services';

// copy alarm to javascript space
var myAlarm = alarm;

var eDname = '', pDname = '';

// alarm contains various paths
var nomeServizioGV = alarm.ServizioGV;
var nomeServizio = alarm.Servizio;
var nomeServizioOrig = alarm.ServizioOrig;
var nomeSSA     = alarm.SSA;
var nomeAcronimo = alarm.Acronimo;
var strDescrizione = alarm.Descrizione;
var nomeClasseCompInfra = alarm.ClasseCompInfra;
var nomeCompInfra =  alarm.CompInfra;
var nomeRisorsa = alarm.Risorsa;
var nomeServer = alarm.Server;
var chiaveAllarme = alarm.ChiaveAllarme;



/*
var nomeServizio = 'TestServizio';
var nomeSSA     = 'TestSSA';
var nomeAcronimo = 'TestAcro';
var nomeClasseCompInfra = 'TestClasseCompInfra';
var nomeCompInfra = 'TestCompInfra';
var nomeRisorsa = 'TestRisorsa';
*/

// Build original structure under Production/Servizio (the one without Global View)
eDname = 'Servizio=' + formula.util.encodeURL(nomeServizioOrig) + '/' + pathRoot;
eDname = 'SSA=' + formula.util.encodeURL(nomeSSA) + '/' + eDname;
eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + eDname;
eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
var found = false;
var created = false;

// no element found, proceed with single steps to build path
devLogger.debug('Step 1: check ' + eDname);
if (state.OrgUtil.findElement(eDname) == null) {
    devLogger.debug("checkSvcMdlStruct not found " + eDname + "\n trying to build path");
    // Step 1 Servizio

    pDname = pathRoot;
    eDname = 'Servizio=' + formula.util.encodeURL(nomeServizioOrig) + '/' + pathRoot;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('Servizio', nomeServizioOrig, pDname);
        if (created) {
            devLogger.debug("checkSvcMdlStruct created " + eDname);
        }
    }

    // step 2, SSA
    pDname = eDname;
    eDname = 'SSA=' + formula.util.encodeURL(nomeSSA) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('SSA', nomeSSA, pDname);
        if (created) {
                    devLogger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 3, Acronimo
    pDname = eDname;
    eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('Acronimo', nomeAcronimo, pDname);
        if (created) {
                    devLogger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 4, ClasseCompInfra
    pDname = eDname;
    eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('ClasseCompInfra', nomeClasseCompInfra, pDname);
        if (created) {
			devLogger.debug("checkSvcMdlStruct created " + eDname);
		}
    }

    // step 5, CompInfra
    pDname = eDname;
    eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('CompInfra', nomeCompInfra, pDname);
        if (created) {
                    devLogger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 6, Risorsa
    pDname = eDname;
    eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
		devLogger.debug("checkSvcMdlStruct LivelloServerRisorsa " + formula.util.encodeURL(chiaveAllarme));
		
		var propNames = new Array();
		var propValues = new Array();
		
		var aMatch = 'LivelloServerRisorsa='+state.StringUtil.quoteMatchString(formula.util.encodeURL(chiaveAllarme))+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
		propNames.push('Matches');
		propValues.push(aMatch);
		
		propNames.push('Algorithm');
		propValues.push('paramHighest');
		propNames.push('Algorithm Parameters');
		propValues.push('gather="ORG";defaultCondition="OK";reason="Default Condition OK";');
		
		propNames.push('Servizio');
      	propValues.push(nomeServizio);
        propNames.push('SSA');
      	propValues.push(nomeSSA);
        propNames.push('Acronimo');
      	propValues.push(nomeAcronimo);
        propNames.push('Descrizione');
      	propValues.push(strDescrizione);
        propNames.push('ClasseCompInfra');
      	propValues.push(nomeClasseCompInfra);
        propNames.push('CompInfra');
      	propValues.push(nomeCompInfra);
        propNames.push('Risorsa');
      	propValues.push(nomeRisorsa);
        propNames.push('Server');
      	propValues.push(nomeServer);
        propNames.push('ChiaveAllarme');
      	propValues.push(chiaveAllarme);

		// should match LivelloServerRisorsa=sapmop31_sapmop31.3A7604.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\+tAllarmiOUTv2/root=Elements
		var match = 'LivelloServerRisorsa='+state.StringUtil.quoteMatchString(formula.util.encodeURL(chiaveAllarme))+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
        devLogger.debug('Match is ' + match);
		created = state.OrgUtil.createElementExt('Risorsa', nomeRisorsa, pDname, propNames, propValues);
        if (created) {
			devLogger.debug("checkSvcMdlStruct created " + eDname);
		}
    }
    devLogger.debug("checkSvcMdlStruct end");
}

// now build Acronimo subtree in Acronimo catalog
pathRoot = 'gen_folder=Acronimo+Catalog/ISP_FolderBase=Production/root=Organizations';

eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + pathRoot;
eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
var found = false;
var created = false;

devLogger.debug('Step 2: check ' + eDname);
if (state.OrgUtil.findElement(eDname) == null) {
    devLogger.debug("checkSvcMdlStruct not found " + eDname + "\n trying to build path");

    // step 1, Acronimo
    pDname = pathRoot;
    eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + pathRoot;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct Acronimo not found " + eDname);
        created = state.OrgUtil.createElement('Acronimo', nomeAcronimo, pDname);
        if (created) {
                    devLogger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 4, ClasseCompInfra
    pDname = eDname;
    eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct ClasseCompInfra not found " + eDname);
        created = state.OrgUtil.createElement('ClasseCompInfra', nomeClasseCompInfra, pDname);
        if (created) {
			devLogger.debug("checkSvcMdlStruct created " + eDname);
		}
    }

    // step 5, CompInfra
    pDname = eDname;
    eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct CompInfra not found " + eDname);
        created = state.OrgUtil.createElement('CompInfra', nomeCompInfra, pDname);
        if (created) {
                    devLogger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 6, Risorsa
    pDname = eDname;
    eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct Risorsa not found " + eDname);
		devLogger.debug("checkSvcMdlStruct LivelloServerRisorsa " + formula.util.encodeURL(chiaveAllarme));

		var propNames = new Array();
		var propValues = new Array();

		var aMatch = 'LivelloServerRisorsa='+state.StringUtil.quoteMatchString(formula.util.encodeURL(chiaveAllarme))+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
		propNames.push('Matches');
		propValues.push(aMatch);

		propNames.push('Algorithm');
		propValues.push('paramHighest');
		propNames.push('Algorithm Parameters');
		propValues.push('gather="ORG";defaultCondition="OK";reason="Default Condition OK";');

        propNames.push('Acronimo');
      	propValues.push(nomeAcronimo);
        propNames.push('Descrizione');
      	propValues.push(strDescrizione);
        propNames.push('ClasseCompInfra');
      	propValues.push(nomeClasseCompInfra);
        propNames.push('CompInfra');
      	propValues.push(nomeCompInfra);
        propNames.push('Risorsa');
      	propValues.push(nomeRisorsa);
        propNames.push('Server');
      	propValues.push(nomeServer);
        propNames.push('ChiaveAllarme');
      	propValues.push(chiaveAllarme);

		// should match LivelloServerRisorsa=sapmop31_sapmop31.3A7604.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\+tAllarmiOUTv2/root=Elements
		var match = 'LivelloServerRisorsa='+state.StringUtil.quoteMatchString(formula.util.encodeURL(chiaveAllarme))+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
        devLogger.debug('Match is ' + match);
		created = state.OrgUtil.createElementExt('Risorsa', nomeRisorsa, pDname, propNames, propValues);
        if (created) {
			devLogger.debug("checkSvcMdlStruct created " + eDname);
		}
    }
    devLogger.debug("checkSvcMdlStruct end");
}

// now build Service subtree in Service2Acro catalog

pathRoot = 'gen_folder=ServicesToAcro/ISP_FolderBase=Production/root=Organizations';

eDname = 'ServizioGlobalView=' + formula.util.encodeURL(nomeServizioGV) + '/' + pathRoot;
eDname = 'Servizio=' + formula.util.encodeURL(nomeServizio) + '/' + eDname;
eDname = 'SSA=' + formula.util.encodeURL(nomeSSA) + '/' + eDname;
eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + eDname;
var found = false;
var created = false;

// no element found, proceed with single steps to build path
devLogger.debug('Step 1: check ' + eDname);
if (state.OrgUtil.findElement(eDname) == null) {
    devLogger.debug("checkSvcMdlStruct not found " + eDname + "\n trying to build path");
    // Step 1 Servizio

    pDname = pathRoot;
    eDname = 'ServizioGlobalView=' + formula.util.encodeURL(nomeServizioGV) + '/' + pathRoot;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('ServizioGlobalView', nomeServizioGV, pDname);
        if (created) {
            devLogger.debug("checkSvcMdlStruct created " + eDname);
        }
    }
	
	eDname = 'Servizio=' + formula.util.encodeURL(nomeServizio) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('Servizio', nomeServizio, pDname);
        if (created) {
            devLogger.debug("checkSvcMdlStruct created " + eDname);
        }
    }

    // step 2, SSA
    pDname = eDname;
    eDname = 'SSA=' + formula.util.encodeURL(nomeSSA) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
        created = state.OrgUtil.createElement('SSA', nomeSSA, pDname);
        if (created) {
                    devLogger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 3, Acronimo
    pDname = eDname;
    eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        devLogger.debug("checkSvcMdlStruct not found " + eDname);
		
		var propNames = new Array();
		var propValues = new Array();

		// [Pattern-LDAP]:(objectClass=ClasseCompInfra)/Acronimo=ABCA0/gen_folder=Acronimo\+Catalog/ISP_FolderBase=Production/root=Organizations
		var aMatch = '\[Pattern-LDAP\]:\(objectClass=ClasseCompInfra\)/Acronimo='+state.StringUtil.quoteMatchString(formula.util.encodeURL(nomeAcronimo))+'/gen_folder=Acronimo\+Catalog/ISP_FolderBase=Production/root=Organizations';
		propNames.push('Matches');
		propValues.push(aMatch);
		
		created = state.OrgUtil.createElementExt('Acronimo', nomeAcronimo, pDname, propNames, propValues);
        if (created) {
                    devLogger.debug("checkSvcMdlStruct created " + eDname);
                }
    }
}

true;

