<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="NT System Monitor" match="ntsystem" >
        <xsl:param name="hostname" select="@hostname" />

        <group name="NT on {$hostname}" class="NTServer">
            <filter>
                <field name="source" value="{$hostname}" />
                <field name="sub_source" value="SP_DM.NT" operator="starts_with" />
            </filter>
            <group name="Percentage Processor Time" class="Process">
                <filter>
                    <field name="probe" value="PrcCpuTime" />
                </filter>
            </group>
            <group name="Network Collisions" class="Process">
                <filter>
                    <field name="probe" value="netcoll" />
                </filter>
            </group>
            <group name="Committed Bytes" class="Process">
                <filter>
                    <field name="probe" value="CommittedBytes" />
                </filter>
            </group>
            <group name="One Minute Load Average" class="Process">
                <filter>
                    <field name="probe" value="ResrcCPUOneMin" />
                </filter>
            </group>
            <group name="Application Log Error" class="Process">
                <filter>
                    <field name="probe" value="Apperrevent" />
                </filter>
            </group>
            <group name="Percent Free Space" class="Process">
                <filter>
                    <field name="probe" value="PrcFreeSpace" />
                </filter>
                <generator field="probe_arg" class="Process" />
            </group>
            <group name="PageFile Percent Usage" class="Process">
                <filter>
                    <field name="probe" value="PrcUsage" />
                </filter>
            </group>
            <group name="System Log Warning" class="Process">
                <filter>
                    <field name="probe" value="Syswarnevent" />
                </filter>
            </group>
            <group name="System Log Error" class="Process">
                <filter>
                    <field name="probe" value="Syserrevent" />
                </filter>
            </group>
            <group name="Output Packets" class="Process">
                <filter>
                    <field name="probe" value="netout" />
                </filter>
            </group>
            <group name="Input Packets" class="Process">
                <filter>
                    <field name="probe" value="netin" />
                </filter>
            </group>
        </group>

    </xsl:template>

    <xsl:template match="*|@*">
      <xsl:copy>
        <xsl:apply-templates select="@*"/>
        <xsl:apply-templates select="node()"/>
      </xsl:copy>
    </xsl:template>

</xsl:stylesheet>

