//
// Copyright (c) 2010 Hogwart srl.  All Rights Reserved.
//
// Author: E. Bomitali
//
// According to exception type, send messages to console
//
//

var ConsoleMessenger = (function () {

    var _sendAlert = function (msg) {
        msg = msg.replace(/'/g, "\\'");
        msg = msg.replace(/"/g, "\\");
        var clientScript = "alert('" + msg + "');"
        session.invokeScript( 'Confirm message',
            clientScript,
            [ ],
            [ ] )
    }

    var _deliver = function (exc) {
        if (exc instanceof excp instanceof Packages.com.hog.noc.ecmop.EOCallException) {
            session.sendMessage("Chiamata al web service ECM fallita.\nL'operazione non e' stata eseguita");
            //sendAlert("Chiamata al web service ECM fallita.\nL'operazione non e' stata eseguita")
        } else if (exc instanceof excp instanceof Packages.com.hog.noc.ecmop.EOSOAPFaultException) {
            session.sendMessage("Chiamata al web service ha risposto con un fault.\n" + exc.getMessage());
            //sendAlert("Chiamata al web service ha risposto con un fault.\n" + exc.getMessage());
        } else {
            session.sendMessage("Internal error\n" + exc.getMessage() + "\nContact administrator");
        }

    }

    return {
        deliver:_deliver
    }
})