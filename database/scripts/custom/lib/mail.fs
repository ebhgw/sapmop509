//////////////////////////////////////////////////////////////////////////////////////////
// Script for sending a mail message using Novell Operations Center mail libraries
//
// Tight aumentation module pattern (see http://www.adequatelygood.com/JavaScript-Module-Pattern-In-Depth.html)

//////////////////////////////////////////////////////////////////////////////////////////
// Utility functions.
load('mail/maillib.fs');

var Message = (function (mgmod) {

    var _logger = Packages.org.apache.log4j.Logger.getLogger('fs.mail');

    mgmod.prototype.auth_send = function (smtp_auth_user, smtp_auth_pass) {
        // copy Message_send but the transport
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
            throw( "Message.auth_send(): no smtpserver specified" )
        if( ! this.sender )
            throw ( "Message.auth_send(): no sender specified" )
        if( ! this.subject )
            throw ( "Message.auth_send(): no subject specified" )
        if( this.recipients.length == 0 )
            throw ( "Message.auth_send(): no recipient specified" )

        _logger.debug("Message.auth_send(): loading mail packages ");
        // Setup some variables
        var RecipientTypeTO = Packages.javax.mail.Message$RecipientType.TO
        var InternetAddress = Packages.javax.mail.internet.InternetAddress
        var MimeBodyPart = Packages.javax.mail.internet.MimeBodyPart
        var MimeMessage = Packages.javax.mail.internet.MimeMessage
        var MimeMultipart = Packages.javax.mail.internet.MimeMultipart

        _logger.debug("Message.auth_send(): constructing message with subject '" + this.subject + "'");

		var transport = null;
		var mail_session = null;
        try {
        // Create mail_session
        var props = new java.util.Properties()
        props.setProperty( "mail.smtp.host", this.smtpserver )
        // auth provided with transport instead of Authenticator
        var mail_session = Packages.javax.mail.Session.getInstance( props, null );
        // Create new mime message
        var message = new MimeMessage( mail_session )
        // Set the mail header
        message.setHeader("X-Mailer", "Operations Center JavaMail Script");

        // Subject
        message.setSubject( this.subject )

        // From address
        message.setFrom( new InternetAddress(this.sender) )

        // Date
        message.setSentDate( new java.util.Date() )

        // Content
        var content = new MimeMultipart()

        _logger.debug( "Message.send(): constructing message body" );

        // Body
        var bodypart = new MimeBodyPart( )
        bodypart.setText( this.body )
        content.addBodyPart( bodypart )

        // Attach all attachments
        for( var i = 0 ; i < this.attachments.length ; ++i )
        {
            var attachment = this.attachments[i]
            _logger.debug( "Message.send(): attaching : " + attachment.filename )
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
            _logger.debug( "Message.send(): sending to: " + this.recipients[i] );
        }
        message.setRecipients( RecipientTypeTO, recipientList )

        // Log debug if available
        if( this.debugfile != null )
        {
            writeln( "Message.send(): debug file = ", this.debugfile )
            message.putByteStream( FileOutputStream( this.debugfile ) )
        }

        // Set your content
        message.setContent(content);

        // Send message
        var transport = mail_session.getTransport("smtp");

        transport.connect(smtp_auth_user,smtp_auth_pass);
        transport.send(message);
		_logger.debug('Sending e-mail message to ' + recipientList.join(','));
        } catch (exc) {
            _logger.error("Message.auth_send error " + exc);
        } finally {
			if (transport !== null)
				transport.close();
        }

    }

    return Message;
})(Message);
