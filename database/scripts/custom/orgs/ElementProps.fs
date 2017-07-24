/*

  Script CopyElementProps.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  Copy elements from a source to a destinationi

*/

var elementProps = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.orgs.elementprops');

    load('custom/util/Properties.fs');
    var element_props = Properties.loadCustomConfig('ElementProps');
	load('custom/lib/underscore.js');
    load('custom/lib/OrgsFinder.fs');

    var modelloRoot = 'gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';

    var _copyFromGenMod = function (ele) {
        var dn = ele.DName;
        var pdn = state.DNamer.parseDn(dn);
        var propofclazz = element_props.getProperty(pdn.className);
        // java null === js null ?
        if (propofclazz) {
            var proplist = propofclazz.split(',');
            // find origin
            var fromDn = pdn.fullName + '/' + 'gen_folder=Acronimi/gen_folder=Baseline' + '/'
                + 'gen_folder=' + ele.Ambiente + '/' + modelloRoot;
            _logger.debug('_copy, find from ' + fromDn);
            var fromEle = OrgsFinder.findElement(fromDn);
            if (fromEle) {
                _.each(proplist, function(p) { _logger.debug('Copying prop ' + p + ', value ' + fromEle[p]);
												ele[p] = fromEle[p]; })
            } else {
                _logger.warn('_copy, unable to find ' + fromDn + '\nfor service model element ' + dn);
            }
        }
    }
	
	var _copyFromGenModintoChildren = function (ele) {
        var chdn = element.children;
		_.each(chdn, function(ele) { _copyFromGenMod(ele) });
    }

    return {
        copyFromGenMod:_copyFromGenMod,
		copyFromGenModintoChildren:_copyFromGenModintoChildren
    }

})();

