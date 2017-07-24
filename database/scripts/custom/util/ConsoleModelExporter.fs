/**
 * Export structure data from console
 *
 * Author: E. Bomitali, Hogwart, 2012
 * Tested with versions: 5.0
 * Version: 1.0
 * Build date: 2 August 2012
 *
 */

load('custom/util/FileWriter.fs');

// Output dir should end with '\\'
var outputDir = 'E:\\APPLICATION\\NOC00\\NovellOperationsCenter\\'
var acronimiRootDname='gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'
var serviziRootDname='gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'

function ConsoleModelExporter() {
};

ConsoleModelExporter.exportAcronimi = function() {
    //logger.info('Starting export acronimo subtree to csv file');

    var myFileWriter = new FileWriter();
    myFileWriter.openFile(outputDir + 'export_acronimi.csv', false);
	// add header as first row
	var header = "'Acronimo';'DescAcronimo';'ClasseComponente';'NomeCompInfra';'Risorsa';'Server'";
	myFileWriter.println(header);

    var visitor =
    {
        visit:function (element) {
            var clazz = element.elementClassname + '';
            //logger.debug('clazz is ' + clazz);
            if (clazz == 'Risorsa') {
                var res = '';
                // Acronimo
                res = "'" + element.parent.parent.parent.name + "'";
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
                //logger.debug('read element: ' + res);

                myFileWriter.println(res);
            }
        }
    }


    var acronimiEle = formula.Root.findElement(acronimiRootDname);
    acronimiEle.walk(visitor);
    myFileWriter.closeFile();

    //logger.info('Export acronimi subtree to csv file ended');
}


// export the structure from ServizioGlobalView to Acronimo
ConsoleModelExporter.exportServizi = function() {
    //logger.info('Starting export servizi subtree to csv file');

    var myFileWriter = new FileWriter();
    myFileWriter.openFile(outputDir + 'export_servizi.csv', false);
	// add header as first row
	var header = "'ServizioGlobalView';'Servizio';'SSA';'Acronimo';'DescAcronimo'";
	myFileWriter.println(header);

    var visitor =
    {
        visit:function (element) {
            var clazz = element.elementClassname + '';
            //logger.debug('clazz is ' + clazz);
			writeln('clazz is ' + clazz);
			writeln('element is ' + element.DName);
            if (clazz == 'Acronimo') {
                var res = '';
                // ServizioGlobalView
                res = "'" + element.parent.parent.parent.name + "'";
                res = res + ";";
				// Service Name (as shown in NOC)
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
				
				writeln('read element: ' + res);
                //logger.debug('read element: ' + res);
                myFileWriter.println(res);
            }
        }
    }


    var serviziEle = formula.Root.findElement(serviziRootDname);
    serviziEle.walk(visitor);
    myFileWriter.closeFile();

    //logger.info('Export servizi subtree to csv file ended');
}

ConsoleModelExporter.doExport = function() {
	ConsoleModelExporter.exportAcronimi()
	ConsoleModelExporter.exportServizi()
}