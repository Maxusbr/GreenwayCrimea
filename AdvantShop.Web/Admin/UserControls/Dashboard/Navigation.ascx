<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Dashboard.Navigation" Codebehind="Navigation.ascx.cs" %>
<article>
    <h2>
        <%= Resources.Resource.Admin_DashboardNavigation_QuickAccess %></h2>
    <menu class="dashboard-nav">
        <li id="dashCatalog" runat="server" class="dashboard-nav-item"><a href="product.aspx?categoryid=0" class="dashboard-nav-lnk">
            <figure class="dashboard-nav-image">
                <img src="images/new_admin/dashboard/add-product.png" alt="" />
            </figure>
            <figcaption class="dashboard-nav-text">
                <%= Resources.Resource.Admin_DashboardNavigation_AddProduct %>
            </figcaption>
        </a></li>
        <li id="dashOrder" runat="server" class="dashboard-nav-item"><a href="editorder.aspx?orderid=addnew" class="dashboard-nav-lnk">
            <figure class="dashboard-nav-image">
                <img src="images/new_admin/dashboard/add-order.png" alt="" />
            </figure>
            <figcaption class="dashboard-nav-text">
                <%= Resources.Resource.Admin_DashboardNavigation_AddOrder %>
            </figcaption>
        </a></li>
        <li id="dashNews" runat="server" class="dashboard-nav-item"><a href="EditNews.aspx" class="dashboard-nav-lnk">
            <figure class="dashboard-nav-image">
                <img src="images/new_admin/dashboard/add-news.png" alt="" />
            </figure>
            <figcaption class="dashboard-nav-text">
                <%= Resources.Resource.Admin_DashboardNavigation_AddNews %>
            </figcaption>
        </a></li>
        <li id="dashImportCsv" runat="server" class="dashboard-nav-item"><a href="importcsv.aspx" class="dashboard-nav-lnk">
            <figure class="dashboard-nav-image">
                <img src="images/new_admin/dashboard/load-excel.png" alt="" />
            </figure>
            <figcaption class="dashboard-nav-text">
                <%= Resources.Resource.Admin_DashboardNavigation_LoadCsv %>
            </figcaption>
        </a></li>
        <li id="dashModules" runat="server" class="dashboard-nav-item"><a href="modulesmanager.aspx" class="dashboard-nav-lnk">
            <figure class="dashboard-nav-image">
                <img src="images/new_admin/dashboard/add-module.png" alt="" />
            </figure>
            <figcaption class="dashboard-nav-text">
                <%= Resources.Resource.Admin_DashboardNavigation_AddModule %>
            </figcaption>
        </a></li>
        <li id="dashDesign" runat="server" class="dashboard-nav-item"><a href="designconstructor.aspx" class="dashboard-nav-lnk">
            <figure class="dashboard-nav-image">
                <img src="images/new_admin/dashboard/edit-design.png" alt="" />
            </figure>
            <figcaption class="dashboard-nav-text">
                <%= Resources.Resource.Admin_DashboardNavigation_ChangeDesign %>
            </figcaption>
        </a></li>
    </menu>
</article>