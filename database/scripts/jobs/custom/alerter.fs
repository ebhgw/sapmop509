try {
load('custom/alerter/CheckAlarmCount.fs');
chAlco.checkAndEmail();
} catch (excp) {
	formula.log.error('Job Alerter failed on CheckAlarmCount excp: ' + excp);
}
try {
load('custom/alerter/CheckAlarmDate.fs');
checkAlarmDate.checkLastScheduleCompleted();
checkAlarmDate.checkLastExport();
} catch (excp) {
	formula.log.error('Job Alerter failed on CheckAlarmDate excp: ' + excp);
}

