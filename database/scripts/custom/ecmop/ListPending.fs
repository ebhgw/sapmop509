(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger('hog.ecm.op');
    var javaheo = Packages.hog.ecm.op;
	var javautil = Packages.java.util;
	var pending = javaheo.PendingConcurrentMapFactory.getSingleton();
	//var pv = pending.getValues();
	var leo = pending.getValues();
	iter = leo.iterator();
	_logger.info('EcmOp pending list');
	while (iter.hasNext()){
		eo = iter.next();
		_logger.info('>>');
		_logger.info(eo.toString());
	}
	_logger.info('<<');
    session.sendMessage( 'Operation completed. See log' );
}());