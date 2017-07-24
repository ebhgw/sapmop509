/*
  Script StructMngr.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  Taken a template structure, adds or remove elements from a target structure
  to match template structure.
  Structure should be equivalent (compare on element name and class name) but the root of the dname
  
  Note:
  - won't work against the adapter as class name changes
  
  To find matching elements uses dnames:
  - change the root of the dname, removes the Service Model path and adds the Gen Models path

*/

var logger = Packages.org.apache.log4j.Logger.getLogger("Dev.StructManager");

function StructMngr () {
}

StructMngr.mapDn = function(dn, from, to) {
	var idx = dn.lastIndexOf(from)
	logger.debug('Mapping: ' + dn)
	var resDn = dn.substr(0,idx) + to;
	logger.debug('Mapped: ' + resDn)
	// map class
	return resDn
}

// Add Livello to ClassName
StructMngr.changeClass4Adapter = function(dn) {
	var svcModelsClassPatt = /^(ServizioGlobalView|Servizio|SSA|Acronimo|ClasseCompInfra|CompInfra|Risorsa)/
	var resMatch = svcModelsClassPatt.exec(dn);
	var resDn = '';
	if (resMatch != null)
		resDn = 'Livello'+dn;
	else
		resDn = dn;
	return resDn;
}

// Remove Livello from ClassName
StructMngr.changeClass4ServiceModels = function(dn) {
	var adaClassPatt = /^(LivelloServizioGlobalView|LivelloServizio|LivelloSSA|LivelloAcronimo|LivelloClasseCompInfra|LivelloCompInfra|LivelloRisorsa)/
	var resMatch = adaClassPatt.exec(dn);
	var resDn = '';
	if (resMatch != null)
		resDn = dn.substr(7);
	else
		resDn = dn;
	return resDn;
}



StructMngr.deleteEleNotInTemplate = function(templateRoot, targetRoot) {

	logger.info('deleteEleNotInTemplate start ' + targetRoot + ' :: ' + templateRoot)
	var deletingVisitor =
	{
	    count: 0,
		idx: 0,
		eleDn: '',
		tgtDn: '',
		tgtEle: null,
		found: false,
	    visit: function ( ele )
	    {
			found = false
			eleDn = ele.DName
			tgtDn = StructMngr.mapDn(eleDn, targetRoot, templateRoot)
			tgtDn = StructMngr.changeClass4Adapter(tgtDn);
			//idx = eleDn.lastIndexOf(targetRoot)
			logger.debug('Target: Visiting ' + eleDn)
			//tgtDn = eleDn.substr(0,idx) + templateRoot;
			logger.debug('Template: Checking ' + tgtDn)
			try {
				tgtEle = formula.Root.findElement(tgtDn)
				found = true
			} catch (Exception) {
				// really nothing to do
			}
			
			if (!found) {
				logger.info('Not found, deleting ' + ele.DName)
				//ele.perform( session, 'LifeCycle|Delete', [], [] )
			    deletingVisitor.count++
			} else {
				logger.debug('Found')
			}
	    }
	}

	var structEle = formula.Root.findElement(targetRoot);
	structEle.walk ( deletingVisitor );
	logger.info("deleteEleNotInTemplate ends, deleted :" + deletingVisitor.count );
}

StructMngr.addTemplateEle = function(templateRoot, targetRoot) {

	logger.info('addTemplateEle.... start ' + templateRoot + ' :: ' + targetRoot)
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
			tgtDn = StructMngr.mapDn(eleDn, templateRoot, targetRoot)
			//idx = eleDn.lastIndexOf(templateRoot)
			logger.debug('Base: Visiting ' + eleDn)
			//tgtDn = eleDn.substr(0,idx) + targetRoot;
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
				tgtParentDn = StructMngr.mapDn(parentDn, templateRoot, targetRoot)
				StructMngr.changeClass4ServiceModels(tgtParentDn);
				//idx = parentDn.lastIndexOf(templateRoot)
				//var tgtParentDn = parentDn.substr(0,idx) + targetRoot;
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

	var baseEle = formula.Root.findElement(templateRoot);
	baseEle.walk ( addingEleVisitor );
	logger.info("addTemplateEle ends, added :" + addingEleVisitor.count );
}

StructMngr.alignToTemplate = function (tmplRoot, tgtRoot) {
	logger.debug('Align tree ' + tgtRoot + ' to template ' + tmplRoot);
	//StructMngr.addTemplateEle (tmplRoot, tgtRoot)
	StructMngr.deleteEleNotInTemplate (tmplRoot, tgtRoot)
}