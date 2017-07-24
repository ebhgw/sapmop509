formula.log.error('Processing ' + alarm.Servizio);
var nomeServizio, nomeServizioGV, len;
nomeServizio = alarm.Servizio;
len = nomeServizio.length;
formula.log.error('Nome servizio ' + nomeServizio);
if (nomeServizio.substr(len-10, len) == 'End To End') {
    nomeServizioGV = nomeServizio.substr(0,len-11);
} else {
    nomeServizioGV = nomeServizio;
    nomeServizio = nomeServizio + ' Tech View';
}
alarm.ServizioGV = nomeServizioGV;
alarm.Servizio = nomeServizio;
formula.log.error('Set nome servizio gv');

true;