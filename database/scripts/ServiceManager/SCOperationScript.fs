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


// The name of this script
var SCRIPT_NAME = 'SCOperationScript.fs'


function getProperties( el, isIntegrationElement )
{
    var props = new java.util.Properties()
    props.put( '__alarms', alarms )
    if ( isIntegrationElement )
    {
        var aproperties = el.getProperties()
        for ( var i = 0; i < aproperties.length; i ++ )
        {
            var k = aproperties[ i ].getName()
            var idx = k.lastIndexOf( "/" )
            if ( idx > -1 )
            {
                k = k.substring( idx + 1 )
            }
            var o = aproperties[ i ].getValue();
            props.put( "element." + k, null == o ? "" : o.toString() )
        }
    }
    else
    {
        for( var p in el.properties )
        {
            var k = p
            var o = el[ p ]
            props.put( "element." + k, null == o ? "" : o.toString() )
        }        
    }
    return props
}


function executeOperation( adapterDName, module, operationName )
{
var adapterElement
var emsg = SCRIPT_NAME + ": could not find adapter named " + adapterDName
try
{
	adapterElement = formula.Root.findElement( 'adapter=' + formula.util.encodeURL( adapterDName ) + "/adapters=Adapters/root=Administration" ) 
}
catch( e )
{
    formula.log.error(emsg + "\n" + e.toString())
    session.sendMessage(emsg);
    return
}

if ( null == adapterElement ) 
{
    formula.log.error(emsg)
    session.sendMessage(emsg)
    return
}

var adapter = adapterElement.getAdapter()
var started = adapter.isStarted()

if ( ! started ) 
{
    var emsg = SCRIPT_NAME + ": adapter " + adapterDName + " not started"
    formula.log.error(emsg)
    session.sendMessage(emsg)
    return
}

// Set up element information


if ( element.getClass().getName() == 'com.mosol.Adapter.integration.IntegrationElement' )
{
    el = element.getElement()
    isIntegrationElement = true
}
else
{
    el = element
    isIntegrationElement = false
}
var elName = element.getName()
var elProperties = getProperties( el, isIntegrationElement )

// Get the service center root element instance and invoke the operation.

var rootElement = adapter.getRootElement().getElement()

rootElement.executeOperation( module, operationName, elName, el, elProperties, session )

return
}
