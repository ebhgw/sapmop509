<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<!-- 
		DO NOT MODIFY THIS FILE!!!!!
		The XML coming from NetIQ is modified and marked up with data to facilitate the transformation. This helps to simplify the structure of the code below. 
		Column names of the original data are changed in some cases to make the looping in the XSLT simpler and more generic. Tags with useful summary data are added.
		Modified or added tags will contain the prefix "NOC_".
	-->

	<xsl:output method="html" indent="yes" />


	<xsl:template match="/">
		<xsl:apply-templates />
	</xsl:template>
	
	
	<xsl:template match="TABLE">
		<!-- The XML from NetIQ is marked up with the following tags to make the transformation easier -->
	
		<table> <!-- Start: HTML table -->

			<xsl:choose> 
				<xsl:when test="NOC_INFO/@NOC_TABLE_TYPE='CDEF'"> <!-- For column definitions -->
				
					<xsl:variable name="columnSpan" select="NOC_INFO/@COLUMN_COUNT"/>  <!-- How many columns does the table have? This tag was added to original data -->
					<xsl:variable name="rowCount" select="NOC_INFO/@ROW_COUNT"/>       <!-- How many rows does the table have? This tag was added to original data -->
				
					<!-- First row: Table header column -->
					<xsl:choose>
						<xsl:when test="@NAME != ''"> <!-- If we have data for a table name then draw it up as a row -->
							<tr bgcolor="#A0A0A0" style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
								<th colspan="{$columnSpan}">
									<font color="#FFFFFF">
										<xsl:choose>
											<xsl:when test="@DESCRIPTION"> 
												<b>
													<xsl:value-of select="@NAME"/>: <xsl:value-of select="@DESCRIPTION"/>
												</b>							
											</xsl:when>
											<xsl:otherwise> <!-- We may not have a "description" attribute so don't insert the colon -->
												<b>
													<xsl:value-of select="@NAME"/>
												</b>
											</xsl:otherwise>
										</xsl:choose>
									</font>
								</th>
							</tr>
						</xsl:when>
						<xsl:otherwise> <!-- Use the "EXTENDEDNAME" tag if we can't get the "NAME" attribute of the table -->
							<tr bgcolor="#A0A0A0" style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
									<th colspan="{$columnSpan}">
										<font color="#FFFFFF">
											<b>
												<xsl:value-of select="EXTENDEDNAME"/>
											</b>
										</font>
									</th>
								</tr>
						</xsl:otherwise>
					</xsl:choose>
			
					<!-- Second row: Column headings -->
					<tr bgcolor="#6666FF" fgcolor="#FFFFFF">
						<xsl:for-each select="NOC_INFO/NOC_COL_NAME">
							<td style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
								<font color="#FFFFFF">
									<b>
										<xsl:value-of select="."/>
									</b>
								</font>
							</td>
						</xsl:for-each>
					</tr>
					
					<!-- Rows that follow with all of the data -->
					<xsl:choose> <!-- The way that we deal with a single row or multiple is different -->
						<xsl:when test="$rowCount = 1"> <!-- If we have just one row then paint the columns white -->
						
							<tr bgcolor="#FFFFFF">
								<xsl:for-each select="R/NOC_COL">
									<td style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
						
										<xsl:choose> <!-- Style hints to be observed --> 
											<xsl:when test="./@STYLE='color:red'">
												<font color="#FF0000">
													<xsl:value-of select="."/> <!-- The tag "COL" represents a modification of the original tag name -->
												</font>
											</xsl:when>
											<xsl:otherwise>
												<font>
													<xsl:value-of select="."/> <!-- The tag "COL" represents a modification of the original tag name -->
												</font>												
											</xsl:otherwise>
											
										</xsl:choose> <!-- End: Style hints to be observed -->							
										
									</td>
								</xsl:for-each>
							</tr>
							
						</xsl:when>
						
						<xsl:otherwise> <!-- We have more then one row. Each row of data must be different color from the previous -->
						
							<xsl:for-each select="R"> <!-- Loop through all of the data rows -->
							
								<xsl:choose> <!-- Even rows are white and odd grey -->
								
									<xsl:when test="(position() mod 2) = 0"> <!-- Rows that should have a white background -->
										<tr bgcolor="#FFFFFF"> <!-- Loop through each column -->	
											<xsl:for-each select="NOC_COL"> <!-- The tag "NOC_COL" represents a modification of the original tag name -->
												<td style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
												
													<xsl:choose> <!-- Style hints to be observed --> 
														<xsl:when test="./@STYLE='color:red'">
															<font color="#FF0000">
																<xsl:value-of select="."/>
															</font>
														</xsl:when>
														<xsl:otherwise>
															<font>
																<xsl:value-of select="."/>
															</font>												
														</xsl:otherwise>
														
													</xsl:choose> <!-- End: Style hints to be observed --> 
													
												</td>
											</xsl:for-each> <!-- End: Loop through each column -->
										</tr>							
									</xsl:when>
									
									<xsl:otherwise> <!-- Rows that should have a light grey background -->
										<tr bgcolor="#D7D7D7"> <!-- Loop through each column -->	
											<xsl:for-each select="NOC_COL"> <!-- The tag "COL" represents a modification of the original tag name -->
												<td style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
												
													<xsl:choose> <!-- Style hints to be observed --> 
														<xsl:when test="./@STYLE='color:red'">
															<font color="#FF0000">
																<xsl:value-of select="."/>
															</font>
														</xsl:when>
														<xsl:otherwise>
															<font>
																<xsl:value-of select="."/>
															</font>												
														</xsl:otherwise>
														
													</xsl:choose> <!-- End: Style hints to be observed --> 
													
												</td>
											</xsl:for-each> <!-- End: Loop through each column -->
										</tr>									
									</xsl:otherwise>
									
								</xsl:choose> <!-- End: Even rows are white and odd grey -->
								
							</xsl:for-each> <!-- End: Loop through all of the data rows -->
											
						</xsl:otherwise>
									
					</xsl:choose>
					<!-- End: Rows that follow with all of the data -->
				
				</xsl:when>  <!-- End: For column definitions -->
				
				<xsl:otherwise> <!-- Definition for RDEF types -->
				
					<xsl:variable name="columnSpan" select="NOC_INFO/@COLUMN_COUNT+1"/>  <!-- How many columns does the table have? Add on for the title -->
					<xsl:variable name="rowCount" select="NOC_INFO/@ROW_COUNT"/>         <!-- How many rows -->
									
					<!-- First row: Table header column -->
					<xsl:if test="@NAME != ''"> <!-- If we have data for a table name then draw it up as a row -->
						<tr bgcolor="#A0A0A0" style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
							<th colspan="{$columnSpan}">
								<font color="#FFFFFF">
									<xsl:choose>
										<xsl:when test="@DESCRIPTION"> 
											<b>
												<xsl:value-of select="@NAME"/>: <xsl:value-of select="@DESCRIPTION"/>
											</b>							
										</xsl:when>
										<xsl:otherwise> <!-- We may not have a "description" attribute so don't insert the colon -->
											<b>
												<xsl:value-of select="@NAME"/>
											</b>
										</xsl:otherwise>
									</xsl:choose>
								</font>
							</th>
						</tr>
					</xsl:if>

					<xsl:for-each select="NOC_ROW">
						<tr>
							<td bgcolor="#6666FF" fgcolor="#FFFFFF" style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
								<font color="#FFFFFF">
									<b>
										<xsl:value-of select="./@NAME" />
									</b>
								</font>
							</td>
					
					
							<xsl:for-each select="./NOC_COL">
									<td bgcolor="#FFFFFF" style="border-width: 1px; padding: 1px; border-style: solid; border-color: gray;">
						
										<xsl:choose> <!-- Style hints to be observed --> 
											<xsl:when test="./@STYLE='color:red'">
												<font color="#FF0000">
													<xsl:value-of select="."/> <!-- The tag "COL" represents a modification of the original tag name -->
												</font>
											</xsl:when>
											<xsl:otherwise>
													<xsl:value-of select="."/> <!-- The tag "COL" represents a modification of the original tag name -->										
											</xsl:otherwise>
											
										</xsl:choose> <!-- End: Style hints to be observed -->							
										
									</td>
							</xsl:for-each>
							
						</tr>
					</xsl:for-each>					

				</xsl:otherwise> <!-- End: Definition for RDEF types -->
			
			</xsl:choose>
			
		</table> <!-- End: HTML table -->
		
		<br/> <!-- Another table may follow this so allow some space for it -->
		
			
	</xsl:template>

</xsl:stylesheet>