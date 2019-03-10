<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.YandexKassaControl" Codebehind="YandexKassa.ascx.cs" %>
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
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Yandex_ShopID %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtShopID" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgShopID" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Yandex_ScID %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtScID" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgScID" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="Способ оплаты"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlPaymentType">
                <asp:ListItem Text="Умный платеж (все доступные методы)" Value="" />
                <asp:ListItem Text="Со счета в Яндекс.Деньгах" Value="PC" />
                <asp:ListItem Text="С банковской карты" Value="AC" />
                <asp:ListItem Text="Со счета мобильного телефона" Value="MC" />
                <asp:ListItem Text="По коду через терминал" Value="GP" />
                <asp:ListItem Text="Оплата через Сбербанк: оплата по SMS или Сбербанк Онлайн" Value="SB" />
				<asp:ListItem Text="Оплата через мобильный терминал (mPOS)" Value="WM" />
				<asp:ListItem Text="Оплата через Альфа-Клик" Value="AB" />
				<asp:ListItem Text="Оплата через MasterPass" Value="МА" />
				<asp:ListItem Text="Оплата через Промсвязьбанк" Value="PB" />
                <asp:ListItem Text="Оплата через QIWI Wallet" Value="QW" />
				<asp:ListItem Text="Оплата через КупиВкредит (Тинькофф Банк)" Value="KV" />
				<asp:ListItem Text="Оплата через Доверительный платеж на Куппи.ру" Value="QP" />
			</asp:DropDownList>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_YandexKassa_Password %>"></asp:Localize><%--<span
                class="required">&nbsp;*</span>--%>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Yandex_DemoMode %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="cbDemoMode"/>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgCertificate" Visible="false" ForeColor="Red"></asp:Label>
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

     <%--<tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="Ставка НДС"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlVatType">
                <asp:ListItem Text="Не выбрано" Value="0" />
                <asp:ListItem Text="Без НДС" Value="1" />
                <asp:ListItem Text="НДС по ставке 0%" Value="2" />
                <asp:ListItem Text="НДС чека по ставке 10%" Value="3" />
                <asp:ListItem Text="НДС чека по ставке 18%" Value="4" />
                <asp:ListItem Text="НДС чека по расчетной ставке 10/110" Value="5" />
				<asp:ListItem Text="НДС чека по расчетной ставке 18/118" Value="6" />
			</asp:DropDownList>
        </td>
        <td class="columnDescr">
        </td>
    </tr>--%>

</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/connect-yandex-kassa" target="_blank">Инструкция. Подключение платежного модуля "Касса от Яндекс.Денег"</a>
</div>