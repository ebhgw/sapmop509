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
// Suppression/acknowledgement disable script
//
// Change to "on" to debug this script
// @debug off
//
////////////////////////////////////////////////////////////////////////////

if ( state.suppress != null ) {
	writeln( "Deinitializing suppression sub-system" )
	state.suppress.stop()
	state.suppress.release()
	state.suppress = null
}
else {
	writeln( "Suppression sub-system NOT initialized - ignore deinitialization attempt." )
}
