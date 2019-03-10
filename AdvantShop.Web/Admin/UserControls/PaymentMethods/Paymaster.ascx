<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.PaymentMethods.PaymasterControl" CodeBehind="Paymaster.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text='Идентификатор сайта'></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgMerchantId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>

    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text='Секретное слово'></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretWord" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgSecretWord" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/connect-paymaster" target="_blank">Инструкция по подключению к сервису Paymaster</a>
</div>
