// Repair Matches with length 0 or length greater than 250

var logger = Packages.org.apache.log4j.Logger.getLogger("Dev");

// namespace
function Matches () {};

// quich workaround to
var counter = 0;

// to avoid touching matches that are already working, check if match is zero length (an error)
// or greater than 250 (surely an error) and set them
Matches.repairMatchesZeroTwo = function (ele) {
    var clazz = ele.elementClassName + "";
    if (clazz == 'Risorsa') {
        var matches = ele.Matches;
        var newMatches = '';
        var dname = ele.dname;
        var len = matches.length;
        if (len > 250 || len == 0) {
            counter++;
            newMatches = 'LivelloServerRisorsa='+ele.match2AlarmExt+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
            ele.Matches = newMatches;
        }
    }
}

Matches.doRepairMatchesZeroTwo = function (ele) {

    logger.info('Repair Matches with length 0 or length greater than 250');

    repair_count = 0;
    ele.walk(Matches.repairMatchesZeroTwo);

    var msg = 'Repaired ' + counter + ' Matches with length 0 or length greater than 250';
    logger.info(msg);
    session.sendMessage(msg);

}


Matches.writeMatchesToLog = function(ele) {
    var clazz = ele.elementClassName + "";
    if (clazz == 'Risorsa') {
        counter++;
        var matches = ele.Matches;
        var dname = ele.dname;
        logger.info('Found ' + dname + '\nMatches : ' + matches +'\n');
    }
}

Matches.writeAnomalousMatchesToLog = function(ele) {
    var clazz = ele.elementClassName + "";
    if (clazz == 'Risorsa') {
        counter++;
        var matches = ele.Matches;
        var dname = ele.dname;
        var len = matches.length;
        logger.info('Length is ' + len);
        if (len > 200 || len == 0) {
            logger.info('Found ' + dname + '\nMatches : ' + matches);
        }
    }
}

Matches.doWriteToLog = function (ele) {

    logger.info('Dump of Matches property for class Risorsa');

    counter = 0;
    ele.walk(Matches.writeAnomalousMatchesToLog);

    var msg = 'Dump Matches for Risorsa Class ended';
    session.sendMessage(msg);

}

Matches.checkMatchesProp = function(ele) {
    var clazz = ele.elementClassName + "";
    var dname = ele.DName;
    if (clazz == 'Risorsa') {
        var eleCurrentMatch = ele.Matches;
        var eleCalcMatch = 'LivelloServerRisorsa='+ele.match2AlarmExt+'.*/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';

        if (eleCurrentMatch.length != eleCalcMatch.length || eleCurrentMatch != eleCalcMatch) {
            counter++;
            logger.info('Found matches suspect for ' + dname);
            logger.info('Current Match    :' + eleCurrentMatch);
            logger.info('Calculated Match :' + eleCalcMatch);
        }
    }
}

Matches.doCheckMatches = function (ele) {

    logger.info('Checking Matches on class Risorsa of subtree');

    counter = 0;
    ele.walk(Matches.checkMatchesProp);

    var msg = 'Found ' + counter + ' Matches not equal for Risorsa Class ended';
    session.sendMessage(msg);

}