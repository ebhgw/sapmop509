/*

Use as an operation with

[Test|Copy 2 Mod Class]
command=
context=element
description=Test|Copy 2 Mod Class
operation=load('custom/orgs/cpElement2ModClass.fs');
permission=view
target=dname:gen_folder=structure/gen_folder=testModStructure/root=Generational+Models/root=Services
type=serverscript

1. get structure root obj path  (no class rewriting up that path, it should be replaced with destination) and element root obj (under which we perform the copy)
	if structure obj is not under structure root element skip
    init an empty path
2. loop up the chain (starting from current element) via parent prop until the parent obj dname is equal to that of root element
	   get the class of the parent
	   rewrite the class
	   add class = name to the partial path (end of string) with '/'
	   
Beware of formula.util.encodeURL

*/

formula.log.info('Modding from ' + element.dname);

// should be stored as a formula encoded URL
if (!state.isp)
	state.isp = new Object();
	
if (!state.isp.bscm)
	state.isp.bscm = new Object ();

state.isp.bscm.structureRootView1 = 'gen_folder=structure/gen_folder=testModStructure/root=Generational+Models/root=Services';
state.isp.bscm.targetRootView1 = 'gen_folder=moddedStructure/gen_folder=testModStructure/root=Generational+Models/root=Services';

var structureRoot = state.isp.bscm.structureRootView1;
var targetRoot = state.isp.bscm.targetRootView1;

formula.log.info('Build root is ' + structureRoot);
formula.log.info('Element dname is ' + element.dname);

if (!element.dname.indexOf(structureRoot)) {
	formula.log.info('Element is not under build root');
	exit;
}

var newDname = '';
var currentElement = element;
var eleClazz;
var eleName;
var stopper = 10;
var counter = 0;

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
formula.log.info('Creating element ' + newDname);
//var newDnameEncoded = formula.util.encodeURL(newDname);
//formula.log.info('Creating element ' + newDnameEncoded);
var scriptElement = server.getElement(newDname);
//bscm.addGeneratedElement( scriptElement );
formula.log.info('Created element ' + newDname);

