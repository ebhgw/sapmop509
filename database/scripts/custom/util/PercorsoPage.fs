/*  Dump data to file
    
        LISPA/ContactCenter/Util/DataReader.fs
        Version 1.0
        E. Bomitali

        Updates:
        Ver    	Author          Description
        1.0    	E. Bomitali    Initial Release
		
	Legge l'elemento

*/

var myLog = Packages.org.apache.log4j.Logger.getLogger("dev");

function PercorsoPage () {
}

PercorsoPage.update = function(element) {
	
	var clazz = element.elementClassName + "";
	if (clazz != 'Risorsa') {
		myLog.info('Invoked on element ' + element.name + ' of class ' + clazz + '. Nothing to do !');
		return;
	}
	myLog.info('Invoked on element ' + element.name + ' of class ' + clazz);
	//myLog.info('element.parent.parent.parent.parent.parent.name ' + element.parent.parent.parent.parent.parent.name);
	element.Servizio = element.parent.parent.parent.parent.parent.name;
	//myLog.info('element.Servizio ' + element.Servizio);
	element.SSA = element.parent.parent.parent.parent.name;
	element.Acronimo = element.parent.parent.parent.name;
	element.ClasseCompInfra = element.parent.parent.name;
	element.CompInfra = element.parent.name;
	element.Risorsa = element.name;
	element.Server = element.parent.name.toLowerCase();
	element.ChiaveAllarme = element.Server + '_' + element.name;
}

