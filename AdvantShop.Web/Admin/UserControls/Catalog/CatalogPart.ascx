<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Catalog.CatalogPart" Codebehind="CatalogPart.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<div class="justify">
    <h2 class="justify-item products-header">
        <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Products %>"></asp:Localize></h2>
    
    <div class="justify-item products-header-controls">
        <a href="javascript:void(0);" class="showtooltip products-header-controls" title="<%= Resources.Resource.Admin_MasterPageAdminCatalog_AddNewProduct %>" data-add-product-call>
            <img src="images/gplus.gif" onmouseover="this.src='images/bplus.gif'" onmouseout="this.src='images/gplus.gif';"/></a>
        <asp:LinkButton runat="server" ID="lbRecalculateProducts" CssClass="showtooltip" ToolTip="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Recalculate%>" 
            OnClick="lbRecalculate_Click">
            <img src="images/groundarrow.gif" alt="" onmouseover="this.src='images/broundarrow.gif';" onmouseout="this.src='images/groundarrow.gif';" />
        </asp:LinkButton>
    </div>
</div>
<ul class="list-products-count">
    <li class="list-products-count-item"><a href="Catalog.aspx?CategoryID=AllProducts"
        class="list-products-count-lnk <%= selectedItem == SelectedItem.AllProducts ? "list-products-count-selected" : "" %>">
        <span class="list-products-count-cat list-products-count-all">
            <%= Resources.Resource.Admin_CatalogPart_AllProducts%>
        </span>&nbsp;<span class="list-products-count-number">
            <%= ProductService.GetProductsCount()%></span></a></li>
    
    <li class="list-products-count-item"><a href="Catalog.aspx?CategoryID=WithoutCategory"
        class="list-products-count-lnk <%= selectedItem == SelectedItem.WithoutCategory ? "list-products-count-selected" : "" %>">
        <span class="list-products-count-cat list-products-count-withoutcategory">
            <%= Resources.Resource.Admin_CatalogPart_WithoutCategories%>
        </span><span class="list-products-count-number">
            <%= CategoryService.GetTolatCounTofProductsWithoutCategories()%></span></a></li>
   
    <li class="list-products-count-item"><a href="ProductsOnMain.aspx?type=Best"
        class="list-products-count-lnk <%= selectedItem == SelectedItem.Best ? "list-products-count-selected" : "" %>">
        <span class="list-products-count-cat list-products-count-best">
            <%= Resources.Resource.Admin_CatalogPart_Best%>
        </span><span class="list-products-count-number">
            <%= ProductOnMain.GetProductCountByType(EProductOnMain.Best)%></span></a></li>
    
    <li class="list-products-count-item"><a href="ProductsOnMain.aspx?type=New" class="list-products-count-lnk <%= selectedItem == SelectedItem.New ? "list-products-count-selected" : "" %>">
        <span class="list-products-count-cat list-products-count-new">
            <%= Resources.Resource.Admin_CatalogPart_New%></span> <span class="list-products-count-number">
                <%= ProductOnMain.GetProductCountByType(EProductOnMain.New)%></span></a></li>
    
    <li class="list-products-count-item"><a href="ProductsOnMain.aspx?type=Sale"
        class="list-products-count-lnk <%= selectedItem == SelectedItem.Sale ? "list-products-count-selected" : "" %>">
        <span class="list-products-count-cat list-products-count-discount">
            <%= Resources.Resource.Admin_CatalogPart_Discount%>
        </span><span class="list-products-count-number">
            <%= ProductOnMain.GetProductCountByType(EProductOnMain.Sale)%></span></a></li>
</ul>
