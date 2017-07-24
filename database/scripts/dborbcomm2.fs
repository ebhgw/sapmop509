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


var _quiet=false;

if (this.quiet && this.quiet=="true") {
   _quiet=true;
}

var _cacheorbsocketlist=false;

if (this.cacheorbsocketlist && this.cacheorbsocketlist=="true") {
   _cacheorbsocketlist=true;
}


function INITorb(OrbServer, ORBport ) {
     sendcommand(OrbServer, ORBport, "INIT" );
}


function EXITorb(OrbServer, ORBport ) {
     sendcommand(OrbServer, ORBport, "EXIT" );
}

function closeorbsocketlist() {

   var mlogq = Packages.org.apache.log4j.Category.getInstance( "dborbcomm2.closeorbsocketlist" )

    if (state.dborbcommlist) {
      for (var e in state.dborbcommlist) {
	  mlogq.info( "close " + e );
	  try {
	    state.dborbcommlistbw[e].close();
	  } catch (Exception) {
	    mlogq.error( "problem closing bw " + e +  " " + Exception );
	  }

	  try {
	    state.dborbcommlist[e].close();
	  } catch (Exception) {
	    mlogq.error( "problem closing  " + e +  " " + Exception );
	  }

      }
    } else {
    }

}

function sendcommand(OrbServer, ORBport, commandToSend ) {
   var mykey = "dborbcomm2." + OrbServer + "." + ORBport;
   var mlogq = Packages.org.apache.log4j.Category.getInstance( mykey )
   var retc=false;
   var mySoc=null;
   try {
	 if( OrbServer )
	 {
	    if( ORBport )
	    {
	       if( commandToSend )
	       {

		  if (_cacheorbsocketlist || !state.dborbcommlist) {
		    state.dborbcommlist= new Array();
		    state.dborbcommlistbw= new Array();
		  } else {
		  }
		  if (_cacheorbsocketlist && state.dborbcommlist) {
		     if (state.dborbcommlist[mykey] && state.dborbcommlist[mykey]!=null) {
			  mlogq.debug("Using Connection to server[" + OrbServer + "] on port[" + ORBport + "]" );
			  mySoc=state.dborbcommlist[mykey];
		     } else {
 		          mlogq.debug("Connecting to SERVER[" + OrbServer + "] on port[" + ORBport + "]" );
		          // Open the socket
		          mySoc = new java.net.Socket( OrbServer, ORBport);
		          state.dborbcommlist[mykey]=mySoc;
                     }
		  } else {
		      mlogq.debug("Connecting to server[" + OrbServer + "] on port[" + ORBport + "]" );
		      // Open the socket
		      mySoc = new java.net.Socket( OrbServer, ORBport);
		  }


		  var bw = null;

		  if (_cacheorbsocketlist && state.dborbcommlist) {
		     if (state.dborbcommlistbw[mykey] && state.dborbcommlistbw[mykey]!=null) {
		      	  mlogq.debug("Connecting to SOCKET... bw" );
			  bw=state.dborbcommlistbw[mykey];
		     } else {
		      	  mlogq.debug("Connecting to SOCKET bw" );
		          // Open the socket

		  	  bw = new java.io.BufferedWriter(new java.io.OutputStreamWriter( mySoc.getOutputStream() ) );
		          state.dborbcommlistbw[mykey]=bw;
                     }
		  } else {
		      mlogq.debug("Connecting to socket bw" );
		      // Open the socket
		      bw = new java.io.BufferedWriter(new java.io.OutputStreamWriter( mySoc.getOutputStream() ) );
		  }

		  // Write the data and flush, flush, flush
		 try {

		      bw.write(commandToSend);
		      bw.flush();
		      retc=true;

		 } catch( Exception ) {
		      mlogq.info( "problem sending to " +OrbServer+":"+ORBport +  " " + Exception );
		      _cacheorbsocketlist=false;

 	              state.dborbcommlistbw[mykey]=null;
		      state.dborbcommlist[mykey]=null;
		 }


		 try {


		  if (_cacheorbsocketlist) {
		  } else {
		    // close the socket
		    mlogq.debug( "attempting close for bw " +OrbServer+":"+ORBport );
		    bw.close();
		    mlogq.debug( "closed for bw " +OrbServer+":"+ORBport );
		  }


		 } catch( Exception ) {
		    mlogq.warn( "closed for bw " +OrbServer+":"+ORBport + ' ' + Exception);
		 }

		 try {
		  if (_cacheorbsocketlist) {
		  } else {
		    // close the socket
		    mlogq.debug( "attempting close for " +OrbServer+":"+ORBport );
		    mySoc.close();
		    mlogq.debug( "closed for " +OrbServer+":"+ORBport );
		  }
		 } catch( Exception ) {
		    mlogq.warn( "socket closed " +OrbServer+":"+ORBport + ' ' + Exception);
		 }


		  if (!_quiet)
		    mlogq.info( "Sent " + commandToSend + " to [" + OrbServer + "] on port[" + ORBport + "]"  );


	       }else{
		  mlogq.warn( "You didn't set 'commandToSend' with the data to send to the ORB" );
	       }

	    }else{
	       mlogq.warn( "You must set the port to talk to the Orb" );
	    }
	 }else{
	    mlogq.warn( "You must set the Server... the server where the Orb is running" );
	 }
    } catch( Exception ) {
	mlogq.info( "Problem Sending To " +OrbServer+":"+ORBport +  " " + Exception );
    }

   return retc;

}


