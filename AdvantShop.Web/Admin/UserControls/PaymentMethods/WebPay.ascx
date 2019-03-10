<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.WebPayControl" Codebehind="WebPay.ascx.cs" %>
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
            Идентификатор магазина в системе WebPay<span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtStoreId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Идентификатор магазина в системе WebPay. Данный идентификатор создается при регистрации в системе WebPay™ и высылается в письме" />
            <asp:Label runat="server" ID="msgShopID" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            Секретный ключ<span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Секретный ключ в системе WebPay" />
            
            <asp:Label runat="server" ID="msgScID" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            Тестовый режим
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="cbTestMode"/>
        </td>
        <td class="columnDescr">
            
        </td>
    </tr>
    <tr>
        <td class="columnName">
            Валюта
        </td>
        <td class="" colspan="2">
            Оплата доступна только в следующих валютах: BYN, USD, EUR, RUB. <br>
            Если заказ оформлен в другой валюте, оплата не пройдет.<br>
            В тестовой среде допускается совершение платежей только в белорусских рублях на сумму от 0.1 до 10 000.
        </td>
    </tr>
</table>