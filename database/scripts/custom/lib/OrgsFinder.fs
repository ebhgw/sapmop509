/*

 Script Orgs.Finder.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0


 */

// tested in fscript
//var Orgs = typeof Orgs === 'undefined' ? {} : Orgs;

OrgsFinder = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.orgs.finder");

    // two forms, clazz, name, parentDn or dn
    var _findElement = function () {
        var dn = ''
        if (arguments.length == 3) {
            dn = formula.util.encodeURL(arguments[0]) + '=' + formula.util.encodeURL(arguments[1]) + '/' + arguments[2];
        } else {
            dn = arguments[0];
        }
        return _findElementHelper(dn);
    }

    var _findElementHelper = function (eleDn) {
        var res = null;
        try {
            res = formula.Root.findElement(eleDn);
        } catch (excp){
            //nothing to do
            _logger.debug('_findElementHelper: ' + excp);
        }
        return res;
    }

    function _collectElementOfClass (tree_dn, filtering_class) {
        filtering_class = filtering_class || null;
        _logger.debug('filtering_class is ' + filtering_class===null?'>null<':'>'+filtering_class+'<');
        var collected = new Array();

        var visitor =
        {
            child_class:'',
            count: 0,
            visit: function ( child )
            {
                if (child.isOrganization()) {
                    visitor.child_class = child.elementClassname + '';
                    if (child_class === filtering_class) {
                        collected.push(child.name);
                        visitor.count++;
                    }
                }
            }
        }

        var treeRoot = _findElementHelper(dn);
        if (treeRoot)
            treeRoot.walk(visitor);
        _logger.debug('Collected ' + visitor.count +' elements (Collected size is ' + collected.length + ')');
        return collected;
    }

    return {
        findElement:_findElement,
        collectElementOfClass:_collectElementOfClass
    }

})();


