//
// Copyright (c) 2014 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// Operation to check if acronimo is within cmdb baseline
//
// [Sdbx|Check Acronimo in Baseline]
//	command=
//	context=element
// description=Sdbx|Check Acronimo in Baseline
//	operation=load('custom/sdbx/CheckSdbxAcronimoInBaseline.fs');\nCheckSdbxAcronimoInBaseline.askUser(element);
//	permission=manage
//	target=dname:gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=Sandbox.*/root=Organizations
//	type=serverscript
//

var CheckSdbxAcronimoInBaseline = (function (ele) {
    // Set logging
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.sdbx');
    load('custom/sdbx/SdbxAcronimoChecker.fs');
    load('custom/lib/Mailer.fs');
    load('custom/util/Properties.fs');

    var props = Properties.loadCustomConfig('sdbx');

    var _sendEmailRequest = function (list, env) {
        _logger.debug('Sending email for acronimo(s) ' + list.join());
		var to = '';
        var subject = 'Richiesta aggiunta acronimo/i al flusso di ' + env + ' per utilizzo nella sandbox';
        var msg = 'Si prega di aggiungere i seguenti acronimi ai dati del flusso di ' + env + ':||' + list.join('|')  + '||per utilizzo nella sandbox';
		if (env === 'SystemTest') {
            to = props.get('sdbx.systemtest.checkacronimo.email.to');
			//to ='evelino.bomitali@hogwart.it,controllo.servizi.critici@intesasanpaolo.com,piergiorgio.centenaro@intesasanpaolo.com';
		} else {
			// Produzione
			//to = 'evelino.bomitali@hogwart.it,controllo.servizi.critici@intesasanpaolo.com';
            to = props.get('sdbx.produzione.checkacronimo.email.to');
		}
        Mailer.send(to, subject, msg);
    }

    var _sendRequest4AcronimiNotInBaseline = function (ele) {
        // from ele check env
        _sendEmailRequest(_.map(SdbxAcronimoChecker.listNotInBaseline(ele), function (item) {return item.acro;}));
    }

    // closure on ele
    var _sendRequest4AcronimiNotInBaselineFromCallback = function (dn) {
        var res = SdbxAcronimoChecker.listNotInBaseline(dn);
        _sendEmailRequest(res.acroList, res.environment);
    }

    // Create our object which will be the "callback" from the client
    var callback =
    {
        ok: function (dn) {
            _sendRequest4AcronimiNotInBaselineFromCallback(dn);
            session.sendMessage('Inviata email di richiesta');
        },

        cancel: function () {
            session.sendMessage('Richiesta cancellata, nessuna email inviata');
        }
    }

    var _askUser  = function (ele) {

        var list = SdbxAcronimoChecker.listNotInBaseline(ele.DName).acroList;
        _logger.info('_askUser list length ' + list.length);
        // actually list is an array
        if (!_.isEmpty(list)) {
            var clientTicketScript = "\
    // @opt -1 \
    // @debug off \
	var msg = 'Gli acronimi " + list.join() + " non sono presenti nella baseline.\\nInviare una email per richiedere\\nl\\'aggiunta al flusso?\\n'\
    var res = confirm(msg);\
	if (res)\
		callback.ok(dn)\
	else\
		callback.cancel()\
"
            session.invokeScript( 'Ask Confirm',
                clientTicketScript,
                [ formula.util.makeRemote( callback ), element.DName ],
                [ 'callback', 'dn' ] )
        } else {
            message = "Acronimo/i presenti in baseline";
            _logger.debug( message );
            session.sendMessage( message );
        }

    }

    return {
        sendRequest:_sendRequest4AcronimiNotInBaseline,
        askUser:_askUser
    }
}())