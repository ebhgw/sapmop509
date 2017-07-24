/*

 Script OrgUtilv2.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0


 */

(function () {

    //state is defined by formula script environment
    // myObject === undefined, !myObject, typeof myObject == 'undefined' seems to be equivalent
    if (state.orgs === undefined) {
        state.orgs = {}
    }
    if (state.orgs.Sources === undefined) {
        state.orgs.Sources = {}
    }
    var root = state.orgs.Sources

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.orgs.sources");
    _logger.info('Loading orgs.sources');

    // use state._183 for underscore v 183
	//load('custom/lib/underscore.js');
    load('custom/lib/DNamer.fs')
	_logger.info('Loaded DNamer');

    // add element as source (that is to SourceElements and Children) if does not exists
    root.addSource = function (sourceDn, ele)
    {
        var newSource = null;
        try {
            newSource = formula.Root.findElement(sourceDn); // check if the source really exists

            var values = ele["SourceElements"]; // reading existing elements
            if (!(state._183.contains(values, sourceDn))) {
                values[values.length] = newSource.DName; // adding new DName
                ele["SourceElements"] = values; // writing back
            }
            values = ele["Children"]; // reading existing elements
            if (!(state._183.contains(values, sourceDn))) {
                values[values.length] = newSource.DName; // adding new DName
                ele["Children"] = values; // writing back
            }
        } catch (excp) {
            _logger.error('_addSourceToElement: ' + excp)
        }
    }

    // remove element from source and children, if exists
    root.removeSource = function (sourceDn, ele)
    {
        try {
            var values = ele["SourceElements"]; // reading existing elements
            ele["SourceElements"] = state._183.without(values, [sourceDn]); // writing back
            values = ele["Children"]; // reading existing elements
            ele["Children"] = state._183.without(values, [sourceDn]); // writing back
        } catch (excp) {
            _logger.error('_removeSourceElement ' + excp)
        }
    }

    // clear sources
    root.removeAllSources = function (ele) {
        var emptyAr = new Arry();
        ele["SourceElements"] = emptyAr;
        ele["Children"] = emptyAr;
    }

    // remove non contribuing mark that sometimes appears
    root.removeNonContributing = function (ele) {
        var parentDn = DNamer.parseDn(ele.DName).parent;
        var parent = formula.Root.findElement(parentDn);
        _addSource(ele.Dname, parent);
    }

    return root
}).call(this);


