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


//////////////////////////////////////////////////////////////////////////////////////////
// Format alarm property information into a string. Mechanism to get alarm properties 
// differs by client-side or server-side automation. For now, handle the differences
// in-line. May break out to separate cases later.

function getAlarmProperties(
                     return_max_length,         // max length of return string; [...Truncated...] if too long
                     property_max_length,       // max length of property name strings; truncated on left if too long
                     property_justify_string,   // string used to right justify property names; should be property_max_length in length or greater
                     value_max_length,          // max length of property value strings; [...] if too long
                     begin_string,              // prefix each property with this string
                     indent_string,             // indent string before each property
                     middle_string,             // delimits property names and values
                     end_string,                // after each property value
                     no_alarms_string)          // if there is no alarm information; i.e. a condition change doesn't have assoc. alarm(s)
{
   // Alarm severity map.
   var severities =
   [
      'UNKNOWN',
      'CRITICAL',
      'MAJOR',
      'MINOR',
      'INFORMATIONAL',
      'OK',
      'INITIAL',
      'IDLE',
      'ACTIVE',
      'BUSY',
   ]

   var propstr = "";    // the end result string
   try {
      // Check if there are alarm(s)
      if ( this.alarm != undefined || this.alarms != undefined ) {

         // Shortcuts
         var pml = property_max_length;
         var pjs = property_justify_string;
         var indent_L1_string = begin_string + indent_string;
         
         // Alarm info could come in on "alarm" or "alarms" object; unify these as newalarms
         var newalarms = new Array();
         if ( this.alarms != undefined )
            newalarms = this.alarms;
         if ( this.alarm != undefined )
            newalarms[ newalarms.length ] = this.alarm;  
		//Using TreeSet to list the properties of the alarm in the debug mode ,as sorted order gives better predictability
		var properties = new java.util.TreeSet();
         // Build alarms string
         
         // For each alarm
         for ( var alarmcount = 0; alarmcount < newalarms.length &&
                     propstr.length < return_max_length; alarmcount++ ) {
            alarm = newalarms[alarmcount];
            propstr += begin_string + (pjs+"Alarm Number").slice(-pml) + middle_string + (alarmcount+1) + end_string;
            propstr += begin_string + (pjs+"Alarm ID").slice(-pml) + middle_string + alarm.ID + end_string;
            propstr += begin_string + (pjs+"Alarm Severity").slice(-pml) + middle_string + severities[ alarm.severity.value() ] + end_string;
            propstr += begin_string + (pjs+"Alarm Element").slice(-pml) + middle_string + alarm.element.name + end_string;
			
			if( this.debug ){
			
			for( var property in alarm.properties )
				properties.add( property )

			// Grab all the alarm properties.
				for( var e = properties.iterator(); e.hasNext(); )
				{
					var property = e.next()
					 formula.log.debug( "Alarm." + property + ": " + this.alarm[ property ]);
				}
				formula.log.debug("\n");
				properties.clear();
			}
            // For each property in the alarm
            for( var prop in alarm.properties ) {
               if( propstr.length >= return_max_length )
                  break;
               
               // Get the alarm property value.
               var propvalue = "" + alarm[ prop ]

               // Replace newlines with a \ \ delimiter
               propvalue = propvalue.replace( /\n/g, "\\ \\");
               // Truncate the property value string if it is too long
               if (propvalue.length > value_max_length) {
                  propvalue = propvalue.substring(0, value_max_length)+"[...]";
               }
               propstr += indent_L1_string + (pjs+prop).slice(-pml) + middle_string + propvalue + end_string;
            }
            propstr += "\n"; // add a newline between alarms
         }
   
      }
   }
   catch ( Exception ) {
      propstr = "Runtime exception getting alarm information: " + Exception;
      Packages.org.apache.log4j.Category.getInstance( 'Automation.Mail' ).warn( 'Exception getting alarm information', Exception )
   }
   
   // If no alarm info then return a message
   if ( propstr.equals("") ) {
      propstr = no_alarms_string;
   }

   // If the entire return string is too long, then truncate it.
   if ( propstr.length > return_max_length ) {
      propstr = propstr.substring(0,return_max_length) + "[...Truncated...]";
   }

   if( this.debug )
   {
      writeln( '[MAIL BEGIN]' )
      writeln( propstr )
      writeln( '[MAIL END]' )
   }
   
   return propstr;
}

