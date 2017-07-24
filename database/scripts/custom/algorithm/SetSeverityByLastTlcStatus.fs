// @debug on
// file custom/algorithm
// should collect this and attach to a local variable

(function () {
    var scriptName='SetSeverityByLastTlcStatus.fs';
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.algorithm");
    _logger.debug('--------------------------------------------------------');
    _logger.debug('Firing '+ scriptName +' ...');

    var thisElement = this.conditionState.subject;
    var lastTlcStatus = thisElement["lastTlcStatus"];
    _logger.debug('LastAlarm TlcStatus: '+ lastTlcStatus);

    if (lastTlcStatus.toLowerCase() == "standby"){
      this.conditionState.setResult( formula.conditions.UNKNOWN );
      _logger.debug('Set Condition to UNKNOWN for: '+thisElement);
    } else if (lastTlcStatus.toLowerCase() == "active"){
      this.conditionState.setResult( formula.conditions.OK );
      _logger.debug('Set Condition to OK for: '+ thisElement);
    } else {
        _logger.error('Error on script:'+scriptName+' - Unknown lastTlcStatus:'+ lastTlcStatus + ' cannot set condition');
    }
    this.conditionState.setState( this.conditionState.FINISHED);
})(this)
