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
// A simple script for sending a mail message from Managed Objects
//

// Load the mail and text processing libraries.
load( "mail/maillib" )
load( "util/text" )

// The main send routine.
function MailElement_send( )
{
   // Designate the recipient from element.

    if( ! this.recipient || this.recipient == null || this.recipient.length == 0 )
    {
       var elementRecipient = makeEmailAddress( element.Email, element.Contact )
       if( ! elementRecipient || elementRecipient.length == null || elementRecipient.length == 0 )
           throwError( "MailElement.send(): no recipient could be derived from element provided." )
       this.recipient = elementRecipient
    }

    // No message text yet?  Provide a default.
    if( this.text == "" )
    {
        // Make a message to send.
        msgtext = "Message from Managed Objects follows:"
        msgtext += "\n\nA problem with element " + this.element.name + " occurred."
        if( this.element.condition )
            this.text += "\n\nThe condition is now " + this.element.condition.value()
    }
	if( this.debug )
		writeln( "MailElement.send(): message text is ok." )

    // Do the message send.
	if( this.debug )
	{
		writeln( "MailElement.send(): doing send to:" )
        writeln( "  server    = ", this.server )
        writeln( "  recipient = ", this.recipient )
        writeln( "  sender    = ", this.sender )
        writeln( "  subject   = ", this.subject )
        writeln( "  element   = ", this.element.name )
	}
    return sendMessage( this.server,
                        this.subject,
                        this.sender,
                        this.text,
                        this.recipient,
						this.debug )
}

function MailElement_setMessageTemplate( templateName )
{
    //
    // If the second argument is here, it is the name of the resource to do
    // template replacement on.  Try evaluating that.
    //
    if( arguments[0] )
    {
        // Make the evaluation to load the text.
        var msgLoaded = getServerDocument( templateName )
        if( msgLoaded )
        {
            if( this.debug )
                writeln( "MailElement.setMessageTemplate(): Got doc: " + templateName + ": " + msgLoaded + ": " + javaTypeOf( msgLoaded ) )

            // Remap the text with replacement parameters.
            this.textExpressionReplace = textExpressionReplace
            this.text = this.textExpressionReplace( msgLoaded )
            delete this.textExpressionReplace

            if( this.debug )
                writeln( this.text )
        }
        else
            throwError( "MailElement.setMessageTemplate(): Could not load template named: " + templateName )
    }
    else
        throwError( "MailElement.setMessageTemplate(): No message template resource was provided." )
}

// The MailElement constructor.
function MailElement( element )
{
    // Check element is here.
    if( ! arguments[0] )
        throwError( "An MailElement must be constructed with an element" )

    // Set the values.
    this.element = element
}

// Set the default properties.
MailElement.prototype.send = MailElement_send
MailElement.prototype.setMessageTemplate = MailElement_setMessageTemplate
MailElement.prototype.text = ""
MailElement.prototype.recipient = null
MailElement.prototype.server = null
MailElement.prototype.subject = "Managed Objects Message"
MailElement.prototype.sender = null
MailElement.prototype.debug = false
