<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.SiteMapGenerate" Codebehind="SiteMapGenerate.aspx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
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
                <%= Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resource.Admin_MasterPageAdmin_Voting %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resource.Admin_MasterPageAdmin_VotingHistory %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item"><a href="Statistics.aspx">
                <%= Resource.Admin_Statistics_Header %></a></li>
            <li class="neighbor-menu-item selected"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div style="text-align: center;">
            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Header %>" /><br />
            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_SubHead %>" />
        </div>
        <div style="text-align: center; margin-bottom: 20px;">
            <asp:Label ID="lblError" runat="server" ForeColor="Blue" Visible="False"></asp:Label>
        </div>
        <table class="table-ui">
            <tr>
                <td class="table-ui-align-right table-ui-font-bold" style="width: 50%;">
                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_ModDateSiteMap %>" />
                </td>
                <td>
                    <asp:Label ID="lastMod" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="table-ui-align-right table-ui-font-bold">
                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_LinkSiteMap %>" />
                </td>
                <td>
                    <%= SettingsMain.SiteUrl + LinkToSiteMapFile %>
                    ( <a href="<%= LinkToSiteMapFile + ("?" + new Random().Next()) %>" target="_blank">
                          <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_LinkSiteMapGo %>" />
                      </a> )
                </td>
            </tr>
            <tr>
                <td class="table-ui-align-right table-ui-font-bold">
                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Schedule %>" />
                </td>
                <td>
                    <a href="CommonSettings.aspx#tabid=task"><asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Set %>" /></a> 
                </td>
            </tr>
        </table>
        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="btnCreateMap" runat="server" OnClick="btnCreateMap_Click" CssClass="btn btn-middle btn-add"
                Text="<%$ Resources:Resource,Admin_SiteMapGenerate_ButtonGenerate%>" />        
        </div>
    </div>
</asp:Content>
