/*
 Script TreeAligner.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Taken a template subtree, adds or remove elements from a target subtree to match template structure.
 Structure should be equivalent (compare on element name and class name) but the root of the dname

 Note:
 - won't work against the adapter as class name changes and therefore dname changes at all levels

 Note:
 - won't work if attached to state. It need a session as it uses creation deletion operation that need
   current session

 */

var TreeAligner = (function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.orgs.treealigner");
	load('custom/lib/Orgs.fs');
    load('custom/lib/OrgsFinder.fs');
    load('custom/lib/underscore.js');

    // delete target elements that are not in the template structure
    var deleteEleNotInSource = function(sourceRootDn, targetRootDn) {
        _logger.debug('deleteEleNotInSource ' + targetRootDn + '::' + sourceRootDn)
        var deletingVisitor =
        {
            count: 0,
            idx: 0,
            eleDn: '',
            tgtDn: '',
            tgtEle: null,
            found: false,
            visit: function ( ele )
            {
                eleDn = ele.DName
                tgtDn = state.DNamer.mapDn(eleDn, targetRootDn, sourceRootDn)
                _logger.debug('Target: Visiting ' + eleDn)
                _logger.debug('Source: check if exists ' + tgtDn);
				// returns null if not found
                tgtEle = OrgsFinder.findElement(tgtDn)
				// if checkeEle not found, it is null
                if (!tgtEle) {
                    _logger.debug('Not found, deleting ' + eleDn)
                    Orgs.deleteElement(ele);
                    deletingVisitor.count++
                } else {
                    _logger.debug('Found')
                }
            }
        }

        var tgtRoot = OrgsFinder.findElement(targetRootDn);
		if (!tgtRoot) {
			_logger.error('deleteEleNotInSource not found: ' + targetRootDn);
		} else {
			_logger.debug('deleteEleNotInSource walk');
			tgtRoot.walk ( deletingVisitor );
		}
        _logger.debug("deleteEleNotInSource ends, deleted :" + deletingVisitor.count );
    }

    // alias
    var deleteEleNotInBaseline = deleteEleNotInSource


    // Deletes the children of the tree root, ie Acronimo or Servizio element
    // Exception list is a list of elemeent fullname (i.e. class=name) that could be in source but should not go in target
    var deleteFirstLevEleNotInSource = function(sourceRootDn, targetRootDn, excpList) {

        _logger.debug('deleteFirstLevEleNotInSource start found ' + OrgsFinder.findElement(targetRootDn).Children.length + ' children\n\tsourceDn: ' + sourceRootDn + '\n\ttargetDn: ' + targetRootDn);
        var count = 0;

		// check if element has not a related element within template
        var testSourcePresent = function (eleDn)
        {
            // replace targetRootDn with sourceRootDn in eleDn.
            var checkDn = state.DNamer.mapDn(eleDn, targetRootDn, sourceRootDn)
            var checkEle = OrgsFinder.findElement(checkDn);
			// if not found in the template, the element should be removed
			var res = (checkEle===null);
            _logger.debug('deleteFirstLevEleNotInSource.testSourcePresent: ' + res + '\n\ttargetDn: ' + eleDn + '\n\tsourceDn: ' + checkDn);
            return res;
		}
		
		// check if element fullname is in excpeption list
		var isInExcpList = function (eleDn) {
			var eleFullname = state.DNamer.parseDn(eleDn).fullName;
			var res = _.contains(excpList, eleFullname);
			_logger.debug('deleteFirstLevEleNotInSource.isInExcpList: ' + res + '\n\tfullName: ' + eleFullname + '\n\texcpList: ' + excpList);
			return res;
		}

        var targetEle = OrgsFinder.findElement(targetRootDn);
        if (targetEle) {
            _.each(targetEle.Children, function (dn) {
                        _logger.debug('deleteFirstLevEleNotInSource checking ' + dn);
                        if (testSourcePresent(dn) || isInExcpList(dn)) {
                            _logger.debug('deleteFirstLevEleNotInSource deleting');
                            Orgs.deleteDn(dn);
                            count++}});
            _logger.debug("deleteFirstLevEleNotInSource ends, deleted :" + count );
        } else {
            _logger.error('deleteFirstLevEleNotInSource not found ' + targetRootDn);
        }
    }

    // copy from source to target
    var addEleFromSource = function(sourceRootDn, targetRootDn) {

        _logger.debug('addEleFromSource\n\tsourceDn: ' + sourceRootDn + '\n\ttargetDn: ' + targetRootDn)
        // start from baseline dname, map to structure dname, if does not exists copy there (using the parent)
        var addingEleVisitor =
        {
            count: 0,
			eleDn: '',
			tgtDn: '',
			tgtParentDn: '',
 
            visit: function ( ele )
            {
                eleDn = ele.DName
                tgtDn = state.DNamer.mapDn(eleDn, sourceRootDn, targetRootDn);
				tgtParentDn = state.DNamer.parseDn(tgtDn).parent;
				if (!OrgsFinder.findElement(tgtDn)) {
					if (Orgs.copy(eleDn,tgtParentDn)) {
						_logger.debug('Copied ' + eleDn + ' to ' + tgtDn);
						addingEleVisitor.count++;
					} else {
						_logger.warn('Failed to copy ' + eleDn + ' to ' + tgtDn);
					}
				}
            }
        }

        var baseEle = OrgsFinder.findElement(sourceRootDn);
        baseEle.walk ( addingEleVisitor );
        _logger.debug("addEleFromSource ends, added :" + addingEleVisitor.count );

    }

    // excpt List is a fullName list (i.e. Acronimo=IBCX0)
    var addFirstLevEleFromSource = function(sourceRootDn, targetRootDn, excpList) {
        _logger.debug('addFirstLevEleFromSource start ' + sourceRootDn + ' :: ' + targetRootDn)
        // start from baseline dname, map to structure dname, if does not exists copy there (using the parent)
        var count = 0;

        // given an element from source, check if the target exists ow adds it
        var addEle = function (eleDn) {
            _logger.debug('addFirstLevEleFromSource.addEle source' + eleDn);

			var ele = OrgsFinder.findElement(eleDn);
			var tgtDn = state.DNamer.mapDn(eleDn, sourceRootDn, targetRootDn);
            var tgtEle = OrgsFinder.findElement(tgtDn);
            //tgtDn = eleDn.substr(0,idx) + targetRootDn;
            _logger.debug('addFirstLevEleFromSource.addEle checking if exists: ' + tgtDn);
            
			// if found, nothing to do, ow copy
            if (tgtEle) {
                _logger.debug('addFirstLevEleFromSource.addEle donothing, ' + tgtDn + ' exists');
                return;
            }

            _logger.debug('addFirstLevEleFromSource.addEle not found, adding ' + tgtDn);
            var tgtParentDn = state.DNamer.parseDn(tgtDn).parent;
            //_logger.debug('addFirstLevEleFromSource.addEle finding parent ' + tgtParentDn);
            var tgtParent = OrgsFinder.findElement(tgtParentDn);
            if (tgtParent){
                _logger.debug('addFirstLevEleFromSource.addEle parent found ' + tgtParentDn);
                tgtParent.copy(session.getReference(), ele.id(), formula.relations.NAM);
                count++;
            } else {
                _logger.error('addFirstLevEleFromSource.addEle parent not found ' + tgtParentDn)
            }
        }

        var sourceRoot = OrgsFinder.findElement(sourceRootDn);
        _.each(sourceRoot.Children,
                   function (dn) {
                        var pdn = state.DNamer.parseDn(dn);
                        if (! _.contains(excpList, pdn.fullName))
                            addEle(dn)
                   }
               );
        _logger.debug("addFirstLevTemplateEle ends, added :" + count );
    }

    // 1. check first level children against excption list
    // 2. then loop on first level children on tgt to update their subtree
    var _alignToSourceWithExcpHelper = function (sourceRootDn, tgtRootDn, excptList) {
        _logger.debug('_alignToSourceWithExcpHelper\n\tsourceDn: ' + sourceRootDn + '\n\ttargetDn: ' + tgtRootDn + '\n\t with exceptions ' + excptList);
        deleteFirstLevEleNotInSource(sourceRootDn, tgtRootDn, excptList);
        addFirstLevEleFromSource(sourceRootDn, tgtRootDn, excptList);
        // align structure for each acronimo
        _logger.debug('_alignToSourceWithExcpHelper looping on first level children');
        var tgtRoot = OrgsFinder.findElement(tgtRootDn);
        _.each(tgtRoot.Children,
            function(dn) {
                try {
                    _logger.debug('_alignToSourceWithExcpHelper : iteration child ' + dn);
                    var pdn = state.DNamer.parseDn(dn);
                    var sourceDn = pdn.fullName + '/' + sourceRootDn;
                    var sourceEle = OrgsFinder.findElement(sourceDn);

                    if (sourceEle) {
                        _logger.info('_alignToSourceWithExcpHelper aligning to  ' + sourceDn);
                        _alignToSourceHelper(sourceDn,dn);
                    } else {
                        _logger.warn('_alignToSourceWithExcpHelper not found : ' + sourceDn);
                    }
                } catch (excp) {
                    _logger.error('_alignToSourceWithExcpHelper, each on modelloAcro error ' + excp);
                }
            });
        _logger.debug('_alignToSourceWithExcpHelper ended');
    }

    // update tree (tgtRootDn) against source structure, adding or removing elements as neeeded
    var _alignToSourceHelper = function (sourceRootDn, tgtRootDn) {
            _logger.debug('_alignToSourceHelper\n\tsourceDn: ' + sourceRootDn + '\n\ttargetDn: ' + tgtRootDn);
            deleteEleNotInSource (sourceRootDn, tgtRootDn);
            addEleFromSource (sourceRootDn, tgtRootDn);
            _logger.debug('_alignToSourceHelper ended');
    }

    // alignToSource(sourceRootDn, tgtRootDn, fullname excpt list)
    // or
    // alignToSource(sourceRootDn, tgtRootDn)
    //
    // excptList applies to first level children (that is the ones under tree root)
    var _alignToSource = function () {
        if (arguments.length == 3) {
            _alignToSourceWithExcpHelper(arguments[0],arguments[1],arguments[2]);
        } else {
            _alignToSourceHelper(arguments[0],arguments[1]);
        }
    }

    // deprecated
    var _alignToTemplate = _alignToSource;

    return {
        addFirstLev4Template:addFirstLevEleFromSource,
		addTemplateEle:addEleFromSource,
        alignToSource:_alignToSource,
        alignToTemplate:_alignToTemplate,
		deleteFirstLevNotInTemplate:deleteFirstLevEleNotInSource,
		deleteEleNotInTemplate:deleteEleNotInSource
    }

})();