//@debug off
// Align Service Models/Production to Generational Models/Structure/Baseline+Alarmed Elements/Servizi

load('custom/orgs/TreeAligner.fs');

//Servizi
var templateDn = 'gen_folder=Servizi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
var targetDn = 'gen_folder=Servizi/ISP_FolderBase=Production/root=Organizations'
formula.log.info('Aligning ' + targetDn + ' to template ' + templateDn);
TreeAligner.alignToTemplate(templateDn,targetDn)

//Acronimi
var templateDn = 'gen_folder=Acronimi/gen_folder=Baseline%2BAlarmedElement/gen_folder=Structure/root=Generational+Models/root=Services'
var targetDn = 'gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'
formula.log.info('Aligning ' + targetDn + ' to template ' + templateDn);
TreeAligner.alignToTemplate(templateDn,targetDn)

formula.log.info('Production subtree aligned');