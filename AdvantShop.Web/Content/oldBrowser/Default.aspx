<%@ Page Language="C#" AutoEventWireup="true" Inherits="ClientPages.ie6_Default" Codebehind="Default.aspx.cs" %>
<%--<%@ Register TagPrefix="adv" TagName="LiveCounter" Src="~/UserControls/MasterPage/LiveinternetCounter.ascx" %>--%>
<%@ Import Namespace="AdvantShop.SEO" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<!DOCTYPE html>
<html>
<head>
    <base href="<%= UrlService.GetUrl() %>" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="content/oldBrowser/css/styles.css" />
</head>
<body>
    <%--<adv:LiveCounter runat="server" />--%>
    <%= new GoogleAnalyticsString().GetGoogleAnalyticsString()%>
    <form id="form" runat="server">
        <div class="wrap">
            <div class="main" id="current-ex2">
                <div class="qtcontent">
                    <h1>
                        <%=Resources.Resource.IE6_Caution %></h1>
                    <p>
                        <%=Resources.Resource.IE6_About %>
                    </p>
                    <p>
                        <%=Resources.Resource.IE6_Recommended %>
                    </p>
                    <br />
                    <table class="brows">
                        <tr>
                            <td>
                                <a target="_blank" href="http://www.microsoft.com/windows/Internet-explorer/default.aspx">
                                    <img src="content/oldBrowser/images/ie.jpg" alt="Internet Explorer" /></a>
                            </td>
                            <td>
                                <a target="_blank" href="http://www.opera.com/download/">
                                    <img src="content/oldBrowser/images/op.jpg" alt="Opera Browser" /></a>
                            </td>
                            <td>
                                <a target="_blank" href="http://www.mozilla.com/firefox/">
                                    <img src="content/oldBrowser/images/mf.jpg" alt="Mozilla Firefox" /></a>
                            </td>
                            <td>
                                <a target="_blank" href="http://www.google.com/chrome">
                                    <img src="content/oldBrowser/images/gc.jpg" alt="Google Chrome" /></a>
                            </td>
                        </tr>
                        <tr class="brows_name">
                            <td>
                                <a target="_blank" href="http://www.microsoft.com/windows/Internet-explorer/default.aspx">Internet Explorer</a>
                            </td>
                            <td>
                                <a target="_blank" href="http://www.opera.com/download/">Opera
                                Browser</a>
                            </td>
                            <td>
                                <a target="_blank" href="http://www.mozilla.com/firefox/">Mozilla
                                Firefox</a>
                            </td>
                            <td>
                                <a target="_blank" href="http://www.google.com/chrome">Google
                                Chrome</a>
                            </td>
                        </tr>
                    </table>
                    <h2>
                        <%=Resources.Resource.IE6_Why %></h2>
                    <p>
                        <%=Resources.Resource.IE6_OldBrowser %>
                    </p>
                    <p>
                        <%=Resources.Resource.IE6_PortableVersion %>
                    </p>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
