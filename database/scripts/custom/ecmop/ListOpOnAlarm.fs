(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecm_soap');
    var oul = new Packages.com.hog.noc.ecmop.res.OpUncompLister();
    _logger.info('Created OpUncompLister');
    var lg = oul.queryUncompOp();
	_logger.info('Called OpUncompLister.queryUncompOp');
	

    if (lg === null) {
        msg = 'No Pending operations';
    } else {
        var msg = 'Found ' + lg.size() + ' pending operations:\n';
		_logger.info(msg);
        var opc = new Packages.com.hog.noc.ecmop.res.OpResultChecker();
        var eo = null;
        for (i=0; i < lg.size(); i++) {
			_logger.info('Querying guid ' + lg.get(i));
            eo = opc.queryOpByGuid(lg.get(i));
            if (eo != null) {
                msg = msg + eo.opexecSummary() + '\n';
            }
        }

    }
    session.sendMessage(msg);
}());