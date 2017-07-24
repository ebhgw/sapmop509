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
// Simple comparison function for string values.
function Compare( desc, arg1, arg2 )
{
    if( String(arg1) == String(arg2) )
        writeln( "Compare: " + desc + " --> true" )
    else
    {
        writeln( "Compare: " + desc + " --> false " )
        writeln( "Compare: arg1: " + arg1 )
        writeln( "Compare: arg2: " + arg2 )
    }
}

// Simple function to write an element.
function writeElement( element, name )
{
    writeln( name + ": " + element + ", type " + javaTypeOf( element ) + ", dname = [" + element.getReadableDName() + "]" )
}

function elementfun()
{
    // Get a simple element by traversal.
    var adapter = formula.Elements["demoMgr=Adapter: Demonstration"]
    if( ! adapter || ! adapter.name )
        throw "It seems the demo adapter is not set or started"
    var router1 = adapter["Rtr_Cisco=NYC-7513-1"]

    // Get a complex element by chaining lookup.
    var router2 = ( formula.Elements["demoMgr=Adapter: Demonstration"] )["Rtr_Cisco=NYC-7513-1"]

    // Get another complex element by variable lookup.
    var nyc = "NYC-7513-1"
    var router3 = adapter["Rtr_Cisco="+nyc]

    // Lookup an element.
    var router4 = formula.Elements[ "Rtr_Cisco=NYC-7513-1/demoMgr=Adapter%3A+Demonstration" ]

    // Find an element.
    var router5 = formula.Elements.findElement( "Rtr_Cisco=NYC-7513-1/demoMgr=Adapter%3A+Demonstration" )

    // Compare.
    writeElement( adapter, "adapter" )
    writeElement( router1, "router1" )
    writeElement( router2, "router2" )
    writeElement( router3, "router3" )
    writeElement( router4, "router4" )
    writeElement( router4, "router5" )
    Compare( "routers 1 vs. 2", router1, router2 )
    Compare( "routers 2 vs. 3", router2, router3 )
    Compare( "routers 3 vs. 4", router3, router4 )
    Compare( "routers 4 vs. 5", router4, router5 )

    // Get a simple organization
    var fork1 = formula.Organizations.findElement( "org=Fork" )
    var fork2 = formula.Organizations["org=Fork"]
    var fork3 = formula.Root[ "org=Fork/root=Organizations" ]
    var fork4 = formula.Root.findElement( fork3.DName )
    writeElement( fork1, "fork1" )
    writeElement( fork2, "fork2" )
    writeElement( fork3, "fork3" )
    writeElement( fork4, "fork4" )

    // Do some simple property lookups.
    for( var prop in fork1.properties )
        writeln( "Property: " + prop + " = " + fork1[ prop ] )
}

load('util/login')
if( login() )
    elementfun()
