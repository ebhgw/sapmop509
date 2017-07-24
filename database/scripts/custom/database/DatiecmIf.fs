/*
 Reads data from db
 */


var DatiecmIf = (function () {

    var _logger = Packages.org.apache.log4j.Logger.getLogger("fs.readmodel4db");
    _logger.info("Read Start");
	
	var myDBdriver = "net.sourceforge.jtds.jdbc.Driver";
	var myDBurl = "jdbc:jtds:sqlserver://PDBCBP001.sede.corp.sanpaoloimi.com:1579/DATIECM";
	var myDBuser = "noc00_app";
	var myDBpassword = "noc00_ApP@";
	var myFShost = "PDBCBP001.sede.corp.sanpaoloimi.com";
	var myFSport = "1579";

    load('custom/lib/DBUtil.fs');

    function wrapNoNullString(paramSQLString) {
        if (paramSQLString != null && paramSQLString.toLowerCase != 'null') {
            return '"' + paramSQLString + '"';
        }
        else {
            return '""';
        }
    }

	function replaceNewlines(txt) {
        return txt.replace(/(\r\n|\n|\r)/gm," ");
    }

    var _readBaseline = function () {

        var recCounter = 0;
        var myString = "";
        var myDB, myQuery, myRS;
        var bodyStep;


        try {

            bodyStep = 'Connecting';
            myDB = DbUtil.getDBConnection(myDBdriver, myDBurl, myDBuser, myDBpassword);

            //_logger.info("FSAFeeder connected");
            bodyStep = 'Querying';
            /*
             SELECT [Ambiente]
             ,[ServizioGlobalView]
             ,[Servizio]
             ,[SSA]
             ,[Acronimo]
             ,[DescAcronimo]
             ,[ClasseComponente]
             ,[NomeCompInfra]
             ,[Risorsa]
             FROM [DATIECM].[dbo].[v_modello_ecm]

             */
            myQuery =
                "SELECT [Ambiente] " +
                    ",[ServizioGlobalView] " +
                    ",[Servizio] " +
                    ",[SSA] " +
                    ",[Acronimo] " +
                    ",[DescAcronimo] " +
                    ",[ClasseComponente] " +
                    ",[NomeCompInfra] " +
                    ",[Risorsa] " +
                    " FROM [DATIECM].[dbo].[v_modello_ecm]"

            _logger.info('Query is ' + myQuery);
            myRS = DbUtil.getSQLResult(myDB, myQuery);

            // Read through ResultSet, build csv row
            // myRS.beforeFirst()

            bodyStep = 'RecordLoop';
            var rec = '';
            var collected_data = new Packages.java.lang.StringBuilder(8192);
            collected_data.append('"Ambiente";"ServizioGlobalView";"Servizio";"SSA";"Acronimo";"DescAcronimo";"ClasseComponente";"NomeCompInfra";"Risorsa"\r\n');
            while (myRS.next()) {
                rec = '';

                _logger.info("Looping step 1 init loop");
                recCounter++;

                myString = wrapNoNullString(myRS.getString("Ambiente")) + ';';
                myString += wrapNoNullString(myRS.getString("ServizioGlobalView")) + ';';
                myString += wrapNoNullString(myRS.getString("Servizio")) + ';';
                myString += wrapNoNullString(myRS.getString("SSA")) + ';';
                myString += wrapNoNullString(myRS.getString("Acronimo")) + ';';
                myString += wrapNoNullString(myRS.getString("DescAcronimo")) + ';';
                myString += wrapNoNullString(myRS.getString("ClasseComponente")) + ';';
                myString += wrapNoNullString(myRS.getString("NomeCompInfra")) + ';';
                myString += wrapNoNullString(myRS.getString("Risorsa")) + '\r\n';

                collected_data.append(myString);
            }

            bodyStep = 'Closing ResultSet';
            myRS.close();

            DbUtil.disconnect(myDB)

        } catch (mErr) {
            switch (bodyStep) {
                case 'Connecting':
                    _logger.error("Exception while connecting: " + mErr);
                    break;
                case 'Querying':
                    _logger.error("Exception while querying: " + mErr);
                    break;
                case 'RecordLoop':
                    _logger.error("REC #" + recCounter + ", Exception " + mErr);
                    break;
                case 'Closing':
                    _logger.error("Exception while closing: " + mErr);
                    break;
                default:
                    _logger.error("Exception: " + mErr);
            }
        }

        _logger.info("_readBaseline read " + recCounter + " record");
        return collected_data + '';
    }
	
	var _readBaselineServizi = function () {

        var recCounter = 0;
        var myString = "";
        var myDB, myQuery, myRS;
        var bodyStep;

        try {

            bodyStep = 'Connecting';
            myDB = DbUtil.getDBConnection(myDBdriver, myDBurl, myDBuser, myDBpassword);

            //_logger.info("FSAFeeder connected");
            bodyStep = 'Querying';
            /*
             SELECT [Ambiente]
             ,[ServizioGlobalView]
             ,[Servizio]
             ,[SSA]
             ,[Acronimo]
             ,[DescAcronimo]
             ,[ClasseComponente]
             ,[NomeCompInfra]
             ,[Risorsa]
             FROM [DATIECM].[dbo].[v_modello_ecm]

             */
            myQuery =
                "SELECT distinct [Ambiente] " +
                    ",[ServizioGlobalView] " +
                    ",[Servizio] " +
                    ",[Acronimo] " +
                    ",[DescAcronimo] " +
                    " FROM [DATIECM].[dbo].[v_modello_ecm]"

            _logger.info('Query is ' + myQuery);
            myRS = DbUtil.getSQLResult(myDB, myQuery);

            // Read through ResultSet, build csv row
            // myRS.beforeFirst()

            bodyStep = 'RecordLoop';
            var rec = '';
            var collected_data = new Packages.java.lang.StringBuilder(8192);
            collected_data.append('"Ambiente";"ServizioGlobalView";"Servizio";"Acronimo";"DescAcronimo"\r\n');
            while (myRS.next()) {
                rec = '';

                _logger.info("Looping step 1 init loop");
                recCounter++;

                myString = wrapNoNullString(myRS.getString("Ambiente")) + ';';
                myString += wrapNoNullString(myRS.getString("ServizioGlobalView")) + ';';
                myString += wrapNoNullString(myRS.getString("Servizio")) + ';';
                myString += wrapNoNullString(myRS.getString("Acronimo")) + ';';
                myString += wrapNoNullString(myRS.getString("DescAcronimo")) + '\r\n';

                collected_data.append(myString);
            }

            bodyStep = 'Closing ResultSet';
            myRS.close();

            DbUtil.disconnect(myDB)

        } catch (mErr) {
            switch (bodyStep) {
                case 'Connecting':
                    _logger.error("Exception while connecting: " + mErr);
                    break;
                case 'Querying':
                    _logger.error("Exception while querying: " + mErr);
                    break;
                case 'RecordLoop':
                    _logger.error("REC #" + recCounter + ", Exception " + mErr);
                    break;
                case 'Closing':
                    _logger.error("Exception while closing: " + mErr);
                    break;
                default:
                    _logger.error("Exception: " + mErr);
            }
        }

        _logger.info("_readBaselineServizi read " + recCounter + " record");
        return collected_data + '';
    }
	
	var _readAlarms48hoursCompleto = function () {

        var recCounter = 0;
        var myString = "";
        var myDB, myQuery, myRS;
        var bodyStep;


        try {

            bodyStep = 'Connecting';
            myDB = DbUtil.getDBConnection(myDBdriver, myDBurl, myDBuser, myDBpassword);

            //_logger.info("FSAFeeder connected");
            bodyStep = 'Querying';
            
            myQuery =
                "SELECT [IDReceivedAlarm] " +
      ",[IDExport] " +
      ",[Ambiente] " +
      ",[Anomalia] " +
      ",[Area_tecnologica] " +
      ",[aree_impattate] " +
      ",[Clienti] " +
      ",[cod_location] " +
      ",[impatto_servizio] " +
      ",[Key_Anomalia] " +
      ",[Livello_architetturale] " +
      ",[mc_owner] " +
      ",[mc_priority] " +
      ",[msg] " +
      ",[Nodo] " +
      ",[Nome_componente] " +
      ",[Nome_risorsa] " +
      ",[Rilevato] " +
      ",[Servizio_infrastrutturale] " +
      ",[severity] " +
      ",[severityN] " +
      ",[orig_severity] " +
      ",[Source] " +
      ",[status] " +
      ",[Ticket_Num] " +
      ",[Tipo_anomalia] " +
      ",[Tipo_componente] " +
      ",[Tipo_risorsa] " +
      ",[mc_date_modification] " +
      ",[CELLA_ORIGINE] " +
      ",[event_handle] " +
      ",[Causa] " +
      ",[msg_catalog] " +
      ",[In_manutenzione] " +
      ",[Fuori_servizio] " +
      ",[Servizio] " +
      ",[NomeComponenteServizio] " +
      ",[Acronimo] " +
      ",[DescAcronimo] " +
      ",[ClasseComponenteInfra] " +
      ",[NomeComponenteInfra] " +
      ",[Server] " +
      ",[NomeElementoTecno] " +
      ",[repeat_count] " +
      ",[mc_notes] " +
      ",[mc_long_msg] " +
      ",[Severita_precedente] " +
      ",[ExportDt] " +
      ",[mc_ueid] " +
	  " FROM [DATIECM].[dbo].[v_48hours_all_alarms] "

            _logger.info('Query is ' + myQuery);
            myRS = DbUtil.getSQLResult(myDB, myQuery);

            // Read through ResultSet, build csv row
            // myRS.beforeFirst()

            bodyStep = 'RecordLoop';
            var rec = '';
            var collected_data = new Packages.java.lang.StringBuilder(8192);
			collected_data.append('"IDReceivedAlarm";"IDExport";"Ambiente";"Anomalia";"Area_tecnologica";"aree_impattate";"Clienti";"cod_location";"impatto_servizio";' +
			'"Key_Anomalia";"Livello_architetturale";"mc_owner";"mc_priority";"msg";"Nodo";"Nome_componente";"Nome_risorsa";"Rilevato";"Servizio_infrastrutturale";' +
			'"severity";"severityN";"orig_severity";"Source";"status";"Ticket_Num";"Tipo_anomalia";"Tipo_componente";"Tipo_risorsa";"mc_date_modification";"CELLA_ORIGINE";' +
			'"event_handle";"Causa";"msg_catalog";"In_manutenzione";"Fuori_servizio";"Servizio";"NomeComponenteServizio";"Acronimo";"DescAcronimo";"ClasseComponenteInfra";' +
			'"NomeComponenteInfra";"Server";"NomeElementoTecno";"repeat_count";"mc_notes";"mc_long_msg";"Severita_precedente";"ExportDt";"mc_ueid"\r\n')
           
		   while (myRS.next()) {
                rec = '';

                _logger.info("Looping step 1 init loop");
                recCounter++;

				myString = wrapNoNullString(myRS.getString("IDReceivedAlarm")) + ';';
                myString += wrapNoNullString(myRS.getString("IDExport")) + ';';
                myString += wrapNoNullString(myRS.getString("Ambiente")) + ';';
                myString += wrapNoNullString(myRS.getString("Anomalia")) + ';';
                myString += wrapNoNullString(myRS.getString("Area_tecnologica")) + ';';
                myString += wrapNoNullString(myRS.getString("aree_impattate")) + ';';
                myString += wrapNoNullString(myRS.getString("Clienti")) + ';';
                myString += wrapNoNullString(myRS.getString("cod_location")) + ';';
                myString += wrapNoNullString(myRS.getString("impatto_servizio")) + ';';
                myString += wrapNoNullString(myRS.getString("Key_Anomalia")) + ';';
				myString += wrapNoNullString(myRS.getString("Livello_architetturale")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_owner")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_priority")) + ';';
				myString += wrapNoNullString(myRS.getString("msg")) + ';';
				myString += wrapNoNullString(myRS.getString("Nodo")) + ';';
				myString += wrapNoNullString(myRS.getString("Nome_componente")) + ';';
				myString += wrapNoNullString(myRS.getString("Nome_risorsa")) + ';';
				myString += wrapNoNullString(myRS.getString("Rilevato")) + ';';
				myString += wrapNoNullString(myRS.getString("Servizio_infrastrutturale")) + ';';
				myString += wrapNoNullString(myRS.getString("severity")) + ';';
				myString += wrapNoNullString(myRS.getString("severityN")) + ';';
				myString += wrapNoNullString(myRS.getString("orig_severity")) + ';';
				myString += wrapNoNullString(myRS.getString("Source")) + ';';
				myString += wrapNoNullString(myRS.getString("status")) + ';';
				myString += wrapNoNullString(myRS.getString("Ticket_Num")) + ';';
				myString += wrapNoNullString(myRS.getString("Tipo_anomalia")) + ';';
				myString += wrapNoNullString(myRS.getString("Tipo_componente")) + ';';
				myString += wrapNoNullString(myRS.getString("Tipo_risorsa")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_date_modification")) + ';';
				myString += wrapNoNullString(myRS.getString("CELLA_ORIGINE")) + ';';
				myString += wrapNoNullString(myRS.getString("event_handle")) + ';';
				myString += wrapNoNullString(myRS.getString("Causa")) + ';';
				myString += wrapNoNullString(myRS.getString("msg_catalog")) + ';';
				myString += wrapNoNullString(myRS.getString("In_manutenzione")) + ';';
				myString += wrapNoNullString(myRS.getString("Fuori_servizio")) + ';';
				myString += wrapNoNullString(myRS.getString("Servizio")) + ';';
				myString += wrapNoNullString(myRS.getString("NomeComponenteServizio")) + ';';
				myString += wrapNoNullString(myRS.getString("Acronimo")) + ';';
				myString += wrapNoNullString(myRS.getString("DescAcronimo")) + ';';
				myString += wrapNoNullString(myRS.getString("ClasseComponenteInfra")) + ';';
				myString += wrapNoNullString(myRS.getString("NomeComponenteInfra")) + ';';
				myString += wrapNoNullString(myRS.getString("Server")) + ';';
				myString += wrapNoNullString(myRS.getString("NomeElementoTecno")) + ';';
				myString += wrapNoNullString(myRS.getString("repeat_count")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_notes")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_long_msg")) + ';';
				myString += wrapNoNullString(myRS.getString("Severita_precedente")) + ';';
				myString += wrapNoNullString(myRS.getString("ExportDt")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_ueid")) + '\r\n';

                collected_data.append(myString);
            }

            bodyStep = 'Closing ResultSet';
            myRS.close();

            DbUtil.disconnect(myDB)

        } catch (mErr) {
            switch (bodyStep) {
                case 'Connecting':
                    _logger.error("Exception while connecting: " + mErr);
                    break;
                case 'Querying':
                    _logger.error("Exception while querying: " + mErr);
                    break;
                case 'RecordLoop':
                    _logger.error("REC #" + recCounter + ", Exception " + mErr);
                    break;
                case 'Closing':
                    _logger.error("Exception while closing: " + mErr);
                    break;
                default:
                    _logger.error("Exception: " + mErr);
            }
        }

        _logger.info("Read Ended");
        return collected_data + '';
    }
	
	var _readAlarms48hours = function () {

        var recCounter = 0;
        var myString = "";
        var myDB, myQuery, myRS;
        var bodyStep;


        try {

            bodyStep = 'Connecting';
            myDB = DbUtil.getDBConnection(myDBdriver, myDBurl, myDBuser, myDBpassword);

            //_logger.info("FSAFeeder connected");
            bodyStep = 'Querying';
            
            myQuery =
                "SELECT [IDReceivedAlarm] " +
      ",[IDExport] " +
      ",[Ambiente] " +
      ",[Anomalia] " +
      ",[Area_tecnologica] " +
      ",[aree_impattate] " +
      ",[Clienti] " +
      ",[cod_location] " +
      ",[impatto_servizio] " +
      ",[Key_Anomalia] " +
      ",[Livello_architetturale] " +
      ",[mc_owner] " +
      ",[mc_priority] " +
      ",[msg] " +
      ",[Nodo] " +
      ",[Nome_componente] " +
      ",[Nome_risorsa] " +
      ",[Rilevato] " +
      ",[Servizio_infrastrutturale] " +
      ",[severity] " +
      ",[severityN] " +
      ",[orig_severity] " +
      ",[Source] " +
      ",[status] " +
      ",[Ticket_Num] " +
      ",[Tipo_anomalia] " +
      ",[Tipo_componente] " +
      ",[Tipo_risorsa] " +
      ",[mc_date_modification] " +
      ",[CELLA_ORIGINE] " +
      ",[event_handle] " +
      ",[Causa] " +
      ",[msg] " +
      ",[msg_catalog] " +
      ",[In_manutenzione] " +
      ",[Fuori_servizio] " +
      ",[Servizio] " +
      ",[NomeComponenteServizio] " +
      ",[Acronimo] " +
      ",[DescAcronimo] " +
      ",[ClasseComponenteInfra] " +
      ",[NomeComponenteInfra] " +
      ",[Server] " +
      ",[NomeElementoTecno] " +
      ",[repeat_count] " +
      ",[mc_notes] " +
      ",[mc_long_msg] " +
      ",[Severita_precedente] " +
      ",[ExportDt] " +
      ",[mc_ueid] " +
	  " FROM [DATIECM].[dbo].[v_48hours_all_alarms] "

            _logger.info('Query is ' + myQuery);
            myRS = DbUtil.getSQLResult(myDB, myQuery);

            // Read through ResultSet, build csv row
            // myRS.beforeFirst()

            bodyStep = 'RecordLoop';
            var rec = '', txt = '';
            var collected_data = new Packages.java.lang.StringBuilder(8192);
			collected_data.append('"Ambiente";"Servizio";"severity";"orig_severity";"Rilevato";"mc_date_modification";"status";"repeat_count";"Nodo";' +
			'"Nome_componente";"Nome_risorsa";"aree_impattate";"Servizio_infrastrutturale";"Area_tecnologica";"Tipo_componente";"Ticket_Num";"mc_owner";"msg";"msg_catalog";"mc_notes";' +
			'"mc_long_msg";"Acronimo";"ClasseComponenteInfra";"NomeComponenteInfra";"NomeElementoTecno";"IDReceivedAlarm";"mc_ueid"\r\n')
           
		   while (myRS.next()) {
                rec = '';

                _logger.info("Looping step 1 init loop");
                recCounter++;

				myString = wrapNoNullString(myRS.getString("Ambiente")) + ';';
				myString += wrapNoNullString(myRS.getString("Servizio")) + ';';
				myString += wrapNoNullString(myRS.getString("severity")) + ';';
				myString += wrapNoNullString(myRS.getString("orig_severity")) + ';';
				myString += wrapNoNullString(myRS.getString("Rilevato")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_date_modification")) + ';';
				myString += wrapNoNullString(myRS.getString("status")) + ';';
				myString += wrapNoNullString(myRS.getString("repeat_count")) + ';';
				myString += wrapNoNullString(myRS.getString("Nodo")) + ';';
				myString += wrapNoNullString(myRS.getString("Nome_componente")) + ';';
				myString += wrapNoNullString(myRS.getString("Nome_risorsa")) + ';';
				myString += wrapNoNullString(myRS.getString("aree_impattate")) + ';';
				myString += wrapNoNullString(myRS.getString("Servizio_infrastrutturale")) + ';';
				myString += wrapNoNullString(myRS.getString("Area_tecnologica")) + ';';
				myString += wrapNoNullString(myRS.getString("Tipo_componente")) + ';';
				myString += wrapNoNullString(myRS.getString("Ticket_Num")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_owner")) + ';';
				myString += wrapNoNullString(myRS.getString("msg")) + ';';
				myString += wrapNoNullString(myRS.getString("msg_catalog")) + ';';
				
				txt = replaceNewlines(myRS.getString("mc_notes") + '');
				myString += wrapNoNullString(txt) + ';';
				txt = replaceNewlines(myRS.getString("mc_long_msg") + '');
				myString += wrapNoNullString(txt) + ';';
				
				myString += wrapNoNullString(myRS.getString("Acronimo")) + ';';
				myString += wrapNoNullString(myRS.getString("ClasseComponenteInfra")) + ';';
				myString += wrapNoNullString(myRS.getString("NomeComponenteInfra")) + ';';
				myString += wrapNoNullString(myRS.getString("NomeElementoTecno")) + ';';
				myString += wrapNoNullString(myRS.getString("IDReceivedAlarm")) + ';';
				myString += wrapNoNullString(myRS.getString("mc_ueid")) + '\r\n';

                collected_data.append(myString);
            }

            bodyStep = 'Closing ResultSet';
            myRS.close();

            DbUtil.disconnect(myDB)

        } catch (mErr) {
            switch (bodyStep) {
                case 'Connecting':
                    _logger.error("Exception while connecting: " + mErr);
                    break;
                case 'Querying':
                    _logger.error("Exception while querying: " + mErr);
                    break;
                case 'RecordLoop':
                    _logger.error("REC #" + recCounter + ", Exception " + mErr);
                    break;
                case 'Closing':
                    _logger.error("Exception while closing: " + mErr);
                    break;
                default:
                    _logger.error("Exception: " + mErr);
            }
        }

        _logger.info("_readAlarms48hours read " + recCounter + " alarms");
        return collected_data + '';
    }


    return {
		readAlarms48hours:_readAlarms48hours,
        readBaseline:_readBaseline,
		readBaselineServizi:_readBaselineServizi
    }

}());