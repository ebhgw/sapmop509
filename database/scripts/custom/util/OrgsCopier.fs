/*

 custom/util/OrgsCopier.fs
 Version 1.0
 E. Bomitali

 Updates:
 Ver    	Author          Description
 1.0    	E. Bomitali    Initial Release

 Scrive su file

 */
 
load('custom/lib/underscore.js');

var OrgsCopier = (function () {

	var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.orgscopier");

    var _mapDnHelper = function(dn, from, to) {
        var idx = dn.lastIndexOf(from)
        var resDn = dn.substr(0,idx) + to;
        return resDn
    }

    // parse dn to extract class, name and parent
    var _parseDn = function (dn) {
        var patt = /(([^=]*)=([^\/]*))\/(.*)/
        var match = patt.exec(dn);
        return {
            classAndName: match[1],
            name: formula.util.decodeURL(match[3]),
            className: formula.util.decodeURL(match[2]),
            encodedName: match[3],
            encodedClassName: match[2],
            parent:match[4]
        }
    }

	// copies the children of fromTreeRootDn under toTreeRootDn
    var _copySubTree = function (fromTreeRootDn, toTreeRootDn) {
		_logger.info('fromTreeRootDn ' + fromTreeRootDn);
		_logger.info('toTreeRootDn ' + toTreeRootDn);
		
		var fromTreeRoot = state.Orgs.findElement(fromTreeRootDn);
		_.each(fromTreeRoot.Children, function (childDn) { state.Orgs.copy(childDn, toTreeRootDn )} );
			
    }
	
	// copies the children of fromTreeRootDn under toTreeRootDn
    var _copyElementTree = function (eleDn, toTreeRootDn) {
		_logger.info('eleDn ' + eleDn);
		_logger.info('toTreeRootDn ' + toTreeRootDn);
		
		state.Orgs.copy(eleDn, toTreeRootDn );
			
    }
	
	var _test = function () {
		_logger.info('ok');
	}

    return {
		copyElementTree:_copyElementTree,
		copySubTree:_copySubTree,
		test:_test
    }
});
