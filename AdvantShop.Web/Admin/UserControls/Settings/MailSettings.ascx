<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Settings.MailSettings" Codebehind="MailSettings.ascx.cs" %>

<table class="info-tb" style="width: 555px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadMailServer%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvWarningNotify">
                <%= Resources.Resource.Admin_CommonSettings_MailNotify%>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_TransportLevel%>
            </span>
            <br />
            <span class="subTitleNotify">
                <%= Resources.Resource.Admin_CommonSettings_MailSettingsSubTitle%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 155px;">
            <label class="form-lbl" for="<%= txtEmailSMTP.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_SmtpServer%></label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailSMTP" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        SMTP сервер почты
                    </header>
                    <div class="help-content">
                        SMTP сервер вашего почтового провайдера.<br />
                        <br />
                        Например, для yandex он будет: <b>smtp.yandex.ru</b><br />
                        У каждого почтового провайдера свой SMTP.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtEmailLogin.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_SupportLogin%></label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailLogin" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Логин от почты
                    </header>
                    <div class="help-content">
                        Логин, который вы используете для входа в почтовый ящик. Его нужно указать здесь.<br />
                        <br />
                        Например: <b>myemail@yandex.ru</b>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtEmailPassword.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_SupportPassword%></label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailPassword" runat="server" class="niceTextBox shortTextBoxClass" TextMode="Password"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Пароль от почты
                    </header>
                    <div class="help-content">
                        Пароль, который вы используете для входа в почтовый ящик. Его нужно указать здесь.<br />
                        <br />
                        Например: <b>MySuperPass123</b>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtEmailPort.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Port%></label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailPort" runat="server" class="niceTextBox shortTextBoxClass2"></asp:TextBox>&nbsp;&nbsp;<span><%= Resources.Resource.Admin_CommonSettings_MailNotify_defPort%></span>
            <div data-plugin="help" class="help-block" style="margin-left: 21px;">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Порт для соединения
                    </header>
                    <div class="help-content">
                        Порт, через который происходит соединение с почтовым сервером.<br />
                        Например: <b>25</b>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtEmail.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EmailDistrib%></label>
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Email адрес почты отправления
                    </header>
                    <div class="help-content">
                        Этот email будет использован в качестве адреса для отправки сообщений от магазина.<br />
                        Обратите внимание, что этот параметр не живет отдельно от других параметров почты.<br />
                        <br />
                        Укажите полное название ящика.<br /> 
                        <br />
                        Например: <b>myemail@yandex.ru</b>
                    </div>
                </article>
            </div>
        </td>
    </tr>

        <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtSenderName.ClientID %>">Имя отправителя</label>
        </td>
        <td>
            <asp:TextBox ID="txtSenderName" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Имя отправления
                    </header>
                    <div class="help-content">
                        Это имя будет использовано в качестве подписи.<br />
                        Не допускается использование html-тегов.<br />
                        Например: <b>Мой интрнет-магазин на платформе AdvantShop</b>
                    </div>
                </article>
            </div>
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkEnableSSL.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableSSL%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkEnableSSL" CssClass="checkly-align"/>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Использовать SSL для почты
                    </header>
                    <div class="help-content">
                        Определяет использовать ли SSL соединения для подключения к почтовому серверу.<br />
                        В большинстве случаев эта галочка необходима.
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
<div class="dvSubHelp">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/email-google-yandex" target="_blank">Инструкция. Настройка email почты магазина</a>
</div>
<br />
<table class="info-tb" style="width: 555px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_SendTestMessage%>
            </span>
            <br />
            <span class="subTitleNotify">
                <%= Resources.Resource.Admin_CommonSettings_MailTestSubTitle%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="info-tb" style="width: 555px;">
            <tbody>
                <tr class="rowsPost row-interactive">
                    <td style="width: 155px;">
                        <label class="form-lbl" for="<%= txtTo.ClientID %>" class="Label">
                            <%= Resources.Resource.Admin_CommonSettings_TestEmail_SendTo%>
                        </label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server" CssClass="niceTextBox shortTextBoxClass"></asp:TextBox>
                    </td>
                </tr>
                <tr class="rowsPost row-interactive">
                    <td>
                        <label class="form-lbl" for="<%= txtSubject.ClientID%>" class="Label">
                            <%= Resources.Resource.Admin_CommonSettings_TestEmail_Subject%>
                        </label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="niceTextBox textBoxClass"></asp:TextBox>
                    </td>
                </tr>
                <tr class="rowsPost rowsPostTop rowsPostBig row-interactive">
                    <td>
                        <label class="form-lbl" for="<%= txtMessage.ClientID %>" class="Label">
                            <%= Resources.Resource.Admin_CommonSettings_TestEmail_Message%>
                        </label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="niceTextArea textArea7Lines" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--<asp:Label ID="lblDegub" runat="server" Text="[]"></asp:Label>--%>
        <br />
        <table>
            <tr>
                <td style="width: 230px; vertical-align: middle;">
                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnSendMail" runat="server"
                        Text="<%$ Resources:Resource, Admin_CommonSettings_TestEmail_SendButtom %>" OnClick="btnSendMail_Click" />
                </td>
                <td style="vertical-align: middle;">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <table>
                                <tr>
                                    <td style="vertical-align: middle; width: 40px;">
                                        <img src="images/ajax-loader.gif" alt="" />
                                    </td>
                                    <td style="vertical-align: middle;">
                                        <%= Resources.Resource.Admin_CommonSettings_TestEmail_Process%>
                                    </td>
                                </tr>
                            </table>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
        <br />
        <asp:Literal ID="Message" runat="server"></asp:Literal>
    </ContentTemplate>
</asp:UpdatePanel>