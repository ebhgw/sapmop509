/*

 Script AcroCatalog.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Acronimo Catalog with descriptions
 Manage a property object. Properties key are

 <ambiente>.<acronimo>.<property>

 where

 environment = Produzione|SystemTest
 acronimo = ABCA0|...|WOTS0
 proprieta = nome proprieta (nota quello che si vede nel client è la label, controllare il nome esatto proprietà)

 */


var AcroCatalog = (function () {
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.acrocatalog");
	load('custom/lib/underscore.js');

    var prod_dn = 'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=Produzione/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';
    var systest_dn = 'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=SystemTest/gen_folder=Modello/gen_folder=Structure/root=Generational+Models/root=Services';

    var acronimo_props = new Packages.java.util.Properties();

    var clear = function () {
        // from hashtable
        acronimo_props.clear();
    }

    var setDescription   = function (env, acro, descr) {
        acronimo_props.setProperty(env+'.'+acro+'.'+'Description', descr);
    }

	// if default supplied return default else null
    var getDescription  = function (env, acro, def) {
        if(!def) {
            return acronimo_props.getProperty(env + '.' + acro + '.' + 'Description');
        } else {
			return acronimo_props.getProperty(env + '.' + acro + '.' + ' Description ', def);
		}
    }

    var loadAcroCatalogFromModello = function () {
        var prod = formula.Root.findElement(prod_dn);
        var systest = formula.Root.findElement(systest_dn);
		_.each(prod.children, function(child) { setDescription('Produzione', child.name, child.Description)});
        _.each(systest.children, function(child) { setDescription('SystemTest', child.name, child.Description)});
        /* when moving to new underscore version
		state._.each(prod.children, function(child) { setDescription('Produzione', child.name, child.Description)});
        state._.each(systest.children, function(child) { setDescription('SystemTest', child.name, child.Description)});
		*/
    };

    var storeAcroCatalogToFile = function (filename) {
        var writer = new Packages.java.io.PrintWriter( new Packages.java.io.FileOutputStream( filename, true ) );
        var comment = 'Acronimo properties wrote on ' + new Date();
        acronimo_props.store(writer, comment);
    }

    var loadAcroCatalogFromFile = function (filename) {
        var reader = new Packages.java.io.BufferedReader(new Packages.java.io.FileReader( filename ));
        acronimo_props.load(reader);
    }

    var getAcronimoProps = function () {
        return acronimo_props;
    }

    var setAcronimoProps = function (props) {
        // todo: check that props is actually of class Properties
        acronimo_props = props;
    }
	
	var setDescriptionOnEle   = function (ele) {
		var clazz = ele.elementClassName + '';
		if (clazz === 'Acronimo') {
			// collect descriptions and set acronimo_props
			loadAcroCatalogFromModello();
			var descr = getDescription(ele.Ambiente, ele.name);
            _logger.info('Setting Description ' + descr + ' on ' + ele.DName);
			// if no description found do nothing
            if (descr) {
				ele.Description = descr + '';
			} else {
				_logger.warn('Description not found for ' + ele.DName);
			}
        }
    }

    var walkAndSetDescriptionOnOrgs = function () {
        // collect descriptions and set acronimo_props
        loadAcroCatalogFromModello();
        // debug
        storeAcroCatalogToFile('E:/application/noc00/NovellOperationsCenter/data/acronimo_descriptions.txt');
        // Service Models element
        var service_models = formula.Organizations;
        var visitor =
        {
            count: 0,
            clazz: null,
            descr: null,
            found: false,
            visit: function ( child )
            {
                visitor.clazz = child.elementClassName + '';
                if (visitor.clazz === 'Acronimo') {
                    visitor.descr = getDescription(child.Ambiente, child.name);
                    _logger.debug('Setting Description ' + visitor.descr + ' on ' + child.DName);
					// if no description found do nothing
                    if (visitor.descr) {
						child.Description = visitor.descr + '';
					} else {
						_logger.warn('Description not found for ' + child.DName);
					}
                }
                visitor.count++;
            }
        }

        service_models.walk ( visitor );
        _logger.info('Set description on ' + visitor.count + ' elements');

    }

    return {
        getDescription:getDescription,
        setDescription:setDescription,
		setDescriptionOnEle:setDescriptionOnEle,
        walkAndSetDescriptionOnOrgs:walkAndSetDescriptionOnOrgs
    }

})();