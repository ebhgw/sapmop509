//
// Copyright (c) 2010 Novell, Inc.  All Rights Reserved.
//
// THIS WORK IS SUBJECT TO U.S. AND INTERNATIONAL COPYRIGHT LAWS AND TREATIES.  IT MAY NOT BE USED, COPIED, 
// DISTRIBUTED, DISCLOSED, ADAPTED, PERFORMED, DISPLAYED, COLLECTED, COMPILED, OR LINKED WITHOUT NOVELL'S
// PRIOR WRITTEN CONSENT. USE OR EXPLOITATION OF THIS WORK WITHOUT AUTHORIZATION COULD SUBJECT THE 
// PERPETRATOR TO CRIMINAL AND CIVIL LIABILITY.
//
// NOVELL PROVIDES THE WORK "AS IS," WITHOUT ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING WITHOUT THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT. NOVELL,
// THE AUTHORS OF THE WORK, AND THE OWNERS OF COPYRIGHT IN THE WORK ARE NOT LIABLE FOR ANY CLAIM,
// DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF,
// OR IN CONNECTION WITH THE WORK OR THE USE OR OTHER DEALINGS IN THE WORK.
//

// @debug off
// @opt -1


function loadPropertiesFile(propertyfilename) {

	thisLPscriptname="loadPropertiesFile";

	thatLPscriptname=formula.logCategory
	formula.logCategory=thisLPscriptname

	propertyfilename=server.getProperty("formula.home")+"/"+propertyfilename;

	propnames=null;

        setuppage = new java.util.Properties();
        try {
                fis = new java.io.FileInputStream( propertyfilename );
                try  {
                        setuppage.load( fis );

                         iin=0;
                        try {
				propnames=new Array();
				iin=10
				spn=setuppage.propertyNames()
				iin=20
				//formula.log.info('spn=' + spn);
				iin=22
				//formula.log.info(spn.elements());
				iin=23
				//e = spn.elements()
				e = spn
				iin=24

				formula.log.debug(e.hasMoreElements());
				iin=26

				ic2=0

				while ( e.hasMoreElements()) {
					iin=30
					thisele=e.nextElement()
					iin=50
					propnames[thisele]=setuppage.getProperty(thisele);
				}

                        } catch (Exception) {
                           formula.log.warn('gp0='+ iin + ' ' +  Exception);
                        }



                } catch (Exception) {
                   formula.log.warn('l1='+ Exception);
                }
                fis.close();
        } catch (Exception) {
           formula.log.warn('l0='+ Exception);
        }

	formula.logCategory=thatLPscriptname
	return propnames;

}

function makeHashtableFromArray(myArray) {

	limaparray=new java.util.Hashtable()
	for (itemkey in myArray) {
		limaparray.put(itemkey, myArray[itemkey])
	}
	return limaparray
}


function makeUpdatedHashtableFromArray(myArray) {

	thisLPscriptname="makeUpdatedHashtableFromArray";

	thatLPscriptname=formula.logCategory
	formula.logCategory=thisLPscriptname

	limaparray=new java.util.Hashtable()
	for (itemkey in myArray) {
		limaparray.put(itemkey, myArray[itemkey])
	}

//	limaparray2=new java.util.Hashtable()
	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proseve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}
	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proseve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}

	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		formula.log.debug('old' + itemkey+"="+myArray[itemkey]);
		myArray[itemkey]=limaparray.get(itemkey);
		formula.log.debug('new' + itemkey+"="+myArray[itemkey]);
	}

	formula.logCategory=thatLPscriptname

	return limaparray

}



function makeUpdatedHashtableFromArrayToSystemProperties(myArray) {

	thisLPscriptname="makeUpdatedHashtableFromArrayToSystemProperties";

	thatLPscriptname=formula.logCategory
	formula.logCategory=thisLPscriptname

	limaparray=new java.util.Hashtable()
	for (itemkey in myArray) {
		limaparray.put(itemkey, myArray[itemkey])
	}

//	limaparray2=new java.util.Hashtable()
	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proserve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}
	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proserve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}

	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		formula.log.debug('old' + itemkey+"="+myArray[itemkey]);
		myArray[itemkey]=limaparray.get(itemkey);

		java.lang.System.setProperty(itemkey, myArray[itemkey])

		formula.log.debug('new' + itemkey+"="+myArray[itemkey]);
	}

	formula.logCategory=thatLPscriptname

	return limaparray

}


function makeUpdatedHashtableFromArrayTomosData(myArray, mapkey) {

	thisLPscriptname="makeUpdatedHashtableFromArrayTomosData";

	thatLPscriptname=formula.logCategory
	formula.logCategory=thisLPscriptname

	limaparray=new java.util.Hashtable()
	for (itemkey in myArray) {
		limaparray.put(itemkey, myArray[itemkey])
	}

//	limaparray2=new java.util.Hashtable()
	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proserve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}
	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proserve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}

	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proserve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}

	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		tMMformat=new Packages.com.proserve.MappedMessageFormat(limaparray.get(itemkey));
		oneurl=tMMformat.format(limaparray)
		limaparray.put(itemkey, oneurl)
	}

	for (itemkey in myArray) {
		if (itemkey.equals('asHashTable')) {
			continue;
		}
		formula.log.debug('old ' + itemkey+"="+myArray[itemkey]);
		myArray[itemkey]=limaparray.get(itemkey);

		formula.log.info('new ' + itemkey+"="+myArray[itemkey]);
	}

	Packages.com.proserve.mosData.mosHashMap.put(new java.lang.String(mapkey), limaparray);

	formula.logCategory=thatLPscriptname

	return limaparray

}

