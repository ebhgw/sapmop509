<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" version="1.0">

<!-- This transformation is used for exporting alarm field details to PDF in the Dashboard -->

 <!--
  The HTML coming from NetIQ is modified and marked up with data to facilitate the transformation. This helps to simplify the structure of the code below. Column names of the original data are changed
  in some cases to make the looping in the XSLT simpler and more generic. Tags with useful summary data are added. Modified or added tags will contain the prefix "NOC_".
 -->

    <xsl:output method="xml" indent="no" omit-xml-declaration="yes" standalone="no" />
	
	<xsl:attribute-set name="AppManagerTableHeader">
	  <xsl:attribute name="background-color">#6666cc</xsl:attribute>
	  <xsl:attribute name="border">solid #bfbfbf</xsl:attribute>
	</xsl:attribute-set>
	
	<xsl:attribute-set name="AppManagerTableHeader2">
	  <xsl:attribute name="background-color">#A0A0A0</xsl:attribute>
	  <xsl:attribute name="border">solid #bfbfbf</xsl:attribute>
	</xsl:attribute-set>
	
	<xsl:attribute-set name="AppManagerTableBodyEven">
	  <xsl:attribute name="background-color">#FFFFFF</xsl:attribute>
	  <xsl:attribute name="border">solid black</xsl:attribute>
	</xsl:attribute-set>
	
	<xsl:attribute-set name="AppManagerTableBodyOdd">
	  <xsl:attribute name="background-color">#D7D7D7</xsl:attribute>
	  <xsl:attribute name="border">solid black</xsl:attribute>
	</xsl:attribute-set>
	
	<xsl:variable name="borderStyle">solid black</xsl:variable>
	
	<!-- For generating standalone pdf documents with just the detail message -->

 	<!-- 
 	    <xsl:template match="/">
	    <fo:root>
	      <fo:layout-master-set>
	        <fo:simple-page-master master-name="A4-portrait"
	              page-height="29.7cm" page-width="21.0cm" margin="2cm">
	          <fo:region-body/>
	        </fo:simple-page-master>
	      </fo:layout-master-set>
	      <fo:page-sequence master-reference="A4-portrait">
	        <fo:flow flow-name="xsl-region-body">
	          <fo:block>
	            <xsl:apply-templates />
	          </fo:block>
	        </fo:flow>
	      </fo:page-sequence>
	    </fo:root>        
    </xsl:template>
        
	-->
	
	<xsl:template match="/">
		<xsl:apply-templates />
    </xsl:template>
 	
	

	  
    <xsl:template match="TABLE"> <!-- The HTML from NetIQ is marked up with the following tags to make the transformation easier -->
        <fo:table border="{$borderStyle}"> <!-- Start: HTML table -->
        	<fo:table-body>
	            <xsl:choose>
	                <xsl:when test="@TRANSPOSED = 1">
	                    <xsl:call-template name="transposeTable"/>
	                </xsl:when>
	                <xsl:otherwise>
	                    <xsl:call-template name="standardTable"/>
	                </xsl:otherwise>
	            </xsl:choose>
            </fo:table-body>
        </fo:table> <!-- End: HTML table -->
    </xsl:template>
    
    <xsl:template name="standardTable">
        <xsl:call-template name="tableHeader"/>
        
        <xsl:variable name="countTH">
           <xsl:value-of select="count(tr/th)"/>
        </xsl:variable>
            	
            	
          
        <!-- Second row: Column headings -->
        <xsl:if test="$countTH != 0">
        <fo:table-row border="{$borderStyle}">
            <xsl:for-each select="tr/th">
                <xsl:call-template name="headerCell"/>
            </xsl:for-each>
        </fo:table-row>
         </xsl:if>

        <!-- Rows that follow with all of the data -->
        <xsl:for-each select="tr[@class='odd']| tr[@class='even']"> <!-- Loop through all of the data rows -->
            <xsl:call-template name="dataRow" />
        </xsl:for-each>
        <!-- End: Rows that follow with all of the data -->
    </xsl:template>
    
    <xsl:template name="transposeTable">
    	<xsl:comment>Transposed</xsl:comment>
        <xsl:call-template name="tableHeader"/>
        <xsl:choose>
            <xsl:when test="CDEF">
				<fo:table-row border="{$borderStyle}">
	                <xsl:for-each select="CDEF/*">
	                    <xsl:call-template name="headerCell"/>
	                </xsl:for-each>
                </fo:table-row>

                <!-- Rows that follow with all of the data -->
                <xsl:for-each select="tr"> <!-- Loop through all of the data rows -->
                    <xsl:call-template name="dataRow" />
                </xsl:for-each>
            </xsl:when>
            <xsl:otherwise>
        	<!-- Second row: Column headings -->
		<xsl:variable name="rows" select="count(RDEF/*)" />
		<xsl:for-each select="RDEF/*">
		    <fo:table-row border="{$borderStyle}">
                        <xsl:call-template name="headerCell"/>
                
		        <xsl:variable name="rowName" select="name(.)" />
		        <xsl:for-each select="../../td/*">
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
		    </fo:table-row>
		</xsl:for-each>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="tableHeader">
        <!-- First row: Table header column -->
        <!-- 
        <xsl:if test="(EXTENDEDNAME and string-length(EXTENDEDNAME) > 0) or (DESCRIPTION and string-length(DESCRIPTION) > 0) or (@NAME and string-length(@NAME) >0) or (@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
            <xsl:variable name="columnSpan">
                <xsl:choose>
                    <xsl:when test="CDEF">
                        <xsl:value-of select="count(CDEF/*)" disable-output-escaping="no"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="count(RDEF) + count(C)" disable-output-escaping="no"/>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            -->
            <fo:table-row border="{$borderStyle}">
        	 
            	<xsl:variable name="columnSpan">
               		<xsl:value-of select="tr[@class='head']/td/@colspan" disable-output-escaping="no"/>
            	</xsl:variable>
            	
            	<xsl:choose>
				     <xsl:when test="$columnSpan = ''">
					      <fo:table-cell number-columns-spanned="1" xsl:use-attribute-sets="AppManagerTableHeader2">
					        <fo:block text-align="center">
                		    <xsl:value-of select="tr[@class='head']/td/center/text()" disable-output-escaping="no"/>
                	        </fo:block>
                          </fo:table-cell>
				     </xsl:when>
				     <xsl:otherwise>
					      <fo:table-cell number-columns-spanned="{$columnSpan}" xsl:use-attribute-sets="AppManagerTableHeader2">
					        <fo:block text-align="center">
                		    <xsl:value-of select="tr[@class='head']/td/center/text()" disable-output-escaping="no"/>
                	        </fo:block>
                            </fo:table-cell>
				     </xsl:otherwise>
			    </xsl:choose>
            	
               	
            <!-- 
                <fo:table-cell number-columns-spanned="{$columnSpan}" xsl:use-attribute-sets="AppManagerTableHeader2">
                	<fo:block text-align="center">
                        <xsl:choose>
                            <xsl:when test="(EXTENDEDNAME and string-length(EXTENDEDNAME) > 0) and (@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
                                <xsl:value-of select="EXTENDEDNAME" disable-output-escaping="no"/> : <xsl:value-of select="@DESCRIPTION" disable-output-escaping="no"/>
                            </xsl:when>
                            <xsl:when test="(@NAME and string-length(@NAME) >0) and (@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
                                <xsl:value-of select="@NAME" disable-output-escaping="no"/> : <xsl:value-of select="@DESCRIPTION" disable-output-escaping="no"/>
                            </xsl:when>
                            <xsl:when test="(EXTENDEDNAME and string-length(EXTENDEDNAME) > 0)">
                                <xsl:value-of select="EXTENDEDNAME" disable-output-escaping="no"/>
                            </xsl:when>
                            <xsl:when test="(DESCRIPTION and string-length(DESCRIPTION) > 0)">
                                <xsl:value-of select="DESCRIPTION" disable-output-escaping="no"/>
                            </xsl:when>
                            <xsl:when test="(@NAME and string-length(@NAME) >0)">
                                <xsl:value-of select="@NAME" disable-output-escaping="no"/>
                            </xsl:when>
                            <xsl:when test="(@DESCRIPTION and string-length(@DESCRIPTION) > 0)">
                                <xsl:value-of select="@DESCRIPTION" disable-output-escaping="no"/>
                            </xsl:when>
                        </xsl:choose>
                	</fo:block>
                </fo:table-cell>
                 -->
            </fo:table-row>
        <!-- /xsl:if -->
    </xsl:template>
    
    <xsl:template name="headerCell">
        <fo:table-cell xsl:use-attribute-sets="AppManagerTableHeader">
        	<fo:block>
            	<xsl:value-of select="." disable-output-escaping="no"/>
            </fo:block>
        </fo:table-cell>
    </xsl:template>
    
    <xsl:template name="dataCell">
        <xsl:param name="cellStyle" />
        <xsl:choose>
        	<xsl:when test="$cellStyle='AppManager_TableBodyEven'">
        		<fo:table-cell xsl:use-attribute-sets="AppManagerTableBodyEven">        
		        	<fo:block>     
			            <xsl:choose>
			                <xsl:when test="name(.) = 'URL' or name(.) = 'LINK'">
			                    <fo:inline color="blue"><fo:basic-link external-destination="{.}"><xsl:value-of select="." disable-output-escaping="no"/></fo:basic-link></fo:inline>
			                </xsl:when>
			                <xsl:otherwise>
			                    <xsl:value-of select="." disable-output-escaping="no" />
			                </xsl:otherwise>
			            </xsl:choose>
		            </fo:block>
		        </fo:table-cell>
        	</xsl:when>
        	<xsl:otherwise>
	        	<fo:table-cell xsl:use-attribute-sets="AppManagerTableBodyOdd">        
		        	<fo:block>     
			            <xsl:choose>
			                <xsl:when test="name(.) = 'URL' or name(.) = 'LINK'">
			                     <fo:inline color="blue"><fo:basic-link external-destination="{.}"><xsl:value-of select="." disable-output-escaping="no"/></fo:basic-link></fo:inline>
			                </xsl:when>
			                <xsl:otherwise>
			                    <xsl:value-of select="." disable-output-escaping="no" />
			                </xsl:otherwise>
			            </xsl:choose>
		            </fo:block>
	        	</fo:table-cell>        	
        	</xsl:otherwise>        
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="dataRow">
        <xsl:variable name="rowStyle">
            <xsl:choose>
                <xsl:when test="((position() mod 2) = 0)">AppManager_TableBodyEven</xsl:when>
                <xsl:otherwise>AppManager_TableBodyOdd</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <fo:table-row border="{$borderStyle}"> <!-- Loop through each column -->
            <xsl:for-each select="td">
                <xsl:call-template name="dataCell">
                    <xsl:with-param name="cellStyle" select="$rowStyle" />
                </xsl:call-template>
            </xsl:for-each> <!-- End: Loop through each column -->
        </fo:table-row>
    </xsl:template>      

</xsl:stylesheet>
