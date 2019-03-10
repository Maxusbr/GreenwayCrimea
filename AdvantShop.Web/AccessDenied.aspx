<%@ Page Language="C#" AutoEventWireup="true" Inherits="ClientPages.AccessDenied" Codebehind="AccessDenied.aspx.cs" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Resources.Resource.Client_AccessDenied_PageTitle%></title>
    <style type="text/css">
        #AccessDeniedMainDiv {
            width: 100%;
            text-align: center;
            margin-top: 50px;
        }

        #AccessDeniedSubDivText {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 20px;
        }

        #AccessDeniedSubDivLink {
            font-size: 13px;
            font-weight: bold;
        }

        a {
            color: Blue;
        }
    </style>
</head>
<body style="font-family: Tahoma;">
    <form id="form1" runat="server">
    <div id="AccessDeniedMainDiv">
        <div id="AccessDeniedSubDivText">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Client_AccessDenied_AccessDeniedText %>"></asp:Localize>
        </div>
        <div id="AccessDeniedSubDivLink">
            <a href="<%= UrlService.GetAbsoluteLink("/") %>">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Client_AccessDenied_Sublink %>"></asp:Literal></a>
        </div>
    </div>
    </form>
</body>
</html>