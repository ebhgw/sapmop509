/*
[Test|Server GetElement]
command=
context=element
description=Test|Server GetElement
operation=load('bscm/testServerGetElement.fs');
permission=view
target=dname:gen_folder=buildModdedStructure/gen_folder=testModStructure/root=Generational+Models/root=Services
type=serverscript
*/

var dname, scriptElement;
dname = 'Risorsa_view1=sapvmp66%3A7584/CompInfra_view1=Web+Server/gen_folder=buildModdedStructure/gen_folder=testModStructure/root=Generational+Models/root=Services'
scriptElement = server.getElement( dname );
formula.log.info('Created ' + scriptElement.name);

dname = 'Risorsa_view1=sapvmp67%3A7584/CompInfra_view1=Web+Server/gen_folder=buildModdedStructure/gen_folder=testModStructure/root=Generational+Models/root=Services'
scriptElement = server.getElement( dname );
formula.log.info('Created ' + scriptElement.name);