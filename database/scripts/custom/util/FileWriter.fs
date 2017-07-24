/*  Dump data to file
    
        custom/util/FileWriter.fs
        Version 1.0
        E. Bomitali

        Updates:
        Ver    	Author          Description
        1.0    	E. Bomitali    Initial Release
		
	Scrive su file

*/

function FileWriter () {
    this.outputFilename = null;
    this.writer = null;
}

FileWriter.prototype.openFile = function(/* string */ filename, /* boolean */ flgAppend ) {
	if (typeof flgAppend == 'undefined') {
		flgAppend = false;
	}
	this.outputFilename = filename;
	this.writer = new Packages.java.io.PrintWriter( new Packages.java.io.FileOutputStream( filename, flgAppend ) );
}

FileWriter.prototype.closeFile = function() {
	this.writer.close();
}

FileWriter.prototype.println = function(/* string */ line) {
	this.writer.println(line);
}

FileWriter.prototype.print = function(/* string */ str) {
	this.writer.print(str);
}