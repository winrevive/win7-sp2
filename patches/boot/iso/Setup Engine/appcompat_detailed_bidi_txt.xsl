<?xml version="1.0"?>

<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns="http://www.w3.org/TR/REC-html40"
  version="1.0">
  <xsl:output method="text"/>

  <xsl:template match="/">
    <html>
      <head>
        <title></title>
      </head>
      <body STYLE="direction: rtl;unicode-bidi: normal;">

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


         <xsl:for-each select="CompatReport/Blocks">
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
          <xsl:for-each select="CompatReport/HardBlocks/Listing">
            <font size = "2" face="Segoe UI" color="black">
            <li>
              <xsl:copy-of select="Message6"/>
            </li>
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
          <xsl:for-each select="CompatReport/SoftOcBlocks/ComplianceIssueWarnings">
            <font size = "2" face="Segoe UI" color="black">
               <li>
                <xsl:value-of select="."/>
              </li>
               <br></br>
            </font>
          </xsl:for-each>
        </UL>

        <UL>
          <xsl:for-each select="CompatReport/SoftOcBlocks/Listing">
            <font size = "2" face="Segoe UI" color="black">
            <li>
              <xsl:copy-of select="Message1"/>
            </li>
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
            <font size = "2" face="Segoe UI" color="black">
              <xsl:copy-of select="Message2"/>
            </font>
          </xsl:for-each>
        </UL>

        <UL>
          <UL>
            <xsl:for-each select="CompatReport/SoftBlocks/DrvNames/DrvName">
              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:copy-of select="Name"/>
                </li>
              </font>
            </xsl:for-each>
          </UL>
        </UL>

        <xsl:for-each select="CompatReport/SoftBlocks/DrvNames">
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
          <xsl:for-each select="CompatReport/SoftBlocks">
            <font size = "2" face="Segoe UI" color="#1370AB">
              <xsl:copy-of select="Message4"/>
            </font>
          </xsl:for-each>
        </UL>


        <UL>
          <UL>
            <xsl:for-each select="CompatReport/SoftBlocks/AppNames/AppName">
              <font size = "2" face="Segoe UI" color="black">
                <li>
                  <xsl:copy-of select="Name"/>
                </li>
              </font>
            </xsl:for-each>
          </UL>
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
