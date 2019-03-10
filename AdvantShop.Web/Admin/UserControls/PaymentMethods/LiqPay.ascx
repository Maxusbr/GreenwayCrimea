<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.LiqPayControl" Codebehind="LiqPay.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Номер мерчанта"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantId" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Номер можно посмотреть в разделе Настройки магазина в системе LiqPay" /><asp:Label
                    runat="server" ID="msgMerchantId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="Пароль мерчанта"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantSig" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Пароль можно посмотреть в разделе Настройки магазина в системе LiqPay" /><asp:Label
                    runat="server" ID="msgMerchantSig" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="Валюта магазина"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <%--<asp:TextBox runat="server" ID="txtMerchantISO" Width="250" >--%>
            <asp:DropDownList ID="ddlMerchantISO" runat="server">
                <asp:ListItem Text="Рубли" Value="RUR" />
                <asp:ListItem Text="Гривны" Value="UAH" />
                <asp:ListItem Text="Евро" Value="EUR" />
                <asp:ListItem Text="Доллар" Value="USD" />                
            </asp:DropDownList>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Валюта в которой клиент будет оплачивать заказ" /><asp:Label
                    runat="server" ID="msgtxtMerchantISO" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
