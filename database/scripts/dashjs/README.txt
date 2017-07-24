A "Dashboard Javascript" operation type is designed to execute custom javascript in the Dashboard. When the operation is invoked for an element or alarm, the following variables are available in javascript scope:

element - A javascript object representing the element for which the element operation is invoked, or the affected element, in the case of an alarm operation.
alarms - An array of alarm objects when an alarm operation is invoked. undefined for element operations.
identity - Identity (encoded dname) of the element for which the operation is performed. This is needed if the Dashboard Javascript operation wants to call a server operation.
alarmsId - A comma delimited list of alarm ids for which the op is performed. This is needed if the Dashboard Javascript operation wants to call a server operations on the alarms.

Sample javascript that displays all the variables that are in scope is available in PrintScopeVariables.js in <NOC_Install_Dir>/database/scripts/dashjs/. Paste the contents of this file into the text area of the operation
definition. 

The primary use case for using a Dashboard Javascript operations is to allow the user to create operations with custom UIs. Since the Dashboard Javascript operation can submit data into a server script operation, this provides
a very powerful mechanism for invoking operations on elements or alarms, creating a custom user interface, as necessary, using the scoped element or alarm to prepopulate some data in the UI, and then subsequently invoke a 
a server script operation to perform additional actions, such an submitting a request into a third party system, starting a new custom workflow, or updating element properties within NOC.

To invoke a server side operation from a Dashboard Javascript operation, use the following javascript: (NOTE: A typical location for this method's call would be in an event handler for the custom UI, where you want to submit data back to
the NOC server.)

NOCOperations.perform({identity:identity, alarmId:alarmId, name:'<Server Side Operation Name>', opParam:'<String parameter for the server script operation.>'});

NOCOperations.perform takes an object as a parameter that must have the following fields.

identity - identity of the element on which to invoke the server operation.
alarmId - an alarm id or a comma delimited list of ids on which to execute the operation. 
Both of the above fields must be specified as shown.
name - The name of the server script operation to invoke.
opParam - A string that contains data that you want to pass as a parameter to the server script operation. This parameter will be available to the server script operation as args[0].

End to End example. 

There are two scripts in <NOC_Install_Dir>/database/scripts/dashjs/ that provide an end to end example of the functionality:
1) Define a Dashboard Javascript operation using the contents of the CreateAlarmsClient.js file for operation definition.
2) Define a Server script operation using the contents of the CreateAlarmsOperation.fs operation. Call it "CreateAlarmsOperation".  Note that for server script operations the file can be imported, using "@dashjs/serverScript.fs" as the contents
of the operation.

Recommendation. To prevent this server script operation from showing up as an option on the element or alarm, check the checkbox that is available for server operations to mark it as hidden. This will still make this operation available, without it showing up on an element's operation menu.

CreateAlarmsClient.js
- The script takes the input the user gives them and formats the data in way that can be interpreted by the function to create the alarm. As a proof of concept and example adapter to use would be the �NOC � Universal Adapter�. Be sure to spell and case the words exactly as they appeared labelled in NOC. 
- The "Submit" button handler 1) processes the fields, 2) formats the fields for proper interpretation by the �CreateAlarmsOperation� operation 3) invokes the hidden "CreateAlarmsOperation" operation.

CreateAlarmsClient.fs
- The script processes and formats the data and creates Alarms for the assigned adapter. Please note that this is function only works for certain adapters, since some have different structure�s and for security reasons, some prevent any alteration or additions to data coming from other sources. 
- opParam must be a string. While the string can contain parameter data in any format, json is a natural choice. To make deserialization easier on the server, we recommend using Gson, since it is already in the classpath.
While arbitrary compex data types are not supported (since you cannot provide your own Java POJO implementations), the provided FormulaScript helper helper object  - formula.util.GsonTypeHelper -
can be used for most common deserialization scenarios:

OBJECT: Javascript objects into Java Map<String, String>
LIST_OF_STRINGS: Javascript arrays of strings into Java List<String>
LIST_OF_OBJECTS: Javascript arrays of objects into Java List<Map<String, String>>

While the above mentioned "CreateAlarmsClient.fs" provides an example of passing data to the server side and having it be functional in NOC, below is a FormulaScript fragment that will demonstrate how to deserialize all three types supported by the formula.util.GsonTypeHelper helper object: (NOTE:  for each type, the json has been provided as an inline string.)

////////////////////
// begin script
////////////////////

//@debug off
Gson = Packages.com.google.gson.Gson;
var gsonObj = new Gson();

// OBJECT Json
var resultStr = "{name: 'customer X', location: 'seattle'}";
var jsObj = gsonObj.fromJson(resultStr, formula.util.GsonTypeHelper.getType(formula.util.GsonTypeHelper.DataFormat.OBJECT)); 
var name = jsObj.get("name");
var location = jsObj.get("location");
writeln("OBJECT name = " + name + ", location = " + location);

// LIST_OF_OBJECTS Json
resultStr = "[{name: 'me', location: 'vienna'}, {name: 'you', location: 'mclean'}, {name: 'them', location: 'houston'}, {name: 'us', location: 'us'}]";
jsObj = gsonObj.fromJson(resultStr, formula.util.GsonTypeHelper.getType(formula.util.GsonTypeHelper.DataFormat.LIST_OF_OBJECTS)); 
writeln("LIST_OF_OBJECTS: size = " + jsObj.size());
for(var i = 0; i < jsObj.size(); i++) {
   var obj = jsObj.get(i);
   var name = obj.get("name");
   var location = obj.get("location");
   writeln("LIST_OF_OBJECTS(" + i + ") name = " + name + ", location = " + location);
}

// LIST_OF_STRINGS Json
resultStr = "['one', 'two', 'three', 'four', 'five']";
jsObj = gsonObj.fromJson(resultStr, formula.util.GsonTypeHelper.getType(formula.util.GsonTypeHelper.DataFormat.LIST_OF_STRINGS)); 
writeln("LIST_OF_STRINGS: size = " + jsObj.size());
for(var i = 0; i < jsObj.size(); i++) {
   var obj = jsObj.get(i);
   writeln("LIST_OF_STRINGS(" + i + ") value = " + obj);

}
////////////////////
// end script
////////////////////

As mentioned above, there is a new checkbox available for "server" type operations.  This box is to allow the user to hide the current operation from the UI, but still make it available from FormulaScript as a remote operation for either a Dashboard or NOC Client.  This checkbox is disabled for all other operation types.


