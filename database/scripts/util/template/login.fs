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
// A function that starts the NetIQ Operations Center client w/o showing the main UI,
// and allows for command line interaction with the NetIQ Operations Center server.
//


function login()
{
    // Already have a connection and session?
    if( this.session != null && this.session != undefined )
    {
        writeln( 'Prior connection detected; no login necessary.' )
        return true
    }

    // Get a nice input stream.
    var inStream = this['in']
    if( inStream == undefined )
        inStream = java.lang.System['in']
    var input = new java.io.BufferedReader( new java.io.InputStreamReader( inStream ) )

    writeln( "" )
    writeln( "NetIQ Operations Center(r) Copyright 2010 NetIQ Corporation." )
    writeln( "" )

    // Running under server control?
    var host = 'localhost'
    var port = 80
    var protocol = 'http'
    if( ! this.server )
    {
        // Get the host.
        if( arguments.length >= 1 )
            host = arguments[0]
        else
        {
           write( 'Enter web server host [~WebServerHost~] : ' )
           var host = input.readLine()
           if( host == '' )
               host = '~WebServerHost~'
        }

        // Get the port.
        if( arguments.length >= 2 )
            port = arguments[1]
        else
        {
            write( 'Enter web server port [~WebServerPort~]      : ' )
            var port = input.readLine()
            if( port == '' )
                port = ~WebServerPort~
        }
    }

    // Get the user.
    var user
    if( arguments.length >= 3 )
        user = arguments[2]
    else
    {
        write( 'Enter your NetIQ Operations Center(r) account userid   : ' )
        user = input.readLine()
        if( user == '' )
        {
            writeln( "Userid is required." )
            return false
        }
    }

    // Get the password.
    var password
    if( arguments.length >= 4 )
        password = arguments[3]
    else
    {
        write( 'Enter your NetIQ Operations Center(r) account password : ' )
        password = input.readLine()
        if( password == '' )
        {
            writeln( "Password is required." )
            return false
        }
    }

    // Perform the login.
    this.session = formula.login( host, parseInt( port ), protocol, user, password, 60 )

    // Reload the Managed Objects extension.
    if( this.session != null && this.session != undefined )
    {
        // Reinitialize and return success.
        formula.initFormula()
        return true
    }
    else
        return false
}
