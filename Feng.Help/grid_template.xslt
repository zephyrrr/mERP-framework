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
    <xsl:value-of select="Grid/@GridName"/>
  </title>
</head> 
  
<body>
<!--顶部(Window)-->
<div>
<center><b class="title"><xsl:value-of select="Grid/@GridName"/></b></center>
</div>
<!--列信息(Grid_Column)-->
<div>
<b class="subtitle">列信息</b>
<table>
<thead>
<tr>
	<th scope="col">列名</th>
	<th scope="col">数据列</th>
	<th scope="col">搜索列</th>
	<th scope="col">说明</th>
</tr>
</thead>

<xsl:for-each select="Grid/GridColumn">
<tr onMouseOver="this.className='over'" onMouseOut="this.className='out'">
	<td><xsl:value-of select="@Caption" /></td>
	<td><b><xsl:if test="@IsDataControl = 'True'">√</xsl:if>
    <xsl:if test="@IsDataControl = 'False'">×</xsl:if>
  </b></td>
	<td><b><xsl:if test="@IsSearchControl = 'True'">√</xsl:if>
    <xsl:if test="@IsSearchControl = 'False'">×</xsl:if>
  </b></td>
	<td><xsl:value-of select="@Help" disable-output-escaping="yes" /></td>
</tr>
</xsl:for-each>
</table>
</div>

</body>
</html>

</xsl:template>
</xsl:stylesheet>