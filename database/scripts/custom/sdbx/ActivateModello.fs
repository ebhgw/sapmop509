//@debug off
// Activate Modello
// Called by operation
//	[Sdbx|Activate Modello]
//	command=
//	context=element
//	description=Sdbx|Activate Modello
//	operation=load( "custom/sdbx/ActivateModello.fs" );
//	permission=manage
//	target=dnamematch:^gen_folder=Modello/ISP_FolderBase=Sandbox.*/root=Organizations
//	type=serverscript
//
// Create correlation from Risorse to Elements/AllarmiSuElementi and link from Servizi to Acronimi
var dn = element.Dname;
state.ViewBuilder.build('gen_folder=Acronimi/' + dn);
state.ViewBuilder.build('gen_folder=Servizi/' +dn);
