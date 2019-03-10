<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.ProductTabs.Admin_DetailsProductTabsSettings" Codebehind="DetailsProductTabsSettings.ascx.cs" %>
<style>
    .tabTable { border-collapse: collapse; }

    .tabTable td, .tabTable th {
        border-bottom: 1px solid #000000;
        height: 30px;
        text-align: left;
        padding: 5px;
    }

    .newTab td {
        padding: 5px 0;
    }

</style>
<script type="text/javascript">
    function showHideAddTabTitlePanel() {
        if ($("#divAddTabTitlePanel").length) {
            if ($("#divAddTabTitlePanel").css("display", "none")) {
                $("#divAddTabTitlePanel").show();
            } else {
                $("#divAddTabTitlePanel").hide();
            }

            var iframe =  $('#moduleIFrame', window.parent.document);
            if (iframe.length) {
                iframe.height($("body").height());
            }
        }
    }
</script>
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
<a href='javascript:void(0)' onclick=" showHideAddTabTitlePanel() ">
    <asp:Label ID="Label4" runat="server" Text='<%$ Resources:DetailsCommonTabs_Create%>'></asp:Label></a>
<div id="divAddTabTitlePanel" style="display: none; margin: 20px 0px 20px 0px;">
    <table class="newTab">
        <tr>
            <td style="width: 150px;">
                <asp:Label ID="Label2" runat="server" Text='<%$ Resources:DetailsCommonTabs_Title%>'></asp:Label>
            </td>
            <td style="width: 300px;">
                <asp:TextBox ID="txtTabTitle" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text='<%$ Resources:DetailsCommonTabs_SortOrder%>'></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSortOrder" runat="server" Width="200px" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text='<%$ Resources:DetailsCommonTabs_Active%>'></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="ckbActive" runat="server" Checked="True"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnAddTabTitle" runat="server" Text='<%$ Resources:DetailsCommonTabs_Add%>'
                            OnClick="btnAddTabTitle_OnClick" />
            </td>
        </tr>
    </table>
</div>
<asp:ListView ID="lvTabs" runat="server" ItemPlaceholderID="itemPlaceholderID" OnItemCommand="lvTabs_ItemCommand"
              OnItemEditing="lvTabs_ItemEditing" OnItemCanceling="lvTabs_ItemCanceling">
    <LayoutTemplate>
        <table class="tabTable">
            <thead>
                <th style="width: 300px;">
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources:DetailsCommonTabs_Title%>'></asp:Label>
                </th>
                <th style="width: 100px;">
                    <asp:Label runat="server" Text='<%$ Resources:DetailsCommonTabs_Active%>'></asp:Label>
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
                <asp:CheckBox ID="ckbTabActive" runat="server" Checked='<%#Eval("Active") %>' Enabled="False" />
            </td>
            <td>
                <%#Eval("SortOrder") %>
            </td>
            <td>
                <asp:LinkButton ID="lnkbEditTab" runat="server" Text='<%$ Resources:DetailsCommonTabs_Edit%>'
                                CommandName="Edit"></asp:LinkButton>
                <asp:LinkButton ID="lnkbDeleteTab" runat="server" Text='<%$ Resources:DetailsCommonTabs_Delete%>' CssClass="valid-confirm"
                                data-confirm="<%$ Resources: DetailsCommonTabs_DeleteConfirmation %>" 
                                CommandName="deleteTab" CommandArgument='<%#Eval("TabTitleId") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <EditItemTemplate>
        <tr>
            <td>
                <asp:TextBox ID="txtEditTitle" runat="server" Text='<%#Bind("Title") %>' Width="280px"></asp:TextBox>
            </td>
            <td>
                <asp:CheckBox ID="ckbEditActive" runat="server" Checked='<%#Bind("Active") %>'></asp:CheckBox>
            </td>
            <td>
                <asp:TextBox ID="txtEditSortOrder" runat="server" Text='<%#Bind("SortOrder") %>'
                             Width="180px"></asp:TextBox>
            </td>
            <td>
                <asp:LinkButton ID="UpdateButton" runat="server" CommandName="updateTab" CommandArgument='<%#Eval("TabTitleId") %>'
                                Text="<%$ Resources:DetailsCommonTabs_Save %>"></asp:LinkButton>
                <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="<%$ Resources:DetailsCommonTabs_Cancel %>"></asp:LinkButton>
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>