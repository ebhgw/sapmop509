/*
  Script FSAQAlarms.fs
*/

formula.log.info("FSAFeeder Send Element Starting");
var user = args[0]
var servizio = args[1]
var ssa = args[2]
var acronimo = args[3]
var classecomponenteinfra = args[4]
var nomecomponenteinfra = args[5]
var nomelementotecno = args[6]
var datada = args[7]
var dataa = args[8]
formula.log.info('call fsa ' + user + ',' + servizio + ','+ ssa + ',' 
   + acronimo + ','+ classecomponenteinfra + ','+ nomecomponenteinfra + ','
   + nomelementotecno + ','+ datada + ',' + dataa)


load('custom/lib/DBUtil.fs');
load('custom/lib/NuaUtil.fs');


function ltrim(str) {
	for(var k = 0; k < str.length && isWhitespace(str.charAt(k)); k++);
	return str.substring(k, str.length);
}
function rtrim(str) {
	for(var j=str.length-1; j>=0 && isWhitespace(str.charAt(j)) ; j--) ;
	return str.substring(0,j+1);
}
function trim(str) {
	return ltrim(rtrim(str));
}
function isWhitespace(charToCheck) {
	var whitespaceChars = " \t\n\r\f";
	return (whitespaceChars.indexOf(charToCheck) != -1);
}

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


//////////////////////////////////////////////////////////////////////////
//                         MAIN
//////////////////////////////////////////////////////////////////////////

formula.log.info('FSA Feeder starting')
var myDBdriver   = "net.sourceforge.jtds.jdbc.Driver";
var myDBurl      = "jdbc:jtds:sqlserver://localhost:1433/DATIECM";
var myDBuser     = "noc00_app";
var myDBpassword = "noc00_app";
var myDBhost     = "localhost";
var myFShost     = "localhost";
var myFSport     = "54355";
var myClass      = "AlarmFromQuery";

var recCounter = 0;
var myString= "";
var myQuery, myRS, mySocket, myStream;
var bodyStep;

var whereClause = {
	addAnd: false,
	add: function(colName, value, op) {
		formula.log.info('add ' + colName + ',' + value + ',' + op)
		var res = ""
		if (typeof value != undefined && value != null && value.length > 0) {
			if (addAnd == true) {
				res = " AND "
			}
			res = res + colName + " " + op + " '" + value + "' "
			addAnd = true;
		}
		formula.log.info('adding clause ' + res)
		return res;
	},
	
	// format 120 for MSSQL aaaa-mm-gg hh:mi:ss (24h)
	addDataFmt120: function(colName, value, op) {
		formula.log.info('addDataFmt120 ' + colName + ',' + value + ',' + op)
		var res = ""
		if (typeof value != undefined && value != null && value.length > 0) {
			if (addAnd == true) {
				res = " AND "
			}
			res = res + colName + " " + op + " CONVERT( DATETIME, '" + value + "', 120) "
			addAnd = true;
		}
		formula.log.info('adding clause ' + res)
		return res;
	}
}

try
{
var myQuery =
        "SELECT [IDReceivedAlarm],[IDExport],[Ambiente],[Anomalia],[Area_tecnologica],[aree_impattate]"+
        ",[Clienti],[cod_location],[impatto_servizio],[Key_Anomalia],[Livello_architetturale],[mc_owner],[mc_priority]"+
         ",[msg],[Nodo],[Nome_componente],[Nome_risorsa],[Rilevato],[Servizio_infrastrutturale],[severity],[severityN]"+
         ",[orig_severity],[Source],[status],[isStatusOpen],[isStatusACKorASS],[Ticket_Num],[hasTicket],[Tipo_anomalia],[Tipo_componente]"+
         ",[Tipo_risorsa],[mc_date_modification],[CELLA_ORIGINE],[event_handle],[Causa],[msg_catalog],[In_manutenzione],[Fuori_servizio]"+
         ",[Servizio],[NomeComponenteServizio],[Acronimo],[DescAcronimo],[ClasseComponenteInfra],[NomeComponenteInfra]"+
        ",[Server],[NomeElementoTecno],[chiave_allarme],[affected_element_name],[repeat_count],[mc_notes],[mc_long_msg]"+
        ",[Severita_precedente],[ExportDt],[mc_ueid] "+
        "FROM [DATIECM].[dbo].[v_alarms] "
	var addAnd = 0;
	if (args.length > 1) {
		myQuery = myQuery + " WHERE "
		myQuery = myQuery + whereClause.add ( "[Servizio]", servizio, "=")
		myQuery = myQuery + whereClause.add ( "[NomeComponenteServizio]", ssa, "=")
		myQuery = myQuery + whereClause.add ( "[Acronimo]", acronimo, "=")
		myQuery = myQuery + whereClause.add ( "[ClasseComponenteInfra]", classecomponenteinfra, "=")
		myQuery = myQuery + whereClause.add ( "[NomeComponenteInfra]", nomecomponenteinfra, "=")
		myQuery = myQuery + whereClause.add ( "[NomeElementoTecno]", nomelementotecno, "=")
		myQuery = myQuery + whereClause.addDataFmt120 ( "[mc_date_modification]", datada, ">")
		myQuery = myQuery + whereClause.addDataFmt120 ( "[mc_date_modification]", dataa, "<")
	}
	
	formula.log.info('myQuery=' + myQuery);

    formula.log.info("FSAFeeder connecting to " + myDBurl);
	bodyStep = 'Connecting';
	var myDB = DbUtil.getDBConnection(myDBdriver, myDBurl, myDBuser, myDBpassword);

    formula.log.info("FSAFeeder connected");
	bodyStep = 'Querying';
	
	myRS = DbUtil.getSQLResult(myDB, myQuery);

	bodyStep = 'SocketOpen';
	// Open socket to FSAdapter
	formula.log.info("Opening socket " + myFShost + ":" + myFSport);
	mySocket = new java.net.Socket(myFShost, myFSport);
	myStream = new java.io.DataOutputStream(mySocket.getOutputStream());
	formula.log.info("Opened");
	// Read through ResultSet, build Alarm string, send Alarm to FSAdapter
	// myRS.beforeFirst()

	bodyStep = 'RecordLoop';
    recCounter = 0;
	var currentField = '';
	
	while(myRS.next())
	{
        formula.log.debug("Looping on record " + recCounter);
		recCounter++;
        now = new Date();

		myString = ""
		
		myString += NuaUtil.prepField("date", now)
        idreceivedalarm = parseInt(getNotNullString(myRS.getString("IDReceivedAlarm")));
        formula.log.debug("idreceivedalarm " + idreceivedalarm);
        myString += NuaUtil.prepField("IDReceivedAlarm", idreceivedalarm);
		
		currentField = getNotNullString(myRS.getString("status"));
		formula.log.debug("status " + currentField);
        myString += NuaUtil.prepField("status", currentField);

		alarmKey = getNotNullString(myRS.getString("chiave_allarme"));
		formula.log.debug("chiave_allarme " + alarmKey);
		myString = myString + NuaUtil.prepField("originating_event_id", alarmKey);
		myString += NuaUtil.prepField("chiave_allarme", alarmKey);
		
		currentField = getNotNullString(myRS.getString("Servizio"));
		formula.log.debug("Servizio " + currentField);
        myString += NuaUtil.prepField("Servizio", currentField);
				
        currentField = getNotNullString(myRS.getString("NomeComponenteServizio"));		formula.log.debug("NomeComponenteServizio " + currentField);
		myString += NuaUtil.prepField("NomeComponenteServizio", currentField);
		
        currentField = getNotNullString(myRS.getString("Acronimo"));
		formula.log.debug("Acronimo " + currentField);
		myString += NuaUtil.prepField("Acronimo", currentField);
		
		currentField = getNotNullString(myRS.getString("DescAcronimo"));
		formula.log.debug("DescAcronimo " + currentField);
		myString += NuaUtil.prepField("DescAcronimo", currentField);
		
        currentField = getNotNullString(myRS.getString("ClasseComponenteInfra"));
		formula.log.debug("ClasseComponenteInfra " + currentField);
		myString += NuaUtil.prepField("ClasseComponenteInfra", currentField);
		
        currentField = getNotNullString(myRS.getString("NomeComponenteInfra"));
		formula.log.debug("NomeComponenteInfra " + currentField);
		myString += NuaUtil.prepField("NomeComponenteInfra", currentField);
		
        currentField = getNotNullString(myRS.getString("NomeElementoTecno"));
		formula.log.debug("NomeElementoTecno " + currentField);
		myString += NuaUtil.prepField("NomeElementoTecno", currentField);
		
		currentField = getNotNullString(myRS.getString("Server"));
		formula.log.debug("Server " + currentField);
		myString += NuaUtil.prepField("Server", currentField);
		
		currentField = getNotNullString(myRS.getString("msg"));
		formula.log.debug("msg " + currentField);
		myString += NuaUtil.prepField("msg", currentField);
		
		currentField = getNotNullString(myRS.getString("mc_date_modification"));
		formula.log.debug("mc_date_modification " + currentField);
		myString += NuaUtil.prepField("mc_date_modification", currentField);
		
		currentField = getNotNullString(myRS.getString("mc_long_msg"));
		formula.log.debug("mc_long_msg " + currentField);
		myString += NuaUtil.prepField("mc_long_msg", currentField);
		
		currentField = getNotNullString(myRS.getString("mc_notes"));
		formula.log.debug("mc_notes " + currentField);
		myString += NuaUtil.prepField("mc_notes", currentField);
		
		currentField = getNotNullString(myRS.getString("repeat_count"));
		formula.log.debug("repeat_count " + currentField);
		myString += NuaUtil.prepField("repeat_count", currentField);
		
		currentField = getNotNullString(myRS.getString("mc_ueid"));
		formula.log.debug("mc_ueid " + currentField);
		myString += NuaUtil.prepField("mc_ueid", currentField);
		currentField = getNotNullString(myRS.getString("orig_severity"));
		formula.log.debug("orig_severity " + currentField);
		myString += NuaUtil.prepField("orig_severity", currentField);
		
		currentField = getNotNullString(myRS.getString("severity"));
		formula.log.debug("severity " + currentField);
		myString += NuaUtil.prepField("severity", currentField);

		// Wrap fields with the FSA envelope, add class field too
        formula.log.debug("Looping step 3");
		myString = myString + NuaUtil.prepField("user", user);
        
        myString  = NuaUtil.addEnvelope(myString, myClass);
		//Send the alarm to the FS Adapter
		myStream.writeBytes(myString);
        formula.log.debug("Looping step 4 end loop");
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
formula.log.info("FSAFeeder Send Element Ended");
