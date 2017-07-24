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
// View Builder setup script
//
// Change to "on" to debug this script
// @debug off
//
// To start the view builder subsystem for Managed Objects, add the following to the
// Script.onStarted parameter of the [Managed Objects] adapter:
//
//    @commands/viewbuilder
//
// Resulting in:
//
//    [Managed Objects]
//    Script.onStarted=@commands/viewbuilder
//
////////////////////////////////////////////////////////////////////////////

// Get the controller and start it.
var controller = Packages.com.mosol.Formula.Server.commands.ViewBuilder.getController()
// controller.setOperationMatch( 'dnamematch:.*Auto+Generated/root=Organizations' )
controller.start()

// @internal viewbuilder.fs d9417af
