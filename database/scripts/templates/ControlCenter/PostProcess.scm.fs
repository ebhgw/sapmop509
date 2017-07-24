var CC=Packages.com.mosol.Formula.Templates.NQCCDB;
var CCType=CC.CCTypeHelper.CCElemType;

var alarmFilterBuilder = CC.eventFilter.NOCAlarmFilterCreatorForControlCenter.getInstance();
var ccElement;

try
{
	formula.log.info("Begin post-processing");
	state.ccCache.setServiceModelRoot(context,element);

	// Perform post process.
	element.walk ( createVisitorImpl() )

	// Remove obsolete service model alarm filters.
	removeObsoleteAlarmFilters();
}
finally
{
  // Remove ccCache from script state.
  state.ccCache = null;

  // Done post processing, so unset populating flag, which removes "(discovering...)" from the element name.
  element.setFlag(Packages.com.mosol.Formula.Server.ElementImpl.Flags_Populating, false);
  element.pulseCondition();
}

function removeObsoleteAlarmFilters() 
{
	alarmFilterBuilder.removeUncachedAppMgrAlarmFilters( state.ccCache );
}

function createVisitorImpl()
{
	var visitor =
	{
		visit: function ( element )
		{      
		
			ccElement = element;
			if(isMarkedForDelete())
			{
				return;
			}
			try
			{
				var type = CC.CCTypeHelper.getCCType(ccElement);
				if( type != null)
				{
					if (type == CCType.EventView )  
					{ 
						doAlarmViewUpdates();
					}
					else if(type == CCType.ServerView)
					{
						doServerViewUpdates();
					}
					else if(type == CCType.ServiceMap)
					{
						doServiceMapUpdates();
					}
					else if(type == CCType.ManagementGroup)
					{
						doManagementGroupUpdates();
					} 
				} 
			}
			catch(e)
			{
				formula.log.error("Unexpected error processing:" + ccElement, e);
			}
		}
	}
	return visitor
}	

function isMarkedForDelete()
{
   return state.ccCache.isMarkedForDelete(context,ccElement)
}

function doServiceMapUpdates() 
{
	trace("Building layout for " + ccElement);
	ccElement.setDisplaySourceElements(false);
	var builder = state.ccCache.getSMVBuilder(ccElement['ObjectID'])
	if(builder != null)
	{
		builder.applyLayout(context,state.ccCache, ccElement);
	}

}
function doManagementGroupUpdates() 
{
	CC.Util.haltAlarms(ccElement, formula.log);
}


function doServerViewUpdates()
{
	buildLinkRelationships();
	ccElement.setDisplaySourceElements(true);
	buildAlarmFilters();
	populateFiltersPropertyPage();
}
function doAlarmViewUpdates()
{
	buildLinkRelationships();
	ccElement.setDisplaySourceElements(false);
	assignElementState();
	
	buildAlarmFilters();
	populateFiltersPropertyPage();
}

function buildLinkRelationships()
{
	trace("Creating links and alarm filters for event view " + ccElement);
	CC.LinkManager.createLinksFor(context, ccElement, state.ccCache);
	trace('Post Links for:' + ccElement); 
}

function assignElementState()
{
	state.ccCache.getConditionManager().assignConditionToElement(ccElement);
}

function getViewInfoForElement()
{
	return state.ccCache.getViewInfo(ccElement['ObjectID']);
}

function buildAlarmFilters()
{
	var viewInfo = getViewInfoForElement();
	if(viewInfo != null)
	{
		var eventfilters = viewInfo.getEventFilters();
		trace('processing filters :' + eventfilters); 
		alarmFilterBuilder.createFiltersForElement( ccElement, eventfilters )
		trace('after processing filters :' + eventfilters); 
	}
}


function populateFiltersPropertyPage()
{
	var viewInfo = getViewInfoForElement();
	if(viewInfo == null)
	{
		return;
	}
	
	var appliedServerFilters = new Array();
	var unsupportedServerFilters = new Array();
	var appliedAlarmFilters = new Array();
	var unsupportedAlarmFilters = new Array();
	
	sortFilters(viewInfo.getServerFilters(), appliedServerFilters,unsupportedServerFilters);	
	sortFilters(viewInfo.getEventFilters(), appliedAlarmFilters,unsupportedAlarmFilters);	
	
	ccElement['Applied Filters'] = buildHTMLProperty(appliedServerFilters, appliedAlarmFilters);
	ccElement['Unsupported Filters'] = buildHTMLProperty(unsupportedServerFilters, unsupportedAlarmFilters);
}

function sortFilters(filters, supported, unsupported)
{
	if(filters == null)
	{
		return;
	}
	
	var listIter = filters.getAllFilters().iterator();
	while(listIter.hasNext())
	{
		var filter = listIter.next();
		if(filter.isSupported())
		{
			supported.push(filter);
		}
		else
		{
			unsupported.push(filter);
		}
	}
}

function buildHTMLProperty(serverFilters, eventFilters)
{
	if(serverFilters.length == 0 && eventFilters.length == 0)
	{
		return 'None';
	}
	
	return '<ul>' + buildHTMLFilterList("Server:", serverFilters) +  buildHTMLFilterList("Event:", eventFilters) + '</ul>';
}

function buildHTMLFilterList(caption, filters)
{
	if(filters.length == 0)
	{
		return '';
	}
	var html = "<li>" + caption + "<ul>";
	
	for (var i = 0; i < filters.length;i++)
	{
		html += "<li>"+filters[i].toString() + "</li>";
	}
	return html + "</li></ul>";
}

function trace(msg)
{
   formula.log.debug(msg);
}


// @internal PostProcess.scm.fs -3a33i0m
