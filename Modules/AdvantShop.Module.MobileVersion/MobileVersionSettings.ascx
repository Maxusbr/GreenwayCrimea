<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.MobileVersion.MobileVersionSettings" Codebehind="MobileVersionSettings.ascx.cs" %>
<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right; margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Admin_MobileSettings_MainPageProductsCount %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox class="niceTextBox textBoxClass" Style="width:50px;" ID="txtMainPageProductCountMobile" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Admin_MobileSettings_CatalogProductsCount %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox class="niceTextBox textBoxClass" Style="width:50px;" ID="txtCatalogProductCountMobile" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: Admin_MobileSettings_ShowCity %>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox CssClass="checkly-align" ID="chkShowCity" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: Admin_MobileSettings_ShowSlider %>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox CssClass="checkly-align" ID="chkShowSlider" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: Admin_MobileSettings_DisplayHeaderTitle %>"></asp:Localize><br/><br/>
            </td>
            <td>
                <asp:CheckBox CssClass="checkly-align" ID="chkDisplayHeaderTitle" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px; vertical-align:top">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: Admin_MobileSettings_HeaderCustomTitle %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtHeaderCustomTitle" runat="server" TextMode="MultiLine"  class="niceTextBox textBoxClass" Style="width:300px; height: 100px" />
                <br/><br/>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px; vertical-align:top">
                <asp:Localize ID="Localize8" runat="server" Text="Оформление заказа (по-умолчанию мобильная версия)"></asp:Localize>
            </td>
            <td>
                <label><asp:CheckBox runat="server" ID="chkIsFullCheckout" /> Как в полной версии</label>
            </td>
        </tr>


        <tr class="rowsPost">
            <td style="text-align: left; width: 200px; vertical-align:top">
                <asp:Localize ID="Localize2" runat="server" Text="Файл robots.txt"></asp:Localize> <br/><br/>
                <span style="color:gray">Файл будет использоваться только если к сайту привязан поддомен m.</span>
            </td>
            <td>
                <asp:TextBox ID="txtRobots" runat="server" TextMode="MultiLine"  class="niceTextBox textBoxClass" Style="width:500px; height: 500px" />
            </td>
        </tr>
        
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Save %>" />
            </td>
        </tr>
    </table>
</div>