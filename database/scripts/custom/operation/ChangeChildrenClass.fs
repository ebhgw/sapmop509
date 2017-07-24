/*
	Change Children Class
*/

var parentElement = element;
var currChildren = parentElement.Children;

for (var i=0; i<currChildren.length; i++){
	currChild = currChildren[i];
	currChild.rename(currChild.name,'AcronimoOpVw');
}
