/*

  Script CopyChildren.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  Copy elements from a source to a destinationi

*/

function OrgCopier (from, to, oldClass, newClass){
}

OrgCopier.cpChildren = function (from, to, oldClass, newClass) {

}

state.isp.bscm.structureRoot = 'gen_folder=structure/gen_folder=testModStructure/root=Generational+Models/root=Services';
state.isp.bscm.targetRootView1 = 'gen_folder=moddedStructure/gen_folder=testModStructure/root=Generational+Models/root=Services';

var structureRoot = state.isp.bscm.structureRoot;
var targetRoot = state.isp.bscm.targetRootView1;

function copyElementToNewClazz (myEle) {
    var myEleDname = myEle.dname;

    var idx = myEleDname.indexOf(structureRoot);
    formula.log.info('Element is ' + myEleDname);
    formula.log.info('Element index ' + myEleDname.indexOf(structureRoot));

    if (idx == 0) {
        formula.log.info('Skipping');
        // nothing to do on root node
        return;
    }

    var newDname = '';
    var currentElement = myEle;
    var currentElementDname = currentElement.dname;
    var eleClazz;
    var eleName;
    var stopper = 10;
    var counter = 0;

//starting from bottom, build up the path
    var idx = currentElement.dname.indexOf(structureRoot);
    formula.log.info('Loop ' + counter);
    formula.log.info('Idx ' + idx)
    while (currentElement.dname != structureRoot && counter < stopper) {
    	formula.log.info('Building path iter : ' + counter);
    	formula.log.info('Current ' + currentElement.dname);

    	eleClazz = currentElement.elementClassname + "_view1";
    	eleName = currentElement.name;
    	newDname = newDname + formula.util.encodeURL(eleClazz) + '=' + formula.util.encodeURL(eleName) + '/';
    	formula.log.info('Partial ' + newDname);
    	currentElement = currentElement.parent;
    	counter++;
    }

newDname = newDname + targetRoot;
formula.log.info('Creating ' + newDname);
var scriptElement = server.getElement( newDname );
//var scriptChildren = new Array ();
//scriptChildren.push(myEleDname);
//scriptElement.Children = scriptChildren;
formula.log.info('Created element ' + newDname);

}

var myRoot = formula.Root.findElement(state.isp.bscm.structureRoot);

formula.log.info('Walking from ' + myRoot);
myRoot.walk(copyElementToNewClazz);
formula.log.info('Walked from ' + myRoot);

