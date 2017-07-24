/*

  Author: Bomitali Evelino
  Tested with versions: 5.0
  
  Build Overview - Acronimos view using bscm + post processing script

*/

load('custom/lib/Orgs.fs');
load('custom/lib/underscore.js');

(function () {

var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
// will build a view of type Overview - Acronimos
// first check element

var modelRootDn = 'gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations';
var modelRoot = formula.Root.findElement(modelRootDn);
var viewRootDn = 'AcronimoOpVw=Overview+-+Acronimos/gen_folder=Test/root=Organizations';

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

var acroList = _.map(modelRoot.Children, function(dn) {_parseDn(dn).name});
_.each(acroList, function (name) { orgs.createElement(name, 'AcronimoOpVw', viewRootDn) });

if (element.name == 'Overview - Acronimos') {
	viewRootDn = element.Dname;
	var children = modelRoot.Children;
	_.each(children, function(dn) {_logger.info('Found child: ' + dn)});
	var acroList = _.map(children, function(dn) { return _parseDn(dn).name});
	_.each(acroList, function (nn) {_logger.info('Found name: ' + nn)});
	_.each(acroList, function (name) { orgs.createElement('AcronimoOpVw', name, viewRootDn) });
	
}

})();