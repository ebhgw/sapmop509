/*

 Script PathBuilder.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Given a set of OrgInfo objects, build a subtree path starting from a root node

 [Test|Create Element]
 command=
 context=element
 description=Test|Create Element
 operation=load('custom/orgs/createElement.fs');
 permission=view
 target=dname:gen_folder=test/root=Generational+Models/root=Services
 type=serverscript

 */