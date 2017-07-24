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
// @title Send element information
//
// @define element
// @param sender The sender email address
// @param server The smtp server address
// @param recipient The recipient email address (optional, can use element)

// Load the library
load( 'mail/mailelement' )

// Do mail element.
function mailElementInformation( element, sender, server )
{
    // Make the mail message.
	if( this.debug )
		writeln( "mailElementInformation(): constructing new MailElement instance" )
    var mailElement = new MailElement( element )
    if( this.debug )
        mailElement.debug = true
    mailElement.sender = sender
    mailElement.server = server

    if( this.recipient )
      mailElement.recipient = this.recipient

    // Designate the recipient from element.
    if( ! this.recipient || this.recipient == null || this.recipient.length == 0 )
    {
       var elementRecipient = makeEmailAddress( element.Email, element.Contact )
       if( ! elementRecipient || elementRecipient.length == null || elementRecipient.length == 0 )
           throwError( "MailElement.send(): no recipient could be derived from element provided." )
       this.recipient = elementRecipient
	 mailElement.recipient = this.recipient
    }

	if( this.debug )
		writeln( "MailElement.send(): message recipient is ok." )

	if( this.debug )
		writeln( "mailElementInformation(): setting message template" )
    mailElement.setMessageTemplate( 'mailtemplates/elementmail.template' )
    mailElement.send()
}

// Entry point.
if( this.element && this.sender && this.server )
    mailElementInformation( this.element, this.sender, this.server )
else
    throwError( "Unable to send element information via mail.\nA required script parameter is missing.\n\n" )
