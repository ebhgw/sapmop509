/*
Script execute_cmd.fs

Author: E. Bomitali, Hogwart, 2012
Tested with versions: 5.0

Wrapper to run msetmsg command.
It uses
   Cell Name: name of ECM cell
   Event Handle: key of event in ECM

Cell Name sent by Mauro is actually a cell name, we should convert 
cell name to cell reference in order to pass it as a paramenter to msetMsg.
Command returns result via errorlevel variable that should be checked immediately
after command execution (in the same context)
    echo %errorlevel%

 If there is an error, %errorlevel% != 0
 Note that closing a non-existent alarm is not an error

*/

load('custom/util/Properties.fs');

var ecmCommander = (function () {

var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmCommander');
_logger.debug('Loading properties file');
var props = Properties.load('ecmintegration');

// Nota: in js i parametri sono opzionali e si puo' chiamare la funzione
// con un numero di parametri inferiore
var executeCmd = function (parCellName, parEventHandle, parUser, parAction, parTicket) {

    var cellaRef, flag, cmdStr = '', parStr = '', matchRes = '';
    var errLev = '99';
	var cellAllarmi = props.get('Allarmi');
	var cellAllarmiDof = props.get('Allarmi_DOF');
	var ambiente = props.get('Ambiente');

    if (parCellName == 'Allarmi') {
        cellaRef = cellAllarmi;
    } else if (parCellName != 'Allarmi_DOF') {
        cellaRef = cellAllarmiDof;
    } else {
		_logger.error("Unknown cell name " + parCellName);
		throw "Unknown cell name " + parCellName;
        // should raise an error
    }
	_logger.info('Ecm op ' + parAction + ' on ' + parCellName + ' (ambiente ' + ambiente + ' cell ' + cellaRef + ') event_handle ' + parEventHandle + ' by user ' + parUser);
	
    if (parAction == 'ASSIGN') {
        flag = '-G'
    } else if (parAction == 'CLOSE') {
        flag = '-C'
    } else if (parAction == 'DECLINE') {
        flag = '-A'
	    // if op is Decline, user should be empty
	    parUser = ''
    } else if (parAction == 'TICKET') {
        flag = '-G'
	} else if (parAction == 'ADD2TICKET') {
        flag = '-G'
    } else {
		_logger.error("Unknown action name " + parAction);
		throw "Unknown action name " + parAction;
        //should rise an error
    }

	// in test we have only data for Allarmi cell name, cannot test Allarmi_DOF
    // for the moment use the test environment, when deploying to production simply comment the next line
	if (ambiente != 'Prod' && parCellName == 'Allarmi_DOF') {
		_logger.info('The alarm have no test cell');
		return;
	}

    /*
	 * Case ASSIGN
	 * msetmsg -n @10.2.230.229:7500#mc -S "mc_owner='xxxx';itsm_operational_category1='timestampcorrente';itsm_operational_category2='ASSIGN'"
	 *
	 * Case TICKET
	 * -S "EAC_TO_REMEDY=1;mc_owner='admin'"
	 *
	 * Case ADD2TICKET
	 * msetmsg -n @10.2.230.229:7500#mc -G -S "mc_owner='U0F4564';mc_long_msg='Associato TicketNr:25804621 da:U0F4564';Ticket_Num='25804621' " -u 8578
	 *
	 */
	 
    parStr = 'mc_owner=\'' + parUser + '\'"';
    // Ticket requires to add parameter EAC_TO_REMEDY for a new ticket, ow don't add flag
    if (parAction == 'TICKET')  {
        parStr = '"EAC_TO_REMEDY=1;' + parStr;
    } else if (parAction == 'ADD2TICKET') {
		if (parTicket.length > 0) {
			// remove ticket
			parStr = '"mc_long_msg=\'Associato TicketNr:' + parTicket + ' da:' + parUser +'\';Ticket_Num=\''+ parTicket +'\';'  + parStr;
		} else {
			// add ticket
			parStr = '"mc_long_msg=\'\';Ticket_Num=\'\';'  + parStr;
		}
	} else {
        parStr = '"' + parStr;
    }

    cmdStr = 'cmd /C E:\\application\\noc00\\cmd\\msetmsgWrapper.bat -n ' + cellaRef 
				+ ' ' + flag + ' -u ' + parEventHandle + ' -S ' + parStr + ' 2>&1 ';
    // example to show how set env variables
    // var stream = formula.util.captureOutputStream( 'cmd /c echo %FOO%', [ 'foo=bar' ] );
    _logger.info('cmd is ' + cmdStr);
    var stream = formula.util.captureOutputStream( cmdStr );
	var ba = formula.util.toByteArray( stream );
	//_logger.info(">>" + ba);
    var output = new java.lang.String( ba );
    //_logger.info('>>> ' + output);

	// Check the error level returned as output from ecm command
    var pattMatchErrLev = /errorlevel=([0-9]+)/
    matchRes = pattMatchErrLev.exec(output);
    if (matchRes == null)
        errLev = '99'
    else
        errLev = matchRes[1]
	if (errLev == 0) {
		_logger.info('Error level is ' + errLev + '. Message : OK');
	} else {
		_logger.error('Error level is ' + errLev + '. Message : ' + getErrorLevelMsg(errLev));
	}
    return errLev;
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

/*
 Code       Description
 0      success
 1      bad usage (command includes nonexistent options or an invalid combination of
 options and arguments)
 10     initialization failure
 11     trace initialization failed
 12     configuration initialization failed
 13     outbound communication setup failed
 14     inbound communication setup failed
 15     message handling initialization failed
 16     persistency setup failed
 17     port range limitation failed
 20     connection to cell failed
 25     memory fault
 26     command failed
 27     syntax error
 28     invalid answer received
 32		inexistent event_handle
 */
var getErrorLevelMsg = function (parErrLvl) {
    var msg;
    switch (parErrLvl)
    {
        case '0':
            msg="success";
            break;
        case '1':
            msg="bad usage (nonexistent options or an invalid combination of options and arguments)";
            break;
        case '10':
            msg="initialization failure";
            break;
        case '11':
            msg="trace initialization failed";
            break;
        case '12':
            msg="configuration initialization failed";
            break;
        case '13':
            msg="outbound communication setup failed";
            break;
        case '14':
            msg="inbound communication setup failed";
            break;
        case '15':
            msg="message handling initialization failed";
            break;
        case '16':
            msg="persistency setup failed";
            break;
        case '17':
            msg="port range limitation failed";
            break;
        case '20':
            msg="connection to cell failed";
            break;
        case '25':
            msg="memory fault";
            break;
        case '26':
            msg="command failed";
            break;
        case '27':
            msg="syntax error";
            break;
        case '28':
            msg="invalid answer received";
            break;
		case '32':
            msg="event handle non esistente";
            break;
        case '99':
            msg="no answer from msetmsg";
            break;
        default:
            msg="unknow error code";
    }

    return msg;
}

var logResult = function (parRetCode)  {
    if (parRetCode == '0') {
        _logger.info('Return with errorlevel = ' + parRetCode + '. Message : OK');
    } else {
        _logger.error('Return with errorlevel = ' + parRetCode + '. Message : ' + EcmCommander.getErrorLevelMsg(parRetCode))
    }
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
		getUserFromDname: getUserFromDname
	}
})();
