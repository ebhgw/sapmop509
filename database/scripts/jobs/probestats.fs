// @debug off
var probeLog = formula.log.getInstance("Probe.Statistics");
var serverMetaElement = formula.Root.findElement('formulaServer=Server/root=Administration');
probeLog.info("Total memory: " + serverMetaElement['Object Heap Allocated Memory (KB)']);
probeLog.info("Total memory used: " + serverMetaElement['Object Heap Used Memory (KB)']);
probeLog.info("Total memory left: " + serverMetaElement['Object Heap Free Memory (KB)']);
probeLog.info("Total elements: " + serverMetaElement['Total Elements']);
probeLog.info("Total alarms: " + serverMetaElement['Total Alarms']);
probeLog.info("Sessions: " + serverMetaElement['Sessions']);
probeLog.info("Adapters defined: " + serverMetaElement['Adapters']);
probeLog.info("Config store status:\n" + server.getConfigStoreStatus().replace( "    ", "" ));
probeLog.info(server.getPerformanceEngine().getRepository().status());
var adapters = server.getAdapters();
for(var e = adapters.elements(); e.hasMoreElements();) {
  var instance = e.nextElement();
  probeLog.info("Name: " + instance.getKey() + ", class: " + instance.adapterClass().id() + ", status: " + instance.status());
  if(instance.detailedStatus) {
     probeLog.info(instance.detailedStatus());
  }
}
probeLog.info("\nService Level Agreements: ");
probeLog.info(server.getOfferAdapter().details());

var metaModelElement = formula.Root.findElement('Metamodel=Metamodel/root=Administration');
probeLog.info("Meta model:");
probeLog.info(metaModelElement.details());
