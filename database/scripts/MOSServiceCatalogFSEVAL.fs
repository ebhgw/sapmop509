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

try {

	if (this.args && args!=undefined) {
		formula.log.info('EVALing args' )

		formula.log.info('        EVALing ' + args[0])

		evc=eval(''+args[0]+'')

		formula.log.info('EVALed returned =' + evc)
	} else
	if (this.vargs && vargs!=undefined) {
		formula.log.info('EVALing vargs ' )
		formula.log.info('        EVALing ' + vargs[0])


		evc=eval(''+vargs[0]+'')

		formula.log.info('EVALed returned =' + vargs)
	} else {
		formula.log.info('called but nothing to run ')

		//formula.log.info('        args=' + args)
		//formula.log.info('        vargs=' + vargs)

	}


} catch (Exception) {
	formula.log.error('EVAL ' + Exception)
}


// @internal MOSServiceCatalogFSEVAL.fs a509ba5
