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

//
// Make a connection to the WAREHOUSE database and run the DBSync Oracle stored
// procedure. Once complete, print the synchronization report.
//
// Note:    Requires the path to the Formula.properties file from the
//          config directory.
//
// @internal
///////////////////////////////////////////////////////////////////////////////////

formula.logCategory = 'CacheStats'
var log = formula.log
var textArea = ''

///////////////////////////////////////////////////////////////////////////////////
// Class aliases
///////////////////////////////////////////////////////////////////////////////////
WarehouseCache = Packages.com.mosol.Formula.Server.db.session.warehouse.WarehouseCache;

var cache = WarehouseCache.getInstance();
if ( ! cache )
{
   log.error( 'Cannot get WarehouseCache instance' )
}
else
{
   log.error( 'Cache=' + cache )

   // Window setup
   var frame = new java.awt.Frame( 'Warehouse Cache Statistics' )
   frame.setSize( 600, 400 )
   formula.util.center( frame )

   frame.addWindowListener(
      new java.awt.event.WindowAdapter() 
      {
         windowClosing: function( evt ) { frame.setVisible( false ) }
      }
   )

   textArea = new java.awt.TextArea( "", 40, 70, java.awt.TextArea.SCROLLBARS_VERTICAL_ONLY )
   textArea.setBackground( java.awt.Color.white )
   textArea.setForeground( java.awt.Color.black )

   var stats = cache.getStats()
   textArea.setText( stats )
   log.info( 'Warehouse Cache Statistics\n' + stats )

   frame.add( textArea )
   frame.setVisible( true )
}

// @internal cacheStats.fs 9c3a5ci
