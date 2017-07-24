/*

*/

load('util/login.fs');
login('localhost', 8080, 'admin', 'formula')

//formula.log.info('Logged in');

load('custom/lib/moment.js');
load('custom/lib/moment-lang/it.js');

function getReportId (pArgs) {
	var now = moment(new Date()).format('YYYY-MM-DD HH:mm:ss');
	var res = now; //should add a format date to cope with italian format
 
	// skip first par, user
	for(var i = 1; i < pArgs.length; i++) {
		currArg = pArgs[i];
		res = res + '-' + currArg;
	}
	return res
}


/*
var user = args[0];
var servizio = args[1];
var ssa = args[2];
var acronimo = args[3];
var classecomponenteinfra = args[4];
var nomecomponenteinfra = args[5];
var nomelementotecno = args[6];
var datada = args[7];
var dataa = args[8];
*/

var repId = moment(new Date()).format('YYYY-MM-DD HH:mm:ss');
var newArgs = new Array();
newArgs.push(repId);
newArgs.push("admin");
newArgs.push("");
newArgs.push("");
newArgs.push("ABCA0");
newArgs.push("");
newArgs.push("");
newArgs.push("");
newArgs.push("");
newArgs.push("");

//newArgs.push(args);
var ele = formula.Root.findElement('logo_managedobjects=ReportAllarmi/root=Generational+Models/root=Services');
formula.log.info('Launching operation');
ele.perform(session, 'CSC|ReportAllarmi', [], newArgs);
//java.lang.Thread.sleep(7000);
//formula.log.info('Report ' + repId + ' launched');

//sendQAlarms(reportId,user,servizio,ssa,acronimo,classecomponenteinfra,nomecomponenteinfra,nomelementotecno,datada,dataa)
alert('End');












