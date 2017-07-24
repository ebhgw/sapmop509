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

//////////////////////////////////////////////////////////////////////////////////////////
// Utility functions.

// Make an email address; display name is optional
function makeEmailAddress( email, displayname )
{
    var returnStr = email

    if( arguments[1] != null )
    {
	  if ( email && email.length > 0 && displayname && displayname.length > 0 )
           returnStr = '"' + displayname + '" ' + '<' + email + '>'
    }

    return returnStr 
}

//////////////////////////////////////////////////////////////////////////////////////////
// Message class implementation.
function Message_addRecipient( email, displayname )
{
    this.recipients[ this.recipients.length ] = makeEmailAddress( arguments[0], arguments[1] )
}

//////////////////////////////////////////////////////////////////////////////////////////
// Add an attachment
function Message_addAttachment( filename, displayname )
{
    var attachment = new Object( )
    attachment.filename = filename
    if( arguments[1] != null )
        attachment.displayname = displayname
    else
        attachment.displayname = null
    this.attachments[ this.attachments.length ] = attachment
}

//////////////////////////////////////////////////////////////////////////////////////////
// Actually send the email
function Message_send()
{
	MailcapCommandMap = Packages.javax.activation.MailcapCommandMap
	CommandMap =        Packages.javax.activation.CommandMap
 	
	var mc = CommandMap.getDefaultCommandMap();
    	mc.addMailcap("text/html;; x-java-content-handler=com.sun.mail.handlers.text_html");
   	mc.addMailcap("text/xml;; x-java-content-handler=com.sun.mail.handlers.text_xml");
    	mc.addMailcap("text/plain;; x-java-content-handler=com.sun.mail.handlers.text_plain");
    	mc.addMailcap("multipart/*;; x-java-content-handler=com.sun.mail.handlers.multipart_mixed");
    	mc.addMailcap("message/rfc822;; x-java-content-handler=com.sun.mail.handlers.message_rfc822");
    	CommandMap.setDefaultCommandMap(mc);

	// Double check parameters.
	if( ! this.smtpserver )
		throwError( "Message.send(): no smtpserver specified" )
	if( ! this.sender )
		throwError( "Message.send(): no sender specified" )
	if( ! this.subject )
		throwError( "Message.send(): no subject specified" )
	if( this.recipients.length == 0 )
		throwError( "Message.send(): no recipient specified" )
	
	if( this.debug ) writeln( "Message.send(): loading mail packages " )
	
	// Setup some variables
	var RecipientTypeTO = Packages.javax.mail.Message$RecipientType.TO
	var InternetAddress = Packages.javax.mail.internet.InternetAddress
	var MimeBodyPart = Packages.javax.mail.internet.MimeBodyPart
	var MimeMessage = Packages.javax.mail.internet.MimeMessage
	var MimeMultipart = Packages.javax.mail.internet.MimeMultipart
	
	
	if( this.debug ) writeln( "Message.send(): constructing message with subject '" + this.subject + "'" )
		
	// Create session
	var props = new java.util.Properties()
	props.setProperty( "mail.smtp.host", this.smtpserver )
	var session = Packages.javax.mail.Session.getDefaultInstance( props, null )
	//
	// The default Session Instance is created once and cannot be updated. - If we are trying to update the
	// smtp host - we need to use Session.getInstance which provides a new session for each call.
	//
	if ( ! ( session.getProperty("mail.smtp.host").equals(new java.lang.String(this.smtpserver)) ) )
	{
		session = Packages.javax.mail.Session.getInstance( props, null )
	}

	// Create new mime message
	var message = new MimeMessage( session )
	
	// Set the mail header
	message.setHeader("X-Mailer", "Managed Objects JavaMail Script");
	
	// Subject
	message.setSubject( this.subject )
	
	// From address
	message.setFrom( new InternetAddress(this.sender) )
	
	// Date
	message.setSentDate( new java.util.Date() )
	
	// Content
	var content = new MimeMultipart()
	
	if( this.debug ) writeln( "Message.send(): constructing message body" )
	
	// Body
	var bodypart = new MimeBodyPart( )
	bodypart.setText( this.body )
	content.addBodyPart( bodypart )

	// Attach all attachments
	for( var i = 0 ; i < this.attachments.length ; ++i )
	{
		var attachment = this.attachments[i]
		if( this.debug ) { writeln( "Message.send(): attaching : " + attachment.filename ) }
		var fds = new Packages.javax.activation.FileDataSource( attachment.filename )
		var attach = new MimeBodyPart( )
		attach.setDataHandler( new Packages.javax.activation.DataHandler( fds ) )
		if( attachment.displayname == null )
		{
			var attachfile = new File( attachment.filename )
			attach.setFileName( attachfile.getName( ) )
		}
		else
		{
			attach.setFileName( attachment.displayname )
		}
		content.addBodyPart( attach )
	}

	// Set up reciepients list
	var recipientList = new Array()

	for( var i = 0 ; i < this.recipients.length ; ++i )
	{
		recipientList[ recipientList.length ] = new InternetAddress( this.recipients[i] )
		if( this.debug ) writeln( "Message.send(): sending to: " + this.recipients[i] )
	}
	message.setRecipients( RecipientTypeTO, recipientList )
	
	// Log debug if available
	if( this.debugfile != null )
	{
		writeln( "Message.send(): debug file = ", this.debugfile )
		message.putByteStream( FileOutputStream( this.debugfile ) )
	}
	
	// Set your content
	message.setContent(content)
	
	// Send message
	Packages.javax.mail.Transport.send(message)

}

// The message object.
function Message()
{
   //reinitialize all fields
   this.smtpserver = null
   this.subject = null
   this.body = null
   this.sender = null
   this.recipients = new Array()
   this.attachments = new Array()   
}
Message.prototype.setServer = function( value )
{
   this.smtpserver = value
}
Message.prototype.setSubject = function( value )
{
   this.subject = value
}
Message.prototype.setBody = function( value )
{
   this.body = value
}
Message.prototype.setSender = function( email, displayname )
{
   this.sender = makeEmailAddress( email, displayname )
}
Message.prototype.send = Message_send
Message.prototype.addRecipient = Message_addRecipient
Message.prototype.addAttachment = Message_addAttachment
Message.prototype.debug = false
Message.prototype.smtpserver = null
Message.prototype.sender = null
Message.prototype.subject = null
Message.prototype.body = null
Message.prototype.recipients = new Array()
Message.prototype.attachments = new Array()
Message.prototype.debugfile = null
Message.prototype.displayStatus = false

//////////////////////////////////////////////////////////////////////////////////////////
// A simple send w/o attachments to one recipient.

function sendMessage( smtpserver,
                      subject,
                      sender,
                      body,
                      recipient,
					  debug )
{
   var msg = new Message()
	if( arguments.length > 5 && debug )
	{
		msg.debug = true
		writeln( "sendMessage(): debug is turned on" )
	}
   msg.setServer( smtpserver )
   msg.setSubject( subject )
   msg.setSender( sender )
   msg.setBody( body )
   msg.addRecipient( recipient )
   return msg.send()
}
