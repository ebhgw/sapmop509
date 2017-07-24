//@debug off

String = Packages.java.lang.String;
List = Packages.java.util.List;
Map = Packages.java.util.Map;
Gson = Packages.com.google.gson.Gson;
GsonTypeHelper = formula.util.GsonTypeHelper;
var resultStr = args[0];
var str = resultStr.split(";");
var mgrElement = formula.Root.findElement("script=Adapter%3A+" + str[0] +"/root=Elements");
var adapter = mgrElement.adapter;
var fields = ["Summary","Class","severity","Description"];
var Vals = [str[3], str[2], str[1], str[4]];
adapter.createAlarm(str[2], fields, Vals, null );