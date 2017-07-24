/*

 Script Orgs.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Functions to find/create/delete/rename elements

 */

var Orgs = (function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.orgs");
    _logger.debug('Loading OrgUtil');

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
        } catch (Exception){
            //nothing to do
            _logger.debug('_findElementHelper: unable to find ' + eleDn);
        }
        return res;
    }

    // create a new element, server script
    // params clazz string, name string, parentDn string, propNames array, propValues array
    var _createElement = function() {
        // check parameters
        if (arguments.length < 3) {
            throw ('_createElement usage: createElement(class, name, parent dname, optional property names array, optional property values array)');
        }

        var res = null;
        var clazz = arguments[0];
        var name = arguments[1];
        var parentDn = arguments[2];
        _logger.debug('_createElement for ' + clazz + '=' + name + '/' + parentDn);
        res = _findElement(clazz, name, parentDn);

        if (res) {
            _logger.debug("_createElement not created because already present");
            return res;
        }

        if (arguments.length == 3) {
            var emptyNames = new Array();
            var emptyValues = new Array();
            res = _createElementHelper(arguments[0], arguments[1], arguments[2], emptyNames, emptyValues);
        } else {
            res = _createElementHelper(arguments[0], arguments[1], arguments[2], arguments[3], arguments[4]);
        }
        _logger.debug("_createElement created " + res);
        return res;
    }

    var _createElementHelper = function(clazz, name, parentDn, propNames, propValues) {
        // find the parent
        var res = null;
        var parent = null;
        var msg = '';
        try {
            parent = formula.Root.findElement(parentDn);
        } catch (excp) {
            msg = "_createElementHelper unable to locate parent element: " + excp;
            _logger.error(msg);
        }

        if (parent) {
            // Find the new class to add.
            var orgClass = null;
            try {
                orgClass = parent.elementClass.findChild( clazz );
            } catch (excp) {
                _logger.error("Unable to perform .elementClass.findChild: " + ( undefined != excp.key ? excp.key :
                    excp.getMessage() ), excp )
            }

            if (orgClass) {
                // add the new organization
                try {
                    orgClass.newElement( session.getReference(), parent, name, propNames, propValues );
                    res = parent.findElement(formula.util.encodeURL(clazz) + '=' + formula.util.encodeURL(name) + '/' + parentDn);
                } catch (excp) {
                    _logger.error( "_createElementHelper, .newElement failed: " + ( undefined != excp.key ? excp.key :
                        excp.getMessage() ), excp )
                }
            }
        }
        return res;
    }
	
	var _getElement = function (clazz, name, parentDn) {
		var eclazz = formula.util.encodeURL(clazz);
		var ename = formula.util.encodeURL(name);
		_logger.debug('_getElement ' + eclazz + '=' + ename + '/' + parentDn);
		server.getElement(eclazz + '=' + ename + '/' + parentDn);
	}

    var _deleteElement = function (ele) {
        try {
			ele.perform( session, 'LifeCycle|Delete', [], [] );
        } catch (excp) {
            res = '_deleteElement, error deleting ' +  ele.Dname + ' : ' + excp;
            _logger.error(res);
            throw(excp);
        }

    }
	
	var _deleteDn = function(eleDn)
    {
        var ele = _findElement(eleDn);
		if (ele)
			_deleteElement(ele);
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // Delete an existing organization
    //
    // dname: DName of the organization element to delete
    //
    var deleteElement2 = function (dn)
    {
        // find child and parent
        var child
        try {
            child = formula.Root.findElement(dn)
        } catch (excp) {
            return "Unable to locate the specified element:" + excp
        }

        try {
            child.destroy()
            return ""
        } catch (excp) {
            return "Unable to delete organization: " + excp
        }
    }

    // uses objects reference (no dn)
	// .copy copies the element and its (NAM) subtree
    var _copyHelper = function(ele, parent) {
        var copied = false;
        try {
            parent.copy(session.getReference(), ele.id(), formula.relations.NAM);
            copied = true;
        } catch (excp) {
            _logger.error( "_copyHelper error: " + ( undefined != excp.key ? excp.key :
                excp.getMessage() ), excp )
        }
        return copied;
    }

    // set force to force copy even if element is existent. This will create a <name> (1) copied element
    var copy = function(eleDn, targetParentDn, force) {

        force = force || false;

        _logger.debug('copy forced=' + force + ': ' + eleDn + ' under ' + targetParentDn)
        var ele, parent, copied = false, doCopy = false;

        ele = state.Orgs.findElement(eleDn);
        ele === null?_logger.debug('ele not found'):_logger.debug('ele found')
        parent = state.Orgs.findElement(targetParentDn);
        parent === null?_logger.debug('targetParent not found'):_logger.debug('targetParent found');
        tgtEle = state.Orgs.findElement(state.DNamer.parseDn(eleDn).fullName + '/' + targetParentDn);
		tgtEle === null?_logger.debug('targetEle not found'):_logger.debug('targetEle found');

        if (ele && parent && !force && !tgtEle)
            doCopy = true;
        else if (ele && parent && force)
            doCopy = true;
        else
            doCopy = false;

        // todo: check if element already exists ?
        if (doCopy) {
            copied = _copyHelper(ele, parent);
           _logger.debug('Copied')
        }
        return copied;
    }

    var rename = function(ele, name, clazz) {
        ele.rename(formula.util.encodeURL(name), formula.util.encodeURL(clazz));
    }

    return {
        createElement:_createElement,
        copy:copy,
		deleteDn:_deleteDn,
        deleteElement:_deleteElement,
        findElement:_findElement,
		getElement:_getElement,
        rename:rename
    }

})();
