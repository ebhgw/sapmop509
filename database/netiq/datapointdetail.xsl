<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0">

	<xsl:output method="html" indent="yes" />

	<xsl:template match="/">
	<xsl:for-each select="//TABLE">
		<table class="noc-performance-view-dpdetail-msgtable">
			<caption class="noc-performance-view-dpdetail-msgtitle">
				<xsl:choose>
					<xsl:when test="DESCRIPTION">
						<xsl:value-of select="DESCRIPTION" />
					</xsl:when>
					<xsl:when test="EXTENDEDNAME">
						<xsl:value-of select="EXTENDEDNAME" />
						<xsl:if test="@DESCRIPTION" >
							<xsl:text>: </xsl:text>
							<xsl:value-of select="@DESCRIPTION" />
						</xsl:if>
					</xsl:when>
					<xsl:when test="@DESCRIPTION">
						<xsl:value-of select="@DESCRIPTION" />
					</xsl:when>
				</xsl:choose>
			</caption>
			<tr>
				<!-- Generate a table header for each C* tag under CDEF -->
				<xsl:for-each select="CDEF/*">
					<th class="noc-performance-view-dpdetail-msgheader">
						<xsl:value-of select="@NAME" />
					</th>
				</xsl:for-each>
			</tr>

			<!-- There may be more than one R tag. Each R tag represents a table row -->
			<xsl:for-each select="R">
				<tr>
					<!-- Each C* tag is a cell within the row represented by the R tag. -->
					<xsl:for-each select="./*">
						<td class="noc-performance-view-dpdetail-msgcell">
					     	    <xsl:choose>
					                <xsl:when test="name(.) = 'URL' or name(.) = 'LINK'">
					                    <a href="{.}" target="_blank"><xsl:value-of select="."/></a>
					                </xsl:when>
					                <xsl:otherwise>
					                    <xsl:value-of select="." />
					                </xsl:otherwise>
					            </xsl:choose>
					        </td>
					</xsl:for-each>
				</tr>
			</xsl:for-each>

		</table>
		<br />
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
