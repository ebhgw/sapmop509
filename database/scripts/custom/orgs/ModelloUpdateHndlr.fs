/*

 Script ModelloUpdateHndlr.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Walking through ModelloUpdate generated hierarchy, apply changes defined by the Operazione field
 Piu = add element
 Meno = remove element
 Adding and modifying are always performed on an element of Risorsa class

 [Structure|Apply Model Update]
 command=
 context=element
 description=Structure|Apply Model Updat
 operation=load('custom/orgs/ModelloUpdateHndlr.fs');
 permission=view
 target=dname:formula_administration=Service+Models/logo_managedobjects=Administration/root=Generational+Models/root=Services
 type=serverscript

 */

function findElementToDelete(sele) {
    var newRootDn = 'gen_folder=Acronimi/gen_folder=Baseline/gen_folder=Structure/root=Generational+Models/root=Services';
    var tgtDn = 'Risorsa=' +  formula.util.encodeURL(sele.Risorsa)
        + '/CompInfra=' +  formula.util.encodeURL(sele.NomeComponenteInfra)
        + '/ClasseCompInfra=' + formula.util.encodeURL(sele.ClasseComponenteInfra)
        + '/Acronimo=' + formula.util.encodeURL(sele.Acronimo) + '/' + newRootDn;

    var ele = null;
    try {
        ele = formula.Root.findElement(tgtDn);
    } catch (excp) {
        formula.log.error('Exception ' + excp);
    }
    return ele;
}

var updateRootDn = 'Acronimi=Acronimi/ModelloUpdate=Adapter%3A+ModelloUpdate/root=Elements';
var updateRootEle = null;
try {
    updateRootEle = formula.Root.findElement(updateRootDn)
} catch (excp) {
    formula.log.error('Exception : ' + excp);
}

var visitor =
{
    count: 0,
    visit: function ( ele )
    {
        var clazz = ele.elementClassname + '';
        var tele;
        formula.log.info('Visiting ' + clazz + ' = ' + ele.name);
        if (clazz == 'LivelloRisorsa') {
            if (ele.Operazione == 'PIU') {
                formula.log.info('On dname PIU = ' + ele.Operazione);
            }
            if (ele.Operazione == 'MENO') {
                formula.log.info('On dname MENO = ' + ele.Operazione);
                tele = findElementToDelete(ele);
                formula.log.info('Deleting element ' + tele.dname);
                tele.perform( session, 'LifeCycle|Delete', [], [] );
            }
        }
    }
}

updateRootEle.walk(visitor);