<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.NotInTariff" Title="Untitled Page" CodeBehind="NotInTariff.aspx.cs" %>

<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.Configuration" %>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder_Head">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="AdminSaasNotify">

        <span><%= Resource.Admin_DemoMode_CurrentTariff%> </span>
        <asp:Label ID="lblCurrentTariff" runat="server" Font-Bold="true"></asp:Label>
        <br>
        <br>
        <%= Resource.Admin_DemoMode_NotAvailableFeature%>
        <br>
        <br>
        <br>
        <asp:Panel ID="pnlNextSaasPlan" runat="server">
            <span><%= Resource.Admin_DemoMode_NextTariff%> </span>
            <asp:Label ID="lblNextTariff" runat="server" Font-Bold="true"></asp:Label>
            <ul class="saas-features">
                <li id="liProductsCount" runat="server" visible="false">
                    <span><%= Resource.Admin_SaaS_ProductsCount %>:</span>
                    <asp:Label ID="lblProductsCount" runat="server"></asp:Label>
                </li>
                <li id="liAllowCustom" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_HaveCustom %></span>
                </li>
                <li id="li1C" runat="server" visible="false">
                    <span><%= Resource.Admin_SaaS_1cIntegration %></span>
                </li>
                <li id="liCrm" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_CRM %></span>
                </li>
                <li id="liManagers" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_ManagersCount %>:</span>
                    <asp:Label ID="lblManagers" runat="server"></asp:Label>
                </li>
                <li id="liTelephony" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_Telephony %></span>
                </li>
                <li id="liCallback" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_Call30seconds %></span>
                </li>
                <li id="liMobileAdmin" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_MobilAdmin %></span>
                </li>
                <li id="liTags" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_Tags %></span>
                </li>
                <li id="liBonus" runat="server" visible="false">
                    <span><%= Resource.Admin_SaaS_BonusSystem %></span>
                </li>
                <li id="liUserTracking" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_UserTracking %></span>
                </li>
                <li id="liVipSupport" runat="server" visible="false">
                    <span><%= Resource.Admin_Saas_VipSupport %></span>
                </li>
            </ul>
            <a href="http://www.advantshop.net/login.aspx?mode=changeSaas&shopId=<%= SettingsLic.LicKey %>" class="btn btn-middle btn-add"><%= Resource.Admin_Saas_More %></a>
        </asp:Panel>
    </div>
</asp:Content>
