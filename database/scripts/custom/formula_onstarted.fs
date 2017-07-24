// launched by Adapters.ini
// script run on Formula initialisation

formula.log.info('loading underscore v183');
//load('custom/lib/underscore183.js')

formula.log.info('loading AdaptersUtil');
load('custom/lib/AdaptersUtil.fs');
state.AdaptersUtil = AdaptersUtil;

formula.log.info('loading AlarmIndexCounterFromAlarms');
load('custom/alarm/AlarmIndexCounterFromAlarms.fs');
state.AlarmIndexCounterFromAlarms = AlarmIndexCounterFromAlarms;

formula.log.info('loading DBUtil');
load('custom/lib/DBUtil.fs');
formula.log.info('loading StringUtil');
load('custom/lib/StringUtil.fs');
state.StringUtil = StringUtil;

formula.log.info('loading DNamer');
load('custom/lib/DNamer.fs');
state.DNamer = DNamer;

formula.log.info('loading Orgs');
load('custom/lib/Orgs.fs');
state.Orgs = Orgs;

formula.log.info('loading OrgUtil');
load('custom/lib/OrgUtil.fs');
state.OrgUtil = OrgUtil;

formula.log.info('loading orgs.EleSourceUtil');
load('custom/lib/OrgsEleSourceUtil.fs');

formula.log.info('loading ViewBuilder');
load('custom/lib/ViewBuilder.fs');
state.ViewBuilder = ViewBuilder;

formula.log.info('loading FileWriter');
load('custom/util/FileWriter.fs');
state.FileWriter=FileWriter

// requires FileWriter
formula.log.info('loading ModelExporter');
load('custom/util/ModelExporter.fs');
state.ModelExporter=modelExporter

formula.log.info('loading ModelBuilder');
load('custom/modello/ModelloBuilder.fs');
state.ModelloBuilder=ModelloBuilder;

formula.log.info('loading StructMngr');
load('custom/orgs/StructMngr.fs');
state.StructMngr=StructMngr
formula.log.info('loading TreeAligner');
load('custom/orgs/TreeAligner.fs');
state.TreeAligner=TreeAligner;

formula.log.info('loading Cmd');
load('custom/lib/RunCmd.fs');
state.RunCmd = runCmd;

formula.log.info('loading EcmOp');
load('custom/ecmop/EcmOp.fs');
state.EcmOp = EcmOp;

// load moment (js date library) into state objects
formula.log.info('loading moment with language it');
load('custom/lib/moment.js');
load('custom/lib/moment-lang/it.js');
state.moment = moment;

formula.log.info('loading mm');
if (!state.mm) {
	state.mm = {}
}

formula.log.info('loading AcroCatalog');
load('custom/modello/acronimo/AcroCatalog.fs');
state.mm.AcroCatalog = AcroCatalog;

load('custom/mm/ServizioAlarmIndexPage.fs');
state.mm.ServizioAlarmIndexPageFunc = ServizioAlarmIndexPageFunc;

load('custom/mm/SIMetricheAlarmIndexPage.fs');
state.mm.SIMetricheAlarmIndexPageFunc = SIMetricheAlarmIndexPageFunc;

formula.log.info('loading bscm');
if (!state.bscm) {
	state.bscm = {}
}
load('custom/bscm/bekoperview.fs');
state.bscm.bekoperview = bekoperview;

formula.log.info('loading completed');