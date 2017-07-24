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

////////////////////////////////////////////////////////////////////////////
// Suppression/acknowledgement setup script
//
// Change to "on" to debug this script
// @debug off
//
//
// To start suppression for a adapter, add the following to the
// Script.onStarted parameter of the [Managed Objects] adapter:
//
//    @commands/suppress
//
// Resulting in:
//
//    Script.onStarted=@commands/suppress
//
// In the Adapters.ini file.
//
// To start no matter what adapter is running, add the following to
// Adapters.ini:
//
//    [Managed Objects]
//    Script.onStarted=@commands/suppress
//
////////////////////////////////////////////////////////////////////////////

// Alias for class.
Suppression = formula.commands.Suppression

// Example 1:
//
// Start suppression/ack subsystem with a single adapter (arg of this script).
//
// state.suppress = new Suppression( adapter )

// Example 2:
//
// Specify suppression wiht an operation match for a single organization
// hierarchy, plus a patrol adapter supplied.  In this case, note that
// the regex has escaped the backslash character by doubling (\\) and
// for +, which is a regex match character, escaped as \\+.
//
// state.suppress = new Suppression( /* adapter */ null )
// state.suppress.setOperationMatch( 'dnamematch:.*org=Online/root=Organizations|.*patrol=PATROL%28r%29\\+on\\+bogey/root=Elements' )

if( state.suppress == null )
{
   writeln( "Initializing suppression sub-system" );
   state.suppress = new Suppression( null );
   
   // Set this value to true to re-acknowledge elements that were acknowledged
   // when the Suppresion sub-system was shut-down.
   state.suppress.setPersistAcks( false );
   state.suppress.setOperationMatch( 'dnamematch:.*'  );
   state.suppress.start();
}
else
{
   writeln( "Suppression sub-system already initialized - ignore re-initialization attempt." );
}

// @internal suppress.fs cdg840m
