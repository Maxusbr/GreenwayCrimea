<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="Advantshop.Modules.UserControls.Admin_MailChimpModule" Codebehind="MailChimpModule.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: MailChimp_Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: MailChimp_Id %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtMailChimpId" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: MailChimp_FromName %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtFromName" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: MailChimp_FromEmail %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtFromEmail" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Label ID="lblMailChimpRightHead" runat="server" Text="<%$ Resources: MailChimp_MailChimpSubscribers %>" />
            </span>
            <hr style="color: #C2C2C4; height: 1px;" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: MailChimp_ListSubscribers %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:DropDownList ID="ddlMailChimpLists" runat="server" DataValueField="Id"
                                    DataTextField="Name" Width="250px">
                  </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: MailChimp_OderCustomersList %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:DropDownList ID="ddlMailChimpOrderCustomer" runat="server" DataValueField="Id"
                                    DataTextField="Name" Width="250px">
                  </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: MailChimp_Save%>" />
        </td>
    </tr>
</table>