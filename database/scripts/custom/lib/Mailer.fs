/*

 Script Mailer.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Send email using blat exe

 */


var Mailer = (function () {

    load('mail/maillib.fs');
    load('custom/lib/underscore.js');
    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.mailer');
    // using blat to send email

    /* command sample
     E:\application\noc00\cmd\blat312>blat.exe - -to evelino.bomitali@hogwart.it -subject "test 1" -body "body test 2" -f noc
     _controllo.servizi.critici@intesasanpaolo.com -server smtp.intesasanpaolo.com -port 25 -try 5 -u 87mail114 -pw EfADB2_A1
     dsBN -log mail.log -timestamp
     */

    var msender = 'noc_controllo.servizi.critici@intesasanpaolo.com';
    var mserver = 'smtp.intesasanpaolo.com';
    var mport = '25';
    var mtry = '5';
    var muser = '87mail11468';
    var mpw = 'EfADB2_A1dsBN';
    var res = false;

    /* removed CmdRunner jar
     var _sendOnRunner = function (to, subj, body) {
     var cmdStr, argx;

     _logger.info('Sending email to ' + to + ' subj:' + subj + ' body: ' + body);
     cmdStr = 'cmd /C "E:\\application\\noc00\\cmd\\blat312\\blat.exe - ' + '-to ' + to + ' -subject "' + subj
     + '" -body "' + body + '" -f ' + msender + ' -server ' + mserver + ' -port ' + mport + ' -try 5 '
     + ' -u ' + muser + ' -pw ' + mpw + '"';
     _logger.info('Mailer.send: cmd is ' + cmdStr);
     argx = new Array();
     argx.push(cmdStr);

     var runner = new Packages.com.hogw.CmdRunner();
     var errLev = runner.exec(argx);

     if (errLev == 0) {
     res = true;
     _logger.info('Mailer.send: email sent successfully');
     } else {
     _logger.error('Mailer.send: email non sent, exit code ' + errLev);
     }
     return true;
     }
     */

    var _sendOnDaemonAPI = function (to, subj, body) {

        _logger.info('Sending email to ' + to + ' subj:' + subj + ' body: ' + body);

        var blatCmd = new Array();
        blatCmd.push('E:\\APPLICATION\\NOC00\\cmd\\blat312\\blat.exe');
        blatCmd.push('-');
        blatCmd.push('-to');
        blatCmd.push(to);
        blatCmd.push('-subject');
        blatCmd.push(subj);
        blatCmd.push('-body')
        blatCmd.push(body);
        blatCmd.push('-f');
        blatCmd.push(msender);
        blatCmd.push('-server');
        blatCmd.push(mserver);
        blatCmd.push('-port');
        blatCmd.push(mport);
        blatCmd.push('-try');
        blatCmd.push(mtry);
        blatCmd.push('-u');
        blatCmd.push(muser);
        blatCmd.push('-pw');
        blatCmd.push(mpw);

        /*
         blatCmd = 'E:\application\noc00\cmd\blat312>blat.exe - ' +
         '-to evelino.bomitali@hogwart.it ' +
         '-subject "test 1" ' +
         '-body "body test 2" ' +
         '-f noc_controllo.servizi.critici@intesasanpaolo.com ' +
         '-server smtp.intesasanpaolo.com ' +
         '-port 25 ' +
         '-try 5 ' +
         '-u 87mail114 ' +
         '-pw EfADB2_A1dsBN'
         */


        var result = server.executeExternalProcess( blatCmd, []);
        if (result.hasErrors()) {
            _logger.error('Blat return code is ' + result.returnCode() + ', msg ' + result.getErrorsAsSingleString());
        } else {
            _logger.debug('Blat returned ' + result.returnCode() + 'msg ' + result.getOutputAsSingleString());
        }
        return result.returnCode();
    }

    //
    var _sendOnMaillib = function (rlist, subj, body) {
        var res = false;

        try {
            // Make the message object
            var msg = new Message();
            msg.setSubject( "NOC mail message: " + subj );
            msg.setServer( mserver );
            msg.setSender( "noc_controllo.servizi.critici@intesasanpaolo.com", "NOC Daemon" );
            var rcl = '';

            _.each(rlist, function(to) { if (_.isString(to)) {
                var patt = /(.*)@/;
                msg.addRecipient( to, patt.exec(to)[1] );
            } else {
                msg.addRecipient( to.email, to.displayName );
            }
            });
            msg.setBody( body )
            // Send the message.
            _logger.info("Sending email via maillib");
            msg.send();
            res = true;
        } catch (exc) {
            _logger.error("Mailer.sendOnMaillib error " + exc);
        }
        return res;
    }

    return {
        send:_sendOnDaemonAPI,
        sendOnDaemonAPI:_sendOnDaemonAPI,
        sendOnMaillib:_sendOnMaillib
    }
}());