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

var retval="";
var pc1=0;
try {
	// load scripts containing functions to be called later
	load('MOSServiceCatalogWorkerInclude.fs')

	if (this.args &&  this.args!=null &&  this.args!=undefined
		&& state[ourpropeties] && state[ourpropeties]!=undefined && state[ourpropeties]!=null) {

		workerscriptname="MOSservicecataloggetConfigValue"
		thatworkerscriptname=formula.logCategory
		formula.logCategory=workerscriptname

		formula.log.info('starts ' + args);
		thisargs0=args[0]
		formula.log.info(' thisargs0=' + thisargs0);
		if (thisargs0.equals("Server")) {
			retval="kjhlkjhkljhlkjh"
		} else {
			retval=state[ourpropeties][args[0]]
		}
		formula.log.info('ends');

		formula.logCategory=thatworkerscriptname
	} else {
		formula.log.error('flails');
	}



} catch (Exception) {
	formula.log.warn('problem skipping  '+ ' ' + pc1 + ' ' + Exception);
}

retval


// @internal MOSServiceCataloggetConfigValue.fs -2jc9051
