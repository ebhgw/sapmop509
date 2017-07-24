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

// @debug off

// Operations ini fragement:
//
//  [Export ConfigStore]
//  description=Export ConfigStore...
//  target=dnamematch:.*
//  context=element
//  type=serverscript
//  operation=@util/exportElementConfigStore
//  command=
//  permission=define
//

// Create file in the server home directory.
var filename = java.lang.String( element.name ).replaceAll( '\\s|\\p{Punct}|\\p{Cntrl}', '_' ) + '.config.xml'
var file = new java.io.File( java.lang.System.getProperty( 'formula.home' ), filename ) 

// Adjust to suit.
var exportRelationships = 'true'
var exportedRelated = 'true' 
var propertyPatterns = ''

// Locate server element and delegate operation to it.
var serverElement = formula.Administration.findElement( 'formulaServer=Server' )
serverElement.perform( session, 'Config|ExportSilent', [], [ element.dname, file.getAbsolutePath(), exportRelationships, exportedRelated, propertyPatterns ] )

// Send notification
session.sendMessage( 'Wrote element export file to: ' + file.getAbsolutePath() )


