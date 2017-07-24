// Read from formula.home + config directory (where you can find formula.custom.properties etc)
//a file with the given name and loads into a Properties object.
// The file is a java properties file, key = value
// Values can be read with props.get(key)

var Properties = (function (aModuleName) {

    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.util.properties');

// load a property file
    var loadFile = function (filepath) {
        return loadHelper(filepath);
    }

// load from formula.home config directory
    var loadConfig = function (mn) {
        var path = Packages.java.lang.System.getProperty("formula.home") + '/' + 'config' + '/' + mn + '.properties';
        return loadHelper(path);
    }

// load from formula.home config directory
    var loadCustomConfig = function (mn) {
        var path = Packages.java.lang.System.getProperty("formula.home") + '/' + 'config' + '/' + 'custom' + '/' + mn + '.properties';
        return loadHelper(path);
    }

    var loadHelper = function (filepath) {
        _logger.debug('loadHelper, loading properties file ' + filepath);
        var props = new Packages.java.util.Properties();
        try {
            var fis = new Packages.java.io.FileInputStream(filepath)
            props.load(fis);
            fis.close();
        } catch (excp) {
            _logger.warn('loadHelper, got excp ' + excp);
        }
        return props;
    }

    return {
        loadConfig:loadConfig,
        loadCustomConfig:loadCustomConfig,
        loadFile:loadFile
    }

})();
