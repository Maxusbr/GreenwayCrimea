<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.MasterPage.AdminSearch" Codebehind="AdminSearch.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>
<div class="justify-item search-wrap">
    <div class="search">
        <div class="search-cell">
            <input type="text" onkeypress="return defaultButtonClick('btnAdminSearch', event)" value="<%= searchRequest%>" id="txtAdminSearch" placeholder="<%= Resource.Admin_MasterPageAdmin_Search %>"
                class="search-input" />
        </div>
        <div class="search-cell search-right">
            <div id="searchSubmenuContainer" class="search-category dropdown-arrow-dark">
                <span class="search-cat" id="searchHeader" data-href="<%=SettingsMain.SearchPage %>">
                    <%= SettingsMain.SearchArea ?? Resource.Admin_MasterPageAdmin_SearchInProducts%></span>
                <div id="searchSubmenu" class="dropdown-menu-wrap dropdown-menu-invert">
                    <ul class="dropdown-menu" id="liSearchItems">
                        <li class="dropdown-menu-item"><a href="Catalog.aspx?CategoryID=AllProducts" data-placeholder="<%= Resources.Resource.Admin_AdminSearch_PlaceholderProduct %>" data-type="product"><%= Resource.Admin_MasterPageAdmin_SearchInProducts %></a></li>
                        <li class="dropdown-menu-item"><a href="OrderSearch.aspx" data-placeholder="<%= Resources.Resource.Admin_AdminSearch_PlaceholderOrder %>" data-type="order"><%= Resource.Admin_MasterPageAdmin_SearchInOrders %></a></li>
                        <li class="dropdown-menu-item"><a href="CustomerSearch.aspx" data-placeholder="<%= Resources.Resource.Admin_AdminSearch_PlaceholderCustomer %>" data-type="customer"><%= Resource.Admin_MasterPageAdmin_SearchInCustomers %></a></li>
                    </ul>
                </div>
            </div>
            <a href="javascript:void(0);" class="search-btn" id="btnAdminSearch">&nbsp;</a>
        </div>
    </div>
</div>
