<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Catalog.CategoryView" Codebehind="CategoryView.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<script type="text/javascript">

    $(document).ready(function () {
        $(".panel-do-categories-count").text(0);

        $("#commandButtonForCategories").on('click', function () {
            var command = $("#commandSelectForCategories").val();

            switch (command) {
                case "selectAll":
                    SelectAllCategories(true);
                    break;
                case "unselectAll":
                    SelectAllCategories(false);
                    break;
                case "deleteSelected":
                    var r = confirm("<%= Resources.Resource.Admin_Catalog_Confirm%>");
                    if (r) { window.__doPostBack('<%=lbDeleteSelectedCategories.UniqueID%>', ''); $("selectedCategoriesIdsCount").text("0"); }
                    break;
            }
        });

        $(".categorieslistname input").on('click', function (e) {
            $("#selectedCategoriesIdsCount").text($(".categorieslistname input:checked").length);
        });
    });

    function SelectAllCategories(xState) {
        $(".categorieslistname input").attr("checked", xState);
        $("#selectedCategoriesIdsCount").text($(".categorieslistname input:checked").length);
    }
</script>
<div runat="server" id="commandPanel">
    <ul class="justify panel-do-grid">
        <li class="justify-item panel-do-grid-item">
            <select id="commandSelectForCategories">
                <option value="selectAll">
                    <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                </option>
                <option value="unselectAll">
                    <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                </option>
                <option value="deleteSelected">
                    <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                </option>
            </select>
            <a href="javascript:void(0)" class="btn btn-middle btn-action btn-do-grid" id="commandButtonForCategories">
                <%= Resource.Admin_Catalog_GO %></a>
            <asp:LinkButton ID="lbDeleteSelectedCategories" Style="display: none" runat="server"
                OnClick="lbDeleteSelectedCategories_Click" />
            <span class="panel-do-grid-selected-rows"><span id="selectedCategoriesIdsCount" class="panel-do-grid-count">0
            </span>
                <%=Resource.Admin_Catalog_ItemsSelected%>
            </span></li>
        <li class="justify-item panel-do-grid-item"><span class="subcategories-count-wrap">
            <%= Resource.Admin_CategoriesService_Categories%>
            <asp:Label ID="lblCategories" CssClass="panel-do-grid-count" runat="server"
                Text="" /></span></li>
    </ul>
</div>
<asp:ListView runat="server" ID="lvCategories" ItemPlaceholderID="liCategory" OnItemCommand="lvCategories_OnItemCommand">
    <LayoutTemplate>
        <ul class="list-categories">
            <li runat="server" id="liCategory" />
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li class="list-categories-item">
            <div class="deleteCategoryButton">
                <a href="<%# "m_Category.aspx?CategoryID=" + Eval("CategoryID") %>"
                    class="editbtn showtooltip">
                    <img src="images/new_admin/cliparts/pencil.jpg" style="border: none;" alt='<%= Resource.Admin_MasterPageAdminCatalog_Edit%>' /></a>
                <asp:LinkButton ID="ibtnDeleteCategory" runat="server" CommandArgument='<%#Eval("CategoryID") %>' CommandName="DeleteCategory" CssClass="valid-confirm"
                    data-confirm="<%$ Resources:Resource, Admin_Product_ConfirmDeletingCategory %>">
                    <img src="images/deletebtn.png" style="border: none;" alt='<%= Resource.Admin_MasterPageAdminCatalog_Delete%>' />
                </asp:LinkButton>
            </div>
            <a href="Catalog.aspx?categoryID=<%# Eval("CategoryID") %>" class="list-categories-lnk">
                <figure class="list-categories-photo">
                    <%# RenderCategoryImage(SQLDataHelper.GetString(Eval("MiniPicture.PhotoName")), SQLDataHelper.GetString(Eval("Name"))) %>
                </figure>
            </a>
            <span class="list-categories-text">
                <asp:CheckBox ID="ckbSelectCategory" runat="server" CssClass="categorieslistname" data-categorylist-categoryid='<%# Eval("CategoryID") %>' />
                <a href="Catalog.aspx?categoryID=<%# Eval("CategoryID") %>" class="list-categories-lnk">
                    <%# Eval("Name") %>
                    <span class="list-categories-pcount">(<%# Eval("ProductsCount") %>)</span>
                </a>
            </span>
        </li>
    </ItemTemplate>
    <EmptyDataTemplate>
        <div style="font-size: 14px">
            <% if (CategoryID == 0)
               { %>
            <%= Resource.Admin_Catalog_CatalogIsEmpty%>
            <a href="m_Category.aspx?ParentCategoryID=<%=Request["categoryid"] ?? "0" %>" class="panel-add-lnk">
                <%= Resource.Admin_Catalog_CreateCategory%></a>
            <% } %>
        </div>
    </EmptyDataTemplate>
</asp:ListView>

<div style="font-size: 14px">
    <% if (CategoryID != 0)
        { %>
        <a href="m_Category.aspx?ParentCategoryID=<%=Request["categoryid"] ?? "0" %>" class="panel-add-lnk">
            <%= Resource.Admin_Catalog_CreateSubCategory%></a>
    <% } else if (lvCategories.Items.Count > 0) { %>
        <a href="m_Category.aspx?ParentCategoryID=<%=Request["categoryid"] ?? "0" %>" class="panel-add-lnk">
            <%= Resource.Admin_Catalog_CreateNewCategory%></a>
    <% } %>
</div>
