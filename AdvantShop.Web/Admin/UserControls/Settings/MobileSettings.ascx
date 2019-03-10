<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Settings.MobileSettings" CodeBehind="MobileSettings.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Saas" %>
<%@ Import Namespace="Resources" %>

<% if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.MobileVersion))
    { %>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_MobileSettings_Header%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost row-interactive">
        <td style="width: 250px;">Активность
        </td>
        <td>
            <asp:CheckBox CssClass="checkly-align" ID="cbEnabled" runat="server" />
        </td>
    </tr>
    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <%= Resources.Resource.Admin_MobileSettings_MainPageProductsCount %>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" Style="width: 50px;" ID="txtMainPageProductCountMobile" runat="server" />
        </td>
    </tr>
    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <%= Resources.Resource.Admin_MobileSettings_ShowCity %>
        </td>
        <td>
            <asp:CheckBox CssClass="checkly-align" ID="chkShowCity" runat="server" />
        </td>
    </tr>
    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <%= Resources.Resource. Admin_MobileSettings_ShowSlider %>
        </td>
        <td>
            <asp:CheckBox CssClass="checkly-align" ID="chkShowSlider" runat="server" />
        </td>
    </tr>
    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <%= Resources.Resource.Admin_MobileSettings_DisplayHeaderTitle %><br />
        </td>
        <td>
            <asp:CheckBox CssClass="checkly-align" ID="chkDisplayHeaderTitle" runat="server" />
        </td>
    </tr>
    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <%= Resources.Resource.Admin_MobileSettings_HeaderCustomTitle %>
        </td>
        <td>
            <asp:TextBox ID="txtHeaderCustomTitle" runat="server" TextMode="MultiLine" class="niceTextBox textBoxClass" Style="width: 300px; height: 100px" />
            <br />
            <br />
        </td>
    </tr>
    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <asp:Localize ID="Localize8" runat="server" Text="Оформление заказа (по-умолчанию мобильная версия)"></asp:Localize>
        </td>
        <td>
            <label>
                <asp:CheckBox runat="server" ID="chkIsFullCheckout" />
                Как в полной версии</label>
        </td>
    </tr>

    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <asp:Localize ID="Localize1" runat="server" Text="Перенаправлять на поддомен m. (поддомен должен быть привязан к сайту)"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkRedirectToSubdomain" />
        </td>
    </tr>

    <tr class="rowPost row-interactive">
        <td style="width: 250px;">
            <asp:Localize ID="Localize2" runat="server" Text="Файл robots.txt"></asp:Localize>
            <br />
            <br />
            <span style="color: gray">Файл будет использоваться только если к сайту привязан поддомен m.</span>
        </td>
        <td>
            <asp:TextBox ID="txtRobots" runat="server" TextMode="MultiLine" class="niceTextBox textBoxClass" Style="width: 500px; height: 500px" />
        </td>
    </tr>
</table>


<%}
    else
    { %>
<div class="AdminSaasNotify">
    <h2>
        <%=  Resource.Admin_DemoMode_NotAvailableFeature%>
    </h2>
</div>
<% } %>