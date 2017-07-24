/*
  Script SendQAlarms.fs
*/

load('custom/lib/DBUtil.fs');
load('custom/lib/NuaUtil.fs');
load('custom/orgs/ElementManager.fs');
load('custom/lib/ResultSetHndlr.fs');
load('custom/util/Properties.fs');
load('custom/lib/md5.js');

function getNotNullString(parmSQLString)
{
  if (parmSQLString != null)
  {
     parmSQLString += "";
     if (parmSQLString == "NULL")
        return "";
     else
        return parmSQLString;
  }
  else
  {
     return "";
  }
}

// getJSDateFromSQLTimestamp generates a Date javascript object
function getNotNullTimestamp(parmSQLTimestamp)
{
  if (parmSQLTimestamp != null)
     return DbUtil.getJSDateFromSQLTimestamp(parmSQLTimestamp);
  else
     return new Date;
}

function getTimestampITA(parmSQLTimestamp)
{
  if (parmSQLTimestamp != null)
     return state.util.getItalianTimestamp(DbUtil.getJSDateFromSQLTimestamp(parmSQLTimestamp));
  else
     return "";
}


function sendQAlarms (reportId,user,servizio,ssa,acronimo,classecomponenteinfra,nomecomponenteinfra,nomelementotecno,datada,dataa) {

formula.log.info('sendQAlarms starting');
// load db connection properties from file
var genrep = Properties.loadConfig('genrep');
//var myDBdriver   = "net.sourceforge.jtds.jdbc.Driver";
//var myDBurl      = "jdbc:jtds:sqlserver://PDBCBP001.sede.corp.sanpaoloimi.com:1579/DATIECM";
//var myDBuser     = "noc00_app";
//var myDBpassword = "noc00_ApP@";
//var myFShost     = "papmop108.sede.corp.sanpaoloimi.com";
//var myFSport     = "54335";

var myDBdriver   = genrep.get("dbdriver");
var myDBurl      = genrep.get("dburl");
var myDBuser     = genrep.get("dbuser");
var myDBpassword = genrep.get("dbpassword");
var myFShost     = genrep.get("fshost");
var myFSport     = genrep.get("fsport");
var myClass      = "AlarmFromQuery";

formula.log.info('Connecting to ' + myDBurl + ' and sending to ' + myFShost + ':' + myFSport);

var recCounter = 0;
var myString= "";
var myQuery, myRS, mySocket, myStream;
var bodyStep;
var addAlarmKey = MD5(reportId);

var whereClause = {
	addWhere: true,
	addAnd: false,
	add: function(colName, value, op) {
		formula.log.debug('add ' + colName + ',' + value + ',' + op)
		var res = ""
		formula.log.debug('typeof value ' + typeof value);
		formula.log.debug('myVar instanceof String ' + value instanceof String);
		formula.log.debug('value is not null ' + value != null);
		formula.log.debug('value length ' + value.length);
		if (typeof value != undefined && value != null && value.length > 0) {
			formula.log.debug('value ok, addWhere ' + this.addWhere + ', addAnd ' + this.addAnd);
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
		formula.log.debug('adding clause ' + res)
		return res;
	},
	
	// format 120 for MSSQL aaaa-mm-gg hh:mi:ss (24h)
	addDataFmt120: function(colName, value, op) {
		formula.log.debug('addDataFmt120 ' + colName + ',' + value + ',' + op)
		var res = ""
		if (typeof value != undefined && value != null && value.length > 0) {
			if (this.addAnd == true) {
				res = " AND "
			}
			res = res + colName + " " + op + " CONVERT( DATETIME, '" + value + "', 120) "
			this.addAnd = true;
		}
		formula.log.debug('adding clause ' + res)
		return res;
	}
}

try
{
	var myQuery =
        "SELECT [IDReceivedAlarm],[Ambiente],[Anomalia],[Area_tecnologica],[aree_impattate]"+
        ",[Clienti],[cod_location],[impatto_servizio],[Key_Anomalia],[Livello_architetturale],[mc_owner],[mc_priority]"+
        ",[msg] as Message,[Nodo],[Nome_componente],[Nome_risorsa],[Rilevato],[Servizio_infrastrutturale]" +
	",[severity],[severity] as alarmSeverity" + 
	",[Source],[status],[Ticket_Num],[Tipo_anomalia],[Tipo_componente]"+
        ",[Tipo_risorsa],[mc_date_modification] as Modified,[CELLA_ORIGINE],[event_handle],[Causa],[msg_catalog],[In_manutenzione],[Fuori_servizio]"+
        ",[Servizio],[NomeComponenteServizio],[Acronimo],[DescAcronimo],[ClasseComponenteInfra],[NomeComponenteInfra]"+
        ",[Server],[NomeElementoTecno],[chiave_allarme],[repeat_count] as Repeated,[mc_notes] as Notes,[mc_long_msg] as Detailed_message"+
        ",[Severita_precedente],[mc_ueid] "+
        "FROM [DATIECM].[dbo].[v_alarms] "
	
	myQuery = myQuery + whereClause.add ( "[Servizio]", servizio, "LIKE")
	myQuery = myQuery + whereClause.add ( "[Acronimo]",  acronimo , "IN")
	myQuery = myQuery + whereClause.add ( "[ClasseComponenteInfra]", classecomponenteinfra, "=")
	myQuery = myQuery + whereClause.add ( "[NomeComponenteInfra]", nomecomponenteinfra, "=")
	myQuery = myQuery + whereClause.add ( "[NomeElementoTecno]", nomelementotecno, "=")
	myQuery = myQuery + whereClause.addDataFmt120 ( "[mc_date_modification]", datada, ">=")
	myQuery = myQuery + whereClause.addDataFmt120 ( "[mc_date_modification]", dataa, "<")

	// add order by
	myQuery = myQuery + " order by IDReceivedAlarm asc"
	
	formula.log.info('myQuery=' + myQuery);

    formula.log.debug("FSAFeeder connecting to " + myDBurl);
	bodyStep = 'Connecting';
	var myDB = DbUtil.getDBConnection(myDBdriver, myDBurl, myDBuser, myDBpassword);

    formula.log.debug("FSAFeeder connected");
	bodyStep = 'Querying';
	
	myRS = DbUtil.getSQLResult(myDB, myQuery);

	bodyStep = 'SocketOpen';
	// Open socket to FSAdapter
	formula.log.debug("Opening socket " + myFShost + ":" + myFSport);
	mySocket = new java.net.Socket(myFShost, myFSport);
	myStream = new java.io.DataOutputStream(mySocket.getOutputStream());
	formula.log.debug("Opened");
	// Todo: check if socket and stream has been opened
	
	// Read through ResultSet, build Alarm string, send Alarm to FSAdapter
	// myRS.beforeFirst()

	bodyStep = 'RecordLoop';
    recCounter = 0;
	var currentField = '';
	
	var myRsHr = new RsHndlr(myRS);
	
	formula.log.debug('Got Result Set');
	
	var msgToSend;
	while(myRsHr.nextRow())
	{
		recCounter++;
		formula.log.debug('Looping on ' + recCounter);
        now = new Date();

		myString = "";
		msgToSend = "";
		myString = myString + NuaUtil.prepField("date", now)
		myString = myString + NuaUtil.prepObject(myRsHr.getRow());

		// Wrap fields with the FSA envelope, add class field too
		myString = myString + NuaUtil.prepField("user", user);
		myString = myString + NuaUtil.prepField("reportId", reportId);
        
		// add alarm severity
		myString = myString + + NuaUtil.prepField("severity" , myRsHr.getField('alarmSeverity')) ;
		
		// add alarm key, used in resolving different events into a single alarm
		alarmKey = addAlarmKey + getNotNullString(myRS.getString("chiave_allarme"));
		formula.log.debug("chiave_allarme " + alarmKey);
		myString = myString + NuaUtil.prepField("originating_event_id", alarmKey);
		
        msgToSend  = NuaUtil.addEnvelope(myString, myClass);
		
		formula.log.debug('Sending ' + msgToSend);
		//Send the alarm to the FS Adapter
		myStream.writeBytes(msgToSend);
    }

    formula.log.info('Sent ' + recCounter + ' events');
    bodyStep = 'Closing';
    myRS.close();
	
	DbUtil.disconnect(myDB)
}
catch(mErr)
{
	switch (bodyStep)
	{
		case 'Connecting':
			formula.log.error("Exception while connecting: " + mErr);
		break;
		case 'Querying':
			formula.log.error("Exception while querying: " + mErr);
		break;
		case bodyStep = 'SocketOpen':
			formula.log.error("Exception while opening socket: " + mErr);
		break;
		case 'RecordLoop':
			formula.log.error("REC #" + recCounter + ", Exception " + mErr);
		break;
		case 'Closing':
			formula.log.error("Exception while closing: " + mErr);
		break;
		default:
		formula.log.error("Exception: " + mErr);
	}
}
// Flush and close the socket
myStream.flush();
myStream.close();
delete myStream;
mySocket.close();
delete mySocket;


// Close output data file
formula.log.debug("FSAFeeder Send Element Ended");
}


function sendQAlarmsOnArgs (args) {
	var reportId = state.moment(new Date()).format('YYYY-MM-DD HH:mm:ss');
	var user = args[0] +'';
	var servizio = args[1] +'';
	var ssa = args[2] +'';
	var acronimo = args[3] +'';
	var classecomponenteinfra = args[4] +'';
	var nomecomponenteinfra = args[5] +'';
	var nomeelementotecno = args[6] +'';
	var datada = args[7] +'';
	var dataa = args[8] +'';
	
	dataa = dataa.replace(/24:00:00/,"00:00:00");
	
	formula.log.info('Dumping args. reportId="' + reportId + '" user="' + user + '" servizio="' + servizio + '" ssa="' + ssa + '" acronimo="' + acronimo + '" nomecomponenteinfra="' + nomecomponenteinfra + '" nomeelementotecno="' + nomeelementotecno + '" datada="' +  datada + '" dataa="' + dataa + '"');

	sendQAlarms (reportId,user,servizio,ssa,acronimo,classecomponenteinfra,nomecomponenteinfra,nomeelementotecno,datada,dataa);

}


