
function SendSoapRequest (soapEnvelope) {
var xhr = new Packages.org.jdesktop.http.async.XmlHttpRequest();
var postMethod = Packages.org.jdesktop.http.Method.POST;
// false is sync
xhr.open(postMethod, "http://10.2.230.242:2007/EnqueueNocOperation.ashx", true);
xhr.setRequestHeader("Content-type", "text/xml;charset=uft-8");
xhr.setRequestHeader("SOAPAction", "urn:ECMWSMethod");
xhr.send(soapEnvelope.toString());
Packages.java.lang.Thread.sleep(1000);
writeln(xhr.status + ':' + xhr.statusText);
var res = xhr.getResponseText();
writeln(res)
return xhr;
}

// message is a xmldoc containing the bocy
function BuildSoapEnvelope(message) {

    var xmlDoc; 
    xmlDoc = Packages.org.jdesktop.dom.SimpleDocumentBuilder.simpleParse("<soap:Envelope " +
                            "xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' " +
                            "xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' " +
                            "xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                        "<soap:Body/>" +
                   "</soap:Envelope>");

    var envelopeNode=xmlDoc.firstChild;
    var bodyNode=envelopeNode.firstChild;
	var importedNode = xmlDoc.importNode(msg.documentElement, true);
    bodyNode.appendChild(importedNode);
   
	return xmlDoc
}

function TakeOwnershipTestData2XmlDoc () {
	var body = '<ns0:Command xmlns:ns0="http://IntesaSanpaolo/ECM/QWS/Messages/NOC" type="TakeOwnership" user="UXXXXX">'+
	'<ns0:ActionsList>'+
		'<ns0:Action mc_ueid="mcUeid1">'+
			'<ns0:ParamsList>'+
				'<ns0:Param name="status">'+
					'<ns0:Value>OPEN</ns0:Value>'+
				'</ns0:Param>'+
			'</ns0:ParamsList>'+
		'</ns0:Action>'+
		'<ns0:Action mc_ueid="mcUeid2">'+
			'<ns0:ParamsList>'+
				'<ns0:Param name="status">'+
					'<ns0:Value>OPEN</ns0:Value>'+
				'</ns0:Param>'+
			'</ns0:ParamsList>'+
		'</ns0:Action>'+
	'</ns0:ActionsList>'+
'</ns0:Command>'
return Packages.org.jdesktop.dom.SimpleDocumentBuilder.simpleParse(body);
}
