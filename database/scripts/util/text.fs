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
// Utility functions for text processing.
//


// Replace tags in text delimited by ~expr~, evaluating and replacing
function textExpressionReplace( text )
{
    // Check parameters
    if( ! text || ( text == "" ) )
        return ""

    // Loop until text is processed.
    var newtext = ""
    var iSearchPos = 0
    while( true )
    {
        // Look for an instance of the search target.
        var iNextSearchPos = text.indexOf( "~", iSearchPos )

        // Not found?  Just return the rest of string, and get out.
        if( iNextSearchPos < 0 )
        {
            newtext += text.substring( iSearchPos )
            break
        }

        // Get the delimiter of the tag.
        var iDelimeterPos = text.indexOf( "~", iNextSearchPos + 1 )
        if( iDelimeterPos < 0 )
        {
            // writeln( "Delimiter of tag at position " + iNextSearchPos + " was not found." )
            return text
        }

        // Get the expression to evaluate.
        var expr = text.substring( iNextSearchPos + 1, iDelimeterPos )
        if( expr != "" )
        {
            // writeln( "Evaluating [" + expr + "] from " + iNextSearchPos + " to " + iDelimeterPos )
            var replacewith = tryEval( expr )
            if( ! replacewith || replacewith.error || ( replacewith.value == undefined ) )
            {
                // Try a "this" lookup.
                var thisexpr = 'this.' + expr
                // writeln( "Not found; trying : ", thisexpr )
                replacewith = tryEval( thisexpr )

                // Bad, can't get anything!
                if( ! replacewith || replacewith.error || ( replacewith.value == undefined ) )
                {
                    // writeln( "Not found" )
                    replacewith = "(TEMPLATE_ERROR: " + expr + ")"
                }
                else
                    replacewith = replacewith.value
            }
            else
                replacewith = replacewith.value
        }
        else
            replacewith = "~"

        // Add next piece of string with replacement.
        newtext += text.substring( iSearchPos, iNextSearchPos ) + replacewith

        // Travel past the replaced text.
        iSearchPos = iDelimeterPos + 1
    }
    return newtext
}

// Replace target in text.
function textSearchAndReplace( text, searchfor, replacewith )
{
    var newtext = ""
    var iSearchPos = 0
    while( true )
    {
        // Look for an instance of the search target.
        var iNextSearchPos = text.indexOf( searchfor, iSearchPos )

        // Not found?  Just return the rest of string, and get out.
        if( iNextSearchPos < 0 )
        {
            newtext += text.substring( iSearchPos )
            break
        }

        // Add next piece of string with replacement.
        newtext += text.substring( iSearchPos, iNextSearchPos ) + replacewith

        // Travel past the replaced text.
        iSearchPos = iNextSearchPos + searchfor.length
    }
    return newtext
}


// Some test functions.

function test()
{
    element = new Object()
    element.Name = 'New York Router'
    element.Contact = 'Kurt Westerfeld'
    element.Email = 'kurt@westerfeld.com'
    var testtext = 'Sending email to ~element.Contact~ at ~element.Email~ b/c of ~arguments[0]~...'
    writeln( "Test 1: ", textExpressionReplace( testtext, element.Name ) )
    if( ! this.arguments || ( this.arguments[0] == undefined ) )
        writeln( " >> NOTE: no arguments are provided, so error is expected." )
    delete element
    writeln( "Test 2: ", textExpressionReplace( "An empty ~~ tag is in this string" ) )
    writeln( "Test 3: ", textExpressionReplace( "No tags are in this string" ) )
    writeln( "Test 4: ", textExpressionReplace( "An error is in this string here: ~badLookup~" ) )
    writeln( "Test 5: ", textExpressionReplace( "A malformed tag ~ is in this string" ) )
}

// @internal text.fs b5he5af
