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

// @opt -1 

// load scripts containing functions to be called later
load('MOSServiceCatalogWorkerInclude.fs') 


// load our properties file
if (state[ourpropeties] && state[ourpropeties]!=undefined && state[ourpropeties]!=null) {
	workerscriptname="MOSservicecatalogWorker"
	thatscriptname=formula.logCategory
	formula.logCategory=workerscriptname

	formula.log.info('starts');


	oneurl=hostStr + state[ourpropeties].reloadAlarmsURL + " happy times state[ourpropeties].reloadingAdapter=" + state[ourpropeties].reloadingAdapter
	
	tMMformat=new Packages.com.mosol.util.MappedMessageFormat(oneurl)
	oneurl=tMMformat.format(state[ourpropeties].asHashTable)

	doNothing(oneurl)


	formula.log.info('ends');

	formula.logCategory=thatscriptname
} else {
	formula.log.error('flails');
}



// @internal MOSServiceCatalogWorker.fs 8i7hf1l
