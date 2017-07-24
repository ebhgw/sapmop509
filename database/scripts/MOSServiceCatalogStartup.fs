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

// this filename is also specified in the ServiceCatalog Webapp WEB-INF dir
state[ourproperties]=loadPropertiesFile('config/'+ourproperties+'.properties');


if (state[ourproperties]==null) {

formula.log.warn('load of ' + ourproperties + ' failed');

} else {

formula.log.debug('catalog root is ' + state[ourproperties].servicecatalogroot);

// add an version of the data as a hashtable for easy use by the MappedMessageFormat
//state[ourproperties].asHashTable=makeHashtableFromArray(state[ourproperties]);
//state[ourproperties].asHashTable=makeUpdatedHashtableFromArrayToSystemProperties(state[ourproperties]);

makesureithasbeencreated=new Packages.com.proserve.mosData;

state[ourproperties].asHashTable=makeUpdatedHashtableFromArrayTomosData(state[ourproperties], ourproperties);

formula.log.debug('state[ourproperties].asHashTable=' + state[ourproperties].asHashTable);

// reload the alarms from alarm history

hostStr = "http://" +server.getProperty("formula.web.server.host")+ ":"  +server.getProperty("formula.web.server.port")
oneurl=hostStr + state[ourproperties].reloadAlarmsURL

tMMformat=new Packages.com.proserve.MappedMessageFormat(oneurl);
oneurl=tMMformat.format(state[ourproperties].asHashTable)

// tell the automation script and or any adapter script that the alarms are from history reload
state[ourproperties].reloadingAdapter=true

runURL(oneurl)

// tell the automation script and or any adapter script that the alarms history reload is completed
state[ourproperties].reloadingAdapter=false

formula.log.info('RAN ' + oneurl);

}
// @internal MOSServiceCatalogStartup.fs -4jf950e
