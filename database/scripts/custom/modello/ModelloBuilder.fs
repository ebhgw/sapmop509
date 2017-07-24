/*

 Script SdbxSystestBuilder.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Build SdbxSystest Modello. Actually only the Acronimo subtree
 Note: it could be possible to copy structure directly from the adapter. However this will copy the adapter element with
 the adapter class while bscm adjust it automatically, so it is safer.

 */
load('custom/lib/underscore.js');

var ModelloBuilder = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.modello");
	
	var _buildProduction = function () {
        //Servizi
        var templateDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
        var targetDn = 'gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'
        _logger.info('Production aligning Servizi ' + targetDn + ' to template ' + templateDn);
        state.TreeAligner.alignToTemplate(templateDn,targetDn)

        //Acronimi
        templateDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
        targetDn = 'gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'
        _logger.info('Production aligning ' + targetDn + ' to template ' + templateDn);
        state.TreeAligner.alignToTemplate(templateDn,targetDn)

	}
	
	// Generate structure for acronimos defined within Sdbx/Staging/Acronimi
    // In this case we build only acronimo structure and not servizio structure.
    // For every acronimo, we take from the CMDB/ECM provided baseline the structure for that acronimo
    // and copy it under the acronimo defined in Sdbx/Staging/Acronimo
    var _buildSdbxSysTest = function () {
		_logger.info('Starting _buildSdbxSysTest');
        // Where Sdbx/Staging/Acronimo is
        var modelloAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations';
        // Where CMDB/ECM baseline for acronimo is
        var baselineDn = 'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=SystemTest/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
        // return null if not found
        var modelloAcro = state.Orgs.findElement(modelloAcroDn);
        var baseline = state.Orgs.findElement(baselineDn);

        if (modelloAcro && baseline) {
			_logger.info('Looping on Sdbx/Staging/Acronimo elements');
            _.each(modelloAcro.Children, function(dn) {
				var pdn = state.DNamer.parseDn(dn);
                var acroBaselineDn = 'Acronimo='+ formula.util.encodeURL(pdn.encodedName) + '/' + baselineDn;
				var acroBaseline = state.Orgs.findElement(acroBaselineDn);
				if (acroBaseline) {
					_logger.info('Aligning ' + dn + ' to ' + acroBaselineDn);
					state.TreeAligner.alignToTemplate(acroBaselineDn,dn);
				} else {
					_logger.warn('Not found in baseline: ' + acroBaselineDn);
				}
            })
        } else {
            _logger.warn('Not found Modello or Baseline root element');
        }
    }

	// collect acronimo found within the dn subtree
    var collectAcroFullname = function (dn) {
        var visitor = {
            acroList: [],
            count: 0,
            visit: function ( ele ) {
				var clazz = ele.elementClassname + '';
                if (clazz == 'Acronimo') {
                    visitor.acroList.push('Acronimo=' + ele.Name);
                    visitor.count++;
                }
            }
        }
        var treeRoot = state.Orgs.findElement(dn);
        if (treeRoot)
            treeRoot.walk(visitor);
        return visitor.acroList;
    }

    /* Generate structure for acronimos defined within SystemTest/Acronimi
    * The service structure is synced to the baseline
    * The acronimo structure is synced to the baseline only for acronimos excluding
    * acronimos that appears in Sdbx, unless they are connected to a service
    * Example, XXXX? are acronimos in baseline:
    * - XXXX1 is in sandbox and is connected to a SRV1-SYSTEST, structure is copied
    * - XXXX2 is in sandbox and is not connected to any service, structure is not copied
    * - XXXX3 is not in sandbox, structure is copied
    */
	var _buildSystemTest = function () {
        
        //Servizi, syncing
        var baselineSrvzDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services';
        var modelloSrvzDn = 'gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations';
        _logger.info('buildSystemTest step Servizi, aligning ' + modelloSrvzDn + ' to base ' + baselineSrvzDn);
        state.TreeAligner.alignToTemplate(baselineSrvzDn,modelloSrvzDn);
		_logger.info('buildSystemTest step Servizi, completed');

        // compute list of acronimos to be excluded
        var acro2srvList = collectAcroFullname(baselineSrvzDn);
        var modelloSdbxStagingAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations';
        var sbxAcroList = collectAcroFullname(modelloSdbxStagingAcroDn);
        var diffList = _.difference(sbxAcroList, acro2srvList)
        _logger.debug('buildSystemTest: exclude list -------->');
        _.each(diffList, function(str) { _logger.debug(str)});
        _logger.debug('<--------')

        //Acronimi, copy only acronimi linked to a Servizio
        var baselineAcroDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services';
        var modelloAcroDn = 'gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations';
        _logger.info('buildSystemTest step Acronimi, aligning ' + modelloAcroDn + ' to template ' + baselineAcroDn);
		var modelloAcro = state.Orgs.findElement(modelloAcroDn);

        // treat first level
        state.TreeAligner.deleteFirstLevNotInTemplate(baselineAcroDn, modelloAcroDn, diffList);
        state.TreeAligner.addFirstLev4Template(baselineAcroDn, modelloAcroDn, diffList);
        // foreach (remaining) acronimos, align
		_.each(modelloAcro.Children, function(dn) {
                var pdn = state.DNamer.parseDn(dn);
                var templateDn = 'Acronimo='+ formula.util.encodeURL(pdn.encodedName) + '/' + baselineAcroDn;
                var template = state.Orgs.findElement(templateDn);
                if (template) {
                    _logger.info('Aligning ' + dn + ' to ' + template);
                    state.TreeAligner.alignToTemplate(templateDn,dn);
                } else {
                    _logger.warn('Not found in baseline: ' + templateDn);
                }
            })

        // Link Servizi 2 Acronimi
		/*
        _logger.info('Building view Production/Servizi');
        state.ViewBuilder.build('gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations');
		*/
    }
	
	// original building SystemTest code
	var _buildSystemTestOld = function () {

        //Servizi
        var templateDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services'
        var targetDn = 'gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations'
        _logger.info('Aligning ' + targetDn + ' to template ' + templateDn);
        //TreeAligner.alignToTemplate(templateDn,targetDn)

        //Acronimi
        templateDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement_SystemTest/gen_folder=Structure/root=Generational+Models/root=Services'
        targetDn = 'gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations'
        _logger.info('Aligning ' + targetDn + ' to template ' + templateDn);
        //TreeAligner.alignToTemplate(templateDn,targetDn)
		TreeAligner.addTemplateEle(templateDn,targetDn)
	}
	return {
		buildProduction: _buildProduction,
		buildSdbxSysTest: _buildSdbxSysTest,
        buildSystemTest:_buildSystemTest,
		buildSystemTestOld:_buildSystemTestOld
	}

})();
