var EcmOpUtil = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.ecmop.ecmoputil');

    var _list_mc_ueid = function (lal) {
        var lmu = new Array();
        for (var i = 0; i < lal.length; i++) {
            lmu.push(lal[i].mc_ueid);
        }
        return lmu.join(',');
    };

    var _createActionListFromAlarms = function (lal) {
        var eal = new Packages.com.hog.noc.ecmop.EcmActionList();
        var al;
        for (var i = 0; i < lal.length; i++) {
            al = lal[i];
            // createEcmOp (String cmd, String usr, String mcid, String mcown, String stus)
            eo = Packages.com.hog.noc.ecmop.EcmOpFactory.createEcmAction(al.IDReceivedAlarm, al.mc_ueid, al.mc_owner, al.status);
            eal.add(eo);
        }
        _logger.debug('EcmOpUtil._createActionListFromAlarms, created ' + eal.size() + ' element list');
        return eal;
    };

    // allow to call without optional pars, set them to null
    var _createEnqEcmOpFromAlarms = function (cmdname, usr, lal, tkt, nt) {
        var eal = _createActionListFromAlarms(lal);
        // safety check, should be null anyway
        if (typeof tkt == undefined)
            tkt = null;
        if (typeof nt == undefined)
            nt = null;
        var eo = Packages.com.hog.noc.ecmop.EcmOpFactory.createEnqEcmOp(cmdname, usr, tkt, nt, eal);
        return eo;
    };

    var _createEcmOpFromAlarms = function (cmdname, usr, al) {
        var eal = _createActionListFromAlarms(lal);
        var eo = Packages.com.hog.noc.ecmop.EcmOpFactory.createEcmOp(cmdname, usr, eal);
        return eo;
    };

    // opt
    var _doEnq = function (cmdname, usr, lal, opt) {
        var tkt, nt;
        // cmdname may be anything, it would be converted to an ENUM.
        // ENUM by convention are uppercase so convert cmdname to upperacase
        cmdname = cmdname.toUpperCase();
        if (cmdname + '' == 'ADDTICKET') {
            tkt = opt;
            nt = null;
            _logger.info('EcmOpUtil.doEnq invoking ' + cmdname + ' by user ' + usr + ' with ticket ' + tkt + ' on alarms ' + _list_mc_ueid(lal));
        } else if (cmdname + '' == 'ADDNOTE') {
            nt = opt;
            tkt = null;
            _logger.info('EcmOpUtil.doEnq invoking ' + cmdname + ' by user ' + usr + ' with note ' + nt.substring(0, 8) + '... on alarms ' + _list_mc_ueid(lal));
        } else {
            tkt = null;
            nt = null;
            _logger.info('EcmOpUtil.doEnq invoking ' + cmdname + ' by user ' + usr + ' on alarms ' + _list_mc_ueid(lal));
        }
        var success = false, message = '', eo = null, res;
        try {
            // get a list of EcmOp with data for request fields filled from alarm data
            // createEnq... process also the command name
            eo = _createEnqEcmOpFromAlarms(cmdname, usr, lal, tkt, nt);
            _logger.debug("EcmOpUtil.doEnq request " + eo.toString());
            var enqueuer = new Packages.com.hog.noc.ecmop.enq.Enqueuer();
            _logger.debug("EcmOpUtil.doEnq created enqueuer");
            // null parameter, used only for ticket and note
            eo = enqueuer.enqueue (eo);
            _logger.debug("EcmOpUtil.doEnq response summary " + eo.enqSummary());
            // force to javascript boolean
            success = eo.getEnqueuing_success().toString().equalsIgnoreCase("true") ? true : false;
            _logger.debug("EcmOpUtil.doEnq enqueuer.enqueue success is " + success);

            if (success) {
                _logger.debug('EcmOpUtil.doEnq ' + cmdname + ' request enqueued successfully by user ' + usr + ' guid ' + eo.guid);
                message = 'ECM::'+ cmdname + ' request enqueued successfullly';
            } else {
                _logger.warn(cmdname + ' request enqueuing failed by user ' + usr );
                message = 'ECM::'+ cmdname + '  request failed. Contact administrator';
            }
        } catch (excp) {
            _logger.error("EcmOpUtils.doEnq exception " + excp);
            // excp = java class name + ": " + message, excp.getMessage = only message, excp.toString() = excp
            //_logger.error("EcmOpUtils.doEnq exception getMessage " + excp.getMessage());
            //_logger.error("EcmOpUtils.doEnq exception toString " + excp.toString());

            if (excp instanceof Packages.com.hog.noc.ecmop.EOCallException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = map2message(excp);
            } else if (excp instanceof Packages.com.hog.noc.props.PropsException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = "Error loading property file, " + excp;
            } else {
                message = excp;
            }
            success = false;

        }
        return  {
            success: success,
            message: message,
            ecmop: eo
        };
    };

    var _doQu = function (eo) {
        var eo = null, success, message;
        try {
            _logger.debug('EcmOpUtil.doQu checking ' + eo.guid);
            var oc = new Packages.com.hog.noc.ecmop.res.OpResultChecker();
            eo = oc.queryStatus(eo);
            success = true;
        } catch (excp) {
            _logger.error("EcmOpUtils.doQu exception " + excp);

            if (excp instanceof Packages.com.hog.noc.ecmop.EOCallException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = map2message(excp);
            } else if (excp instanceof Packages.com.hog.noc.props.PropsException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = "Error loading property file, " + excp;
            } else {
                message = excp;
            }
            success = false;
        }
        return  {
            success: success,
            message: message,
            ecmop: eo
        };
    };

    var _doQuByGuid = function (guid) {
        var eo = null, success, message;
        try {
            _logger.debug('EcmOpUtil.doQuByGuid checking ' + guid);
            // Get Noc Operation end poing
            var gnorendpt = "http://10.2.230.242:2007/GetNocOperationsResults.ashx";
            var oc = new Packages.com.hog.noc.ecmop.res.OpResultChecker();
            eo = oc.queryOpByGuid(guid);
        } catch (excp) {
            _logger.error('EcmOpUtil.doQuByGuid got error ' + excp);

            if (excp instanceof Packages.com.hog.noc.ecmop.EOCallException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = map2message(excp);
            } else if (excp instanceof Packages.com.hog.noc.props.PropsException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = "Error loading property file, " + excp;
            } else {
                message = excp;
            }
            success = false;
        }
        return  {
            success: success,
            message: message,
            ecmop: eo
        };
    };

    // endpt = "http://10.2.230.242:2007/GetNocOperationsResults.ashx";
    var _quLastGuidByMcueid = function (mcid) {
        var eo = null, success, message, guid = null;
        try {
            _logger.debug('EcmOpUtil.quGuidByMcueid checking ' + mcid);
            var obml = new Packages.com.hog.noc.ecmop.res.OpByMcueidLister();
            guid = obml.queryLastOpByMcueid(mcid);
            _logger.debug('EcmOpUtil.quGuidByMcueid guid: ' + guid);

            if (guid == null) {
                success = false;
            } else {
                success = true;
                message = 'Guid is ' + guid;
            }
        } catch (excp) {
            _logger.error('EcmOpUtil.quGuidByMcueid got error ' + excp);
            if (excp instanceof Packages.com.hog.noc.ecmop.EOCallException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = map2message(excp);
            } else if (excp instanceof Packages.com.hog.noc.props.PropsException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = "Error loading property file, " + excp;
            } else {
                message = excp;
            }
            success = false;
        }
        return  {
            success: success,
            message: message,
            guid: guid
        };
    };

    // endpt = "http://10.2.230.242:2007/GetNocOperationsResults.ashx";
    var _doQuLastopByMcueid = function (mcid) {
        var eo = null, success, message = '', guid = null;
        try {
            _logger.debug('EcmOpUtil.doQuLastopByMcueid checking ' + mcid);
            var obml = new Packages.com.hog.noc.ecmop.res.OpByMcueidLister();
            guid = obml.queryLastOpByMcueid(mcid);
            // query successful, if found return guid ow null. In case of error raise exception
            success = true;
            if (guid != null) {
                // found an operation for the mcid
                _logger.debug('EcmOpUtil.doQuLastopByMcueid, call to OpByMcueidLister found guid: ' + guid);
                var oc = new Packages.com.hog.noc.ecmop.res.OpResultChecker();
                // extract info from guid
                eo = oc.queryOpByGuid(guid);
                //_logger.debug("queryOpByGuid returned " + eo.toString());
                //_logger.debug("Returning from queryOpByGuid");
                if (eo !== null) {
                    _logger.info("EcmOpUtil.doQuLastopByMcueid found operation");
                    // found operation, filter action
                    //_logger.debug("Before");
                    //_logger.debug(eo.toString())
                    //_logger.debug("----------------------------");
                    eo = Packages.com.hog.noc.ecmop.EcmOpHndlr.filterEcmAction(mcid, eo);
                    //_logger.debug("After");
                    //_logger.debug(eo.toString());
                    //_logger.debug("----------------------------");
                    message = 'Found operation'
                } else {
                    _logger.info("EcmOpUtil.doQuLastopByMcueid operation not found");
                    message = 'Not found operation'
                }
            } else {
                // guid is null, trace in debug
                _logger.debug('EcmOpUtil.doQuLastopByMcueid, no guid found');
            }

        } catch (excp) {
            _logger.error('EcmOpUtil.doQuLastopByMcueid got error ' + excp);
            if (excp instanceof Packages.com.hog.noc.ecmop.EOCallException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = map2message(excp);
            } else if (excp instanceof Packages.com.hog.noc.props.PropsException) {
                // cannot access wrapped exception and therefore cannot getCode to find error code
                // addedd to message, extract via regexp in map2message
                message = "Error loading property file, " + excp;
            } else {
                message = excp;
            }
            success = false;
        }

        _logger.debug('EcmOpUtil._doQuLastopByMcueid returning with success = ' + success);
        _logger.debug("EcmOpUtil._doQuLastopByMcueid returning eo " + eo);
        var test1 = eo == null;
        _logger.debug("eo test == " + test1);
        var test2 = eo === null;
        _logger.debug("eo test === " + test2);
        // return success true if query were ok, eo null if found nothing
        return  {
            success: success,
            message: message,
            guid: guid,
            ecmop: eo
        };
    };

    var map2message = function (excp) {
        var extrcodere = /Errcode:(EO-(\d+))\s/;
        var match = extrcodere.exec(excp);
        var errcode = parseInt(match[2]);
        var msg = '';
        if (match === null) {
            msg = 'Error code sconosciuto. ' + ' >> ' + excp;
        } else {
            switch (errcode) {
                case 1:
                    msg = match[1] + ". Errore nel caricamento del file dell proprieta'. Contattare l'amministatore";
                    break;
                case 101:
                    msg = match[1] + '. Chiamata al web service fallita';
                    break;
                default:
                    msg = match[1] + '. Errore sconosciuto';
            }
        }
        return msg;
    }

    var showDaoProps = function () {
        var props = Packages.com.hog.noc.props.NOCPropLoader.reloadCustomProperties('EcmOperation');
        var msg = '';
        msg = 'Connecting to ' + props.getProperty("noc.datiecm.dbhost") + ':, port: ' + props.getProperty("noc.datiecm.dbport");
        msg = msg + ', db ' + props.getProperty("noc.datiecm.db");
        msg = msg + ' using user ' + props.getProperty("noc.datiecm.dbuser");
    }

    var reloadProperties = function () {
        var props = Packages.com.hog.noc.props.NOCPropLoader.reloadCustomProperties('EcmOperation');
    }

    return {
        doEnq:_doEnq,
        doQu:_doQu,
        doQuByGuid:_doQuByGuid,
        doQuLastopByMcueid:_doQuLastopByMcueid,
        quLastGuidByMcueid:_quLastGuidByMcueid
    };

})();