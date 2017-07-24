/*
1. get structure root obj path  (no class rewriting up that path, it should be replaced with destination) and element root obj (under which we perform the copy)
	if structure obj is not under structure root element skip
    init an empty path
2. loop up the chain (starting from current element) via parent prop until the parent obj dname is equal to that of root element
	   get the class of the parent
	   rewrite the class
	   add class = name to the partial path (end of string) with '/'
3. add source as children

Beware of formula.util.encodeURL
*/

// Initialize
if (!state.isp)
	state.isp = new Object();
if (!state.isp.bscm)
	state.isp.bscm = new Object ();

state.isp.bscm.structureRootView1 = 'gen_folder=structure/gen_folder=testModStructure/root=Generational+Models/root=Services';
state.isp.bscm.targetRootView1 = 'gen_folder=moddedStructure/gen_folder=testModStructure/root=Generational+Models/root=Services';

function copyElementToNewClazz (myEle) {

var structureRoot = state.isp.bscm.structureRootView1;
var targetRoot = state.isp.bscm.targetRootView1;

var myEleDname = myEle.dname;
var idx = myEleDname.indexOf(structureRoot);
if (idx == 0) {
    formula.log.info('Skipping');
	return;
}

formula.log.info('Element is ' + myEleDname);
formula.log.info('Element index ' + myEleDname.indexOf(structureRoot));

var newDname = '';
var currentElement = myEle;
var eleClazz;
var eleName;
var stopper = 100;
var counter = 0;

while (currentElement.dname != structureRoot && counter < stopper) {
	eleClazz = currentElement.elementClassname + "_view1";
	eleName = currentElement.name;
	
	newDname = newDname + eleClazz + '=' + eleName + '/';
}

newDname = newDname + targetRoot;

var scriptElement = server.getElement( newElementDname );
var scriptChildren = new Array ();
scriptChildren.push(myEleDname);
scriptElement.Children = scriptChildren;
formula.log.info('Created element ' + newDname);

}

var myRoot = formula.Root.findElement(state.isp.bscm.structureRootView1);

formula.log.info('Walking from ' + myRoot);
myRoot.walk(copyElementToNewClazz);
formula.log.info('Walked from ' + myRoot);

