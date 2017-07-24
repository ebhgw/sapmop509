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

//ourproperties='MOSservicecatalog'

var ttformulalog4j = Packages.org.apache.log4j.Logger
var ttformulalog = ttformulalog4j.getLogger( ourproperties + '.WorkerTestTag' )

var nowtime=(new java.util.Date()).getTime();


ttformulalog.debug('Starts ' + alarm.originating_event_id + ' '+ alarm.ID + ' ' + alarm.status + ' ' + alarm.severity + '.')

var retval=true;
var pc1=0;
try {
	// load scripts containing functions to be called later
	load('MOSServiceCatalogWorkerInclude.fs')


	// load our properties file
	if (state[ourproperties] && state[ourproperties]!=undefined && state[ourproperties]!=null) {

		ttformulalog.debug('starts');

		if (state[ourproperties].reloadingAdapter) {

			retval=AdapterReloading(alarm)

		} else {

			retval=ProcessEvent(alarm)

		}

		ttformulalog.debug('ends');

	} else {
		ttformulalog.error('flails');
	}



} catch (Exception) {
	ttformulalog.warn('problem skipping  '+ ' ' + pc1 + ' ' + Exception);
}
ttformulalog.debug('Ends ' +  ((new java.util.Date()).getTime() - nowtime) + 'ms ' + alarm);

retval


// @internal MOSServiceCatalogWorkerTestTag.fs 3l6e71f
