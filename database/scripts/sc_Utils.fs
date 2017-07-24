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

/*   Service Catalog Utils  sc_utils.fs */


/*
move the specified element 'fromDName' to a new location under 'toDname'.
if 'commentField' is specified, the 'comment' will be appended to the element
property
*/

//       moveElement( 'org=1/org=a/root=Organizations', 'org=b/root=Organizations', '', '' ) )

function formulaRootfindElement( targettofind ) {
	var targettofindtarget=null;
	try {
//		formulalog4j.getLogger( ourproperties + '.formulaRootfindElement' ).debug( 'finding ' + targettofind );

		//formula.log.info('finding ' + targettofind );

		targettofindtarget = formula.Root.findElement( targettofind );

//		formulalog4j.getLogger( ourproperties + '.formulaRootfindElement' ).debug( 'found ' + targettofind );
		//formula.log.info( 'found ' + targettofind );

	} catch( Exception ) {

		//formulalog4j.getLogger( ourproperties + '.formulaRootfindElement' ).warn( 'attempting to use found this.formula.Root.findElement for ' + targettofind );

		//formula.log.warn( 'attempting to use found this.formula.Root.findElement for ' + targettofind );
//		formulalog4j.getLogger( ourproperties + '.formulaRootfindElement' ).info( "   session "  + session );

		targettofindtarget = this.formula.Root.findElement( targettofind );

//		formulalog4j.getLogger( ourproperties + '.formulaRootfindElement' ).debug( 'this.formula.Root.findElement suceeded where formula.Root.findElement flailed ' + Exception );
		//formula.log.info( 'this.formula.Root.findElement suceeded where formula.Root.findElement flailed ' + Exception );
	}
	return targettofindtarget;
}


function deleteElement( fromDname )
{
	var returnCode = false;
	try
	{
		sourceElement = formulaRootfindElement( fromDname );
		sourceElement.perform( session, 'LifeCycle|Delete', [], [] )
		returnCode = true
	}
	catch( Exception )
	{
		formula.log.warn( "Exception delete Service Catalog element: " + Exception );
		formula.log.warn( "Source: " + fromDname );
	}
	return returnCode;
}


function moveElement( fromDname, toDname, commentField, comments )
{
	var returnCode = false;
	try
	{
		if (copyElement( fromDname, toDname, commentField, comments )) {
			returnCode = deleteElement( fromDname )
		}

	}
	catch( Exception )
	{
		formula.log.warn( "Exception moving Service Catalog element: " + Exception );
		formula.log.warn( "Source: " + fromDname );
		formula.log.warn( "Dest: " + toDname );
	}

	return returnCode;
}

function copyCloneElement( fromDname, newname, toDname, commentField, comments ) {


	try {

		if (true) {

			target = formulaRootfindElement( toDname );
			sourceElement = formula.Root.findElement( fromDname );

			thenewelement=target.copy( session.getReference(), sourceElement.id(), formula.relations.NAM, newname)

			formula.log.info( " thenewelement=" + thenewelement);
			returnCode = thenewelement;


		} else {
			// we really need the name passed from this
			returnCode=copyElement( fromDname, toDname, commentField, comments );

			if (returnCode) {

				lastpartpos=fromDname.indexOf("/");
				if (lastpartpos!=-1) {
					newCopiedDname=fromDname.substring(0, lastpartpos)+ "/" + toDname
					target = formula.Root.findElement( newCopiedDname );
					// 'target' is the element and we are changing the name
					target.rename( session.getReference(), newname )


					lastpartpos1=fromDname.indexOf("=");
					if (lastpartpos1!=-1) {
						newCopiedname=fromDname.substring(0, lastpartpos1)+ "=" + newname
					}

					targetparent = formula.Root.findElement( toDname );

					var pedna=new Array();
					pedna[0]=newCopiedname+"/"+toDname; // this is a linkto the alarm in an originating_event_id element
					targetparent.addChildren(pedna, true);

					// still need to wack the old child ref

					// this would change the class
					//target.rename( oldclass, newclass )

				}

				returnCode = true;

			}
		}
	} catch( Exception ) {

		formula.log.warn( "Exception copyCloneElement Service Catalog element: " + Exception );
		formula.log.warn( " Source: " + fromDname );
		formula.log.warn( " newname: " + newname );
		formula.log.warn( " Dest: " + toDname );

	}
	return returnCode;
}



function copyElement( fromDname, toDname, commentField, comments )
{
	var returnCode = false;
	try
	{
		target = formulaRootfindElement( toDname );
		sourceElement = formulaRootfindElement( fromDname );

		// we really need the name passed from this
		target.copy( session.getReference(), sourceElement.id(), formula.relations.NAM)
		// should return the new name....

		if( commentField != '' && sourceElement[ commentField ] ){
			sourceElement[ commentField ] = sourceElement[ commentField ] + '\n' + '\n' + comments;
		}

		returnCode = true;

	}
	catch( Exception )
	{
		formula.log.warn( "Exception copying Service Catalog element: " + Exception );
		formula.log.warn( "Source: " + fromDname );
		formula.log.warn( "Dest: " + toDname );
	}

	return returnCode;
}












/////////////////////////////////////////////////////////////////////////



function BuildSecurityGroups(mySecurityGroupViewsBuilderGroupsOrg) {

	retval=""

	// walk the workflows and build groups and users objects for every workflow found

	thisfunctionname="BuildSecurityGroups";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	// structure like

	formulalog.debug("starts " + mySecurityGroupViewsBuilderGroupsOrg);
	finddelim="::"

	try {
		mySecurityGroupViewsBuilderGroupsOrgtarget = formulaRootfindElement( mySecurityGroupViewsBuilderGroupsOrg );

		bsgTM=getChildrenMap(mySecurityGroupViewsBuilderGroupsOrgtarget, finddelim)

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

			formulalog.debug( " BuildSecurityGroupsOneWF: " + bsgtestitem + " == " + bsgtestitemname);
			// for every workflow
			BuildSecurityGroupsOneWF(bsgtestitem)


		}

	} catch( Exception ) {
		formulalog.warn( "Exception ::: " + Exception );
	}



	formulalog.debug("ends " + mySecurityGroupViewsBuilderGroupsOrg);


	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return retval
}





function BuildSecurityGroupsOneWF(mySecurityGroupViewsBuilderGroupsOrg) {

	// build the security for one workflow - the dname of the workflow is passed in

	retval=""

	thisfunctionnameWF="BuildSecurityGroupsOneWF";
	thatfunctionnameWF=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionnameWF + "." + thisfunctionnameWF  )

	// structure like

	formulalog.debug("starts " + mySecurityGroupViewsBuilderGroupsOrg);

	CatalogofFlows=mySecurityGroupViewsBuilderGroupsOrg

	CatalogofFlowsname=nameFromDnameUN(CatalogofFlows)


//	state[ourproperties].SecurityGroupViewsBuilderEndUserViews // =admin_users=End+User+Views/{servicecatalogroot}
//	state[ourproperties].SecurityGroupViewsBuildergroupsTarget // =admin_groups=Groups/{SecurityGroupViewsBuilderEndUserViews}
//	state[ourproperties].SecurityGroupViewsBuilderusersTarget   // =admin_users=Users/{SecurityGroupViewsBuilderEndUserViews}


//	place to keep the users Sandboxes, Carts, etc - these areas should not be deleted - they are created and then relaed back to the end users object under (state[ourproperties].SecurityGroupViewsBuilderusersTarget)
//	apps should use (state[ourproperties].SecurityGroupViewsBuilderusersTarget) for accessing a user - it will contain the children of interest either as ORG or NAM's
//	need to add the item beow to the prop files
//	state[ourproperties].SecurityGroupViewsBuilderEndUserViews // =admin_users=End+User+Views/{servicecatalogroot}


//	state[ourproperties].SecurityGroupViewsBuilderEndUserUsersWorkAreas // =admin_users=End+User+Views/{servicecatalogroot}
	var usersworkareas='admin_users=UsersData/' + state[ourproperties].SecurityGroupViewsBuilderEndUserViews;


// walk workflows under item
// for every workflow step
// for every group, create a group entry
	validworkflows=new java.util.TreeMap()

	// an item under workflow (at this time) exists under an org which has the same name as an item under the 'CatalogWorkflow'

	finddelim='::'

	// get a list of the workflows which are the subflows of the 1st levels
	//  return this as a list
	// no children means the item does not need a flow to be started
	//  though something 'has' to start it !??!!!

	// CatalogWorkflowParent has (at least one level which contains the highest levels of flows)
	//   010 Sandbox
	//   030 approved
	// 	subflows of approved...

	try {
		mySecurityGroupViewsBuilderGroupsOrgtarget = formulaRootfindElement( mySecurityGroupViewsBuilderGroupsOrg );

		cpmTM=getChildrenMap(mySecurityGroupViewsBuilderGroupsOrgtarget, finddelim)

		cpmTMks=cpmTM.keySet()
		cpmTMkeys=cpmTMks.iterator()

		foundcurrent=false
		currentwaslast=false
		nextitem=''

		var relatedCatalogWorkflowsItemelement=null

		addSandboxestotheParent=false;
		if (state[ourproperties].addSandboxestotheParent && state[ourproperties].addSandboxestotheParent!=null && state[ourproperties].addSandboxestotheParent!=undefined ) {
			if (state[ourproperties].addSandboxestotheParent=='true') {
				addSandboxestotheParent=true;
			}
		}
		//formulalog.debug(" addSandboxestotheParent = " + addSandboxestotheParent);


		// create the EndUserViews

		EndUserViews=null
		EndUserViews=create1org(state[ourproperties].SecurityGroupViewsBuilderEndUserViews, true, true, false, null);

		formulalog.debug(" EndUserViews=" + EndUserViews);


		// create the CatalogWorkflows workflow
		tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].CatalogWorkflowsParent)
		CatalogWorkflowsSearchParent=tMMformat.format(state[ourproperties].asHashTable)
		formulalog.debug("  CatalogWorkflowsSearchParent=" + CatalogWorkflowsSearchParent);

		relatedCatalogWorkflowsItem=null
		try {
			CatalogWorkflowsSearchParenttarget = formulaRootfindElement( CatalogWorkflowsSearchParent );
			CatalogWorkflowsSearchchildren=getChildrenMap(CatalogWorkflowsSearchParenttarget, finddelim)

			cwfcTMks=CatalogWorkflowsSearchchildren.keySet()
			cwfcTMkskeys=cwfcTMks.iterator()

			foundcurrentcwfcTMks=false
			while (cwfcTMkskeys.hasNext() ) {
				cwfcTMkskeysitem=cwfcTMkskeys.next()

				// get the items dname
				cwfctestitem=CatalogWorkflowsSearchchildren.get(cwfcTMkskeysitem)
				cwfctestitemname=nameFromDnameUN(cwfctestitem)
				//	CatalogofFlowsname=nameFromDnameUN(CatalogofFlows)
				try {
					itestitemname11=CatalogofFlowsname.indexOf(cwfctestitemname)
					if (itestitemname11==4) {
						foundcurrentcwfcTMks=true
						relatedCatalogWorkflowsItem=cwfctestitem
						break
					}
					//formulalog.debug(" Exists " + neweledname);
				} catch (Exception) {
				}
				//formulalog.debug("CatalogofFlowsname=" + CatalogofFlowsname + " cwfctestitemname=" + cwfctestitemname);

			}

			if (foundcurrentcwfcTMks) {
			} else {
				sCatalogofFlowsname=CatalogofFlowsname.substring(4)
				relatedCatalogWorkflowsItem="catalog="+sCatalogofFlowsname+"/"+CatalogWorkflowsSearchParent
			}
			//formulalog.debug("foundcurrentcwfcTMks=" + foundcurrentcwfcTMks + " relatedCatalogWorkflowsItem=" + relatedCatalogWorkflowsItem);

			relatedCatalogWorkflowsItemelement=create1org(relatedCatalogWorkflowsItem, true, true, false, null);

			formulalog.debug("relatedCatalogWorkflowsItem CatalogWorkflows=" + relatedCatalogWorkflowsItem);


		} catch (Exception) {
			formulalog.warn( 'CatalogWorkflows not found? ' + Exception  );
		}



		while (cpmTMkeys.hasNext() ) {
			currentwaslast=false

			cpmTMkeysitem=cpmTMkeys.next()

			// get the items dname
			testitem=cpmTM.get(cpmTMkeysitem)

			testitemname=nameFromDname(testitem)

			formulalog.debug( " BuilderGroupsOrg: " + testitem + " == " + testitemname);



			try {
				workflowsteptarget = formulaRootfindElement( testitem );

				workflowstepTM=getChildrenMap(workflowsteptarget, finddelim)

				workflowstepTMks=workflowstepTM.keySet()
				workflowstepTMkeys=workflowstepTMks.iterator()

				foundcurrent=false
				currentwaslast=false
				nextitem=''

				while (workflowstepTMkeys.hasNext() ) {
					workflowstepcurrentwaslast=false

					workflowstepTMkeysitem=workflowstepTMkeys.next()

					// get the items dname
					workflowsteptestitem=workflowstepTM.get(workflowstepTMkeysitem)

					workflowsteptestitemname=nameFromDname(workflowsteptestitem)

					formulalog.debug( "  workflowsteptestitem: " + workflowsteptestitem + " == " + workflowsteptestitemname);

			//Workflows=001+Hardware+Request/Workflows=025+Service+Catalog+%28In+Process%29/Workflows=Ordered+WorkFlows/script=MOSServiceCatalog/root=Elements
//					thisworkflow="Workflows="+ nameFromDnameUN(workflowsteptestitem)+"/Workflows="+ nameFromDnameUN(testitem)+"/Workflows=Ordered+WorkFlows/" + state[ourproperties].workflowadapterroot
//					thisworkflow="Workflows="+ nameFromDnameUN(workflowsteptestitem)+"/Workflows="+ CatalogofFlowsname+"/Workflows=Ordered+WorkFlows/" + state[ourproperties].workflowadapterroot

					thisworkflow="Workflows="+ nameFromDnameUN(workflowsteptestitem)+"/Workflows="+ CatalogofFlowsname+"/Workflow="+ nameFromDnameUN(testitem)+"/Catalog="+ state[ourproperties].thiscatalog +"/Workflows=Full+Ordered+WorkFlows/" + state[ourproperties].workflowadapterroot


					try {

						// this is a workflow step - like '030 Someone Does some work' - it has children which are the goups which get child relationships
						groupsecuitytarget = formulaRootfindElement( workflowsteptestitem );

						gstTM=getChildrenMap(groupsecuitytarget, finddelim)

						gstTMks=gstTM.keySet()
						gstTMkeys=gstTMks.iterator()

						foundcurrent=false
						currentwaslast=false
						nextitem=''

						// now we loop over the children of the step - these are 'groups' objects from Administration/Security/Groups
						while (gstTMkeys.hasNext() ) {
							gstcurrentwaslast=false

							gstTMkeysitem=gstTMkeys.next()

							// get the items dname
							gsttestitem=gstTM.get(gstTMkeysitem)

							gsttestitemname=nameFromDname(gsttestitem)

							formulalog.debug( "   gsttestitem: " + gsttestitem + " == " + gsttestitemname);


							parentthisgroup=state[ourproperties].SecurityGroupViewsBuildergroupsTarget
							parentthisgroupTarget=null
							parentthisgroupTarget=create1org(parentthisgroup, true, true, false, null);

							formulalog.debug(" parentthisgroupTarget=" + parentthisgroupTarget);



							parentthisuser=state[ourproperties].SecurityGroupViewsBuilderusersTarget
							parentthisuserTarget=null
							parentthisuserTarget=create1org(parentthisuser, true, true, false, null);

							formulalog.debug(" parentthisuserTarget=" + parentthisuserTarget);



							parentusersworkareas=usersworkareas
							parentusersworkareasTarget=null

							parentusersworkareasTarget=create1org(parentusersworkareas, true, true, false, null);

							formulalog.debug(" parentusersworkareasTarget=" + parentusersworkareasTarget);

							if (gsttestitem.indexOf("Contact=")==0) {
								try {
									thisMMlookupelement = formulaRootfindElement( gsttestitem );

									thisMMfielddata=getMMfieldData(thisMMlookupelement, '', 'Managed Objects User')
									formulalog.debug('thisMMlookupelement ='+ thisMMlookupelement + "    thisMMfielddata=" + thisMMfielddata)

									if (thisMMfielddata!=null) {
										mmfdgetFieldFormat=thisMMfielddata.getFieldFormat();

										thismmval=thisMMlookupelement['Managed Objects User'];
										formulalog.debug(" thismmval=" + thismmval);

										try {
											thisMMlookupelementparent = formulaRootfindElement( mmfdgetFieldFormat);
											var mpci=thisMMlookupelementparent.children;
											var mpci1=mpci.length;

											formulalog.debug(" " + thisMMlookupelementparent + " mpci1=" + mpci1);

											for (iju=0; iju < mpci1; iju++) {


												//formulalog.debug(" mpci[iju].name=" + mpci[iju].name + " ??? " + thismmval);
												if (mpci[iju].name.equals(thismmval)) {

													formulalog.debug(" mpci[iju]=" + mpci[iju]);


													formulalog.debug(" old gsttestitem=" + gsttestitem);

													//formulalog.debug(" mmfdgetFieldFormat=" + mmfdgetFieldFormat);

//													gsttestitem="user=" + nameFromDname(gsttestitem) + "/" + mmfdgetFieldFormat;

													gsttestitem=''+mpci[iju];

													formulalog.debug(" new gsttestitem=" + gsttestitem);


													// 'Workflow=' is special - this class is filtered from many children views in the 'Services' portlets (via regex filteroption)
													thisgroup="Workflow=Workflows/user="+ nameFromDname(gsttestitem) + "/" + parentthisuser

													formulalog.debug(" Contact gsttestitem=" + gsttestitem + "         thisgroup=" + thisgroup);
													
													// create the parent group under groups
													thisgrouptarget=create1org(thisgroup, true, true, false, null);
													try {
														var pedna=new Array();
														pedna[0]=thisworkflow;
														thisgrouptarget.addChildren(pedna, true);
														formulalog.debug( "     add workflow " + thisworkflow + "     to     " + thisgrouptarget)
														
														formulalog.debug( "     add workflow " + thisworkflow + " to " + thisgrouptarget.name)

													} catch (Exception) {
														formulalog.warn( 'add thisworkflow new child to thisgrouptarget ' + Exception  );
													}


													thisusername=nameFromDnameUN(gsttestitem)

													thisuser="user="+thisusername+"/"+parentthisuser

													thisuserworkareas="user="+thisusername+"/"+parentusersworkareas

													formulalog.debug( "    thisusername " + thisusername)

													// sandboxes are a shared area and should NOT exist user the 'UserData' area
													thisuserSandboxes="Sandboxes=SandBoxes/" + thisuser

													thisuserSandbox="Sandbox=SandBox/" + thisuserworkareas

													thisuserSubscribing="Subscribing=Subscribing/" + thisuserworkareas
													thisuserSubscribed="Subscribed=Subscribed/" + thisuserworkareas

													thisrealuser=gsttestitem

													if (processoneuser(thisworkflow, grouptarget, onegroupORusername, thisuser, thisgroup, thisgrouptarget, thisuserworkareas, thisuserSandbox, thisuserSandboxes, thisuserSubscribing, thisuserSubscribed, thisrealuser, parentthisgroupTarget, parentthisuserTarget, parentusersworkareasTarget, relatedCatalogWorkflowsItemelement)) {

													}

													break;
												}
											}

										} catch (Exception) {
											formulalog.warn('Problem Generating Contact user -- '+ Exception)
										}
									}
								} catch (Exception) {
									formulalog.warn('Problem Generating Contact user '+ Exception)
								}

							}
							else if (gsttestitem.indexOf("user=")==0) {

									formulalog.debug(" gsttestitem=" + gsttestitem);
									// 'Workflow=' is special - this class is filtered from many children views in the 'Services' portlets (via regex filteroption)
									thisgroup="Workflow=Workflows/"+parentthisuser

									formulalog.debug(" USER gsttestitem=" + gsttestitem + "         thisgroup=" + thisgroup);

									// create the parent group under groups
									thisgrouptarget=create1org(thisgroup, true, true, false, null);
									try {
										var pedna=new Array();
										pedna[0]=thisworkflow;
										thisgrouptarget.addChildren(pedna, true);
										formulalog.debug( "     add workflow " + thisworkflow + " to " + thisgrouptarget.name)

									} catch (Exception) {
										formulalog.warn( 'add thisworkflow new child to thisgrouptarget ' + Exception  );
									}


									thisusername=nameFromDnameUN(gsttestitem)

									thisuser="user="+thisusername+"/"+parentthisuser

									thisuserworkareas="user="+thisusername+"/"+parentusersworkareas

									formulalog.debug( "    thisusername " + thisusername)

									// sandboxes are a shared area and should NOT exist user the 'UserData' area
									thisuserSandboxes="Sandboxes=SandBoxes/" + thisuser

									thisuserSandbox="Sandbox=SandBox/" + thisuserworkareas

									thisuserSubscribing="Subscribing=Subscribing/" + thisuserworkareas
									thisuserSubscribed="Subscribed=Subscribed/" + thisuserworkareas

									thisrealuser=gsttestitem

									if (processoneuser(thisworkflow, grouptarget, onegroupORusername, thisuser, thisgroup, thisgrouptarget, thisuserworkareas, thisuserSandbox, thisuserSandboxes, thisuserSubscribing, thisuserSubscribed, thisrealuser, parentthisgroupTarget, parentthisuserTarget, parentusersworkareasTarget, relatedCatalogWorkflowsItemelement)) {

									}

							} else
							if (gsttestitem.indexOf("group=")==0) {


								formulalog.debug(" gsttestitem = " + gsttestitem);


								thisgroup="group="+encodeURL(gsttestitemname)+"/"+parentthisgroup

								// create the parent group under groups
								thisgrouptarget=create1org(thisgroup, true, true, false, null);
								try {
									var pedna=new Array();
									pedna[0]=thisworkflow;
									thisgrouptarget.addChildren(pedna, true);
									formulalog.debug( "     add workflow " + thisworkflow + " to " + thisgrouptarget.name)

								} catch (Exception) {
									formulalog.warn( 'add thisworkflow new child to thisgrouptarget ' + Exception  );
								}


							// groups contain users but you have to use an API to get to the users in a group
								try {
									grouptarget = formulaRootfindElement( gsttestitem );
									if (grouptarget.group && grouptarget.group!=null && grouptarget.group!=undefined) {

										grouptargetgroups=grouptarget.group.memberList()

							// for every group - walk the users in the group
							// and
										if (grouptargetgroups.length==0) {
											formulalog.warn( "   no users in " + gsttestitem + " which is used in " + gsttestitem);
										} else {
											for (i12=0; i12 < grouptargetgroups.length; i12++) {
												onegroupORusername=grouptargetgroups[i12];

												formulalog.debug( "    groupmember " + onegroupORusername)

												thisuser="user="+encodeURL(onegroupORusername)+"/"+parentthisuser

												thisuserworkareas="user="+encodeURL(onegroupORusername)+"/"+parentusersworkareas


												thisuserSandbox="Sandbox=SandBox/" + thisuserworkareas


												// sandboxes are a shared area and should NOT exist user the 'UserData' area
												thisuserSandboxes="Sandboxes=SandBoxes/" + thisuser

												thisuserSubscribing="Subscribing=Subscribing/" + thisuserworkareas
												thisuserSubscribed="Subscribed=Subscribed/" + thisuserworkareas

												thisrealuser="user="+onegroupORusername+"/users=Users/security=Security/root=Administration"


												if (processoneuser(thisworkflow, grouptarget, onegroupORusername, thisuser, thisgroup, thisgrouptarget, thisuserworkareas, thisuserSandbox, thisuserSandboxes, thisuserSubscribing, thisuserSubscribed, thisrealuser, parentthisgroupTarget, parentthisuserTarget, parentusersworkareasTarget, relatedCatalogWorkflowsItemelement)) {

												}

											}
										}

									} else {
										formulalog.warn( "   problem grouptarget.groups missing");
									}



								} catch( Exception ) {
									formulalog.warn( "Exception --:grp:: " + Exception );
								}

							} else {
							}

						}
					} catch( Exception ) {
						formulalog.warn( "Exception --::: " + Exception );
					}

				}
			} catch( Exception ) {
				formulalog.warn( "Exception :::-- " + Exception );
			}


		}
	} catch( Exception ) {
		formulalog.warn( "Exception ::: " + Exception );
	}


	formulalog.debug("ends");


	formulalog = formulalog4j.getLogger( thatfunctionnameWF )

	return retval;
}




function processoneuser(thisworkflow, grouptarget, onegroupORusername, thisuser, thisgroup, thisgrouptarget, thisuserworkareas, thisuserSandbox, thisuserSandboxes, thisuserSubscribing, thisuserSubscribed, thisrealuser, parentthisgroupTarget, parentthisuserTarget, parentusersworkareasTarget, relatedCatalogWorkflowsItemelement) {
	var retval=true;

/*
	// create the parent group under groups
	thisgrouptarget=create1org(thisgroup, true, true, false, null);

	try {
		var pedna=new Array();
		pedna[0]=thisworkflow;
		thisgrouptarget.addChildren(pedna, true);
		formulalog.debug( "     add workflow " + thisworkflow + " to " + thisgrouptarget.name)

	} catch (Exception) {
		formulalog.warn( 'add thisworkflow new child to thisgrouptarget ' + Exception  );
	}
*/



	// this item is under 'Users'
	thisusertarget=create1org(thisuser, true, true, false, null);



	// this item is under 'Userworkareas'
	thisuserworkareastarget=create1org(thisuserworkareas, true, true, false, null);
		//create the sandbox for the user
		thisuserSandboxtarget=create1org(thisuserSandbox, true, true, false, null);

		//create the thisuserSubscribing for the user
		thisuserSubscribingtarget=create1org(thisuserSubscribing, true, true, false, null);

		//create the thisuserSubscribed for the user
		thisuserSubscribedtarget=create1org(thisuserSubscribed, true, true, false, null);


	// these are added to the user under 'Users'
	try {
		// add the group to the user
		var pedna=new Array();
		pedna[0]=thisrealuser;
		pedna[1]=thisgroup;

		pedna[2]=thisuserSandboxtarget;
		pedna[3]=thisuserSubscribingtarget;
		pedna[4]=thisuserSubscribedtarget;

		thisusertarget.addChildren(pedna, true);

		formulalog.debug( "     added thisgroup to " + thisusertarget.name)

	} catch (Exception) {
		formulalog.warn( 'thisrealuser & thisgroup to thisusertarget ' + Exception  );
	}



	// inhibit the alarms from the org into the parent
	//create the sandboxes for the user
		thisuserSandboxestarget=create1org(thisuserSandboxes, false, true, true, null);


// add any catalog the user has access to as a sandboxes item
		relatedCatalogWorkflowsItemelementSandboxElement=true;

		//formulalog.debug( relatedCatalogWorkflowsItemelement + ' relatedCatalogWorkflowsItemelement ' +relatedCatalogWorkflowsItemelement['SandboxElement']  );
		if (relatedCatalogWorkflowsItemelement['SandboxElement'] && relatedCatalogWorkflowsItemelement['SandboxElement']!=null && relatedCatalogWorkflowsItemelement['SandboxElement']!=undefined) {
			if (relatedCatalogWorkflowsItemelement['SandboxElement']=='false' || relatedCatalogWorkflowsItemelement['SandboxElement']=='no') {
				relatedCatalogWorkflowsItemelementSandboxElement=false;
			}
		}
		if (relatedCatalogWorkflowsItemelementSandboxElement) {
			try {
				//thisuserSandboxestarget=formulaRootfindElement(thisuserSandboxes);
				//thisuserSandboxestarget['DisplaySourceElements']=true;

				// find the parent and add the child to the parent
				var pedna=new Array();
				pedna[0]=relatedCatalogWorkflowsItem;
				thisuserSandboxestarget.addChildren(pedna, true);

			} catch (Exception) {
				formulalog.warn( 'addChildren thisuserSandboxes new relatedCatalogWorkflowsItem ' + Exception  );
			}
		}


	// add the user sandbox to the sandboxes item
		try {
			var pedna=new Array();
			pedna[0]=thisuserSandbox;
			thisuserSandboxestarget.addChildren(pedna, true);

		} catch (Exception) {
			formulalog.warn( 'addChildren thisuserSandboxes new relatedCatalogWorkflowsItem ' + Exception  );
		}



	return retval;
}


function create1org(orgname, addtoparent, displaysource, ignoreChildalarms, anotherchild) {

	var orgnametarget=null;

	try {
		orgnametarget=formulaRootfindElement(orgname);
		//formulalog.debug(" Exists " + neweledname);

		if (addtoparent) {
			// find the parent and add the child to the parent

			formulalog.debug(" create1org addtoparent");

			var pedna=new Array();
			pedna[0]=orgname;
			if (anotherchild!=null) {
				pedna[1]=anotherchild;
			}
			orgnameparenttarget=formulaRootfindElement(getParentDname(orgname));
			orgnameparenttarget.addChildren(pedna, true);

		} else {

		}
		orgnametarget['IgnoreChildAlarms']=ignoreChildalarms;

		formulalog.debug(" create1org Complete " + orgname);

		formulalog.debug(" create1org Complete ignoreChildalarms=" + ignoreChildalarms);

		formulalog.debug(" create1org Complete orgnametarget=" + orgnametarget);

	} catch (Exception) {

		try {
			formulalog.debug(" create1org     Creating " + orgname);

			// create the element
			var bb=session.getElement(orgname);
			formulalog.debug(" create1org     Created " + orgname);

			orgnametarget=formulaRootfindElement(orgname);
			orgnametarget['DisplaySourceElements']=displaysource;

			if (addtoparent) {
				// find the parent and add the child to the parent
				formulalog.debug(" create1org addtoparent " + orgname);

				var pedna=new Array();
				pedna[0]=orgname;
				if (anotherchild!=null) {
					pedna[1]=anotherchild;
				}

				orgnameparenttarget=formulaRootfindElement(getParentDname(orgname));
				orgnameparenttarget.addChildren(pedna, true);

			} else {

			}
			orgnametarget['IgnoreChildAlarms']=ignoreChildalarms;

			formulalog.debug(" create1org Create Complete " + orgname);

			formulalog.debug(" create1org Create Complete ignoreChildalarms=" + ignoreChildalarms);

			formulalog.debug(" create1org Create Complete orgnametarget=" + orgnametarget);

		} catch (Exception) {
			formulalog.warn( 'create1org new child to parent' + Exception  );
		}
	}

	return orgnametarget;

}


function nameFromDnameUN(adname) {
	retval=''
	partsdn=adname.split("/");
	if (partsdn.length > 0) {
		onepartsdn=partsdn[0].split("=");
		if (onepartsdn.length==2) {
			retval=onepartsdn[1]
		}
	}
	return retval
}


function getParentNameFromDname(adname) {
	retval=''
	partsdn=adname.split("/");
	if (partsdn.length > 1) {
		onepartsdn=partsdn[1].split("=");
		if (onepartsdn.length==2) {
			retval=onepartsdn[1]
		}
	}
	return decodeURL(retval)
}


function getParentNameFromDnameUN(adname) {
	retval=''
	partsdn=adname.split("/");
	if (partsdn.length > 1) {
		onepartsdn=partsdn[1].split("=");
		if (onepartsdn.length==2) {
			retval=onepartsdn[1]
		}
	}
	return decodeURL(retval)
}


function getParentDname(adname) {
	retval='foobar'
	partsdn=adname.indexOf("/");
	if (partsdn!=-1) {
		retval=adname.substring(partsdn+1)
	}
	return retval
}


