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
      return "Unable to find class child: " + Exception
   }

   // add the new organization
   try {
	  clazz.newElement( session, parent, orgInfo.name, names, values )
   } catch (Exception) {
      return "Unable to create new element:" + Exception
   }

   // signal success
   return ""
}