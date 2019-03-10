<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Settings.ApiSettings" CodeBehind="ApiSettings.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Saas" %>
<%@ Import Namespace="Resources" %>

<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">API</span>
            <br />
            <span class="subTitleNotify">Настройка интеграции с дополнительными сервисами.
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= txtApiKey.ClientID %>"><%= Resource.Admin_UserControl_ApiSettings_ApiKey%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" Style="width: 500px;" ID="txtApiKey" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        API ключ
                    </header>
                    <div class="help-content">
                        API ключ - Это параметр необходимый для обеспечения возможности подключения сторонних сервисов к магазину.<br />
                        <br />
                        Обратите внимание, если вы повторно сгенерируете ключ, все ссылки в которых он используется, так же необходимо обновить, включая те, что были указанны ранее в сторонних сервисах.
                    </div>
                </article>
            </div>
            <br />
            <asp:LinkButton runat="server" ID="lbGenerateApiKey" Text="<%$ Resources: Resource, Admin_UserControl_ApiSettings_GenerateApiKey%>"
                OnClick="lbGenerateApiKey_Click" />
        </td>
    </tr>
</table>

<% if (IsRu)
   { %>
<table border="0" cellpadding="2" cellspacing="2">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">1C</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <% if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.Have1C))
           { %>
    <tr class="rowsPost row-interactive">
        <td style="width: 260px;">
            <%= Resource.Admin_UserControl_ApiSettings_1C_Enabled %>
        </td>
        <td>
            <asp:CheckBox ID="chk1CEnabled" runat="server" />
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>
            Отключить списание товаров в магазине.<br/>
            (весь учет наличия ведется в 1с)
        </td>
        <td>
            <asp:CheckBox ID="chk1CDisableProductsDecremention" runat="server" />
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>
            <%= Resource.Admin_UserControl_ApiSettings_1C_ExportOrdersType %>
        </td>
        <td>
            <asp:DropDownList ID="ddlExportOrdersType" runat="server">
                <asp:ListItem Text="<%$ Resources:Resource, Admin_UserControl_ApiSettings_1C_UseIn1CType %>" Value="0" />
                <asp:ListItem Text="<%$ Resources:Resource, Admin_UserControl_ApiSettings_1C_AllType %>" Value="1" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>Обновлять статусы заказов из 1C
        </td>
        <td>
            <asp:CheckBox ID="chk1CUpdateStatuses" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>Обновление товаров из 1С
        </td>
        <td>
            <asp:DropDownList ID="ddl1CUpdateProducts" runat="server">
                <asp:ListItem Text="Добавлять и обновлять товары из 1C" Value="0" />
                <asp:ListItem Text="Только добавлять новые товары" Value="1" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>Отправка номенклатуры в 1C
        </td>
        <td>
            <asp:DropDownList ID="ddl1CSendProducts" runat="server">
                <asp:ListItem Text="Отправлять весь каталог" Value="0" />
                <asp:ListItem Text="Отправлять только товары участвующие в заказах" Value="1" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resource.Admin_UserControl_ApiSettings_1C_ImportPhotos %>
        </td>
        <td>
            <asp:Label runat="server" ID="lblImportPhotosUrl" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <%= Resource.Admin_UserControl_ApiSettings_1C_ImportProducts %>
        </td>
        <td>
            <asp:Label runat="server" ID="lblImportProductsUrl" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resource.Admin_UserControl_ApiSettings_1C_ExportProducts %>
        </td>
        <td>
            <asp:Label runat="server" ID="lblExportProductsUrl" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <%= Resource.Admin_UserControl_ApiSettings_1C_DeletedProducts %>
        </td>
        <td>
            <asp:Label runat="server" ID="lblDeletedProducts" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resource.Admin_UserControl_ApiSettings_1C_ExportOrders %>
        </td>
        <td>
            <asp:Label runat="server" ID="lblExportOrdersUrl" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resource.Admin_UserControl_ApiSettings_1C_DeletedOrders %>
        </td>
        <td>
            <asp:Label runat="server" ID="lblDeletedOrdersUrl" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resource.Admin_UserControl_ApiSettings_1C_ChangeOrderStatusUrl %>
        </td>
        <td>
            <asp:Label runat="server" ID="lblChangeOrderStatusUrl" />
        </td>
    </tr>
    <% }
           else
           { %>
    <tr>
        <td colspan="2">
            <div class="AdminSaasNotify">
                <h2>
                    <%= Resource.Admin_DemoMode_NotAvailableFeature %>
                </h2>
            </div>
        </td>
    </tr>
    <% } %>
</table>
<% } %>
