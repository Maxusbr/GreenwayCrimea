<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarketingNeighborMenu.ascx.cs" Inherits="AdvantShop.Admin.UserControls.Menu.MarketingNeighborMenu" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="Resources" %>
<menu class="neighbor-menu neighbor-catalog">
    <li class="neighbor-menu-item"><a href="Discount_PriceRange.aspx">
        <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
    <li class="neighbor-menu-item"><a href="Coupons.aspx">
        <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
    <li class="neighbor-menu-item"><a href="Certificates.aspx">
        <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
    <li class="neighbor-menu-item"><a href="SendMessage.aspx">
        <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
    <li class="neighbor-menu-item"><a href="LandingPage.aspx">
        <%= Resource.Admin_MasterPageAdmin_LandingPage%></a></li>
    <li class="neighbor-menu-item"><a href="BonusSystemAdmin.aspx">
        <%= Resource.Admin_MasterPageAdmin_BonusSystem%></a></li>
    <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
        <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
    <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
        <%= Resources.Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
    <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
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
