<?xml version="1.0"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
		<xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="TABLE">
		<table>
			<tr><td colspan="3"><b><font size="+2"><xsl:value-of select="@DESCRIPTION"/></font></b></td></tr>
			<tr>
				<xsl:apply-templates select="CDEF"/>
			</tr>
			<xsl:apply-templates select="R"/>
		</table>
	</xsl:template>

	<xsl:template match="CDEF">
		<td>
		  <b>
			  <xsl:apply-templates select="C0"/>
			  <xsl:value-of select="C0/@NAME"/>
		  </b>
		</td>
		<td>
		  <b>
			  <xsl:apply-templates select="C1"/>
			  <xsl:value-of select="C1/@NAME"/>
		  </b>
		</td>
		<td>
		  <b>
			  <xsl:apply-templates select="C2"/>
			  <xsl:value-of select="C2/@NAME"/>
		  </b>
		</td>
	</xsl:template>

	<xsl:template match="R">
		<tr>
			<td>
				<xsl:apply-templates select="C0"/>
			</td>
			<td>
				<xsl:apply-templates select="C1"/>
			</td>
			<td>
				<xsl:apply-templates select="C2"/>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="C0">
		<xsl:copy-of select="@*"/>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="C1">
		<xsl:copy-of select="@*"/>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="C2">
		<xsl:copy-of select="@*"/>
		<xsl:apply-templates/>
	</xsl:template>

</xsl:stylesheet>
