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

//////////////////////////////////////////////////////////////////////
// Script to import an image file and convert it to Managed Objects GO format

function convert( sourceFile, destinationFile )
{
    // Check for required file
    if( File( sourceFile ).isFile() )
    {
        // Check for existing file
        if( this.overwrite || ! File( destinationFile ).isFile() )
        {
            // Read the source file.
            var ifile = new java.io.FileInputStream( sourceFile )
            var imagedata = formula.util.toByteArray( ifile )
            ifile.close()

            // Import from image.
            if( imagedata && imagedata.length > 0 )
            {
                // Convert to image.
                var image = java.awt.Toolkit.getDefaultToolkit().createImage( imagedata )
                if( image )
                {
                    // Make the target dimension.
                    var dimension = null
                    if( this.size == "default" || this.size == "null" )
                        dimension = null
                    else if( this.size == "image" )
                        dimension = new java.awt.Dimension( 0, 0 )
                    else
                    {
                        var commaPos = this.size.indexOf( ',' )
                        if( commaPos > 0 )
                            dimension = new java.awt.Dimension(
                                    Number( this.size.substring( 0, commaPos ) ),
                                    Number( this.size.substring( commaPos + 1 ) ) )
                        else if( typeof( this.size ) == 'number' || Number( this.size ) != NaN )
                            dimension = new java.awt.Dimension( Number( this.size ), Number( this.size ) )
                        else
                            writeln( "Warning: size (", this.size, ") does not seem to be a correct parameter." )
                    }

                    // Convert the image.
                    formula.util.image2GOFile( image, destinationFile, dimension, this.shape )
                    writeln( "Image converted to go format." )
                }
                else
                    writeln( "AWT toolkit could not convert image." )
            }
            else
                writeln( "Image could not be imported." )
        }
        else
            writeln( "Error: destination file '", destinationFile, "' already exists." )
    }
    else
        writeln( "Error: source file '", sourceFile, "' does not exist." )
}

function usage()
{
    writeln()
    writeln( 'NetIQ Operations Center(r) (c) 2014, NetIQ Corporation.' )
    writeln()
    writeln( "Usage:    image2go   [options] imageFile goFile" )
    writeln()
    writeln( "Options:" )
    writeln( "          -o        overwrite destination file (if it exists)" )
    writeln( "          -sSize    output size" )
    writeln( "                                (number = n x n)" )
    writeln( "                                (dimension = n x m)" )
    writeln( "                                (default = largest image boundary)" )
    writeln( "                                (image   = use image boundaries)" )
    writeln( "          -tShape   (circle, rectangle, roundrectangle, points, or none)" )
    writeln()
    writeln( "Examples:" )
    writeln( "          image2go               myimage.gif myimage.go" )
    writeln( "          image2go  -s32         myimage.gif myimage.go" )
    writeln( "          image2go  -s50,51      myimage.gif myimage.go" )
    writeln( "          image2go  -s100,0      myimage.gif myimage.go" )
    writeln( "          image2go  -sdefault    myimage.gif myimage.go" )
    writeln( "          image2go  -simage      myimage.gif myimage.go" )
    writeln( "          image2go  -trectangle  myimage.gif myimage.go" )
    writeln()
    writeln( "Points:" )
    writeln( "          A semicolon-delimited list of coordinates:" )
    writeln( "             (eg. 0,0;40,0;40,40;0,40,0,0 )" )
}

// Argument driver
if( args.length < 2 )
    usage()
else
{
    // Set defaults.
    this.size = 'default'
    this.shape = 'rectangle'
    this.overwrite = false

    // Parse arguments.
    var inFile, outFile;
    for( var i = 0 ; i < args.length ; ++i )
    {
        // writeln( 'arg ', i, ' = ', args[i] )
        var start = args[i].substring( 0, 2 )
        if( start.substring( 0, 1 ) == '-' )
        {
            if( start == '-s' )
            {
                this.size = args[i].substring( 2 )
                // writeln( "Using size = ", this.size, ", type ", typeof( this.size ) )
            }
            else if( start == '-t' )
            {
                this.shape = args[i].substring( 2 )
                // writeln( "Using shape = ", this.shape )
            }
            else if( start == '-o' )
                this.overwrite = true
        }
        else
        {
            if( ! inFile )
                inFile = args[i]
            else
                outFile = args[i]
        }
    }

    // Run the conversion.
    // writeln( 'in = ', inFile, ' out = ', outFile )
    if( inFile && outFile )
        convert( inFile, outFile );
    else
        usage()
}

// Why do we need to do this?
java.lang.System.exit( 0 )


