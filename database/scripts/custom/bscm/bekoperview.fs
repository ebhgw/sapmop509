/**
 * Created with JetBrains PhpStorm.
 * User: Evelino
 * Date: 29/05/15
 * Time: 16.09
 * To change this template use File | Settings | File Templates.
 */

/*

 Hogwart srl
 Copyright (C) 2015

 Util attached to state for be_koper view

 */


var bekoperview = (function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.bekoperview");

    var filterBackupCompInfra = function (ele) {
		var res = false;
        var clazz = ele.elementClassname + "";
        var parentName = ele.parent.name;
        var eleName = ele.name;
        res =  clazz==="CompInfra"&&parentName.indexOf("BACKUP")!=-1;
		if (res) {
			_logger.info(res + " -> ele: " + clazz + ":" + eleName + " parent " + parentName + " idx " + parentName.indexOf("BACKUP"));
		}
        return res;
    }
	
	var filterCompInfraOnParentName = function (ele,filterOnParentName) {
		var res = false;
        var clazz = ele.elementClassname + "";
        var parentName = ele.parent.name;
        var eleName = ele.name;
        res =  clazz==="CompInfra"&&parentName.indexOf(filterOnParentName)!==-1;
		if (res) {
			_logger.info(res + " -> ele: " + clazz + ":" + eleName + " parent " + parentName + " idx " + parentName.indexOf(filterOnParentName));
		}
        return res;
    }

    return {
        filterBackupCompInfra:filterBackupCompInfra,
		filterCompInfraOnParentName:filterCompInfraOnParentName
    }
})();