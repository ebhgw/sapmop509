/*
	
*/

(function () {
var logger = Packages.org.apache.log4j.Logger.getLogger("fs.walkset");
logger.info("Walk Set starting");

// remove - where nedeede
function setCorrectServiceName (name) {
	return name.replace(' - ', '');
}

function walkset (startEle) {

	var settingVisitor =
	{
	    count: 0,
		idx: 0,
		eleDn: '',
		tgtDn: '',
		tgtEle: null,
		found: false,
	    visit: function ( ele )
	    {
			elename = ele.name;
			elenewname = setCorrectServiceName(elename);
			ele.servizioNomeNoc = elenewname;
			
			try {
				tgtEle = formula.Root.findElement(tgtDn)
				found = true
			} catch (Exception) {
				// really nothing to do
			}
			
			if (!found) {
				logger.info('Not found, deleting ' + ele.DName)
				ele.perform( session, 'LifeCycle|Delete', [], [] )
			    deletingVisitor.count++
			} else {
				logger.debug('Found')
			}
	    }
	}

	var structEle = formula.Root.findElement(structRoot);
	structEle.walk ( deletingVisitor );
	logger.info("deleteStructEle.... ends, deleted :" + deletingVisitor.count );
}

function addBaselineEleToStruct (structRoot, baseRoot) {

	logger.info('addBaselineEle.... start ' + baseRoot + ' :: ' + structRoot)
	// start from baseline dname, map to structure dname, if does not exists copy there (using the parent)
	var addingEleVisitor =
	{
	    count: 0,
		idx: 0,
		eleDn: '',
		tgtDn: '',
		tgtEle: null,
		parentDn: null,
		tgtParentDn: null,
		tgtParent: null,
		found: false,
	    visit: function ( ele )
	    {
			found = false
			eleDn = ele.DName
			tgtDn = mapDn(eleDn, baseRoot, structRoot)
			//idx = eleDn.lastIndexOf(baseRoot)
			logger.debug('Base: Visiting ' + eleDn)
			//tgtDn = eleDn.substr(0,idx) + structRoot;
			logger.debug('Struct: Checking ' + tgtDn)
			try {
				tgtEle = formula.Root.findElement(tgtDn)
				found = true
			} catch (Exception) {
				// really nothing to do
			}
			
			if (!found) {
				logger.info('Not found, adding ' + tgtDn)
				parentDn = ele.parent.DName
				tgtParentDn = mapDn(parentDn, baseRoot, structRoot)
				//idx = parentDn.lastIndexOf(baseRoot)
				//var tgtParentDn = parentDn.substr(0,idx) + structRoot;
				logger.debug('Finding parent ' + tgtParentDn)
				var tgtParent = formula.Root.findElement(tgtParentDn);
				logger.debug('Parent found')
				tgtParent.copy(session.getReference(), ele.id(), formula.relations.NAM);
			    addingEleVisitor.count++
			} else {
				logger.debug('Found')
			}
	    }
	}

	var baseEle = formula.Root.findElement(baseRoot);
	baseEle.walk ( addingEleVisitor );
	logger.info("addBaselineEleToStruct ends, added :" + addingEleVisitor.count );
}

/****************** Kind of main ******************/

if (!server.isCoordinator())
{

	logger.debug('BSCM build skipped as server is not cluster coordinator');
	
} else {

	// export model
	logger.info('Exporting model');

	// Should end with '\\'
	// original var cmdDir = java.lang.System.getProperty("formula.home") + '..\\tlnd\\jobs\\'
	var cmdDir = java.lang.System.getProperty("formula.home") + '\\..\\..\\tlnd\\jobs\\'
	
	logger.info('Writing csv files');
	state.ModelExporter.exportAcronimi();
	state.ModelExporter.exportServizi();
	state.ModelExporter.mapNomiServiziExporter();
	
	logger.info('Loading csv to database');
	var cmdToExe, stream, output;
		
	cmdToExe = cmdDir + 'ExpMapNomiServizi\\ExpMapNomiServizi_run.bat';
	state.Cmd.execute(cmdToExe, logger)
	
	cmdToExe = cmdDir + 'ExpAcronimi\\ExpAcronimi_run.bat';
	state.Cmd.execute(cmdToExe,logger)

	cmdToExe = cmdDir + 'ExpServizi\\ExpServizi_run.bat';
	state.Cmd.execute(cmdToExe, logger)
	
	logger.info('Importing ECM data');
	// The table Servizi should always be populated
	cmdToExe = cmdDir + 'ImpServizi\\ImpServizi_run.bat';
	state.Cmd.execute(cmdToExe, logger)
	
	// Should check a way to cope with upd vs baseline
	//cmdToExe = cmdDir + 'ImpAcronimi\\ImpAcronimi_run.bat';
	//state.Cmd.execute(cmdToExe,logger)

	cmdToExe = cmdDir + 'ImpBaseline\\ImpBaseline_run.bat';
	state.Cmd.execute(cmdToExe, logger)
	
	// Start and stop the adapter to force loading to Baseline (out of adapter schedule)
	state.AdaptersUtil.restartImpModel()
	
	// Process baseline
	logger.info('Build bscm: baseline Acronimi start');
	state.ViewBuilder.build(baseAcronimi);
	logger.info('Build bscm: baseline Servizi start');
	state.ViewBuilder.build(baseServizi);
	
	logger.info('Align Gen Models/Baseline to Prod');
	deleteStructEleNotInBaseline(structAcronimi, baseAcronimi)
	deleteStructEleNotInBaseline(structServizi, baseServizi)

	addBaselineEleToStruct(structAcronimi, baseAcronimi)
	addBaselineEleToStruct(structServizi, baseServizi)

	// as Acronimi should run evey 30 seconds, this could be useless
	logger.info('Build bscm: structure Acronimi start');
	state.ViewBuilder.build(structAcronimi);
	logger.info('Build bscm: structure Servizi start');
	state.ViewBuilder.build(structServizi);
	
	logger.info('AlignModel ends')
	formula.log.info('AlignModel ends')

}
})();