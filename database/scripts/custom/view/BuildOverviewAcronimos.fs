/*

  Author: Bomitali Evelino
  Tested with versions: 5.0
  
  Build Overview - Acronimos view using bscm + post processing script

*/

(function () {

// will build a view of type Overview - Acronimos
// first check element

var classRenameVisitor =
{
    count: 0,
    visit: function ( child )
    {
		child.rename(child.name, 'AcronimoOpVw');
        classRenameVisitor.count++;
    }
}


if (element.name == 'Overview - Acronimos') {

	element.perform( session, "ViewBuilder|Run", [], [] );
	// loop on children and rename them
	var toReclass = element.children;
	var current, name;
	/*
	for (var i = 0; i < toReclass.length; i++) {
		current = toReclass[i];
		name = current.name;
		current.rename(name, 'AcronimoOpVw');
	}
	*/
	// element.walk ( classRenameVisitor );
	
}

})();