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

///////////////////////////////////////////////////////////////////////////////////
//
// Make a simple connection and get a list of tables in the database.
//
// Note:    Requires the mSQL imaginary.com driver to be installed,
//          and in the path given in the initiaztion statement (or
//          loaded via classpath).
//
// This is a direct adapatation of the Imaginary mSQL example
// in examples/Select.java
//
// This example can be modified to use any jdbc source by modifying
// the url, and changing the connection method to use the proper
// user id and password.
//

function main()
{
    // For the connection.
    var url = "jdbc:msql://carthage.imaginary.com:1114/test";
    java.lang.Class.forName("com.imaginary.sql.msql.MsqlDriver");

    // Make the connection.
    var con = java.sql.DriverManager.getConnection(url, "borg", "");

    // Do a query.
    var stmt = con.createStatement();
    var rs = stmt.executeQuery("SELECT * from test ORDER BY test_id");
    var meta = rs.getMetaData();
    writeln("Got results:");
    while(rs.next()) {
        var a = rs.getInt("test_id");
        var str = rs.getString("test_val");

        write("\ttest_id= " + a);
        writeln("/str= '" + str + "'");
    }
    stmt.close();

    // Do the meta data lookup.
    var dbmd = con.getMetaData();
    rs = dbmd.getTables(null, null, null, null);
    while( rs.next() ) {
        writeln("Table: " + rs.getString("TABLE_NAME"));
    }
    con.close();
}

main()
