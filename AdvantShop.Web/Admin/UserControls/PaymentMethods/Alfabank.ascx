<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Alfabank.ascx.cs" Inherits="Admin.UserControls.PaymentMethods.AlfabankControl" %>
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
            <asp:Localize runat="server" Text="Логин"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtUserName" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Логин
                    </header>
                    <div class="help-content">
                        Логин магазина, полученный при подключении
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgUserName" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Пароль"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Пароль
                    </header>
                    <div class="help-content">
                        Пароль магазина, полученный при подключении
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>

     <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Логин дочернего мерчанта"></asp:Localize><%--<span class="required">&nbsp;*</span>--%>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantLogin" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Логин дочернего мерчанта
                    </header>
                    <div class="help-content">
                        Чтобы регистрировать заказы от имени дочернего мерчанта, укажите его логин в этом параметре
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgMerchantLogin" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
