/*

 Script SetTlcMatch.fs

 Author: Bomitali Evelino
 Tested with versions: 5.0

 Walk and set Match
 Match is like

 [Pattern-LDAP]:(cn=apsftoccm-nx2v1-cpr_*)/gen_folder=Produzione/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\+tAllarmiOUTv2/root=Elements

 where cn depends on the name of the object. No formula.util.encodeURL heare

 */


// on classes BackEnd, Bilanciatori, Firewall, Router

(function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
    var walkRootDn = 'gen_folder=Servizio+TLC/ISP_FolderBase=Production/root=Organizations';

    var visitor =
    {
        count: 0,
        visit: function ( child )
        {
            var clazz = child.elementClassname + '';
            var lname, ldappattern, oldpattern;
            if (clazz == 'BackEnd'  || clazz == 'Bilanciatori' || clazz == 'Firewall' || clazz == 'Router') {
                lname = child.name;
                _logger.info('Checking ' + clazz + '=' + lname);
                _logger.info('Old Pattern ' + child.getMatchExpression());
                ldappattern = '[Pattern-LDAP]:(cn=' + lname +'_*)/gen_folder=Produzione/AllarmiSuElementi=AllarmiSuElementi/tAllarmiOUTv2=Adapter%3A\\+tAllarmiOUTv2/root=Elements';
				_logger.info('New Pattern ' + ldappattern);
				child.setElementMatchExpression(ldappattern);
                visitor.count++;
            }
        }
    }

    var ele = state.Orgs.findElement(walkRootDn);
    ele.walk ( visitor );
    _logger.info('Reset match for ' + visitor.count + ' elements under ' + element.Dname);
})();