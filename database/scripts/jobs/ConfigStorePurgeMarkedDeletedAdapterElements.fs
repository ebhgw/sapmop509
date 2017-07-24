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
// @internal
///////////////////////////////////////////////////////////////////////////////////

if ( server.isCoordinator() )
{
   Packages.com.mosol.Formula.Server.ConfigUtil.removeMarkedDeletedRootElementsConfigEntries(
      false /* disable if tracking history is on */,
      true /* remove even if has element history */,
      true /* post traverse down not deleted parents */,
      0 /* no max traverse depth */, 0 /* no max traverse entries */, 0 /* no max remove entries */ );
}
else
{
   server.log.warn("The current server is not a cluster coordinator.The ConfigStore 'Adapter Elements Marked Deleted' data purge script will be executed only on the cluster coordinator server.");
}

// @internal ConfigStorePurgeMarkedDeletedAdapterElements.fs d8c6m98
