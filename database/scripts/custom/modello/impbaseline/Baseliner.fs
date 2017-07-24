/*
 Script BaselineInit.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Set Ambiente property for the 3 environments Produzione, Produzione/SystemTest, SystemTest

 */


var Baseliner = function (cfgprop) {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.impbaseline.baseliner");
	load('custom/lib/underscore.js');
    load('custom/lib/OrgsFinder.fs');
    load('custom/lib/OrgsTree.fs');
		
    load('custom/orgs/TreeAligner.fs');
    load('custom/lib/ViewBuilder.fs');

    // Building GenMod/<Ambiente> and Align Service Models/<Ambiente>
    // Ambiente is Produzione or SystemTest
    // rebuildGenMod se false disattiva il bscm
    // 1. generate Generational Models/Baseline+AlarmedElement_SystemTest both for Acronimi and Servizi (with Delete-Before-Execute)
    // 2. align Service Models/SystemTest to Generational Models/Baseline+AlarmedElement_SystemTest removing extra elements
    //    and adding new ones
    // 3. link Acronimo elements in Servizi structure to ClasseCompInfra elements (children to Acronimi) in Acronimi structure
    var _buildModelloEnvironment = function (ambiente, rebuildGenMod) {
		try {
			if (typeof rebuildGenMod === 'undefined')
				rebuildGenMod = true;
			_logger.debug('buildModelloEnvironment for ambiente ' + ambiente + ' with rebuild ' + rebuildGenMod );

			//_logger.debug('buildModelloEnvironment typeof Orgs ' + typeof Orgs);
			//_logger.debug('buildModelloEnvironment typeof Orgs.Tree ' + typeof Orgs.Tree);
			//_logger.debug('buildModelloEnvironment typeof Orgs.Tree.collectFullNameForElementOfClass ' + typeof Orgs.Tree.collectFullNameForElementOfClass);
			
			var genModBaselineAcroDn = '';
			var genModBaselineSrvDn = '';
			var modelloAcroDn = '';
			var modelloSrvzDn = '';
			var sdbxStagingAcroDn = '';


			if (ambiente === 'Produzione') {
				genModBaselineAcroDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Produzione/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
				genModBaselineSrvDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Produzione/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
				modelloAcroDn = 'gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations';
				modelloSrvzDn = 'gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations';
				// used to compute the difference list
				sdbxStagingAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxProduzione/root=Organizations';
			} else  if (ambiente === 'SystemTest') {
				genModBaselineAcroDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder=SystemTest/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
				genModBaselineSrvDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder=SystemTest/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
				modelloAcroDn = 'gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations';
				modelloSrvzDn = 'gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations';
				sdbxStagingAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations';
			}

			if (rebuildGenMod) {
				// Baseline+AlarmedElement_SystemTest
				_logger.info('buildModelloEnvironment, building view Baseline+Alarmed Elements/Acronimi');
				ViewBuilder.build(genModBaselineAcroDn);
				_logger.info('_buildModelloEnvironment, building view Baseline+Alarmed Elements/Servizi');
				ViewBuilder.build(genModBaselineSrvDn);
			}
			
			_logger.debug('buildModelloEnvironment, collecting acronimo lists');
			//_logger.debug('buildModelloEnvironment typeof Orgs ' + typeof Orgs);
			//_logger.debug('buildModelloEnvironment typeof Orgs.Tree ' + typeof Orgs.Tree);
			//_logger.debug('buildModelloEnvironment typeof Orgs.Tree.collectFullNameForElementOfClass ' + typeof OrgsTree.collectFullNameForElementOfClass);
			_logger.debug('buildModelloEnvironment calling on ' + genModBaselineSrvDn);
			var acroList = OrgsTree.collectFullNameForElementOfClass(genModBaselineSrvDn, 'Acronimo');
			_logger.debug('buildModelloEnvironment, acroList ' + acroList.join(','));
			var sbxAcroList = OrgsTree.collectFullNameForElementOfClass(sdbxStagingAcroDn, 'Acronimo');
			_logger.debug('buildModelloEnvironment, sbxAcroList ' +  + acroList.join(','));
			var diffList = _.difference(sbxAcroList, acroList);
			_logger.debug('buildModelloEnvironment, diffList ' +  diffList.join(','));

			_logger.debug('buildModelloEnvironment, aligning to source')
			TreeAligner.alignToSource(genModBaselineAcroDn, modelloAcroDn, diffList);
			TreeAligner.alignToSource(genModBaselineSrvDn, modelloSrvzDn);
			// Link Servizi 2 Acronimi
			_logger.debug('buildModelloEnvironment, link (bscm) Servizi to Acronimi structure');
			ViewBuilder.build('gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations');
			_logger.info('buildModelloEnvironment for ' + ambiente + ' completed');
		} catch (excp) {
			_logger.error('buildModelloEnvironment, got ' + excp);
			throw excp;
		}
    }

    // Build Sdbx
    var _buildSdbxEnvironment = function (ambiente, rebuildGenMod) {
		try {
        if (typeof rebuildGenMod == 'undefined')
            rebuildGenMod = true;
        _logger.debug('buildSdbxEnvironment, ambiente ' + ambiente + ' with rebuild ' + rebuildGenMod );

        var stagingAcroDn = '';
        var stagingSrvDn = '';
        var sourceDn = '';

        if (ambiente === 'Produzione') {
			//               gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxProduction/root=Organizations
            stagingAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxProduzione/root=Organizations';
            stagingSrvDn = 'gen_folder=Servizi/gen_folder=Staging/ISP_FolderBase=SandboxProduzione/root=Organizations';
			//          gen_folder=Acronimi/gen_folder=Baseline/gen_folder=Produzione/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services
            sourceDn = 'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=Produzione/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
        } else  if (ambiente === 'SystemTest') {
            stagingAcroDn = 'gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations';
            stagingSrvDn = 'gen_folder=Servizi/gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations';
            sourceDn = 'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=SystemTest/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
        } else {
			_logger.error('_buildSdbxEnvironment unknown ambiente ' + ambiente);
			throw "Baseliner _buildSdbxEnvironment unknown ambiente " + ambiente;
		}

        if (rebuildGenMod) {
            // Modello/<ambiente>/Baseline/Acronimi
            _logger.info('_buildSdbxEnvironment, run bscm for ' + sourceDn);
            ViewBuilder.build( sourceDn );
        }

        var stagingAcro = OrgsFinder.findElement(stagingAcroDn);
		_logger.info('_buildSdbxEnvironment found stagingAcro ' + stagingAcro.DName);
        var sourceEle = OrgsFinder.findElement(sourceDn);
		_logger.info('_buildSdbxEnvironment found sourceEle ' + sourceEle.DName);
		

        // if found continue
        if (stagingAcro != null && sourceEle != null) {
            var chdn = stagingAcro.Children;
			var child, dn;
			_logger.debug('_buildSdbxEnvironment, looping on Sdbx/Staging/Acronimo ' + chdn.length +  ' elements');
            for (var i=0; i < chdn.length; i++) {
                try {
					dn = chdn[i];
					_logger.debug('_buildSdbxEnvironment, processing ' + dn);
                    var pdn = state.DNamer.parseDn(dn);
                    var acroBaselineDn = 'Acronimo='+ formula.util.encodeURL(pdn.encodedName) + '/' + sourceDn;
                    var acroBaseline = OrgsFinder.findElement(acroBaselineDn);
					
                    if (acroBaseline) {
                        _logger.info('_buildSdbxEnvironment aligning ' + dn + ' to ' + acroBaselineDn);
                        TreeAligner.alignToSource(acroBaselineDn,dn);
                    } else {
                        _logger.warn('_buildSdbxEnvironment, not found in sourceEle: ' + acroBaselineDn);
                    }
                } catch (excp) {
                    _logger.error('_buildSdbxEnvironment, each on stagingAcro.Children error ' + excp)
                }
            }
            _logger.info('_buildSdbxEnvironment looping on Sdbx/Staging/Acronimo elements completed');
        } else {
			_logger.error('_buildSdbxEnvironment not found Acronimi on Staging or Baseline on Structure (Gen Models)');
			throw 'Baseliner _buildSdbxEnvironment not found Acronimi on Staging or Baseline on Structure (Gen Models)'
        }

        // Link Servizi 2 Acronimi
        _logger.info('_buildSdbxEnvironment, link (bscm) Servizi to Acronimi structure');
        ViewBuilder.build( stagingSrvDn );
        _logger.info('_buildSdbxEnvironment, link (bscm) Servizi to Acronimi structure completed');
		} catch (excp) {
			_logger.error('buildSdbxEnvironment, got ' + excp);
			throw excp;
		}
    }

    // Building Production structure
// 1. generate Generational Models/Baseline+Alarmed Elements both for Acronimi and Servizi (with Delete-Before-Execute)
// 2. align Service Models/Production to Generational Models/Baseline+Alarmed Elements removing extra elements
//    and adding new ones
// 3. link Acronimo elements in Servizi structure to ClasseCompInfra elements (children to Acronimi) in Acronimi structure
    var _buildProduction = function () {
        // Baseline+Alarmed Elements
        var ambiente = 'Produzione';
		_logger.debug('_buildProduction starting on ' + ambiente);
        _buildModelloEnvironment(ambiente, true);
        _buildSdbxEnvironment(ambiente, true);
        _logger.info('Production and Sdbx Production updated');
    }



// Building SystemTest
// 1. generate Generational Models/Baseline+AlarmedElement_SystemTest both for Acronimi and Servizi (with Delete-Before-Execute)
// 2. align Service Models/SystemTest to Generational Models/Baseline+AlarmedElement_SystemTest removing extra elements
//    and adding new ones
// 3. link Acronimo elements in Servizi structure to ClasseCompInfra elements (children to Acronimi) in Acronimi structure
    var _buildSysTest = function () {
        // Baseline+Alarmed Elements
        var ambiente = 'SystemTest';
		_logger.debug('_buildSysTest starting on ' + ambiente);
        _buildModelloEnvironment(ambiente, true);
        _buildSdbxEnvironment(ambiente, true)
        _logger.info('Production and Sdbx Production updated');

    }

    // Building Servizi Infrastrutturali (Produzione)
    // 1. build a baseline for whole service structure under Generational Models/Structure/Modello/Baseline Servizi Infrastrutturali, bscm with delete-before-execute
    // 2. align Production/Servizi Infrastrutturali to baseline
    var _buildServiziInfrastrutturali = function (rebuildGenMod) {
        if (typeof rebuildGenMod == 'undefined')
            rebuildGenMod = true;

        if (rebuildGenMod) {
            // Modello/SystemTest/Baseline/Acronimi
            _logger.info('Env:Production,gen_folder=Baseline+Servizi+Infrastrutturali/gen_folder=Structure/root=Generational+Models/root=Services');
            ViewBuilder.build('gen_folder=Baseline+Servizi+Infrastrutturali/gen_folder=Structure/root=Generational+Models/root=Services');
        }

        _logger.info('Env:Production, align Servizi Infrastrutturali');
        var targetDn = 'gen_folder=Servizi+Infrastrutturali/ISP_FolderBase=Production/root=Organizations';
        var templateDn = 'gen_folder=Baseline+Servizi+Infrastrutturali/gen_folder=Structure/root=Generational+Models/root=Services';

        _logger.info('Production aligning ' + targetDn + ' to template ' + templateDn);
        TreeAligner.alignToTemplate(templateDn,targetDn)

        // Link 2 alarms
        _logger.info('Env:Production, link (bscm) Servizi Infrastrutturali to Alarms');
        var viewElement = formula.Root.findElement(targetDn);
        viewElement.perform( session, "ViewBuilder|Run", [], [] );
        _logger.info('Env:Production, link (bscm) Servizi Infrastrutturali to Alarms completed');

    }

    return {
        buildModelloEnvironment:_buildModelloEnvironment,
        buildProduction: _buildProduction,
        buildSdbxEnvironment:_buildSdbxEnvironment,
        buildSysTest:_buildSysTest,
        buildServiziInfrastrutturali:_buildServiziInfrastrutturali
    }
};