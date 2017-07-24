load('util/login.fs');
login('localhost', 8080, 'admin', 'formula')

var ele = formula.Root.findElement('logo_managedobjects=ReportAllarmi/root=Generational+Models/root=Services');
ele.perform(session, 'CSC|ReportAllarmi', [], args);
java.lang.Thread.sleep(7000);
formula.log.info('Report launched');