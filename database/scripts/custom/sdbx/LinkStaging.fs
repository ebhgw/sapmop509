//@debug off
// Activate Modello
//
//
//	[Sdbx|Staging|Link Acronimi to Servizi]
//	command=
//	context=element
//	description=Sdbx|Link Acronimi to Servizi
//	operation=load( "custom/sdbx/LinkStaging.fs" );
//	permission=manage
//	target=dnamematch:^gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations
//	type=serverscript
//

state.ViewBuilder.build('gen_folder=Servizi/' + element.DName);
