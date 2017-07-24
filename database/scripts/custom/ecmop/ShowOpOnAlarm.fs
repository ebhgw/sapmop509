(function () {
	if (alarms.length > 1) {
		session.sendMessage('Operation is supported for only one alarm at a time');
		return;
	}
	
    var _logger = Packages.org.apache.log4j.Logger.getLogger('hog.ecm.op');
    var javaheo = Packages.hog.ecm.op;
	var javautil = Packages.java.util;
	var myAlarm = alarms[0];
	
    // return a java.util.ArrayList<EcmOp>
	var obml = new javaheo.res.OpByMcueidLister();
	
	var mcid = myAlarm.mc_ueid;
	_logger.info('ShowOpOnAlarms found mc_ueid ' + mcid + ', guid ' + guid)
	var eo = null;
	var endpt = "http://10.2.230.242:2007/GetMcUeidOperations.ashx";
	var guid = obml.getLastGuidFromResult(mcid, endpt);
	_logger.info('guid: ' + guid);
	// extract info from guid
	
    session.sendMessage( 'Operation result ' + eo.toString() );
	return;
	
}());