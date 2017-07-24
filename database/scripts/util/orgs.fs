//
// Copyright (c) 2014 NetIQ Corporation.  All Rights Reserved.
//
// THIS WORK IS SUBJECT TO U.S. AND INTERNATIONAL COPYRIGHT LAWS AND TREATIES.  IT MAY NOT BE USED, COPIED, 
// DISTRIBUTED, DISCLOSED, ADAPTED, PERFORMED, DISPLAYED, COLLECTED, COMPILED, OR LINKED WITHOUT NETIQ'S
// PRIOR WRITTEN CONSENT. USE OR EXPLOITATION OF THIS WORK WITHOUT AUTHORIZATION COULD SUBJECT THE 
// PERPETRATOR TO CRIMINAL AND CIVIL LIABILITY.
//
// NETIQ PROVIDES THE WORK "AS IS," WITHOUT ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING WITHOUT THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT. NETIQ,
// THE AUTHORS OF THE WORK, AND THE OWNERS OF COPYRIGHT IN THE WORK ARE NOT LIABLE FOR ANY CLAIM,
// DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF,
// OR IN CONNECTION WITH THE WORK OR THE USE OR OTHER DEALINGS IN THE WORK.
//

//////////////////////////////////////////////////////////////////////////////////////////
// Dynamic Organization Manipulation
//
// createElement creates a new organization element within a Managed Objects server.
// Pass an orgInfo object to it, containing information about the
// new organization to create.   The following properties should be
// set on the object:
//
// name : The name of the new organization.
// parent : DName of the parent org.
// children : new Array() consisting of the DNames of all elements "in" the org
// clazz : A string representing the class of the new org
// contact:
// company:
// address:
// phone:
// fax:
// pager:
// email:
// url:
// displaySourceElements: Boolean, indicating whether the elements should be displayed for this org if possible.
// script:
// matches:
// rollup:
// rollupParameters:
//
// createElement returns a string.  The string is empty if the function succeeded; it
// contains an error message if success wasn't possible.
//
var undefined

function createElement(orgInfo)
{
   if (!orgInfo.parent) {
      return "parent must be specified"
   }
   if (!orgInfo.name) {
      return "name must be specified"
   }
   if (!orgInfo.clazz) {
      return "clazz must be defined"
   }

   // convert the properties into Managed Objects properties
   var names = new Array()
   var values = new Array()

   if (orgInfo.props.Children || orgInfo.props.Children == '') {
      var children = orgInfo.props.Children
      if( typeof children == 'string' )
      {
         if( children.indexOf( ',' ) > 0 )
            children = children.split( ',' )
         else if( children.indexOf( ';' ) > 0 )
            children = children.split( ';' )
      }
      names[ names.length ] = 'Children'
      values[ values.length ] = children
   } else {
      return "children must be specified"
   }

   // Setup the custom properties
   for( var p in orgInfo.props )
      if( p != 'Children' )
      {
         names[ names.length ] = p
         values[ values.length ] = orgInfo.props[p]
      }

   // find the parent
   var parent
   try {
      parent = formula.Root.findElement(orgInfo.parent)
   } catch (Exception) {
      return "Unable to locate the specified parent element: (" + Exception + ")"
   }

   // Find the new class to add.
   var orgClass
   try {
      orgClass = parent.elementClass.findChild( orgInfo.clazz )
   } catch (Exception) {
      return "Unable to find class child: " + Exception
   }

   // add the new organization
   try {


        orgClass.newElement( session, parent, orgInfo.name, names, values )
   } catch (Exception) {
      return "Unable to create new element:" + Exception
   }

   // signal success
   return ""
}

function createElement2(name, parent, children, clazz, contact, company, address, phone, fax, pager, email, url, displaySourceElements)
{
    var obj = new Object
    obj.name = name
    obj.parent = parent
    obj.clazz = clazz
    obj.props = new Object
    obj.props.Children = children
    if( arguments[4]  ) obj.props.Contact           = contact
    if( arguments[5]  ) obj.props.Company           = company
    if( arguments[6]  ) obj.props.Address           = address
    if( arguments[7]  ) obj.props.Phone             = phone
    if( arguments[8]  ) obj.props.Fax               = fax
    if( arguments[9]  ) obj.props.Pager             = pager
    if( arguments[10] ) obj.props.Email             = email
    if( arguments[11] ) obj.props.URL           = url
    if( arguments[12] ) obj.props.DisplaySourceElements = displaySourceElements ? true : false

   return createElement(obj)
}

function createElement3(name,
                  parent,
                  children,
                  clazz,
                  contact,
                  company,
                  address,
                  phone,
                  fax,
                  pager,
                  email,
                  url,
                  displaySourceElements,
                  script,
                  matches,
                  rollup,
                  rollupParameters)
{
   var obj = new Object
   obj.name = name
   obj.parent = parent
   obj.clazz = clazz
   obj.props = new Object
   obj.props.Children = children

   if( arguments[4]  ) obj.props.Contact           = contact
   if( arguments[5]  ) obj.props.Company           = company
   if( arguments[6]  ) obj.props.Address           = address
   if( arguments[7]  ) obj.props.Phone             = phone
   if( arguments[8]  ) obj.props.Fax               = fax
   if( arguments[9]  ) obj.props.Pager             = pager
   if( arguments[10] ) obj.props.Email             = email
   if( arguments[11] ) obj.props.URL           = url
   if( arguments[12] ) obj.props.DisplaySourceElements = displaySourceElements ? true : false
   if( arguments[13] ) obj.props.Script            = script
   if( arguments[14] ) obj.props.Matches           = matches
   if( arguments[15] ) obj.props.Rollup            = rollup
   if( arguments[16] ) obj.props.RollupParameters     = rollupParameters

   return createElement(obj)
}

/////////////////////////////////////////////////////////////////////////////////////
// Returns an object with the known information about an organization.  This object
// can be passed to modifyElement with modified information
//
function getElement(dname)
{
   var elementUI
   try {
      elementUI = formula.Root.findElement(dname)
   } catch (Exception) {
      return "Unable to find specified element: " + Exception
   }

   var ret = new Object
   ret.dname = dname;
   ret.name = elementUI.getName()
   ret.parent = elementUI.parent.dname
   ret.elementUI = elementUI
   ret.props = new Object
   for( var p in elementUI.properties )
   {
	if(p.indexOf('Root Cause') != -1) 
	{
		ret.props[p] = 'No root cause'
		continue
	}
	if(p.indexOf('Graphic') != -1) 
	{
			continue
	}
  	var value = elementUI[ p ]
  	if( value )
  	{
      		ret.props[p] = value
  	}
	//writeln(p + ' property getting '+value)
   }
   return ret
}

/////////////////////////////////////////////////////////////////////////////////////
// Modify an existing organization
//
// dname : Dname of the organization to modify (can't be changed)
// children : new Array() consisting of the DNames of all elements "in" the org
// contact:
// company:
// address:
// phone:
// fax:
// pager:
// email:
// url: http url 
// displaySourceElements: Boolean, indicating whether the elements should be displayed for this org if possible.
// script:
// matches:
// rollup:
// rollupParameters:
//
function modifyElement(orgInfo)
{
   // find element
   var currentInfo = getElement(orgInfo.dname)

   if (typeof currentInfo == "string") {
      return "Unable to locate information on element"
   }

   // Check the children field and see if there are any differences
   var foundDiff = orgInfo.props.Children.length != currentInfo.props.Children.length
   if (!foundDiff) {
      // do a child by child check
      for (var i = 0; i < orgInfo.props.Children.length; i++) {
         if (orgInfo.props.Children[i] != currentInfo.props.Children[i]) {
            foundDiff = true
            break
         }
      }
   }
   if (foundDiff) {
      currentInfo.elementUI.Children =  orgInfo.props.Children
   }

   // See which fields are different, and set them
   for( var p in orgInfo.props )
      if( !currentInfo.props[p] || orgInfo.props[p] != currentInfo.props[p] ) {
        currentInfo.elementUI[ p ] = orgInfo.props[ p ]
      }

   return ""
}

/////////////////////////////////////////////////////////////////////////////////////
// Delete an existing organization
//
// dname: DName of the organization element to delete
//
function deleteElement(dname)
{
   // find child and parent
   var child
   try {
      child = formula.Root.findElement(dname)
   } catch (Exception) {
      return "Unable to locate the specified element:" + Exception
   }

   try {
      child.destroy()
      return ""
   } catch (Exception) {
      return "Unable to delete organization: " + Exception
   }
}

// @internal orgs.fs -1533dd4
