/*  Dump data to file
    
        LISPA/ContactCenter/Util/DataReader.fs
        Version 1.0
        E. Bomitali

        Updates:
        Ver    	Author          Description
        1.0    	E. Bomitali    Initial Release
		
	Legge l'elemento

*/

function DataReader () {
    this.inputFilename = null;
    this.reader = null;
}

DataReader.prototype.openFile = function(/* string */ filename) {
	this.inputFilename = filename;
	this.reader = new Packages.java.io.FileReader( filename );
	this.bufferedreader = new Packages.java.io.BufferedReader(this.reader);
}

DataReader.prototype.closeFile = function() {
	this.reader.close();
}

DataReader.prototype.readln = function(/* string */ line) {
	return this.bufferedreader.readLine();
}

// read the content of a file storing it 
DataReader.prototype.getTextFileContent = function() {
	var contents = new Packages.java.lang.StringBuilder();
	var sep = Packages.java.lang.System.getProperty("line.separator");
	var line = null;
	while (( line = this.bufferedreader.readLine()) != null){
          contents.append(line);
          contents.append(sep);
    }
	return contents.toString();
}