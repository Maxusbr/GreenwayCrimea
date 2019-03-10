<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.fm.Default" Codebehind="Default.aspx.cs" %>
<%@ Register Src="~/Tools/fm/FileManagerControl/FileManagerControl.ascx" TagName="fm" TagPrefix="fm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>AdvantShop.Net 3.0 File Manager //
        <%=DateTime.Parse("08.08.2012").ToString("d MMM yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper()%>
    </title>
    <link href="FileManagerControl/WebFileManagerStyles.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><link href="FileManagerControl/WebFileManagerStyles-IE-67.css" rel="stylesheet" type="text/css" /><![endif]-->
    <link href="DynamicTreeStyles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" enctype="multipart/form-data" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnableHistory="true" runat="server">
    </asp:ScriptManager>
    <div>
        <fm:fm runat="server" />
    </div>
    </form>
</body>
</html>
