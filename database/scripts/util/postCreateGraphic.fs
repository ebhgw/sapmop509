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


   var dname = newGraphicElementDName;
   try      
   {
      var element = ElementManager.getElementManager().findElement(dname);
      var browserFrame = Packages.com.mosol.Formula.Client.ui.Main.getMain().getMainFrame().getTopBrowser()
      browserFrame.switchToElementAndView(element, Packages.com.mosol.ORB.Formula.RelationKind.NAM, "SVG Layout");
      var currView = browserFrame.getActiveView()
      if ( currView instanceof Packages.com.mosol.Formula.Client.ui.layoutSVG.ElementLayoutSVGPanel )
      {
         formula.log.info( "Editing new " + graphicType + ": " + element.getName());
	     currView.createAndEditNewDrawing(graphicType)
      }
   }
   catch (e)
   {
      // TODO Auto-generated catch block
      writeln("got exception" + e)
   }


// @internal postCreateGraphic.fs -1ilmij6
