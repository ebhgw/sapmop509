//@debug off
/*

 */

load('custom/util/OrgsCopier.fs');

var _logger = Packages.org.apache.log4j.Logger.getLogger("fs");
_logger.info('running CopyStaging');
var actor = OrgsCopier();
//actor.test();
actor.copySubTree('gen_folder=Staging/ISP_FolderBase=SandboxSystemTest/root=Organizations', 'gen_folder=Acronimi/ISP_FolderBase=SandboxSystemTest/root=Organizations');
_logger.info('CopyStaging completed');