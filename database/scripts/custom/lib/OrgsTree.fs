/*

 Script Orgs.Tree.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0


 */

// tested in fscript
// var Orgs = Orgs === undefined ? {} : Orgs;
// oppure
//var Orgs = typeof Orgs === 'undefined' ? {} : Orgs;

var OrgsTree = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.lib.orgstree");
    load('custom/lib/DNamer.fs');
    load('custom/lib/OrgsFinder.fs');
    load('custom/lib/underscore.js');

    // returns a list (array) of names
    function _collectNameForElementOfClass (tree_dn, filtering_class) {

        if (arguments.length != 2)
            throw "collectFullNameForElementOfClass called with invalid arguments";
        _logger.debug('collectNameForElementOfClass collect using class ' + filtering_class + '\n\tunder ' + tree_dn);
        var collected = new Array();
        var visitor =
        {
            count: 0,
            visit: function ( child )
            {
                if (child.isOrganization()) {
                    var child_class = child.elementClassname + '';
                    if (child_class === filtering_class) {
                        collected.push(child.name);
                        visitor.count++;
                    }
                }
            }
        }

        var treeRoot = OrgsFinder.findElement(tree_dn);
        if (treeRoot) {
            treeRoot.walk(visitor);
        } else {
            _logger.warn('_collectNameForElementOfClass, not found element ' + tree_dn);
        }
        _logger.debug('collectNameForElementOfClass, collected ' + collected.length);
        return collected;
    }

    // returns boolean if found
    function _treeContains (tree_dn, filtering_class, name) {

        if (arguments.length != 3)
            throw "_treeContains usage: tree, class, name";
        _logger.debug('_treeContains collect using class ' + filtering_class + ', name ' + name +'\n\tunder ' + tree_dn);
        var collected = new Array();
        var visitor =
        {
            count: 0,
            visit: function ( child )
            {
                if (child.isOrganization()) {
                    var child_class = child.elementClassname + '';
                    if (child_class === filtering_class) {
                        collected.push(child.name);
                        visitor.count++;
                    }
                }
            }
        }

        var treeRoot = OrgsFinder.findElement(tree_dn);
        if (treeRoot) {
            treeRoot.walk(visitor);
        } else {
            _logger.warn('_treeContains, not found element ' + tree_dn);
        }
        _logger.debug('_treeContains, collected ' + collected.length);
        var found = _.contains(collected, name);
        _logger.debug('_treeContains, found ' + found);
        return found;
    }

    function _collectFullNameForElementOfClass (tree_dn, filtering_class) {
        if (arguments.length != 2)
            throw "collectFullNameForElementOfClass called with invalid arguments";
		_logger.debug('_collectFullNameForElementOfClass using class ' + filtering_class + '\n\tunder ' + tree_dn);


        // does not work in this rhino version: filtering_class = filtering_class || null;
        var collected = new Array();
        var visitor =
        {
            count: 0,
            visit: function ( child )
            {
                if (child.isOrganization()) {
                    var child_class = child.elementClassname + '';
                    if (child_class === filtering_class) {
                        collected.push(DNamer.parseDn(child.Dname).fullName);
                        visitor.count++;
                    }
                }
            }
        }

        var treeRoot = OrgsFinder.findElement(tree_dn);
        _logger.debug('collectFullNameForElementOfClass found ' + treeRoot);
        if (treeRoot)
            treeRoot.walk(visitor);
        else
            _logger.warn('_collectNameForElementOfClass, not found element ' + tree_dn);
        _logger.debug('_collectFullNameForElementOfClas returning');
        return collected;
    }

    function _collectDNameForElementOfClass (tree_dn, filtering_class) {
        if (arguments.length != 2)
            throw "collectFullNameForElementOfClass called with invalid arguments";
        _logger.debug('_collectFullNameForElementOfClass using class ' + filtering_class + '\n\tunder ' + tree_dn);


        // does not work in this rhino version: filtering_class = filtering_class || null;
        var collected = new Array();
        var visitor =
        {
            count: 0,
            visit: function ( child )
            {
                if (child.isOrganization()) {
                    var child_class = child.elementClassname + '';
                    if (child_class === filtering_class) {
                        collected.push(child.Dname);
                        visitor.count++;
                    }
                }
            }
        }

        var treeRoot = OrgsFinder.findElement(tree_dn);
        _logger.debug('collectFullNameForElementOfClass found ' + treeRoot);
        if (treeRoot)
            treeRoot.walk(visitor);
        else
            _logger.warn('_collectNameForElementOfClass, not found element ' + tree_dn);
        _logger.debug('_collectFullNameForElementOfClas returning');
        return collected;
    }

    return {
        collectDNameForElementOfClass:_collectDNameForElementOfClass,
        collectElementOfClass:_collectNameForElementOfClass,
        collectNameForElementOfClass:_collectNameForElementOfClass,
        collectFullNameForElementOfClass:_collectFullNameForElementOfClass,
        treeContains:_treeContains
    }
})();
