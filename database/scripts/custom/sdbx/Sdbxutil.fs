// util for Sdbx

var SdbxUtil = (function () {
    var _getEnvironment = function(ele) {
        var patt = /(ISP_FolderBase=Sandbox(.*))\/root=Organizations/;
        var match = patt.exec(ele.DName);
        var envr
        if (match) {
            //_logger.debug('_getContext, env ' + match[2] + ' acroFolderDn ' + match[1] + '/root=Organizations');
            envr = match===null?null:match[2];
            if (envr === 'Production')
                envr = 'Produzione';
            return envr;
        }
    }

    return {
        getEnvironment:_getEnvironment
    }
})();
