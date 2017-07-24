(function () {
	if (alarms.length > 1) {
		session.sendMessage('Operation is supported for only one alarm at a time');
		return;
	}
	
    var _logger = Packages.org.apache.log4j.Logger.getLogger('hog.ecm.op.test');
	try {
		_logger.info(new Date() + "Trying to set prop");
		_logger.info("Found ecmop_guid >" + alarms[0].ecmop_guid + "<");
		alarms[0].ecmop_guid = "new_guid";
		_logger.info("Set ecmop_guid to >" + alarms[0].ecmop_guid + "<");
		alarms[0]["ecmop_test"] = "test";
		_logger.info("Set ecmop_test to >" + alarms[0].ecmop_test + "<");
		session.sendMessage( 'Operation completed');
	} catch (ex) {
		_logger.error("Modification not supported");
	}
	
}());