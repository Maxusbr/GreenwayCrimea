<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="false" AutoEventWireup="true" Inherits="Admin.VotingHistory" Codebehind="VotingHistory.aspx.cs" %>

<%@ Import Namespace="Resources" %>

<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="ContentVoting" ContentPlaceHolderID="cphMain" runat="server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Discount_PriceRange.aspx">
                <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
            <li class="neighbor-menu-item"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>

            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resources.Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent selected"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_Voting %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_VotingHistory %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item"><a href="Statistics.aspx">
                <%= Resources.Resource.Admin_Statistics_Header %></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <div style="text-align: center;">
        <span class="AdminHead">
            <%=Resources.Resource.Admin_VotingHistory_Voting%></span>
        <br />
        <span class="AdminSubHead">
            <%=Resources.Resource.Admin_VotingHistory_VotingHistory%></span><br />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 400px; height: 100%;">
            <tr>
                <td align="center">
                    <div class="PagesNavigateVoice">
                        <%=GetPagesIndex()%>
                    </div>
                    <%=GetHtmlTableVoiceThemes()%>
                    <div class="PagesNavigateVoice">
                        <%=GetPagesIndex()%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
