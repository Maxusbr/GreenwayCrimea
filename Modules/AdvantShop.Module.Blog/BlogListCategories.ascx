<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Blog.BlogListCategories" Codebehind="BlogListCategories.ascx.cs" %>

<style>
    .reviewsTable{width: 100%;border-collapse: collapse;}
    .reviewsTable td, .reviewsTable th{text-align: left;border-bottom: 1px solid #000000;height: 30px;}
    #pnlAddBlogCategory input[type=text]{width: 200px;}
    #pnlAddBlogCategory table td:nth-child(1){width: 180px;}
</style>
<div>
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
    </table>
    <div>
        <a class="btn btn-middle btn-add" href="javascript:open_window('../modules/blog/blogEditCategory.aspx?id=0',700,600)">
            <asp:Literal runat="server" Text="<%$ Resources: AddNewCategory%>" />
        </a>
    </div>
    <asp:ListView ID="lvBlogCategories" runat="server" ItemPlaceholderID="itemPlaceHolder" OnItemCommand="lvBlogCategoriesItemCommand">
        <LayoutTemplate>
            <table class="reviewsTable">
                <tr>
                    <th style="width: 200px;">
                        <asp:Label ID="lblName" runat="server" Text='<%$ Resources: Name%>'></asp:Label>
                    </th>
                    <th style="width: 250px;">
                        <asp:Label ID="lblUrlPath" runat="server" Text='<%$ Resources: UrlPath%>'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblSortOrder" runat="server" Text='<%$ Resources: SortOrder%>'></asp:Label>
                    </th>
                    <th style="width: 90px;"></th>
                </tr>
                <tr runat="server" id="itemPlaceHolder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("Name")%>
                </td>
                <td>
                    <%#Eval("UrlPath")%>
                </td>
                <td>
                    <%#Eval("SortOrder")%>
                </td>
                <td>
                    <a href='<%# "javascript:open_window(\"../modules/blog/blogeditCategory.aspx?id=" + Eval("ItemCategoryId") +"\",700,600)"%>'>
                        <asp:Label ID="Label1" runat="server" Text='<%$ Resources: Edit%>'></asp:Label>
                    </a>
                    <asp:LinkButton ID="lbtnDelete" runat="server" Text="<%$ Resources: Delete%>" CssClass="valid-confirm" data-confirm="<%$ Resources: Admin_ConfirmDeleting %>"
                        CommandName="deleteCategory" CommandArgument='<%#Eval("ItemCategoryId") %>' />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
