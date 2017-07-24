// Repair Matches with length 0 or length greater than 250



// namespace
var MatchMngr = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");

    var touch = function (eleDn) {
        try {
            var ele = formula.Root.findElement(eleDn);
            var script = ele.getMatchExpression();
            ele.setElementMatchExpression(script);
        } catch (excp) {
            _logger.warn( "Unable to perform .elementClass.findChild: " + ( undefined != excp.key ? excp.key :
                excp.getMessage() ), excp )
        }
    }

    var set = function (eleDn, match) {
        try {
            var ele = formula.Root.findElement(eleDn);
            ele.setElementMatchExpression(match);
        } catch (excp) {
            _logger.warn( "Unable to perform .set (MatchExpression): " + ( undefined != excp.key ? excp.key :
                excp.getMessage() ), excp )
        }
    }

    var get = function (eleDn) {
        var res = null;
        try {
            var ele = formula.Root.findElement(eleDn);
            res = ele.getMatchExpression();
        } catch (excp) {
            _logger.warn( "Unable to get (Match Expression): " + ( undefined != excp.key ? excp.key :
                excp.getMessage() ), excp );
        }
        return res;
    }

    var patternOnEleAsJson = function (ele, pmatch, pscript) {
        var res = '{\n'
        res = res + '"dname": "' + ele.DName + '",\n';
        res = res + '"match": "' + pmatch + '"\n,';
        res = res + '"script": "' + pscript + '"\n';
        res = res + '}'
        //_logger.info('json is ' + res);
        return res;
    }


    var dumpPatternAsJson = function (ele) {
        load('custom/util/FileWriter.fs');
        load('custom/lib/moment.js');

        var now = moment(new Date());
        var ff = new FileWriter();
        var formulahome = Packages.java.lang.System.getProperty("formula.home")
        ff.openFile(formulahome + '/export/' + 'collected_patterns_'+ now.format("YYYYMMDD_HHmmss") + '.json');
        ff.println('{');
        ff.println('"root": "' + element.DName + '",');
        ff.println('"date": "' + now.format("YYYY-MM-DD_HH:mm:ss") + '",');
        ff.println('"pattern": [');

        var visitor =
        {
            count: 0,
            add: false,
            visit: function ( child )
            {
                _logger.info('Exploring ' + child.DName);
                var lmatch = child.getMatchExpression();
                var lscript = child.getScript();
                if ((lmatch && lmatch != '') || (lscript && lscript != '')) {
                    if (!visitor.add) {
                        visitor.add = true;
                    } else {
                        ff.println(',');
                    }
                    ff.println(patternOnEleAsJson(child,lmatch,lscript) + '');
                    visitor.count++;
                }
            }
        }

        ele.walk ( visitor );
        ff.println(']');
        ff.println('}');
        ff.closeFile();
    }

    var checkMatchesForRisorsa = function(ele) {
        var clazz = ele.elementClassName + "";
        var dname = ele.DName;
        if (clazz == 'Risorsa') {
            var eleCurrentMatch = ele.Matches;
            var eleCalcMatch = 'LivelloServerRisorsa='+ele.match2AlarmExt+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';

            if (eleCurrentMatch.length != eleCalcMatch.length || eleCurrentMatch != eleCalcMatch) {
                counter++;
                _logger.info('Found matches suspect for ' + dname);
                _logger.info('Current Match    :' + eleCurrentMatch);
                _logger.info('Calculated Match :' + eleCalcMatch);
            }
        }
    }

    return {
        touch:touch,
        set:set,
        get:get,
        dumpPatternAsJson:dumpPatternAsJson
    }

})();