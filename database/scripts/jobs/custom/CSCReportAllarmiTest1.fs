formula.log.info("Perform CSC Report Allarmi Test 1")

var user = "test_user"
var servizio = ""
var ssa = ""
var acronimo = "ABCA0"
var classecomponenteinfra = ""
var nomecomponenteinfra = ""
var nomelementotecno = ""
var datada = ""
var dataa = ""


formula.log.info('user ' + user);
formula.log.info('reportId ' + reportId);
formula.log.info('servizio ' + servizio);
formula.log.info('ssa ' + ssa);
formula.log.info('acronimo ' + acronimo);
formula.log.info('classecomponenteinfra ' + classecomponenteinfra);
formula.log.info('nomecomponenteinfra ' + nomecomponenteinfra);
formula.log.info('nomelementotecno ' + nomelementotecno);
formula.log.info('datada ' + datada);
formula.log.info('dataa ' + dataa);

var args = new Array();

// user is already in the hierarchy
args[0] = ""
args[1] = servizio
args[2] = ssa
args[3] = acronimo
args[4] = classecomponenteinfra
args[5] = nomecomponenteinfra
args[6] = nomelementotecno
args[7] = datada
args[8] = dataa

function getReportId(args) {
	var rid = state.moment(new Date()).format('YYYY-MM-DD HH:mm:ss');
    for (var i = 0; i < args.length; i++) {
		if (args[i] && args[i].length > 0) {
			rid = rid + '-' + args[i];
		}
	}
	return rid;
}

var reportId = getReportId(args);
formula.log.info('Generating reportId >' + reportId + '<');

load('custom/FSA/SendQAlarms.fs');
sendQAlarms(reportId,user,servizio,ssa,acronimo,classecomponenteinfra,nomecomponenteinfra,nomelementotecno,datada,dataa)

