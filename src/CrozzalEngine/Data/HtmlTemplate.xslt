<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/crozzle">
    <html>
      <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
      <head>
        <title>Crozzle simple web template</title>
        <style>
          h1.success
          {
            color: green;
          }
          
          h1.fail
          {
            color: red;
          }
          
          td
          {
            background-color: aquamarine;
            border: 1px solid blue;
            width: 40px;
            height: 40px;
            text-align: center;
            font-size: 20px;
          }
          td:hover
          {
            background-color: aquamarine;
            border: 1px solid red;
            color: red;
          }
          .empty
          {
            background-color: gray;
            border: 1px solid black;
            width: 20px;
            height: 20px;
            padding: 2px 2px 2px 10px
          }
          .empty:hover
          {
            background-color: gray;
            border: 1px solid red;
            width: 20px;
            height: 20px;
            padding: 2px 2px 2px 10px
          }
          table.center
          {
            margin-left:auto;
            margin-right:auto;
          }
          td.error
          {
            border: 1px solid red;
            color: red;
          }
        </style>
      </head>
      <body>
        <h1 align="center" style="color: blue">
          Your level is: <xsl:value-of select="difficult" />
        </h1>
        <xsl:choose>
          <xsl:when test="validation[@valid='false']">
            <h1 align="center" class="fail">
              Your score is: 0
            </h1>
            <h1 align="center" class="fail">
              Error count is: <xsl:value-of select="validation/errors" />
            </h1>
          </xsl:when>
          <xsl:otherwise>
            <h1 align="center" class="success">
              Your score is: <xsl:value-of select="score" />
            </h1>
          </xsl:otherwise>
        </xsl:choose>
        <table class="center">
          <xsl:for-each select="rows/row">
            <tr>
              <xsl:for-each select="cell">
                <xsl:choose>
                  <xsl:when test=". != ''">
                    <td>
                      <xsl:value-of select="."/>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td class="empty">
                      <xsl:value-of select="."/>
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:for-each>
            </tr>
          </xsl:for-each>
        </table>
        <br />
        <br />
        <div align="center">
          <xsl:for-each select="validation/messages">
            <xsl:for-each select="message">
              <b style="color: red">
                <xsl:value-of select="."/>
              </b>
              <br />
            </xsl:for-each>
          </xsl:for-each>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>