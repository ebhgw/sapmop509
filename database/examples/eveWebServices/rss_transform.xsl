<?xml version="1.0" encoding="ISO-8859-1"?>

<xsl:transform version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">

	<xsl:variable name="header">
		rss_version=<xsl:value-of select="rss/@version"/>;
		title=<xsl:value-of select="rss/channel/title"/>;
		link=<xsl:value-of select="rss/channel/link"/>;
		Description=<xsl:value-of select="rss/channel/description"/>;
		language=<xsl:value-of select="rss/channel/language"/>;
		copyright=<xsl:value-of select="rss/channel/copyright"/>;
		lastBuildDate=<xsl:value-of select="rss/channel/lastBuildDate"/>;
		docs=<xsl:value-of select="rss/channel/docs"/>;
		generator=<xsl:value-of select="rss/channel/generator"/>;
		managingEditor=<xsl:value-of select="rss/channel/managingEditor"/>;
		webMaster=<xsl:value-of select="rss/channel/webMaster"/>;
		category=<xsl:value-of select="rss/channel/category"/>;
		domain=<xsl:value-of select="rss/channel/@domain"/>;
	</xsl:variable>


      <events>
   <xsl:for-each select="rss/channel/item">
			<event class="rss_rule">
            <xsl:copy-of select="$header" />;
				itemGUID=<xsl:value-of select="guid"/>;
				itemPubDate=<xsl:value-of select="pubDate"/>;
				itemCategory=<xsl:value-of select="category"/>;
            msg=<xsl:value-of select="title"/>;
            itemLink=<xsl:value-of select="link"/>;
            itemDescription=<xsl:value-of select="description"/>;
            severity=INFO;
			</event>
	</xsl:for-each>
		</events>
</xsl:template>

</xsl:transform>