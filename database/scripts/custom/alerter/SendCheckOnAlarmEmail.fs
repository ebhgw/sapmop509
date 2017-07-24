/*

 Script SendCheckOnAlarmEmail.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Check Alarms and then send email

 */

(function () {
    load('custom/lib/Mailer.fs');
    load('custom/lib/Orgs.fs');
    load('custom/lib/underscore.js')
	
    //var cr = Packages.java.lang.System.getProperty("line.separator");
	//formula.log.info('Cr is ' + cr);
	var checkFail = false;
	
    // list of dn name to check
    var dn_list = new Array();
    dn_list.push('gen_folder=Produzione/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements'); // 0
    dn_list.push('gen_folder=SystemTest/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A+tAllarmiOUTv2/root=Elements'); // 1
    dn_list.push('gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'); // 2
    dn_list.push('gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'); // 3
	dn_list.push('ServizioGlobalView=TLC/gen_folder=Servizio+TLC/ISP_FolderBase=Production/root=Organizations'); // 4
    dn_list.push('gen_folder=Acronimi/ISP_FolderBase=SystemTest/root=Organizations');
    dn_list.push('gen_folder=Servizi/ISP_FolderBase=SystemTest/root=Organizations');
    dn_list.push('PortletView=Global+View/gen_folder=Grafici/Community=Go/gen_folder=Views/Dashboard=Dashboard/root=Organizations'); // 7
    dn_list.push('PortletView=Global+View/Community=Intesa+San+Paolo/gen_folder=Views/Dashboard=Dashboard/root=Organizations'); // 8
    dn_list.push('PortletView=Global+View/gen_folder=Grafici/Community=Supporto+Open/gen_folder=Views/Dashboard=Dashboard/root=Organizations'); // 9
	dn_list.push('ServizioGlobalView=INFRA+SYS/gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'); // 10

    var ele_list = new Array();
    _.each(dn_list, function(dn) { ele_list.push(formula.Root.findElement(dn))});

    var alrm_count_list = new Array();
    _.each(ele_list, function(ele) { alrm_count_list.push(ele.alarms.length)});

	// feed msg to blat that converts | to cr
    var msg = new Packages.java.lang.StringBuffer();
	msg.append("Report generated " +  new Date() + ' on ' + 	Packages.java.net.InetAddress.getLocalHost().getHostName() + '||'); 
    msg.append("On adapter " );
    msg.append("Produzione has\t" + alrm_count_list[0] + " alarms" + '|' + '|');
	msg.append("On Service Models" + '|');
    msg.append("Production/Acronimi " + alrm_count_list[2] + '|');
	if (alrm_count_list[0] != alrm_count_list[2]) {
		msg.append("   Alarm count on Production/Acronimi under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
		msg.append('|');
		msg.append('|');
		checkFail = true;
	}
	msg.append('|');
    msg.append("Production/Servizi " + alrm_count_list[3] + '|');
	msg.append("Production/Servizi TLC " + alrm_count_list[4] + '|');
	msg.append('|');
	if (alrm_count_list[3] + alrm_count_list[4] != alrm_count_list[2]) {
		msg.append("   Alarm count on Production/Servizi + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
		msg.append('|');
		msg.append('|');
				checkFail = true;
	}
	msg.append("Go/Global View " + alrm_count_list[7] + '|');
	if (alrm_count_list[7] + alrm_count_list[4] != alrm_count_list[2]) {
		msg.append("   Alarm count on Go/Global View + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
		msg.append('|');
		msg.append('|');
	}
    msg.append("ISP/Global View " + alrm_count_list[8] + '|');
	if (alrm_count_list[8] + alrm_count_list[4] != alrm_count_list[2]) {
		msg.append("   Alarm count on ISP/Global View + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
		msg.append('|');
		msg.append('|');
		checkFail = true;
	}
	// Supporto Open non ha INFRA SYS e TLC quindi bisogna escluderne gli allarmi
	// 9 Supporto Open, 4 TLC, 10 INFRA SYS, 2 Acronimi
    msg.append("Supporto Open/Global View " + alrm_count_list[9] + '|');
	if (alrm_count_list[9] + alrm_count_list[4] + alrm_count_list[10] != alrm_count_list[2]) {
		msg.append("   Alarm count on Supporto Open/Global View + Production/Servizi TLC under Service Models should equal alarms count on Produzione under Adapter tAllarmiOUTv2 - KO");
		msg.append('|');
		msg.append('|');
		checkFail = true;
	}
	msg.append('|');
	msg.append('|');
	msg.append("On adapter " );
	msg.append("SystemTest has\t" + alrm_count_list[1] + " alarms" + '|' + '|');
	msg.append("On Service Models" + '|');
    msg.append("SystemTest/Acronimi " + alrm_count_list[5] + '|');
		if (alrm_count_list[5] != alrm_count_list[1]) {
		msg.append("   Alarm count on SystemTest/Acronimi under Service Models should equal alarms count on SystemTest under Adapter tAllarmiOUTv2 - KO");
		msg.append('|');
		msg.append('|');
		checkFail = true;
	}
    msg.append("SystemTest/Servizi " + alrm_count_list[6] + '|');
	    msg.append("SystemTest/Acronimi " + alrm_count_list[5] + '|');
		if (alrm_count_list[6] != alrm_count_list[1]) {
		msg.append("   Alarm count on SystemTest/Servizi under Service Models should equal alarms count on SystemTest under Adapter tAllarmiOUTv2 - KO");
		msg.append('|');
		msg.append('|');
		checkFail = true;
	}
    msg.append( '|');
    
	if (checkFail) {
		msg.append('|');
		msg.append("   Consistency test between Adapter count and Service Model count failed");
		msg.append("   Please note that due to delay (up to one minute) between new element appearance within adapter and new element appearance in service models|");
		msg.append("   alarm count may differ until service model is aligned");
	} else {
	msg.append('|');
		msg.append("   Consistency test between Adapter count and Service Model count was successful");
	}
	
	var subject;
	if (checkFail) {
		subject = 'NOC Alarm count report - Check Failed'
	} else {
		subject = 'NOC Alarm count report - Check OK'
	} 
	
    Mailer.send('evelino.bomitali@hogwart.it,david.cacioli@hogwart.it', subject, msg);
})()