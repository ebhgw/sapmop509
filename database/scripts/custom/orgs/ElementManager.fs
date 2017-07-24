/*
  Script ElementMngr.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  Given two trees, template and target, uses the template tree and modifies the target tree
  so that is has the same structure than the template, adding and removing leaves and branches as needed

*/

var emlogger = Packages.org.apache.log4j.Logger.getLogger("fs");

function ElementMngr () {
}

// Find an element or returns null
ElementMngr.findElement = function(dn) {
	var ele = null
	try {
		ele = formula.Root.findElement(dn)
	} catch (excp) {
		// really nothing to do
	}
	return ele
}

ElementMngr.mapDn = function(dn, from, to) {
	var idx = dn.lastIndexOf(from)
	emlogger.debug('Mapping: ' + dn)
	var resDn = dn.substr(0,idx) + to;
	emlogger.debug('Mapped: ' + resDn)
	return resDn
}

ElementMngr.remove = function(eleDn) {
	var ele, found
	try {
		ele = formula.Root.findElement(eleDn)
		found = true
	} catch (Exception) {
		// really nothing to do
	}
			
	if (found) {
		emlogger.debug('Found, deleting ' + ele.DName)
		ele.perform( session, 'LifeCycle|Delete', [], [] )
	} else {
		emlogger.debug('Found')
	}
}

ElementMngr.removeChildren = function(eleDn) {
	var ele, found
	try {
		ele = formula.Root.findElement(eleDn)
		emlogger.debug('Found ' + ele.DName)
		found = true
	} catch (Exception) {
		// really nothing to do
	}
			
	if (found) {
		var eleChildren = ele.Children
		emlogger.debug('Deleting children')
		for (var i = 0; i < eleChildren.length; i++) {
			ElementMngr.remove(eleChildren[i])
		}
	} else {
		emlogger.debug('No delete as not found ' + ele.DName)
	}
}

// create element from its dname
// newElement = server.getElement(targetDName) ;

ElementMngr.copy = function(eleDn, targetParentDn) {

	emlogger.debug('copy ' + eleDn + ' under ' + targetParentDn)
	var ele, parent;

	try {
		ele = formula.Root.findElement(eleDn)
		emlogger.debug('eleDn found')
		parent = formula.Root.findElement(targetParentDn)
		found = true
		emlogger.debug('targetParentDn found')
	} catch (Exception) {
		emlogger.debug('Unable to find element to copy or parent to copy to')
		// really nothing else to do
	}
			
	if (found) {
		parent.copy(session.getReference(), ele.id(), formula.relations.NAM);
		emlogger.debug('Copied')
	}
}

// uses objects reference (no dn)
ElementMngr._copy = function(ele, parent) {
	emlogger.info('_copy ' + ele + ' : ' + parent)
	emlogger.info('_copy typeof ' + typeof ele + ' : ' + typeof parent)
	try {
		parent.copy(session.getReference(), ele.id(), formula.relations.NAM);
	} catch (excp) {
		emlogger.error('While copy ' + ele.dname + ' to ' + parent.dname + ' error : ' + excp);
	}
}

// Copy all children (both real and link) from current element to a target element.
// Copied children are all direct children
ElementMngr.copyChildren = function(currParentDn, targetParentDn) {
	var currParent, targetParent, found
	try {
		currParent = formula.Root.findElement(currParentDn)
		targetParent = formula.Root.findElement(targetParentDn)
		emlogger.debug('Found ' + currParent.DName)
		found = true
	} catch (Exception) {
		// really nothing to do
	}
			
	if (found) {
		var childrenToCopy = currParent.Children
		emlogger.debug('Copy children')
		for (var i = 0; i < childrenToCopy.length; i++) {
			ele = formula.Root.findElement(childrenToCopy[i])
			ElementMngr._copy(ele,targetParent)
		}
	}
	emlogger.debug('Copied ' + i + ' children')
}
