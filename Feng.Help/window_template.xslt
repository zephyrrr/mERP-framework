<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="UTF-8"/>
  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <META HTTP-EQUIV="Content-Type" CONTENT="text/html; charset=UTF-8" />
        <META NAME="MS.LOCALE" CONTENT="ZH-CN" />
        <link rel='stylesheet' type='text/css' href='./style/help.css' />
        <title>
          <xsl:value-of select="Window/@Text"/>
        </title>
      </head>

      <body>
        <!--顶部(Window)-->
        <div>
          <center>
            <b class="title">
              <xsl:value-of select="Window/@Text" />
            </b>
          </center>
          <p>
            <xsl:value-of select="Window/@Help" disable-output-escaping="yes" />
          </p>
        </div>

        <!--按钮功能(Window_Menu)-->
        <div>
          <b class="subtitle">按钮功能</b>
          <table>
            <thead>
              <tr>
                <th scope="col">名称</th>
                <th scope="col">图标</th>
                <th scope="col">说明</th>
              </tr>
            </thead>
            <xsl:for-each select="Window/WindowMenu">
              <tr onMouseOver="this.className='over'" onMouseOut="this.className='out'">
                <td>
                  <xsl:value-of select="@Text" />
                </td>
                <td>
                  <img src="{concat('image/', @Icon, '.png')}" width="20px" height="20px"/>
                </td>
                <td>
                  <xsl:value-of select="@Help" disable-output-escaping="yes" />
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>

        <!--表格数据(Window_Tab)-->
        <div>
          <b class="subtitle">表格数据</b>
          <table>
            <thead>
              <tr>
                <th scope="col">名称</th>
                <th scope="col">表格名称</th>
                <th scope="col">说明</th>
              </tr>
            </thead>
            <xsl:for-each select="Window/WindowTab">
              <tr onMouseOver="this.className='over'" onMouseOut="this.className='out'">
                <td>
                  <xsl:value-of select="@Text" />
                </td>
                <td>
                  <a href="{concat('grid_', @GridName, '.html')}">
                    <xsl:value-of select="@GridName" />
                  </a>
                </td>
                <td>
                  <xsl:value-of select="@Help" disable-output-escaping="yes" />
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>

        <!--相关信息(Grid_Related)-->
        <div>
          <b class="subtitle">相关信息</b>
          <table>
            <thead>
              <tr>
                <th scope="col">名称</th>
                <th scope="col">目标窗体名称</th>
                <th scope="col">说明</th>
              </tr>
            </thead>
            <xsl:for-each select="Window/WindowTab/GridRelated">
              <tr onMouseOver="this.className='over'" onMouseOut="this.className='out'">
                <td>
                  <xsl:value-of select="@Text" />
                </td>
                <td>
                  <a href="{concat('window_', @ActionWindowName, '.html')}" >
                    <xsl:value-of select="@ActionWindowText" />
                  </a>
                </td>
                <td>
                  <xsl:value-of select="@Help" disable-output-escaping="yes" />
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>

        <!--预定义(Search_Custom)-->
        <div>
          <b class="subtitle">预定义</b>
          <table>
            <thead>
              <tr>
                <th scope="col">名称</th>
                <th scope="col">说明</th>
              </tr>
            </thead>
            <xsl:for-each select="Window/WindowTab/SearchCustom">
              <tr onMouseOver="this.className='over'" onMouseOut="this.className='out'">
                <td>
                  <xsl:value-of select="@Text" />
                </td>
                <td>
                  <xsl:value-of select="@Help" disable-output-escaping="yes" />
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>

        <!--附件(Attachment)-->
        <div>
          <b class="subtitle">详细文档</b>
          <table>
            <thead>
              <tr>
                <th scope="col">名称</th>
              </tr>
            </thead>
            <xsl:for-each select="Window/Attachment">
              <tr onMouseOver="this.className='over'" onMouseOut="this.className='out'">
                <td>
                  <a href="{concat('attachment/', @FileName)}" target="_blank">
                    <xsl:value-of select="@Description" />
                  </a>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>
      </body>
    </html>

  </xsl:template>
</xsl:stylesheet>