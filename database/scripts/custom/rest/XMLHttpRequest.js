/* xmlhttp.js -- Simple XMLHttpRequest for Rhino
 * by M.Hiyama (hiyama{at}chimaira{dot}org) 2005
 * URL: http://www.chimaira.org/
 */

function XMLHttpRequest() {
 this.requestURL = "";
 this.responseText = "";
 this.readyState = 0;
 this.status = 0;

 this.open = function(method, url, async) {
  if (method != "GET") {
   throw new Error("This XMLHttpRequest supports GET method only.");
  }
  if (async != false) {
   throw new Error("This XMLHttpRequest cannot perform asynchronous read.");
  }
  this.requestURL = url;
  this.responseText = "";
  this.readyState = 0;
  this.status = 0;
 }
 this.send = function(query) {
  var s;
  try {
   s = readUrl(this.requestURL);// readUrl is Rhino's primitive
   this.status = 200;
  } catch (e) {
   s = "";
   this.status = 1;
  }
  this.responseText = s;
  this.requestURL = null;
  this.readyState = 4;
 }
}

var myRequest = new XMLHttpRequest();
myRequest.open("GET","primo.php");
myRequest.send(null);