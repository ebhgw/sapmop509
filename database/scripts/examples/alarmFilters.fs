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



/**
This script shows how to programmatically manupulate user's alarm filters.
The following functions are available to be invoked agains a UserElement object.

1)
@return A list of filter names from the user's profile
--- public List<String> getAlarmFilterNameList()

2)   
@param names An array of alarm fitler names to delete from the user's profile
@return A number indicating how many alarm filters were successfully deleted   
--- public int deleteAlarmFiltersByName(String[] names)

3) 
@param filePath Location of a file that contains previously exported alarm filters
@throws IOException When the file cannot be property opened and/or read.
@throws StreamException If the contents of the file cannot be parsed. (Most likely the file has been
                        manually edited post-export)
Applies the alarm filters stored in the file into the user's profile. 

--- public void importAlarmFiltersFromFile(String filePath) throws IOException, StreamException

**/

//@debug off
List = Packages.java.lang.util.List;
ArrayList = Packages.java.lang.util.ArrayList;

/***** Update the following variables to reflect your settings *****/
var username = 'admin';
var importFile = 'C:/Documents and Settings/smikhailov/Desktop/boolean_test.xml';
var alarmFilterNameToDelete = 'boolean_test';
/********************************************************************/

var user = formula.Root.findElement('user='+username+'/users=Users/security=Security/root=Administration');

/*** Show all alarm filters for the given user ***/
if(user != null){
   formula.log.info('The following alarm filters are available for user '+username);
   var listOfFilterNames = user.getAlarmFilterNameList();
   if(listOfFilterNames != null){
      for(var i=0; i<listOfFilterNames.size(); i++){
         formula.log.info('filter name: '+listOfFilterNames.get(i));
      }
   }
}

/*** Delete the specified alarm filter. The user must be logged out of the system when this operations runs. ***/
var namesOfFiltersToDelte = new Array(1);
namesOfFiltersToDelte[0] = alarmFilterNameToDelete;
var deleted = user.deleteAlarmFiltersByName(namesOfFiltersToDelte);
formula.log.info('Deleted ' + deleted + ' filters.');

/*** Apply previously exported alarm filters into user's profile ***/
//the file must be located on the server
user.importAlarmFiltersFromFile(importFile);
