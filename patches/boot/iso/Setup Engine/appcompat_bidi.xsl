<?xml version="1.0"?>

<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns="http://www.w3.org/TR/REC-html40"
  version="1.0">
  <xsl:output method="html"/>

  <xsl:template match="/">
    <html>
      <head>
        <title></title>
      </head>
      <body STYLE="direction: rtl;unicode-bidi: normal;">

        <b>
          <xsl:for-each select="CompatReport">
             <font size = "2" face="Segoe UI" color="black">
                <xsl:copy-of select="Help"/>
             </font>
          </xsl:for-each>
        </b>

         <br></br>


         <xsl:for-each select="CompatReport/WarnAllApps">
          <table>
            <tr>
              <td style="width: 20px" valign="top">
                <xsl:element name="img">
                  <xsl:attribute name="src">
                    <xsl:value-of select="AppHead" />
                  </xsl:attribute>
                  <xsl:attribute name="align">
                    <xsl:copy-of select="'center'" />
                  </xsl:attribute>
                </xsl:element>
              </td>
              <td>
                <font size = "2" face="Segoe UI" color="#1370AB">
                  <xsl:copy-of select="Message"/>
                </font>
              </td>
            </tr>
          </table>
          <br />
        </xsl:for-each>


        <xsl:for-each select="CompatReport/StaticList">
          <table>
            <tr>
              <td style="width: 20px" valign="top">
                <xsl:element name="img">
                  <xsl:attribute name="src">
                    <xsl:value-of select="AppHead" />
                  </xsl:attribute>
                  <xsl:attribute name="align">
                    <xsl:copy-of select="'center'" />
                  </xsl:attribute>
                </xsl:element>
              </td>
              <td>
                <font size = "2" face="Segoe UI" color="#1370AB">
                  <xsl:copy-of select="Message"/>
                </font>
              </td>
            </tr>
          </table>
          <br />
        </xsl:for-each>
        <UL>
          <UL>
            <xsl:for-each select="CompatReport/StaticList/OcName/Name">
              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:value-of select="."/>
                </li>
              </font>
            </xsl:for-each>
          </UL>
        </UL>

        

        <xsl:for-each select="CompatReport/HardBlocks">
          <table>
            <tr>
              <td style="width: 20px" valign="top">
                <xsl:element name="img">
                  <xsl:attribute name="src">
                    <xsl:value-of select="AppHead" />
                  </xsl:attribute>
                  <xsl:attribute name="align">
                    <xsl:copy-of select="'center'" />
                  </xsl:attribute>
                </xsl:element>
              </td>
              <td>
                <font size = "2" face="Segoe UI" color="#1370AB">
                  <xsl:copy-of select="Message"/>
                </font>
              </td>
            </tr>
          </table>
          <br />
        </xsl:for-each>

        <UL>
          <xsl:for-each select="CompatReport/HardBlocks/ComplianceIssuePri1">
            <font size = "2" face="Segoe UI" color="black">
              <li>
                <xsl:value-of select="."/>
              </li>
              <br></br>
            </font>
          </xsl:for-each>
        </UL>
        <UL>
          <xsl:for-each select="CompatReport/HardBlocks/ComplianceIssuePri2">
            <font size = "2" face="Segoe UI" color="black">
              <li>
                <xsl:value-of select="."/>
              </li>
              <br></br>
            </font>
          </xsl:for-each>
        </UL>
        <UL>
          <xsl:for-each select="CompatReport/HardBlocks/ComplianceIssuePri3">
            <font size = "2" face="Segoe UI" color="black">
              <li>
                <xsl:value-of select="."/>
              </li>
              <br></br>
            </font>
          </xsl:for-each>
        </UL>
        <UL>
          <xsl:for-each select="CompatReport/HardBlocks/ComplianceIssueNoPri">
            <font size = "2" face="Segoe UI" color="black">
              <li>
                <xsl:value-of select="."/>
              </li>
              <br></br>
            </font>
          </xsl:for-each>
        </UL>

         <UL>
          <xsl:for-each select="CompatReport/HardBlocks">
            <font size = "2" face="Segoe UI" color="#1370AB">
              <xsl:copy-of select="Message6"/>
            </font>
          </xsl:for-each>
        </UL>

        <UL>
          <xsl:for-each select="CompatReport/HardBlocks">
            <font size = "2" face="Segoe UI" color="black">
              <xsl:copy-of select="Message5"/>
            </font>
          </xsl:for-each>
        </UL>
        <UL>
          <UL>
            <xsl:for-each select="CompatReport/HardBlocks/OcNames/OcName">
              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:copy-of select="Name"/>
                </li>
              </font>
            </xsl:for-each>
          </UL>
        </UL>
        <xsl:for-each select="CompatReport/HardBlocks/OcNames">
          <UL>
            <UL>
              <a href="#{generate-id(.)}">
                <font size = "2" face="Segoe UI" color="blue">
                  <xsl:value-of select="./Help"/>
                </font>
              </a>
            </UL>
          </UL>
        </xsl:for-each>



        <UL>
          <xsl:for-each select="CompatReport/HardBlocks">
            <font size = "2" face="Segoe UI" color="black">
              <xsl:copy-of select="Message3"/>
            </font>
          </xsl:for-each>
        </UL>
        <UL>
          <UL>
            <xsl:for-each select="CompatReport/HardBlocks/AppNames/AppName">
              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:copy-of select="Name"/>
                </li>
              </font>
            </xsl:for-each>
          </UL>
        </UL>
        <xsl:for-each select="CompatReport/HardBlocks/AppNames">
          <UL>
            <UL>
              <a href="#{generate-id(.)}">
                <font size = "2" face="Segoe UI" color="blue">
                  <xsl:value-of select="./Help"/>
                </font>
              </a>
            </UL>
          </UL>
        </xsl:for-each>

        <UL>
          <xsl:for-each select="CompatReport/HardBlocks">
            <font size = "2" face="Segoe UI" color="black">
              <xsl:copy-of select="Message2"/>
            </font>
          </xsl:for-each>
        </UL>



        <UL>
          <UL>
            <xsl:for-each select="CompatReport/HardBlocks/DrvNames/DrvName">
              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:copy-of select="Name"/>
                </li>
              </font>
            </xsl:for-each>
          </UL>
        </UL>
        <xsl:for-each select="CompatReport/HardBlocks/DrvNames">
          <UL>
            <UL>
              <a href="#{generate-id(.)}">
                <font size = "2" face="Segoe UI" color="blue">
                  <xsl:value-of select="./Help"/>
                </font>
              </a>
            </UL>
          </UL>
        </xsl:for-each>

        <UL>
          <xsl:for-each select="CompatReport/HardBlocks">
            <font size = "2" face="Segoe UI" color="black">
              <xsl:value-of select="Message0"/>
            </font>
          </xsl:for-each>
        </UL>

        <UL>
          <UL>
            <xsl:for-each select="CompatReport/HardBlocks/UnDrvNames/UnDrvName">

              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:copy-of select="Name"/>
                </li>
                <br></br>
              </font>
            </xsl:for-each>
          </UL>
        </UL>

        <xsl:for-each select="CompatReport/HardBlocks/UnDrvNames">
          <UL>
            <UL>
              <a href="#{generate-id(.)}">
                <font size = "2" face="Segoe UI" color="blue">
                  <xsl:value-of select="./Help"/>
                </font>
              </a>
            </UL>
          </UL>
        </xsl:for-each>

        <xsl:for-each select="CompatReport/SoftOcBlocks">
          <table>
            <tr>
              <td style="width: 20px" valign="top">
                <xsl:element name="img">
                  <xsl:attribute name="src">
                    <xsl:value-of select="AppHead" />
                  </xsl:attribute>
                  <xsl:attribute name="align">
                    <xsl:copy-of select="'center'" />
                  </xsl:attribute>
                </xsl:element>
              </td>
              <td>
                <font size = "2" face="Segoe UI" color="#1370AB">
                  <xsl:copy-of select="Message"/>
                </font>
              </td>
            </tr>
          </table>
          <br />
        </xsl:for-each>

        <UL>
          <xsl:for-each select="CompatReport/SoftOcBlocks">
            <font size = "2" face="Segoe UI" color="black">
              <xsl:copy-of select="Message1"/>
            </font>
          </xsl:for-each>
        </UL>

        <UL>
          <UL>
            <xsl:for-each select="CompatReport/SoftOcBlocks/OcNames/OcName">
              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:copy-of select="Name"/>
                </li>
              </font>
            </xsl:for-each>
          </UL>
        </UL>

        <xsl:for-each select="CompatReport/SoftOcBlocks/OcNames">
          <UL>
            <UL>
              <a href="#{generate-id(.)}">
                <font size = "2" face="Segoe UI" color="blue">
                  <xsl:value-of select="./Help"/>
                </font>
              </a>
            </UL>
          </UL>
        </xsl:for-each>

        <xsl:for-each select="CompatReport/SoftBlocks">
          <table>
            <tr>
              <td style="width: 20px" valign="top">
                <xsl:element name="img">
                  <xsl:attribute name="src">
                    <xsl:value-of select="AppHead" />
                  </xsl:attribute>
                  <xsl:attribute name="align">
                    <xsl:copy-of select="'center'" />
                  </xsl:attribute>
                </xsl:element>
              </td>
              <td>
                <font size = "2" face="Segoe UI" color="#1370AB">
                  <xsl:copy-of select="Message"/>
                </font>
              </td>
            </tr>
          </table>
          <br />
        </xsl:for-each>

        <UL>
          <xsl:for-each select="CompatReport/SoftBlocks">
            <font size = "2" face="Segoe UI" color="black">
              <xsl:copy-of select="Common"/>
            </font>
          </xsl:for-each>
        </UL>


        <UL>
          <xsl:for-each select="CompatReport/SoftBlocks">
            <xsl:for-each select="Links/link">
              <font size = "2" face="Segoe UI" color="blue">
                <a>
                  <xsl:attribute name="href">
                    <xsl:value-of select="@url"/>
                  </xsl:attribute>
                  <xsl:value-of select="@description"/>
                </a>
              </font>
            </xsl:for-each>
          </xsl:for-each>
        </UL>

        <xsl:for-each select="CompatReport/SoftBlocks/AppNames">
          <UL>
            <UL>
              <a href="#{generate-id(.)}">
                <font size = "2" face="Segoe UI" color="blue">
                  <xsl:value-of select="./Help"/>
                </font>
              </a>
            </UL>
          </UL>
        </xsl:for-each>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
