/* file chooser dialog */

	javax = Packages.javax
	javaio = Packages.java.io
	javatxt= Packages.java.text
	javautil = Packages.java.util
	
	data2collect = data2collect + '';
	
	jFileChooser1 = new javax.swing.JFileChooser();
	//jFileChooser1.setFileSelectionMode(JFileChooser.FILES_ONLY );
	// doesn't work jFileChooser1.setCurrentDirectory(new javaio.File("C:\Temp"));
	
	// data2collect set by invokescript
	formatter = new javatxt.SimpleDateFormat("yyyyMMddhhmmss");
	tm = javautil.Calendar.getInstance().getTime();
	sdate = formatter.format(tm);
	
	switch (data2collect) {
    case 'baseline':
        jFileChooser1.setSelectedFile( new javaio.File("baseline-"+sdate+".txt"));
        break;
    case 'servizi':
        jFileChooser1.setSelectedFile( new javaio.File("servizi_acronimi-"+sdate+".txt"));
        break;
	case 'allarmi':
        jFileChooser1.setSelectedFile( new javaio.File("allarmi-"+sdate+".txt"));
        break;
	case 'utenti':
        jFileChooser1.setSelectedFile( new javaio.File("utenti-"+sdate+".csv"));
        break;
    default:
        // donothing
	}

	frame = new javax.swing.JFrame( 'Hidden' )
	// Create a dialog that holds the jPanel1.
	dialog = new javax.swing.JDialog( frame, 'Choose file', true )
	dialog.getContentPane().add( jFileChooser1 )
	
	
	frame.setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

	// javax.swing.GroupLayout
	layout = new javax.swing.GroupLayout(frame.getContentPane());
	frame.getContentPane().setLayout(layout);
	layout.setHorizontalGroup(
		layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
		.addComponent(jFileChooser1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
	);
	layout.setVerticalGroup(
		layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
		.addComponent(jFileChooser1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
	);
	
	dialog.pack()
	formula.util.center( dialog )
	
	result = jFileChooser1.showSaveDialog(dialog);
	
	if (result == javax.swing.JFileChooser.CANCEL_OPTION ) {
		alert('Operation cancelled');
	} else if (result == javax.swing.JFileChooser.APPROVE_OPTION) {
		// returns a file
		selectedFile = jFileChooser1.getSelectedFile();

		// callback get data
		var data = '';
		if (data2collect == 'utenti') {
			data = callback.getUtentiData();
		} else {
			data = callback.readDataFromDb();
		}
		
		load('custom/util/FileWriter.fs');
		
		var myFileWriter = new FileWriter();
		myFileWriter.openFile(selectedFile, false);
		myFileWriter.print(data);
		myFileWriter.closeFile();
		alert('Operation completed');
		
	} else {
		// boh ?
	}
	
