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

// @opt -1 // Don't optimize; some compilation errors ensue!
//
// @debug off
//
// viewbuilderExporter.fs
// Created By: Matthew Murray
//        Managed Objects
// Date: 9/6/2001
// Version 1.0 (Tested on 2.0.3 SR6, 2.0.7, 2.10)
//
//
// Purpose
//    This script will export the Organizations at and below the active element
//    for the right-click
//
// Notes
//    This script places the output into the ManagedObjects/bin directory.
//    The filename is viewbuilderout.xml
//    This script places a CSV output file also into the ManagedObjects/bin directory.
//    as viewbuilderout.csv
//    No import exists for the CSV format - though this script contains SOME of the
//    required code to go from CSV to XML for importing.
//
// Additional Notes
//
// Limitations
//    Images are not packaged up by this script.  Only the viewbuilder XML is created.
//    Associated drawings are not packaged up by this script.  Only the viewbuilder XML is created.
//
// Required Changes
//    An serverscript Operation must be created to use this script
//
// Optional Changes
//
// Implementation
//    1) copy viewbuilderexporter.fs to ManagedObjects/database/scripts/util
//    2) Within Managed Objects Browser go to Administration, Server, Operation Definition
//    3) right-click on Operation Definition and choose "Create Operation"
//    4) Use the following settings...
//       Name: Viewbuilderexporter
//       Menu Text: Viewbuilder|Export
//       Context: Element
//       Match by: Distinguished name (root=Organizations)
//       Permission: Define (any permission is accecptable)
//       Type: Prompt
//       Operation: @util/viewbuilderexporter.fs
//       Prompt Title: ViewBuilder Export
//       Prompt 1: Enter destination file name to export:
//    5) right-click on an element and choose "Export ViewBuilder File...".  A dialog will appear
//       prompting you for a file name.  If you enter blank, the file is written to a default
//       location.  If you enter any other valid path (on the server), the file is written there.
//       If the file extension you provide is "csv", then this script will write a .CSV file,
//       suitable for import into any spreadsheet application.
//
// Expected Results
//     A file is created in the ManagedObjects/bin directory of the name
//      viewbuilderout.xml (and viewbuilderout.csv)
//     This file can be imported into any Managed Objects system.
//
//
//  Sample Operations.ini definition:
//
//  [Export ViewBuilder]
//  description=Export ViewBuilder File...
//  context=element
//  target=dname:root=Organizations
//  permission=view
//  type=prompt
//  operation=@util/viewbuilderexporter
//  title=ViewBuilder Export
//  prompt1=Enter destination file name to export:
//  multiline1=false
//


// Setup our log utility
formula.logCategory = "Viewbuilder.ViewbuilderExporter"
var log = formula.log
function mylog(text2log) {
    log.info(text2log);
}
function mylogerror(text2log) {
    log.error(text2log);
}

/////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////


    // Some constants
    var indentstr = "\t";
    var ORGroot = "root=Organizations";
    var ViewBuilderbasename="viewbuilderout";
    var ViewBuilderxml=ViewBuilderbasename + ".xml";
    var ViewBuildercsv=ViewBuilderbasename + ".csv";
    var quote='"';

    // Data output destination
    var outputFile


//      subscript is the value to the XML
//      data is the property from Managed Objects
//        - this allows a name change

    // things with good stuff and NEEDED for an org
    formulaOrgProperties =
    [
       "name",
       "class",
       "Company",
       "Contact",
       "Pager",
       "Phone",
       "Graphic",
       "Email",
       "Fax",
       "Algorithm",
       "AlgorithmParameters",
       "DisplaySourceElements",
       "Script",
       "Matches",
       "Children",
    ]

    formulaPropertiesTranslationTable =
    {
       Children: 'relate'
    }

////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

function encodeXML(infield) {
   var newfield = formula.util.encodeXML(infield);
   return newfield;
}

function encode(infield) {
   var newfield = formula.util.encodeURL(infield);
   return newfield;
}

function decode(infield) {
   var newfield = formula.util.decodeURL(infield);
   return newfield;
}


/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

function translate(varname) {

   // Lookup via translation table first
   var translated
   if( ( translated = formulaPropertiesTranslationTable[varname] ) )
      return translated

   // Otherwise, just remove uppercase first char
   else
   {
      varname = String( varname )
      return varname[0].toLowerCase() + varname.substring( 1 )
   }
}

function getVal(ele, sub, mode) {
  var tmp;

  try {
    if (sub == 'Children' ) {
        var children = ele[sub]
        tmp="";
        var i = 1;
        for (var ff = 0; ff < children.length; ++ff) {
            if (i==0) {
              if (mode == 1) {
                tmp+="</relate>";
                tmp+="<relate>";
              } else
              if (mode == 2) {
                tmp+=",";
              }
            }
            i=0;
            tmp+=encodeXML(children[ff]);
        }
    } else
    if (sub == 'DisplaySourceElements' ) {
       tmp=ele[sub]; //
       if (tmp == true) {
       } else {
           tmp="false"; //
       }
    } else
    if (sub.equals("class")) {
       tmp=ele.getElementClass().getName()
    } else
    if (sub.equals("drawing")) {
        var filename = formula.util.nameToFile(ele.name) + ".drawing";
        tmp=filename;
    } else {
         if (ele[sub] != null) {
             tmp=encodeXML(ele[sub]);
         } else {
             tmp=ele[sub]; //
         }
    }
  } catch( Exception ) {
      mylogerror("Error processing getval " + Exception);
  }

  return tmp;
}


function writeOneElement2( writer, itemE, indent, mode ) {
  try {
      var elementhead="<element>";
      var elementtail="</element>";
      var foundsomething = 0;
      // drive the loop from the data as only the filled items are important
      for (var e in itemE) {
          if (foundsomething == 0) {
              foundsomething = 1;
          } else {
          }
          if (itemE[e] == "") {
            // skip the(any) empty vals
          } else {
              if (itemE[e].length == 0) {
                    writeOneElement2( writer, itemE[e].toString(), indent + indentstr, mode );
              } else {
                    writer.print(indent + "<" + e  + ">");
                    writer.print(itemE[e].toString());
                    writer.println("</" + e + ">");
              }
          }
      }
      if (foundsomething == 1) {
      } else {
      }
  } catch (Exception){
      mylogerror("Error processing writeOneElement2 " + Exception);
      throw Exception
  }
  return foundsomething;
}

function writeOneElementCSV( writer, itemE, indent, mode, parentdname ) {
  var comma=",";
  try {
      writer.print(quote);
      writer.print(parentdname);
      writer.print(quote);
      // drive the loop from the predefined array so the items are written in the orig order
      for(var i = 0; i<formulaOrgProperties.length; i++ ) {
          writer.print(comma);
          writer.print(quote);
          if (itemE[translate(formulaOrgProperties[i])]==null) {
            writer.print("");
          } else {
            writer.print(itemE[translate(formulaOrgProperties[i])]);
          }
          writer.print(quote);
      }
      writer.println(" ");

  } catch (Exception){
      mylogerror("Error processing writepar " + Exception);
      throw Exception
  }
  return;
}

function writechild( mywriter, ele, indent, mode ) {
  try {
      var domore=0;
      try {
        if ( ele.dname != "" && !(ele.path.equals(removeLeftOrg(ele.dname)))) {
            domore=1;
        }
      } catch (Exception){
          domore=2;
      }
      if (domore > 0) {
          var nvarray = new Array();
          for (var i = 0; domore == 1 && i < formulaOrgProperties.length; ++i) {
              var tmp = getVal(ele, formulaOrgProperties[i], mode)
              var varname = translate(formulaOrgProperties[i])
              if (tmp == null || tmp == undefined  || tmp == "") {
                  if (mode == 2) {
                    nvarray[varname]=""; //
                  }
              } else {
                   nvarray[varname]=tmp; //
              }
          }

          // check for script / expression / children
          if ((nvarray['script'] == null) || (nvarray['script'] == "")) {
          } else {
              nvarray['matches'] = "";
              nvarray['relate'] = "";
          }
          if ((nvarray['matches'] == null) || (nvarray['matches'] == "")) {
          } else {
              nvarray['relate'] = "";
          }
          if ((nvarray['relate'] == null) || (nvarray['relate'] == "")) {
          } else {
          }

          if (mode == 1) {
              writeOneElement2( mywriter, nvarray, indent, mode);
          } else
          if (mode == 2) {
              writeOneElementCSV( mywriter, nvarray, indent, mode, removeLeftOrg(ele.dname));
          }

          // reallocate/detroy the old array as the next time the items availiable may be different and we dont want
          // trash from a previous looping
          var nvarray = new Array();
      }
  } catch (Exception){
      mylogerror("Error processing writechild " + Exception);
      throw Exception
  }
  return null;
}

function writepar( mywriter, myelement, indent, mode ) {
  var numger=0;
  try {
    var elementhead="<element>";
    var elementtail="</element>";
    var Result = true;

    var initorg=myelement.dname;
    if (initorg == null) {
    } else {
        var r=removeRightOrg(initorg);
        var r1=returnRightOrg(initorg);
        var checkforstop=r1;

        var almostinitorg=removeLeftOrg(initorg);
        var xvarray = new Array();
        var nvarray = new Array();
        while (checkforstop != almostinitorg) {
            numger++;
            checkforstop= returnRightOrg(r) + "/" + checkforstop;
            if (mode == 1) {
               mywriter.println(indent + elementhead);
               mywriter.print(indent + indentstr + "<class>");
               mywriter.print("org");
               mywriter.println("</class>");

               mywriter.print(indent + indentstr + "<name>");
               mywriter.print(decode(getOrgName(returnRightOrg(r))));
               mywriter.println("</name>");
            } else
            if (mode == 2) {
                xvarray['class']="org";
                xvarray['name']=decode(getOrgName(returnRightOrg(r)));
                writeOneElementCSV( mywriter, xvarray, indent, mode, r );
            }
            r=removeRightOrg(r);
            r1=returnRightOrg(r);
            indent+=indentstr;
        }
    }
  } catch (Exception){
      mylogerror("Error processing writepar " + Exception);
      throw Exception
  }
  return numger;
}

function writeparents( mywriter, myelement, indent, mode ) {
    var retc=writepar( mywriter, myelement, indent, mode );
    return retc;
}


function writeOrgs( mywriter, myelement, indent, mode ) {
  var Result = true;
  try {
    var elementhead="<element>";
    var elementtail="</element>";
    var children = myelement.children;
    var nextindent = indent;
    if (mode == 1 && (myelement.dname != ORGroot)) {
      mywriter.println(indent + elementhead);
      nextindent = indent + indentstr;
    }
    if (children.length == 0) {
        writechild( mywriter, myelement, nextindent, mode );
    } else {
        writechild( mywriter, myelement, nextindent, mode );
        for (var e = 0; e < children.length; ++e) {
            writeOrgs( mywriter, children[e], nextindent, mode );
        }
    }

    if (mode == 1 && (myelement.dname != ORGroot)) {
      mywriter.println(indent + elementtail);
    }
  } catch (Exception){
      mylogerror("Error processing writeOrgs " + Exception);
      throw Exception
  }
  return Result;
}


/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

function writetheviewbuilder( filename, mode ) {
  var comma=",";
  outputFile = new java.io.File( filename )
  var file = outputFile
  mylog( 'Writing XML to ' + outputFile.absolutePath )
  var writer = new java.io.PrintWriter( new java.io.FileOutputStream( file, false ) );
  if (mode == 1) {
    var viewbuilderhead='<views destroy="no">';
    var viewbuildertreehead='   <tree start_at="root=Organizations">';
    writer.println( viewbuilderhead );
    writer.println( viewbuildertreehead );
  } else
  if (mode == 2) {
      var foundsomething = 0;
      writer.print(quote);
      writer.print("parent");
      writer.print(quote);

      for(var i = 0; i<formulaOrgProperties.length; i++ ) {
          writer.print(comma);
          writer.print( quote );
          writer.print( translate(formulaOrgProperties[i]) );
          writer.print( quote );
      }
      writer.println( " " );
  }

  return writer;
}


function returnLeftOrg(rootnamein) {
  var rootnameout=rootnamein;
  var y=rootnameout.indexOf("/");
  if (y != -1) {
      rootnameout=rootnameout.substr(0, y);
  }
  return rootnameout;
}

function removeLeftOrg(rootnamein) {
  var rootnameout=rootnamein;
  var y=rootnameout.indexOf("/");
  if (y != -1) {
      rootnameout=rootnameout.substr(y+1);
  }
  return rootnameout;
}

function returnRightOrg(rootnamein) {
  var rootnameout=rootnamein;
  var y=rootnameout.lastIndexOf("/");
  if (y != -1) {
      rootnameout=rootnameout.substr(y+1);
  }
  return rootnameout;
}

function removeRightOrg(rootnamein) {
  var rootnameout=rootnamein;
  var y=rootnameout.lastIndexOf("/");
  if (y != -1) {
      rootnameout=rootnameout.substr(0,y);
  }
  return rootnameout;
}

function getOrgName (rootnamein) {
  var rootnameout=rootnamein;
  var y=rootnameout.indexOf("=");
  if (y != -1) {
      rootnameout=rootnameout.substr(y+1);
  }
  y=rootnameout.indexOf("/");
  if (y != -1) {
      rootnameout=rootnameout.substr(0, y);
  }
  return rootnameout;
}


function writeitout(myelement, mode, filename) {
  try {
      var mywriter=writetheviewbuilder(filename, mode);

        var indentcnt=writeparents( mywriter, myelement, indentstr, mode );
        var indent=indentstr;
        for(var i = 0; i<indentcnt; i++ )
              indent+=indentstr;

        writeOrgs( mywriter, myelement, indent, mode );

        if (mode == 1) {
            for(var i = 0; i<indentcnt; i++ ) {
                indent=indent.substr(1);
                mywriter.println(indent + elementtail);
            }
            mywriter.println( "   </tree>" );
            mywriter.println( "</views>" );
        }
        mywriter.close();
  } catch (Exception){
      mylogerror("Error processing writeitout " + Exception);
      throw Exception
  }
}


var continuethework=false;
var exportdata=true;

try {
   if (returnRightOrg(element.dname) == ORGroot) {
      continuethework=true;
   }
} catch (Exception) {
}

if (continuethework && exportdata) {
    try {
        var mode = 1
        var outputFileName = mode == 1 ? ViewBuilderxml : ViewBuildercsv
        if( args && args.length > 0 && args[0].trim() != '' )
        {
           outputFileName = args[0]
           if( outputFileName.indexOf( 'csv' ) > 0 )
              mode = 2
        }
        mylog(' Starts' );
        mylog(' XML export' );
        writeitout(element, mode, outputFileName);
        mylog(' Ends' );
        if( outputFile != undefined )
           session.sendMessage( 'ViewBuilder information was written to ' + outputFile.absolutePath )
    } catch (Exception){
        mylogerror("Error processing Export " + Exception);
        throw Exception
    }
}

// @internal viewbuilderExporter.fs -3lcak06
