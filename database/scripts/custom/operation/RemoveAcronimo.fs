//@debug off
/*

 */

load('custom/util/OrgsCopier.fs');

var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
_logger.info('running RemoveAcronimo');
state.Orgs.deleteElement(element)
_logger.info('RemoveAcronimo completed');