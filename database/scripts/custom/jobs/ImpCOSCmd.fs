/*
 Script ImpCosCmd.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0
 
 cmds to build COS Model

 The process will run a bscm (none option) against the COS structure of bdi in order to add new elements
 Then will walk again and delete element that are will not be found

*/

load('custom/lib/underscore.js');

// use function mainly as a wrapper for environment, but it supply also a convenient access to return exit point
var ImpCOSCmd = (function () {

    var _modelDn = 'gen_folder=Servizio+COS/ISP_FolderBase=Production/root=Organizations'
    // baseline read from bdi
    var _baselineDn = 'Produzione=Produzione/ImpCosModello=Adapter%3A+ImpCosModello/root=Elements'

	load('custom/orgs/TreeAligner.fs');
    // Org usa la sessione
	load('custom/lib/Orgs.fs');
	load('custom/lib/ViewBuilder.fs');
	
    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.impcoscmd");

    // mapping structure (ServizioGlobalView grouping and mapping ServizioNomeNoc - ServizioNomeEcm) is defined
    // manually, so we add the Ambiente property in case user didn't do.
    var setAmbienteToProduzione = function (rootDn) {

        var envSetter = function (environment) {
            return {
                count: 0,
                visit: function ( child )
                {
                    clazz = child.elementClassname + '';
                    if (clazz == 'Servizio' || clazz == 'Acronimo' || clazz == 'AcronimoOpVw') {
                        child['Ambiente'] = environment;
                        this.count++;
                    }
                }
            }
        }

        var prodVisitor = buildEnvSetterVisitor('Produzione');
        var prodRoot = Orgs.findElement(rootDn);
        prodRoot.walk(prodVisitor);

    }

    // generate the event in order to trigger RunOnce schedule, that is fire query to db and update
    // the structure un Elements with data from updated tables
    var _runImpCOSBdi = function () {
        // Now rebuild the structure under Generational Models, the structure will be used as a template for Production
        _logger.info('Send bdi event to ImpCOS');
        load( 'util/bdievent' );
        fireBDIEvent( 'RunOnce' , 'Adapter: ImpCos' );
        // wait 30 seconds, no way to find when the bdi ends its processing so set a "safe" value
        Packages.java.lang.Thread.sleep(30000);
    }

    var _build = function () {
        setAmbienteToProduzione(_modelDn)
        // bdi to read model
        _runImpCOSBdi()
        // bscm to copy new elements
        _logger.info('Env:Production, building view Baseline+Alarmed Elements/Acronimi');
        ViewBuilder.build(_modelDn);
        // remove elements not found
        TreeAligner.deleteEleNotInBaseline(_modelDn, _baselineDn)
    }

    return {
        build:_build
    }
})();
