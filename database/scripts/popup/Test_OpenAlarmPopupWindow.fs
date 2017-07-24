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

// Test harness for automation action for open alarm popup window
//
// Usage: fscript -f Test_OpenAlarmPoupupWindow.fs
// 
// @opt -1

// Setup ui.
this.appFrame = new Packages.javax.swing.JFrame()

function getIcon( name )
{
   return Packages.com.mosol.util.gui.AWTUtils.getImage( this.appFrame, '/images/icons/image/large/' + name + '.gif' )
}

// Setup alarm.
var alarm = 
	{
		element: 
		{
			name: 'My Element',
			largeIcon: getIcon( 'applications' ),
			dname: 'myCls=My+Element/some=Adapter/root=Elements'
		},
		Reason: 'Yer SLA is violatin; predicted violation will occur in 30 seconds',
		severity: formula.severities.MAJOR,
		Agreement: 'Bronze',
		Objective: 'Age',
		id: 41
	}

//Setup alarm.
var testAlarms = new Array()
testAlarms[0] =
	{
		element: 
		{
			name: 'My Element',
			largeIcon: getIcon( 'applications' ),
			dname: 'myCls=My+Element/some=Adapter/root=Elements'
		},
		Reason: 'Yer SLA is violatin; predicted violation will occur in 30 seconds',
		severity: formula.severities.MAJOR,
		Agreement: 'Bronze',
		Objective: 'Age',
		id: 42
	}
testAlarms[1] =
	{
	   element: 
	   {
         name: 'My Element with a Really Really Really Really Really (tm) Long Name',
	      largeIcon: getIcon( 'applications' ),
	      dname: 'myCls=My+Element/some=Adapter/root=Elements'
	   },
	   Reason: 'Yer SLA is violatin; predicted violation will occur in 30 seconds',
	   severity: formula.severities.CRITICAL,
	   Agreement: 'Gold',
	   Objective: 'Q',
	   id: 43
	}
testAlarms[2] =
{
   element: 
   {
      name: 'Short',
      largeIcon: getIcon( 'applications' ),
      dname: 'myCls=My+Element/some=Adapter/root=Elements'
   },
   Reason: 'Yer SLA is violatin; predicted violation will occur in 30 seconds',
   severity: formula.severities.MINOR,
   Agreement: 'Platinum',
   Objective: 'Z',
   id: 44
}

field1 = 'Agreement'
field2 = 'Objective'
messageExpression = 'predict.*'
maximumAlarms = 3

// Test
load( 'Action_OpenAlarmPopupWindow' )

// Some test data
var classes = [ 'applications', 'application', 'mgmt_ci', 'mgmt_doia', 'gen_container', 'industry_banking', 'industry_telecom', 'layout_hierarchical' ]
var messages = [ 'Yer SLA is violatin', 'A performance problem was detected', 'Your door is open', 'Please be quiet', 'Your CPU is a 100%', 'Database is full' ]
var agreements = [ 'Gold', 'Silver', 'Bronze', 'Putty', 'Glue', 'Tape', 'Stickums' ]
var objectives = [ 'Availability', 'Transactions', 'Performance', 'Faults', 'Outages', 'Tickets' ]

// Test run post first pop
delete testAlarms
testAlarms = undefined
var test = 100
java.lang.Thread.sleep( 5000 )
while( dlg.isVisible() || ( ! dlg.isVisible() && state.alarmNotification.expiration != 0 ) )
{
   java.lang.Thread.sleep( 1000 )
   if( ( ++test % 2 ) == 0 )
   {
      // Create a new alarm
      var cls = classes[ ( test % classes.length ) ]
      alarm = 
   	{
   		element: 
   		{
   			name: 'My Element: ' + test,
   			largeIcon: getIcon( cls ),
            elementClassName: cls,
   			dname: cls + '=My+Element:+'+(test)+'/some=Adapter/root=Elements'
   		},
   		Reason: messages[ ( test % messages.length ) ] + '; predicted violation will occur in ' + ( test % 45 ) + ' seconds',
   		severity: formula.severities[ ( test % 7 ) ],
   		Agreement: agreements[ ( test % agreements.length ) ],
   		Objective: objectives[ ( test % objectives.length ) ],
   		id: test
   	}

      main()
   }
}



// @internal Test_OpenAlarmPopupWindow.fs 7fjk5e3
