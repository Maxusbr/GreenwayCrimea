<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.OAuthSettings" CodeBehind="OAuthSettings.ascx.cs" %>
<table runat="server" id="table1" style="width: 600px; margin-bottom: 10px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Авторизация по OpenID</span>
            <br />
            <span class="subTitleNotify">Вы можете настроить авторизацию на сайте через популярные соц. сети или сервисы.<br />
                Для этого выберите нужный сервис и произведите настройку.
            </span>
        </td>
    </tr>
</table>
<table runat="server" id="tableGoogle" class="info-tb" style="width: 600px; margin-bottom: 10px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Google</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= ckbGoogleActive.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthActive%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbGoogleActive" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtGoogleClientId.ClientID %>">Client ID</label>
        </td>
        <td>
            <asp:TextBox CssClass="niceTextBox textBoxClass" runat="server" ID="txtGoogleClientId"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtGoogleClientSecret.ClientID %>">Client secret</label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtGoogleClientSecret"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>Redirect URL
        </td>
        <td>
            <%= AdvantShop.Configuration.SettingsMain.SiteUrl.TrimEnd('/') + "/login"%>
        </td>
    </tr>
</table>

<div class="dvSubHelp">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/openid-google-text" target="_blank">Инструкция. Настройка кнопок авторизации OAuth Google</a>
</div>

<table runat="server" id="tableYandex" class="info-tb" style="width: 600px; margin-bottom: 10px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Yandex</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= ckbYandexActive.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthActive%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbYandexActive" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtYandexClientId.ClientID %>">Client ID</label>
        </td>
        <td>
            <asp:TextBox CssClass="niceTextBox textBoxClass" runat="server" ID="txtYandexClientId"></asp:TextBox>
        </td>
    </tr>
     <tr class="rowsPost row-interactive">
        <td>Callback URL
        </td>
        <td>
            <%= AdvantShop.Configuration.SettingsMain.SiteUrl.TrimEnd('/') + "/login"%>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtYandexClientSecret.ClientID %>">Client secret</label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtYandexClientSecret"></asp:TextBox>
        </td>
    </tr>
</table>

<div class="dvSubHelp">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/openid-yandex" target="_blank">Инструкция. Настройка кнопок авторизации OAuth Yandex</a>
</div>
<table runat="server" id="tableMailru" class="info-tb" style="width: 600px; margin-bottom: 10px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Mail.ru</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= ckbMailActive.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthActive%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbMailActive" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMailAppId.ClientID %>">Application ID</label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtMailAppId"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMailClientId.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OAuthOdnoklassnikiPublicApiKey%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtMailClientId"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMailClientSecret.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OAuthOdnoklassnikiSecretApiKey%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtMailClientSecret"></asp:TextBox>
        </td>
    </tr>
</table>

<div class="dvSubHelp">
    <asp:Image ID="Image6" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/openid-mail-ru" target="_blank">Инструкция. Настройка кнопок авторизации OAuth Mail.ru</a>
</div>
<table runat="server" id="tableVk" class="info-tb" style="width: 600px; margin-bottom: 10px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Vk.com</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= ckbVKActive.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthActive%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbVKActive" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtVKAppId.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthVKappid%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtVKAppId"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtVKSecret.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthVKSecretKey%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtVKSecret"></asp:TextBox>
        </td>
    </tr>
</table>

<div class="dvSubHelp">
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/openid-vk" target="_blank">Инструкция. Настройка кнопок авторизации OAuth вКонтакте</a>
</div>

<table runat="server" id="tableFacebook" class="info-tb" style="width: 600px; margin-bottom: 10px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Facebook</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= ckbFacebookActive.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthActive%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbFacebookActive" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtFacebookClientId.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OAuthFbApiKey%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtFacebookClientId"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtFacebookApplicationSecret.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OAuthFbApiSecret%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtFacebookApplicationSecret"></asp:TextBox>
        </td>
    </tr>
</table>

<div class="dvSubHelp">
    <asp:Image ID="Image3" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/openid-fb-text" target="_blank">Инструкция. Настройка кнопок авторизации OAuth Facebook</a>
</div>

<table runat="server" id="tableOdnoklassniki" class="info-tb" style="width: 600px; margin-bottom: 10px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Odnoklassniki.ru</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= ckbOdnoklassnikiActive.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_HeadOAuthActive%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbOdnoklassnikiActive" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOdnoklassnikiClientId.ClientID %>">Application ID</label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtOdnoklassnikiClientId"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOdnoklassnikiPublicApiKey.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OAuthOdnoklassnikiPublicApiKey%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtOdnoklassnikiPublicApiKey"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOdnoklassnikiSecret.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OAuthOdnoklassnikiSecretApiKey%></label>
        </td>
        <td>
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtOdnoklassnikiSecret"></asp:TextBox>
        </td>
    </tr>
</table>

<div class="dvSubHelp">
    <asp:Image ID="Image4" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/openid-odk" target="_blank">Инструкция. Настройка кнопок авторизации OAuth Odnoklassniki (Одноклассники)</a>
</div>

<%--
<table runat="server" id="tableTwitter" style="width: 600px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize6" runat="server" Text="Twitter"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthActive %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:CheckBox ID="ckbTwitterActive" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label3" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwConsumerKey %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtTwitterConsumerKey"
               ></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label4" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwConsumerSecret %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtTwitterConsumerSecret"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">

        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwAccessToken %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtTwitterAccessToken"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">

        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwAccessTokenSecret %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox textBoxClass" runat="server" ID="txtTwitterAccessTokenSecret"></asp:TextBox>

        </td>
    </tr>
</table>--%>
