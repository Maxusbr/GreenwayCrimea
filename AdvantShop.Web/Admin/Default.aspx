<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.DefaultPage" Title="Untitled Page" Codebehind="Default.aspx.cs" %>

<%@ Register Src="~/Admin/UserControls/Dashboard/BigOrdersChart.ascx" TagName="BigOrdersChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/PlanProgressChart.ascx" TagName="PlanProgressChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/MasterPage/CurrentSaasData.ascx" TagName="CurrentSaasData"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/IndicatorsStatistic.ascx" TagName="IndicatorsStatistic"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/LastOrders.ascx" TagName="LastOrders"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/GoogleAnalyticStatistic.ascx" TagName="GoogleAnaliticStatistic"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Order/OrdersStatuses.ascx" TagName="OrdersStatuses"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/LastTasks.ascx" TagName="LastTasks"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/LastLeads.ascx" TagName="LastLeads"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/UserInformation/AdminUserInformation.ascx" TagName="AdminUserInformation"
    TagPrefix="adv" %>
<%@ Register TagPrefix="adv" TagName="SearchStatistic" Src="~/Admin/UserControls/Dashboard/SearchStatistic.ascx" %>
<%@ Register TagPrefix="adv" TagName="DashboardNavigation" Src="~/Admin/UserControls/Dashboard/Navigation.ascx" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder_Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ul class="three-column">
        <li class="three-column-item">
            <div class="three-column-inside">
                <adv:OrdersStatuses ID="ordersStatuses" runat="server" />
                <adv:IndicatorsStatistic ID="IndicatorsStatistic" runat="server" />
            </div>
        </li>
        <li class="three-column-item">
            <div class="three-column-inside">
                <adv:DashboardNavigation ID="DashboardNavigation" runat="server" />
                <adv:BigOrdersChart ID="BigOrdersChart" runat="server" />
                <adv:LastOrders ID="LastOrders" runat="server" />
                <adv:LastLeads ID="LastLeads" runat="server" />
                <adv:LastTasks ID="LastTasks" runat="server" />
            </div>
        </li>
        <li class="three-column-item">
            <div class="three-column-inside">
                <adv:PlanProgressChart ID="PlanProgressChart" runat="server" />
                <adv:GoogleAnaliticStatistic ID="GoogleAnaliticStatistic" runat="server" />
                <adv:SearchStatistic ID="SearchStatistic" runat="server" />
            </div>
        </li>
    </ul>
    <adv:CurrentSaasData ID="CurrentSaasData" runat="server" />
    <adv:AdminUserInformation ID="AdminUserInformation" runat="server" Visible="true" />
</asp:Content>
