//
// Copyright (c) 2013 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//

	javax = Packages.javax

	jPanel1 = new javax.swing.JPanel();
	jLabel1 = new javax.swing.JLabel();
	jTextField1 = new javax.swing.JTextField();
	okButton = new javax.swing.JButton();
	cancelButton = new javax.swing.JButton();

	jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
	jPanel1.setLayout(jPanel1Layout);
	jPanel1Layout.setHorizontalGroup(
			jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
			.addGap(0, 100, 64555)
	);
	jPanel1Layout.setVerticalGroup(
			jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
			.addGap(0, 100, 64555)
	);

	jLabel1.setText("Add Ticket Number");

	jTextField1.setText("");

	okButton.setText("OK");

	cancelButton.setText("Cancel");

	// set Short.MAXVALUE at 64655
	layout = new javax.swing.GroupLayout(jPanel1);
		jPanel1.setLayout(layout);
		layout.setHorizontalGroup(
			layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
			.addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
				.addGap(0, 0, 64655)
				.addComponent(okButton)
				.addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
				.addComponent(cancelButton)
				.addGap(9, 9, 9))
			.addGroup(layout.createSequentialGroup()
				.addContainerGap()
				.addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
					.addComponent(jTextField1, javax.swing.GroupLayout.PREFERRED_SIZE, 320, javax.swing.GroupLayout.PREFERRED_SIZE)
					.addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 71, javax.swing.GroupLayout.PREFERRED_SIZE))
				.addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, 64555))
		);
		layout.setVerticalGroup(
			layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
			.addGroup(layout.createSequentialGroup()
				.addContainerGap()
				.addComponent(jLabel1)
				.addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
				.addComponent(jTextField1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
				.addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
				.addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
					.addComponent(okButton)
					.addComponent(cancelButton))
				.addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, 64555))
		);
		

	frame = new javax.swing.JFrame( 'Hidden' )
	// Create a dialog that holds the jPanel1.
	dialog = new javax.swing.JDialog( frame, 'Insert Ticket', true )
	dialog.getContentPane().add( jPanel1 )
	dialog.pack()
	formula.util.center( dialog )		
		
	// Add a window listener to automatically close.
	dialog.addWindowListener
	(
		new java.awt.event.WindowAdapter()
		{
			windowClosing: function( evt )
			{
			  dialog.setVisible( false )
			}
		}
	)
	// Add an OK listener.
	okButton.addActionListener
	(
		new java.awt.event.ActionListener()
		{
			actionPerformed: function( evt )
			{
				var response = (typeof jTextField1.getText() == 'undefined')?'':jTextField1.getText();
				callback.addTicket(response)
				dialog.setVisible( false )
			}
		}
	)
	// Add a Cancel listener.
	cancelButton.addActionListener
	(
		new java.awt.event.ActionListener()
		{
			actionPerformed: function( evt )
			{
			  callback.cancel();
			  dialog.setVisible( false )
			}
		}
	)

	// Show the dialog
	dialog.setVisible( true )


