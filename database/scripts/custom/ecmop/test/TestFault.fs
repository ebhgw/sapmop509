//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// Operation definitions to add to an element: (paste into Operations.ini)
//
// [ECM|Add Ticket]
// description=ECM|Add Ticket Multiple
// context=alarm
// target=dname:root=Organizations
// permission=manage
// type=serverscript
// operation=load( "custom/ecm_integration/AddTicket.fs" );
//

// wrapping to avoid polluting the global env (and to force garbage collecting)
(function () {

	var _logger = Packages.org.apache.log4j.Logger.getLogger('com.hog.noc');
	var _callFault = function () {
        try {
            _logger.info('Operation test fault');
            
            var oc = Packages.com.hog.noc.ecmop.fs.SOAPFaultRaiser.raise();

        } catch (excp) {
			if (excp instanceof Packages.com.hog.noc.EcmOpException) {
            _logger.error('Test Fault found EcmOpExcpetion ' + excp);
			_logger.error('Type ' + excp.getType());
			} else {
			_logger.error('Test Fault unknown excp type' + excp);
			}
        }
    };

	_callFault();
	
}).call(this);