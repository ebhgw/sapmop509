/*

 Script SendCheckOnAlarmEmail.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Check Alarms and then send email

 */

var chAlco = (function () {
    load('custom/lib/Mailer.fs');
    load('custom/lib/Orgs.fs');
    load('custom/lib/underscore.js')

    //var cr = Packages.java.lang.System.getProperty("line.separator");
    //formula.log.info('Cr is ' + cr);
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.checkalarmcount");

    var _checkCount = function () {
        var checkFail = false;

        // list of dn name to check
        var dn_list = new Array();
        dn_list.push('gen_folder=Produzione/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements'); // 0
        dn_list.push('gen_folder=SystemTest/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements'); // 1
        dn_list.push('gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'); // 2
        dn_list.push('gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'); // 3
        dn_list.push('ServizioGlobalView=TLC/gen_folder=Servizio+TLC/ISP_FolderBase=Production/root=Organizations'); // 4
        dn_list.push('gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations');
        dn_list.push('gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations'); // 6
        dn_list.push('PortletView=Global+View/gen_folder=Grafici/Community=Go/gen_folder=Views/Dashboard=Dashboard/root=Organizations'); // 7
        dn_list.push('PortletView=Global+View/Community=Intesa+San+Paolo/gen_folder=Views/Dashboard=Dashboard/root=Organizations'); // 8
        dn_list.push('PortletView=Global+View/gen_folder=Grafici/Community=Supporto+Open/gen_folder=Views/Dashboard=Dashboard/root=Organizations'); // 9
        dn_list.push('ServizioGlobalView=INFRA+SYS/gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'); // 10

        var ele_list = new Array();
        _.each(dn_list, function(dn) { ele_list.push(formula.Root.findElement(dn));});

        var alrm_count_list = new Array();
        _.each(ele_list, function(ele) { alrm_count_list.push(ele.alarms.length)});

        // debug
        _.each(ele_list, function(ele) { _logger.info(ele.Dname + ':' + ele.alarms.length) });

        var res = {
            allarmisuelementi_prod_ko: null, //0
            allarmisuelementi_systemtest_ko: null,
            orgs_prod_acronimi_ko: null, // 2
            orgs_prod_servizi_ko: null, // 3
            orgs_prod_servizio_tlc_ko: null, //4
            diff_orgs_prod_acronimi_vs_servizi_servizio_tlc: null,
            orgs_systemtest_acronimi_ko: null,
            orgs_systemtset_servizi_ko: null, // 6
            vw_go_globalview_ko: null,
            diff_vw_go_globalview_vs_orgs_prod_acronimi_plus_orgs_prod_servizio_tlc: null,
            vw_isp_globalview_ko: null, //8
            vw_supportopen_globalview_ko: null,
            orgs_prod_servizi_infrasys_ko: null, // 10
            allarmisuelementi_prod_count: alrm_count_list[0],
            allarmisuelementi_systemtest_count: alrm_count_list[1],
            orgs_prod_acronimi_count: alrm_count_list[2], // 2
            orgs_prod_servizi_count: alrm_count_list[3],
            orgs_prod_servizio_tlc_count: alrm_count_list[4], //4
            orgs_systemtest_acronimi_count: alrm_count_list[5],
            orgs_systemtset_servizi_count: alrm_count_list[6], // 6
            vw_go_globalview_count: alrm_count_list[7],
            vw_isp_globalview_count: alrm_count_list[8], //8
            vw_supportopen_globalview_count: alrm_count_list[9],
            orgs_prod_servizi_infrasys_count: alrm_count_list[10], // 10
            flag_fail: false
        }


        // 2 orgs_prod_acronimi, 0 allarmisuelementi_prod
        if (alrm_count_list[2] != alrm_count_list[0]) {
            res.orgs_prod_acronimi_ko = true;
            res.flag_fail = true;
            // find which are in acronimi but not in servizi + servizio_tlc

            var allarmisuelementi_alarms = ele_list[0].alarms;
            //_logger.info('Found ' + ele_list[0].alarms.length + ' alarms on ' + ele_list[0]);
            var allarmisuelementi_id_receviced_alarm = _.map(allarmisuelementi_alarms, function(al) { return parseInt(al.IDReceivedAlarm) });
            //_logger.info('allarmisuelementi_id_receviced_alarm: ' + allarmisuelementi_id_receviced_alarm.join(','));

            var acronimi_alarms = ele_list[2].alarms;
            //_logger.info('Found ' + ele_list[2].alarms.length + ' alarms on ' + ele_list[2]);
            var acronimi_id_receviced_alarm = _.map(acronimi_alarms, function(al) {
                //_logger.info(parseInt(al.IDReceivedAlarm));
                return parseInt(al.IDReceivedAlarm) });
            //_logger.info('acronimi_id_receviced_alarm: ' + acronimi_id_receviced_alarm.join(','));

            res.diff_allarmisuelementi_vs_orgs_prod_acronimi = _.difference(allarmisuelementi_id_receviced_alarm,  acronimi_id_receviced_alarm);
            //_logger.info('res.diff_allarmisuelementi_vs_orgs_prod_acronimi: ' + res.diff_allarmisuelementi_vs_orgs_prod_acronimi.join(','));
        }
        // 3 orgs_prod_servizi, 4 orgs_prod_servizio_tlc, 2 orgs_prod_acronimi
        if (alrm_count_list[3] + alrm_count_list[4] != alrm_count_list[2]) {
            res.orgs_prod_servizi_ko = true;
            res.flag_fail = true;
            // find which are in acronimi but not in servizi + servizio_tlc
            var acronimi_alarms = ele_list[2].alarms;
            var servizi_alarms = ele_list[3].alarms;
            var servizio_tlc_alarms = ele_list[4].alarms;
            var acronimi_id_receviced_alarm = _.map(acronimi_alarms, function(al) { return parseInt(al.IDReceivedAlarm) });
            var servizi_id_receviced_alarm = _.map(servizi_alarms, function(al) { return parseInt(al.IDReceivedAlarm) });
            var servizio_tlc_id_receviced_alarm = _.map(servizio_tlc_alarms, function(al) { return parseInt(al.IDReceivedAlarm) });
            res.diff_orgs_prod_acronimi_vs_servizi_plus_servizio_tlc = _.difference(acronimi_id_receviced_alarm,  _.union(servizi_id_receviced_alarm, servizio_tlc_id_receviced_alarm));
        }

        // 5 orgs_systemtest_acronimi, 1 allarmisuelementi_systemtest
        if (alrm_count_list[5] != alrm_count_list[1]) {
            res.orgs_systemtest_acronimi_ko = true;
            res.flag_fail = true;
        }

        // 6 orgs_systemtset_servizi, 1 allarmisuelementi_systemtest
        if (alrm_count_list[6] != alrm_count_list[1]) {
            res.orgs_systemtset_servizi_ko = true;
            res.flag_fail = true;
        }

        // 7 vw_go_globalview, 4 orgs_prod_servizio_tlc, 2 orgs_prod_acronimi
        if (alrm_count_list[7] + alrm_count_list[4] != alrm_count_list[2]) {
            res.vw_go_globalview_ko = true;
            res.flag_fail = true;
            var vw_go_globalview_alarms = ele_list[7].alarms;
            var orgs_prod_acronimi_alarms = ele_list[2].alarms;
            var orgs_prod_servizio_tlc = ele_list[4].alarms;

            var orgs_prod_acronimi_id_receviced_alarm = _.map(orgs_prod_acronimi_alarms, function(al) { return parseInt(al.IDReceivedAlarm) });
            var vw_go_globalview_id_received_alarm = _.map(vw_go_globalview_alarms, function(al) { return parseInt(al.IDReceivedAlarm) });
            var orgs_prod_servizio_tlc_id_receviced_alarm = _.map(orgs_prod_acronimi_alarms, function(al) { return parseInt(al.IDReceivedAlarm) });
            res.diff_vw_go_globalview_vs_orgs_prod_acronimi_plus_orgs_prod_servizio_tlc = _.difference(orgs_prod_acronimi_alarms,  _.union(vw_go_globalview_id_received_alarm, orgs_prod_servizio_tlc_id_receviced_alarm));
        }

        // 8 vw_isp_globalview, 4 orgs_prod_servizio_tlc, 2 orgs_prod_acronimi
        if (alrm_count_list[8] + alrm_count_list[4] != alrm_count_list[2]) {
            res.vw_isp_globalview_ko = true;
            res.flag_fail = true;
        }
        // Supporto Open non ha INFRA SYS e TLC quindi bisogna escluderne gli allarmi
        // 9 Supporto Open, 4 TLC, 10 INFRA SYS, 2 Acronimi
        // 9 vw_supportopen_globalview, 10 orgs_prod_servizi_infrasys, 2 orgs_prod_acronimi
        // Alex lo ha aggiunto, da verificare se ? corretto o meno
        // originale senza INFRA SYS if (alrm_count_list[9] + alrm_count_list[4] + alrm_count_list[10] != alrm_count_list[2]) {
        if (alrm_count_list[9] + alrm_count_list[4] != alrm_count_list[2]) {
            res.vw_supportopen_globalview_ko = true;
            res.flag_fail = true;
        }


        return res;
    }


    // buil report builds a report message where lines are divided by |. This allows for changing the line separator later
    var _buildReport = function (testRes) {

        // feed _msg to blat that converts | to cr
        var _msg = new Packages.java.lang.StringBuffer();
        _msg.append("Report generated " +  new Date() + ' on ' + Packages.java.net.InetAddress.getLocalHost().getHostName() + '||');
        _msg.append("On adapter " );
        _msg.append("Produzione has\t" + testRes.allarmisuelementi_prod_count + " alarms" + '|' + '|');
        _msg.append("On Service Models" + '|');
        _msg.append("Production/Acronimi " + testRes.orgs_prod_acronimi_count + '|');

        // 2
        if (testRes.orgs_prod_acronimi_ko) {
            _msg.append("   Alarm count on Production/Acronimi under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
            _msg.append('|');
            _msg.append(">>> Lacking IDReceivedAlarm: " + testRes.diff_allarmisuelementi_vs_orgs_prod_acronimi.join(',') + '|');
            _msg.append('|');
            _msg.append('|');
        }

        _msg.append('|');
        _msg.append("Production/Servizi " + testRes.orgs_prod_servizi_count + '|');
        _msg.append("Production/Servizi TLC " + testRes.orgs_prod_servizio_tlc_count + '|');
        _msg.append('|');
        // 3
        if (testRes.orgs_prod_servizi_ko) {
            _msg.append("   Alarm count on Production/Servizi + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
            _msg.append('|');
            _msg.append(">>> Lacking IDReceivedAlarm: " + testRes.res.diff_orgs_prod_acronimi_vs_servizi_plus_servizio_tlc.join(',') + '|');
            _msg.append('|');
            _msg.append('|');
        }

        _msg.append("Go/Global View " + testRes.vw_go_globalview_count + '|');
        // 7
        if (testRes.vw_go_globalview_ko) {
            _msg.append("   Alarm count on Go/Global View + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
            _msg.append('|');
            _msg.append('|');
        }
        _msg.append("ISP/Global View " + testRes.vw_isp_globalview_count + '|');
        // 8
        if (testRes.vw_isp_globalview_ko) {
            _msg.append("   Alarm count on ISP/Global View + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
            _msg.append('|');
            _msg.append('|');
        }

        // Supporto Open non ha INFRA SYS e TLC quindi bisogna escluderne gli allarmi
        // 9 Supporto Open, 4 TLC, 10 INFRA SYS, 2 Acronimi
        _msg.append("Supporto Open/Global View " + testRes.vw_supportopen_globalview_count + '|');
        if (testRes.vw_supportopen_globalview_ko) {
            _msg.append("   Alarm count on Supporto Open/Global View + Production/Servizi TLC  + Production/Servizi/INFRA SYS under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
            _msg.append('|');
            _msg.append('|');
        }

        // ------------- System Test -------------
        _msg.append('|');
        _msg.append('|');
        _msg.append("On adapter " );
        _msg.append("SystemTest has\t" + testRes.allarmisuelementi_systemtest_count + " alarms" + '|' + '|');
        _msg.append("On Service Models" + '|');
        _msg.append("SystemTest/Acronimi " + testRes.orgs_systemtest_acronimi_count + '|');
        if (testRes.orgs_systemtest_acronimi_ko) {
            _msg.append("   Alarm count on SystemTest/Acronimi under Service Models should equal alarms count on SystemTest under Adapter tAllarmiOUTv2 - KO");
            _msg.append('|');
            _msg.append('|');
        }

        _msg.append("SystemTest/Servizi " + testRes.orgs_systemtset_servizi_count + '|');
        if (testRes.orgs_systemtset_servizi_ko) {
            _msg.append("   Alarm count on SystemTest/Servizi under Service Models should equal alarms count on SystemTest under Adapter tAllarmiOUTv2 - KO");
            _msg.append('|');
            _msg.append('|');
        }
        _msg.append( '|');

        if (testRes.flag_fail) {
            _msg.append('|');
            _msg.append("   Consistency test between Adapter count and Service Model count failed");
            _msg.append("   Please note that due to delay (up to one minute) between|new element appearance within adapter and new element appearance in service models|");
            _msg.append("   alarm count may differ until service model is aligned");
        } else {
            _msg.append('|');
            _msg.append("   Consistency test between Adapter count and Service Model count was successful");
        }

        var _subj;
        if (testRes.flag_fail) {
            _subj = 'NOC Alarm count report - Check Failed on ' + Packages.java.net.InetAddress.getLocalHost().getHostName();
        } else {
            _subj = 'NOC Alarm count report - Check OK on ' + Packages.java.net.InetAddress.getLocalHost().getHostName();
        }
        return {
            subject: _subj,
            message: _msg + ''
        }
    }

    var _buildEmailMessage = function (testRes) {
        var report = _buildReport(testRes);
        _logger.debug('CheckAlarmCountv2, buildEmailMessage\n' + report);
        return report;
    }

    var _buildPopUpMessage = function (testRes) {

        var report =  _buildReport(testRes);
        var cr = Packages.java.lang.System.getProperty("line.separator");
        _logger.debug(report.message);
        _logger.debug(report.message.replace(/\|/g,'>'));


        return {
            subject: report.subject,
            message: report.message.replace(/\|/g, cr)
        }
    }

    var _buildPopUpMessageSave = function (testRes) {

        var cr = Packages.java.lang.System.getProperty("line.separator");
        var _msg = new Packages.java.lang.StringBuffer();
        _msg.append("------ Produzione ------" + cr)
        _msg.append("On adapter Produzione has\t" + testRes.allarmisuelementi_prod_count + " alarms" + cr);
        _msg.append("Production/Acronimi " + testRes.orgs_prod_acronimi_count + cr);

        // 2
        if (testRes.orgs_prod_acronimi_ko) {
            _msg.append(">>> Alarm count on Production/Acronimi under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO" + cr);
            _msg.append(">>> Lacking IDReceivedAlarm: " + testRes.diff_allarmisuelementi_vs_orgs_prod_acronimi.join(',') + cr);
        }

        _msg.append("Production/Servizi " + testRes.orgs_prod_servizi_count + cr);
        _msg.append("Production/Servizi TLC " + testRes.orgs_prod_servizio_tlc_count + cr);
        // 3
        if (testRes.orgs_prod_servizi_ko) {
            _msg.append(">>> Alarm count on Production/Servizi + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO" + cr);
            _msg.append(">>> Lacking IDReceivedAlarm: " + testRes.res.diff_orgs_prod_acronimi_vs_servizi_plus_servizio_tlc.join(',') + cr);

        }

        _msg.append("Views Go/Global View " + testRes.vw_go_globalview_count + cr);
        // 7
        if (testRes.vw_go_globalview_ko) {
            _msg.append(">>> Alarm count on Go/Global View + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO" + cr);
        }
        _msg.append("Views ISP/Global View " + testRes.vw_isp_globalview_count + cr);
        // 8
        if (testRes.vw_isp_globalview_ko) {
            _msg.append(">>>  Alarm count on ISP/Global View + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO" + cr);
        }

        // Supporto Open non ha INFRA SYS e TLC quindi bisogna escluderne gli allarmi
        // 9 Supporto Open, 4 TLC, 10 INFRA SYS, 2 Acronimi
        _msg.append("Views Supporto Open/Global View " + testRes.vw_supportopen_globalview_count + cr);
        if (testRes.vw_supportopen_globalview_ko) {
            _msg.append(">>> Alarm count on Supporto Open/Global View + Production/Servizi TLC  + Production/Servizi/INFRA SYS under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO"  + cr);
        }

        // ------------- System Test -------------
        _msg.append(cr);
        _msg.append("------ SystemTest ------" + cr)
        _msg.append("On adapter SystemTest has\t" + testRes.allarmisuelementi_systemtest_count + " alarms" + cr);
        _msg.append("SystemTest/Acronimi " + testRes.orgs_systemtest_acronimi_count + cr);
        if (testRes.orgs_systemtest_acronimi_ko) {
            _msg.append(">>> Alarm count on SystemTest/Acronimi under Service Models should equal alarms count on SystemTest under Adapter tAllarmiOUTv2 - KO");
        }

        _msg.append("SystemTest/Servizi " + testRes.orgs_systemtset_servizi_count + cr);
        if (testRes.orgs_systemtset_servizi_ko) {
            _msg.append(">>> Alarm count on SystemTest/Servizi under Service Models should equal alarms count on SystemTest under Adapter tAllarmiOUTv2 - KO");
        }

        var _subj;
        if (testRes.flag_fail) {
            _subj = 'NOC Alarm count report - Check Failed'
        } else {
            _subj = 'NOC Alarm count report - Check OK'
        }
        _logger.info('subj is ' + _subj);
        _logger.info('msg is ' + _msg);
        return {
            subject: _subj,
            message: _msg + ''
        }
    }

    var _checkAndEmail = function (force) {
        force = force||false;

        var tr = _checkCount();
        var trx = null;
        if (force) {
            var txt = _buildEmailMessage(trx==null?tr:trx);
            _logger.info('CheckAlarmCountv2, force send');
            Mailer.send('evelino.bomitali@hogwart.it,david.cacioli@hogwart.it', txt.subject, txt.message);
        } else if (tr.flag_fail) {
            _logger.info('CheckAlarmCountv2, first failure, sleeping 65 secs');
            if (tr.allarmisuelementi_prod_count != tr.orgs_prod_acronimi_count) {
                Packages.java.lang.Thread.sleep(65000);
                trx = _checkCount();
            }
            if (tr.flag_fail && trx == null||tr.flag_fail && trx.flag_fail) {
                var txt = _buildEmailMessage(trx==null?tr:trx);
                _logger.info('CheckAlarmCountv2, checkAndEmail second failure and sending email');
                Mailer.send('evelino.bomitali@hogwart.it,david.cacioli@hogwart.it,marcello.melzi@hogwart.it,alessandro.trebbi@hogwart.it', txt.subject, txt.message);
            }
        }
    }

    var _checkAndPopup = function () {
        _logger.info('CheckAlarmCount.checkAndPopup');
        var tr = _checkCount();
        var txt = _buildPopUpMessage(tr);
        session.sendMessage(txt.subject +'\n\n'+ txt.message)
    }

    return {
        checkAndPopUp: _checkAndPopup,
        checkAndEmail: _checkAndEmail
    }

}())