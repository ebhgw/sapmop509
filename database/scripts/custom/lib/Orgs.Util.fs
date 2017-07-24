/*

 Script OrgUtilv2.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0


 */

var Orgs = Orgs === undefined ? {} : Orgs;

Orgs.Util = (function () {

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

    var _removeAllSources = function (ele) {
        var emptyAr = new Array();
        ele["SourceElements"] = emptyAr;
        ele["Children"] = emptyAr;
    }

    var _removeNonContributingMark = function (ele) {
        var parentDn = DNamer.parseDn(ele.DName).parent;
        var parent = formula.Root.findElement(parentDn);
        _addSource(ele.Dname, parent);
    }

    return {
        addSource:_addSource,
        removeSource:_removeSource,
        removeAllSources:_removeAllSources,
        removeNonContributingMark:_removeNonContributingMark
    }
})();


