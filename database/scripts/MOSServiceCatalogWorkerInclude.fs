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

// // @opt -1

// function to do work related to servicecatalog

//ourproperties='MOSservicecatalog'

//var formulalog = Packages.org.apache.log4j.Category.getInstance( ourproperties )
var formulalog4j = Packages.org.apache.log4j.Logger
var formulalog = formulalog4j.getLogger( ourproperties )



// load scripts containing functions to be called later
load('loadPropertiesFile.fs')
load('runURL.fs')


load("sc_Utils.fs")
load("dborbcomm2.fs")
load("cmScriptInclude.fs")


// atttempt to load a script so its functions are available
if (state[ourproperties] && state[ourproperties]!=null && state[ourproperties]!=undefined &&
	state[ourproperties].loadScript1 && state[ourproperties].loadScript1!=undefined && state[ourproperties].loadScript1!=null  && state[ourproperties].loadScript1!='') {

	try {
		load(state[ourproperties].loadScript1)
		formulalog.debug( "state[ourproperties].loadScript1=" + state[ourproperties].loadScript1 );
	} catch( Exception ) {
		formulalog.error( "Exception 1 state[ourproperties].loadScript1=" + state[ourproperties].loadScript1 + "<-->" + Exception );
	}

}


// atttempt to load a script so its functions are available
if (state[ourproperties] && state[ourproperties]!=null && state[ourproperties]!=undefined &&
	state[ourproperties].loadScript2 && state[ourproperties].loadScript2!=undefined && state[ourproperties].loadScript2!=null  && state[ourproperties].loadScript2!='') {

	try {
		load(state[ourproperties].loadScript2)
		formulalog.debug( "state[ourproperties].loadScript2=" + state[ourproperties].loadScript2 );
	} catch( Exception ) {
		formulalog.error( "Exception 1 state[ourproperties].loadScript2=" + state[ourproperties].loadScript2 + "<-->" + Exception );
	}

}


// atttempt to load a script so its functions are available
if (state[ourproperties] && state[ourproperties]!=null && state[ourproperties]!=undefined &&
	state[ourproperties].loadScript3 && state[ourproperties].loadScript3!=undefined && state[ourproperties].loadScript3!=null  && state[ourproperties].loadScript3!='') {

	try {
		load(state[ourproperties].loadScript3)
		formulalog.debug( "state[ourproperties].loadScript3=" + state[ourproperties].loadScript3 );
	} catch( Exception ) {
		formulalog.error( "Exception 1 state[ourproperties].loadScript3=" + state[ourproperties].loadScript3 + "<-->" + Exception );
	}

}


// atttempt to load a script so its functions are available
if (state[ourproperties] && state[ourproperties]!=null && state[ourproperties]!=undefined &&
	state[ourproperties].loadScript4 && state[ourproperties].loadScript4!=undefined && state[ourproperties].loadScript4!=null  && state[ourproperties].loadScript4!='') {

	try {
		load(state[ourproperties].loadScript4)
		formulalog.debug( "state[ourproperties].loadScript4=" + state[ourproperties].loadScript4 );
	} catch( Exception ) {
		formulalog.error( "Exception 1 state[ourproperties].loadScript4=" + state[ourproperties].loadScript4 + "<-->" + Exception );
	}

}



/////////////////////////////////////////////////////////////////////////
////////// OPERATIONS - right clicks or element.perform ...
/////////////////////////////////////////////////////////////////////////


function sendAMessageq(rwfht, commandToSend, formulalogq) {


	thisfunctionname="sendAMessageq";
	thatfunctionname=formulalogq.getName()
	formulalogq = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )

	formulalogq.info("starts ");


	tMMformat=new Packages.com.proserve.MappedMessageFormat(commandToSend)

	commandToSend=tMMformat.format(rwfht)
	// handle replaced items containing replacements
	commandToSend=tMMformat.format(rwfht)

	sendcommand(state[ourproperties].workflowadapterhost, state[ourproperties].workflowadapterport, commandToSend )


	formulalogq.info("ends ");

	formulalogq = formulalog4j.getLogger( thatfunctionname )

}


function sendAMessage(rwfht, commandToSend) {


	thisfunctionname="sendAMessage";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )

	formulalog.info("starts ");


	tMMformat=new Packages.com.proserve.MappedMessageFormat(commandToSend)

	commandToSend=tMMformat.format(rwfht)
	// handle replaced items containing replacements
	commandToSend=tMMformat.format(rwfht)

	sendcommand(state[ourproperties].workflowadapterhost, state[ourproperties].workflowadapterport, commandToSend )


	formulalog.info("ends ");

	formulalog = formulalog4j.getLogger( thatfunctionname )

}



function RejectWorkFlow(myargs) {
	pueretval=true

	thisfunctionname="RejectWorkFlow";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )

	formulalog.info("starts ");


	formulalog.info(myargs.length+ ' <----> ' + arguments.length);
	logArgs('myargs', myargs)
	logArgs('arguments', arguments)

	commandToSend=state[ourproperties].RejectWorkFlowcommandToSend

	rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)

	rwfht.put('Reason', "'" + myargs[0] + "'")
	rwfht.put('Reason2', "'" + myargs[1] + "'")
	rwfht.put('dname', "'" + arguments[1] + "'")

		if (this.user) {
			formulalog.info(myargs.length+ ' <--r--> ' + arguments.length + " " + user.name);
			rwfht.put('user', "'" + user.name + "'")
		} else
		if (arguments.length>3) {
			formulalog.info(myargs.length+ ' <--r--> ' + arguments.length + " using " + arguments[3]);
			rwfht.put('user', "'" + arguments[3] + "'")
		} else {
			formulalog.info(myargs.length+ ' <--r--> ' + arguments.length + " using systemInternal");
			rwfht.put('user', "'systemInternal'")
		}



	rwfht.put('morefields', "")

	if (arguments.length>2) {
		rwfht.put('alarm.originating_event_id', "'" + arguments[2][0]['originating_event_id'] + "'")
	}

	sendAMessage(rwfht, commandToSend)

	formulalog.info("ends ");

	formulalog = formulalog4j.getLogger( thatfunctionname )

	// allow the Event to be removed from the adapter by returning false
	return pueretval
}


function AcceptWorkFlow(myargs) {
	pueretval=true

	thisfunctionname="AcceptWorkFlow";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )

	formulalog.info("starts " + myargs.length+ ' <----> ' + arguments.length);

	logArgs('myargs', myargs)
	logArgs('arguments', arguments)

	commandToSend=state[ourproperties].AcceptWorkFlowcommandToSend

	rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)

//	formulalog.debug('rwfht.size()='+ rwfht.size())

	rwfht.put('Reason', "'" + myargs[0] + "'")
	rwfht.put('Reason2', "'" + myargs[1] + "'")
	rwfht.put('dname', "'" + arguments[1] + "'")

		if (this.user) {
			formulalog.info(myargs.length+ ' <--a--> ' + arguments.length + " " + user.name);
			rwfht.put('user', "'" + user.name + "'")
		} else
		if (arguments.length>3) {
			formulalog.info(myargs.length+ ' <--a--> ' + arguments.length + " using " + arguments[3]);
			rwfht.put('user', "'" + arguments[3] + "'")
		} else {
			formulalog.info(myargs.length+ ' <--a--> ' + arguments.length + " using systemInternal");
			rwfht.put('user', "'systemInternal'")
		}


	rwfht.put('morefields', "")

	if (arguments.length>2) {
		rwfht.put('alarm.originating_event_id', "'" + arguments[2][0]['originating_event_id'] + "'")
	}

	sendAMessage(rwfht, commandToSend)

	formulalog.info("ends ");

	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return pueretval
}


function StartWorkFlow(myargs) {
	pueretval=true

	thisfunctionname="StartWorkFlow";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	formulalog.info("starts " + myargs.length+ ' <----> ' + arguments.length);

	logArgs('myargs', myargs)
	logArgs('arguments', arguments)

	commandToSend=state[ourproperties].StartWorkFlowcommandToSend
	rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)

//	formulalog.debug('rwfht.size() ='+ rwfht.size())


	goodflow=true

	thisworkflow=''
	thisworkflowmyargs=myargs[0]

	thisworkflowsMap=null
	thisworkflowsMapFromChoice=null

	tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].defaultWorkFlow)
	defaultWorkFlowthisworkflow=tMMformat.format(state[ourproperties].asHashTable)



	if (thisworkflow.equals('')) {
		thisworkflow=thisworkflowmyargs
		if (thisworkflow.equals('')) {
		} else {
			// validate the flow
			// should check that the value is a value workflow
			formulalog.info('passed property thisworkflow= ='+ thisworkflow)
			try {
				thisworkflowtarget = formulaRootfindElement( thisworkflow );

				formulalog.info('passed property exists ='+ thisworkflow)

			} catch( Exception ) {
				formulalog.warn( "Exception passed property finding constructed workflow name: " + Exception );
				goodflow=false
				thisworkflow=''
			}


		}
	}

	if (thisworkflow.equals('')) {
		if (arguments.length>1) {
			thiselement=arguments[1]
			try {
				formulalog.info(' arguments[1] being used to load workflows associated to its parent ='+ thiselement)
				thiselementtarget = formulaRootfindElement( thiselement );

				thisworkflowsMap=getWorkflowsFor(thiselement)

				formulalog.info('  thisworkflowsMap for parent ='+ thisworkflowsMap)
			} catch( Exception ) {
				formulalog.warn( "Exception loading thisworkflowsMap for parent : " + Exception );
				goodflow=false
			}
		}
	}

	if (thisworkflow.equals('')) {
		// arg 2 should be passed in and available
		if (arguments.length>1) {
			thiselement=arguments[1]
			try {
				formulalog.info(' arguments[1] being checked for workflow in property field ('+state[ourproperties].elementWorkflowProperty+') of ==>' + thiselement)

				thiselementtarget = formulaRootfindElement( thiselement );

				tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].elementWorkflowProperty)
				thiselementWorkflowProperty=tMMformat.format(state[ourproperties].asHashTable)

				formulalog.info(' arguments[1] ' + thiselement + ' --- being checked for property ' + thiselementWorkflowProperty)

				/////////////////////////////////////////////////////////////////////////////////////////////
				/////   verifgy the MM workflow
				/////////////////////////////////////////////////////////////////////////////////////////////
				try {

					formulalog.info(' getMMfieldData for ='+ thiselementtarget)
					thisMMlookupelement = formulaRootfindElement( thiselementtarget );

					formulalog.info(' getMMfieldData thiselementWorkflowProperty='+ thiselementWorkflowProperty)
					thisMMfielddata=getMMfieldData(thisMMlookupelement, '', thiselementWorkflowProperty)
					formulalog.info(' thisMMfielddata ='+ thisMMfielddata)
					if (thisMMfielddata!=null) {
						mmfdgetFieldFormat=thisMMfielddata.getFieldFormat();
						if (mmfdgetFieldFormat!=null) {
							formulalog.info(' mmfdgetFieldFormat ='+ mmfdgetFieldFormat)

							formulalog.info('.. thisMMfielddata.getDescription===='+ thisMMfielddata.getDescription());
							if (true) {
								formulalog.info("  .. thisMMfielddata.getPropertyName()=" + thisMMfielddata.getPropertyName() );
								formulalog.info("  .. thisMMfielddata.getDisplayLabel()=" + thisMMfielddata.getDisplayLabel() );
								formulalog.info("  .. thisMMfielddata.getTooltip()=" + thisMMfielddata.getTooltip() );
								formulalog.info("  .. thisMMfielddata.getRangeName()=" + thisMMfielddata.getRangeName() );
								formulalog.info("  .. thisMMfielddata.getInputMask()=" + thisMMfielddata.getInputMask() );
								formulalog.info("  .. thisMMfielddata.getFieldFormat()=" + thisMMfielddata.getFieldFormat() );
								formulalog.info("  .. thisMMfielddata.getDefaultValue()=" + thisMMfielddata.getDefaultValue() );
								formulalog.info("  .. thisMMfielddata.getDefaultValue().getClass()=" + thisMMfielddata.getDefaultValue().getClass() );
							}



							try {
								if (thisMMfielddata.getDescription().indexOf("_")==0 && thisMMfielddata.getDescription().indexOf("setdefault")!=-1) {
									thisworkflowsMapFromChoice=mmfdgetFieldFormat
									formulalog.info(' -- thisMMfielddata.getDescription='+ thisMMfielddata.getDescription());
								} else {
									mmfdgetFieldFormattarget = formulaRootfindElement( mmfdgetFieldFormat );

									formulalog.info('  getWorkflowsForthiselement ='+ mmfdgetFieldFormattarget)
									thisworkflowsMapFromChoice=getWorkflowsForthiselement(mmfdgetFieldFormat, mmfdgetFieldFormat)
								}

								formulalog.info('  thisworkflowsMapFromChoice ='+ thisworkflowsMapFromChoice)

								thisworkflowSimpleName=thiselementtarget[thiselementWorkflowProperty]

								formulalog.info(' thisworkflowSimpleName..='+ thisworkflowSimpleName)

								rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)

								rwfht.put('elementWorkflowPropertyValue', encodeURL(thisworkflowSimpleName))

								tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].elementWorkflowPropertyConstructedDname)
								thisworkflow=tMMformat.format(rwfht)

								formulalog.info(' thisworkflow 1 ='+ thisworkflow)

								// run it through one more time in cause the replace value has replace-able values
								tMMformat=new Packages.com.proserve.MappedMessageFormat(thisworkflow)
								thisworkflow=tMMformat.format(rwfht)


								try {
									thisworkflowtarget = formulaRootfindElement( thisworkflow );

									formulalog.info(' element property thisworkflowsMapFromChoice thisworkflow= ='+ thisworkflow)

									if (thisworkflowsMapFromChoice!=null) {
										goodflow=verifyWorkflow(thisworkflowsMapFromChoice, thisworkflowtarget)
										if (goodflow) {
											thisworkflow=thisworkflowtarget
											formulalog.info(' this workflow thisworkflowsMapFromChoice is VALID and will be used ' + thisworkflowtarget)
										}
									} else {
										formulalog.warn( "problem element property  thisworkflowsMap was null" );
										goodflow=false
										thisworkflow=''
									}
								} catch( Exception ) {
									formulalog.warn( "Exception element property  finding constructed workflow name: " + Exception );
									goodflow=false
									thisworkflow=''
								}


								if (goodflow) {
								} else {
									try {
										thisworkflowtarget = formulaRootfindElement( thisworkflow );

										formulalog.info(' is a flow -==- element property thisworkflow is valid element '+ thisworkflow)

										goodflow=true
			/*
										if (thisworkflowsMap!=null) {
											goodflow=verifyWorkflow(thisworkflowsMap, thisworkflowtarget)
											if (goodflow) {
												thisworkflow=thisworkflowtarget
												formulalog.info(' is a flow -- this workflow is VALID and can be used ' + thisworkflowtarget)
											}
										} else {
											formulalog.warn( "is a flow -- problem element property  thisworkflowsMap was null" );
											goodflow=false
											thisworkflow=''
										}
			*/

									} catch( Exception ) {
										formulalog.warn( "is a flow -- Exception element property  finding constructed workflow name: " + Exception );
										goodflow=false
										thisworkflow=''
									}
								}


							} catch( Exception ) {
								formulalog.warn( "Exception getMMfieldData findElement : " + Exception );
							}

						} else {
							formulalog.info(' mmfdgetFieldFormat is null ')
						}
					} else {
						formulalog.info(' thisMMfielddata is null ')
					}


				} catch( Exception ) {
					formulalog.warn( "Exception getMMfieldData : " + Exception );
				}
				if (thisworkflow.equals('')) {

					/////////////////////////////////////////////////////////////////////////////////////////////
					/////   verifgy the Parent elemnt property value as a workflow
					/////////////////////////////////////////////////////////////////////////////////////////////
					thiselementtargetparent = formulaRootfindElement( getParentDname(thiselement) );

					formulalog.info(' using parent ' + thiselementtargetparent + ' thisworkflowSimpleName ='+ thisworkflowSimpleName);


					thisworkflowSimpleName=thiselementtargetparent[thiselementWorkflowProperty]
					formulalog.info(' thisworkflowSimpleName ='+ thisworkflowSimpleName)

					rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)

					rwfht.put('elementWorkflowPropertyValue', encodeURL(thisworkflowSimpleName))

					tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].elementWorkflowPropertyConstructedDname)
					thisworkflow=tMMformat.format(rwfht)

					formulalog.info(' thiselementtargetparent[thiselementWorkflowProperty] thisworkflow 2 ='+ thisworkflow)

					// run it through one more time in cause the replace value has replace-able values
					tMMformat=new Packages.com.proserve.MappedMessageFormat(thisworkflow)
					thisworkflow=tMMformat.format(rwfht)


					try {
						thisworkflowtarget = formulaRootfindElement( thisworkflow );

						formulalog.info(' element property thisworkflow= ='+ thisworkflow)

						if (thisworkflowsMap!=null) {
							goodflow=verifyWorkflow(thisworkflowsMap, thisworkflowtarget)
							if (goodflow) {
								thisworkflow=thisworkflowtarget
								formulalog.info(' this workflow is VALID and shall be used ' + thisworkflowtarget)
							}
						} else {
							formulalog.warn( "problem element property  thisworkflowsMap was null" );
							goodflow=false
							thisworkflow=''
						}
					} catch( Exception ) {
						formulalog.warn( "Exception element property  finding constructed workflow name: " + Exception );
						goodflow=false
						thisworkflow=''
					}

					if (goodflow) {
					} else {
						try {
							thisworkflowtarget = formulaRootfindElement( thisworkflow );

							formulalog.info(' is a flow -- element property thisworkflow is valid element '+ thisworkflow)

							goodflow=true
/*
							if (thisworkflowsMap!=null) {
								goodflow=verifyWorkflow(thisworkflowsMap, thisworkflowtarget)
								if (goodflow) {
									thisworkflow=thisworkflowtarget
									formulalog.info(' is a flow -- this workflow is VALID and wil be used ' + thisworkflowtarget)
								}
							} else {
								formulalog.warn( "is a flow -- problem element property  thisworkflowsMap was null" );
								goodflow=false
								thisworkflow=''
							}
*/

						} catch( Exception ) {
							formulalog.warn( "is a flow -- Exception element property  finding constructed workflow name: " + Exception );
							goodflow=false
							thisworkflow=''
						}
					}

				}

			} catch( Exception ) {
				formulalog.warn( "Exception element property StartWorkFlow determine workflow name: " + Exception );
				goodflow=false
			}
		}

	}

	if (thisworkflow.equals('')) {

		if (arguments.length>1) {
			thiselement=arguments[1]

			// verify the workflow list for this element
			// 1 has a workflow
			// 2 has more than 1 - if only one, then it must be used overiding all values passed (present or future or future past or past particilple? perfect)


			try {
				formulalog.info(' arguments[1] being checked for workflows associated to its parent ='+ thiselement)
				thiselementtarget = formulaRootfindElement( thiselement );


				// thisworkflowsMap=getWorkflowsFor(thiselement)

				formulalog.info('  thisworkflowsMap ='+ thisworkflowsMap)

				if (thisworkflowsMap.size() == 0) {
					formulalog.info(' thiselement has no flows ='+ thiselement)
				} else
				if (thisworkflowsMap.size() == 1) {
					thisworkflow=thisworkflowsMap.get( thisworkflowsMap.firstKey() )
					formulalog.info(' thiselement has one flow ='+ thisworkflow)
				} else {
					if (thisworkflowmyargs.equals('')) {
						formulalog.info(' no workflow was passed when called ')

					} else {
						//formulalog.debug(' thisworkflowsMap ='+ thisworkflowsMap)
						formulalog.info(' this workflow was passed in ' + thisworkflowmyargs)
						try {
							thiselementtarget = formulaRootfindElement( thisworkflowmyargs );
							goodflow=verifyWorkflow(thisworkflowsMap, thisworkflowmyargs)
							if (goodflow) {
								thisworkflow=thisworkflowmyargs
								formulalog.info(' this workflow is VALID and wil be used ' + thisworkflowmyargs)
							}
						} catch( Exception ) {
							formulalog.warn( "Exception StartWorkFlow thisworkflowmyargs : " + Exception );
							goodflow=false
						}
					}
				}



			} catch( Exception ) {
				formulalog.warn( "Exception StartWorkFlow determine workflow name: " + Exception );
				goodflow=false
			}
		}

	}


	if (thisworkflow.equals('')) {
		formulalog.warn( "thisworkflow 2 StartWorkFlow thisworkflowmyargs is empty");

		formulalog.debug( "thisworkflow 2 myargs[1]=" + myargs[1]);

		if (myargs[1].equals('Auto workflow start')) {

			formulalog.info( "thisworkflow 2 skipping default for autostart ");

			goodflow=false

		} else {
			//	goodflow=false
			formulalog.info( "thisworkflow 2 try default");

			try {
				thiselementtarget = formulaRootfindElement( defaultWorkFlowthisworkflow );
				goodflow=verifyWorkflow(thisworkflowsMap, defaultWorkFlowthisworkflow)
				if (goodflow) {
					thisworkflow=defaultWorkFlowthisworkflow
				} else {
					formulalog.warn( "StartWorkFlow defaultWorkFlowthisworkflow not good... default=" + defaultWorkFlowthisworkflow );
					formulalog.warn( "StartWorkFlow defaultWorkFlowthisworkflow not good... map=" + thisworkflowsMap );
				}
			} catch( Exception ) {
				formulalog.warn( "Exception StartWorkFlow defaultWorkFlowthisworkflow: " + Exception );
				goodflow=false
			}
		}
	}

/*
	if (thisworkflow.equals('')) {
		thisworkflow=defaultWorkFlowthisworkflow
		formulalog.info( "thisworkflow is being set to default " + thisworkflow);
	}
*/

	if (goodflow) {
		rwfht.put('WorkFlowDname', "'" + thisworkflow + "'")

		rwfht.put('Reason', "'" + myargs[1].replace("'", '"') + "'")
		rwfht.put('Reason2', "'" + myargs[2].replace("'", '"') + "'")
		rwfht.put('ItemToBeStartedItemDname', "'" + arguments[1] + "'") // ends up in 'ItemToBeStarted'

		if (this.user) {
			formulalog.info(myargs.length+ ' <----> ' + arguments.length + " " + user.name);
			rwfht.put('user', "'" + user.name + "'")
		} else
		if (arguments.length>3) {
			formulalog.info(myargs.length+ ' <----> ' + arguments.length + " using " + arguments[3]);
			rwfht.put('user', "'" + arguments[3] + "'")
		} else {
			formulalog.info(myargs.length+ ' <----> ' + arguments.length + " using systemInternal");
			rwfht.put('user', "'systemInternal'")
		}


		rwfht.put('morefields', "")

		formulalog.info("goodflow   myargs.length=" + myargs.length+ ' <----> arguments.length=' + arguments.length + " ");


		tMMformat=new Packages.com.proserve.MappedMessageFormat(commandToSend)
		commandToSend=tMMformat.format(rwfht)

		sendcommand(state[ourproperties].workflowadapterhost, state[ourproperties].workflowadapterport, commandToSend )

	} else {
		formulalog.warn( "NOT Starting Workflow  -- invalid StartWorkFlow not a good workflow for element ");
		logArgs('myargs', myargs)
		logArgs('arguments', arguments)
	}

	formulalog.info("ends ");


	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return pueretval
}







/////////////////////////////////////////////////////////////////////////
////////// script adapter test tag function - functions act on and may update an alarm - but do NOT call alarmupdate ...
/////////////////////////////////////////////////////////////////////////



function ProcessStartWorkFlowEvent(thisEvent) {
	pueretval=true

	thisfunctionname="ProcessStartWorkFlowEvent";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	// these are the available alarm fields
	formulalog.info("starts" + thisEvent);

	for (ap in thisEvent.properties) {
		formulalog.debug('  ' + ap +'='+ thisEvent[ap]);
	}

	WorkFlowStepLevel=1
	if (thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==undefined || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==null || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=='') {
		formulalog.warn(' undefined thisEvent[state[ourproperties].workflowEventControlField_StepLevel] from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
	} else {
		try {
			WorkFlowStepLevel=Math.round(thisEvent[state[ourproperties].workflowEventControlField_StepLevel])
		} catch (Exception) {
			formulalog.warn(' problem with thisEvent[state[ourproperties].workflowEventControlField_StepLevel] ='+ thisEvent[state[ourproperties].workflowEventControlField_StepLevel] + ' ' + Exception + '  from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
		}
	}
	thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=WorkFlowStepLevel

	psweCurrentWorkFlowStepstr=state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel

	currentWorkFlowStep=getEventfield(thisEvent, psweCurrentWorkFlowStepstr+'N', true);

	// we are starting a new workflow
	pueretval=ProcessWorkFlowEvent(state[ourproperties].workflowEventVerb_AcceptWorkFlow, thisEvent, 'ProcessStartWorkFlowEvent', WorkFlowStepLevel)

	formulalog.info("ends " + thisEvent);

	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return pueretval

}


// only called by ProcessWorkFlowEvent
function ProcessStartWorkFlowEvent_CreateOrgs(thisEvent, newTOOOdname, thisEventInAdapter) {

	pueretval=true

	thisfunctionname="ProcessStartWorkFlowEventCreateOrgs"
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )

	formulalog.info("starts " + thisEvent);

	retval=CreateOrgsR(thisEvent, newTOOOdname, thisEventInAdapter)

	formulalog.info("ends " + thisEvent);

	formulalog = formulalog4j.getLogger( thatfunctionname )


	return retval


}

// only called by ProcessStartWorkFlowEvent_CreateOrgs
function CreateOrgsR(thisEvent, ItemStarted, thisEventInAdapter) {

	pueretval=true

	// passed an alarm which has an element name (somewhere in the system (alarm.ItemToBeStarted)) and the target place to move the element (ItemStarted)
	//  also passed an element in the adapter which the alarm is a generated item
	//  add the element name from the scriptadapter (under Workflow/workflow ID) to the element
	// to the target of the move

	thisfunctionname="CreateOrgsR";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	formulalog.info( "  thisEvent=" + thisEvent );
	formulalog.info( "  ItemStarted=" + ItemStarted );
	formulalog.info( "  thisEventInAdapter=" + thisEventInAdapter );

	for (ap in thisEvent.properties) {
		formulalog.info('   ' + ap +'='+ thisEvent[ap]);
	}

	if (thisEvent!=null) {
		// these are the available alarm fields
		formulalog.debug(thisEvent);
		for (ap in thisEvent.properties) {
			formulalog.debug('  ' + ap +'='+ thisEvent[ap]);
		}

		try {
//			newTOOOdname=thisEvent['ItemStarted']
			newTOOOdname=ItemStarted

			WorkFlowStepLevel=0
			if (thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==undefined || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==null || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=='') {
				formulalog.warn(' undefined . thisEvent[state[ourproperties].workflowEventControlField_StepLevel] from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
			} else {
				try {
					WorkFlowStepLevel=Math.round(thisEvent[state[ourproperties].workflowEventControlField_StepLevel])
				} catch (Exception) {
					formulalog.warn(' problem . with thisEvent[state[ourproperties].workflowEventControlField_StepLevel] ='+ thisEvent[state[ourproperties].workflowEventControlField_StepLevel] + ' ' + Exception + '  from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
				}
			}

			foundthetargetofthecopy=false;
			try {
				newTOOOdnametarget = formulaRootfindElement( newTOOOdname )
				foundthetargetofthecopy=true;
				formulalog.error( "   foundthetargetofthecopy: this means the next step has the same named obect" );
				formulalog.error( "-  foundthetargetofthecopy: this means the next step has the same named obect" );
				formulalog.error( "-- foundthetargetofthecopy: this means the next step has the same named obect" );

				thisEvent['status']="ACK";
				thisEvent['severity']=formula.severities.CRITICAL;

			} catch( Exception ) {
				formulalog.info( " Yeah, the element was not found and the copy can proceed Exception newTOOOdname 1: " + Exception);
			}

			if (foundthetargetofthecopy) {

			} else {

				fromDname=thisEvent['ItemToBeStarted']
				formulalog.info( " fromDname = " + fromDname);
				try {
					fromDnametarget = formulaRootfindElement( fromDname )

					formulalog.debug( " eval for removal of workflow adapter element " + fromDnametarget);

					thiselementtoremove=thisEvent['element']
					if (thiselementtoremove==null && thisEvent['status'].equals("CLOSED")) {

						formulalog.debug( " remove this related workflow thiselementtoremove=" + thiselementtoremove);

						if (thiselementtoremove==null || thiselementtoremove==undefined || thiselementtoremove=='') {
							formulalog.debug( " nothing to remove ???? == thiselementtoremove==null || thiselementtoremove==undefined || thiselementtoremove=='' ");
						} else {
							try {
								newChildrenList=removeElementFromChildren(fromDnametarget, thiselementtoremove)
								formulalog.debug( " setting fromDnametarget['Children']=" + newChildrenList);
								newChildrenListarray=newChildrenList.split(",")
								formulalog.info( " setting newChildrenListarray.length=" + newChildrenListarray.length);
								//fromDnametarget['Children']=newChildrenList
								fromDnametarget['Children']=newChildrenListarray
								thisEventInAdapter=''; //
							} catch( Exception ) {
								formulalog.warn( " Exception accessing fromDname to remove from children: " + Exception );
							}
						}
					}
				} catch( Exception ) {
					formulalog.warn( " Exception accessing fromDname to remove from children: " + Exception );
				}
			}

			if (foundthetargetofthecopy) {

			} else {


				toDname=ItemStarted

				formulalog.info( "  newTOOOdname=" + newTOOOdname);
				formulalog.info( "  fromDname=" + fromDname);
				formulalog.info( "  toDname=" + toDname);


				toDname=newTOOOdname.substring(newTOOOdname.indexOf('/')+1)

				formulalog.info("  copy from " + fromDname);
				formulalog.info("  copy to " + toDname);
				copyElement( fromDname, toDname, '', '' )
				//moveElement( fromDname, toDname, '', '' )

				try {
					newTOOOdnametarget = formulaRootfindElement( newTOOOdname )

					formulalog.info("  found element --- the copy was successful --- newTOOOdname=" + newTOOOdname);

					formulalog.info("  thisEvent['action']=" + thisEvent['action']);

					cmProcessWorkFlowScript(thisEvent);

					if (thisEvent['action'].equals(state[ourproperties].workflowEventVerb_Reject) && thisEvent['status'].equals("CLOSED")) {
						formulalog.info("   this was a closed alarm - this workflow is rejected - ")
						formulalog.info("   item was moved to the previous workflow item ")
					} else {

						if (thisEvent['status'].equals("CLOSED")) {

							formulalog.info("   this was a closed alarm - this workflow is completed")


							formulalog.info("   update closed alarm to reflect new location")
							thisEvent['PreviousItemToBeStarted']=thisEvent['ItemToBeStarted']
							thisEvent['ItemToBeStarted']=newTOOOdname

							processasautostart=true;
							autostartSingleWorkflow=false;
							if (true) {
								// code will attempt to invalidate the ability to continue based upon a single workflow
								// if only a single flow exists then the process will continue using the same alarm under a new flow
								autostartSingleWorkflow=true


								newworkflowtouse='Unknown Unset Workflow';
								// check for one workflow in the current elements related parent
								try {
									formulalog.info(' newTOOOdname being used to load workflows associated to its parent ='+ newTOOOdname)
									thiselementtarget = formulaRootfindElement( newTOOOdname );

									thisworkflowsMap=getWorkflowsFor(newTOOOdname)

									formulalog.info('  thisworkflowsMap for parent ='+ thisworkflowsMap)
								} catch( Exception ) {
									formulalog.warn( "Exception loading thisworkflowsMap for parent : " + Exception );
									autostartSingleWorkflow=false
								}

								if (autostartSingleWorkflow) {

									if (thisworkflowsMap.size() == 0) {
										autostartSingleWorkflow=false
										processasautostart=false;
									} else
									if (thisworkflowsMap.size() == 1) {
										newworkflowtouse=thisworkflowsMap.get( thisworkflowsMap.firstKey() )
										formulalog.info(' thiselement has one flow ='+ newworkflowtouse)
									} else {
										autostartSingleWorkflow=false
									}

								}

								if (autostartSingleWorkflow) {
									// if one WF, set the alarm to use the workflow
									thisEvent['WorkFlowToUse']=newworkflowtouse; // dname of the one valid flow in the element
									thisEvent['WorkFlowToUseName']=nameFromDname(newworkflowtouse); // dname of the one valid flow in the element
									thisWorkFlowToUse=newworkflowtouse;

									thisEvent['Reason']='System Auto Initiated ' + thisEvent['Reason']
								}

							}

							if (autostartSingleWorkflow) {
								processasautostart=false;
								thisEvent['status']="OPEN"; // reopen the event and allow the workflow to be processed
							}
							// attempt to call sartworkfow on the item
							if (processasautostart) {
								// erase the value for the Bound Workflow - either the element has one workflow or many

								try {
									formulalog.info(' newTOOOdname being used to access eleemnt to clear '+state[ourproperties].elementWorkflowProperty+ '  ='+ newTOOOdname)

									thiselementtarget = formulaRootfindElement( newTOOOdname );

									tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].elementWorkflowProperty)
									thiselementWorkflowProperty=tMMformat.format(state[ourproperties].asHashTable)

									thiselementtarget[thiselementWorkflowProperty]='';

									// should maybe set the workflow to a default value for the outer step or something
									// this will probably lead to an event in the org which is not started but waiting for someone to start it...

									// add a property which can be copied into this property?
									// the above code might be destroying a value set by a script...



								} catch (Exception) {
									formulalog.error(' problem clearing Bound Workflow ' + Exception);
								}

								// many will die a death while trying,
								// only one workflow and the one   should be used
								// the system defalut workflow could also kick in
								//   which may cause security problems .. now may have access to the alarm ...



								try {
									newargs=['', 'Auto workflow start', 'Auto workflow start']
									formulalog.info("  call StartWorkFlow using " + newargs)
									retval=StartWorkFlow(newargs, newTOOOdname)
									formulalog.info("  called StartWorkFlow using " + newargs)
								} catch( Exception ) {
									formulalog.warn( "  called StartWorkFlow Exception " + Exception );
								}

							}

						} else {

							// this was passed on the original message which started the workflow
							thisWorkFlowToUse=thisEvent['WorkFlowToUse']
							if (thisWorkFlowToUse!=null && thisWorkFlowToUse!=undefined && thisWorkFlowToUse!='' ) {
								thisEvent['WorkFlowToUseName']=nameFromDname(thisWorkFlowToUse); // dname of the one valid flow in the element
							}
							formulalog.info(" add child to " + newTOOOdname + "--     child=" + thisEventInAdapter)

							// add link workflow to the child
							var pedna=new Array();
							pedna[0]=thisEventInAdapter; // this is a linkto the alarm in an originating_event_id element
							newTOOOdnametarget.addChildren(pedna, true);

						}

						if (thisEvent['status'].equals("CLOSED")) {

							formulalog.info("   this was a closed alarm - this workflow has completed")


						} else {
							//formulalog.debug("  thisEvent['ItemStarted']=" + thisEvent['ItemStarted']);

							// populate the alarm with fields

							// add element to new item so alarms attach to the item

							// we now have an item which can be started!

							// we pass the dname of the object we have created because getting it from the alarm does not 'always' work
							WorkFlowStepLevel++
							thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=WorkFlowStepLevel

							thisEvent[state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted']=thisWorkFlowToUse // newTOOOdname

							if (true) {

								formulalog.info("  queue_AcceptRejectWorkFlow of " + newTOOOdname);
								queue_AcceptRejectWorkFlow(state[ourproperties].workflowEventVerb_Accept, thisEvent['Reason'], thisEvent['Reason2'], newTOOOdname, thisEvent, thisEvent['user'] )

							} else {
								if (ProcessWorkFlowEvent(state[ourproperties].workflowEventVerb_Accept, thisEvent, newTOOOdname, WorkFlowStepLevel)) {
									formulalog.debug("  ProcessAcceptWorkFlowEvent success");
								} else {
									formulalog.warn("  ProcessAcceptWorkFlowEvent failed");
								}
							}


						}
					}
					if (fromDname.equals('')) {
					} else{
						// this is related to the copyElement and make it effectivly a move
						formulalog.info("  queue_deleteElement of " + fromDname);
						queue_deleteElement(fromDname)
					}

				} catch( Exception ) {
					formulalog.warn( " Exception newTOOOdname 1: " + Exception );
				}

			}

		} catch( Exception ) {
			formulalog.warn( " Exception copyElement kids 1: " + Exception );
		}
	}

	formulalog.info( " pueretval=" + pueretval );

	formulalog = formulalog4j.getLogger( thatfunctionname )

	// allow the Event to be removed from the adapter by returning false
	return pueretval
}




function ProcessWorkFlowEvent(typeevent, thisEvent, thisItemStarted, WorkFlowStepLevel) {
	// the alarm/event
	// the dname of the item which was started - same as alarm field 'ItemStarted'

	pueretval=true

	thisfunctionname="ProcessWorkFlowEvent_" + typeevent;
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	formulalog.info( "Starts" );

	// these are the available alarm fields
	formulalog.info("thisEvent=" + thisEvent);
	formulalog.info(" thisEvent['originating_event_id']=" + thisEvent['originating_event_id'] + ".");
	formulalog.info(" thisEvent['parent_originating_event_id']=" + thisEvent['parent_originating_event_id'] + ".");

	formulalog.info("thisItemStarted is the workflow controling how the item moves <---------" );
	formulalog.info("thisItemStarted=" + thisItemStarted + ".");

	formulalog.info("WorkFlowStepLevel=" + WorkFlowStepLevel);
	for (ap in thisEvent.properties) {
//		formulalog.info('   ' + ap +'='+ thisEvent[ap]);
	}


	formulalog.info("thisEvent['ItemToBeStarted'] is the org under workflow <---------" );
	formulalog.info("thisEvent['ItemToBeStarted']=" + thisEvent['ItemToBeStarted'] + ".");


	if (WorkFlowStepLevel=='' || WorkFlowStepLevel==undefined || WorkFlowStepLevel==null) {
		formulalog.error(' on entry to ProcessWorkFlowEvent WorkFlowStepLevel is undefined? from ' +WorkFlowStepLevel + ' trying to get the value from the alarm' );
		WorkFlowStepLevel=1

		if (thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==undefined || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==null || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=='') {
			formulalog.warn(' undefined .. thisEvent[state[ourproperties].workflowEventControlField_StepLevel] from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
		} else {
			try {
				WorkFlowStepLevel=Math.round(thisEvent[state[ourproperties].workflowEventControlField_StepLevel])
			} catch (Exception) {
				formulalog.warn(' problem .. with thisEvent[state[ourproperties].workflowEventControlField_StepLevel] ='+ thisEvent[state[ourproperties].workflowEventControlField_StepLevel] + ' ' + Exception + '  from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
			}
		}
		formulalog.warn(' on entry to ProcessWorkFlowEvent now using ' +WorkFlowStepLevel);

	}
	thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=WorkFlowStepLevel


	CurrentWorkFlowStepstr=state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel

	// this should probably be passed in as the
	currentWorkFlowStep=getEventfield(thisEvent, CurrentWorkFlowStepstr+'N', true);

	formulalog.info("currentWorkFlowStep=" + currentWorkFlowStep + ".");
	currentWorkFlowStepnameFromDname=currentWorkFlowStep;

	tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].CatalogWorkflow)
	CatalogWorkflowParent=tMMformat.format(state[ourproperties].asHashTable)

	originalthisItemStarted=thisItemStarted

	if (thisItemStarted==undefined || thisItemStarted==null || thisItemStarted=='') {
		formulalog.info("!!! thisItemStarted IS ??? empty !!! " + thisItemStarted)
		thisItemStarted=''
	} else
	if (thisItemStarted=='ProcessStartWorkFlowEvent') {
		formulalog.info("processing StartWorkFlow StartWorkFlow StartWorkFlow StartWorkFlow <===============")
		thisItemStarted=''
	} else {
		if (thisItemStarted.equals(CatalogWorkflowParent)) {
			formulalog.info("!!! thisItemStarted IS CatalogWorkflowParent " + thisItemStarted)
			thisItemStarted=''
		}
	}

	var itemIsMoving=false


	var finddelim="::"

	var wfpteTM=null

	if (thisItemStarted.equals('')) {

		formulalog.info("find the CatalogWorkflowParent =" + CatalogWorkflowParent)

		try {
			CatalogWorkflowParenttarget = formulaRootfindElement( CatalogWorkflowParent );
			try {
				thisItemStarted=CatalogWorkflowParent
				formulalog.info("thisItemStarted 111 =" + thisItemStarted);

				thisEvent[CurrentWorkFlowStepstr+'ItemStarted']=CatalogWorkflowParent
				formulalog.debug(CurrentWorkFlowStepstr+'ItemStarted'+"=" + CatalogWorkflowParent);

				// process properties use 'CatalogWorkflowParenttarget' to determine what to do ...


				finddelim='::'
				wfpteTM=getChildrenMap(CatalogWorkflowParenttarget, finddelim)
				//formulalog.info("  children for CatalogWorkflowParent=" + wfpteTM)

				testItemToBeStarted=thisEvent['ItemToBeStarted']
				myparent=getParentNameFromDname(testItemToBeStarted)
				formulalog.info("  testItemToBeStarted=" + testItemToBeStarted)
				formulalog.info("  testItemToBeStarted myparent=" + myparent)

				formulalog.info("               currentWorkFlowStep=" + currentWorkFlowStep + "<--")
				formulalog.info("  currentWorkFlowStepnameFromDname=" + currentWorkFlowStepnameFromDname + "<--")

				try {
					currentWorkFlowStepnumber=currentWorkFlowStepnameFromDname.substring(0,3);
					formulalog.info( ".currentWorkFlowStepnumber=" + currentWorkFlowStepnumber );
					if (isNaN(currentWorkFlowStepnumber)) {
						currentWorkFlowStepnumber="000";
						currentWorkFlowStep=currentWorkFlowStepnameFromDname;
					} else {
						//currentWorkFlowStepnumber=''+ Math.round( currentWorkFlowStepnumber )
						formulalog.info( "currentWorkFlowStepnumber 0 =" + currentWorkFlowStepnumber );

						try {
							currentWorkFlowStep=currentWorkFlowStepnameFromDname.substring(4);
						} catch( Exception ) {
							formulalog.warn( "Exception currentWorkFlowStep remove number: " + Exception );
						}
					}

				} catch( Exception ) {
					formulalog.warn( "Exception currentWorkFlowStepnumber 0 : " + Exception  + " .." + currentWorkFlowStepnameFromDname + "..");
					currentWorkFlowStepnumber="000";
					currentWorkFlowStep=currentWorkFlowStepnameFromDname;
				}


/// need to determine the starting plac in the entire tree when starting from an arbitrary item (sandbox or subscribing areas for instance...)



				if (currentWorkFlowStep.equals("")) {
					formulalog.warn("  currentWorkFlowStep is empty")
					if (!myparent.equals("")) {
						formulalog.warn("  setting currentWorkFlowStep to myparent ")
						currentWorkFlowStep=myparent
					}
				}

				formulalog.info(" " + typeevent +  " currentWorkFlowStep=" + currentWorkFlowStep + "<---")

				currentWorkFlowStepDN='';
				if (typeevent.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {
					currentWorkFlowStepDN=getNextStep(wfpteTM, finddelim, currentWorkFlowStep, myparent)
				} else {
					currentWorkFlowStepDN=getPreviousStep(wfpteTM, finddelim, currentWorkFlowStep, myparent)
				}

				if (currentWorkFlowStepDN.equals('')) {
					formulalog.info(" no previous or next step was found ");

					formulalog.info(" myparent may simply have no workflows. " + myparent);

				}

				nextWorkFlowStepDN=null;
				resolvedeventServiceTarget=null;
				thisServiceTarget=null;

				if (currentWorkFlowStepDN.equals('')) {
					eventItemToBeStarted=getEventfield(thisEvent, 'ItemToBeStarted', true);
					if (eventItemToBeStarted!='')  {
						try {
							formulalog.info('Service Target eventItemToBeStarted ='+ eventItemToBeStarted)
							eventItemToBeStartedtarget = formulaRootfindElement( eventItemToBeStarted );
							thisServiceTarget=eventItemToBeStartedtarget['Service Target']
							if (thisServiceTarget && thisServiceTarget!=null && thisServiceTarget!=undefined && thisServiceTarget!='')  {
								formulalog.info("eventItemToBeStarted['Service Target'] ="+ thisServiceTarget)

								thisMMfielddatast=getMMfieldData(eventItemToBeStartedtarget, '', 'Service Target')
								formulalog.info('Service Target thisMMfielddatast ='+ thisMMfielddatast)
								if (thisMMfielddatast!=null) {
									mmfdgetFieldFormat=thisMMfielddatast.getFieldFormat();
									formulalog.info('Service Target thisMMfielddatast.getDescription===='+ thisMMfielddatast.getDescription());
									if (true) {
										formulalog.info("   thisMMfielddatast.getPropertyName()=" + thisMMfielddatast.getPropertyName() );
										formulalog.info("   thisMMfielddatast.getDisplayLabel()=" + thisMMfielddatast.getDisplayLabel() );
										formulalog.info("   thisMMfielddatast.getTooltip()=" + thisMMfielddatast.getTooltip() );
										formulalog.info("   thisMMfielddatast.getRangeName()=" + thisMMfielddatast.getRangeName() );
										formulalog.info("   thisMMfielddatast.getInputMask()=" + thisMMfielddatast.getInputMask() );
										formulalog.info("   thisMMfielddatast.getFieldFormat()=" + thisMMfielddatast.getFieldFormat() );
										formulalog.info("   thisMMfielddatast.getDefaultValue()=" + thisMMfielddatast.getDefaultValue() );
										formulalog.info("   thisMMfielddatast.getDefaultValue().getClass()=" + thisMMfielddatast.getDefaultValue().getClass() );
									}

									if (mmfdgetFieldFormat!=null) {
										formulalog.info('Service Target mmfdgetFieldFormat ='+ mmfdgetFieldFormat);

										formulalog.info('Service Target thisMMfielddatast.getDescription===='+ thisMMfielddatast.getDescription());
										if (thisMMfielddatast.getDescription().indexOf("_")==0 && thisMMfielddatast.getDescription().indexOf("setdefault")!=-1) {
											resolvedeventServiceTarget=mmfdgetFieldFormat;
											formulalog.info('Service Target thisMMfielddatast.getDescription='+ thisMMfielddatast.getDescription());
										} else {
											formulalog.info('Service Target call findChildByName using '+ mmfdgetFieldFormat + " and  " + thisServiceTarget);
											resolvedeventServiceTarget=findChildByName(mmfdgetFieldFormat, thisServiceTarget);
										}
										formulalog.info('Service Target resolvedeventServiceTarget='+ resolvedeventServiceTarget);

									} else {
										formulalog.info('Service Target mmfdgetFieldFormat==null mmfdgetFieldFormat ='+ mmfdgetFieldFormat);
									}
								} else {

									try {
										resolvedeventServiceTarget=formulaRootfindElement( thisServiceTarget );
										formulalog.info('Service Target being set as resolvedeventServiceTarget='+ resolvedeventServiceTarget + ' from thisServiceTarget');
									} catch( Exception ) {
										formulalog.warn( "Exception cold not find thisServiceTarget 0 : " + Exception  + " .." + thisServiceTarget + "..");
									}

								}
								if (resolvedeventServiceTarget!=null){
									nextWorkFlowStepDN=resolvedeventServiceTarget;

									// we have a new dname which we got from the element 'Choice' value
									// find the logical next step for this item
									//  this is probably one step too far - as the copy may send us to the step after the one the element is pointing at
									formulalog.info('Service Target '+typeevent+' currentWorkFlowStep='+ currentWorkFlowStep);

									// since we are starting, only an accept will be performed
									// will get the previous step so the code executed lated will advance it to the same step that came from the element property


//											if (typeevent.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {
//												currentWorkFlowStepDN=getNextStep(wfpteTM, finddelim, '', resolvedeventServiceTarget)
//											} else {

									currentWorkFlowStepDN=getCurrentStep(wfpteTM, finddelim, '', nameFromDname(resolvedeventServiceTarget))

//											}
									formulalog.info('Service Target currentWorkFlowStepDN (should be a ##)='+ currentWorkFlowStepDN);

									// force this into a subflow
									//  WorkFlowStepLevel=thisEvent[state[ourproperties].workflowEventControlField_StepLevel]
//											WorkFlowStepLevel=2
//											thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=1
											// assign the lavel workflow
//											thisEvent[state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted']=thisEvent['WorkFlowToUse']

//											formulalog.info('thisEvent['+state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted'+']='+ thisEvent[state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted']);

								}
							}

						} catch( Exception ) {
							formulalog.error('Service Target general exception '+ Exception);
						}
					}
				}

				tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].CatalogWorkflowsParent)
				CatalogWorkflowsSearchParent=tMMformat.format(state[ourproperties].asHashTable)
				formulalog.debug("  CatalogWorkflowsSearchParent=" + CatalogWorkflowsSearchParent);

				try {
					CatalogWorkflowsSearchParenttarget = formulaRootfindElement( CatalogWorkflowsSearchParent );

					if (currentWorkFlowStepDN.equals('')) {

						// this is the beginning of the code wich should be deleted removed killed ?

						formulalog.info("*** ***MOVING ELEMENT ");
						formulalog.info("*** *** if we got here something is not right in the setup ");

						//  the move is going to fail as this code is incomplete


						newthisEventaction="Completed - No Step " + thisEvent['action']
						thisEvent['action']=newthisEventaction

						formulalog.debug(" newthisEventaction =" + newthisEventaction);

						// NNN should have been removed

						if (resolvedeventServiceTarget==null) {
							nextWorkFlowStepDN=findStepDname(CatalogWorkflowsSearchParenttarget, currentWorkFlowStep)
						}

						formulalog.info("  target org for the copy is nextWorkFlowStepDN=" + nextWorkFlowStepDN + "<==");

						fromDname=thisEvent['ItemToBeStarted']

						if (originalthisItemStarted=='ProcessStartWorkFlowEvent') {
							formulalog.info("processing MOVING StartWorkFlow StartWorkFlow StartWorkFlow StartWorkFlow <===============")


						} else {

							// should wack the workflow from the 'fromDname' element

							formulalog.info("** *** typeevent=" + typeevent + " workflowEventVerb_StartWorkFlow=" + state[ourproperties].workflowEventVerb_StartWorkFlow  + " workflowEventVerb_AcceptWorkFlow=" + state[ourproperties].workflowEventVerb_AcceptWorkFlow )
							if ( typeevent.equals(state[ourproperties].workflowEventVerb_AcceptWorkFlow) || typeevent.equals(state[ourproperties].workflowEventVerb_StartWorkFlow)) {
								formulalog.info(" thisEvent['status'] setting to 'ACK'")
								thisEvent['status']='ACK'
							} else {
								formulalog.debug(" thisEvent['status'] setting to 'CLOSED'")
								thisEvent['status']='CLOSED'
							}

							itemIsMoving=true
						}

						thisendfromDname=fromDname.substring(0, fromDname.indexOf('/')+1)
						formulalog.debug("thisendfromDname " + thisendfromDname);

						toDname=nextWorkFlowStepDN

						newTOOOdname=thisendfromDname+nextWorkFlowStepDN
						//toDname=newTOOOdname.substring(newTOOOdname.indexOf('/')+1)

						formulalog.info("move from " + fromDname);
						formulalog.info("move to " + toDname);

						if (thisServiceTarget && thisServiceTarget!=null && thisServiceTarget!=undefined && thisServiceTarget!='')  {
							retvalfrommove=copyCloneElement( fromDname, nameFromDname(fromDname), toDname, '', '' )
							if (retvalfrommove!=null) {
								// need to think about creating a link and leaving the original around
								//
								returnCode = deleteElement( fromDname )
							}
						} else {
							formulalog.info('Service Target eventItemToBeStarted ='+ thisServiceTarget)
							retvalfrommove=moveElement( fromDname, toDname, '', '' )

						}


						formulalog.info("retvalfrommove=" + retvalfrommove);

						formulalog.debug("moveElement done " + newTOOOdname);

						if (originalthisItemStarted=='ProcessStartWorkFlowEvent') {
							formulalog.info("processing MOVING StartWorkFlow StartWorkFlow StartWorkFlow StartWorkFlow <===============")

							// next levels item started...
							tWorkFlowStepLevel=WorkFlowStepLevel+1
							tCurrentWorkFlowStepstr=state[ourproperties].workflowEventControlFieldPrefix + tWorkFlowStepLevel

							///////////////////////
							// these lines turn the inbound alam into an active workflow

							thisEvent[tCurrentWorkFlowStepstr+'ItemStarted']=newTOOOdname

							tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].ItemsBeingApprovedRoot)
							thisEventInAdapter=tMMformat.format(state[ourproperties].asHashTable)


							NameL1='Workflow ID'
							ClassL1='WorkFlows'

							NameL2=thisEvent['originating_event_id']
							ClassL2='WorkFlow'

							thisEventInAdapter=encodeURL(ClassL2)+"="+encodeURL(NameL2)+"/"+encodeURL(ClassL1)+"="+encodeURL(NameL1)+"/"+thisEventInAdapter

							formulalog.debug("computed dn=" + thisEventInAdapter )

							// ensure the alarm is populated so the related item links 'correctly'
							thisEvent['NameL1']=NameL1
							thisEvent['ClassL1']=ClassL1

							thisEvent['NameL2']=NameL2
							thisEvent['ClassL2']=ClassL2

							// this tags the event with the target of the copy
							//	thisEvent['ItemToBeStarted']=newTOOOdname

							////////////////////////


							try {
								retvalfrommovetarget = formulaRootfindElement( newTOOOdname );
								// add link workflow to the child
								var pedna=new Array();
								pedna[0]=thisEventInAdapter; // this is a linkto the alarm in an originating_event_id element
								retvalfrommovetarget.addChildren(pedna, true);
							} catch( Exception ) {
								formulalog.error(" add link workflow to the child " + newTOOOdname + " " + thisEventInAdapter );
							}


							thisEvent['ItemToBeStarted']=newTOOOdname
							formulalog.debug(" updating thisEvent['ItemToBeStarted'] to " + newTOOOdname );

							thisEvent['action']="Completed " + thisEvent['action']

						}

						formulalog.info("*** End of ***MOVING ELEMENT ");
						// this is the end of the code wich should be deleted removed killed ?

					} else {

						formulalog.info("*** ***processing alarm");



						formulalog.debug("currentWorkFlowStep=" + currentWorkFlowStep);

						formulalog.info("THIS should BE a NumBER ### currentWorkFlowStepDN=" + currentWorkFlowStepDN);

						// currentWorkFlowStepDN needs to be a numbered name --- 031 Somestepname
						currentWorkFlowStepnameFromDname=nameFromDname(currentWorkFlowStepDN);
						currentWorkFlowStep=currentWorkFlowStepnameFromDname;

						formulalog.info("  next WorkFlowStep=" + currentWorkFlowStep + " " + currentWorkFlowStepDN);
						thisEvent[CurrentWorkFlowStepstr+'N']=currentWorkFlowStep
						formulalog.info("  thisEvent["+CurrentWorkFlowStepstr+"N]" + thisEvent[CurrentWorkFlowStepstr+'N']);


						try {
							currentWorkFlowStepnumber=''+currentWorkFlowStepnameFromDname.substring(0,3);
							formulalog.info( ".currentWorkFlowStepnumber=" + currentWorkFlowStepnumber );
							if (isNaN(currentWorkFlowStepnumber)) {
								formulalog.info("  isNaN(currentWorkFlowStepnumber)" );
								currentWorkFlowStepnumber="000";
								currentWorkFlowStep=currentWorkFlowStepnameFromDname;
							} else {
								//currentWorkFlowStepnumber=''+ Math.round( currentWorkFlowStepnumber )
								formulalog.info( "currentWorkFlowStepnumber 0 =" + currentWorkFlowStepnumber );

								try {
									currentWorkFlowStep=currentWorkFlowStepnameFromDname.substring(4);
								} catch( Exception ) {
									formulalog.warn( "Exception currentWorkFlowStep remove number: " + Exception );
								}
							}

						} catch( Exception ) {
							formulalog.warn( "Exception currentWorkFlowStepnumber 0 : " + Exception  + " .." + currentWorkFlowStepnameFromDname + "..");
							currentWorkFlowStepnumber="000";
							currentWorkFlowStep=currentWorkFlowStepnameFromDname;
						}
						formulalog.info("  using currentWorkFlowStepnumber=" + currentWorkFlowStepnumber + " currentWorkFlowStep=" + currentWorkFlowStep);
						formulalog.info("  using currentWorkFlowStepDN=" + currentWorkFlowStepDN);


						thisEvent[CurrentWorkFlowStepstr+'DN']=currentWorkFlowStepDN
						thisEvent[CurrentWorkFlowStepstr]=currentWorkFlowStep

						formulalog.info("  WorkFlowStep org name=" + currentWorkFlowStep);

						nextWorkFlowStepDN=findStepDname(CatalogWorkflowsSearchParenttarget, currentWorkFlowStep)

						formulalog.info("  nextWorkFlowStepDN=" + nextWorkFlowStepDN);

						fromDname=thisEvent['ItemToBeStarted']
						// this is the orginial item

						thisendfromDname=fromDname.substring(0, fromDname.indexOf('/')+1)
						//  this is the target of the copy and the location it will be copied to
						//  this is the target of the copy and the location it will be copied to
						//  this is the target of the copy and the location it will be copied to
						newTOOOdname=thisendfromDname+nextWorkFlowStepDN
						//  this is the target of the copy and the location it will be copied to
						//  this is the target of the copy and the location it will be copied to
						//  this is the target of the copy and the location it will be copied to


						// items set in an alarm may not be available during this run
						// if the item alarm slot exist, it is updated immediately

						// if it is a new field, it is not accessable during this MODL execution

						// so we must set local vars and use this (in addition to setting in the alarm for use in later events)



						// next levels item started...
						tWorkFlowStepLevel=WorkFlowStepLevel+1
						tCurrentWorkFlowStepstr=state[ourproperties].workflowEventControlFieldPrefix + tWorkFlowStepLevel

///////////////////////
// these lines turn the inbound alam into an active workflow

						//thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=tWorkFlowStepLevel
						//formulalog.info(" updated " +state[ourproperties].workflowEventControlField_StepLevel + " to " + tWorkFlowStepLevel )

						thisEvent[tCurrentWorkFlowStepstr+'ItemStarted']=newTOOOdname

						tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].ItemsBeingApprovedRoot)
						thisEventInAdapter=tMMformat.format(state[ourproperties].asHashTable)


						NameL1='Workflow ID'
						ClassL1='WorkFlows'

						NameL2=thisEvent['originating_event_id']
						ClassL2='WorkFlow'

						thisEventInAdapter=encodeURL(ClassL2)+"="+encodeURL(NameL2)+"/"+encodeURL(ClassL1)+"="+encodeURL(NameL1)+"/"+thisEventInAdapter

						formulalog.debug("computed dn=" + thisEventInAdapter )

						// ensure the alarm is populated so the related item links 'correctly'
						thisEvent['NameL1']=NameL1
						thisEvent['ClassL1']=ClassL1

						thisEvent['NameL2']=NameL2
						thisEvent['ClassL2']=ClassL2

			// this tags the event with the target of the copy
			//	thisEvent['ItemToBeStarted']=newTOOOdname

////////////////////////

						formulalog.info("*** ** typeevent=" + typeevent + " workflowEventVerb_StartWorkFlow=" + state[ourproperties].workflowEventVerb_StartWorkFlow  + " workflowEventVerb_AcceptWorkFlow=" + state[ourproperties].workflowEventVerb_AcceptWorkFlow )
						if ( typeevent.equals(state[ourproperties].workflowEventVerb_AcceptWorkFlow) || typeevent.equals(state[ourproperties].workflowEventVerb_StartWorkFlow)) {
						} else {
							formulalog.debug(" thisEvent['status'] setting to 'CLOSED'")
							thisEvent['status']='CLOSED'

							// we are closing this event - its workflow has completed and

						}

						itemIsMoving=true
						ProcessStartWorkFlowEvent_CreateOrgs(thisEvent, newTOOOdname, thisEventInAdapter)

						if (thisEvent['status'].equals("CLOSED")) {
							formulalog.info(" finishing closed item " );
						} else {

							formulalog.debug(" current thisEvent['ItemToBeStarted'] is " + thisEvent['ItemToBeStarted'] );
							thisEvent['ItemToBeStarted']=newTOOOdname
							formulalog.debug(" updating thisEvent['ItemToBeStarted'] to " + newTOOOdname );

						}

						thisEvent['action']="Completed " + thisEvent['action']

					}

				} catch( Exception ) {
					formulalog.warn( "Exception CatalogWorkflowsSearchParent kids 1: " + Exception );
					for (ap in thisEvent.properties) {
						formulalog.info('   :dump event ' + ap +'='+ thisEvent[ap]);
					}

					thisEvent['severity']=formula.severities.CRITICAL;
					thisEvent['action']="Completed Y00 " + thisEvent['action']

				}




			} catch( Exception ) {
				formulalog.warn( "Exception CatalogWorkflowParent kids 1: " + Exception );
///////////////
				for (ap in thisEvent.properties) {
					formulalog.info('   dump event ' + ap +'='+ thisEvent[ap]);
				}
				thisEvent['severity']=formula.severities.CRITICAL;
				thisEvent['action']="Completed X1a " + thisEvent['action']
			}

		} catch( Exception ) {
			formulalog.warn( "Exception CatalogWorkflowParent: " + Exception );
			for (ap in thisEvent.properties) {
				formulalog.info('   .dump event ' + ap +'='+ thisEvent[ap]);
			}

			thisEvent['severity']=formula.severities.CRITICAL;
			thisEvent['action']="Completed X2a " + thisEvent['action']

		}


	} else {
		formulalog.info( "thisItemStarted!='' " + thisItemStarted );
	}

	if (itemIsMoving) {
		formulalog.info("!!! itemIsMoving !!! ")
		// another event will be fire (out of ProcessStartWorkFlowEvent_CreateOrgs) which will perform the logic below
	} else
	if (thisItemStarted=='') {
		formulalog.info("!!! thisItemStarted IS empty !!! " + thisItemStarted)
	} else {
		formulalog.info("thisItemStarted  333 =" + thisItemStarted)

		try {

			thisItemStartedtarget = formulaRootfindElement( thisItemStarted );
			try {

				OneWorkFlowDNtarget=thisItemStartedtarget
				OneWorkFlowDN=thisItemStarted

				//thisEvent[CurrentWorkFlowStepstr+'ItemStarted']=OneWorkFlowDN

				thisEvent[CurrentWorkFlowStepstr+'ItemStarted']=OneWorkFlowDN
				formulalog.info("--CurrentWorkFlowStepstrItemStarted = " + CurrentWorkFlowStepstr+'ItemStarted'+" = " + OneWorkFlowDN);

				if (processworkflowscripts(thisEvent[CurrentWorkFlowStepstr+'N'], thisEvent[CurrentWorkFlowStepstr+'DN'], 'OnEntryToStep', thisItemStarted, thisEvent, typeevent)) {

					currentWorkFlowStepnumber=thisEvent[CurrentWorkFlowStepstr+'N'];
					formulalog.info( "currentWorkFlowStepnumber=" + currentWorkFlowStepnumber + " from CurrentWorkFlowStepstrN " + thisEvent[CurrentWorkFlowStepstr+'N'] );

					if (currentWorkFlowStepnumber.equals('000')) {

						formulalog.info( "SPECIAL 000 Processing " );

					} else
					if (currentWorkFlowStepnumber.equals('099')) {

						formulalog.info( "SPECIAL 099 Processing " );

					} else
					if (currentWorkFlowStepnumber.equals('999')) {

						formulalog.info( "SPECIAL 999 Processing " );

					}
				}

				outerflow=0; // 0 did not more --- 1 move up  ---  -1 moved back

				finddelim='::'
				var gcmwfpteTM=getChildrenMap(OneWorkFlowDNtarget, finddelim)
				if (gcmwfpteTM.size()==0) {

					formulalog.debug(" getChildrenMap EMPTY =" + gcmwfpteTM.size() + " for " + OneWorkFlowDN);
					thisEvent['action']="Completed - No Next Step " + thisEvent['action']

				} else {

					formulalog.debug(" getChildrenMap =" + gcmwfpteTM.size() + " for " + OneWorkFlowDN);
					// the current workflow step is a simple name '010 Some text'

					formulalog.info("---__________________________________^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^---");
					formulalog.info("currentWorkFlowStep=" + currentWorkFlowStep + "(" +thisEvent[CurrentWorkFlowStepstr+'DN']+ ")");

					inpara=isParallel(gcmwfpteTM, finddelim, thisEvent[CurrentWorkFlowStepstr+'DN']);
					formulalog.info("--- is current a parallel workflow--- ? " + inpara);

					if (inpara) {
						formulalog.info("--- need parent alarm to update reason, etc");
						// can send a msg to the parent in a queued item

						// children alarms should have a originating_event_id which starts with the parent and has 'child' 'sequencenumber and or stepname appended to it

					}

					processingaccept=false;
					foundotherchildevents=false;

					if (typeevent.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {

						processingaccept=true;

						formulalog.info("---accept  if " + inpara + " - if move 'parent' to next non para workflow ---  " + inpara);

						try {
							thisEventoriginating_event_id=new java.lang.String(thisEvent['originating_event_id']);
							thisEventparent_originating_event_id=new java.lang.String(thisEvent['parent_originating_event_id']);


							if (inpara && !(thisEventparent_originating_event_id=='undefined' || thisEventparent_originating_event_id==undefined || thisEventparent_originating_event_id==null || thisEventparent_originating_event_id=='') ) {
								formulalog.info("--- if parallel - close this event if many alarms---  " + inpara);

								try {
									mItemToBeStarted = formulaRootfindElement( thisEvent['ItemToBeStarted'] );
									amItemToBeStarted=mItemToBeStarted.alarms

									for(ai=0; ai<amItemToBeStarted.length; ai++) {
										if (amItemToBeStarted[ai]['originating_event_id'].equals(thisEventoriginating_event_id)) {
											formulalog.info('---a+ Found myself! in alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' ' + amItemToBeStarted[ai]['originating_event_id']);
											continue;
										}
										formulalog.info('---------- alarm.originating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '<==   alarm.parent_originating_event_id=' + amItemToBeStarted[ai]['parent_originating_event_id']);
										if (amItemToBeStarted[ai]['parent_originating_event_id']=='undefined' || amItemToBeStarted[ai]['parent_originating_event_id']==undefined || amItemToBeStarted[ai]['parent_originating_event_id']==null || amItemToBeStarted[ai]['parent_originating_event_id']=='') {

											amItemToBeStartedaioriginating_event_id=new java.lang.String(amItemToBeStarted[ai]['originating_event_id'] )
											//formulalog.info('---------- thisEventparent_originating_event_id=' + thisEventparent_originating_event_id + '<==   amItemToBeStartedaioriginating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '<----  tf=' + thisEventparent_originating_event_id.equals(amItemToBeStartedaioriginating_event_id));

											if (thisEventparent_originating_event_id.equals(amItemToBeStartedaioriginating_event_id)) {
												formulalog.info('---a+ this is my parent ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' ' + amItemToBeStartedaioriginating_event_id);

												formulalog.info('---a+ should update my parent with accept using this alarms reason' );

												mSimplemessagecommandToSend=state[ourproperties].SimplemessagecommandToSend
												if (mSimplemessagecommandToSend=='undefined' || mSimplemessagecommandToSend==null || mSimplemessagecommandToSend=='') {
													mSimplemessagecommandToSend="{commandToSendStart}originating_event_id={alarm.originating_event_id};severity='OK';action='Simple Update';Reason={Reason};Reason2={Reason2};user={user};msg={msg};{morefields}{commandToSendEnd}";
												} else {
												}


												commandToSend=mSimplemessagecommandToSend
												rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)
												rwfht.put('Reason', "'Parallel Step " + thisEvent['WorkFlowStepLevel2'] + ' '+ thisEvent['Reason'] + "'")
												rwfht.put('msg', "'Parallel Step " + thisEvent['WorkFlowStepLevel2'] + ' '+ thisEvent['Reason'] + "'")
												rwfht.put('Reason2', "'" + thisEvent['Reason2'] + "'")
												rwfht.put('user', "'" + thisEvent['user'] + "'")
												rwfht.put('morefields', "")
												rwfht.put('alarm.originating_event_id', "'" + amItemToBeStartedaioriginating_event_id + "'")
												sendAMessage(rwfht, commandToSend)

												formulalog.info('---a+ parent updated' );

											} else {
												formulalog.debug('---a+ skip close ?other? alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' originating_event_id='+  amItemToBeStartedaioriginating_event_id);
											}
										} else {
											formulalog.info('---a+ check     thisEventoriginating_event_id=' + thisEventoriginating_event_id +'    alarm.parent_originating_event_id=' +amItemToBeStarted[ai]['parent_originating_event_id'] + '    indexof=' + thisEventoriginating_event_id.indexOf(amItemToBeStarted[ai]['parent_originating_event_id']));
											if (thisEventoriginating_event_id.indexOf(amItemToBeStarted[ai]['parent_originating_event_id'])==0) {
												formulalog.info('---a+ close!! alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' originating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '     parent_originating_event_id=' + amItemToBeStarted[ai]['parent_originating_event_id']);
												// performing closes
												// post close of this event
												// calling change ina atest tag could be dangerous or just thread lock
												//amItemToBeStarted[ai]['status']='CLOSED';
												//amItemToBeStarted[ai].changeAlarm();
												formulalog.info('---a+ ' + amItemToBeStarted[ai]['status']);
												if (amItemToBeStarted[ai]['status'].equals('OPEN')) {
													foundotherchildevents=true;
												}
											} else {
												formulalog.debug('---a+ skip close?? (child of diff workflow for same object) alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' originating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '     element alarms for ' + thisEvent['ItemToBeStarted']);
											}
										}
									}
									if (foundotherchildevents) {
										formulalog.info("---a+ other paras exist for parent " + thisEvent['parent_originating_event_id']);
									} else {
										formulalog.info("---a+ should advance 'parent' out of parallel " + thisEvent['parent_originating_event_id']);



										commandToSend=state[ourproperties].AcceptWorkFlowcommandToSend
										rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)
										rwfht.put('Reason', "'Parallel Steps Completed " + thisEvent['WorkFlowStepLevel2'] + ' '+ thisEvent['Reason'] + "'")
										rwfht.put('Reason2', "'" + thisEvent['Reason2'] + "'")
										rwfht.put('dname', "'" + thisEvent['thiselement'] + "'")
										rwfht.put('user', "'" + thisEvent['user'] + "'")
										rwfht.put('morefields', "")
										rwfht.put('alarm.originating_event_id', "'" + thisEventparent_originating_event_id + "'")
										sendAMessage(rwfht, commandToSend)


									}
								} catch( Exception ) {
									formulalog.error("---a+ para alarms  1 " + Exception );
								}

								currentParaWorkFlowStepDN=getNextStepoverParallel(gcmwfpteTM, finddelim, currentWorkFlowStep, '')

								if (currentParaWorkFlowStepDN=='') {
									formulalog.info("currentParaWorkFlowStepDN is empty " + currentParaWorkFlowStepDN);
								}

								formulalog.info("next nonPara WF " + currentParaWorkFlowStepDN);

								currentWorkFlowStepDN=currentParaWorkFlowStepDN

								thisEvent['status']="CLOSED";

							} else {

								if (inpara && (thisEventparent_originating_event_id=='undefined' || thisEventparent_originating_event_id==undefined || thisEventparent_originating_event_id==null || thisEventparent_originating_event_id=='') ) {
									formulalog.info("--- parallel accept - acting on the parent event " + inpara);
									currentWorkFlowStepDN=getNextStepoverParallel(gcmwfpteTM, finddelim, currentWorkFlowStep, '')
								} else {

									currentWorkFlowStepDN=getNextStep(gcmwfpteTM, finddelim, currentWorkFlowStep, '')
								}

							}

						} catch( Exception ) {
							formulalog.error("---para alarms checkers accept " + Exception );

							currentWorkFlowStepDN=getNextStep(gcmwfpteTM, finddelim, currentWorkFlowStep, '')
						}



					} else {


						formulalog.info("--- if " + inpara + " - reject - if move 'parent' to prev non para workflow ---  " + inpara);

						try {
							thisEventoriginating_event_id=new java.lang.String(thisEvent['originating_event_id']);
							thisEventparent_originating_event_id=new java.lang.String(thisEvent['parent_originating_event_id']);


							if (inpara && !(thisEventparent_originating_event_id=='undefined' || thisEventparent_originating_event_id==undefined || thisEventparent_originating_event_id==null || thisEventparent_originating_event_id=='') ) {
								formulalog.info("--- if parallel - close parallel events---  " + inpara);


								try {
									mItemToBeStarted = formulaRootfindElement( thisEvent['ItemToBeStarted'] );
									amItemToBeStarted=mItemToBeStarted.alarms
									foundotherchildevents=false;


										for(ai=0; ai<amItemToBeStarted.length; ai++) {
											if (amItemToBeStarted[ai]['originating_event_id'].equals(thisEventoriginating_event_id)) {
												formulalog.info('---r+ Found myself! in alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' ' + amItemToBeStarted[ai]['originating_event_id']);
												continue;
											}
											formulalog.info('---------- alarm.originating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '<==   alarm.parent_originating_event_id=' + amItemToBeStarted[ai]['parent_originating_event_id']);
											if (amItemToBeStarted[ai]['parent_originating_event_id']=='undefined' || amItemToBeStarted[ai]['parent_originating_event_id']==undefined || amItemToBeStarted[ai]['parent_originating_event_id']==null || amItemToBeStarted[ai]['parent_originating_event_id']=='') {

												amItemToBeStartedaioriginating_event_id=new java.lang.String(amItemToBeStarted[ai]['originating_event_id'] )
												//formulalog.info('---------- thisEventparent_originating_event_id=' + thisEventparent_originating_event_id + '<==   amItemToBeStartedaioriginating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '<----  tf=' + thisEventparent_originating_event_id.equals(amItemToBeStartedaioriginating_event_id));

												if (thisEventparent_originating_event_id.equals(amItemToBeStartedaioriginating_event_id)) {
													formulalog.info('---r+ this is my parent ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' ' + amItemToBeStartedaioriginating_event_id);

													formulalog.info('---r+ should update my parent with rejection using this alarms reason' );

													commandToSend=state[ourproperties].RejectWorkFlowcommandToSend
													rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)
													rwfht.put('Reason', "'Parallel Step Update " + thisEvent['WorkFlowStepLevel2'] + ' '+ thisEvent['Reason'] + "'")
													rwfht.put('Reason2', "'" + thisEvent['Reason2'] + "'")
													rwfht.put('dname', "'" + thisEvent['thiselement'] + "'")
													rwfht.put('user', "'" + thisEvent['user'] + "'")
													rwfht.put('morefields', "")
													rwfht.put('alarm.originating_event_id', "'" + amItemToBeStartedaioriginating_event_id + "'")
													sendAMessage(rwfht, commandToSend)

													formulalog.info('---r+ my parent rejectd' );

												} else {
													formulalog.debug('---r+ skip close ?other? alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' originating_event_id='+  amItemToBeStartedaioriginating_event_id);
												}
											} else {
												formulalog.info('---r+ check     thisEventoriginating_event_id=' + thisEventoriginating_event_id +'    alarm.parent_originating_event_id=' +amItemToBeStarted[ai]['parent_originating_event_id'] + '    indexof=' + thisEventoriginating_event_id.indexOf(amItemToBeStarted[ai]['parent_originating_event_id']));
												if (thisEventoriginating_event_id.indexOf(amItemToBeStarted[ai]['parent_originating_event_id'])==0) {
													formulalog.info('---r+ close!! alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' originating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '     parent_originating_event_id=' + amItemToBeStarted[ai]['parent_originating_event_id']);
													// performing closes
													// post close of this event
													// calling change ina atest tag could be dangerous or just thread lock
													//amItemToBeStarted[ai]['status']='CLOSED';
													//amItemToBeStarted[ai].changeAlarm();


													mSimplemessagecommandToSend=state[ourproperties].SimplemessagecommandToSend
													if (mSimplemessagecommandToSend=='undefined' || mSimplemessagecommandToSend==null || mSimplemessagecommandToSend=='') {
														mSimplemessagecommandToSend="{commandToSendStart}originating_event_id={alarm.originating_event_id};severity='MAJOR';action='Simple Update';Reason={Reason};Reason2={Reason2};user={user};msg={msg};{morefields}{commandToSendEnd}";
													} else {
													}


													commandToSend=mSimplemessagecommandToSend
													rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)
													rwfht.put('Reason', "'Parallel Step Reject " + thisEvent['WorkFlowStepLevel2'] + ' '+ thisEvent['Reason'] + "'")
													rwfht.put('msg', "'Parallel Step Reject " + thisEvent['WorkFlowStepLevel2'] + ' '+ thisEvent['Reason'] + "'")
													rwfht.put('Reason2', "'" + thisEvent['Reason2'] + "'")
													rwfht.put('user', "'" + thisEvent['user'] + "'")
													rwfht.put('morefields', "status='CLOSED';")
													rwfht.put('alarm.originating_event_id', "'" + amItemToBeStarted[ai]['originating_event_id'] + "'")
													sendAMessage(rwfht, commandToSend)


													foundotherchildevents=true;
												} else {
													formulalog.debug('---r+ skip close?? (child of diff workflow for same object) alarms ' + amItemToBeStarted[ai]['WorkFlowStepLevel2'] + ' originating_event_id=' + amItemToBeStarted[ai]['originating_event_id'] + '     element alarms for ' + thisEvent['ItemToBeStarted']);
												}
											}
										}

										// should
										formulalog.info("---r+ should reject 'parent' out of parallel  --- parent_originating_event_id=" + thisEvent['parent_originating_event_id']);
										if (foundotherchildevents) {
										} else {
											formulalog.info("---r+ should reject 'parent' out of parallel +++ parent_originating_event_id=" + thisEvent['parent_originating_event_id']);

										}

								} catch( Exception ) {
									formulalog.error("---r+ para alarms  1 " + Exception );
								}


								currentParaWorkFlowStepDN=getPreviousStepoverParallel(gcmwfpteTM, finddelim, currentWorkFlowStep, '')

								formulalog.info("prev nonPara WF " + currentParaWorkFlowStepDN);

								currentWorkFlowStepDN=currentParaWorkFlowStepDN


								// always close an accepted or rejected parallel item
								thisEvent['status']="CLOSED";


							} else {

								if (inpara && (thisEventparent_originating_event_id=='undefined' || thisEventparent_originating_event_id==undefined || thisEventparent_originating_event_id==null || thisEventparent_originating_event_id=='') ) {
									formulalog.info("--- parallel reject - acting on the parent event " + inpara);
									currentWorkFlowStepDN=getPreviousStepoverParallel(gcmwfpteTM, finddelim, currentWorkFlowStep, '')
								} else {

									currentWorkFlowStepDN=getPreviousStep(gcmwfpteTM, finddelim, currentWorkFlowStep, '')
								}

							}

						} catch( Exception ) {
							formulalog.error("---para alarms checkers " + Exception );
							currentWorkFlowStepDN=getPreviousStep(gcmwfpteTM, finddelim, currentWorkFlowStep, '')
						}



					}



					formulalog.info("---------------------------------------------------------------------------");
					formulalog.info("currentWorkFlowStepDN=" + currentWorkFlowStepDN);

					processActionsToParent=false;
					if (currentWorkFlowStepDN.equals('')) {
						formulalog.info("currentWorkFlowStepDN is empty - this should mean the current workflow is completed");
						processActionsToParent=true;
					}

					formulalog.info("inpara=" + inpara + "  processingaccept=" + processingaccept  + " foundotherchildevents=" + foundotherchildevents);
					if ( (inpara && processingaccept && foundotherchildevents)) {
						formulalog.info("override moving to next step");
						processActionsToParent=false;
					}

					if (processActionsToParent) {
						// advance to parent workflow next step

						formulalog.info("old WorkFlowStepLevel=" + WorkFlowStepLevel);

						if (typeevent.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {

							// up to parent to advance the parent forward by 1 item
							formulalog.info("  VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV   ");
							WorkFlowStepLevel--
							outerflow=1;

						} else {
							formulalog.info("  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^  ");
							WorkFlowStepLevel--
							outerflow=-1;
						}

						formulalog.info("new WorkFlowStepLevel=" + WorkFlowStepLevel);

						if (WorkFlowStepLevel<1) {
							formulalog.error("  stopping for ZERO? WorkFlowStep=" + WorkFlowStepLevel);

							thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=WorkFlowStepLevel
							formulalog.info(" updated--. " +state[ourproperties].workflowEventControlField_StepLevel + " to " + WorkFlowStepLevel )

							thisEvent['action']="Completed - Zero step " + thisEvent['action']
						} else {
							thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=WorkFlowStepLevel
							formulalog.info(" updated. " +state[ourproperties].workflowEventControlField_StepLevel + " to " + WorkFlowStepLevel )

							// remove the bound workflow at the current level
							formulalog.info("====---> set "+ CurrentWorkFlowStepstr + "=" + currentWorkFlowStepDN +"<==")

							thisEvent[CurrentWorkFlowStepstr+'N']=currentWorkFlowStepDN
							thisEvent[CurrentWorkFlowStepstr+'ItemStarted']=currentWorkFlowStepDN


							thisEvent[CurrentWorkFlowStepstr+'DN']=currentWorkFlowStepDN
							thisEvent[CurrentWorkFlowStepstr]=currentWorkFlowStepDN

							tCurrentWorkFlowStepstr=state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel
							thisItemStarted=getEventfield(thisEvent, tCurrentWorkFlowStepstr+'ItemStarted', false);

							thisEvent['WorkFlowToUseName']=nameFromDname(currentWorkFlowStepDN); // dname of the one valid flow in the element
							//thisEvent['WorkFlowToUseName']=nameFromDname(thisItemStarted); // dname of the one valid flow in the element


							queue_AcceptRejectWorkFlow(typeevent, thisEvent['Reason'], thisEvent['Reason2'], thisItemStarted, thisEvent, thisEvent['user']  )

							thisEvent['action']="Completed - Step Ends " + thisEvent['action']
						}

					} else {

						newispara=isParallel(gcmwfpteTM, finddelim, currentWorkFlowStepDN);

						formulalog.info("--- is Next/Previous a parallel workflow--- ? " + newispara );

						formulalog.info("--- if parallel - create parallel events---  " );

						if (newispara) {

							if (createParallelChildEvents(gcmwfpteTM, thisEvent, finddelim, currentWorkFlowStepDN)) {

								formulalog.info(" update 'parent' event" );

								// need notifications scripting, etc for generated children...

							}

						}

						formulalog.info(" from event " +state[ourproperties].workflowEventControlField_StepLevel + " ====" + thisEvent[state[ourproperties].workflowEventControlField_StepLevel] )

						//currentWorkFlowStep=nameFromDname(currentWorkFlowStepDN);
						currentWorkFlowStepnameFromDname=nameFromDname(currentWorkFlowStepDN);
						currentWorkFlowStep=currentWorkFlowStepnameFromDname;
						currentWorkFlowStepWithNumber=currentWorkFlowStepnameFromDname;

						thisEvent[CurrentWorkFlowStepstr+'DN']=currentWorkFlowStepDN

						formulalog.debug("  next WorkFlowStep=" + currentWorkFlowStep + " " + currentWorkFlowStepDN);

						// these are updates to the alarm and must occur via a send of a message


						try {
							currentWorkFlowStep=currentWorkFlowStepWithNumber.substring(4);
						} catch( Exception ) {
							formulalog.warn( "Exception currentWorkFlowStep remove number: " + Exception );
						}
						formulalog.debug("  WorkFlowStep org name=" + currentWorkFlowStep);



						try {
							currentWorkFlowStepnumber=currentWorkFlowStepnameFromDname.substring(0,3);
							formulalog.info( "currentWorkFlowStepnumber=" + currentWorkFlowStepnumber + " from " + currentWorkFlowStepnameFromDname );

							if (currentWorkFlowStepnumber.equals('000')) {
								formulalog.info( "SPECIAL 000 Processing   outerflow=" + outerflow );
							} else
							if (currentWorkFlowStepnumber.equals('099')) {
								formulalog.info( "SPECIAL 099 Processing   outerflow=" + outerflow );
							} else
							if (currentWorkFlowStepnumber.equals('999')) {
								formulalog.info( "SPECIAL 999 Processing   outerflow=" + outerflow );
							}
						} catch( Exception ) {
							formulalog.warn( "Exception currentWorkFlowStepnumber: " + Exception + " from " + currentWorkFlowStep );
						}


						if (newispara) {
							// Change the text so the event dissappears from the inbox -
							//

							// with the number
							thisEvent[CurrentWorkFlowStepstr+'N']=currentWorkFlowStepWithNumber + ' Parallel Parent'
							// without the number
							thisEvent[CurrentWorkFlowStepstr]=currentWorkFlowStep + ' Parallel Parent'

							formulalog.info( CurrentWorkFlowStepstr+'N=' +thisEvent[CurrentWorkFlowStepstr+'N'] );
							formulalog.info( CurrentWorkFlowStepstr+'= '+ thisEvent[CurrentWorkFlowStepstr]);

							thisEvent['action']="Completed - Parallel Steps Continues " + thisEvent['action']
						} else {

							// with the number
							thisEvent[CurrentWorkFlowStepstr+'N']=currentWorkFlowStepWithNumber
							// without the number
							thisEvent[CurrentWorkFlowStepstr]=currentWorkFlowStep
							thisEvent['action']="Completed - Step Continues " + thisEvent['action']
						}
					}
				}



				try {
					currentWorkFlowStepnumber="000";
					try {
						currentWorkFlowStepnumber=''+thisEvent[CurrentWorkFlowStepstr+'N'].substring(0,3);
						formulalog.info( "-->from .currentWorkFlowStepnumber=" + currentWorkFlowStepnumber );
						if (isNaN(currentWorkFlowStepnumber)) {
							currentWorkFlowStepnumber="000";
							currentWorkFlowStep=thisEvent[CurrentWorkFlowStepstr+'N'];
						} else {
							formulalog.info( "-->from currentWorkFlowStepnumber 0 =" + currentWorkFlowStepnumber );

							try {
								currentWorkFlowStep=thisEvent[CurrentWorkFlowStepstr+'N'].substring(4);
							} catch( Exception ) {
								formulalog.warn( "-->from Exception currentWorkFlowStep remove number: " + Exception );
							}
						}
					} catch( Exception ) {
						formulalog.debug( "-->from Exception currentWorkFlowStepnumber 0 : " + Exception  + " .." + thisEvent[CurrentWorkFlowStepstr+'N'] + "..");
						currentWorkFlowStepnumber="000";
						currentWorkFlowStep=thisEvent[CurrentWorkFlowStepstr+'N'];
					}

					formulalog.info( "before processworkflowscripts =" + currentWorkFlowStepnumber );
					if (processworkflowscripts(currentWorkFlowStepnumber, thisEvent[CurrentWorkFlowStepstr+'DN'], '', thisItemStarted, thisEvent, typeevent)) {
						if (currentWorkFlowStepnumber.equals('000')) {
							formulalog.info( "SPECIAL 000 before runanyscript Processing   outerflow=" + outerflow );
						} else
						if (currentWorkFlowStepnumber.equals('099')) {
							formulalog.info( "SPECIAL 099 before runanyscript Processing   outerflow=" + outerflow );
							// this should only be a final step
							if (typeevent.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {
								queue_AcceptRejectWorkFlow(state[ourproperties].workflowEventVerb_Accept, 'Special Step 099 ' + thisEvent['Reason'], thisEvent['Reason2'], thisItemStarted, thisEvent, thisEvent['user'] )
							}
						} else
						if (currentWorkFlowStepnumber.equals('999')) {
							formulalog.info( "SPECIAL 999 before runanyscript Processing   outerflow=" + outerflow );
							if (typeevent.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {
								queue_AcceptRejectWorkFlow(state[ourproperties].workflowEventVerb_Accept, 'Special Step 999 ' + thisEvent['Reason'], thisEvent['Reason2'], thisItemStarted, thisEvent, thisEvent['user']   )
							}
						}
					}

				} catch( Exception ) {
					formulalog.warn( "Exception currentWorkFlowStepnumber runanyscript: " + Exception );
				}


				try {
					thisEvent['msg']=thisEvent['action'] + ' ' + thisEvent['Reason']
				} catch (Exception) {
					formulalog.error( " around update event msg: " + Exception );
				}


			} catch( Exception ) {
				formulalog.error( "Exception CatalogWorkflowParent kids 1: " + Exception );
				for (ap in thisEvent.properties) {
					formulalog.info('   -dump event ' + ap +'='+ thisEvent[ap]);
				}

				thisEvent['action']="Failed X1 " + thisEvent['action']
				//thisEvent['severity']="CRITICAL"
				thisEvent['severity']=formula.severities.CRITICAL;
				thisEvent['msg']="Exception CatalogWorkflowParent kids 1: " + Exception
			}

		} catch( Exception ) {
			formulalog.error( "Exception CatalogWorkflowParent: " + Exception );
			for (ap in thisEvent.properties) {
				formulalog.info('   >dump event ' + ap +'='+ thisEvent[ap]);
			}

//			try {
//
//				thisItemStartedtarget = formulaRootfindElement( thisItemStarted );
//				formulalog.error( " oddly, this worked: " + thisItemStartedtarget );
//			} catch( Exception ) {
//				formulalog.error( "Exception CatalogWorkflowParent: " + Exception );
//				for (ap in this) {
//					formulalog.info('   >dump this ' + ap +'='+ this[ap]);
//				}
//			}

			thisEvent['action']="Failed X2 " + thisEvent['action']
//			thisEvent['severity']="CRITICAL"
			thisEvent['severity']=formula.severities.CRITICAL;
			thisEvent['msg']="Exception CatalogWorkflowParent: " + Exception
		}


	}


	formulalog.info( "Ends" );



	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return pueretval
}


function processworkflowscripts(thisEventCurrentWorkFlowStepstrN, thisdnameFromEvent, scripttype, thisItemStarted, thisEvent, typeevent) {
	// scripttype OnEntry or '' for the
	var retval=true;
	try {
		//thisEventCurrentWorkFlowStepstrN=thisEvent[CurrentWorkFlowStepstr+'N']

		if (thisEventCurrentWorkFlowStepstrN==null || thisEventCurrentWorkFlowStepstrN==undefined || thisEventCurrentWorkFlowStepstrN=='') {
			formulalog.info( "processworkflowscripts thisEventCurrentWorkFlowStepstrN is empty "  + thisEventCurrentWorkFlowStepstrN );
			formulalog.info( "processworkflowscripts thisdnameFromEvent="  + thisdnameFromEvent );
			retval=false;
		} else {
			currentWorkFlowStepnumber=thisEventCurrentWorkFlowStepstrN.substring(0,3);
			formulalog.info( "processworkflowscripts currentWorkFlowStepnumber before the step is advanced =" + currentWorkFlowStepnumber );

			//thisdnameFromEvent=thisEvent[CurrentWorkFlowStepstr+'DN'];
			thisdnameFromEventelement=null;
			thisdnameFromEventelementScript=null;
			thisdnameFromEventelementScriptAccept=null;
			thisdnameFromEventelementScriptReject=null;

			if (thisdnameFromEvent!=undefined && thisdnameFromEvent!=null && thisdnameFromEvent!='') {

				formulalog.info( 'processworkflowscripts before .'+scripttype+'. runanyscript=' + thisdnameFromEvent );
				try {
					thisdnameFromEventelement = formulaRootfindElement(thisdnameFromEvent);

					formulalog.info( 'processworkflowscripts '+scripttype+' thisdnameFromEventelement=' + thisdnameFromEventelement.name + ' ' + thisdnameFromEventelement.Condition);
				} catch( Exception ) {
					formulalog.warn( 'processworkflowscripts Exception '+scripttype+' thisdnameFromEventelement: ' + Exception );
				}


			} else {
				formulalog.info( 'thisdnameFromEvent '+scripttype+' is undefined');
			}

			if (thisdnameFromEventelement!=undefined && thisdnameFromEventelement!=null  && thisdnameFromEventelement!='') {
				// get the element property

				try {

					thisdnameFromEventelementScript=thisdnameFromEventelement['WorkFlowScript'+scripttype]
					if (thisdnameFromEventelementScript!=undefined && thisdnameFromEventelementScript!=null  && thisdnameFromEventelementScript!='') {
						formulalog.info( 'processworkflowscripts WorkFlowStepScript'+scripttype+' thisdnameFromEventelementScript=' + thisdnameFromEventelementScript );

						queue_runAnyScript(thisItemStarted,  thisdnameFromEventelementScript, thisEvent, currentWorkFlowStepnumber, this  )
						//evalScript(thisItemStarted,  thisdnameFromEventelementScript, thisEvent, currentWorkFlowStepnumber, this   )

					} else {
						formulalog.debug( "processworkflowscripts WorkFlowScriptOnEntryToStep thisdnameFromEventelementScript=" + thisdnameFromEventelementScript );
					}

					formulalog.info( "WorkFlowScript typeevent=" + typeevent );
					if (typeevent.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {
						thisdnameFromEventelementScriptAccept=thisdnameFromEventelement['WorkFlowScript'+scripttype+'Accept']
						if (thisdnameFromEventelementScriptAccept!=undefined && thisdnameFromEventelementScriptAccept!=null  && thisdnameFromEventelementScriptAccept!='') {
							formulalog.info( 'processworkflowscripts WorkFlowScript'+scripttype+'Accept thisdnameFromEventelementScriptAccept=' + thisdnameFromEventelementScriptAccept );

							//queue_runAnyScript(thisItemStarted,  thisdnameFromEventelementScriptAccept, thisEvent, currentWorkFlowStepnumber, this   )
							evalScript(thisItemStarted,  thisdnameFromEventelementScriptAccept, thisEvent, currentWorkFlowStepnumber, this  )

						} else {
							formulalog.debug( 'processworkflowscripts WorkFlowScript'+scripttype+'Accept thisdnameFromEventelementScriptAccept=' + thisdnameFromEventelementScriptAccept );
						}
					} else {
						thisdnameFromEventelementScriptReject=thisdnameFromEventelement['WorkFlowScript'+scripttype+'Reject']
						if (thisdnameFromEventelementScriptReject!=undefined && thisdnameFromEventelementScriptReject!=null  && thisdnameFromEventelementScriptReject!='') {
							formulalog.info( 'processworkflowscripts WorkFlowScript'+scripttype+'Reject thisdnameFromEventelementScriptReject=' + thisdnameFromEventelementScriptReject );

							//queue_runAnyScript(thisItemStarted,  thisdnameFromEventelementScriptReject, thisEvent, currentWorkFlowStepnumber, this   )
							evalScript(thisItemStarted,  thisdnameFromEventelementScriptReject, thisEvent, currentWorkFlowStepnumber, this   )

						} else {
							formulalog.debug( 'processworkflowscripts WorkFlowScript'+scripttype+'Reject thisdnameFromEventelementScriptReject=' + thisdnameFromEventelementScriptReject );
						}
					}


				} catch( Exception ) {
					formulalog.warn( "processworkflowscripts Exception during script gets and queues thisdnameFromEventelement: " + Exception );
				}


			} else {
				formulalog.warn( "processworkflowscripts thisdnameFromEventelement=" + thisdnameFromEventelement );
			}

		}
	} catch( Exception ) {
		formulalog.warn( "processworkflowscripts Exception currentWorkFlowStepnumber 1 : " + Exception );
		retval=false;
	}
	return retval;
}

function ProcessAcceptWorkFlowEvent(thisEvent, thisItemStarted, WorkFlowStepLevel) {
	// the alarm/event
	// the dname of the item which was started - same as alarm field 'ItemStarted'


	return ProcessWorkFlowEvent(state[ourproperties].workflowEventVerb_Accept, thisEvent, thisItemStarted, WorkFlowStepLevel)

}

function ProcessRejectWorkFlowEvent(thisEvent, thisItemStarted, WorkFlowStepLevel) {


	return ProcessWorkFlowEvent(state[ourproperties].workflowEventVerb_Reject, thisEvent, thisItemStarted, WorkFlowStepLevel)

}







function ProcessUpdateEvent(thisEvent) {
	pueretval=true

	thisfunctionname="ProcessUpdateEvent";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	formulalog.debug("starts " + thisEvent);

	myaction=getEventfield(thisEvent, 'action', false);

	formulalog.debug( "Starting thisEvent.action  " + myaction );

	for (ap in thisEvent.properties) {
		formulalog.debug('   dump the inbound alarm ' + ap +'='+ thisEvent[ap]);
	}


	WorkFlowStepLevel=1
	if (thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==undefined || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]==null || thisEvent[state[ourproperties].workflowEventControlField_StepLevel]=='') {
		formulalog.warn(' undefined ... thisEvent[state[ourproperties].workflowEventControlField_StepLevel] from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
	} else {
		try {
			WorkFlowStepLevel=Math.round(thisEvent[state[ourproperties].workflowEventControlField_StepLevel])
		} catch (Exception) {
			formulalog.warn(' problem ... with thisEvent[state[ourproperties].workflowEventControlField_StepLevel] ='+ thisEvent[state[ourproperties].workflowEventControlField_StepLevel] + ' ' + Exception + '  from '+ state[ourproperties].workflowEventControlField_StepLevel + ' using ' +WorkFlowStepLevel);
		}
	}

	if (myaction.equals('foobar')) {

	} else
	if (myaction.equals(state[ourproperties].workflowEventVerb_Reject)) {
		pueretval=ProcessWorkFlowEvent(myaction, thisEvent, thisEvent[state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted'], WorkFlowStepLevel)
	} else
	if (myaction.equals(state[ourproperties].workflowEventVerb_Accept)) {
		pueretval=ProcessWorkFlowEvent(myaction, thisEvent, thisEvent[state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted'], WorkFlowStepLevel)
	} else
	if (myaction.equals(state[ourproperties].workflowEventVerb_StartWorkFlow)) {
		pueretval=ProcessStartWorkFlowEvent(thisEvent)
	} else
	if (myaction.indexOf(state[ourproperties].workflowEventVerb_Accept)==0) {
		pueretval=ProcessWorkFlowEvent(myaction, thisEvent, thisEvent[state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted'], WorkFlowStepLevel)
	} else
	if (myaction.indexOf(state[ourproperties].workflowEventVerb_Reject)==0) {
		pueretval=ProcessWorkFlowEvent(myaction, thisEvent, thisEvent[state[ourproperties].workflowEventControlFieldPrefix + WorkFlowStepLevel + 'ItemStarted'], WorkFlowStepLevel)
	} else
	if (myaction.indexOf('Completed - Step Continues')==0) {
		formulalog.debug( "skipping thisEvent.action  " + myaction + " <== mostlikely a reloaded alarm" );
	} else
	if (myaction.indexOf('')==0) {
		formulalog.debug( "skipping thisEvent.action  no action to take" );
	} else
	if (myaction.indexOf('Simple Update')==0) {
		formulalog.debug( "skipping thisEvent.action  " + myaction + " <== Simple Update is to allow updates to history to occur without affecting the event " );
	} else
	if (myaction.indexOf('Already Processed')==0) {
		formulalog.debug( "skipping thisEvent.action  " + myaction + " <== Simple Update is to allow updates to history to occur without affecting the event " );
	} else {
		formulalog.warn( "unhandled thisEvent.action  " + myaction + " <== mostlikely a reloaded alarm" );
	}


	formulalog.debug( "Ending thisEvent.action  " + myaction );

	//thisEvent['action']='Already Processed'; // change the action to inhibit further processing?

	formulalog.debug("ends " + thisEvent);


	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return pueretval
}


function ProcessNewEvent(thisEvent) {
	pneretval=true

	thisfunctionname="ProcessNewEvent";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )



	formulalog.info("starts " + thisEvent);
	// perform any work for new events not previously seen in this run of the adapter

	pneretval=ProcessUpdateEvent(thisEvent)

	formulalog.info("ends " + thisEvent);


	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return pneretval
}



/////////////////////////////////////////////////////////////////////////
////////// entry points from test tag ...


function AdapterReloading(thisEvent) {

	aRlretval=true

	thisfunctionname="AdapterReloading";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	formulalog.info("starts " + thisEvent);

	oneurl=state[ourproperties].reloadAlarmsURL + " happy times state[ourproperties].reloadingAdapter=" + thisEvent

	tMMformat=new Packages.com.proserve.MappedMessageFormat(oneurl)
	oneurl=tMMformat.format(state[ourproperties].asHashTable)

	formulalog.info("ends " + thisEvent);

	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return aRlretval

}


function ProcessEvent(thisEvent) {
	thisfunctionname="ProcessEvent";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )

		// the first time an alarm is processed by model, the affected element is not known

	peretval=true;

	if (thisEvent.element) {
		peretval=ProcessUpdateEvent(thisEvent)
	} else {
		// the first time an alarm is processed by model, the affected element is not known
		// iff the test tag is at the very top of the MODL file - if its at the very bottom or
		//  this is an update event to a previously sent alarm, the element field 'should' be empty
		peretval=ProcessNewEvent(thisEvent)
	}

	formulalog.info("ends " + thisEvent);


	formulalog = formulalog4j.getLogger( thatfunctionname )


	// allow the Event to be removed from the adapter by returning false
	return peretval
}





/////////////////////////////////////////////////////////////////////////
////////// other utility functions - prototypes, junk, etc ...
/////////////////////////////////////////////////////////////////////////


function getWorkflowsFor( elementtocheck ) {


	// an item under workflow (at this time) exists under an org which has the same name as an item under the 'CatalogWorkflow'
	eleparent=getParentNameFromDname(elementtocheck)

	return getWorkflowsForthiselement( elementtocheck, eleparent );

}

function getWorkflowsForthiselement( elementtocheck, eleparent ) {

	validworkflows=new java.util.TreeMap()

	eleparentname=nameFromDname(eleparent)

	tMMformat=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].CatalogWorkflow)
	CatalogWorkflowParent=tMMformat.format(state[ourproperties].asHashTable)
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
		CatalogWorkflowParenttarget = formulaRootfindElement( CatalogWorkflowParent );

		formulalog.info( " getWorkflowsFor: ==>" + eleparentname + "<==  elementtocheck--->" + elementtocheck );

		formulalog.debug( " getWorkflowsFor getChildrenMap: ==>" + CatalogWorkflowParenttarget );

		cpmTM=getChildrenMap(CatalogWorkflowParenttarget, finddelim)

		cpmTMks=cpmTM.keySet()
		cpmTMkeys=cpmTMks.iterator()

		foundcurrent=false
		currentwaslast=false
		nextitem=''

		while (cpmTMkeys.hasNext() ) {
			currentwaslast=false

			cpmTMkeysitem=cpmTMkeys.next()

			// get the items dname
			testitem=cpmTM.get(cpmTMkeysitem)

			testitemname=nameFromDname(testitem)

			formulalog.debug( "   getWorkflowsFor testitem: " + testitem + " == " + testitemname);

			itestitemname=testitemname.indexOf(eleparentname)
			if (testitemname.equals(eleparentname) || itestitemname==0 || itestitemname==4) {
				// we found the outer group
				// get its kids
				try {
					formulalog.debug( " getWorkflowsFor child of cpmTM: " + testitem );
					testitemtarget = formulaRootfindElement( testitem );
					validworkflows=getChildrenMap(testitemtarget, finddelim)
					formulalog.debug( " getWorkflowsFor validworkflows: " + validworkflows );
				} catch( Exception ) {
					formulalog.warn( "Exception getWorkflowsFor: " + Exception );
				}
				break
			}


		}
	} catch( Exception ) {
		formulalog.warn( "Exception getWorkflowsFor::: " + Exception );
	}

	return validworkflows
}

function verifyWorkflow( cpmTM,  elementtocheck ) {
	isvalidworkflow=false


	formulalog.info( " verifyWorkflow elementtocheck=: " + elementtocheck );
	//formulalog.debug( " verifyWorkflow: " + cpmTM );

	cpmTMks=cpmTM.keySet()
	cpmTMkeys=cpmTMks.iterator()

	foundcurrent=false
	currentwaslast=false
	nextitem=''

	while (cpmTMkeys.hasNext() ) {
		currentwaslast=false

		cpmTMkeysitem=cpmTMkeys.next()

		// get the items dname
		testitem=cpmTM.get(cpmTMkeysitem)

		testitemname=nameFromDname(testitem)

		itestitemname=testitem.indexOf(elementtocheck)
		if (testitem.equals(elementtocheck) || itestitemname==0 || itestitemname==4) {
			isvalidworkflow=true
			formulalog.info( " verifyWorkflow: " + testitem );
			break
		} else {
			formulalog.info( "  !verifyWorkflow: " + testitem );
		}

	}


	return isvalidworkflow
}

function findStepDname(CatalogWorkflowsSearchParent, currentWorkFlowStep) {
	nextstep='ERROR'

	try {
		finddelim='::'
		formulalog.debug( " findStepDname currentWorkFlowStep=" + currentWorkFlowStep );

		var fsnwfpteTM=getChildrenMap(CatalogWorkflowsSearchParent, finddelim)

		fsnwfpteTMks=fsnwfpteTM.keySet()
		fsnwfpteTMkeys=fsnwfpteTMks.iterator()

		foundcurrent=false
		nextitem=''
		testcurrentWorkFlowStep=currentWorkFlowStep+finddelim

		formulalog.debug( " findStepDname fsnwfpteTMkeys.hasNext()" );
		while (fsnwfpteTMkeys.hasNext() ) {
			fsnwfpteTMkeysitem=fsnwfpteTMkeys.next()

			testitem=fsnwfpteTM.get(fsnwfpteTMkeysitem)
			if (nameFromDname(testitem).equals(currentWorkFlowStep)) {
				nextitem=testitem
				break
			}
		}
		formulalog.debug( " findStepDname nextitem=" + nextitem );

		nextstep=nextitem


	} catch( Exception ) {
		formulalog.warn( "Exception findStepDname kids 1: " + Exception );
	}

	return nextstep

}



function findChildByName(SearchParentdn, orgnameS) {
	nextstep=null


	try {
		orgname=new java.lang.String(orgnameS);

		finddelim='::'
		formulalog.debug( " findChildByName orgname=" + orgname );

		SearchParent = formulaRootfindElement( SearchParentdn );

		var fcbnwfpteTM=getChildrenMap(SearchParent, finddelim)

		formulalog.debug( " findChildByName fcbnwfpteTM=" + fcbnwfpteTM.size() );

		fcbnwfpteTMks=fcbnwfpteTM.keySet()
		fcbnwfpteTMkeys=fcbnwfpteTMks.iterator()

		foundcurrent=false
		testcurrentWorkFlowStep=orgname+finddelim

		while (fcbnwfpteTMkeys.hasNext() ) {
			fcbnwfpteTMkeysitem=fcbnwfpteTMkeys.next()

			testitem=fcbnwfpteTM.get(fcbnwfpteTMkeysitem)
			nfdn=nameFromDname(testitem)

			formulalog.debug( " findChildByName nfdn=" + nfdn + " orgname=" +  orgname );
			if (nfdn.equals(orgname) || nfdn==orgname) {
				nextstep=testitem
				break
			}
		}
		formulalog.debug( " findChildByName nextitem=" + nextstep );

	} catch( Exception ) {
		formulalog.warn( "Exception findChildByName kids 1: " + Exception );
	}

	return nextstep

}


function getChildrenMap(ParenttargetElement, finddelim) {

	var gcmfwfpteTM=new java.util.TreeMap()

	try {
		// loop through children
		// build a map with the name as the start of the key
		wfpte=ParenttargetElement.children

		formulalog.debug( " getChildrenMap " + ParenttargetElement.name + " has " + wfpte.length + " children");

		for (var iwfpte=0; iwfpte < wfpte.length; iwfpte++) {
			onechild=wfpte[iwfpte]
			gcmfwfpteTM.put(onechild.name+finddelim+onechild.dname, onechild.dname)
		}

		// merge in the 'related items'
		if (ParenttargetElement['Children'] && ParenttargetElement['Children']!=null &&
			ParenttargetElement['Children']!=undefined && ParenttargetElement['Children']!='') {

			var arr1=formula.util.breakOnTokens( ParenttargetElement['Children'], "," );

			for (i12=0; i12 < arr1.length; i12++) {
				newele=arr1[i12];

				gcmfwfpteTM.put(nameFromDname(newele)+finddelim+newele, newele)
			}
		}

		formulalog.debug( " getChildrenMap " + ParenttargetElement.name + " after merge has " + gcmfwfpteTM.size() + " children");

	} catch( Exception ) {
		formulalog.warn( "Exception getChildrenMap kids 1: " + Exception );
	}
	return gcmfwfpteTM

}



function removeElementFromChildrenFromMap(childrenlistmap, nametoremove) {
	newchildren=new java.lang.StringBuffer()

	childrenlistmapTMks=childrenlistmap.keySet()
	childrenlistmapTMkeys=childrenlistmapTMks.iterator()

	//formulalog.debug( " findStepDname childrenlistmap.hasNext()" );
	firsttime=true
	while (childrenlistmapTMkeys.hasNext() ) {
		childrenlistmapTMkeysitem=childrenlistmapTMkeys.next()

		testitem=childrenlistmapTM.get(childrenlistmapTMkeysitem)

		if (!testitem.equals(nametoremove)) {
			if (!firsttime) {
				newchildren.append(",")
			}
			firsttime=false

			newchildren.append(testitem)
		}

	}
	return (newchildren.toString()) // return a comma delimited string

}

function removeElementFromChildren(ParenttargetElement, nametoremove) {

	var newchildren=new java.lang.StringBuffer()

	var nametoremovestr=new java.lang.String(nametoremove)

	formulalog.debug( " removeElementFromChildren ParenttargetElement=" + ParenttargetElement);
	formulalog.debug( " removeElementFromChildren nametoremove=" + nametoremovestr+".");

	if (ParenttargetElement['Children'] && ParenttargetElement['Children']!=null &&
		ParenttargetElement['Children']!=undefined && ParenttargetElement['Children']!='') {

		var arr1=formula.util.breakOnTokens( ParenttargetElement['Children'], "," );

		firsttime=true
		for (i12=0; i12 < arr1.length; i12++) {
			newele=arr1[i12];

			if (newele.equals(nametoremovestr)) {
				formulalog.debug( " removeElementFromChildren  -skipping newele=" + newele);
			} else {
				if (!firsttime) {
					newchildren.append(",")
				}
				firsttime=false

				formulalog.debug( " removeElementFromChildren  +adding newele=" + newele);

				newchildren.append(newele)
			}
		}

		formulalog.debug( " removeElementFromChildren newchildren=" + newchildren.toString());

	}

	return (newchildren.toString()) // return a comma delimited string

}

function removeNAMdeleteORGElementChildren(ParenttargetElement, nametoremove) {

	retcode=false;

	try {
		isORGchild=false;

		thisparent=ParenttargetElement.dname;
		thischildsparent=getParentDname(nametoremove);

		formulalog.info( " removeNAMdeleteORGElementChildren       thisparent=" + thisparent);
		formulalog.info( " removeNAMdeleteORGElementChildren thischildsparent=" + thischildsparent);

		isORGchild=!thisparent.equals(thischildsparent);

		if (isORGchild) {

			formulalog.info( " removeNAMdeleteORGElementChildren remove ORG child ref");

			newlist=removeElementFromChildren(ParenttargetElement,nametoremove);
			nl = newlist.split(',');
			if( nl.length == 1 && nl[0] == '' )
				ParenttargetElement['Children'] = [];
			else
				ParenttargetElement['Children']=newlist.split(',');

			retcode=true;

		} else {


			try {

				nametoremoveele = formulaRootfindElement( nametoremove );
				formulalog.info( " removeNAMdeleteORGElementChildren destroy NAM child " + nametoremove);

				nametoremoveele.destroy();

			} catch( Exception ) {
				formulalog.warn( "Exception removeNAMdeleteORGElementChildren  delete NAM child " + Exception );
			}

			retcode=true;
		}


	} catch( Exception ) {
		formulalog.warn( "Exception removeNAMdeleteORGElementChildren :" + Exception );
	}

	return retcode;

}

function isParallel(ipTM, finddelim, currentWorkFlowStepDN) {
	retvalip=false;
	try {

		formulalog.info( " isParallel  ipTM size=" + ipTM.size() + " currentWorkFlowStepDN=" + currentWorkFlowStepDN );

		ipTMks=ipTM.keySet()
		ipTMkeys=ipTMks.iterator()

		foundcurrent=false
		currentwaslast=false
		nextitem=''
		testcurrentWorkFlowStep=nameFromDname(currentWorkFlowStepDN)+finddelim

		formulalog.info( " isParallel testcurrentWorkFlowStep=" + testcurrentWorkFlowStep );

		currentwaslast=false
		manycount=0;

		currentWorkFlowStepnumber=testcurrentWorkFlowStep.substring(0,3);
		while (ipTMkeys.hasNext() ) {

			ipTMkeysitem=ipTMkeys.next()

			testitem=ipTM.get(ipTMkeysitem)
			testitemname=nameFromDname(testitem)

			if (testitemname.indexOf(currentWorkFlowStepnumber)==0) {
				manycount++;
			}
		}
		retvalip=(manycount>1);
		formulalog.info( " isParallel " + retvalip + " " + manycount );

	} catch( Exception ) {
		formulalog.warn( "Exception isParallel kids 1: " + Exception );
	}

	return retvalip;

}


function createParallelChildEvents(ipTM, thisEvent, finddelim, currentWorkFlowStepDN) {
	retvalip=false;
	try {

		formulalog.info( "createParallelChildEvents  ipTM size=" + ipTM.size() + " currentWorkFlowStepDN=" + currentWorkFlowStepDN );

//		for (ap in thisEvent.properties) {
//			formulalog.info('createParallelChildEvents   ' + ap +'='+ thisEvent[ap]);
//		}



		ipTMks=ipTM.keySet()
		ipTMkeys=ipTMks.iterator()

		foundcurrent=false
		currentwaslast=false
		nextitem=''
		testcurrentWorkFlowStep=nameFromDname(currentWorkFlowStepDN)+finddelim

		formulalog.info( " createParallelChildEvents testcurrentWorkFlowStep=" + testcurrentWorkFlowStep );

		currentwaslast=false
		manycount=0;

		currentWorkFlowStepnumber=testcurrentWorkFlowStep.substring(0,3);
		while (ipTMkeys.hasNext() ) {

			ipTMkeysitem=ipTMkeys.next()

			testitem=ipTM.get(ipTMkeysitem)
			testitemname=nameFromDname(testitem)

			if (testitemname.indexOf(currentWorkFlowStepnumber)==0) {

				formulalog.info( " createParallelChildEvents create for step=" + testitemname );

				mNewParaWorkFlowcommandToSend=state[ourproperties].NewParaWorkFlowcommandToSend
				if (mNewParaWorkFlowcommandToSend=='undefined' || mNewParaWorkFlowcommandToSend==null || mNewParaWorkFlowcommandToSend=='') {
					mNewParaWorkFlowcommandToSend="{commandToSendStart}{morefields}{commandToSendEnd}"
				} else {
				}

/*
NewParaWorkFlowcommandToSend=
originating_event_id={alarm.originating_event_id};
severity='MAJOR';
action='{workflowEventVerb_Reject}';
thiselement={dname};Reason={Reason};Reason2={Reason2};{morefields}{commandToSendEnd}
#
#
#
StartWorkFlowcommandToSend=
{commandToSendStart}action='{workflowEventVerb_StartWorkFlow}';
ItemToBeStarted={ItemToBeStartedItemDname};WorkFlowToUse={WorkFlowDname};
severity='MINOR';Reason={Reason};Reason2={Reason2};
*/

				neworig=''
				for (ap in thisEvent.properties) {
					tf=thisEvent[ap];
					if (ap=='originating_event_id') {
						neworig=tf+'_child_'+testitemname
						break;
					}
				}

				tMMformat2=new Packages.com.proserve.MappedMessageFormat(state[ourproperties].ItemsBeingApprovedRoot)
				thisEventInAdapter=tMMformat2.format(state[ourproperties].asHashTable)
				NameL1=thisEvent['NameL1']
				ClassL1=thisEvent['ClassL1']
				NameL2=neworig
				ClassL2=thisEvent['ClassL2']
				thisEventInAdapter=encodeURL(ClassL2)+"="+encodeURL(NameL2)+"/"+encodeURL(ClassL1)+"="+encodeURL(NameL1)+"/"+thisEventInAdapter
				formulalog.info("createParallelChildEvents computed dn=" + thisEventInAdapter )



				commandToSend=mNewParaWorkFlowcommandToSend
				rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)
				copiednewfields='';
				pct6=0;

				copiednewfields+="parent_originating_event_id='"+ thisEvent['originating_event_id'] + "';";

				// these field might not be uin the alarm 'yet' so assignining during the loop will fail to get these VERy important values in the alarm
				copiednewfields+="WorkFlowStepLevel2='"+ testitemname.substring(4) + "';";
				copiednewfields+="WorkFlowStepLevel2N='"+ testitemname + "';";
				copiednewfields+="WorkFlowStepLevel2DN='"+ testitem + "';";
				copiednewfields+="thiselement='"+ thisEventInAdapter + "';";
				copiednewfields+="action='Parallel Accept';";
// what should the severity be?
//				copiednewfields+="severity='INFORMATIONAL';";
				copiednewfields+="msg='Start Parallel Item';";


				for (ap in thisEvent.properties) {
					tf=thisEvent[ap];
					if (ap=='date' || ap=='Date/Time' || ap=='lastUpdate'||
					    ap=='thiscatalog' || ap=='element') {
						continue;
					}
					if (ap=='originating_event_id' || ap=='NameL2') {
						tf=neworig;
					}
					if (ap=='WorkFlowStepLevel2') {
						continue;
					}
					if (ap=='WorkFlowStepLevel2N') {
						continue;
					}
					if (ap=='WorkFlowStepLevel2DN') {
						continue;
					}
					if (ap=='thiselement') {
						continue;
					}
					if (ap=='action') {
						continue;
					}
					if (ap=='msg') {
						continue;
					}
// what should the severity be?
//					if (ap=='severity' || ap=='Severity' ) {
//						continue;
//					}

					copiednewfields+=ap +"='"+ tf + "';";
					pct6++;

				}
				rwfht.put('morefields', copiednewfields)

				tMMformat=new Packages.com.proserve.MappedMessageFormat(commandToSend)
				commandToSend=tMMformat.format(rwfht)

//				formulalog.info( " createParallelChildEvents commandToSend=" + commandToSend );



				if (true) {

					//  add event to the parant
					sendcommand(state[ourproperties].workflowadapterhost, state[ourproperties].workflowadapterport, commandToSend )

					// ensure the alarm is populated so the related item links 'correctly'

					try {
						retvalfrommovetarget = formulaRootfindElement( thisEvent['ItemToBeStarted'] );
						// add link workflow to the child
						var pedna=new Array();
						pedna[0]=thisEventInAdapter; // this is a linkto the alarm in an originating_event_id element
						retvalfrommovetarget.addChildren(pedna, true);
					} catch( Exception ) {
						formulalog.error(" createParallelChildEvents  add link workflow to the child " + newTOOOdname + " " + thisEventInAdapter );
					}
				}

				manycount++;

			}
		}
		retvalip=(manycount>1);
		formulalog.info( "createParallelChildEvents " + retvalip + " " + manycount );

	} catch( Exception ) {
		formulalog.warn( "createParallelChildEvents Exception kids 1: " + Exception );
	}

	return retvalip;

}


function getNextStepoverParallel(gnsTM, finddelim, currentWorkFlowStep, parentItemname) {

	return getNextStepOrSameStep(gnsTM, finddelim, currentWorkFlowStep, parentItemname, true, true);

}


function getNextStep(gnsTM, finddelim, currentWorkFlowStep, parentItemname) {

	return getNextStepOrSameStep(gnsTM, finddelim, currentWorkFlowStep, parentItemname, true, false);
}

function getNextStepOrSameStep(gnsossTM, finddelim, currentWorkFlowStep, parentItemname, returnnextitem, parallelwf) {
	nextstep='ERROR'

	// loop through the sorted map and find the 'starting item' returning the 'next' item
	try {

		formulalog.debug( " getNextStepOrSameStep returnnextitem="+returnnextitem+" gnsossTM size=" + gnsossTM.size() + " parentItemname=" + parentItemname + " parallelwf=" + parallelwf);
		formulalog.debug( " getNextStepOrSameStep currentWorkFlowStep=" + currentWorkFlowStep );

		gnsossTMks=gnsossTM.keySet()
		gnsossTMkeys=gnsossTMks.iterator()

		foundcurrent=false

		foundcurrentpara=false
		manycount=0;

		currentwaslast=false
		nextitem=''
		testcurrentWorkFlowStep=currentWorkFlowStep+finddelim
		testcurrentWorkFlowStepParallelParent=currentWorkFlowStep+ ' Parallel Parent'

//		formulalog.info( " getNextStepOrSameStep testcurrentWorkFlowStep=" + testcurrentWorkFlowStep );

		currentWorkFlowStepnumber="";
		if (parallelwf) {
			try {
				currentWorkFlowStepnumber=testcurrentWorkFlowStep.substring(0,3);
			} catch( Exception ) {
				formulalog.warn( " getNextStepOrSameStepException getting currentWorkFlowStepnumber: " + Exception );
			}
		}

		while (gnsossTMkeys.hasNext() ) {
			currentwaslast=false

			gnsossTMkeysitem=gnsossTMkeys.next()
			if (returnnextitem) {
				if (testcurrentWorkFlowStep.equals(finddelim)) {
					// return first item
					formulalog.debug( " getNextStepOrSameStep    return first item" );
					foundcurrent=true
				}
			} else {
			}



			testitem=gnsossTM.get(gnsossTMkeysitem)
			testitemname=nameFromDname(testitem)
			if (testitemname.indexOf("000")==0) {
				// skip any 000 item even if they are the first item
				formulalog.debug( " getNextStepOrSameStep    skip 000 " + testitem);
				continue
			}

			formulalog.debug( " getNextStepOrSameStep    testitem=" + testitem);
			formulalog.debug( " getNextStepOrSameStep    testitemname=" + testitemname);

			if (parentItemname.equals('')) {
			} else {
				if (testitemname.indexOf(parentItemname)==4) {
					foundcurrent=true
					formulalog.info( " getNextStepOrSameStep   ==4 same Name " + testitem + " " + parentItemname + " ...testitemname=" + testitemname);
					if (returnnextitem) {
						formulalog.info( " getNextStepOrSameStep   ==4 skip ");
						continue
					} else {
						nextitem=testitem
					}
				}
				if (testitemname.indexOf(parentItemname)>3) {
					foundcurrent=true
					formulalog.info( " getNextStepOrSameStep   >3 same Name " + testitem + " " + parentItemname + " ...testitemname=" + testitemname);
					if (returnnextitem) {
						formulalog.info( " getNextStepOrSameStep   >3 skip ");
						continue
					} else {
						nextitem=testitem
					}
				}
				if (testitemname.equals(parentItemname)) {
					foundcurrent=true
					formulalog.info( " getNextStepOrSameStep   equals same Name " + testitem + " " + parentItemname + " ...testitemname=" + testitemname);
					if (returnnextitem) {
						formulalog.info( " getNextStepOrSameStep   equals skip ");
						continue
					} else {
						nextitem=testitem
					}
				}
			}

			if (foundcurrent) {
				formulalog.info( " getNextStepOrSameStep    foundcurrent returning " + testitem);
				// this function loops one extra time to gat the next item in the list
				if (returnnextitem) {
					nextitem=testitem
				}
				break
			} else {
				formulalog.debug( " getNextStepOrSameStep gnsossTMkeysitem=" + gnsossTMkeysitem );

				if (parallelwf) {
					thisWorkFlowStepnumber=""
					try {
						thisWorkFlowStepnumber=testitemname.substring(0,3);
					} catch( Exception ) {
						formulalog.warn( " getNextStepOrSameStepException parallelwf getting thisWorkFlowStepnumber: " + Exception );
					}

					if (testitemname.indexOf(currentWorkFlowStepnumber)==0) {
						manycount++;
						foundcurrentpara=true
						currentwaslast=true
						formulalog.info( " getNextStepOrSameStep parallelwf   found same stepnumber !! " + testitem + " (" + currentWorkFlowStepnumber + ")");
					} else {
						if (foundcurrentpara) {
							formulalog.info( " getNextStepOrSameStep parallelwf   foundnext!! " + testitem + " (" + testcurrentWorkFlowStep + ")");
							nextitem=testitem
							break
						} else {
							formulalog.debug( " getNextStepOrSameStep parallelwf   passing " + testitem );
						}
					}

				} else {
					formulalog.debug( " getNextStepOrSameStep testcurrentWorkFlowStep=" + testcurrentWorkFlowStep  + gnsossTMkeysitem.indexOf(testcurrentWorkFlowStep) + ' ' + gnsossTMkeysitem.indexOf(testcurrentWorkFlowStepParallelParent));

					if (gnsossTMkeysitem.indexOf(testcurrentWorkFlowStep)==0) {
						formulalog.info( " getNextStepOrSameStep    foundcurrent!! " + testitem );
						if (returnnextitem) {
						} else {
							nextitem=testitem
							break
						}

						currentwaslast=true
						foundcurrent=true;
					} else
					if (testcurrentWorkFlowStepParallelParent.indexOf(testitemname)==0) {
						formulalog.info( " getNextStepOrSameStep    foundcurrent!!++ " + testitem );
						if (returnnextitem) {
						} else {
							nextitem=testitem
							break
						}

						currentwaslast=true
						foundcurrent=true;
					} else {
						formulalog.debug( " getNextStepOrSameStep    passing " + testitem );
					}

				}
			}
		}
		if (currentwaslast) {
			formulalog.info( " getNextStepOrSameStep currentwaslast returning empty nextitem=" + nextitem )
			nextitem=''
		}
		formulalog.debug( " getNextStepOrSameStep nextitem=" + nextitem );

		nextstep=nextitem

	} catch( Exception ) {
		formulalog.warn( " getNextStepOrSameStepException getNextStep kids 1: " + Exception );
	}

	formulalog.info( " getNextStepOrSameStep returning nextitem=" + nextitem );

	return nextstep
}

function getCurrentStep(gcsTM, finddelim, currentWorkFlowStep, parentItemname) {

	return getNextStepOrSameStep(gcsTM, finddelim, currentWorkFlowStep, parentItemname, false, false);

}


function getPreviousStep(gpsTM, finddelim, currentWorkFlowStep, parentItemname) {

	rgpsTM=new java.util.TreeMap(java.util.Collections.reverseOrder())
	rgpsTM.putAll(gpsTM)
	return getNextStepOrSameStep(rgpsTM, finddelim, currentWorkFlowStep, parentItemname, true, false)

}


function getPreviousStepoverParallel(gpsTM, finddelim, currentWorkFlowStep, parentItemname) {

	rgpsTM=new java.util.TreeMap(java.util.Collections.reverseOrder())
	rgpsTM.putAll(gpsTM)
	return getNextStepOrSameStep(rgpsTM, finddelim, currentWorkFlowStep, parentItemname, true, true)

}




function getEventfield(thisEvent, thisfieldname, allowmissing) {
	retval=''
	try {
		if (thisEvent && thisEvent[thisfieldname] && thisEvent[thisfieldname]!=undefined && thisEvent[thisfieldname]!=null) {
			retval=thisEvent[thisfieldname]
		} else {
			if (allowmissing) {
			} else {
				formulalog.warn( "unhandled getEventfield " + thisEvent + " " + thisfieldname )
			}
		}
	} catch( Exception ) {
		formulalog.error( "unhandled getEventfield " + thisfieldname + ": " + Exception )
	}

	return retval
}


function nameFromDname(adname) {
	return Packages.com.proserve.Util.extractNameFromDName(adname)
}

function encodeURL(adname) {
	return Packages.com.proserve.Util.encodeURL(adname)
}

function decodeURL(adname) {
	return Packages.com.proserve.Util.decodeURL(adname)
}





/////////////////////


function queue_AcceptRejectWorkFlow(operat, myargs1, myargs2, myargs3, myargs4, myargs5) {
	thisfunctionname="queue_AcceptRejectWorkFlow";
	thatfunctionname=formulalog.getName()
	qformulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	qp1=true;

        try {

		qformulalog.debug( "------>starts..." + operat);
		qformulalog.debug( "  ------>starts..." + myargs1);
		qformulalog.debug( "  ------>starts..." + myargs2);

		AutomationScriptQueue = formula.util.AutomationScriptQueue;
		AutomationScriptTask = formula.util.AutomationScriptTask;

		//define the queue and start it running...do it only once.
		qformulalog.debug( "------> check to see if new queue needed..." );

		if (state[ourproperties+'_AutomationScriptQueue']==null ||
			state[ourproperties+'_AutomationScriptQueue']==undefined) {
			qformulalog.debug( "------> needs state[ourproperties+'_AutomationScriptQueue'] == "  + state[ourproperties+'_AutomationScriptQueue'] );
			state[ourproperties+'_AutomationScriptQueue'] = new AutomationScriptQueue(ourproperties+'_AutomationScriptQueue');
			qformulalog.debug( "------> created state[ourproperties+'_AutomationScriptQueue'] " + state[ourproperties+'_AutomationScriptQueue'] );
		} else {
			qformulalog.debug( "------> using state[ourproperties+'_AutomationScriptQueue'] " + state[ourproperties+'_AutomationScriptQueue'] );
		}


		if (state[ourproperties+'_AutomationScriptQueue'] &&
			state[ourproperties+'_AutomationScriptQueue']!=null &&
			state[ourproperties+'_AutomationScriptQueue']!=undefined) {

			qformulalog.debug( "  add to the queue " + state[ourproperties+'_AutomationScriptQueue']);

			// define the task
			var task = new Object();
			task.task_operat=operat;
			task.task_myargs1=myargs1;
			task.task_myargs2=myargs2;
			task.task_myargs3=myargs3;
			task.task_myargs4=myargs4;
			task.task_myargs5=myargs5;

			task.performTask = function() {


				qformulalog4j = Packages.org.apache.log4j.Logger
				qqformulalog = qformulalog4j.getLogger( "queue_AcceptRejectWorkFlow.task" )

				try {

					qqformulalog.info( "--> perform the task... " + this.task_operat)
					qqformulalog.info( "  --> task_myargs1..reason.." + this.task_myargs1)
					qqformulalog.debug( "  --> task_myargs2.." + this.task_myargs2)
					qqformulalog.info( "  --> task_myargs3..dname.." + this.task_myargs3)
					qqformulalog.info( "  --> task_myargs4..alarm.." + this.task_myargs4)
					qqformulalog.info( "  --> task_myargs5..user.." + this.task_myargs5)

					commandToSend=''
					if (this.task_operat.equals(state[ourproperties].workflowEventVerb_Accept)) {
						commandToSend=state[ourproperties].AcceptWorkFlowcommandToSend
					} else {
						commandToSend=state[ourproperties].RejectWorkFlowcommandToSend
					}

					rwfht=new java.util.Hashtable(state[ourproperties].asHashTable)

					rwfht.put('Reason', "'" + this.task_myargs1 + "'")
					rwfht.put('Reason2', "'" + this.task_myargs2 + "'")
					rwfht.put('dname', "'" + this.task_myargs3 + "'")

					rwfht.put('morefields', "")

					rwfht.put('alarm.originating_event_id', "'" + this.task_myargs4['originating_event_id'] + "'")

					rwfht.put('user', "'" + this.task_myargs5 + "'")

					sendAMessageq(rwfht, commandToSend, qqformulalog)

					qqformulalog.info( "--> performed the task... " )

				} catch (Exception) {
					qqformulalog.error( " task exception "  + Exception)
				}

				this.task_operat=null
				this.task_myargs1=null
				this.task_myargs2=null
				this.task_myargs3=null
				this.task_myargs4=null
				this.task_myargs5=null

			}

			qformulalog.debug( "------>  task for JavaAdapter " + task);

			// now adapt the task to the AutomationScriptTask object
			var aTask = new JavaAdapter(AutomationScriptTask, task);

			qformulalog.debug( "------>  JavaAdapter " + aTask);

			try {
				// now push the task onto the queue
				state[ourproperties+'_AutomationScriptQueue'].push(aTask);

			} catch (Exception) {
				qformulalog.warn( " ------>exception MUST be a bad queue - recreate and retry "  + Exception)
				state[ourproperties+'_AutomationScriptQueue'] = null;
				try {
					state[ourproperties+'_AutomationScriptQueue'] = new AutomationScriptQueue(ourproperties+'_AutomationScriptQueue');

					qformulalog.debug( " retry push ")

					// now push the task onto the queue
					state[ourproperties+'_AutomationScriptQueue'].push(aTask);

				} catch (Exception) {
					qformulalog.error( " ------>exception MUST be REALLY a bad queue - "  + Exception)
					state[ourproperties+'_AutomationScriptQueue'] = null;
				}

			}
			qformulalog.debug( "------>added to the queue " + state[ourproperties+'_AutomationScriptQueue']);
		}

	} catch (Exception) {
		qformulalog.debug( " ------>exception "  + Exception)
	}

	qformulalog.debug( "------>done! ")


	//formulalog = formulalog4j.getLogger( thatfunctionname )



}




function evalScript(thisElement, thisscript, thisEvent, currentWorkFlowStepnumber, thisthis ) {

	thisfunctionname="evalScript";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	try {
		formulalog.debug( "--> thisscript " + thisscript)

		formulalog.info('currentWorkFlowStepnumber=' +  currentWorkFlowStepnumber)
//		for (thisthisp in thisthis) {
//			formulalog.info('thisthisp ' + thisthisp + '=' +  thisthis[thisthisp])
//		}

		evc=eval(''+thisscript+'')
		formulalog.info( "--> evaled thisscript " + evc)
	} catch (Exception) {
		formulalog.error( " eval exception "  + Exception)
	}



	formulalog = formulalog4j.getLogger( thatfunctionname )


}



function queue_runAnyScript(thisElement, thisscript, thisEvent, currentWorkFlowStepnumber, thisthis ) {
	thisfunctionname="queue_runAnyScript";
	thatfunctionname=formulalog.getName()
	qrformulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	qp1=true;

        try {

		qrformulalog.info( "------>starts..." + thisElement);

		AutomationScriptQueue = formula.util.AutomationScriptQueue;
		AutomationScriptTask = formula.util.AutomationScriptTask;

		//define the queue and start it running...do it only once.
		qrformulalog.debug( "------> check to see if new queue needed..." );

		if (state[ourproperties+'_AutomationScriptQueue']==null ||
			state[ourproperties+'_AutomationScriptQueue']==undefined) {
			qrformulalog.debug( "------> needs state[ourproperties+'_AutomationScriptQueue'] == "  + state[ourproperties+'_AutomationScriptQueue'] );
			state[ourproperties+'_AutomationScriptQueue'] = new AutomationScriptQueue(ourproperties+'_AutomationScriptQueue');
			qrformulalog.debug( "------> created state[ourproperties+'_AutomationScriptQueue'] " + state[ourproperties+'_AutomationScriptQueue'] );
		} else {
			qrformulalog.debug( "------> using state[ourproperties+'_AutomationScriptQueue'] " + state[ourproperties+'_AutomationScriptQueue'] );
		}


		if (state[ourproperties+'_AutomationScriptQueue'] &&
			state[ourproperties+'_AutomationScriptQueue']!=null &&
			state[ourproperties+'_AutomationScriptQueue']!=undefined) {

			qrformulalog.info( "  add to the queue " + state[ourproperties+'_AutomationScriptQueue']);

			// define the task
			var task = new Object();
			task.taskElement=thisElement;
			task.taskscript=thisscript;
			task.taskthisEvent=thisEvent;
			task.taskcurrentWorkFlowStepnumber=currentWorkFlowStepnumber;
			task.taskthisthis=thisthis;

			task.performTask = function() {


				qrformulalog4j = Packages.org.apache.log4j.Logger
				qrqrformulalog = qrformulalog4j.getLogger( "queue_runAnyScript.task" )


				try {

					qrqrformulalog.debug( "--> perform the task... eval taskscript " + this.taskscript)

					var evc=eval(''+this.taskscript+'')

					qrqrformulalog.info( "--> performed the task... taskscript " + evc )

				} catch (Exception) {
					qrqrformulalog.error( " task eval exception "  + Exception)
				}

				this.taskElement=null
				this.taskscript=null
				this.taskthisEvent=null;
				this.taskcurrentWorkFlowStepnumber=null;
				this.taskthisthis=null;

			}

			qrformulalog.debug( "------>  task for JavaAdapter " + task);

			// now adapt the task to the AutomationScriptTask object
			var aTask = new JavaAdapter(AutomationScriptTask, task);

			qrformulalog.debug( "------>  JavaAdapter " + aTask);

			try {
				// now push the task onto the queue
				state[ourproperties+'_AutomationScriptQueue'].push(aTask);

			} catch (Exception) {
				qrformulalog.warn( " ------>exception MUST be a bad queue - recreate and retry "  + Exception)
				state[ourproperties+'_AutomationScriptQueue'] = null;
				try {
					state[ourproperties+'_AutomationScriptQueue'] = new AutomationScriptQueue(ourproperties+'_AutomationScriptQueue');

					qrformulalog.debug( " retry push ")

					// now push the task onto the queue
					state[ourproperties+'_AutomationScriptQueue'].push(aTask);

				} catch (Exception) {
					qrformulalog.error( " ------>exception MUST be REALLY a bad queue - "  + Exception)
					state[ourproperties+'_AutomationScriptQueue'] = null;
				}

			}
			qrformulalog.debug( "------>added to the queue " + state[ourproperties+'_AutomationScriptQueue']);
		}

	} catch (Exception) {
		qrformulalog.error( " ------>exception "  + Exception)
	}

	qrformulalog.info( "------>done! ")


	//formulalog = formulalog4j.getLogger( thatfunctionname )



}





function queue_deleteElement(thisElement) {
	thisfunctionname="queue_deleteElement";
	thatfunctionname=formulalog.getName()
	qdformulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )



	qp1=true;

        try {

		qdformulalog.debug( "------>starts..." + thisElement);

		AutomationScriptQueue = formula.util.AutomationScriptQueue;
		AutomationScriptTask = formula.util.AutomationScriptTask;

		//define the queue and start it running...do it only once.
		qdformulalog.debug( "------> check to see if new queue needed..." );

		if (state[ourproperties+'_AutomationScriptQueue']==null ||
			state[ourproperties+'_AutomationScriptQueue']==undefined) {
			qdformulalog.debug( "------> needs state[ourproperties+'_AutomationScriptQueue'] == "  + state[ourproperties+'_AutomationScriptQueue'] );
			state[ourproperties+'_AutomationScriptQueue'] = new AutomationScriptQueue(ourproperties+'_AutomationScriptQueue');
			qdformulalog.debug( "------> created state[ourproperties+'_AutomationScriptQueue'] " + state[ourproperties+'_AutomationScriptQueue'] );
		} else {
			qdformulalog.debug( "------> using state[ourproperties+'_AutomationScriptQueue'] " + state[ourproperties+'_AutomationScriptQueue'] );
		}


		if (state[ourproperties+'_AutomationScriptQueue'] &&
			state[ourproperties+'_AutomationScriptQueue']!=null &&
			state[ourproperties+'_AutomationScriptQueue']!=undefined) {

			qdformulalog.debug( "  add to the queue " + state[ourproperties+'_AutomationScriptQueue']);

			// define the task
			var task = new Object();
			task.taskElement=thisElement;
			task.performTask = function() {

				qdformulalog4j = Packages.org.apache.log4j.Logger
				qdqdformulalog = qdformulalog4j.getLogger( "queue_deleteElement.task" )

				try {

					qdqdformulalog.debug( "--> perform the task... deleteElement " + this.taskElement)

					deleteElement(this.taskElement)

					qdqdformulalog.debug( "--> performed the task... deleteElement " )

				} catch (Exception) {
					qdqdformulalog.debug( " task exception "  + Exception)
				}

				this.taskElement=null
			}

			qdformulalog.debug( "------>  task for JavaAdapter " + task);

			// now adapt the task to the AutomationScriptTask object
			var aTask = new JavaAdapter(AutomationScriptTask, task);

			qdformulalog.debug( "------>  JavaAdapter " + aTask);

			try {
				// now push the task onto the queue
				state[ourproperties+'_AutomationScriptQueue'].push(aTask);

			} catch (Exception) {
				qdformulalog.warn( " ------>exception MUST be a bad queue - recreate and retry "  + Exception)
				state[ourproperties+'_AutomationScriptQueue'] = null;
				try {
					state[ourproperties+'_AutomationScriptQueue'] = new AutomationScriptQueue(ourproperties+'_AutomationScriptQueue');

					qdformulalog.debug( " retry push ")

					// now push the task onto the queue
					state[ourproperties+'_AutomationScriptQueue'].push(aTask);

				} catch (Exception) {
					qdformulalog.error( " ------>exception MUST be REALLY a bad queue - "  + Exception)
					state[ourproperties+'_AutomationScriptQueue'] = null;
				}

			}
			qdformulalog.debug( "------>added to the queue " + state[ourproperties+'_AutomationScriptQueue']);
		}

	} catch (Exception) {
		qdformulalog.debug( " ------>exception "  + Exception)
	}

	qdformulalog.debug( "------>done! ")



	//formulalog = formulalog4j.getLogger( thatfunctionname )




}





function getMMfieldData(thiselement, thepage, thefield) {

	thisfunctionname="getMMfieldData";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	mmfd=null;
	alldone=false;

	// provides access to thing like
	// mmfd.getDisplayLabel());
	// mmfd.getPropertyName());
	// mmfd.getRangeName());

	//formulalog.info( " using page "  + thepage + " thefield=" + thefield)

	if (thepage!='') {

		try {
			mmfd=thiselement.getMetamodelField(session, thepage, thefield);
			//formulalog.info( " using page "  + thepage + " thefield=" + thefield)
		} catch (Exception) {
			formulalog.error( " @@@+++>exception "  + Exception)
		}

	} else {

		try {
			formulalog.debug( "   thepage=='' ")
			//formulalog.info( "   session "  + session)

			var proppages=thiselement.getMetamodelPageNames() // retunrs the pretty 'Display Name'  with '|'
//			var proppagesN=thiselement.getMetamodelPropertyPageNames(); // returns hte actual page name
// lets HOPE these 2 arrays have the same order...

			var founditever=null;

			formulalog.debug( "   proppages.length=" + proppages.length)

			for (i=0; i < proppages.length; i++) {

				if (proppages[i].equals(thepage) || thepage.equals('')) {

					formulalog.debug( " checking page "  + proppages[i] );
					try {
						proppagesattrs=thiselement.getMetamodelPageAttrIDs(proppages[i] , false)
						//formulalog.info( "   page proppagesattrs="  + proppagesattrs)
						formulalog.debug( "   page proppagesattrs.length="  + proppagesattrs.length)
						for (j=0; j < proppagesattrs.length; j++) {
							try {
								//thisname=proppagesattrs[j]
								//thisvalue=thiselement[proppagesattrs[j]]
								try {

									if (proppagesattrs[j].equals(thefield)) {
										formulalog.debug( "   found field "  + proppagesattrs[j] +" in page " +proppages[i])
										mmfd=thiselement.getMetamodelField(session, proppages[i], proppagesattrs[j])

										formulalog.debug( "       mmfd="  + mmfd)
										if (mmfd!=null) {
											//formulalog.debug( "     mmfd.getRangeName()="  + mmfd.getRangeName())
											formulalog.debug( "       found field "  + proppagesattrs[j] +" in page " +proppages[i] + " " + mmfd.getRangeName())
											founditever=mmfd;
											if (proppages[i].indexOf('Hidden')!=-1 || proppages[i].indexOf('__')==0) {
												formulalog.debug( "       skipping found field "  + proppagesattrs[j] +" in page " +proppages[i]  + " " + mmfd.getRangeName())
											} else {

												alldone=true;
												break;
											}
										}
									} else {
										//formulalog.debug( "   skipping field "  + proppagesattrs[j])
									}

								} catch (Exception) {
									formulalog.error( " +++++>exception "  + Exception)
								}
							} catch (Exception) {
								formulalog.error( " ------>exception "  + Exception)
							}
							if (alldone) {
								break;
							}

						}
						formulalog.debug( "   page proppagesattrs complete")
					} catch (Exception) {
						formulalog.error( " -page loop--->exception "  + Exception)
					}
				} else {
					//formulalog.debug( " skipping page "  + proppages[i])
				}
				if (alldone) {
					break;
				}
			}
			if (!alldone && founditever!=null) {
				mmfd=founditever;
				formulalog.debug( "  returning skipped " + mmfd.getRangeName())
			}


		} catch (Exception) {
			formulalog.error( " ===-->exception "  + Exception)
		}
	}
	formulalog.debug( "-->done! " + mmfd)


	formulalog = formulalog4j.getLogger( thatfunctionname )


	return mmfd;

}




///////////////////////////////////////////////////////////////////////

function logArgs(desc, argspassed) {
	for (iiik=0; iiik<argspassed.length; iiik++) {
		formulalog.info(desc+ '[' + iiik +']=' + argspassed[iiik]);
	}
}



function doNothing(passedparm) {
	thisfunctionname="doSomething";
	thatfunctionname=formulalog.getName()
	formulalog = formulalog4j.getLogger( thatfunctionname + "." + thisfunctionname  )


	formulalog.debug(passedparm);

	formulalog = formulalog4j.getLogger( thatfunctionname )

	return passedparm;
}

