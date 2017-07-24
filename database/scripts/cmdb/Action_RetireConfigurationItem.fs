//
// Copyright (c) 2010 Novell, Inc.  All Rights Reserved.
//
// THIS WORK IS SUBJECT TO U.S. AND INTERNATIONAL COPYRIGHT LAWS AND TREATIES.  IT MAY NOT BE USED, COPIED, 
// DISTRIBUTED, DISCLOSED, ADAPTED, PERFORMED, DISPLAYED, COLLECTED, COMPILED, OR LINKED WITHOUT NOVELL'S
// PRIOR WRITTEN CONSENT. USE OR EXPLOITATION OF THIS WORK WITHOUT AUTHORIZATION COULD SUBJECT THE 
// PERPETRATOR TO CRIMINAL AND CIVIL LIABILITY.
//
// NOVELL PROVIDES THE WORK "AS IS," WITHOUT ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING WITHOUT THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT. NOVELL,
// THE AUTHORS OF THE WORK, AND THE OWNERS OF COPYRIGHT IN THE WORK ARE NOT LIABLE FOR ANY CLAIM,
// DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF,
// OR IN CONNECTION WITH THE WORK OR THE USE OR OTHER DEALINGS IN THE WORK.
//

// @debug off
// @param location The location to place retired configuration items (optional)
// @param retiredProperty The name of the retired state property (optional)
// 
// This script moves an element whose state changes from true/false or
// false/true to a desired "retired" location.  The location defaults
// to "org=Retired/root=Organizations", or can be specified by the user
// setting up the automation.
// 
// A property value called "Retired" is used to control the retire state.

formula.logCategory = 'Retire/Unretire' 

if( !this['location'] || ( this.location == '' ) )
   location = 'org=Retired/root=Organizations'
if( !this['retiredProperty'] || ( this.retiredProperty == '' ) )
   retiredProperty = 'Retired'

// TURN to true for debug log messages
var DEBUG = false

// Get dname to handle.
var dname = java.lang.String( element.dname )
if( DEBUG ) formula.log.info( 'Retiring element ' + dname + ' to ' + location + ' using property ' + retiredProperty )

if( property == retiredProperty || property.equals( retiredProperty ) )
{
   if( DEBUG ) formula.log.info( 'Looking at property ' + property + ' with value ' + newValue )
   if( newValue == 'true' || newValue.equalsIgnoreCase( 'true' ) )
   {
      if( !dname.endsWith( location ) )
      {
         var newParentDName = dname.replaceAll( 'root=Organizations$', location ).replaceFirst( '^([^\x2f]*)/(.*)', '$2' )
         formula.log.info( 'Retiring ' + dname + ' to ' + newParentDName )      
         element.move( server.getElement( newParentDName ) )
      }
      else
         formula.log.warn( 'CI ' + dname + ' is already stored in retired CI location' )
   }
   else if( newValue == 'false' || newValue.equalsIgnoreCase( 'false' ) )
   {
      if( dname.endsWith( location ) )
      {
         var newParentDName = dname.replaceAll( location + '$', 'root=Organizations' ).replaceFirst( '^([^\x2f]*)/(.*)', '$2' )
         formula.log.info( 'Un-retiring ' + dname + ' to ' + newParentDName )      
         element.move( server.getElement( newParentDName ) )
      }
      else
         formula.log.warn( 'CI ' + dname + ' is not stored in retired CI location' )
   }
   else
   {
      formula.log.warn( 'Not processing property value ' + newValue + ' (not true/false)' )
   }
}
else
{
   if( DEBUG ) formula.log.info( 'Not processing property ' + property )
}
