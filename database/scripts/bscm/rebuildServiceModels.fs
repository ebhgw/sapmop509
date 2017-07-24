// This script will rebuild all the Service Models structure, starting from Production/Services to all Dashboard Views

var currentElement;
var fromElement;
var currentDname;
var found = false;

// Create structure and associated dashboards

// Rebuild Services, policy Merge
currentElement=formula.Root.findElement('folder_application=Services/ISP_FolderBase=Production/root=Organizations');
currentElement.perform(session, "ViewBuilder|Run", [], []);
currentElement=null;

// Treat Dashboard - Business Views - ClasseCompInfra List
currentElement=formula.Root.findElement('gen_folder=ClasseCompInfra+List/gen_folder=Business+Views/Dashboard=Dashboard/root=Organizations');
currentElement.perform(session, "ViewBuilder|Run", [], []);
currentElement=null;

/* Strutture non utilizzate
// Treat Dashboard - Business Views - Host Acronimos
currentElement=formula.Root.findElement('gen_folder=Host+Acronimos/gen_folder=Business+Views/Dashboard=Dashboard/root=Organizations');
currentElement.perform(session, "ViewBuilder|Run", [], []);
currentElement=null;

// Treat Dashboard - Business Views - Open Acronimos
currentElement=formula.Root.findElement('gen_folder=Open+Acronimos/gen_folder=Business+Views/Dashboard=Dashboard/root=Organizations');
currentElement.perform(session, "ViewBuilder|Run", [], []);
currentElement=null;
*/

// Treat Dashboard - Service List - copy only the first three levels of Service
currentElement=formula.Root.findElement('gen_folder=Service+List/gen_folder=Business+Views/Dashboard=Dashboard/root=Organizations');
currentElement.perform(session, "ViewBuilder|Run", [], []);
currentElement=null;

// Top Acronimos è un match

// Top Classe Infra Comp è un match

// Top Services è un match

// Operations Views
// Acronimo maps è un match
// Host Acronimo Maps è a mano
// Open Acronimo Maps è a mano
// Open-Host Acronimo Maps è a mano
// Service Tree in parte match con aggiunta a mano
// Service Flows a mano

// Service Views
// Service ABC match
// Service ABC - Open bscm
// Treat Dashboard - Service List - copy only the first three levels of Service
currentElement=formula.Root.findElement('PortletView=Service+ABC+-+Open/gen_folder=Service+Views/Dashboard=Dashboard/root=Organizations');
currentElement.perform(session, "ViewBuilder|Run", [], []);
currentElement=null;

// Tech Views
// Top Services Overview match


// Treat Dashboard - Business Views - Top Services - Create link to PortletView=Top+Services/gen_folder=Business+Views/Dashboard=Dashboard/root=Organizations
try {
	currentDname = 'PortletView=Top+Services/gen_folder=Business+Views/Dashboard=Dashboard/root=Organizations');
	currentElement = formula.Root.findElement(currentDname);
	// copy from
	currentDname = 'gen_folder=Service+List/gen_folder=Business+Views/Dashboard=Dashboard/root=Organizations';
	fromElement = formula.Root.findElement(currentDname);
	found = true;
} catch (findExcp) {
	sessione.sendMessage('Not found ' + currentDname);
}

if (found) {
	var fromChildren = fromElement.Children;
	currentElement.Children = fromChildren;
}

found = false;

// Build Service Views Open - PortletView=Service+ABC+-+Open/gen_folder=Service+Views/Dashboard=Dashboard/root=Organizations
// Todo: match only ABC (and not ABC end to end)
currentElement=formula.Root.findElement('PortletView=Service+ABC+-+Open/gen_folder=Service+Views/Dashboard=Dashboard/root=Organizations');
currentElement.perform(session, "ViewBuilder|Run", [], []);
currentElement=null;

// Costruito con Match: PortletView=Top+Services+Overview/gen_folder=Tech+Views/Dashboard=Dashboard/root=Organizations
