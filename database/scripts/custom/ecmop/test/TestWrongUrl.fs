//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [ECM|EcmOp|Call Wrong URL]
// description=ECM|EcmOp|Call Wrong URL
// context=element
// target=script:true
// permission=manage
// type=serverscript
// operation=load('custom/ecm_soap/test/TestWrongUrl.fs');
//

// wrapping to avoid polluting the global env (and to force garbage collecting)
(function () {

	var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecm_soap.test');
	
	var clientScript = "alert('Chiamata al web service ECM fallita.\nL'operazione non è stata eseguita');"
	var _callUrl = function () {
        try {
            
            
            var oc = new Packages.com.hog.noc.ecmop.fs.SOAPMsgFromStringBuilder();
			var msg = oc.buildTest();
			Packages.com.hog.soap.Caller.call(msg, "http://127.0.0.1:10001/wrong/url.html");

        } catch (excp) {
			if (excp instanceof Packages.com.hog.noc.ecmop.EOCallException) {
				//confirm("Call to webservice failed. Contact support or try again later");
				session.invokeScript( 'Confirm message',
        clientScript,
        [ ],
        [ ] )
			}
			_logger.error("Test Wrong URL got error " + excp);
        }	
    };

	_logger.info('Operation test wrong url start');
	_callUrl();
	_logger.info('Operation test wrong url end');
	
}).call(this);