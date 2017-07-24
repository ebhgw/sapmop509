/*

 Script OrgUtilv2.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0


 */



var OrgUtilv2 = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
    _logger.info('Loading OrgUtil');

	load('custom/lib/underscore.js');
	_logger.info('Loaded underscore');
    load('custom/lib/DNamer.fs')
	_logger.info('Loaded DNamer');

    // add element as source (that is to SourceElements and Children) if does not exists
    var _addSource = function (sourceDn, ele)
    {
        var newSource = null;
        try {
            newSource = formula.Root.findElement(sourceDn); // we check if the source really exists

            var values = ele["SourceElements"]; // reading existing elements
            if (!(_.contains(values, sourceDn))) {
                values[values.length] = newSource.DName; // adding new DName
                ele["SourceElements"] = values; // writing back
            }
            values = ele["Children"]; // reading existing elements
            if (!(_.contains(values, sourceDn))) {
                values[values.length] = newSource.DName; // adding new DName
                ele["Children"] = values; // writing back
            }
        } catch (excp) {
            _logger.error('_addSourceToElement: ' + excp)
        }
    }

    // remove element from source and children, if exists
    var _removeSource = function (sourceDn, ele)
    {
        try {
            var values = ele["SourceElements"]; // reading existing elements
            ele["SourceElements"] = _.without(values, [sourceDn]); // writing back
            values = ele["Children"]; // reading existing elements
            ele["Children"] = _.without(values, [sourceDn]); // writing back
        } catch (excp) {
            _logger.error('_removeSourceElement ' + excp)
        }
    }

    // clear sources
    var _removeAllSources = function (ele) {
        var emptyAr = new Arry();
        ele["SourceElements"] = emptyAr;
        ele["Children"] = emptyAr;
    }

    // remove non contribuing mark that sometimes appears
    var _removeNonContributing = function (ele) {
        var parentDn = DNamer.parseDn(ele.DName).parent;
        var parent = formula.Root.findElement(parentDn);
        _addSource(ele.Dname, parent);
    }

    return {
        addSource:_addSource,
        removeSource:_removeSource,
        removeAllSources:_removeAllSources,
		removeNonContributing:_removeNonContributing
    }
})();


