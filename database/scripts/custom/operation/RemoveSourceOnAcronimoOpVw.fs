/*
	Remove Sources from element of class AcronimoOpVw
*/

(function () {

var visitor =
{
	empty: new Array(),
    count: 0,
    visit: function ( child )
    {
		var clazz = child.elementClassname + '';
		if (clazz == 'AcronimoOpVw') {
			child["SourceElements"] = visitor.empty;
			child["Children"] = visitor.empty;
			visitor.count++;
		}
    }
}
element.walk ( visitor );
formula.log.info('Reset source for ' + visitor.count + ' elements under ' + element.Dname);
})();
