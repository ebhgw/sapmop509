// test in hierarchy file
var logger = Packages.org.apache.log4j.Logger.getLogger("Dev.CheckSvcMdlStruct");
logger.debug("checkSvcMdlStruct Starting");

// Add a quote (\) 
function quoteMatchString (str) {
	var ar = str.split("");
	var res = '';
	for (var i = 0; i<ar.length;i++) {
		ch = ar[i];
		if (ch == '+' || ch == '%' || ch == '-' || ch == '?') {
			res = res + '.';
		}  else {
			res = res + ch;
		}
	}
	return res;
}

function getMatchPattern(pRisorsa, pServer, pCompInfra) {
	var match = '';
	var matchrisorsa = formula.util.encodeURL(pRisorsa.toLowerCase()) + "";
	var matchserver = formula.util.encodeURL(pServer.toLowerCase()) + "";
	var matchcompInfra = formula.util.encodeURL(pCompInfra.toLowerCase()) + "";
	if (matchserver == '') {
		match = '.*_' + matchrisorsa;
	} else if (matchserver == matchcompInfra) {
		match = matchserver + '_' + matchrisorsa;
	} else {
		match = '(' + matchserver + '|' + matchcompInfra + ')_' + matchrisorsa;
	}

	// should match LivelloServerRisorsa=sapmop31_sapmop31.3A7604.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\+tAllarmiOUTv2/root=Elements
	match = 'LivelloServerRisorsa='+state.StringUtil.quoteMatchString(match)+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
	logger.debug('Match is ' + match);
	return match;
}

// start from root
var pathRoot = 'folder_application=Services/ISP_FolderBase=Production/root=Organizations';
//var pathRoot = 'gen_folder=Services/gen_folder=testBSCM/gen_folder=test/root=Generational+Models/root=Services'
//var pathRoot = 'gen_folder=testFSA/root=Generational+Models/root=Services';

load('custom/lib/OrgUtil.fs');

var myAlarm = alarm;
//logger.debug("1 checkSvcMdlStruct " + alarm.Servizio + " " + alarm.SSA + " "+ alarm.Acronimo + " " + alarm.ClasseCompInfra  + " " + alarm.CompInfra  + " " +  alarm.Risorsa);

var eDname = '', pDname = '';

// alarm contains various paths
var nomeServizio = myAlarm.ServizioOrig;
var nomeSSA     = myAlarm.SSA;
var nomeAcronimo = myAlarm.Acronimo;
var strDescrizione = myAlarm.Descrizione;
var nomeClasseCompInfra = myAlarm.ClasseCompInfra.toUpperCase();
var nomeCompInfra =  myAlarm.CompInfra.toUpperCase();
var nomeRisorsa = myAlarm.Risorsa;
var nomeServer = myAlarm.Server;
var chiaveAllarme = myAlarm.ChiaveAllarme;
logger.debug("2 checkSvcMdlStruct " + nomeServizio + " " + nomeSSA + " "+ nomeAcronimo + " " + nomeClasseCompInfra  + " " + nomeCompInfra  + " " +  nomeRisorsa);

/*
var nomeServizio = 'TestServizio';
var nomeSSA     = 'TestSSA';
var nomeAcronimo = 'TestAcro';
var nomeClasseCompInfra = 'TestClasseCompInfra';
var nomeCompInfra = 'TestCompInfra';
var nomeRisorsa = 'TestRisorsa';
*/


// --------------------------------------
//            Services subtree
// --------------------------------------
/*
eDname = 'Servizio=' + formula.util.encodeURL(nomeServizio) + '/' + pathRoot;
eDname = 'SSA=' + formula.util.encodeURL(nomeSSA) + '/' + eDname;
eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + eDname;
eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
var found = false;
var created = false;

// no element found, proceed with single steps to build path
if (OrgUtil.findElement(eDname) == null) {
    logger.debug("checkSvcMdlStruct not found " + eDname + "\n trying to build path");
	
	var propNames, propValues;
	
    // Step 1 Servizio

    pDname = pathRoot;
    eDname = 'Servizio=' + formula.util.encodeURL(nomeServizio) + '/' + pathRoot;
    if (OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct not found " + eDname);
        created = OrgUtil.createElement('Servizio', nomeServizio, pDname);
        if (created) {
            logger.info("checkSvcMdlStruct created " + eDname);
        }
    }

    // step 2, SSA
    pDname = eDname;
    eDname = 'SSA=' + formula.util.encodeURL(nomeSSA) + '/' + eDname;
    if (OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct not found " + eDname);
		
		propNames = new Array();
		propValues = new Array();
		propNames.push('Algorithm');
		propValues.push('twothirdmajor');
        created = OrgUtil.createElementExt('SSA', nomeSSA, pDname, propNames, propValues);
        if (created) {
                    logger.info("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 3, Acronimo
    pDname = eDname;
    eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + eDname;
    if (OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct not found " + eDname);
		
		propNames = new Array();
		propValues = new Array();
		propNames.push('Algorithm');
		propValues.push('twothirdmajor');
        created = OrgUtil.createElementExt('Acronimo', nomeAcronimo, pDname, propNames, propValues);
        if (created) {
                    logger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 4, ClasseCompInfra
    pDname = eDname;
    eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
    if (OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct not found " + eDname);
        created = OrgUtil.createElement('ClasseCompInfra', nomeClasseCompInfra, pDname);
        if (created) {
			logger.debug("checkSvcMdlStruct created " + eDname);
		}
    }

    // step 5, CompInfra
    pDname = eDname;
    eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
    if (OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct not found " + eDname);
		
		propNames = new Array();
		propValues = new Array();
		propNames.push('Algorithm');
		propValues.push('twothirdmajor');
        created = OrgUtil.createElement('CompInfra', nomeCompInfra, pDname, propNames, propValues);
        if (created) {
                    logger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 6, Risorsa
    pDname = eDname;
    eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
    if (OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct not found " + eDname);
		logger.debug("checkSvcMdlStruct LivelloServerRisorsa " + formula.util.encodeURL(chiaveAllarme));
		
		propNames = new Array();
		propValues = new Array();
		
		var aMatch = getMatchPattern(nomeRisorsa, nomeServer, nomeCompInfra);
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

		// should match LivelloServerRisorsa=sapmop31_sapmop31.3A7604 .* /AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\+tAllarmiOUTv2/root=Elements
		var puntoStar = '.*'
		var match = 'LivelloServerRisorsa='+state.StringUtil.quoteMatchString(formula.util.encodeURL(chiaveAllarme))+puntoStar+'/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
        logger.debug('Match is ' + match);
		created = OrgUtil.createElementExt('Risorsa', nomeRisorsa, pDname, propNames, propValues);
        if (created) {
			logger.info("checkSvcMdlStruct created " + eDname);
		}
    }
    logger.info("checkSvcMdlStruct end");
}
*/

// --------------------------------------
//            AcroCatalago subtree
// --------------------------------------
// now build Acronimo subtree in Acronimo catalog
pathRoot = 'gen_folder=AcroCatalog/ISP_FolderBase=Production/root=Organizations';

eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + pathRoot;
eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
var found = false;
var created = false;

logger.debug('Step 2: check ' + eDname);
if (state.OrgUtil.findElement(eDname) == null) {
    logger.debug("checkSvcMdlStruct not found " + eDname + "\n trying to build path");

    // step 1, Acronimo
    pDname = pathRoot;
    eDname = 'Acronimo=' + formula.util.encodeURL(nomeAcronimo) + '/' + pathRoot;
    if (state.OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct Acronimo not found " + eDname);
        created = state.OrgUtil.createElement('Acronimo', nomeAcronimo, pDname);
        if (created) {
                    logger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 4, ClasseCompInfra
    pDname = eDname;
    eDname = 'ClasseCompInfra=' + formula.util.encodeURL(nomeClasseCompInfra) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct ClasseCompInfra not found " + eDname);
        created = state.OrgUtil.createElement('ClasseCompInfra', nomeClasseCompInfra, pDname);
        if (created) {
			logger.debug("checkSvcMdlStruct created " + eDname);
		}
    }

    // step 5, CompInfra
    pDname = eDname;
    eDname = 'CompInfra=' + formula.util.encodeURL(nomeCompInfra) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct CompInfra not found " + eDname);
        created = state.OrgUtil.createElement('CompInfra', nomeCompInfra, pDname);
        if (created) {
                    logger.debug("checkSvcMdlStruct created " + eDname);
                }
    }

    // step 6, Risorsa
    pDname = eDname;
    eDname = 'Risorsa=' + formula.util.encodeURL(nomeRisorsa) + '/' + eDname;
    if (state.OrgUtil.findElement(eDname) == null) {
        logger.debug("checkSvcMdlStruct Risorsa not found " + eDname);
		logger.debug("checkSvcMdlStruct LivelloServerRisorsa " + formula.util.encodeURL(chiaveAllarme));

		var propNames = new Array();
		var propValues = new Array();

		var aMatch = getMatchPattern(nomeRisorsa, nomeServer, nomeCompInfra);
		var now = new Date();
		
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
        propNames.push('source_name');
        propValues.push('alarm');
        propNames.push('source_date');
		propValues.push(now);

		created = state.OrgUtil.createElementExt('Risorsa', nomeRisorsa, pDname, propNames, propValues);
        if (created) {
			logger.info("checkSvcMdlStruct created " + eDname);
		}
    }
    logger.debug("checkSvcMdlStruct end");
}

delete myAlarm;
true;

