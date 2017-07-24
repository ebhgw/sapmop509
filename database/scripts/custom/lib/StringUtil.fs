/*
  Script StringUtil.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  String functions

*/

// overwrites native function if present, so remove it in case
String.prototype.trim=function(){return this.replace(/^\s+|\s+$/g, '');};
String.prototype.ltrim=function(){return this.replace(/^\s+/,'');};
String.prototype.rtrim=function(){return this.replace(/\s+$/,'');};

function StringUtil () {
}

StringUtil.ltrim = function (str) {
	for(var k = 0; k < str.length && isWhitespace(str.charAt(k)); k++);
	return str.substring(k, str.length);
}
StringUtil.rtrim = function (str) {
	for(var j=str.length-1; j>=0 && isWhitespace(str.charAt(j)) ; j--) ;
	return str.substring(0,j+1);
}
StringUtil.trim = function (str) {
	return ltrim(rtrim(str));
}

StringUtil.isWhitespace = function (charToCheck) {
	var whitespaceChars = " \t\n\r\f";
	return (whitespaceChars.indexOf(charToCheck) != -1);
}

StringUtil.quoteMatchString = function (str) {
	var ar = str.split("");
	var res = '';
	for (var i = 0; i<ar.length;i++) {
		ch = ar[i];
		if (ch == '+' || ch == '%' || ch == '-' || ch == '?') {
			res = res + '.';
		}  else {
			res = res + ch;
		}
	}
	return res;
}

StringUtil.stripTechView = function (str) {
	return str.replace(/ Tech View$/, '');
}

// just add some function directly to string
if(typeof(String.prototype.trim) === "undefined")
{
    String.prototype.trim = function() 
    {
        return String(this).replace(/^\s+|\s+$/g, '');
    };
}
