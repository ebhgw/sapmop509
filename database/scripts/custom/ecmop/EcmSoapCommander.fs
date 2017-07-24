/*
Script EcmSoapCommander.fs

Author: E. Bomitali, Hogwart, 2012
Tested with versions: 5.0

*/

load('custom/util/Properties.fs');

var EcmSoapCommander = (function () {

var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmsoap');
_logger.debug('Loading properties file');
//var props = Properties.load('ecmintegration');

var processAlarm = function (alarm, cmd) {
	try {
	    ecmop = EcmOpFactory.createEcmOpEnqueue(cmd, currUser, alarm.mc_ueid);
            // createCloseEvent(String user, String mc_ueid, String status)
            msg = ecmSoapClient.createCloseEvent(currUser, alarm.mc_ueid, alarm.status);
            respmsg = ecmSoapClient.call(msg,ecmEndpt);
            ecmop = ecmSoapClient.getEnqueuingResult(respmsg, ecmop);
			
	    _logger.debug('Success ' + ecmop.getEnqueuingresult_success() +', Guid ' + ecmop.getGuid());
            if (ecmop.getEnqueuingresult_success() == 'true') {
                _logger.info('Close Event enqueued successfully by user ' + currUser + ' on mc_ueid ' + alarm.mc_ueid
                    + ' with guid ' + ecmop.getGuid() + ' end point ' + ecmEndpt );
		    // add to pending request
				var pending = javaheo.PendingConcurrentMapFactory.getSingleton();
				pending.putIfAbsent(ecmop);
            } else {
                failcount++;
                _logger.warn('Close Event enqueuing failed, user ' + currUser + ' on mc_ueid ' + alarm.mc_ueid
                    + ' end point ' + ecmEndpt );
            }

        } catch (excp) {
            failOnError = true;
            _logger.error('Got error ' + excp);
        }
}

var retrieveActionList = function (parCellName, parEventHndlr, parSource, parMsg, parNodo, parServizioInfra, parSevPrec) {

    _logger.info('Ecm op retrieveActionList');
    var cellaRef, envStr ='';
    var now = new Date();

    if (parCellName == 'Allarmi') {
        cellaRef = '@10.2.230.229:7500#mc';
    } else if (parCellName != 'Allarmi_DOF') {
        cellaRef = '@10.2.230.229:8500#mc';
    } else {
        // should raise an error
    }

    // for the moment use the test environment
    cellaRef = '@10.2.231.110:7500#mc';

    cmdStr = 'E:\\application\\noc00\\cmd\\htaWrapper.bat ' + parSource + ' ' + parMsg  + ' ' + parNodo  + ' ' + parServizioInfra  + ' ' + parSevPrec;
	/*
	envArray = new Array();
    envArray.push('Source=' + parSource);
    envArray.push('msg=' + parMsg);
    envArray.push('Nodo=' + parNodo);
    envArray.push('Servizio_infrastrutturale='+parServizioInfra);
    envArray.push('Severita_precedente='+parSevPrec);
	*/
    //envStr = '[Source=' + parSource + ',msg=' + parMsg + ',Nodo=' + parNodo+',Servizio_infrastrutturale='+parServizioInfra+',Severita_precedente='+parSevPrec+']';

    var stream = formula.util.captureOutputStream( cmdStr);
    var output = new java.lang.String( formula.util.toByteArray( stream ) );
    _logger.info(output);
}

var _quote = function (str) {
		var ar = str.split("");
		var res = '';
		for (var i = 0; i<ar.length;i++) {
			ch = ar[i];
			if (ch == '"') {
				res = res + '\\\\"';
			}  else {
				res = res + ch;
			}
		}
		return res;
	}
	
// Uso: BEMNotes.exe <nome cella> mc_ueid userId testo
// Do not call directly BEMNotes.exec, use BEMNotesWrapper.bat instead to capture the errorlevel code and put in the output stream
// The mapping from cell name to actual address is performed within BEMNotes.exe

var executeAddNote = function (parCellName, parMcUeid, parUser, parText) {
    var cellaRef, envStr ='', errLev = '99';
    var now = new Date();
	_logger.debug('executeAddNote: note is >' + parText + '<');
	var stream, ba, output;

    //cmdStr = '"E:\\application\\noc00\\cmd\\BEMNotes\\BEMNotes.exe ' + parCellName + ' ' + parMcUeid  + ' ' + parUser + ' "' + parText + '"' + ' >NUL 2>&1" & "echo errorlevel=%errorlevel%' ;
	//cmdStr = 'E:\\application\\noc00\\cmd\\BEMNotes\\BEMNotes.exe ' + parCellName + ' ' + parMcUeid  + ' ' + parUser + ' "' + parText + '"' + ' >NUL 2>&1' ;
	//_logger.info('executeAddNote: cmd is ' + cmdStr);
	

	
	var cmd = new Array()
	cmd[0] = 'E:\\application\\noc00\\cmd\\BEMNotes\\BEMNotes.exe' 
	cmd[1] = parCellName;
	cmd[2] = parMcUeid;
	cmd[3] = parUser;
	cmd[4] = parText
	_logger.info('executeAddNote: executing BEMNotes on cell  ' + parCellName + ', mcueid ' + parMcUeid + ', user ' + parUser + ' note >' + parText + '<');
	
	//var runner = new Packages.com.hogw.CmdRunner();
	//var errLev = runner.exec(cmdStr);
	
	//var result = server.executeExternalProcess([cmdStr], []);
	var result = server.executeExternalProcess( cmd, []);
	var errLev = result.returnCode();
	//stream = formula.util.captureOutputStream( cmdStr);
	//ba = formula.util.toByteArray( stream );
    //output = new java.lang.String( ba );
    //_logger.debug('BEMNotes: ' + output);
    //var pattMatchErrLev = /errorlevel=([0-9]+)/
	
    //matchRes = pattMatchErrLev.exec(output);
	/*
    if (matchRes == null)
        errLev = '99'
    else
        errLev = matchRes[1]
	*/
	if (errLev == 0) {
		_logger.info('Error level is ' + errLev + '. Message : OK');
	} else {
		_logger.error('executeAddNote output: ' + result.getErrorsAsSingleString());
		_logger.error('executeAddNote error level is ' + errLev + ', message : ' + getErrorLevelMsg(errLev));
	}
    return errLev;
}

var getErrorLevelMsg = function (parErrLvl) {
    var msg = 'Not implemented yet'
    return msg;
}


	var getUserFromDname = function (parUser) {
		_logger.debug('User is ' + parUser);
		var patt = /user=(\w*)\/.*/
		var res = patt.exec(parUser);
		if (res == null) {
			_logger.error('Not found user ');
			return null;
		} else {
			return res[1];
		}
	}
	
	return {
		retrieveActionList: retrieveActionList,
		executeCmd: executeCmd,
		executeAddNote: executeAddNote,
		getErrorLevelMsg: getErrorLevelMsg,
		getUserFromDname:getUserFromDname
	}
})();
