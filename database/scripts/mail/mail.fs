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
// Script for sending a mail message from Managed Objects
//

// Load the mail library.
load( "mail/maillib" )

// The test main routine to send a mail message
function main()
{
    // Set this for debugging; not in an applet!
    var embedded = 0

    // Make the message object
    var msg = new Message()
    if( this.debug )
	{
		writeln( "Script.mail(): debug is on" )
        msg.debug = true
	}
    writeln( "Script.mail(): sending email message" )
    msg.setSubject( "Managed objects mail message" )
    msg.setServer( "smtp.ManagedObjects.com" )
    msg.setSender( "joeuser@ManagedObjects.com", "Joe User" )
    if( this.RecipientEmail != null && this.RecipientDisplayName != null )
    {
        msg.addRecipient( this.RecipientEmail, this.RecipientDisplayName )
        embedded = 1
    }
    else if( this.element != null && this.element.Email != null && this.element.Email != "" )
    {
        msg.addRecipient( this.element.Email, this.element.Contact )
        embedded = 1
    }
    else
        msg.addRecipient( "joeuser@ManagedObjects.com", "Joe User" )
    var msgtext = "" ;
    msgtext += "A message from Managed Objects follows." + "\n\n"
    msgtext += "Time of message: " + Date() + "\n\n"
    if( this.notification != null )
    {
        embedded = 1
        msgtext += "Notification name: " + this.notification.name + "\n\n"
    }
    if( this.event != null )
    {
        embedded = 1
        msgtext += "Event source: " + this.event.getSource() + "\n\n"
    }
    if( this.element != null )
    {
        embedded = 1
        msgtext += "An alarm or condition raised an unacceptible level.\n\n"
        msgtext += "Element: " + this.element.name + "\n\n"
        msgtext += "Element dname: " + this.element.DName + "\n\n"
        msgtext += "Element condition: " + this.element.condition.value() + "\n\n"
    }
    if( this.alarm != null )
    {
        embedded = 1
        msgtext += "Alarm element: " + this.alarm.getElement().getName() + "\n\n"
        msgtext += "Alarm element dname: " + this.alarm.getElement().getDName() + "\n\n"
        msgtext += "Alarm element condition: " + this.alarm.getElement().getCondition().value() + "\n\n"
        msgtext += "Alarm condition: " + this.alarm.getCondition().value() + "\n\n"
    }

    // Some testing.
    if( ! embedded )
    {
        msgtext += "Check out the attachments for the FormulaScript scripts that sent this message.\n"
        msg.addRecipient( "joeuser@ManagedObjects.com", "Joe User" )
        msg.addAttachment( "mail.fs" , "Mail.fs.txt" )
        msg.addAttachment( "maillib.fs" , null )
        msg.debugfile = 'out.eml'
    }

    // Set the message text.
    msg.setBody( msgtext )

    // Send the message.
    msg.send()
}

main()
