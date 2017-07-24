/**
 * Export structure data
 *
 * Author: E. Bomitali, Hogwart, 2012
 * Tested with versions: 5.0
 * Version: 1.0
 * Build date: 2 August 2012
 *
 */

if (typeof state.FileWriter == 'undefined') {
	formula.log.info('Loading FileWriter within ModelExporter')
	load('custom/util/FileWriter.fs');
	state.FileWriter=FileWriter
}

var modelExporter = (function () {
	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
	// Output dir should end with '\\'
	var _outputDir = 'E:\\application\\noc00\\tlnd\\data\\'
	var _acronimiProdRootDname='gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'
	var _serviziProdRootDname='gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'
	var _acronimiSysTestRootDname='gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations'
	var _serviziSysTestRootDname='gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations'
	// we don't have Produzione/SystemTest root now, it is included in Production
	//var _acronimiProdSysTestRootDname='gen_folder=Acronimi/ISP_FolderBase=Produzione%2FSystemTest/root=Organizations'
	//var _serviziProdSysTestRootDname='gen_folder=Servizi/ISP_FolderBase=Produzione%2FSystemTest/root=Organizations'
	var _serviziMapRootDname='gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services'
	var _genModelServiziRootDname='gen_folder=Servizi/gen_folder=Structure/root=Generational+Models/root=Services'


	var exportAcronimi = function() {
		_logger.info('Starting export acronimo subtree to csv file');

		var myFileWriter = new state.FileWriter();
		myFileWriter.openFile(_outputDir + 'export_acronimi.csv', false);
		// add header as first row
		var header = "'Ambiente';'Acronimo';'DescAcronimo';'ClasseComponente';'NomeCompInfra';'Risorsa';'Server'";
		
		myFileWriter.println(header);

		var visitor =
		{
			visit:function (element) {
				var clazz = element.elementClassname + '';
				_logger.debug('clazz is ' + clazz);
				if (clazz == 'Risorsa') {
					var res = '';
					// Ambiente
					//formula.log.info('Adding ambiente ' + element.parent.parent.parent.parent.name);
					res = (element.parent.parent.parent.parent.parent.name=='Production')?'Produzione':element.parent.parent.parent.parent.parent.name ;
					res = "'" + res +"'";
					res = res + ";";
					// Acronimo
					res = res + "'" + element.parent.parent.parent.name + "'";
					res = res + ";";
					// DescAcronimo
					res = res + "'" + element.Description + "'";
					res = res + ";";
					// Classe Componente Infrastrutturale
					res = res + "'" + element.parent.parent.name + "'";
					res = res + ";";
					// Nome Componente Infrastrutturale
					res = res + "'" + element.parent.name + "'";
					res = res + ";";
					// Risorsa o Elemento Tecnologico
					res = res + "'" + element.name + "'";
					res = res + ";";
					// Server 
					if (typeof element.Server == 'undefined' || element.Server == '') {
						res = res + "'" + "'";
					} else {
						res = res + "'" + element.Server + "'";
					}
					_logger.debug('read element: ' + res);
					

					myFileWriter.println(res);
				}
			}
		}

		
		var acronimiEle = formula.Root.findElement(_acronimiProdRootDname);
		acronimiEle.walk(visitor);
		acronimiEle = formula.Root.findElement(_acronimiSysTestRootDname);
		acronimiEle.walk(visitor);
		myFileWriter.closeFile();

		_logger.info('Export acronimi subtree to csv file ended');
	}


	// export the structure from ServizioGlobalView to Acronimo
	var exportServizi = function() {
		_logger.info('Starting export servizi subtree to csv file');

		var myFileWriter = new state.FileWriter();
		myFileWriter.openFile(_outputDir + 'export_servizi.csv', false);
		// add header as first row
		var header = "'Ambiente';'ServizioGlobalView';'Servizio';'ServizioNomeNoc';'SSA';'Acronimo';'DescAcronimo'";
		myFileWriter.println(header);

		var visitor =
		{
			visit:function (element) {
				var clazz = element.elementClassname + '';
				_logger.debug('clazz is ' + clazz);
				if (clazz == 'Acronimo') {
					var res = '';
					// Ambiente
					//formula.log.info('Adding ambiente ' + element.parent.parent.parent.parent.name);
					res = (element.parent.parent.parent.parent.parent.name=='Production')?'Produzione':element.parent.parent.parent.parent.parent.name ;
					res = "'" + res +"'";
					res = res + ";";
					// ServizioGlobalView
					res = res + "'" + element.parent.parent.parent.name + "'";
					res = res + ";";
					// Servizio, remove ' Tech View' from service name to align to ECM requirements
					res = res + "'" + state.StringUtil.stripTechView(element.parent.parent.name) + "'";
					res = res + ";";
					// Add original Service Name
					res = res + "'" + element.parent.parent.name + "'";
					res = res + ";";
					// SSA
					res = res + "'" + element.parent.name + "'";
					res = res + ";";
					// Acronimo
					res = res + "'" + element.name + "'";
					res = res + ";";
					// DescAcronimo
					res = res + "'" + element.Description + "'";
					
					_logger.debug('read element: ' + res);
					myFileWriter.println(res);
				}
			}
		}

		
		var serviziEle = formula.Root.findElement(_serviziProdRootDname);
		serviziEle.walk(visitor);
		serviziEle = formula.Root.findElement(_serviziSysTestRootDname);
		serviziEle.walk(visitor);
		myFileWriter.closeFile();

		_logger.info('Export servizi subtree to csv file ended');
	}

	// export the structure from Ambiente/ServizioGlobalView to ServizioEcm
	var exportMapServizi = function() {
		_logger.info('Starting export Map Nomi Servizi to csv file');

		var myFileWriter = new state.FileWriter();
		myFileWriter.openFile(_outputDir + 'export_map_nomi_servizi.csv', false);
		// add header as first row
		var header = "'Ambiente';'ServizioGlobalView';'ServizioNomeNoc';'ServizioNomeEcm';'flgEcm'";
		myFileWriter.println(header);

		var visitor =
		{
			visit:function (element) {
				var clazz = element.elementClassname + '';
				_logger.debug('clazz is ' + clazz);
				if (clazz == 'ServizioEcm') {
					var res = '';
					// Ambiente
					//formula.log.info('Adding ambiente ' + element.parent.parent.parent.name);
					res = "'" + element.parent.parent.parent.name + "'";
					res = res + ";";
					// ServizioGlobalView
					res = res + "'" + element.parent.parent.name + "'";
					res = res + ";";
					// Add Service name as known within NOC
					res = res + "'" + element.parent.name + "'";
					res = res + ";";
					// Add Service name as known within ECM, in case it is empty 
					res = res + "'" + element.name + "'";
					res = res + ";";
					//should be derived res = res + "'" + element.parent.flgEcm + "'";
					res = res + '0';
					res = res + ";";

					_logger.debug('read element: ' + res);
					myFileWriter.println(res);
				}
			}
		}

		var serviziEle = formula.Root.findElement( _serviziMapRootDname);
		serviziEle.walk(visitor);
		myFileWriter.closeFile();

		_logger.info('Export servizi subtree to csv file ended');
	}

	doExport = function() {
		exportMapServizi()
		exportAcronimi()
		exportServizi()
	}
	
	return {
		doExport: doExport,
		exportMapServizi: exportMapServizi,
		exportAcronimi: exportAcronimi,
		exportServizi: exportServizi		
	}
})();