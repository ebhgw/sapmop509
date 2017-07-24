/*
1. get structure root obj path  (no class rewriting up that path, it should be replaced with destination) and element root obj (under which we perform the copy)
	if structure obj is not under structure root element skip
    init an empty path
2. loop up the chain (starting from current element) via parent prop until the parent obj dname is equal to that of root element
	   get the class of the parent
	   rewrite the class
	   add class = name to the partial path (end of string) with '/'
	   
Beware of formula.util.encodeURL

*/

// should be stored as a formula encoded URL
if (!state.isp)
	state.isp = new Object();
	
if (!state.isp.bscm)
	state.isp.bscm = new Object ();

state.isp.bscm.structureRootView1 = 'gen_folder=structure/gen_folder=testModStructure/root=Generational+Models/root=Services';
state.isp.bscm.targetRootView1 = 'gen_folder=moddedStructure/gen_folder=testModStructure/root=Generational+Models/root=Services';

var structureRoot = state.isp.bscm.structureRootView1;

function createElement(parentDn, eleName, eleClazz)
{
   if (!parentDn) {
      return "parent must be specified"
   }
   if (!eleName) {
      return "name must be specified"
   }
   if (!eleClazz) {
      return "clazz must be defined"
   }

   // find the parent
   var parent
   try {
      parent = formula.Root.findElement(parentDn)
   } catch (Exception) {
      return "Unable to locate the specified parent element: (" + Exception + ")"
   }

   // Find the new class to add.
   var clazz
   try {
      clazz = parent.elementClass.findChild( eleClazz )
   } catch (Exception) {
      return "Unable to find class child: (" + Exception + ")"
   }

   // add the new organization
   try {
        clazz.newElement( session, parent, eleName, [], [] )
   } catch (Exception) {
      return "Unable to create new element:" + Exception
   }

   // signal success
   return ""
}

var struct;
try {
      struct = formula.Root.findElement(structureRoot)
   } catch (Exception) {
      formula.log.info( "Unable to locate the specified parent element: (" + Exception + ")" );
}

formula.log.info('Create element ' + createElement(struct, 'test1', 'clazz1'));


try {
      struct = formula.Root.findElement('test1' + '=' + 'clazz1' + '/' + structureRoot)
   } catch (Exception) {
      formula.log.info( "Unable to locate the first level element: (" + Exception + ")" );
}

formula.log.info('Create element ' +  createElement(struct, 'test2', 'clazz2'));
   
try {
      struct = formula.Root.findElement('test2' + '=' + 'clazz2' + '/' + 'test1' + '=' + 'clazz1' + '/' + structureRoot)
   } catch (Exception) {
      formula.log.info( "Unable to locate the specified parent element: (" + Exception + ")" );
}
   
