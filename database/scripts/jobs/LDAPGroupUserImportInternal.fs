//
// Copyright � 2014 NetIQ Corporation.  All Rights Reserved.
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
function performLDAPGroupUserImport(xiGroupName)
{
    var groupElement;

    try
    {
        var elementName = xiGroupName.replace(/ /g, '+');
        elementName = 'group='+elementName+'/groups=Groups/security=Security/root=Administration';

        groupElement = formula.Root.findElement(elementName);
    }
    catch( ex )
    {
        var exMsg = ex.key != undefined ? ex.key : ex.getMessage()
        formula.log.error( "Unable to locate the group meta element: " + xiGroupName + ":\n" + exMsg )
        return
    }

    try
    {
        groupElement.performLDAPGroupUserImport();
    }
    catch( ex )
    {
        var exMsg = ex.key != undefined ? ex.key : ex.getMessage()
        formula.log.error( "Could not perform LDAP group import: " + xiGroupName + ":\n" + exMsg )
        return
    }
}
// @internal LDAPGroupUserImport.fs
