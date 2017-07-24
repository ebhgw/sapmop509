/*
  Script ResultSetHndlr.fs
  
  Author: E. Bomitali - Hogwart
  
  Manipulate a ResultSet, 
  
*/


RsHndlr = function(rs) {
	this.rs = rs;
	this.md = rs.getMetaData();;
	this.colCount = this.md.getColumnCount();
	this.processed = 0;
}

RsHndlr.prototype.nextRow = function() {
	formula.log.debug('next on hndlr');
	return this.rs.next();
}

// Return a field as a String
RsHndlr.prototype.getField = function (fieldName) {
	return this.rs.getString(fieldName);
}

// Return a row as an object 
// with column names as properties 
// and column values as values for the properties
RsHndlr.prototype.getRow = function () {

	try {

		var i = 0, colname = "", colvalue = "";
		// var colType;
		var row; 

		row = new Object();
		for (i = 1; i <= this.colCount; i++)
		{
			// colType = md.getColumnTypeName(i)
			colname = this.md.getColumnName(i);
			// posix regex for non printable characters
			colvalue = this.rs.getString(i).replaceAll("\\p{C}","");
			row[colname] = colvalue;
			//logger.info("Column " + colname + " : " + colvalue);
		}
		//logger.info("Loaded " + recCount + " : " + row);
		//logger.info("Processing " + recCount + " record");
		
	} catch (excp) {
		formula.log.error("Exception in xtractResultSet :" + excp);
	}
	
	this.processed++;
	return row;
}

