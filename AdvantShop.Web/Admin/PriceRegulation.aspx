<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.PriceRegulation" Codebehind="PriceRegulation.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<%@ Register Src="~/Admin/UserControls/ChangePrice.ascx" TagName="ChangePrice"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
        <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Catalog.aspx">
                <%= Resource.Admin_MasterPageAdmin_CategoryAndProducts %></a></li>
            <li class="neighbor-menu-item"><a href="ProductsOnMain.aspx?type=New">
                <%= Resource.Admin_MasterPageAdminCatalog_FirstPageProducts%></a></li>
            <li class="neighbor-menu-item"><a href="Properties.aspx">
                <%= Resource.Admin_MasterPageAdmin_ProductProperties%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx">
                <%= Resource.Admin_MasterPageAdmin_Export%></a></li>
            <li class="neighbor-menu-item"><a href="ImportCSV.aspx">
                <%= Resource.Admin_MasterPageAdmin_Import%></a></li>
            <li class="neighbor-menu-item"><a href="Brands.aspx">
                <%= Resource.Admin_MasterPageAdmin_Brands%></a></li>
            <li class="neighbor-menu-item"><a href="Reviews.aspx">
                <%= Resource.Admin_MasterPageAdmin_Reviews%></a></li>
        </menu>
        <div class="panel-add">
            <%= Resource.Admin_MasterPageAdmin_Add %>
            <a href="Product.aspx?categoryid=<%=Request["categoryid"] ?? "0" %>" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Product %></a>, <a href="#" onclick="open_window('m_Category.aspx?CategoryID=<%=Request["categoryid"] ?? "0" %>&mode=create', 750, 640); return false;"
                    class="panel-add-lnk">
                    <%= Resource.Admin_MasterPageAdmin_Category %></a>
        </div>
    </div>
    <div class="content-own">
        <div id="mainDiv" runat="server">
            <center>
                <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_PriceRegulation_Header %>"></asp:Label><br />
                <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_PriceRegulation_SubHeader %>"></asp:Label>
                <br />
            </center>
            <br />
            <br />
            <div style="text-align: center">
                <adv:ChangePrice ID="ChangePrice" runat="server" />
            </div>
        </div>
        <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
            <center>
            <h2>
                <%= Resources.Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </center>
        </div>
    </div>
</asp:Content>
