<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Callback.Admin_ViewCallbacks" CodeBehind="ViewCallbacks.ascx.cs" %>
<style>
    .reviewsTable {
        border-collapse: collapse;
        width: 100%;
    }

        .reviewsTable td, .reviewsTable th {
            border-bottom: 1px solid #000000;
            height: 30px;
            text-align: left;
        }
</style>
<script type="text/javascript">
    function confirmDelete() {
        return confirm("Вы уверены, что хотите удалить запрос?");
    }
</script>
<div>
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Callback_ManagerHeader%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
    </table>
    <asp:ListView ID="lvCallbacks" runat="server" ItemPlaceholderID="itemPlaceHolder" OnItemCommand="lvCallbacksItemCommand">
        <layouttemplate>
            <table class="reviewsTable">
                <tr>
                    <th style="width: 100px;">
                        <asp:Label ID="lblDate" runat="server" Text='<%$ Resources: Callback_DateAdded%>'></asp:Label>
                    </th>
                    <th style="width: 250px;">
                        <asp:Label ID="lblName" runat="server" Text='<%$ Resources: Callback_Name%>'></asp:Label>
                    </th>
                    <th style="width: 200px;">
                        <asp:Label ID="lblPhone" runat="server" Text='<%$ Resources: Callback_Phone%>'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblComment" runat="server" Text='<%$ Resources: Callback_Comment%>'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblAdminComment" runat="server" Text='<%$ Resources: Callback_AdminComment%>'></asp:Label>
                    </th>
                    <th style="width: 80px;">
                        <asp:Label ID="lblProcessed" runat="server" Text='<%$ Resources: Callback_Processed%>'></asp:Label>
                    </th>
                    <th style="width: 90px;">
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceHolder">
                </tr>
            </table>
        </layouttemplate>
        <itemtemplate>
            <tr>
                <td>
                    <%# ((DateTime) Eval("DateAdded")).ToString("yyyy.MM.dd HH:mm") %>
                </td>
                <td>
                    <%#Eval("Name") %>
                </td>
                <td>
                    <%#Eval("Phone") %>
                </td>
                <td>
                    <%#Eval("Comment") %>
                </td>
                <td>
                    <%#Eval("AdminComment") %>
                </td>
                <td>
                    <asp:CheckBox ID="ckbModerated" runat="server" Checked='<%# Eval("Processed") %>' Enabled="False" />
                </td>
                <td>
                    <a href='<%# "javascript:open_window(\"../modules/callback/editcallback.aspx?id=" + Eval("Id") + "\",700,600)" %>'>
                        <asp:Label ID="Label1" runat="server" Text='<%$ Resources: Callback_Edit%>'></asp:Label></a>
                    <asp:LinkButton ID="btnDelete" runat="server" Text="<%$ Resources: Callback_Delete%>" CommandName="deleteCallBack"
                                    CommandArgument='<%#Eval("Id") %>'  OnClientClick="return confirmDelete();"/>
                </td>
            </tr>
        </itemtemplate>
        <emptydatatemplate>
            <div style="text-align: center;font-size: 16px;">
             <asp:Label ID="Label2" runat="server" Text='<%$ Resources: Callback_NotData%>'></asp:Label>
                </div>
        </emptydatatemplate>

    </asp:ListView>
</div>
