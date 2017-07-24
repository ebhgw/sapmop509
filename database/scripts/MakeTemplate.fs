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

function makeTemplateCopy( thisEvent ) {

	target1='workflow_step=Templates/applications=Catalogs/org=Service+Catalog/root=Organizations'

    return makeTemplateCopyTarget( thisEvent, target1 )

}

function makeTemplateCopyTarget( thisEvent, targ1 )
{
	dn1=thisEvent['ItemToBeStarted']
	thisele=formulaRootfindElement(dn1)
	nn2= thisele.name
	
	newElement = copyCloneElement(dn1, nn2, targ1, '', '')

	tempElement = formulaRootfindElement( newElement )

	formulalog.info( "Template creation completed successfully: " + tempElement.name );

	bsgTM = getChildrenMap( tempElement, ",")
	bsgTMks=bsgTM.keySet()
	bsgTMkeys=bsgTMks.iterator()

	foundcurrent=false
	currentwaslast=false
	nextitem=''

	while (bsgTMkeys.hasNext() ) {
		currentwaslast=false
		bsgTMkeysitem=bsgTMkeys.next()

		// get the items dname
		bsgtestitem=bsgTM.get(bsgTMkeysitem)
		bsgtestitemname=nameFromDname(bsgtestitem)

		formulalog.info( " BuildSecurityGroupsOneWF: " + bsgtestitem + " == " + bsgtestitemname);


	}
	return newElement;
}


function auditItem( thisEvent, action )
{
	dn = thisEvent[ 'ItemToBeStarted' ];
	thisEle = formulaRootfindElement( dn )

	thisEle[ 'Last Workflow Action' ] = action

	auditTrail = thisEvent['date'] + ">" + action + ": " + thisEvent['user'] + " : " + thisEvent['Reason'] +"\n"
	auditTrail += '\n';
	thisEle[ 'Workflow History' ] = element[ 'Workflow History' ] + auditTrail
}
