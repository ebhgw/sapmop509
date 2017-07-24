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


// Server side script example functions for notifying clients of element icon changes.
//
// These functions may be tried in place of restarting clients when element icons change.
//
// If a client misses a notification then re-run the script that invokes these functions.
//
// Note that there may be performance issues if you run fireElementGraphicChangedDeep
// on a large Element or Services hierarchy.

// Function: fireElementGraphicChanged
//
//    Notify clients that the icons for a given element changed and
//    the icons need to be re-acquired from the server.
//
// Param: el - the element to fire the notification for
//
// Example: fireElementGraphicChanged( element )

function fireElementGraphicChanged( el )
{
  el.fireElementGraphicChanged()
}


// Function: fireElementGraphicChangedDeep
//
//    Notify clients that the icons for a given element and all of its
//    children (deep) potentially changed and the icons need to be
//    re-acquired from the server.
//
// Param: el - the element to fire the notification for
// Param: relationKind - the relation kind to follow when walking child relationships
// Param: discover - force discovery boolean
//
// Example: fireElementGraphicChangedDeep( element, formula.relations.NAM, false )

function walkVisitor( child )
{
   child.fireElementGraphicChanged()
}
function fireElementGraphicChangedDeep( el, relationKind, discover )
{
  el.walk( walkVisitor, relationKind, discover )
}
