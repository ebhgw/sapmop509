(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.clearpending');
    var javaheo = Packages.hog.ecm.op;
	var pending = javaheo.PendingConcurrentMapFactory.getSingleton();
	pending.clear();
    session.sendMessage( 'Pending cleared.' );
}());