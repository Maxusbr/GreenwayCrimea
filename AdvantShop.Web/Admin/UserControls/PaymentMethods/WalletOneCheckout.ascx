<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.PaymentMethods.WalletOneCheckoutControl" CodeBehind="WalletOneCheckout.ascx.cs" %>
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
            <asp:Localize runat="server" Text='<%$ Resources:Resource, Admin_PaymentMethod_WalletOneCheckout_StoreId %>'></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_PaymentMethod_WalletOneCheckout_StoreId%>
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_PaymentMethod_WalletOneCheckout_StoreId_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgMerchantId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text='<%$ Resources:Resource, Admin_PaymentMethod_WalletOneCheckout_SecretKey%>'></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_PaymentMethod_WalletOneCheckout_SecretKey%>
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_PaymentMethod_WalletOneCheckout_SecretKey_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgSecretKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text='Используемые платежные системы'></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPayWaysEnabled" Height="50" Width="250" TextMode="MultiLine"></asp:TextBox>
            <br/>
            <a href="http://www.walletone.com/ru/merchant/documentation/#step4 ">Список доступных спобосов</a>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Используемые платежные системы
                    </header>
                    <div class="help-content">
                        Перечисление используемых способ оплаты, значение строго определены в системы  Wallet One(через пробел)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text='Запрещенные платежные системы'></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPayWaysDisabled" Height="50" Width="250" TextMode="MultiLine"></asp:TextBox>
            <br/>
            <a href="http://www.walletone.com/ru/merchant/documentation/#step4 ">Список спобосов</a>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Запрещенные платежные системы
                    </header>
                    <div class="help-content">
                        Перечисление запрещенных способ оплаты, значение строго определены в системы  Wallet One(через пробел)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label2" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/connect-wallet-one" target="_blank"><%= Resources.Resource.Admin_PaymentMethod_WalletOneCheckout_Instruction%></a>
</div>
