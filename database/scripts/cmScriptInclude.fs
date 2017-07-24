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

//
//
//       Need to move these scripts to a seperate script - on rejects - increase the version number.
//
function cmRejectScript(thisEvent)
{
         var dn1=thisEvent['ItemToBeStarted'];
         var thisele=formulaRootfindElement(dn1);
         try
         {
            var _version  = new java.math.BigDecimal("1.0");
            try
            {
               _version  = new java.math.BigDecimal(thisele['Version']);
            }
            catch (ex)
            {
               formula.log.warn("Could not get version for contract - " + dn1 + " - Using default of 1.0 ");
            }
            _version = _version.add(new java.math.BigDecimal("0.1"));
            thisele['Version'] = _version.doubleValue() + "";
            formula.log.info("cmRejectScript :  " + thisele.getName() + " :NEW VERSION3 = " + thisele['Version'] );
	    var _reason  = thisele['Notes'];
	    var _newreason =  thisEvent['Reason'];
	    thisele['Notes'] = thisEvent['date'] + ">Reject : " + thisEvent['user'] + " : " + thisEvent['Reason'] +"\n"  + _reason  ;
	    thisele['Workflow Status'] = thisEvent['WorkFlowStepLevel2'] ;
         }
         catch ( Exception )
         {
            formula.log.error("cmRejectScript : " + Exception);
         }
}

//
// Run this script when entering a workflow.
//
function cmEnterWorkFlow(thisEvent)
{
       	try
        {
                formula.log.error("cmEnterWorkFlow : " + thisEvent);
	   	var dn1=thisEvent['ItemToBeStarted'];
         	var thisele=formulaRootfindElement(dn1);
		thisele['Workflow Status'] = thisEvent['WorkFlowStepLevel2'] ;
        }
        catch ( Exception )
        {
            formula.log.error("cmEnterWorkFlow : " + Exception);
        } 
}
//
// Run this script during the accept process.
//
function cmAcceptScript(thisEvent)
{
         try
         {
            var dn1=thisEvent['ItemToBeStarted'];
            thisele=formulaRootfindElement(dn1);
            formula.log.warn("cmAcceptScript3 Customer" + thisele['Customer']);

// Customers exist under - org=Customers/org=SCMS/root=Organizations

	    var customerDN = "Customer=" +  Packages.com.proserve.Util.encodeURL(thisele['Customer']) + "/org=Customers/org=SCMS/root=Organizations" ;
            formula.log.warn("cmAcceptScript3 " + thisEvent['WorkFlowStepLevel2N']);
            formula.log.warn("cmAcceptScript3 dname =" + customerDN);
            formula.log.warn("cmAcceptScript3 reason =" + thisEvent['Reason']);
	    var _reason  = thisele['Notes'];
	    var _newreason =  thisEvent['Reason'];
	    thisele['Notes'] = thisEvent['date'] + ">Accept : " + thisEvent['user'] + " : " + thisEvent['Reason'] +"\n"  + _reason  ;
	    thisele['Workflow Status'] = thisEvent['WorkFlowStepLevel2'] ;
      }
         catch ( Exception )
         {
            formula.log.error("cmAcceptScript : " + Exception);
         }

}

//
//    We are in the Production workflow - lets fix up the service groups et al.
//
function cmProcessWorkFlowScript(thisEvent)
{

try
{

formula.log.info("cmProcessWorkFlowScript : workflow : " + thisEvent['WorkFlowStepLevel1'] + " : " +  thisEvent['WorkFlowStepLevel1N']  ); 
	//
	// If we are in the production workflow
	//
         if ("Production".equals(thisEvent['WorkFlowStepLevel1']) ) 
         {
//
   	    // We have moved to the production workspace - need to take some actions.
	    //
            var dn1=thisEvent['WorkFlowStepLevel2ItemStarted'];
            formula.log.info("formulaRootfindElement looking for : " + dn1);
            thisele=formulaRootfindElement(dn1);
            formula.log.info("formulaRootfindElement found Contract : " + thisele.getDName());

	    thisele['Contract Status'] = "Approved";

	    //
	    // We will only handle Contract objects here - we need to do some postprocessing.
	    //
	    if ( thisele.getDName().startsWith("Contract=") )
	    {
		try
		{
			//
			//
			// Remove the Workflow elements when we move to the production area.
			//
	    		var orgchildrn = thisele.getChildren(formula.relations.ORG);
	    		if ( orgchildrn != null && orgchildrn.size() > 0 )
 	    		{
				for ( var k=0; k <orgchildrn.size(); k++ )
				{
		   			var achild = orgchildrn.get(k);
					if (achild.getDName().startsWith("WorkFlow=") ) 
					{
						thisele.removeORGChild(achild.getDName());
					}
				}
			}
	    	}
		catch (Exception)
		{	
		}

   	    var childrn = thisele.getChildren(formula.relations.NAM);
   	    if ( childrn != null && childrn.size() > 0 )
   	    {
   	    	for ( var k=0; k < childrn.size(); k++ )
   	    	{
   	    		var servicegrp = childrn.get(k);

   	    		var newChildrenList = new Array();

   	    		var serviceGrpChildren = servicegrp.getChildren(formula.relations.NAM);
   	    		if ( serviceGrpChildren != null && serviceGrpChildren.size() > 0 )
   	    		{
   	    			formula.log.info("cmProcessWorkFlowScript : Service Groups = " + serviceGrpChildren.size());

   	    			for ( var l=0; l < serviceGrpChildren.size(); l++ )
   	    			{
   	    				var serviceAgmt = serviceGrpChildren.get(l);
   	    				//cmAddSLAObjective(serviceAgmt);	
   	    				try
   	    				{
   	    					if ( serviceAgmt.getDName().startsWith("_SLA_Metric_Class=") ) // If its an external datasource service - add the external SLA
   	    					{
   	    						var offdefs =serviceAgmt.getExternalDbSLA();
   	    						//var aval = new Packages.com.mosol.ORB.Formula.AttrVal();
   	    						//aval.blob_val(Packages.com.mosol.util.Util.serializeObject(offdefs,false));
   	    						//var serviceOffersObj = new Packages.com.mosol.ORB.Formula.Attr("LocalOfferDefs", aval, 0);
   	    						//serviceAgmt.setAttr(serviceOffersObj);
   	    						formula.log.debug("cmAddSLAObjective2 : set offer on " + serviceAgmt);
   	    					} 
   	    				}
   	    				catch ( exception )
   	    				{
   	    					formula.log.error("cmAddSLAObjective : : " + exception);
   	    				}
   	    			}

   	    			//
   	    			// remove all the ORG children - this service group's ORG children will be 
   	    			// - so doing it with removeChildren instead of servicegrp['Children']=newChildrenList;
   	    			servicegrp.removeChildren(formula.relations.ORG);
   	    		}
   	    		else
   	    		{
   	    			formula.log.warn("Service Group does not have any Services : " + servicegrp.getName() );
   	    		}		
   	    	}
   	    }
	    else
	    {
		formula.log.warn("Contract does not have any service groups : " + thisele.getName() );
	    }
		   // Add link under the customer.
		   try
		   {
		   	var customerDN = "Customer=" +   Packages.com.proserve.Util.encodeURL(thisele['Customer']) + "/org=Customers/org=SCMS/root=Organizations" ;
			formula.log.info("Customer " + customerDN);
            		var customerElement = formulaRootfindElement( customerDN );//formula.Root.findElement(customerDN);
			formula.log.info("Found Customer " + customerElement);
			var pa=new Array();
			pa[0] =  thisele.getDName();
			customerElement['DisplaySourceElements']=true;
			customerElement.addChildren(pa, true);
		   }
		   catch ( exception )
		   {
		   	  formula.log.error("cmProcessWorkFlowScript : Could not find customer [" + thisele['Customer'] + "]  for contract \"" + thisele.getName() + "\" under org=Customers/org=SCMS/root=Organizations : " + exception  );
		   }
	    }
         }
	else
	{
            formula.log.info("cmProcessWorkFlowScript : Not in Production");
	}
}
catch ( Exception )
{
	formula.log.error("cmProcessWorkFlowScript " + Exception);
}
}

//
// Update the Customer by linking our contract under the Customer Object.
//
function cmUpdateCustomer(thisEvent)
{
// Add link under the customer.
try
{
        var dn1=thisEvent['ItemToBeStarted'];
        thisele=formulaRootfindElement(dn1);
			
	var customerDN = "Customer=" +  Packages.com.proserve.Util.encodeURL(thisele['Customer']) + state[ourproperties].ServiceCatalogCustomer;
// Read the Customer folder from our properties file.e.g. "/org=Customers/org=SCMS/root=Organizations" ;
        var customerElement = formulaRootfindElement(customerDN);
	var pa=new Array();
	pa[0] =  thisele.getDName();
	customerElement.addChildren(pa, true);
	formula.log.info("Updated Customer " +  Packages.com.proserve.Util.encodeURL(thisele['Customer']));
}
catch ( exception )
{
		formula.log.error("Could not find customer [" + thisele['Customer'] + "]  for contract \"" + thisele.getName() + "\" under org=Customers/org=SCMS/root=Organizations "  );
}
}

function cmUpdateSLALinks(servicegrp)
{
//          var slaRoot=formulaRootfindElement("root=Service+Level+Agreements")
// slaRoot.getOfferElements();
// The condition of the Service Group Elements - need to be driven by the condition of their offers.
// We will add the offer elements as ORG relations to the Service Group Elements.
	   var serviceGrpChildren = servicegrp.getChildren(formula.relations.ORG);
    	   if ( serviceGrpChildren != null && serviceGrpChildren.size() > 0 )
     	   {

			for ( var l=0; l < serviceGrpChildren.size(); l++ )
			{
		          var serviceAgmt = serviceGrpChildren.get(l);
			}
	   }
}

function cmAddLink()
{

var newChildrenList = new Array();

 newChildrenList[0] = ""
 newChildrenList[0] = ""
 newChildrenList[0] = ""
 element['Children']=newChildrenList;


//          Get the SLA Element
//          var slaRoot=formulaRootfindElement("root=Service+Level+Agreements")
//          var cld = slaRoot.getChildren(formula.relations.NAM);

}

function cmAddSLAObjective(serviceElement)
{
  try
  {
    if ( serviceElement.getDName().startsWith("_SLA_Metric_Class=") ) // If its an external datasource service - add the external SLA
    {
	var offdefs =serviceElement.getExternalDbSLA();
	var aval = new Packages.com.mosol.ORB.Formula.AttrVal();
	aval.blob_val(Packages.com.mosol.util.Util.serializeObject(offdefs,false));
	var serviceOffersObj = new Packages.com.mosol.ORB.Formula.Attr("LocalOfferDefs", aval, 0);
	serviceElement.setAttr(serviceOffersObj);
    } 
  }
  catch ( exception )
  {
	formula.log.error("cmAddSLAObjective : : " + exception);
  }
}

//
// formulaRootfindElement can sometimes require a "this" ?
function formulaRootfindElement( targettofind ) {
        var targettofindtarget=null;
        try {
                targettofindtarget = formula.Root.findElement( targettofind );

        } catch( Exception ) {
                targettofindtarget = this.formula.Root.findElement( targettofind );
                formula.log.warn( 'this.formula.Root.findElement suceeded where formula.Root.findElement failed' );
        }
        return targettofindtarget;
}


// @internal cmScriptInclude.fs -dc4a3e4
