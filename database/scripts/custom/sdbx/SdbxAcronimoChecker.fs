//
// Copyright (c) 2014 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// Operation to check if acronimo is within cmdb baseline
//
// [Sdbx|Check Acronimo in Baseline]
//	command=
//	context=element
// description=Sdbx|Check Acronimo in Baseline
//	operation=load('custom/sdbx/CheckSdbxAcronimoInBaseline.fs');\nCheckSdbxAcronimoInBaseline.askUser(element);
//	permission=manage
//	target=dname:gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=Sandbox.*/root=Organizations
//	type=serverscript
//

var SdbxAcronimoChecker = (function (ele) {
    // Set logging
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.sdbx');
    load('custom/lib/Orgs.fs');
    load('custom/lib/OrgsTree.fs');
    load('custom/lib/underscore.js');

    // ele may be an acronimo
    var _getAcronimoList = function (eleDn) {
        var ele = Orgs.findElement(eleDn);
        if (ele)
            return ele.Children
        else
            return new Array();
    }

	// returns
	// acronimo folder dname = gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxProduzione/root=Organizations
	// context =  SystemTest|Produzione
    var _getContext = function (eleDn) {
        // previous var patt = /(gen_folder=Acronimi\/gen_folder=Staging\/ISP_FolderBase=Sandbox(.*))\/root=Organizations/;
        var patt = /(gen_folder=Acronimi\/gen_folder=Staging\/ISP_FolderBase=Sandbox(.*))\/root=Organizations/;
        var match = patt.exec(eleDn);
        var acroName,acroFolderDn,environment,pattAcro,matchAcro;

        if (match) {
            acroFolderDn = match[1];
            environment = match[2];
            pattAcro =  /(^Acronimo=([^\/]*))/;
            matchAcro = pattAcro.exec(eleDn);
            if (matchAcro) {
                acroName = matchAcro[2];
            } else {
                acroName = null;
            }

            _logger.debug('_getContext, env ' + environment + ' acroName ' + acroName + ' acroFolderDn ' + acroFolderDn + '/root=Organizations');
            return {
                acroName:acroName,
                acroFolderDn: acroFolderDn + '/root=Organizations',
                environment:environment
            }
        } else {
            _logger.warn('_getEnvironment unexpected error on operation CheckSdbxAcronimoInBaseline, match failed');
            return null;
        }
    }

    var _testInBaseline = function (acroName) {
        var res = false
        _logger.debug('Checking ' + 'Acronimo=' + formula.util.encodeURL(acroName)
            + '/gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services');
        var ele = Orgs.findElement('Acronimo=' + formula.util.encodeURL(acroName)
            + '/gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services')
        if (ele) {
            res = true;
        }
        _logger.debug('checkAcronimoInSysTestBaseline result ' + res);
        return {
            acro:acroName,
            inBaseline:res
        }
    }

    // starting from a tree root, collect acronimo elements and returns the one that are not in baseline
    var _listNotInBaseline = function (eleDn) {
        _logger.debug('_listNotInBaseline start on ' + eleDn);
        // ctx, root of acronimo and environment
        var ctx = _getContext(eleDn);
        var acroFolder = Orgs.findElement(ctx.acroFolderDn);
        // collect name of acronimo
        var sdbxNameList = OrgsTree.collectNameForElementOfClass(ctx.acroFolderDn,'Acronimo');
		_logger.debug('_listNotInBaseline sdbx list is ' + sdbxNameList.join());
        // gen_folder=Acronimi/gen_folder=Baseline/gen_folder=Produzione/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services
        var baseDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder='
            + ctx.environment + '/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services'
        var baseNameList = OrgsTree.collectNameForElementOfClass(baseDn,'Acronimo');
		_logger.debug('_listNotInBaseline base list is ' + baseNameList.join() + '\n\t\tchecked against ' + baseDn);
        var diff = _.difference(sdbxNameList, baseNameList);
		_logger.debug('_listNotInBaseline diff list is ' + diff.join());

        _logger.debug('_listNotInBaseline, acro non in baseline list >' + diff + '<');
        _logger.debug('_listNotInBaseline completed')
        return {
            acroList:diff,
            environment:ctx.environment
        };
    }

    // given the acronimo Dname, return true if there is a service that uses the acronimo
    // To find services, it looks under Gen Mod, Structure, Modello, Env (Prod or Systest), Baseline
    // (i.e. gen_folder=Baseline/gen_folder=Produzione/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services
    // for production) where reference model is built during ImpModello job
    var _isAcronimoReferredByServizio = function (acroDn) {
        var found = false,acroName=null;
        try {
            _logger.debug('_isAcronimoReferredByServizio, checking >' + acroDn + '<');
            // getting Acronimo name
            var ctx = _getContext(acroDn);
            var modelloRefServizioDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder='
                + ctx.environment + '/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services'
            _logger.debug('_isAcronimoReferredByServizio, looking for >' + ctx.acroName + '< under >' + modelloRefServizioDn + '<');
            found = OrgsTree.treeContains(modelloRefServizioDn,'Acronimo',ctx.acroName);
            _logger.debug('_isAcronimoReferredByServizio, contains ' + found);
        } catch (excp) {
            _logger.error('_isAcronimoReferredByServizio excp ' + excp);
            throw excp;
        }
        return found;
    }

    return {
        getContext:_getContext,
        isAcronimoReferredByServizio:_isAcronimoReferredByServizio,
        listNotInBaseline:_listNotInBaseline,
        testInBaseline:_testInBaseline
    }
}())