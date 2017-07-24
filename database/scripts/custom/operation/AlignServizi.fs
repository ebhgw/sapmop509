//@debug off
// Align Generational Models/Structure/Baseline+Alarmed Elements/Servizi
// to Generational Models/Structure/Test/Servizi

// Define template and target dname
//var templateDn = 'Acronimi=Acronimi/ImpModello=Adapter%3A+ImpModello/root=Elements'
//var targetDn = 'gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'

load('custom/orgs/TreeAligner.fs');

//Servizi
var templateDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
var targetDn = 'gen_folder=Servizi/gen_folder=Test/gen_folder=Structure/root=Generational+Models/root=Services'
formula.log.info('Aligning ' + targetDn + ' to template ' + templateDn);
TreeAligner.alignToTemplate(templateDn,targetDn)

//Acronimi
var templateDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
var targetDn = 'gen_folder=Acronimi/gen_folder=Test/gen_folder=Structure/root=Generational+Models/root=Services'
formula.log.info('Aligning ' + targetDn + ' to template ' + templateDn);
TreeAligner.alignToTemplate(templateDn,targetDn)

formula.log.info('Aligned');