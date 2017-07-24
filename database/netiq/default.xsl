<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

 <!--
  The XML coming from NetIQ is modified and marked up with data to facilitate the transformation. This helps to simplify the structure of the code below. Column names of the original data are changed
  in some cases to make the looping in the XSLT simpler and more generic. Tags with useful summary data are added. Modified or added tags will contain the prefix "NOC_".
 -->

    <xsl:output method="html" indent="yes" />

    <xsl:template match="/">
        <style>
.AppManager_TableHeader
{
    background-color: #6666cc;
    border-width: 1px;
    border-style: solid;
    border-color: #bfbfbf;
    color: white;
    font-family: Arial, Verdana;
    font-weight: bold;
}
.AppManager_TableHeader2
{
    background-color: #A0A0A0;
    border-width: 1px;
    border-style: solid;
    border-color: #bfbfbf;
    color: white;
    font-family: Arial, Verdana;
    font-weight: bold;
}

.AppManager_TableBodyEven
{
    background-color: #FFFFFF;
    border-width: 1px;
    border-style: solid;
    border-color: #7f7f7f;
    color: black;
    font-family: Arial, Verdana;
    padding-right:10px;
    padding-left:10px;
}
.AppManager_TableBodyOdd
{
    background-color: #D7D7D7;
    border-width: 1px;
    border-style: solid;
    border-color: #7f7f7f;
    color: black;
    font-family: Arial, Verdana;
    padding-right:10px;
    padding-left:10px;
}
.AppManager_Table
{
    background-color: transparent;
    border-width: 1px;
    border-style: solid;
    border-color: black;
    font-family: Arial, sans-serif;
    font-style: normal;
    font-weight: normal;
}

        </style>
        <xsl:apply-templates />
    </xsl:template>


    <xsl:template match="TABLE"> <!-- The XML from NetIQ is marked up with the following tags to make the transformation easier -->
        <table cellspacing="0" class="AppManager_Table" style="table-layout:fixed"> <!-- Start: HTML table -->
            <xsl:choose>
                <xsl:when test="@TRANSPOSED = 1">
                    <xsl:call-template name="transposeTable"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:call-template name="standardTable"/>
                </xsl:otherwise>
            </xsl:choose>
            
        </table> <!-- End: HTML table -->
        <br />
    </xsl:template>
    
    <xsl:template name="standardTable">
        <xsl:call-template name="tableHeader"/>

        <!-- Second row: Column headings -->
        <tr style="height:auto">
            <xsl:for-each select="CDEF/*">
                <xsl:call-template name="headerCell"/>
            </xsl:for-each>
        </tr>

        <!-- Rows that follow with all of the data -->
        <xsl:for-each select="R"> <!-- Loop through all of the data rows -->
            <xsl:call-template name="dataRow" />
        </xsl:for-each>
        <!-- End: Rows that follow with all of the data -->
    </xsl:template>
    
    <xsl:template name="transposeTable">
    <xsl:comment>Transposed</xsl:comment>
        <xsl:call-template name="tableHeader"/>
        <xsl:choose>
            <xsl:when test="CDEF">
		<tr style="height:auto">
                <xsl:for-each select="CDEF/*">
                    <xsl:call-template name="headerCell"/>
                </xsl:for-each>
                </tr>

                <!-- Rows that follow with all of the data -->
                <xsl:for-each select="R"> <!-- Loop through all of the data rows -->
                    <xsl:call-template name="dataRow" />
                </xsl:for-each>
            </xsl:when>
            <xsl:otherwise>
        	<!-- Second row: Column headings -->
		<xsl:variable name="rows" select="count(RDEF/*)" />
		<xsl:for-each select="RDEF/*">
		    <tr style="height:auto">
                        <xsl:call-template name="headerCell"/>
                
		        <xsl:variable name="rowName" select="name(.)" />
		        <xsl:for-each select="../../C/*">
                	    <xsl:if test="name(.) = $rowName">
		                <xsl:variable name="colStyle">
                    	            <xsl:choose>
		                        <xsl:when test="((position() mod (2*$rows)) > $rows) or ((position() mod (2*$rows)) = 0)">AppManager_TableBodyEven</xsl:when>
                	                <xsl:otherwise>AppManager_TableBodyOdd</xsl:otherwise>
		                    </xsl:choose>
		                </xsl:variable>
                	        <xsl:call-template name="dataCell">
		                    <xsl:with-param name="cellStyle" select="$colStyle" />
                	        </xsl:call-template>
		            </xsl:if>
                	</xsl:for-each>
		    </tr>
		</xsl:for-each>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="tableHeader">
        <!-- First row: Table header column -->
        <xsl:if test="(EXTENDEDNAME and string-length(EXTENDEDNAME) > 0) or (DESCRIPTION and string-length(DESCRIPTION) > 0) or (@NAME and string-length(@NAME) >0) or (@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
            <xsl:variable name="columnSpan">
                <xsl:choose>
                    <xsl:when test="CDEF">
                        <xsl:value-of select="count(CDEF/*)" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="count(RDEF) + count(C)" />
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <tr style="height:auto">
                <td colspan="{$columnSpan}" class="AppManager_TableHeader2">
                    <center>
                        <xsl:choose>
                            <xsl:when test="(EXTENDEDNAME and string-length(EXTENDEDNAME) > 0) and (@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
                                <xsl:value-of select="EXTENDEDNAME" /> : <xsl:value-of select="@DESCRIPTION" />
                            </xsl:when>
                            <xsl:when test="(@NAME and string-length(@NAME) >0) and (@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
                                <xsl:value-of select="@NAME" /> : <xsl:value-of select="@DESCRIPTION" />
                            </xsl:when>
                            <xsl:when test="(EXTENDEDNAME and string-length(EXTENDEDNAME) > 0)">
                                <xsl:value-of select="EXTENDEDNAME" />
                            </xsl:when>
                            <xsl:when test="(DESCRIPTION and string-length(DESCRIPTION) > 0)">
                                <xsl:value-of select="DESCRIPTION" />
                            </xsl:when>
                            <xsl:when test="(@NAME and string-length(@NAME) >0)">
                                <xsl:value-of select="@NAME" />
                            </xsl:when>
                            <xsl:when test="(@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
                                <xsl:value-of select="@DESCRIPTION" />
                            </xsl:when>
                        </xsl:choose>
                    </center>
                </td>
            </tr>
        </xsl:if>
    </xsl:template>
    
    <xsl:template name="headerCell">
        <td class="AppManager_TableHeader">
            <xsl:value-of select="@NAME" />
        </td>
    </xsl:template>
    <xsl:template name="dataCell">
        <xsl:param name="cellStyle" />
        <td class="{$cellStyle}" style="{@STYLE}">
            <xsl:choose>
                <xsl:when test="name(.) = 'URL' or name(.) = 'LINK'">
                    <a href="{.}"><xsl:value-of select="." disable-output-escaping="yes"/></a>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="." disable-output-escaping="yes" />
                </xsl:otherwise>
            </xsl:choose>
        </td>
    </xsl:template>
    
    <xsl:template name="dataRow">
        <xsl:variable name="rowStyle">
            <xsl:choose>
                <xsl:when test="((position() mod 2) = 0)">AppManager_TableBodyEven</xsl:when>
                <xsl:otherwise>AppManager_TableBodyOdd</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <tr style="height:auto"> <!-- Loop through each column -->
            <xsl:for-each select="*">
                <xsl:call-template name="dataCell">
                    <xsl:with-param name="cellStyle" select="$rowStyle" />
                </xsl:call-template>
            </xsl:for-each> <!-- End: Loop through each column -->
        </tr>
    </xsl:template>

</xsl:stylesheet>
