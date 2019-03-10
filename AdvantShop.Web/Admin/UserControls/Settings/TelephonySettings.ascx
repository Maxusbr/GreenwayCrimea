<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Settings.TelephonySettings" CodeBehind="TelephonySettings.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Services.IPTelephony" %>
<%@ Import Namespace="AdvantShop.Saas" %>
<%@ Import Namespace="Resources" %>

<table class="info-tb" style="width: 640px;">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory"><%= Resource.Admin_CommonSettings_IpTelephonySettings %>
            </span>
            <br />
            <span class="subTitleNotify"></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
</table>

<% if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTelephony))
   { %>
<table class="info-tb" style="width: 700px;">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= ddlOperatorType.ClientID %>"><%= Resource.Admin_CommonSettings_Telephony_Operator %></label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlOperatorType" DataSourceID="edsOperatorType"
                DataTextField="LocalizedName" DataValueField="Name" data-switchTabs="true" />
            <adv:EnumDataSource ID="edsOperatorType" runat="server"
                EnumTypeName="AdvantShop.Core.Services.IPTelephony.EOperatorType">
            </adv:EnumDataSource>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkPhonerLiteActive.ClientID %>"><%= Resource.Admin_CommonSettings_Telephony_PhonerLite %></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkPhonerLiteActive" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Интеграция с PhonerLite
                    </header>
                    <div class="help-content">
                        Оператор устанавливает программу PhonerLite себе на компьютер. При клике на номер телефона в админ. панели начинается звонок клиенту.
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
<%-- Telphin --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Telphin %>">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= txtTelphinAppKey.ClientID %>"><%= Resource.Admin_CommonSettings_TelphinAppKey %></label>
        </td>
        <td>
            <asp:TextBox ID="txtTelphinAppKey" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtTelphinAppSecret.ClientID %>"><%= Resource.Admin_CommonSettings_TelphinAppSecret %></label>
        </td>
        <td>
            <asp:TextBox ID="txtTelphinAppSecret" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl">Url для оповещений</label>
        </td>
        <td>
            <%= AdvantShop.Configuration.SettingsMain.SiteUrl.TrimEnd('/') + "/telphinold/pushnotification" %>
        </td>
    </tr>
</table>
<%-- Mango --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Mango %>">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= txtMangoApiUrl.ClientID %>"><%= Resource.Admin_CommonSettings_MangoApiUrl %></label>
        </td>
        <td>
            <asp:TextBox ID="txtMangoApiUrl" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= txtMangoApiKey.ClientID %>"><%= Resource.Admin_CommonSettings_MangoApiKey %></label>
        </td>
        <td>
            <asp:TextBox ID="txtMangoApiKey" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMangoSecretKey.ClientID %>"><%= Resource.Admin_CommonSettings_MangoSecretKey %></label>
        </td>
        <td>
            <asp:TextBox ID="txtMangoSecretKey" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_Mango_ServiceUrl %></label>
        </td>
        <td>
            <%= AdvantShop.Configuration.SettingsMain.SiteUrl.TrimEnd('/') + "/mangoadvantshopold" %>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Адрес сервиса
                    </header>
                    <div class="help-content">
                        На данный адрес будут отправляться уведомления о событиях на АТС.<br/> 
                        Необходимо указать его в настройках в личном кабинете Mango Office (Настройки АТС -> Подключение по API -> Данные внешней системы -> поле "Адрес внешней системы")
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
<%-- Sipuni --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Sipuni %>">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= txtSipuniApiKey.ClientID %>"><%=Resource.Admin_CommonSettings_Sipuni_ApiKey %></label>
        </td>
        <td>
            <asp:TextBox ID="txtSipuniApiKey" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Ключ интеграции
                    </header>
                    <div class="help-content">
                        Ключ интеграции можно сгенерировать в личном кабинете Sipuni во вкладке "Интеграция" на странице настроек
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_Sipuni_ServiceUrl %></label>
        </td>
        <td>
            <%= AdvantShop.Configuration.SettingsMain.SiteUrl.TrimEnd('/') + "/sipuniold/pushnotification" %>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Адрес сервиса
                    </header>
                    <div class="help-content">
                        На данный адрес будут отправляться уведомления о событиях на АТС.<br/> 
                        Необходимо указать его в настройках в личном кабинете Sipuni (Настройки -> Интеграция -> События на АТС -> поле "URL принимающего скрипта")
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_Sipuni_ConsiderInnerCalls %></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkSipuniConsiderInnerCalls" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Учитывать внутренние вызовы
                    </header>
                    <div class="help-content">
                        Настройка определяет, отображать или нет уведомления о внутренних вызовах в панели администрирования
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive" style="display: none">
        <td>
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_WebPhone_Active %></label>
        </td>
        <td>
            <span class="checkly-align">
                <asp:CheckBox runat="server" ID="chkSipuniWebPhoneEnabled" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost row-interactive" style="display: none">
        <td>
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_WebPhone_Widget %></label>
        </td>
        <td>
            <span class="checkly-align">
                <asp:TextBox runat="server" ID="txtSipuniWebPhoneWidget" class="niceTextBox textArea7Lines" TextMode="MultiLine" />
            </span>
        </td>
    </tr>
</table>
<%-- Callback head --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Mango %> <%= EOperatorType.Sipuni %> <%= EOperatorType.Telphin %>">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory"><%= Resource.Admin_CommonSettings_CallBack %></span>
            <br />
            <span class="subTitleNotify"></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_CallBack_Active %></label>
        </td>
        <td>
            <span class="checkly-align">
                <asp:CheckBox runat="server" ID="chkEnableCallBack" />
            </span>
        </td>
    </tr>
</table>
<%-- Sipuni callback settings --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Sipuni %>">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= txtCallBackSipuniAccount.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_SipuniAccount %></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackSipuniAccount" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Номер аккаунта
                    </header>
                    <div class="help-content">
                        Номер в системе Sipuni
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtCallBackSipuniShortNumber.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_ShortNumber %></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackSipuniShortNumber" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Внутренний номер
                    </header>
                    <div class="help-content">
                        Короткий номер (от 100 до 999), на который будет осуществлен вызов
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_CallBack_Type %></label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCallbackSipuniType" Width="380px">
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_CallBack_CallNumber %>" Value="0" />
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_CallBack_CallTree %>" Value="1" />
            </asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Тип звонка
                    </header>
                    <div class="help-content">
                        
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtCallBackSipuniTree.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_Tree %></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackSipuniTree" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Схема
                    </header>
                    <div class="help-content">
                        Номер схемы, используемой для соединения
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
<%-- Mango callback settings --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Mango %>">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= txtCallBackMangoExtension.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_Mango_Extension %></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackMangoExtension" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Внутренний номер
                    </header>
                    <div class="help-content">
                        Идентификатор сотрудника ВАТС. Обязательное поле. Если у сотрудника ВАТС нет идентификатора (внутреннего номера), он не сможет выполнять команду инициирования вызова.
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
<%-- Telphin callback settings --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Telphin %>">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= txtCallBackTelphinExtension.ClientID %>"><%= Resource.Admin_CommonSettings_CallBackTelphinExtension %></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackTelphinExtension" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtCallBackTelphinLocalNumber.ClientID %>"><%= Resource.Admin_CommonSettings_CallBackTelphinLocalNumber %></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackTelphinLocalNumber" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
</table>
<%-- Callback settings --%>
<table class="info-tb" style="width: 700px;" data-switchTabs-tabId="<%= EOperatorType.Mango %> <%= EOperatorType.Sipuni %> <%= EOperatorType.Telphin %>">
    <tr class="rowsPost row-interactive">
        <td style="width: 250px;">
            <label class="form-lbl" for="<%= ddlOperatorType.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_ShowMode %></label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCallBackShowMode" DataSourceID="edsCallBackShowMode"
                DataTextField="LocalizedName" DataValueField="Name" />
            <adv:EnumDataSource ID="edsCallBackShowMode" runat="server"
                EnumTypeName="AdvantShop.Core.Services.IPTelephony.CallBack.ECallBackShowMode">
            </adv:EnumDataSource>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtCallBackTimeInterval.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_TimeInterval %></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackTimeInterval" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Количество секунд
                    </header>
                    <div class="help-content">
                        Только для отображения пользователю.<br/>На скорость работы сервиса не влияет.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtCallBackWorkTimeText.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_WorkTimeText%></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackWorkTimeText" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Текст успешной заявки в рабочее время
                    </header>
                    <div class="help-content">
                        Текст, отображаемый после нажатия кнопки "Заказать звонок" в рабочее время.
                    </div>
                </article>
            </div>
            <div class="subSaveNotify"><%= Resource.Admin_CommonSettings_CallBack_WorkTimeText_Message %></div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtCallBackNotWorkTimeText.ClientID %>"><%= Resource.Admin_CommonSettings_CallBack_NotWorkTimeText%></label>
        </td>
        <td>
            <asp:TextBox ID="txtCallBackNotWorkTimeText" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Текст успешной заявки в нерабочее время
                    </header>
                    <div class="help-content">
                        Текст, отображаемый после нажатия кнопки "Заказать звонок" в нерабочее время.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl"><%= Resource.Admin_CommonSettings_CallBack_WorkSchedule %></label>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Время приема звонков
                    </header>
                    <div class="help-content">
                        Расписание приема звонков.<br/>
                        В рабочее время звонок будет направлен на внутренний номер, в нерабочее будет создан лид.
                    </div>
                </article>
            </div>
        </td>
        <td id="tdWorkSchedule" runat="server">
        </td>
    </tr>
</table>
<br />
<span class="subTitleNotify">Чтобы сохранить изменения, нажмите оранжевую кнопку "Сохранить" вверху.
</span>
<% }
   else
   { %>
<div class="AdminSaasNotify">
    <h2>
        <%=  Resource.Admin_DemoMode_NotAvailableFeature%>
    </h2>
</div>
<% } %>