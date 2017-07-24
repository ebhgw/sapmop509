/*
  Script AlarmReport.fs
  
  Author: E. Bomitali - Hogwart - 2013
  Tested with versions: 5

  Launch talend job to generate report
*/


load('custom/lib/ExtCmdWrapper.fs');
//load('custom/lib/execCmd.fs');
load('custom/lib/moment.js');

var logger = Packages.org.apache.log4j.Logger.getLogger("fs");

function getWhereClause (servizio, acronimo, classecomponenteinfra, nomecomponenteinfra, nomelementotecno, datada, dataa) {

	var whereClause = {
		addWhere: true,
		addAnd: false,
		add: function(colName, value, op) {
			//logger.info('add ' + colName + ',' + value + ',' + op)
			var res = ""
			//logger.info('typeof value ' + typeof value);
			//logger.info('myVar instanceof String ' + value instanceof String);
			//logger.info('value is not null ' + value != null);
			//logger.info('value length ' + value.length);
			if (typeof value != "undefined" && value != null && value.length > 0) {
				//logger.info('value ok, addWhere ' + this.addWhere + ', addAnd ' + this.addAnd);
				if (this.addWhere == true) {
					res = " WHERE ";
					this.addWhere = false;
				}
				if (this.addAnd == true) {
					res = " AND "
				}
				if (op == "IN")
					res = res + colName + " " + op + " ( " + value + " ) ";
				else
					res = res + colName + " " + op + " '" + value + "' ";
				this.addAnd = true;
			}
			//logger.info('adding clause ' + res)
			return res;
		},
		
		// format 120 for MSSQL aaaa-mm-gg hh:mi:ss (24h)
		addDataFmt120: function(colName, value, op) {
			//logger.debug('addDataFmt120 ' + colName + ',' + value + ',' + op)
			var res = ""
			if (typeof value != undefined && value != null && value.length > 0) {
				if (this.addAnd == true) {
					res = " AND "
				}
				res = res + colName + " " + op + " CONVERT( DATETIME, '" + value + "', 120) "
				this.addAnd = true;
			}
			//logger.info('adding clause ' + res)
			return res;
		}
	}

	var myWhere = '';
	
	myWhere = myWhere + whereClause.add ( "[Servizio]", servizio, "LIKE")
	myWhere = myWhere + whereClause.add ( "[Acronimo]",  acronimo , "IN")
	myWhere = myWhere + whereClause.add ( "[ClasseComponenteInfra]", classecomponenteinfra, "=")
	myWhere = myWhere + whereClause.add ( "[NomeComponenteInfra]", nomecomponenteinfra, "=")
	myWhere = myWhere + whereClause.add ( "[NomeElementoTecno]", nomelementotecno, "=")
	myWhere = myWhere + whereClause.addDataFmt120 ( "[Rilevato]", datada, ">=")
	myWhere = myWhere + whereClause.addDataFmt120 ( "[Rilevato]", dataa, "<")
	//logger.info('Returning myWhere ' + myWhere);
	return myWhere;
}


function AlarmReport () {
}

AlarmReport.checkOutput = function(output) {
	var res = '';
	var matched = null;
	var resOkPatt = /Job OK\. Report:(\w*)\|\|Lines:(\d*)/im;
	var resKoPatt = /(Job KO\. Error reading or writing alarm data)/im;
	var excpKoPatt = /Exception in(.*)\n/im;

	matched = resOkPatt.exec(output);
	
	if (matched !== null) {
		logger.info('matched ok');
		res = 'Job OK. ' + matched[0];
		logger.info('res ' + res);
		return res;
	}
	
	matched = resKoPatt.exec(output)
	if (matched !== null) {
		logger.info('matched ko');
		res = 'Job KO, see report.log. ' + matched[0];
		logger.info('res ' + res);
		return res;
	}
	
	matched = excpKoPatt.exec(output);
	if (matched !== null) {
		logger.info('matched excp');
		res = 'Job KO, see report.log. ' + matched[0];
		logger.info('res ' + res);
		return res;
	}
	
	return 'Unknown result. See report.log';
}


function filterJobOutcome (str) {
	var res = '';
	var matched = null;
	var tlndPatt = /AlarmReport::.*\n/;
	var excpKoPatt = /Exception in(.*)\n/;

	matched = tlndPatt.exec(str);
	
	if (matched !== null) {
		res = matched[0];
		logger.info('res ' + res);
		return res;
	}
	
	matched = excpKoPatt.exec(str);
	if (matched !== null) {
		res = 'AlarmReport::KO::Message::See report.log. ' + matched[0];
		logger.info('res ' + res);
		return res;
	}
	
	return 'AlarmReport::KO::Message::Unknown result. See report.log';
}

// moment returns a date whatever the string. ex 2013/15 -> 2014/03
AlarmReport.checkDate = function ( str ) {
	return moment(str).format('YYYY/MM/DD HH:mm:ss');
}

AlarmReport.generate = function () {
	var res = '';
	try {
		logger.info('AlarmReport.generate job starting');

		var user = args[0] + '';
		var servizio = args[1] + '';
		var ssa = args[2] + '';
		var acronimo = args[3] + '';
		var classecomponenteinfra = args[4] + '';
		var nomecomponenteinfra = args[5] + '';
		var nomelementotecno = args[6] + '';
		var datada = AlarmReport.checkDate(args[7] + '');
		var dataa = AlarmReport.checkDate(args[8] + '');
		logger.info('call op with ' + user + ',' + servizio + ','+ ssa + ',' 
		   + acronimo + ','+ classecomponenteinfra + ','+ nomecomponenteinfra + ','
		   + nomelementotecno + ','+ datada + ',' + dataa)
		var repName = moment(new Date()).format('YYYYMMDD_HHmmss');
		
		// Should end with '\\'
		var cmdDir = java.lang.System.getProperty("formula.home") + '\\..\\..\\tlnd\\jobs\\'
		var myWhereClause = getWhereClause (servizio, acronimo, classecomponenteinfra, nomecomponenteinfra, nomelementotecno, datada, dataa)
		logger.info('myWhereClause = ' + myWhereClause);
		var cmd;
		
		cmd = cmdDir + 'LoadAlarmReport\\LoadAlarmReport_run.bat' + ' --context_param repUser="' + user + '"  --context_param repName="' + repName + '" --context_param where_clause="' +  myWhereClause + '"' + ' 2>&1 ';
		logger.info('Sending cmd ' + cmd);
		res = ExtCmdWrapper.runWithFilteredRes(cmd, filterJobOutcome);
		writeln(res);
	} catch (excp) {
		logger.info('AlarmReport::KO::Message::'+excp);
	}
	return res;
}

// MAIN
AlarmReport.generate(args);