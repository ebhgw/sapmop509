var eleName = element.name;
var eleClazz = element.elementClassname + '_view1';
formula.log.info('Renaming on element ' + element.dname);
element.perform( session, 'Rename', [], [ 'pippo', 'caio' ] );
formula.log.info('Renamed');