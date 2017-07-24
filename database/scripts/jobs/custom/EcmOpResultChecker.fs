(function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecm_soap');
    var oul = new Packages.com.hog.noc.ecmop.res.OpUncompLister();
    _logger.debug('EcmOpChecker OpUncompLister created');
	var lg = null;
	var query_ok = true;
	// May return null (No pending), ArrayList of pending, error (server side error)
	try {
		lg = oul.queryUncompOp();
		_logger.debug('EcmOpChecker OpUncompLister.queryUncompOp queried');
	} catch (exc) {
		_logger.error("EcmOpChecker error " + exc);
		query_ok = false;
	}
    
    if (lg === null) {
		_logger.debug('No pending at ' + new Date());
    } else {
		// found pending
		load('custom/lib/mail.fs');
		var msg = null;
		if (query_ok) {
			var msg = new Message();
			msg.setSubject( "Ecm pending operations found" )
			msg.setServer( "smtp.intesasanpaolo.com" )
			msg.setSender( "noc_controllo.servizi.critici@intesasanpaolo.com", "NOC" );
			msg.addRecipient( "evelino.bomitali@hogwart.it", "Evelino Bomitali" )
			var msgtxt = '';
			for (i=0; i < lg.size(); i++) {
				   msgtxt = i + '. guid: ' + lg.get(i) + '\r\n';
				}
			msg.setBody( msgtxt )
			_logger.info("Sending email with pending list to: evelino.bomitali@hogwart.it");
		} else {
			// got error
			
			var msg = new Message();
			msg.setSubject( "Ecm pending operations returned error" )
			msg.setServer( "smtp.intesasanpaolo.com" )
			msg.setSender( "noc_controllo.servizi.critici@intesasanpaolo.com", "NOC" );
			msg.addRecipient( "evelino.bomitali@hogwart.it", "Evelino Bomitali" )
			msg.setBody ( "When checking pending, got error. See log");
			_logger.info("Sending email with error message to: evelino.bomitali@hogwart.it");
		}
		// Send the message.
    		msg.auth_send('87mail11468','EfADB2_A1dsBN');
    }
}());
