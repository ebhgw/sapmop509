var acroCatDn='gen_folder=Acronimo+Catalog/ISP_FolderBase=Production/root=Organizations';



var acroEle=state.OrgUtil.findElement(acroCatDn);



var acroChildren=acroEle.Children;
var match;
for (var i=0; i<acroChildren.length; i++){
            currChild = acroChildren[i];
	    match='ClasseCompInfra=.*/'+currChild.dname;
            state.OrgUtil.createElementMatches('AcronimoOpVw', currChild.name, element.dname, match); 
}
session.sendMessage('Copy Children ended');