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

javax = Packages.javax;
lang = Packages.java.lang;

      var LOCAL_OFFER_DEFS = "LocalOfferDefs"

      try
      {
            var frame = new javax.swing.JDialog( appFrame, 'Select SLA Element', false )
            var  elementSelection = Packages.com.mosol.Formula.Client.ui.BrowseForElementDialog.BrowseForElement(frame,formula.Root.findElement("root=Organizations" ));
            var serviceOffersObj = elementSelection.getAttr(LOCAL_OFFER_DEFS);


            if (serviceOffersObj != null) {
               var offerdefs = Packages.com.mosol.util.Util.deserializeObject( serviceOffersObj.val.blob_val(), true);
               writeln(offerdefs);

               var aval = new Packages.com.mosol.ORB.Formula.AttrVal();
               aval.blob_val(Packages.com.mosol.util.Util.serializeObject(offerdefs,false));
               serviceOffersObj = new Packages.com.mosol.ORB.Formula.Attr(LOCAL_OFFER_DEFS, aval, 0)
               element.setAttr(serviceOffersObj);
               alert("All SLAs have been copied from " + elementSelection.getName() + " to " + element.getName());

            } 
      }
      catch (Exception) {  
         writeln(Exception);
      }

// @internal copysla.fs aea60ah
