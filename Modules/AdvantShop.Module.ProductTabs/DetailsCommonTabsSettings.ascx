<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.ProductTabs.Admin_DetailsCommonTabsSettings" Codebehind="DetailsCommonTabsSettings.ascx.cs" %>
<style>
    .tabTable { border-collapse: collapse; }

    .tabTable td, .tabTable th {
        border-bottom: 1px solid #000000;
        height: 30px;
        text-align: left;
    }
</style>
<table style="width: 100%;">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: DetailsCommonTabs_Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
</table>
<a href='<%= "javascript:open_window(\"../modules/producttabs/addedittab.aspx\",700,600)" %>'>
    <asp:Label ID="Label4" runat="server" Text='<%$ Resources:DetailsCommonTabs_Create%>'></asp:Label></a>
<asp:ListView ID="lvTabs" runat="server" ItemPlaceholderID="itemPlaceholderID" OnItemCommand="lvTabs_ItemCommand">
    <LayoutTemplate>
        <table class="tabTable">
            <thead>
                <th style="width: 300px;">
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources:DetailsCommonTabs_Title%>'></asp:Label>
                </th>
                <th style="width: 100px;">
                    <asp:Label ID="Label2" runat="server" Text='<%$ Resources:DetailsCommonTabs_Active%>'></asp:Label>
                </th>
                <th style="width: 200px;">
                    <asp:Label ID="Label3" runat="server" Text='<%$ Resources:DetailsCommonTabs_SortOrder%>'></asp:Label>
                </th>
                <th style="width: 150px;">
                </th>
            </thead>
            <tbody>
                <tr runat="server" id="itemPlaceholderID">
                </tr>
            </tbody>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <%#Eval("Title") %>
            </td>
            <td>
                <asp:CheckBox ID="ckbActive" runat="server" Checked='<%#Eval("Active") %>' Enabled="False"/> 
            </td>
            <td>
                <%#Eval("SortOrder") %>
            </td>
            <td>
                <a href='<%# "javascript:open_window(\"../modules/producttabs/addedittab.aspx?tabBodyId=" + Eval("TabBodyId") + "&tabTitleId=" + Eval("TabTitleId") + "\",700,600)" %>'>
                    <asp:Label ID="Label4" runat="server" Text='<%$ Resources:DetailsCommonTabs_Edit%>'></asp:Label></a>
                <asp:LinkButton ID="lnkbDeleteTab" runat="server" Text='<%$ Resources:DetailsCommonTabs_Delete%>' CssClass="valid-confirm"
                                data-confirm="<%$ Resources:DetailsCommonTabs_DeleteConfirmation %>" 
                                CommandName="deleteTab" CommandArgument='<%#Eval("TabBodyId") %>'></asp:LinkButton>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>