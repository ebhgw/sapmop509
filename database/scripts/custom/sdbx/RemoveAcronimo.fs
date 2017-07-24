//
// Copyright (c) 2014 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// Operation to check if acronimo is within cmdb baseline
//
// [Sdbx|Remove Acronimo]
//	command=
//	context=element
// description=Sdbx|Remove Acronimo
//	operation=load('custom/sdbx/RemoveAcronimo.fs');
//	permission=manage
//	target=dname:gen_folder=Acronimi/gen_folder=Staging/ISP_FolderBase=Sandbox.*/root=Organizations
//	type=serverscript
//

//

(function () {
// Set logging
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.sdbx');

    try {
        load('custom/sdbx/SdbxAcronimoChecker.fs');
        // first verify that acronimo is not used under any other service
        var acroDn = element.DName;
        var acroName = element.Name;
        var ctx = SdbxAcronimoChecker.getContext(acroDn)
        var used = SdbxAcronimoChecker.isAcronimoReferredByServizio(acroDn);

        // do delete before sending email in case an error happens
        state.Orgs.deleteElement(element);

        if (!used) {

            load('custom/util/Properties.fs');
            var props = Properties.loadCustomConfig('sdbx');
            load('custom/lib/Mailer.fs');

            var _sendEmailRequest = function (acroName, env) {
                _logger.debug('Sending email for removed acronimo ' + acroName);
                var to = '';
                var subject = 'Rimozione acronimo dal flusso di ' + env;
                var msg = "Rimozione dalla sandbox dell'acronimo " + acroName + " non riferito in alcun servizio nell'ambiente " + env;
                if (env === 'SystemTest') {
                    to = props.get('sdbx.systemtest.remove.email.to');
                    //to ='evelino.bomitali@hogwart.it,controllo.servizi.critici@intesasanpaolo.com,piergiorgio.centenaro@intesasanpaolo.com';
                } else {
                    // Produzione
                    //to = 'evelino.bomitali@hogwart.it,controllo.servizi.critici@intesasanpaolo.com';
                    to = props.get('sdbx.produzione.remove.email.to');
                }
                Mailer.send(to, subject, msg);
            }

            _sendEmailRequest(ctx.acroName,ctx.environment);
        }
    } catch (exc) {
        _logger.error('RemoveAcronimo excp ' + exc);
    }
})();