<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.GeneralSettings" Codebehind="GeneralSettings.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Register Src="~/Admin/UserControls/LogoImage.ascx" TagName="Logo" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/MasterPage/Favicon.ascx" TagName="Favicon" TagPrefix="adv" %>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadGeneral%>
            </span>
            <br />
            <span class="subTitleNotify">
                Настройки магазина, включая основные параметры и базовый внешний вид
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width:250px;">
            <label class="form-lbl" for="<%= fuLogoImage.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_LogoImage%></label>
        </td>
        <td>
            <asp:Panel ID="pnlLogo" runat="server" Width="100%">
                <div class="dvLogoBorder">
                    <adv:Logo ID="Logo" EnableHref="False" runat="server"/>
                </div>
                <div class="dvLogoDelete">
                    <asp:Button ID="DeleteLogo" runat="server" Text="<%$ Resources:Resource, Admin_Delete%>" OnClick="DeleteLogo_Click" />
                </div>
            </asp:Panel>
            <asp:FileUpload ID="fuLogoImage" runat="server" Height="20px" Width="308px" BackColor="White" />
            &nbsp;<a  href="../?logoGeneratorEditOnPageLoad=true" target="_blank">Создать логотип</a>
            <div class="subSaveNotify">
                <%= Resources.Resource.Admin_CommonSettings_LogoImageSize%>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= fuFaviconImage.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Favicon%></label>
        </td>
        <td style="padding:10px 0px;">
            <asp:Panel ID="pnlFavicon" runat="server" Width="100%">
                <div class="dvFaviIco">
                    <adv:Favicon ID="Favicon" runat="server" GetOnlyImage="true" CssClassImage="imgFaviIco" ForAdmin="True" />
                </div>
                <div class="dvFaviIcoDelete">
                    <asp:Button ID="DeleteFavicon" runat="server" Text="<%$ Resources:Resource, Admin_Delete%>" OnClick="DeleteFavicon_Click" />
                </div>
            </asp:Panel>
            <asp:FileUpload ID="fuFaviconImage" runat="server" Height="20px" Width="308px" BackColor="White" />
            <div class="subSaveNotify">
                <%= Resources.Resource.Admin_CommonSettings_FaviconFormat%>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtShopURL.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShopURL%></label>
        </td>
        <td>
            <asp:TextBox ID="txtShopURL" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Url адрес магазина
                    </header>
                    <div class="help-content">
                        Тут вы указываете, какой адрес был куплен для магазина. Данный параметр влияет на построение ссылок по всей клиентской части магазина.
                        <br /><br />
                        Обратите внимание, что данный параметр лишь указывает, какой домен используется для магазина, сам домен должен быть куплен и привязан к сайту заранее.
                        <br />
                        <br />
                        Подробнее о домене:
                        <br />
                        <a href="http://www.advantshop.net/help/pages/svoy-domen" target="_blank">Инструкция. Как привязать магазин к домену (доменному имени)?</a>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_CommonSettings_ShopURL_FB%>
        </td>
        <td>
            <asp:Label runat="server" ID="lSocialLinkFb" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Url адрес магазина для facebook.com (Facebook)
                    </header>
                    <div class="help-content">
                        Адрес магазина, который необходимо использовать при настройке отображения магазина в соц. сети facebook.
                        <br />
                        <br />
                        <a href="http://www.advantshop.net/help/pages/install-eshop-facebook" target="_blank">Инструкция. Настройка интернет-магазина в соц. сети facebook.com</a>
                    </div>
                </article>
            </div>
            <label><asp:CheckBox runat="server" ID="chkFbTemplateActive" />
            <asp:Label runat="server" ID="lblFbTemplateActive" /></label>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_CommonSettings_ShopURL_VK%>
        </td>
        <td>
            <asp:Label runat="server" ID="lSocialLinkVk" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Url адрес магазина для vk.com (ВКонтакте) 
                    </header>
                    <div class="help-content">
                        Адрес магазина, который необходимо использовать при настройке отображения магазина в соц. сети ВКонтакте.
                        <br />
                        <br />
                        <a href="http://www.advantshop.net/help/pages/create-store-vk" target="_blank">Инструкция. Настройка интернет-магазина в соц. сети ВКонтакте</a>
                    </div>
                </article>
            </div>
            <label><asp:CheckBox runat="server" ID="chkVkTemplateActive" />
            <asp:Label runat="server" ID="lblVkTemplateActive" /></label>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtShopName.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShopName%></label>
        </td>
        <td>
            <asp:TextBox ID="txtShopName" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Название магазина
                    </header>
                    <div class="help-content">
                        Краткое описание названия магазина. Будет использовано в SEO полях, в качестве переменной #STORE_NAME#.
                        <br /><br />
                        Используйте небольшое описание, например:
                        <br />
                        <i>MyShop.ru - Товары для животных</i>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtImageAlt.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ImgAlt%></label>
        </td>
        <td>
            <asp:TextBox ID="txtImageAlt" class="niceTextBox textBoxClass" runat="server"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        "Alt" тэг изображения логотипа
                    </header>
                    <div class="help-content">
                        Тут можно задать, какой текст указать в тэге alt для логотипа.
                        <br /><br />
                        Обычно используется то же, что указывается в поле "Название магазина"
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtFormat.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Format%></label>
        </td>
        <td>
            <asp:TextBox ID="txtFormat" class="niceTextBox textBoxClass" runat="server"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Формат даты в части администрирования
                    </header>
                    <div class="help-content">
                        Вы можете указать свой формат, однако мы рекомендуем оставить формат по умолчанию (как есть)
                        <br /><br />
                        По умолчанию: <b>dd.MM.yyyy HH:mm:ss</b>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtShortFormat.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShortFormat%></label>
        </td>
        <td>
            <asp:TextBox ID="txtShortFormat" class="niceTextBox textBoxClass" runat="server"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Короткий формат даты
                    </header>
                    <div class="help-content">
                        Вы можете указать свой формат, однако мы рекомендуем оставить формат по умолчанию (как есть)
                        <br /><br />
                        По умолчанию: <b>dd MMMM yyyy</b>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCountry.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_SalerCountry%></label>
        </td>
        <td>
            <asp:SqlDataSource SelectCommand="SELECT CountryID, CountryName FROM [Customers].[Country]"
                ID="sdsCountry" runat="server" OnInit="sds_Init"></asp:SqlDataSource>
            <asp:DropDownList runat="server" Cssclass="niceTextBox" OnSelectedIndexChanged="ddlCountry_SelectedChanged"
                AutoPostBack="true" DataSourceID="sdsCountry" ID="ddlCountry" DataTextField="CountryName"
                DataValueField="CountryID">
            </asp:DropDownList>
            <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Страна продавца
                    </header>
                    <div class="help-content">
                        Выставите страну, в которой находится магазин или склад.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlRegion.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_SalerRegion%></label>
        </td>
        <td>
            <asp:SqlDataSource SelectCommand="SELECT RegionID, RegionName FROM [Customers].[Region] WHERE [CountryID] = @CountryID"
                ID="sdsRegion" runat="server" OnInit="sds_Init">
                <SelectParameters>
                    <asp:ControlParameter Name="CountryID" Type="Int32" ControlID="ddlCountry" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:DropDownList runat="server" Cssclass="niceTextBox" ID="ddlRegion" DataSourceID="sdsRegion" 
                DataValueField="RegionID" DataTextField="RegionName">
            </asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Регион продавца
                    </header>
                    <div class="help-content">
                        Выставите регион, в котором находится магазин или склад.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtCity.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_City%></label>
        </td>
        <td>
            <asp:TextBox ID="txtCity" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Город
                    </header>
                    <div class="help-content">
                        Выставите город, в котором находится магазин или склад. Этот город будет выбран по умолчанию для клиента, если не удалось определить город клиента.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtPhone.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Phone%></label>
        </td>
        <td style="padding:10px 0px;">
            <asp:TextBox ID="txtPhone" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Телефон
                    </header>
                    <div class="help-content">
                        Основной телефон магазина, будет показан для всех городов, для которых не задан телефон.<br />
                        <br />
                        Вы так же можете изменить внешний вид номера телефона и указать 2 и более номера - <a href="http://www.advantshop.net/help/pages/phone-change" target="_blank">подробнее</a>
                    </div>
                </article>
            </div>
            <br />
            <span class="subSaveNotify">
                Вы так же можете <a href="Cities.aspx?countryid=<%= SettingsMain.SellerCountryId %>"><%= Resources.Resource.Admin_CommonSettings_OtherCitiesPhones %></a>
            </span>
        </td>
    </tr>


    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMobilePhone.ClientID %>">Телефон мобильной версии</label>
        </td>
        <td style="padding:10px 0px;">
            <asp:TextBox ID="txtMobilePhone" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Телефон
                    </header>
                    <div class="help-content">
                        Основной телефон магазина для мобильной версии, будет показан для всех городов, для которых не задан телефон.<br />
                        <br />
                        Телефон вводится без форматирования
                    </div>
                </article>
            </div>
            <br />
            <span class="subSaveNotify">
                Вы так же можете <a href="Cities.aspx?countryid=<%= SettingsMain.SellerCountryId %>"><%= Resources.Resource.Admin_CommonSettings_OtherCitiesPhones %></a>
            </span>
        </td>
    </tr>


    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkIsStoreClosed.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_IsSoreClosed%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkIsStoreClosed" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_CommonSettings_IsSoreClosed%>
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_CommonSettings_IsSoreClosedHint%>
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbEnableCheckConfirmCode.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_CheckConfirmCode%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbEnableCheckConfirmCode" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Включить CAPTCHA
                    </header>
                    <div class="help-content">
                        Включить защиту от роботов. Картинка, содержание которой необходимо ввести, чтобы подтвердить, что вы не робот.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <%--<tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbEnableCheckConfirmCode.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnablePhoneMask%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbEnablePhoneMask" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_CommonSettings_EnablePhoneMask %>
                    </header>
                    <div class="help-content">
                        Включить маску ввода номера телефона +7(000)000-00-00
                    </div>
                </article>
            </div>
        </td>
    </tr>--%>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbEnableInplaceEditor.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableInplace%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbEnableInplaceEditor" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        In-place редактирование
                    </header>
                    <div class="help-content">
                        Включить или выключить In-place редактирование в клиентской части для администратора.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbDisplayToolBarBottom.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayToolBarBottom%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbDisplayToolBarBottom" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать нижнюю панель
                    </header>
                    <div class="help-content">
                        Включать или нет отображение нижней вспомогательной панели в клиентской части магазина.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbDisplayCityInTopPanel.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayCityInTopPanel%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbDisplayCityInTopPanel" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Определять город автоматически
                    </header>
                    <div class="help-content">
                        Использовать ли механизм определения города клиента.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbDisplayCityBubble.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayCityBubble%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbDisplayCityBubble" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Спрашивать город
                    </header>
                    <div class="help-content">
                        Использовать окошко уточнения верно ли выбран город, в котором находится клиент.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbShowCopyright.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowCopyright%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbShowCopyright" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать Copyright
                    </header>
                    <div class="help-content">
                        Опция включает или отключает отображение надписей об Copyright
                    </div>
                </article>
            </div>
        </td>
    </tr>

   <%-- <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbEnableLoging.ClientID %>">Включить логирование действий пользователя</label>
        </td>
        <td>
            <asp:CheckBox ID="ckbEnableLoging" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Включить логирование действий пользователя
                    </header>
                    <div class="help-content">
                        Опция включает или отключает логирование действий пользователя на сайте, отправленных sms, e-mail. 
                    </div>
                </article>
            </div>
        </td>
    </tr>--%>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkShowClientId.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowClientId%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowClientId" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать Номер корзины
                    </header>
                    <div class="help-content">
                        Опция включает или отключает отображение Номера корзины
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost" runat="server" id="trTrialClearShopHeader">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_ClearShopHeader%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr runat="server" id="trTrialClearShop">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Button runat="server" ID="bntClearShop" OnClick="bntClearShop_OnClick" Text="<%$Resources:Resource, Admin_ClearShop %>"
                CssClass="valid-confirm" data-confirm="<%$ Resources:Resource, Admin_ClearShopConfirm %>" />
        </td>
        <td></td>
    </tr>

</table>
