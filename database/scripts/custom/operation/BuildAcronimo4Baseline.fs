try  
{
   var vb = new formula.util.ViewBuilder()
   vb.buildFromFile( "C:/NovellOperationsCenter/NOC/database/Acronimi4Baseline.svcconf.xml");
}
catch( Exception )
{
   formula.log.error( 'Could not build from view builder: ' + Exception )
}