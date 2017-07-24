/*

 Script DNamer.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Utilities to handle dnames

 */

var DNamer = (function(){

    load('custom/lib/underscore.js');

    // parse dn to extract class, name and parent
    // parse dn to extract class, name and parent
    var _parseDn = function (dn) {
        var patt = /(([^=]*)=([^\/]*))\/(.*)/
        var match = patt.exec(dn);
        return {
            fullName: match[1],                             // Server=svtp001
            name: formula.util.decodeURL(match[3]),         // svtp001
            className: formula.util.decodeURL(match[2]),    // Server
            encodedName: match[3],
            encodedClassName: match[2],
            parent:match[4]                                 // gen_folder=CMS/root=Organizations
        }
		
    }

    // take dn, remove the trailing from, add a trailing to
    // example: dn: a/b/c/d, from: c/d, to: e/f -> _mapDn(dn) = a/b/e/f
    var _mapDn = function(dn, from, to) {
        var idx = dn.lastIndexOf(from)
        var resDn = dn.substr(0,idx) + to;
        // map class
        return resDn
    }

    // returns an array of encoded fullname
    var _splitOnLevelDn = function (dn) {
        return dn.split("/");
    }

    // is there a criteria to verify that is it a dname and not a generic string ?
    var _isDName = function (dn) {
		var res = false;
		// is string
		if (!_.isString(dn)) {
			return false;
		}
		// split on names and check that every element has a = in the middle
		var list = _splitOnLevelDn(dn);
		res  = _.every(list, function (fn) {
			return fn.substring(1,fn.length-1).indexOf('=') != -1;
		})
        return res;

    }

    var _makeDn = function (clazz, name, parent) {
        var dn = formula.util.encodeURL(clazz) + '=' + formula.util.encodeURL(name) + '/' + parent;
        return dn;
    }
	
	var _nameOfFullname = function (dn) {
        var patt = /([^=]*)=([^\/]*)/
        var match = patt.exec(dn);
        return formula.util.decodeURL(match[2])
    }
	
	var _getUserFromDname = function (parUser) {
		var patt = /user=(\w*)\/.*/
		var res = patt.exec(parUser);
		if (res == null) {
			return null;
		} else {
			return res[1];
		}
	}

    return {
		getUserFromDname:_getUserFromDname,
        isDName:_isDName,
        makeDn:_makeDn,
        mapDn:_mapDn,
		nameOfFullname:_nameOfFullname,
        parseDn:_parseDn,
        splitOnLevelDn:_splitOnLevelDn
    }

}());