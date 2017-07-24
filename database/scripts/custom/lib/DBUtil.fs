/*
  Script DbUtil.fs
  
  Author: NessPRO Italy - 2006
  Tested with versions: 3.1.0 to 4.7

  Utilities for DB connection and generic SQL-related issue
*/


function DbUtil () {
}

/*
// Set up a DB connection using the parameters provided
// Return the connection
*/
DbUtil.getDBConnection = function(driver, url, user, password)
{
 var conn = null;
 try
 {
   formula.log.debug("Setting DB connection, driver=" + driver + ", url=" + url + ", user=" + user);
   java.lang.Class.forName(driver);
   conn = java.sql.DriverManager.getConnection(url, user, password);
 }
 catch (Exception)
 {
   formula.log.error("Exception <" + Exception + "> in getDBConnection(" + driver + ", " + url + ", " + user + ", ********)");
 }
 return conn ;
}

/* Close the DB connection 'conn'
   Return true (disconnect OK) or false (disconnect not OK)
*/
DbUtil.disconnect = function(conn)
{
 try
 {
   conn.close();
   conn = null ;
   return true;
 }
 catch (Exception)
 {
   formula.log.error("Exception <" + Exception + "> in disconnect()");
   return false;
 }
}

/* Read a SQL query text from the text file with name 'file'
   Return the SQL query as string
*/
DbUtil.getSQLTextFromFile = function(file)
{
 var mySQLStmt = "";
 try
 {
   if (dbutil.log.isDebugEnabled())
      formula.log.debug("Reading SQL query from file " + file);

   var myLine = null;
   var myFile = java.io.File(file);
   var myInput = java.io.RandomAccessFile(myFile,"r");
   while ((myLine = myInput.readLine()) != null)
   {
     mySQLStmt += myLine.trim() + "\n";
   }
   myInput.close();

   if (dbutil.log.isDebugEnabled())
      formula.log.debug("SQL query: " + mySQLStmt);
 }
 catch (Exception)
 {
   formula.log.error("Exception <" + Exception + "> in getSQLTextFromFile(" + file + ")");
 }
 return mySQLStmt;
}


/* Retrieve a resultset executing the query 'sqltext' on the connection 'conn'
// Return the resultset
*/
DbUtil.getSQLResult = function(conn, sqltext)
{
 try
 {
   var myStmt = conn.createStatement();
   return myStmt.executeQuery(sqltext);
 }
 catch (Exception)
 {
   formula.log.error("Exception <" + Exception + "> in getSQLResult(conn, \"" + sqltext + "\")");
   return null;
 }
}

/* Retrieve a resultset executing a query read on the connection 'conn', the
// query text is read from the file with name 'file'
// Return the resultset
*/
DbUtil.getSQLResultFromFile = function(conn, file)
{
 return dbutil.getSQLResult(conn, getSQLTextFromFile(file));
}

/* Return a Date object converted from a java.sql.Timestamp
*/
DbUtil.getJSDateFromSQLTimestamp = function(sqlTimestamp)
{
 return new Date(sqlTimestamp.getTime());
}
// End of function getJSDateFromSQLTimestamp


/*/ Return a java.sql.Timestamp converted from a Date object
*/
DbUtil.getSQLTimestampFromJSDate = function (jsDate)
{
 return new java.sql.Timestamp(jsDate);
}


/* Dump the content of the resultset 'rs': if the optional 'limit' is specified,
// stops reading the 'rs' after 'limit' records.
// (limit < 0 => no limit; limit == 0 => dumps only the metadata).
// Return nothing
// This function is intended for rough debug only
*/
DbUtil.dumpResultSet = function(rs, limit)
{
 try
 {
   if (limit == null)
      limit = -1;
   limit = parseInt(limit);

   dbutil.log.info("--- Begin dumping recordset " + rs.toString() +
                   " (read limit set to " + (limit < 0 ? "INFINITE" : limit) + ")");

   var recCount = 0;
   var md = rs.getMetaData();
   var col = md.getColumnCount();
   var dumpstr = "";
   var i = 0;

   for (i = 1; i <= col; i++)
   {
       dumpstr += " <" + md.getColumnName(i) + "(" + md.getColumnTypeName(i) + ")>";
   }
   dbutil.log.info("#:" + dumpstr);

   while (rs.next() && recCount != limit)
   {
     recCount++;
     dumpstr = "";
     for (i = 1; i <= col; i++)
     {
         dumpstr += " <" + rs.getString(i) + ">";
     }
     dbutil.log.info(recCount + ":" + dumpstr);
   }

   dbutil.log.info("--- End of dump of recordset " + rs.toString() + ", " + recCount + " records read");
 }
 catch (Exception)
 {
   formula.log.error("Exception <" + Exception + "> in dumpResultSet()");
 }
}

/* Dump the content of the resultset 'rs': if the optional 'limit' is specified,
// stops reading the 'rs' after 'limit' records.
// (limit < 0 => no limit; limit == 0 => dumps only the metadata).
// Return nothing
// This function is intended for rough debug only
*/
DbUtil.xtractResultSet = function(rs, limit)
{
	try
	{
		if (limit == null)
			limit = -1;
		limit = parseInt(limit);

		logger.info("--- Begin xtracting recordset " + rs.toString() +
					   " (read limit set to " + (limit < 0 ? "INFINITE" : limit) + ")");

		var recCount = 0;
		var md = rs.getMetaData();
		var col = md.getColumnCount();
		var i = 0, colname = "", colvalue = "";
		var row; 
		var arRows = new Array();

		while (rs.next() && recCount != limit)
		{
			row = new Object();
			for (i = 1; i <= col; i++)
			{
				colname = md.getColumnName(i);
				colvalue = rs.getString(i);
				row[colname] = colvalue;
				//logger.info("Column " + colname + " : " + colvalue);
			}
			//logger.info("Loaded " + recCount + " : " + row);
			arRows[recCount] = row;
			recCount++;
			//logger.info("Processing " + recCount + " record");
		}
		
	} catch (Exception) {
		logger.error("Exception in xtractResultSet :" + Exception);
	}
	
	return arRows;
}

/* Cache the default DB connection for later use
// Return nothing
*/
DbUtil.start = function()
{
 dbutil.defaultDBConnection = dbutil.getDefaultDBConnection();
 formula.log.debug("Cached defaultDBConnection is ready");
}

/*/ Close the cached DB connection
// Return nothing
*/
DbUtil.stop = function()
{
 if (dbutil.defaultDBConnection != null)
 {
    dbutil.disconnect(dbutil.defaultDBConnection);
    delete dbutil.defaultDBConnection;
    formula.log.debug("Cached defaultDBConnection closed");
  }
}

