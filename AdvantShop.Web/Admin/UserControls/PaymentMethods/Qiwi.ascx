<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.QiwiControl" Codebehind="Qiwi.ascx.cs" %>
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
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_QIWI_ProviderID %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtQiwiId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Числовой идентификатор провайдера 
                    </header>
                    <div class="help-content">
                        Это ваш логин в личный кабинет.<br />
                        Обычно 6ти значное число.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgQiwiId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
     <tr>
        <td class="columnName">
            Rest ID
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtRestID" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Rest ID
                    </header>
                    <div class="help-content">
                        Идентификатор для протокола.<br />
                        <br />
                        Получить можно из личного кабинета в системе оплаты.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_QIWI_Password %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Rest ID пароль
                    </header>
                    <div class="help-content">
                        Обратите вримание, что это не пароль от личного кабинета, это отдельный, специальный пароль.<br />
                        <br />
                        Получить пароль можно в личном кабинете системы оплаты, при получении параметра Rest ID.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Пароль оповещения"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPasswordNotify" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Пароль оповещения
                    </header>
                    <div class="help-content">
                        Используется интерфейсом оповещения о платеже.<br />
                        <br />
                        Получить пароль можно в личном кабинете системы оплаты в настройках Pull (REST) протокола.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgPasswordNotify" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_QIWI_ProviderName %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtProviderName" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Название магазина
                    </header>
                    <div class="help-content">
                        Название магазина - Это название магазина, тут всё просто.<br />
                        <br />
                        Название должно быть кратким, оно будет отображаться на странице оплаты когда пользователь уже перейдет на сайт qiwi для оплаты. 
                        <br />
                        <br />
                        Например: Бижутерия "Кики".
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgProviderName" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_QIWI_CurrencyCode %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyCode" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Валюта платежа
                    </header>
                    <div class="help-content">
                        Выберите валюту платежа, в которой будет происходить оплата
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgCurrencyCode" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_QIWI_CurrencyValue %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyValue" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_PaymentMethod_CurrencyValue%>
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_PaymentMethod_CurrencyValue_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgCurrencyValue" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/connect-qiwi" target="_blank"><%= Resources.Resource.Admin_PaymentMethod_QIWI_Instruction%></a>
</div>