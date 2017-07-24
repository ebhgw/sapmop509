/*  Dump data to file
    
        LISPA/ContactCenter/Util/DataDumper.fs
        Version 1.0
        E. Bomitali

        Updates:
        Ver    	Author          Description
        1.0    	E. Bomitali    Initial Release
		
	Legge l'elemento

*/

function DataDumper () {
    this.outputFilename = null;
    this.writer = null;
}

DataDumper.prototype.openFile = function(/* string */ filename) {
	this.outputFilename = filename;
	// quick way to leave out strange chars   filename.replace(/[^\w]/gi,"");
	this.writer = new Packages.java.io.PrintWriter( new Packages.java.io.FileOutputStream( filename, true ) );
}

DataDumper.prototype.closeFile = function() {
	this.writer.close();
}

DataDumper.prototype.println = function(/* string */ line) {
	this.writer.println(line);
}
