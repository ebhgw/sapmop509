/*

 Script ChecktAllarmiOUTv2.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Check Bdi Schedule and then send email

 */

(function () {
    load('custom/lib/Mailer.fs');
    load('custom/lib/Orgs.fs');
    load('custom/lib/underscore.js')
    load('custom/lib/moment.js');

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.tallarmitoutv2");
    var checkDtAttr = function (dn, attr, threshold) {
        var res = null;
        var ele = Orgs.findElement(dn);
        if (ele) {
            try {
                var checkDate = moment(ele[attr]);
                _logger.debug('Alarm date ' + checkDate);
                var now = moment(new Date());
                _logger.debug('Checking at ' + now.format('DD-MM-YYYY HH:mm:ss'));
                _logger.debug('Attr ' + attr + ' date is ' + checkDate.format('DD-MM-YYYY HH:mm:ss'));
                var diff = now.diff(checkDate, 'seconds');
                _logger.debug('Diff is ' + diff + ' checked against ' + threshold);
                if (diff > threshold) {
                    res = {
                        now: now,
                        lastUpdate: checkDate,
                        diff: diff,
                        checkElementDn: dn,
                        threshold: threshold,
                    }
                }
            } catch (excp) {
                _logger.error('ChecktAllarmiOUTv2, checkDtAttr excp ' + excp)
            }
        }
        return res;
    }

    // Check last export dt on AllarmiSuElementi
    var allarmisuelementiDn = 'AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements';

    // on Adapter Runtime Information, collecting RunOnce schedule alarms
    var msg = '';
    var checkRes = checkDtAttr(allarmisuelementiDn, 'LastExportDt', 1800);

    if (checkRes != null) {
        // schedule maybe stopped
        _logger.info('Sending email, too old update detected. Difference is ' + checkRes.diff + ' seconds, threshold is ' + checkRes.threshold);
        msg = new Packages.java.lang.StringBuffer();
        msg.append("Report generated " +  checkRes.now.format('DD-MM-YYYY HH:mm:ss') + ' on ' + 	Packages.java.net.InetAddress.getLocalHost().getHostName() + '||');
        msg.append('Last update too old. Last alarm update was received on ' + checkRes.lastUpdate.format('DD-MM-YYYY HH:mm:ss') + ' (Diff is ' + checkRes.diff + ' seconds)');
        Mailer.send('evelino.bomitali@hogwart.it', 'tAllarmiOUTv2 last alarm update too old', msg);
    }

})()