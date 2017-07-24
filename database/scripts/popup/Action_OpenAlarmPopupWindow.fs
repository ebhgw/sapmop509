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

//////////////////////////////////////////////////////////////////////////
// @title Open alarm popup window
//
// @define element
// @define alarm
// @param field1 Alarm detail field #1 (optional)
// @param field2 Alarm detail field #2 (optional)
// @param maximumAlarms Maximum alarms to keep in popup, defaults to 100 (optional)
// @param messageExpression Alarm mesage field expression (optional)

// @debug off

// Make some class aliases.
javax = Packages.javax

// Turn on for tracing.
this.debug = false

// Logging.
formula.logCategory = 'Alarm.Notifier'

// Parameters
LIFO = false
MAXIMUM_ALARMS = 100
if( this.maximumAlarms )
   MAXIMUM_ALARMS = this.maximumAlarms
ALARM_FIELD1 = '__field1__'
ALARM_FIELD2 = '__field2__'
MESSAGE_EXPRESSION = ''
if( this.field1 )
    ALARM_FIELD1 = this.field1
if( this.field2 )
    ALARM_FIELD2 = this.field2
if( this.messageExpression )
    MESSAGE_EXPRESSION = new RegExp( messageExpression )

// Alarm description possibles.
var ALARM_DESCRIPTIONS =
[
    'Description',
    'Message',
    'msg',
    'Reason',
]

// Snoozes.
var SNOOZE_TEXT =
[
   '1 Minute',
   '5 Minutes',
   '10 Minutes',
   '30 Minutes',
   'One Hour',
   '8 Hours',
]
var SNOOZE_VALUE =
[
   1000 * 60 * 1,
   1000 * 60 * 5,
   1000 * 60 * 10,
   1000 * 60 * 30,
   1000 * 60 * 60,
   1000 * 60 * 60 * 8,
]

function isValid(xiObj)
{
    var rc  = false;
    var obj = xiObj+'';

    if(obj != 'null' && obj != 'undefined')
    {
        rc = true;
    }

    return rc;
};

function setupDialog()
{
   // BEGIN NETBEANS PASTE (AlarmForm)

   alarmPanel = new javax.swing.JPanel();
   alarmElementIcon = new javax.swing.JLabel();
   alarmElementLabel = new javax.swing.JLabel();
   alarmSeverityIcon = new javax.swing.JLabel();
   alarmDescriptionLabel = new javax.swing.JLabel();
   alarmField1NameLabel = new javax.swing.JLabel();
   alarmField1ValueLabel = new javax.swing.JLabel();
   alarmField2NameLabel = new javax.swing.JLabel();
   alarmField2ValueLabel = new javax.swing.JLabel();

   alarmPanel.setLayout(new java.awt.GridBagLayout());

   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 0;
   gridBagConstraints.gridy = 0;
   gridBagConstraints.gridheight = java.awt.GridBagConstraints.REMAINDER;
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
   gridBagConstraints.weighty = 1.0;
   gridBagConstraints.insets = new java.awt.Insets(4, 4, 0, 4);
   alarmPanel.add(alarmElementIcon, gridBagConstraints);

   alarmElementLabel.setFont(new java.awt.Font("MS Sans Serif", 1, 11));
   alarmElementLabel.setText("Some Element");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 1;
   gridBagConstraints.gridy = 0;
   gridBagConstraints.fill = java.awt.GridBagConstraints.NONE;
   gridBagConstraints.ipadx = 0;
   gridBagConstraints.insets = new java.awt.Insets(4, 4, 0, 4);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
   alarmPanel.add(alarmElementLabel, gridBagConstraints);

   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 1;
   gridBagConstraints.gridy = 1;
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
   gridBagConstraints.insets = new java.awt.Insets(0, 4, 0, 4);
   alarmPanel.add(alarmSeverityIcon, gridBagConstraints);

   alarmDescriptionLabel.setText("<html><head><body><font=\"Sans Serif\" size=2>Here is a long description of some alarm text that, hey, you might be interested in.  Howdy Howdy Howdy.");
   alarmDescriptionLabel.setVerticalAlignment(javax.swing.SwingConstants.TOP);
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 1;
   gridBagConstraints.gridy = 2;
   gridBagConstraints.gridwidth = java.awt.GridBagConstraints.REMAINDER;
   gridBagConstraints.fill = java.awt.GridBagConstraints.HORIZONTAL;
   gridBagConstraints.insets = new java.awt.Insets(0, 4, 4, 4);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
   gridBagConstraints.weightx = 1.0;
   alarmPanel.add(alarmDescriptionLabel, gridBagConstraints);

   alarmField1NameLabel.setText("Field 1:");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 2;
   gridBagConstraints.gridy = 0;
   gridBagConstraints.insets = new java.awt.Insets(4, 0, 0, 0);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHEAST;
   alarmPanel.add(alarmField1NameLabel, gridBagConstraints);

   alarmField1ValueLabel.setText("Value 1");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 3;
   gridBagConstraints.gridy = 0;
   gridBagConstraints.insets = new java.awt.Insets(4, 4, 0, 4);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
   alarmPanel.add(alarmField1ValueLabel, gridBagConstraints);

   alarmField2NameLabel.setText("Some Field 2:");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 2;
   gridBagConstraints.gridy = 1;
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHEAST;
   alarmPanel.add(alarmField2NameLabel, gridBagConstraints);

   alarmField2ValueLabel.setText("Value 2");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 3;
   gridBagConstraints.gridy = 1;
   gridBagConstraints.insets = new java.awt.Insets(0, 4, 0, 4);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
   alarmPanel.add(alarmField2ValueLabel, gridBagConstraints);

   // END NETBEANS PASTE (AlarmForm)

   // Affix the element label size.
   var dim = alarmElementLabel.getPreferredSize()
   dim.width = 300
   alarmElementLabel.setPreferredSize( dim )
   alarmElementLabel.setMinimumSize( dim )
   alarmElementLabel.setMaximumSize( dim )

   // BEGIN NETBEANS PASTE (AlarmNotifyForm)

   clientPanel = new javax.swing.JPanel();
   eventsLabel = new javax.swing.JLabel();
   eventsScrollPane = new javax.swing.JScrollPane();
   eventsList = new javax.swing.JList();
   openButton = new javax.swing.JButton();
   snoozeLabel = new javax.swing.JLabel();
   snoozeComboBox = new javax.swing.JComboBox();
   snoozeButton = new javax.swing.JButton();
   dismissButtonPanel = new javax.swing.JPanel();
   dismissButton = new javax.swing.JButton();
   dismissAllButton = new javax.swing.JButton();
   selectionLabel = new javax.swing.JLabel();

   clientPanel.setLayout(new java.awt.GridBagLayout());

   eventsLabel.setText("The following alarms have occurred that require attention:");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 0;
   gridBagConstraints.gridy = 0;
   gridBagConstraints.gridwidth = 2;
   gridBagConstraints.anchor = java.awt.GridBagConstraints.WEST;
   gridBagConstraints.insets = new java.awt.Insets(8, 8, 0, 0);
   clientPanel.add(eventsLabel, gridBagConstraints);

   eventsScrollPane.setViewportView(eventsList);

   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 0;
   gridBagConstraints.gridy = 1;
   gridBagConstraints.gridwidth = 3;
   gridBagConstraints.fill = java.awt.GridBagConstraints.BOTH;
   gridBagConstraints.ipadx = 200;
   gridBagConstraints.ipady = 100;
   gridBagConstraints.weightx = 1.0;
   gridBagConstraints.weighty = 1.0;
   gridBagConstraints.insets = new java.awt.Insets(4, 8, 0, 8);
   clientPanel.add(eventsScrollPane, gridBagConstraints);

   openButton.setMnemonic('O');
   openButton.setText("Open Element");
   openButton.setEnabled( false )
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 2;
   gridBagConstraints.gridy = 3;
   gridBagConstraints.ipadx = 5;
   gridBagConstraints.ipady = 1;
   gridBagConstraints.insets = new java.awt.Insets(8, 8, 8, 8);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.EAST;
   clientPanel.add(openButton, gridBagConstraints);

   snoozeLabel.setHorizontalAlignment(javax.swing.SwingConstants.LEFT);
   snoozeLabel.setText("Click Snooze to resume notifications in:");
   snoozeLabel.setVerticalAlignment(javax.swing.SwingConstants.BOTTOM);
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 0;
   gridBagConstraints.gridy = 4;
   gridBagConstraints.gridwidth = 2;
   gridBagConstraints.insets = new java.awt.Insets(8, 8, 0, 0);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.SOUTHWEST;
   clientPanel.add(snoozeLabel, gridBagConstraints);

   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 0;
   gridBagConstraints.gridy = 5;
   gridBagConstraints.gridwidth = 2;
   gridBagConstraints.fill = java.awt.GridBagConstraints.HORIZONTAL;
   gridBagConstraints.ipadx = 300;
   gridBagConstraints.insets = new java.awt.Insets(0, 8, 8, 0);
   gridBagConstraints.weightx = 1.0;
   clientPanel.add(snoozeComboBox, gridBagConstraints);

   snoozeButton.setMnemonic('S');
   snoozeButton.setText("Snooze");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 2;
   gridBagConstraints.gridy = 5;
   gridBagConstraints.fill = java.awt.GridBagConstraints.HORIZONTAL;
   gridBagConstraints.ipadx = 5;
   gridBagConstraints.ipady = 1;
   gridBagConstraints.insets = new java.awt.Insets(0, 8, 8, 8);
   clientPanel.add(snoozeButton, gridBagConstraints);

   dismissButtonPanel.setLayout(new java.awt.GridLayout(1, 0, 8, 0));

   dismissButton.setMnemonic('D');
   dismissButton.setText("Dismiss");
   dismissButton.setEnabled( false )
   dismissButtonPanel.add(dismissButton);

   dismissAllButton.setMnemonic('A');
   dismissAllButton.setText("Dismiss All");
   dismissButtonPanel.add(dismissAllButton);

   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 0;
   gridBagConstraints.gridy = 3;
   gridBagConstraints.ipadx = 30;
   gridBagConstraints.ipady = 1;
   gridBagConstraints.insets = new java.awt.Insets(0, 8, 0, 0);
   clientPanel.add(dismissButtonPanel, gridBagConstraints);

   selectionLabel.setText("No selection");
   gridBagConstraints = new java.awt.GridBagConstraints();
   gridBagConstraints.gridx = 0;
   gridBagConstraints.gridy = 2;
   gridBagConstraints.gridwidth = java.awt.GridBagConstraints.REMAINDER;
   gridBagConstraints.insets = new java.awt.Insets(0, 10, 0, 4);
   gridBagConstraints.anchor = java.awt.GridBagConstraints.WEST;
   clientPanel.add(selectionLabel, gridBagConstraints);

   // END NETBEANS PASTE (AlarmNotifyForm)

   // Populate snoozes.
   snoozeComboBox.setModel( new javax.swing.DefaultComboBoxModel( SNOOZE_TEXT ) )

   // Setup renderer
   var labelToRender = new Packages.javax.swing.JLabel()
   var listRenderer = new Packages.javax.swing.ListCellRenderer()
   {
      getListCellRendererComponent: function(
      /* JList */ list,
      /* Object */ value,
      /* int */ index,
      /* boolean */ isSelected,
      /* boolean */ cellHasFocus )
      {
         Result = alarmPanel

         // Determine best text
         var text = '(no alarm details)'
         for( var i = 0; i < ALARM_DESCRIPTIONS.length; ++i )
            if( value.alarm[ ALARM_DESCRIPTIONS[i] ] )
            {
               text = value.alarm[ ALARM_DESCRIPTIONS[i] ]
               break
            }
         text = java.lang.String( text ).replaceAll( ';', ';<br>' )

         // Do we have a text expression?
         if( MESSAGE_EXPRESSION != '' )
         {
            var newText = text.match( MESSAGE_EXPRESSION )
            if( newText != null )
               text = newText
         }

         // Do we have field1, field2?
         var field1 = value.alarm[ ALARM_FIELD1 ]
         var field2 = value.alarm[ ALARM_FIELD2 ]

         // Populate
         alarmElementLabel.setText( value.elementName )
         alarmSeverityIcon.setIcon( value.alarmSeverityIcon )
         alarmElementIcon.setIcon( value.elementConditionIcon )
         alarmDescriptionLabel.setText("<html><head><body><font size=2 face=\"Sans Serif\">" + text )

         // Have field1?
         if( field1 )
         {
            alarmField1NameLabel.setText( ALARM_FIELD1 + ':' )
            alarmField1ValueLabel.setText( field1 )
         }
         else
         {
            alarmField1NameLabel.setText( '' )
            alarmField1ValueLabel.setText( '' )
         }

         // Have field2?
         if( field2 )
         {
            alarmField2NameLabel.setText( ALARM_FIELD2 + ':' )
            alarmField2ValueLabel.setText( field2 )
         }
         else
         {
            alarmField2NameLabel.setText( '' )
            alarmField2ValueLabel.setText( '' )
         }

         // Do color.
         if( isSelected )
         {
            Result.setBackground( list.getSelectionBackground() )
            alarmDescriptionLabel.setForeground( java.awt.Color.white )
            alarmElementLabel.setForeground( java.awt.Color.white )
            alarmField1NameLabel.setForeground( java.awt.Color.white )
            alarmField1ValueLabel.setForeground( java.awt.Color.white )
            alarmField2NameLabel.setForeground( java.awt.Color.white )
            alarmField2ValueLabel.setForeground( java.awt.Color.white )
            Result.setOpaque( true )
         }
         else
         {
            Result.setBackground( list.getBackground() )
            alarmDescriptionLabel.setForeground( list.getForeground() )
            alarmElementLabel.setForeground( list.getForeground() )
            alarmField1NameLabel.setForeground( list.getForeground() )
            alarmField1ValueLabel.setForeground( list.getForeground() )
            alarmField2NameLabel.setForeground( list.getForeground() )
            alarmField2ValueLabel.setForeground( list.getForeground() )
            Result.setOpaque( false )
         }

         return Result
      }
   }
   eventsList.setCellRenderer( listRenderer )
   eventsList.setSelectionMode( javax.swing.ListSelectionModel.MULTIPLE_INTERVAL_SELECTION )
   eventsList.addListSelectionListener
   (
     new Packages.javax.swing.event.ListSelectionListener()
     {
       valueChanged: function( lsEvent )
       {
         if (  eventsList.getSelectedIndex() >= 0 )
         {
            selectionLabel.setText( 'Selected ' + eventsList.getSelectedIndices().length + ' of ' + eventsList.getModel().getSize() )
            openButton.setEnabled( true )
            dismissButton.setEnabled( true )
         }
         else
         {
            selectionLabel.setText( 'No selection' )
            openButton.setEnabled( false )
            dismissButton.setEnabled( false )
         }
       }
     }
   )
   eventsList.addMouseListener
   (
      new java.awt.event.MouseListener()
      {
         mouseClicked: function(e)
         {
            if( e.clickCount == 2 )
            {
               var selValue = eventsList.getSelectedValue();
               if ( null != selValue )
               {
                  var el = selValue.alarm.element
                  if ( null != el )
                  {
                     main.mainFrame.topBrowser.elementOpened( el, true, true )
                  }
               }
            }
         }
      }
   )

   // Setup action listeners.
   var actionListener = new java.awt.event.ActionListener()
   {
      actionPerformed: function( evt )
      {
         if( evt.getSource() == snoozeButton )
         {
            // Set dialog expiration
            var selected = snoozeComboBox.selectedIndex
            state.alarmNotification.expiration = java.lang.System.currentTimeMillis() + SNOOZE_VALUE[ selected ]
            formula.log.info( 'Snoozing for: ' + SNOOZE_VALUE[ selected ] + ' until: ' + java.util.Date( state.alarmNotification.expiration ) )

            // Dismiss the dialog we're attached to
            javax.swing.SwingUtilities.getAncestorOfClass( java.awt.Window, evt.getSource() ).setVisible( false )
         }
         else if( evt.getSource() == dismissAllButton )
         {
            // Set dialog expiration
            state.alarmNotification.alarms = new java.util.ArrayList()
            state.alarmNotification.expiration = 0
            populate()
            formula.log.debug( 'Cleared and dismissed all' )

            // Dismiss the dialog we're attached to
            javax.swing.SwingUtilities.getAncestorOfClass( java.awt.Window, evt.getSource() ).setVisible( false )
         }
         else if( evt.getSource() == dismissButton )
         {
            var indices = eventsList.getSelectedIndices()
            if( indices.length > 0 )
            {
                // Perform modifications in bulk.
                var alarms = new java.util.ArrayList( state.alarmNotification.alarms )
                for( var i = indices.length - 1; i >= 0; --i )
                    alarms.remove( indices[i] )
                state.alarmNotification.alarms = alarms

                populate()
            }
         }
         else if( evt.getSource() == openButton )
         {
            var selValue = eventsList.getSelectedValue();
            if ( null != selValue )
            {
               var el = selValue.alarm.element
               if ( null != el )
               {
                  main.mainFrame.topBrowser.elementOpened( el, true, true )
               }
            }
         }
      }
   }
   snoozeButton.addActionListener( actionListener )
   dismissButton.addActionListener( actionListener )
   dismissAllButton.addActionListener( actionListener )
   openButton.addActionListener( actionListener )
}

function makeAlarmValue( alarm )
{
   return { 
      alarm: alarm,
      alarmSeverityIcon: Packages.com.mosol.Formula.Client.ui.IconCache.getConditionImageIcon( alarm.severity ),
      elementName: alarm.element.name,
      elementConditionIcon: new Packages.javax.swing.ImageIcon( alarm.element.largeIcon )
   }
}

function populate()
{
   var model = new javax.swing.DefaultListModel()
   var size = state.alarmNotification.alarms.size()
   for( var i = 0; i < size; ++i )
      model.addElement( makeAlarmValue( state.alarmNotification.alarms.get( i ) ) )
   state.alarmNotification.eventsList.setModel( model )
   formula.log.debug( 'Pouplated with ' + model.getSize() + ' alarms' )
}

function capAlarms( container )
{
   if( ! this.LIFO || ( this.LIFO == false ) )
   {
      if( formula.log.isDebugEnabled() ) formula.log.debug( 'Doing FIFO aging' )
      while( container.size() > MAXIMUM_ALARMS )
      {
         if( formula.log.isDebugEnabled() ) formula.log.debug( 'Max alarms trimming of : ' + container.size() + ' down to ' + MAXIMUM_ALARMS )
         container.remove( 0 )
      }
   }
   else // LIFO
   {
      if( formula.log.isDebugEnabled() ) formula.log.debug( 'Doing LIFO aging' )
      while( container.size() > MAXIMUM_ALARMS )
      {
         if( formula.log.isDebugEnabled() ) formula.log.debug( 'Max alarms trimming of : ' + container.size() + ' down to ' + MAXIMUM_ALARMS )
         container.remove( container.size() - 1 )
      }
   }
}

function setup()
{
	//quietly exit is 'alarm' parameter is not defined
	try{
		var v = alarm;
	} catch (Exception) { return; }

	if( alarm != null){
	   // Setup notification state data.
	   if( ! state.alarmNotification )
	       state.alarmNotification = {}
	   
	   // Store the notification
	   if( formula.log.isDebugEnabled() )
	      formula.log.debug( 'Alarm notification received for alarm: ' + alarm.id + ' on ' + alarm.element.name + ' (' + alarm.severity + ')' )
	   if( ! state.alarmNotification.alarms )
	      state.alarmNotification.alarms = new java.util.ArrayList()
	   state.alarmNotification.alarms.add( alarm )
	   if ( this.testAlarms && testAlarms.length > 0 )
	   	state.alarmNotification.alarms = new java.util.ArrayList( new java.util.Arrays.asList( testAlarms ) )
	   capAlarms( state.alarmNotification.alarms )
	   if( state.alarmNotification.dialog && state.alarmNotification.dialog.showing )
	   {
	      if( formula.log.isDebugEnabled() )
	         formula.log.debug( 'Adding single alarm ' + alarm.id )
	      javax.swing.SwingUtilities.invokeLater
	      (
	         new java.lang.Runnable()
	         {
	            run: function()
	            {
	               state.alarmNotification.eventsList.getModel().addElement( makeAlarmValue( alarm ) )
	               capAlarms( state.alarmNotification.eventsList.getModel() )
	            }
	         }
	      )
	   }
	   else if( state.alarmNotification.eventsList && state.alarmNotification.dialog && state.alarmNotification.dialog.visible )
	      javax.swing.SwingUtilities.invokeLater
	      (
	         new java.lang.Runnable()
	         {
	            run: function()
	            {
	               populate()
	            }
	         }
	      )
	   
	   // Are we snoozed?
	   if( ! state.alarmNotification.dialog )
	   {
	      // Create dialog, and stick it at the bottom.
	      var title = 'Formula\u00ae'
	      if( this.appFrame && this.appFrame.getTitle && this.appFrame.getTitle() != '' )
	         title = this.appFrame.getTitle()
	      this.dlg = new Packages.javax.swing.JFrame( title )
	      dlg.setIconImage( appFrame.getIconImage() )
	      state.alarmNotification.dialog = dlg
	      setupDialog()
	      dlg.contentPane.add( clientPanel )
	      dlg.pack()
	      dlg.setSize( dlg.getWidth() + 100, dlg.getHeight() + 50 )
	      var screenSize = java.awt.Toolkit.getDefaultToolkit().getScreenSize()
	      var windowSize = dlg.getSize()
	      dlg.setLocation( screenSize.width - windowSize.width,
	                       screenSize.height - windowSize.height - 100 );
	      Packages.com.mosol.util.guix.SwingUtils.registerDefaultAction( snoozeButton )
	      Packages.com.mosol.util.guix.SwingUtils.registerCancelAction( snoozeButton )
	   
	      // Set expiration in 2 seconds.
	      state.alarmNotification.expiration = java.lang.System.currentTimeMillis() + ( 2 * 1000 )
	      state.alarmNotification.eventsList = eventsList
	   }
	   else if( state.alarmNotification.expiration == 0 && ! state.alarmNotification.dialog.showing )
	   {
	      // Set expiration in 2 seconds.
	      state.alarmNotification.expiration = java.lang.System.currentTimeMillis() + ( 2 * 1000 )
	   }
	   
//?? CR24921 if( ! state.alarmNotification.thread )
        if(!isValid(state.alarmNotification.thread))
	   {
	      state.alarmNotification.thread = new java.lang.Thread
	      (
	         new java.lang.Runnable()
	         {
	            run: function()
	            {
	                while( true )
	                {
	                   try
	                   {
	                       java.lang.Thread.sleep( 1*1000 )
	                   }
	                   catch( Exception )
	                   {
	                       break
	                   }
	   
	                   // Something to notify?
	                   if( state.alarmNotification.alarms.size() > 0 )
	                   {
	                      // Did we expire?
	                      formula.log.debug( 'Checking alarm notification' )
	                      if( state.alarmNotification.expiration > 0 && java.lang.System.currentTimeMillis() > state.alarmNotification.expiration )
	                      {
	                         formula.log.info( 'Expiration detected' )
	   
	                         // Show the dialog.
	                         formula.log.info( 'Showing notification dialog' )
	                         var dlg = state.alarmNotification.dialog
	                         javax.swing.SwingUtilities.invokeLater
	                         (
	                            new java.lang.Runnable()
	                            {
	                               run: function()
	                               {
	                                  formula.log.info( 'Populating' )
	                                  populate()
	                                  formula.log.info( 'Setting visible' )
	                                  dlg.setVisible( true )
	                                  dlg.toFront()
	                               }
	                            }
	                         )
	   
	                         // Reset next notification
	                         state.alarmNotification.expiration = 0
	                      }
	                   }
	                }
	            }
	         },
	         "Alarm Notification Thread"
	      )
	      state.alarmNotification.thread.start()
	   }
	}
}

setup()

// @internal Action_OpenAlarmPopupWindow.fs e72ie3f
