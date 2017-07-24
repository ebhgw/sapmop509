//
// Copyright (c) 2013 Hogwart srl.  All Rights Reserved.
// 
// Author: E. Bomitali
//

	javax = Packages.javax
	Dimension = Packages.java.awt.Dimension

	jPanel1 = new javax.swing.JPanel();
	jLabel1 = new javax.swing.JLabel();
	jTextField1 = new javax.swing.JTextField();
	jButton1 = new javax.swing.JButton();
	jButton2 = new javax.swing.JButton();

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

	d = jLabel1.getPreferredSize();  
    jLabel1.setPreferredSize(new Dimension(d.width+100,d.height));
	jLabel1.setText("Insert Acronimo");

	jTextField1.setText("");

	jButton1.setText("OK");

	jButton2.setText("Cancel");

	// set Short.MAXVALUE at 64655
	layout = new javax.swing.GroupLayout(jPanel1);
		jPanel1.setLayout(layout);
		layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel1)
                    .addComponent(jTextField1)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                        .addComponent(jButton1)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jButton2, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, 64655)))
                .addContainerGap())
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, 64655)
                .addComponent(jLabel1)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jTextField1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jButton2)
                    .addComponent(jButton1))
                .addContainerGap())
        );
		

	frame = new javax.swing.JFrame( 'Hidden' )
	// Create a dialog that holds the jPanel1.
	dialog = new javax.swing.JDialog( frame, 'Insert Acronimo', true )
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
	jButton1.addActionListener
	(
		new java.awt.event.ActionListener()
		{
			actionPerformed: function( evt )
			{
				var newAcro = jTextField1.getText();
				callback.addAcronimo(newAcro)
				dialog.setVisible( false )
			}
		}
	)
	// Add a Cancel listener.
	jButton2.addActionListener
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


