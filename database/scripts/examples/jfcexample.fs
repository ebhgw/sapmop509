//
// Copyright (c) 2014 NetIQ Corporation.  All Rights Reserved.
//
// THIS WORK IS SUBJECT TO U.S. AND INTERNATIONAL COPYRIGHT LAWS AND TREATIES.  IT MAY NOT BE USED, COPIED, 
// DISTRIBUTED, DISCLOSED, ADAPTED, PERFORMED, DISPLAYED, COLLECTED, COMPILED, OR LINKED WITHOUT NETIQ'S
// PRIOR WRITTEN CONSENT. USE OR EXPLOITATION OF THIS WORK WITHOUT AUTHORIZATION COULD SUBJECT THE 
// PERPETRATOR TO CRIMINAL AND CIVIL LIABILITY.
//
// NETIQ PROVIDES THE WORK "AS IS," WITHOUT ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING WITHOUT THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT. NETIQ,
// THE AUTHORS OF THE WORK, AND THE OWNERS OF COPYRIGHT IN THE WORK ARE NOT LIABLE FOR ANY CLAIM,
// DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF,
// OR IN CONNECTION WITH THE WORK OR THE USE OR OTHER DEALINGS IN THE WORK.
//

// Forward decleration of "javax", since it isn't a predefined package name for JS
javax = Packages.javax

// BEGIN PASTE FROM NETBEANS

      clientPanel = new javax.swing.JPanel();
      buttonPanel = new javax.swing.JPanel();
      OKButton = new javax.swing.JButton();
      CancelButton = new javax.swing.JButton();
      specifyLabel = new javax.swing.JLabel();
      oldPasswordLabel = new javax.swing.JLabel();
      oldPasswordField = new javax.swing.JPasswordField();
      newPasswordLabel = new javax.swing.JLabel();
      newPasswordField = new javax.swing.JPasswordField();
      newPasswordAgainLabel = new javax.swing.JLabel();
      newPasswordAgainField = new javax.swing.JPasswordField();

      clientPanel.setLayout(new java.awt.GridBagLayout());

      buttonPanel.setLayout(new java.awt.GridLayout(1, 2, 4, 4));

      OKButton.setText("OK");
      buttonPanel.add(OKButton);

      CancelButton.setText("Cancel");
      buttonPanel.add(CancelButton);

      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 0;
      gridBagConstraints.gridy = 7;
      gridBagConstraints.gridwidth = java.awt.GridBagConstraints.REMAINDER;
      gridBagConstraints.ipadx = 30;
      gridBagConstraints.anchor = java.awt.GridBagConstraints.EAST;
      gridBagConstraints.weightx = 1.0;
      gridBagConstraints.insets = new java.awt.Insets(24, 0, 8, 8);
      clientPanel.add(buttonPanel, gridBagConstraints);

      specifyLabel.setText("Enter new password:");
      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 0;
      gridBagConstraints.gridy = 0;
      gridBagConstraints.gridwidth = 4;
      gridBagConstraints.fill = java.awt.GridBagConstraints.BOTH;
      gridBagConstraints.anchor = java.awt.GridBagConstraints.WEST;
      gridBagConstraints.insets = new java.awt.Insets(8, 8, 24, 8);
      clientPanel.add(specifyLabel, gridBagConstraints);

      oldPasswordLabel.setText("Old password:");
      oldPasswordLabel.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 0;
      gridBagConstraints.gridy = 1;
      gridBagConstraints.anchor = java.awt.GridBagConstraints.EAST;
      gridBagConstraints.insets = new java.awt.Insets(4, 24, 0, 8);
      clientPanel.add(oldPasswordLabel, gridBagConstraints);

      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 1;
      gridBagConstraints.gridy = 1;
      gridBagConstraints.gridwidth = 2;
      gridBagConstraints.gridheight = 2;
      gridBagConstraints.fill = java.awt.GridBagConstraints.BOTH;
      gridBagConstraints.ipadx = 80;
      gridBagConstraints.ipady = 4;
      gridBagConstraints.weightx = 1.0;
      gridBagConstraints.insets = new java.awt.Insets(4, 0, 0, 24);
      clientPanel.add(oldPasswordField, gridBagConstraints);

      newPasswordLabel.setText("New password:");
      newPasswordLabel.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 0;
      gridBagConstraints.gridy = 3;
      gridBagConstraints.anchor = java.awt.GridBagConstraints.EAST;
      gridBagConstraints.insets = new java.awt.Insets(4, 24, 0, 8);
      clientPanel.add(newPasswordLabel, gridBagConstraints);

      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 1;
      gridBagConstraints.gridy = 3;
      gridBagConstraints.gridwidth = 2;
      gridBagConstraints.gridheight = 2;
      gridBagConstraints.fill = java.awt.GridBagConstraints.BOTH;
      gridBagConstraints.ipadx = 80;
      gridBagConstraints.ipady = 4;
      gridBagConstraints.weightx = 1.0;
      gridBagConstraints.insets = new java.awt.Insets(4, 0, 0, 24);
      clientPanel.add(newPasswordField, gridBagConstraints);

      newPasswordAgainLabel.setText("New password (again):");
      newPasswordAgainLabel.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 0;
      gridBagConstraints.gridy = 5;
      gridBagConstraints.anchor = java.awt.GridBagConstraints.EAST;
      gridBagConstraints.insets = new java.awt.Insets(4, 24, 0, 8);
      clientPanel.add(newPasswordAgainLabel, gridBagConstraints);

      gridBagConstraints = new java.awt.GridBagConstraints();
      gridBagConstraints.gridx = 1;
      gridBagConstraints.gridy = 5;
      gridBagConstraints.gridwidth = 2;
      gridBagConstraints.gridheight = 2;
      gridBagConstraints.fill = java.awt.GridBagConstraints.BOTH;
      gridBagConstraints.ipadx = 80;
      gridBagConstraints.ipady = 4;
      gridBagConstraints.weightx = 1.0;
      gridBagConstraints.insets = new java.awt.Insets(4, 0, 0, 24);
      clientPanel.add(newPasswordAgainField, gridBagConstraints);

// END PASTE FROM NETBEANS

// Create an owner frame.
frame = new javax.swing.JFrame( 'Hidden' )

// Create a dialog that holds the clientPanel.
dialog = new javax.swing.JDialog( frame, 'Change Password', true )
dialog.getContentPane().add( clientPanel )
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
OKButton.addActionListener
(
   new java.awt.event.ActionListener()
   {
      actionPerformed: function( evt )
      {
         info( 'You pressed OK' )
      }
   }
)

// Add a Cancel listener.
CancelButton.addActionListener
(
   new java.awt.event.ActionListener()
   {
      actionPerformed: function( evt )
      {
         alert( 'You pressed Cancel' )
      }
   }
)

// Show the dialog
dialog.setVisible( true )


