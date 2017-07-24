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
var file = new java.io.File( java.lang.System.getProperty( 'formula.home' ) + java.io.File.separator + 'export', filename ) 

// Adjust to suit.
var exportRelationships = 'true'
var exportedRelated = 'true' 
var propertyPatterns = ''

// Locate server element and delegate operation to it.
var serverElement = formula.Administration.findElement( 'formulaServer=Server' )
serverElement.perform( session, 'Config|ExportSilent', [], [ element.dname, file.getAbsolutePath(), exportRelationships, exportedRelated, propertyPatterns ] )

// Send notification
session.sendMessage( 'Wrote element export file to: ' + file.getAbsolutePath() )


