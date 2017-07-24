/*

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Find the difference on alarms between Servizi and Acronimi

 */

// on ISP_FolderBase=SystemTest/root=Organizations

(function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.diffonalarms");
    load('custom/lib/underscore.js');

    // element is SystemTest, find Servizi, find Acronimi, collect alarmID and get difference

    var differenceOnAlarms = function (acroalarmset, svzalarmset) {
        // suppose acroalarmset > svzalarmset as acronimi feed servizi
        var acroidset = _.map(acroalarmset, function (al) {
            return al.ID +''
        });
		
		//_logger.info('Acro set ' + _.reduce(acroidset, function (memo, id) { return memo==''?id:memo + ' ,' + id}, ''));
        var svzidset = _.map(svzalarmset, function (al) {
            return al.ID +''
        });
		
		//_logger.info('Svz set ' + _.reduce(svzidset, function (memo, id) { return memo==''?id:memo + ' ,' + id}, ''));
        var diffset = _.difference(acroidset, svzidset);
		//_logger.info('Diff set ' + _.reduce(diffset, function (memo, id) { return memo==''?id:memo + ' ,' + id }, ''));
		_logger.debug('Acro set size is ' + acroidset.length + ', Svz set size is ' + svzidset.length + ', diff set is ' + diffset.size);
        return diffset;

    }

    var logStructure = function (idset, alarmset) {
		_logger.info('Checking structure for ' + idset.length + ' elements');
        _.each(idset, function (id) {
            _.each(alarmset, function (al) {
                if (id == al.ID+'') {
                    _logger.info('Alarm ID ' + id + ' Structure: ' + al.Ambiente + ', ' + al.Servizio + ', ' + al.Acronimo
                        + ', ' + al.ClasseComponenteInfra + ', ' + al.NomeComponenteInfra + ', ' + al.NomeElementoTecno);
                }
            });
        });
    }

    var checkAlarms = function (rootElement) {
            var acro = formula.Root.findElement('gen_folder=Acronimi/' + rootElement.DName);
            var svzio = formula.Root.findElement('gen_folder=Servizi/' + rootElement.DName);
            var acroAlarms = acro.alarms;
            var svzAlarms = svzio.alarms;

            var diffset = differenceOnAlarms(acroAlarms, svzAlarms);
			_logger.info("Diff set size is " + diffset.length);
			if (diffset.lenght != 0) 
				logStructure(diffset, acroAlarms);
    }

    // main
	_logger.info('Collect diffs under Acronimi/Servizi under ' + element.DName);
    checkAlarms(element);
	_logger.info('Collect completed');
	
	session.sendMessage('Operation completed');

})();