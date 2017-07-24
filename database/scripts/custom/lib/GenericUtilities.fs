/*
  Script GenericUtilities.fs

  Author: NessPRO Italy - 2006
  Tested with versions : 3.5 to 4.7
  Should work also with: 3.1.0, 3.2, 3.3

  Collection of generic, contextless utilities
*/


////////////////////////////////////////////////////////////////////////////////
function initializeConstants()
// Initialize all constant variables
//
// Please! Use this function if you create a new constant, don't make a mess of
// this script
////////////////////////////////////////////////////////////////////////////////
{
 util.log_getFormulaCondition = formula.log.getInstance("NessPRO.getFormulaCondition");

 // The following sets up an array mapping condition numbers to condition names (es: 1->CRITICAL)
 util.formulaConditionArray = state.prop.getProperty("formulaCondition").toUpperCase().trim().split(";");

 // The following sets up a hashtable mapping condition names to condition numbers (es.: CRITICAL->1)
 util.formulaConditionHash = java.util.Hashtable();
 for (var i = 0; i < util.formulaConditionArray.length; i++)
 {
     util.formulaConditionHash.put(util.formulaConditionArray[i], i);
 }

 // The followings set up some arrays for month names in several flavors
 util.englishLongMonths  = state.prop.getProperty("englishLongMonths").trim().split(";");
 util.englishShortMonths = new Array();
 for (var i = 0; i < util.englishLongMonths.length; i++)
 {
     util.englishShortMonths[i] = util.englishLongMonths[i].substr(0, 3);
 }
 util.italianLongMonths  = state.prop.getProperty("italianLongMonths").trim().split(";");
 util.italianShortMonths = new Array();
 for (var i = 0; i < util.italianLongMonths.length; i++)
 {
     util.italianShortMonths[i] = util.italianLongMonths[i].substr(0, 3);
 }

 // The followings set up some arrays for weekday names in several flavors
 util.englishLongWeekdays  = state.prop.getProperty("englishLongWeekdays").trim().split(";");
 util.englishShortWeekdays = new Array();
 for (var i = 0; i < util.englishLongWeekdays.length; i++)
 {
     util.englishShortWeekdays[i] = util.englishLongWeekdays[i].substr(0, 3);
 }
 util.italianLongWeekdays  = state.prop.getProperty("italianLongWeekdays").trim().split(";");
 util.italianShortWeekdays = new Array();
 for (var i = 0; i < util.italianLongWeekdays.length; i++)
 {
     util.italianShortWeekdays[i] = util.italianLongWeekdays[i].substr(0, 3);
 }

}
// End of function initializeConstants


////////////////////////////////////////////////////////////////////////////////
function getFormulaCondition(fc)
// Convert Formula conditions from/to number/string
// - If 'fc' is a condition number, returns a all-uppercase condition string
//   (1->"CRITICAL"). Returns empty string for invalid number
// - If 'fc' is a condition string, returns a condition number ("Critical"->1):
//   lowercase are admitted, but translations are not supported (es.: INFO for
//   INFORMATIONAL). Returns -1 for invalid string
////////////////////////////////////////////////////////////////////////////////
{
 var returned = null;
 var tmpfc = null;

 try
 {
  var typeof_fc = typeof(fc);
  switch(typeof_fc)
  {
    case "string":
      tmpfc = java.lang.String(fc).toUpperCase().trim();
      if (util.formulaConditionHash.containsKey(tmpfc))
      {
         returned = parseInt(util.formulaConditionHash.get(tmpfc));
      }
      else
      {
         returned = -1;
         util.log_getFormulaCondition.debug("String value '" + fc + "' is not recognizable as a Formula Condition");
      }
      break;
    case "number":
      tmpfc = parseInt(fc);
      if (tmpfc < 0 || tmpfc >= util.formulaConditionArray.length)
      {
         returned = "";
         util.log_getFormulaCondition.debug("Number value " + fc + " is not recognizable as a Formula Condition");
      }
      else
      {
         returned = util.formulaConditionArray[tmpfc] + "";
      }
      break;
    default:
      throw "Invalid parameter type, found [" + typeof_fc + "], string or number expected";
  }
 }
 catch (Exception)
 {
   util.log_getFormulaCondition.error("Exception <" + Exception + ">");
 }

 return returned;
}
// End of function getFormulaCondition


////////////////////////////////////////////////////////////////////////////////
function dumpObjectProps(parmObj, log)
// Dump the structure of the object 'parmObj' on the log instance 'log'.
// Depending on the object structure, it displays APIs, field values, classes,
// usages, etc.
// 'log' is optional, if it is not set a default instance is set up by
// the function itself.
//
// --- KNOWN BUGS AND LIMITATIONS ---
// This function is known as not working properly in some cases (it may cause
// Java errors + stack dump + pop-ups), nevertheless it can be very useful most
// of the times.
// It has also been noticed it may not show Java static methods.
////////////////////////////////////////////////////////////////////////////////
{
 var myTypeof = typeof(parmObj);
 var myClass = null;
 var myName = null;
 var myContent = null;
 var deleteThisLog = false

 if (log == null)
 {
    var log = formula.log.getInstance("NessPRO.dumpObjectProps");
    deleteThisLog = true;
 }

 try
 {
   myClass = (myTypeof == "object") ? parmObj.getClass() : myTypeof;
 }
 catch (Exception)
 {
   myClass = myTypeof;
 }

 var headerMsg = " properties/methods dump of (" + myClass + ") " + parmObj.toString()
 log.info("Begin of" + headerMsg)

 try
 {
   for (myName in parmObj)
   {
       switch(myName)
       {
         case "externalDbSLA" :
           log.info(myName + " - skipped to avoid a known bug")
           break

         default :
           try
           {
             myContent = parmObj[myName];
             myTypeof = typeof(myContent);
             try
             {
               myClass = (myTypeof == "object") ? myContent.getClass() : myTypeof;
             }
             catch (Exception)
             {
               myClass = myTypeof;
             }
             log.info(myName + " = (" + myClass + ") " + myContent);
           }
           catch(Exception)
           {
             log.warn("Exception <" + Exception + "> while trying to access the current sub-object");
           }
           break
       }
   }
 }
 catch (Exception)
 {
   log.warn("Exception <" + Exception + "> while trying to access the object");
 }

 log.info("End of" + headerMsg)

 if (deleteThisLog)
    delete log;
}
// End of function dumpObjectProps


////////////////////////////////////////////////////////////////////////////////
function fromHexToDec(hex)
// Convert a hex number to decimal (example "A2" -> 162)
////////////////////////////////////////////////////////////////////////////////
{
 eval ("dec = 0x" + hex);
 return dec;
}
// End of function fromHexToDec


////////////////////////////////////////////////////////////////////////////////
function fromHexToAsc(hex)
// Convert a hex string to ASCII (example "48454C4C4F" -> "HELLO")
////////////////////////////////////////////////////////////////////////////////
{
 hex += "";
 var ascii = "";
 for (i = 0; i < hex.length - 1; i += 2)
 {
     ascii += String.fromCharCode(util.fromHexToDec(hex.substr(i, 2)));
 }
 return ascii;
}
// End of function fromHexToAsc


////////////////////////////////////////////////////////////////////////////////
function padWithZero(parmNumber, parmPad)
// Align 'parmNumber' to 'parmPad' digits by introducing non-significant zeroes
// Return a string
////////////////////////////////////////////////////////////////////////////////
{
 retString = parmNumber.toString();
 while (retString.length < parmPad)
    retString = "0000000000000000" + retString;
 return retString.substr(retString.length - parmPad);
}
// End of function padWithZero


////////////////////////////////////////////////////////////////////////////////
function getItalianDate(parmTimestamp)
// Return a string with the Date 'parmTimestamp' converted in Italian format
// (DD/MM/YYYY - with leading zeroes, time is dropped)
////////////////////////////////////////////////////////////////////////////////
{
 return util.padWithZero(parmTimestamp.getDate()     , 2) + "/" +
        util.padWithZero(parmTimestamp.getMonth() + 1, 2) + "/" +
        util.padWithZero(parmTimestamp.getFullYear() , 4)
}
// End of function getItalianDate


////////////////////////////////////////////////////////////////////////////////
function getItalianTimestamp(parmTimestamp)
// Return a string with the Date 'parmTimestamp' converted in Italian format
// (DD/MM/YYYY hh:mm:ss - with leading zeroes, time is included)
////////////////////////////////////////////////////////////////////////////////
{
 return util.getItalianDate(parmTimestamp)              + " " +
        util.padWithZero(parmTimestamp.getHours()  , 2) + ":" +
        util.padWithZero(parmTimestamp.getMinutes(), 2) + ":" +
        util.padWithZero(parmTimestamp.getSeconds(), 2)
}
// End of function getItalianTimestamp


////////////////////////////////////////////////////////////////////////////////
function getStdoutFromRuntimeExec(parmCmd)
// Execute the system command 'parmCmd' and return its Stdout
// Useful even in void context (to execute a command not caring about Stdout)
// Return an array
////////////////////////////////////////////////////////////////////////////////
{
 var retArray = new Array();
 try
 {
   var runtime = java.lang.Runtime.getRuntime();
   var process = runtime.exec(parmCmd);
   var stdInput = new java.io.BufferedReader(new java.io.InputStreamReader(process.getInputStream()));
   while ((retArray[retArray.length] = stdInput.readLine()) != null) {}
   retArray.length--;
   stdInput.close();
   delete stdInput;
   delete process;
   delete runtime;
 }
 catch (Exception)
 {
   state.log.error("Exception <" + Exception + "> in getStdoutFromRuntimeExec(" + parmCmd + ")");
 }
 return retArray;
}
// End of function getStdoutFromRuntimeExec


////////////////////////////////////////////////////////////////////////////////
function getContentFromFile(parmFile)
// Return the content of the text file with name 'parmFile'
// Return an array
////////////////////////////////////////////////////////////////////////////////
{
 var retArray = new Array();
 try
 {
   var myFile = java.io.File(parmFile);
   var myInput = java.io.RandomAccessFile(myFile,"r");
   while ((retArray[retArray.length] = myInput.readLine()) != null) {}
   retArray.length--;
   myInput.close();
   delete myInput;
   delete myFile;
 }
 catch (Exception)
 {
   state.log.error("Exception <" + Exception + "> in getContentFromFile(" + parmFile + ")");
 }
 return retArray;
}
// End of function getContentFromFile


////////////////////////////////////////////////////////////////////////////////
function getContentFromURL(parmURL)
// Return the content of the text file with URL string 'parmURL'
// Tested with the following protocols:
// HTTP:     getContentFromURL("http://Server[:Port]/file")
// HTTPS:    getContentFromURL("https://Server[:Port]/file")
// FTP:      getContentFromURL("ftp://user:password@Server[:Port]/file")
// Return an array
////////////////////////////////////////////////////////////////////////////////
{
 var retArray = new Array();
 try
 {
   var myURL = new java.net.URL(parmURL);
   var myURLConnection = myURL.openConnection();
   var myInput = new java.io.BufferedReader(new java.io.InputStreamReader(myURLConnection.getInputStream()));
   while ((retArray[retArray.length] = myInput.readLine()) != null) {}
   retArray.length--;
   myInput.close();
   delete myInput;
   delete myURLConnection;
   delete myURL;
 }
 catch (Exception)
 {
   state.log.error("Exception <" + Exception + "> in getContentFromURL(" + parmURL + ")");
 }
 return retArray;
}
// End of function getContentFromURL


////////////////////////////////////////////////////////////////////////////////
function getFTPDir(parmURL)
// Return the listing of a FTP directory.
// The argument parmURL must be provided as for function getContentFromURL, just
// append a closing slash (es "ftp://user:password@Server/theDir/")
// Return an 3-entry array whose entries are themselves arrays
// - array[0] -> array of file names
// - array[1] -> array of directory names
// - array[2] -> array of other names (prepended with a "<flag>,", where <flag>
//               is the 1st char in the 'dir' output)
////////////////////////////////////////////////////////////////////////////////
{
 var retFileArray = new Array()
 var retDirArray = new Array()
 var retOtherArray = new Array()
 try
 {
   // The following RegExps match a line returned by a ftp 'dir' command
   ftp_unix_re = new RegExp("(.).{9}\\s+\\d+\\s+\\S+\\s+\\S+\\s+\\d+\\s+\\w{3}\\s{1,2}\\d{1,2}\\s(?:\\s\\d{4}|\\s?\\d{1,2}:\\d{2})\\s(.+)")
   ftp_microsoft_re = new RegExp("\\d{2}-\\d{2}-\\d{2}\\s{2}\\d{2}:\\d{2}[AP]M\\s{7}(<DIR>|\\s{5})[\\s\\d]{9}\\s(.+)")

   var myContent = getContentFromURL(parmURL)
   for (var i = 0; i < myContent.length; i++)
   {
       if (ftp_unix_re.test(myContent[i]))
       {
          ftp_unix_re.exec(myContent[i])
          switch(RegExp.$1)
          {
            case "-" :
              retFileArray[retFileArray.length] = RegExp.$2
              break
            case "d" :
              retDirArray[retDirArray.length] = RegExp.$2
              break
            default :
              retOtherArray[retOtherArray.length] = RegExp.$1 + "," + RegExp.$2
          }
       }
       else
       {
          if (ftp_microsoft_re.test(myContent[i]))
          {
             ftp_microsoft_re.exec(myContent[i])
             switch(RegExp.$1)
             {
               case "     " :
                 retFileArray[retFileArray.length] = RegExp.$2
                 break
               case "<DIR>" :
                 retDirArray[retDirArray.length] = RegExp.$2
                 break
               default :
                 retOtherArray[retOtherArray.length] = RegExp.$1 + "," + RegExp.$2
             }
          }
       }
   }
   delete myContent
   delete ftp_unix_re
   delete ftp_microsoft_re
 }
 catch (Exception)
 {
   state.log.error("Exception <" + Exception + "> in getFTPDir(" + parmURL + ")");
 }
 return new Array(retFileArray, retDirArray, retOtherArray);
}
// End of function getFTPDir


////////////////////////////////////////////////////////////////////////////////
function toTCPSocket(parmHost, parmPort, parmMsg)
// Send the string 'parmMsg' to the TCP listener on parmHost:parmPort
// Return: 0->OK 1->Error
////////////////////////////////////////////////////////////////////////////////
{
 try
 {
   var mySocket = new java.net.Socket(parmHost, parmPort);
   var myStream = new java.io.DataOutputStream(mySocket.getOutputStream());
   myStream.writeBytes(parmMsg);
   myStream.flush();
   myStream.close();
   delete myStream;
   mySocket.close();
   delete mySocket;
   retCode = 0;
 }
 catch(Exception) { retCode = 1; }
 return retCode;
}
// End of function toTCPSocket


////////////////////////////////////////////////////////////////////////////////
function walkElement(parmElement, parmRE, parmFlagIncludeRoot)
// Recursively crosses all the children of the Element parmElement, checks if
// dnames comply with the regular expression parmRE, returns the array of all
// the compliant children.
// parmRE is optional, its default is ".*" (matches any string).
// The optional parmFlagIncludeRoot (true/false, default=true) is used to
// include/exclude parmElement in the returned array.
// The returned array contains Elements (class com.mosol.Adapter.Formula.Org).
// Remind: to get an Element from its dname, use formula.Root.findElement(dname)
////////////////////////////////////////////////////////////////////////////////
{
  var returned = new Array()
  if (parmFlagIncludeRoot == null) { parmFlagIncludeRoot = true }
  if (parmRE              == null) { parmRE              = new RegExp(".*") }
  try
  {
    if (parmFlagIncludeRoot && parmRE.test(parmElement.dname))
    {
       returned[returned.length] = parmElement
    }
    for (var i = 0; i < parmElement.children.length; i++)
    {
        var myTemp = state.util.walkElement(parmElement.children[i], parmRE, true)
        // Note: the method returned.concat(myTemp) does not work, the next 'for' emulates it
        for (var j = 0; j < myTemp.length; returned[returned.length] = myTemp[j++]) {}
        delete myTemp
    }
  }
  catch(e)
  {
    var msg = "walkElement() caugth exception '" + e.toString() + "' while retrieving children for " + parmElement.dname
    state.log.error(msg)
    throw msg
  }
  return returned
}
// End of function walkElement


//////////////////////////////////  M A I N  ///////////////////////////////////

var util = new Object();
initializeConstants();

// Append all functions but 'initializeConstants' to the object util
util.getFormulaCondition      = getFormulaCondition;
util.dumpObjectProps          = dumpObjectProps;
util.fromHexToDec             = fromHexToDec;
util.fromHexToAsc             = fromHexToAsc;
util.padWithZero              = padWithZero;
util.getItalianDate           = getItalianDate;
util.getItalianTimestamp      = getItalianTimestamp;
util.getStdoutFromRuntimeExec = getStdoutFromRuntimeExec;
util.getContentFromFile       = getContentFromFile;
util.getContentFromURL        = getContentFromURL;
util.getFTPDir                = getFTPDir;
util.toTCPSocket              = toTCPSocket;
util.walkElement              = walkElement;

// Set up a generic, ready for use, log instance
if (state.log == null)
   state.log = formula.log.getInstance("NessPRO.Generic");
