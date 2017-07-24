//@debug off
// Align Production/Acronimi to Adapter: ImpModello/Acronimi

// Define template and target dname
var templateDn = 'Acronimi=Acronimi/ImpModello=Adapter%3A+ImpModello/root=Elements'
var targetDn = 'gen_folder=Acronimi/ISP_FolderBase=Production/root=Organizations'

formula.log.info('Aligning ' + targetDn + ' to template ' + templateDn);
state.StructMngr.alignToTemplate(templateDn, targetDn);
formula.log.info('Aligned');