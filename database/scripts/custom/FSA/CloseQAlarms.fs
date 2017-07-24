/*
This is the event needed to close an alarm

### EVENT ###
originating_event_id=34532;
status=CLOSED;
END
### END EVENT ###
PROCESSED

*/

var myFShost     = "localhost";
var myFSport     = "54355";

function closeAlarms (eleDn) {
	var limitForSendMessageText = 20
	var testo = "";
	var ele = formula.Root.findElement(eleDn);
	var myAlarms = ele.alarms;
	var myString;
	var mySocket;
	var myStream;

    mySocket = new java.net.Socket(myFShost, myFSport);
	myStream = new java.io.DataOutputStream(mySocket.getOutputStream());
	
	for (var i = 0; i < myAlarms.length; i++)
	{
		try
		{
		  var myAlarm = myAlarms[i]
		  var myID = myAlarm.originating_event_id;

		  var myString = NuaUtil.prepField("status", "CLOSED")
		  myString = myString + NuaUtil.prepField("originating_event_id", myID) + '\n'
		  myString = NuaUtil.addEnvelope(myString, myAlarm.Class)
		  myStream.writeBytes(myString);
		  formula.log.info("Wrote " + myString);

		  if (i < limitForSendMessageText)
		  {
			 testo += "\nClosed alarm #" + myID
		  }
		  else
		  {
			 if (i == limitForSendMessageText)
			 {
				testo += "\nmore..."
			 }
		  }
		}
		catch(e)
		{
		  formula.log.error("Exception " + e.toString() + " (i=" + i + ")")
		  testo += "\nGot Exception, see formula.trc"
		}
	}
	
	myStream.flush();
	myStream.close();
	mySocket.close();
}