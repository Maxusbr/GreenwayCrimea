<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Blog.BlogListItems" Codebehind="BlogListItems.ascx.cs" %>

<style>
    .reviewsTable{width: 100%;border-collapse: collapse;}
    .reviewsTable td, .reviewsTable th{text-align: left;border-bottom: 1px solid #000000;height: 30px;padding: 3px;}
</style>
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
    <a class="btn btn-middle btn-add" href="javascript:open_window('../modules/blog/blogedititem.aspx?id=0',800,700)">
        <asp:Literal runat="server" Text="<%$Resources: AddNewItem %>"></asp:Literal>
    </a>
</div>
<asp:ListView ID="lvBlogItems" runat="server" ItemPlaceholderID="itemPlaceHolder" OnItemCommand="lvBlogItemsItemCommand">
    <LayoutTemplate>
        <table class="reviewsTable">
            <tr>
                <th style="width: 200px;">
                    <asp:Label ID="lblName" runat="server" Text='<%$ Resources: Name%>'></asp:Label>
                </th>
                <th>
                    <asp:Label ID="Label2" runat="server" Text='<%$ Resources: CategoryName%>'></asp:Label>
                </th>
                <th>
                    <asp:Label ID="lblUrlPath" runat="server" Text='<%$ Resources: UrlPath%>'></asp:Label>
                </th>
                <th style="width: 90px;"></th>
                <th>
                    <asp:Label ID="lblEnabled" runat="server" Text='<%$ Resources: Enabled%>'></asp:Label>
                </th>
                <th style="width: 120px">
                    <asp:Label ID="lblSortOrder" runat="server" Text='<%$ Resources: AddingDate%>'></asp:Label>
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
                <%#Eval("Title")%>
            </td>
            <td>
                <%# RenderCategoryName(Convert.ToInt32(Eval("ItemCategoryID")))%>
            </td>
            <td>
                <%#Eval("UrlPath")%>
            </td>
            <td>
                <a href='<%#RenderItemLink(Convert.ToString(Eval("UrlPath")), Convert.ToInt32(Eval("ItemCategoryID")))  %>' target="_blank">
                    <asp:Label ID="Label3" runat="server" Text='<%$ Resources: Goto%>'></asp:Label>
                </a>
            </td>
            <td style="text-align: center">
                <input type="checkbox" <%#Convert.ToBoolean( Eval("Enabled")) ? "checked='checked'" : "" %> disabled="disabled" />
            </td>
            <td>
                <%#Eval("AddingDate")%>
            </td>

            <td>
                <a href='<%# "javascript:open_window(\"../modules/blog/blogedititem.aspx?id=" + Eval("ItemId") +"\",800,700)"%>'>
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources: Edit%>'></asp:Label></a>
                <asp:LinkButton ID="btnDelete" runat="server" Text="<%$ Resources: Delete%>" CssClass="valid-confirm" data-confirm="<%$ Resources: Admin_ConfirmDeleting %>"
                    CommandName="deleteItem" CommandArgument='<%#Eval("ItemId") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <EditItemTemplate>
    </EditItemTemplate>
</asp:ListView>
