<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.TinkoffControl" Codebehind="Tinkoff.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="margin-left: 5px; margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="Идентификатор терминала"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtTerminalKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgTerminalKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Секретный ключ"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgSecretKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>

     <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Передавать данные для чека"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="cbSendReceiptData"/>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="Система налогообложения"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlTaxation">
                <asp:ListItem Text="Общая" Value="osn" />
                <asp:ListItem Text="Упрощенная (доходы)" Value="usn_income" />
                <asp:ListItem Text="Упрощенная (доходы минус расходы)" Value="usn_income_outcome" />
                <asp:ListItem Text="Единый налог на вмененный доход" Value="envd" />
                <asp:ListItem Text="Единый сельскохозяйственный налог" Value="esn" />
                <asp:ListItem Text="Патентная" Value="patent" />
			</asp:DropDownList>
        </td>
        <td class="columnDescr">
        </td>
    </tr>

</table>
<%--<div class="dvSubHelp2">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/connect-yandex-kassa" target="_blank">Инструкция. Подключение платежного модуля ""</a>
</div>--%>