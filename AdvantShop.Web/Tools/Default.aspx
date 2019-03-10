<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.Default" CodeBehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdvantShop.NET Tools for 5.x</title>
</head>
<body>
    <form id="form1" runat="server">
        <span style="font-family: Tahoma; font-weight: bold;">AdvantShop.NET tools for 5.x</span> -
        <asp:HyperLink ID="HyperLink6" runat="server"
            ForeColor="Green" NavigateUrl="~/Default.aspx" Text="Back to main page"></asp:HyperLink>
        <br />
        <br />
        <div style="margin-left: 15px;">
            <a href="cntest_tools.aspx" style="color: Blue;">Connection test</a>
            <br />
            <br />
            <a href="cnhelper.aspx" style="color: Blue;">Build SQL Connection</a>
            <br />
            <br />
            <a href="sendmail.aspx" style="color: Blue;">Send mail tool</a>
            <br />
            <br />
            <a href="pool.aspx" style="color: Blue;">IIS Pool</a>
            <br />
            <br />
            <a href="session.aspx" style="color: Blue;">Show session ID</a>
            <br />
            <br />
            <div style="color: black; font-family: Tahoma; font-size: 14px;">
                <%=AdvantShop.Configuration.SettingsGeneral.SiteVersion%>
            </div>
            <br />
            <div style="color: black; font-family: Tahoma; font-size: 14px;">
                IP:
                <input type="text" value='<%=Request.UserHostAddress%>' />
            </div>
        </div>
    </form>
</body>
</html>
