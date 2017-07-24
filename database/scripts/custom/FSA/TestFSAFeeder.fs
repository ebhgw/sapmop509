/*
  Script FSAFeeder.fs
*/


var myLog = Packages.org.apache.log4j.Logger.getLogger("Dev");
myLog.info("FSAFeeder Send Element Starting");

if (typeof state.FSAFeeder == 'undefined') state.FSAFeeder  = new Object();

load('custom/lib/DBUtil.fs');
load('custom/lib/FSAUtil.fs');


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
     return state.dbutil.getJSDateFromSQLTimestamp(parmSQLTimestamp);
  else
     return new Date;
}

function getTimestampITA(parmSQLTimestamp)
{
  if (parmSQLTimestamp != null)
     return state.util.getItalianTimestamp(state.dbutil.getJSDateFromSQLTimestamp(parmSQLTimestamp));
  else
     return "";
}



////////////////////////////////////////////////////////////////////////////////
// aggiunta in FSAUtil
function numericField(parmKey, parmValue)
// Return a key/value pair as a FSA field
// Es. field("Key", "Value") returns the string 'Key=Value;'
// _BUT_ if key="class" returns only 'Value;'
////////////////////////////////////////////////////////////////////////////////
{
  if (parmKey == "class")
     return parmValue + ";"
  else
     return parmKey + "=" + parmValue + ";"
}
// End of function


//////////////////////////////////////////////////////////////////////////
//                         MAIN
//////////////////////////////////////////////////////////////////////////

var myDBdriver   = "net.sourceforge.jtds.jdbc.Driver";
var myDBurl      = "jdbc:jtds:sqlserver://localhost:1433/ABCTest";
var myDBuser     = "formula";
var myDBpassword = "formula";
var myFShost     = "localhost";
var myFSport     = "34567";
var myClass      = "FSAFeeder";

var recCounter = 0;
var prevParsedDate = 0, pprevParsedDate = 0;
var currDate, currParsedDate, tmpDate;
var myString= "";
var myQuery, myRS, mySocket, myStream;
var bodyStep;


try
{

    //myLog.info("FSAFeeder connecting");
	bodyStep = 'Connecting';
	var myDB = state.dbutil.getDBConnection(myDBdriver, myDBurl, myDBuser, myDBpassword);

    //myLog.info("FSAFeeder connected");
	bodyStep = 'Querying';
	var myQuery =
        "SELECT IDReceivedAlarm, status as Status, Servizio, NomeComponenteServizio as SSA, Acronimo, DescAcronimo, ClasseComponenteInfra as ClasseCompInfra," +
              " NomeComponenteInfra as CompInfra, NomeElementoTecno as Risorsa, severity, Server, chiave_allarme as ChiaveAllarme " +
        " FROM view_allarmi_out al " +
		"where al.IDReceivedAlarm = 16582";
	
	myLog.info('myQuery=' + myQuery);
	myRS = state.dbutil.getSQLResult(myDB, myQuery);

	bodyStep = 'SocketOpen';
	// Open socket to FSAdapter
	//myLog.debug("Opening socket " + myFShost + ":" + myFSport);
	mySocket = new java.net.Socket(myFShost, myFSport);
	myStream = new java.io.DataOutputStream(mySocket.getOutputStream());

	// Read through ResultSet, build Alarm string, send Alarm to FSAdapter
	// myRS.beforeFirst()

	bodyStep = 'RecordLoop';
    var now, idreceivedalarm, maxid = 0;
	while(myRS.next())
	{
        myLog.info("Looping step 1 init loop");
		recCounter++;
        now = new Date();

		myString = ""
		myString += state.fsautil.field("date", now)
        myLog.info("Looping step 2");
        idreceivedalarm = parseInt(getNotNullString(myRS.getString("IDReceivedAlarm")));
		chiaveAllarme = getNotNullString(myRS.getString("ChiaveAllarme"));
        myLog.info("idreceivedalarm " + idreceivedalarm);

        myString += state.fsautil.field("IDReceivedAlarm", idreceivedalarm);
        myString += state.fsautil.field("Status", getNotNullString(myRS.getString("Status")));
		myString += state.fsautil.field("ChiaveAllarme", chiaveAllarme);
        myString += state.fsautil.field("Servizio", getNotNullString(myRS.getString("Servizio")));
        myString += state.fsautil.field("SSA", getNotNullString(myRS.getString("SSA")));
        myString += state.fsautil.field("Acronimo", getNotNullString(myRS.getString("Acronimo")));
		myString += state.fsautil.field("Descrizione", getNotNullString(myRS.getString("DescAcronimo")));
        myString += state.fsautil.field("ClasseCompInfra", getNotNullString(myRS.getString("ClasseCompInfra")));
        myString += state.fsautil.field("CompInfra", getNotNullString(myRS.getString("CompInfra")));
        myString += state.fsautil.field("Risorsa", getNotNullString(myRS.getString("Risorsa")));
		myString += state.fsautil.field("Server", getNotNullString(myRS.getString("Server")));

		// Wrap fields with the FSA envelope, add class field too
        myLog.info("Looping step 3");
        myString = myString + state.fsautil.field("originating_event_id", chiaveAllarme);
        myString += state.fsautil.field("severity" , "INFO") ;
        myString  = state.fsautil.addEnvelope(myString, myClass);
		//Send the alarm to the FS Adapter
		myStream.writeBytes(myString);
        myLog.info("Looping step 4 end loop");
    }

    bodyStep = 'Closing';
    myRS.close();
	
	state.dbutil.disconnect(myDB)
}
catch(mErr)
{
	switch (bodyStep)
	{
		case 'Connecting':
			myLog.error("Exception while connecting: " + mErr);
		break;
		case 'Querying':
			myLog.error("Exception while querying: " + mErr);
		break;
		case bodyStep = 'SocketOpen':
			myLog.error("Exception while opening socket: " + mErr);
		break;
		case 'RecordLoop':
			myLog.error("REC #" + recCounter + ", Exception " + mErr);
		break;
		case 'Closing':
			myLog.error("Exception while closing: " + mErr);
		break;
		default:
		myLog.error("Exception: " + mErr);
	}
}

// Flush and close the socket
myStream.flush();
myStream.close();
delete myStream;
mySocket.close();
delete mySocket;

// Close output data file
myLog.info("FSAFeeder Send Element Ended");
