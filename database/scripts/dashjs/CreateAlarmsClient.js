var myFormPanel = Ext.create('Ext.form.Panel', {

    bodyPadding: 5,

    width: 350,
	
	autoscroll: true,


    // The form will submit an AJAX request to this URL when submitted

    url: 'save-form.php',


    // Fields will be arranged vertically, stretched to full width

    layout: 'anchor',

    defaults: {

        anchor: '100%'

    },


    // The fields

    defaultType: 'textfield',

    items: [{

        fieldLabel: 'Alarm Adapter',

        name: 'adapter',

        allowBlank: false

    },{

        fieldLabel: 'Alarm Condition',

        name: 'condition',

        allowBlank: false

    },{

        fieldLabel: 'Class',

        name: 'class',

        allowBlank: false

    },{

        fieldLabel: 'Summary',

        name: 'summary',

        allowBlank: false

    },{

        fieldLabel: 'Description',

        name: 'condition',

        allowBlank: false

    }],


    // Reset and Submit buttons

    buttons: [{

        text: 'Reset',

        handler: function() {

            this.up('form').getForm().reset();

        }

    }, {

        text: 'Submit',

        formBind: true, //only enabled once the form is valid

        disabled: true,

        handler: function() {

            var form = this.up('form').getForm();

            if (form.isValid()) {
		
                var flds = form.getFields();
                var change = flds.get(0).getValue();
				var newValue = change.replace(/ /g, '+');
				var adapterName = newValue;	
				var param = adapterName + ";" + flds.get(1).getValue()+ ";"+ flds.get(2).getValue() + ";"+ flds.get(3).getValue() +";"+ flds.get(4).getValue();
                Ext.Msg.alert('Alarms Properties Have Been Submitted');
	
				NOCOperations.perform({identity:identity, alarmId:alarmId, name:'CreateAlarmsOperation', opParam:param});


            }

        }

    }]

});


Ext.create('Ext.window.Window', {

  title: 'Entering Values for ' + element.name,

  height: 250,

  width: 350,

  layout: 'fit',

  items: myFormPanel


}).show();