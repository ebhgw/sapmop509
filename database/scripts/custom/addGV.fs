/*
  Script addGV.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  modl test script that add global view service name
*/

/*
var now = new Date();
var devLogger = Packages.org.apache.log4j.Logger.getLogger("Dev");
devLogger.debug("addGV starting " + now);
*/

// copy alarm to javascript space
//var myAlarm = alarm;

/*
 1. se nome del servizio termina in EndToEnd strippa EndToEnd e copia il nome del servizio in ServizioGV
 2. altrimenti copia solo servizio in servizioGV e aggiunge TechView al nome del servizio
 */

formula.log.error('Processing alarm 1');
var nomeServizio, nomeServizioGV, len;
nomeServizio = element.alarm[0].Servizio;
len = nomeServizio.length;
formula.log.error('Processing alarm 1');
formula.log.error('Processing alarm for ' + nomeServizio);
if (nomeServizio.substr(len-10, len) = 'EndToEnd') {
    nomeServizioGV = nomeServizio.substr(0,len-11);
} else {
    nomeServizioGV = nomeServizio;
    nomeServizio = nomeServizio + ' TechView';
}
formula.log.error('Assigning ServizioGV ' + nomeServizioGV);
//alarm.ServizioGV = nomeServizioGV;
//alarm.Servizio = nomeServizio;
true;