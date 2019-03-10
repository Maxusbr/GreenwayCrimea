<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Settings.SEOSettings" CodeBehind="SEOSettings.ascx.cs" %>
<%@ Import Namespace="AdvantShop.SEO" %>
<asp:UpdateProgress runat="server" ID="uprogress">
    <ProgressTemplate>
        <div id="inprogress">
            <div id="curtain" class="opacitybackground">
                &nbsp;
            </div>
            <div class="loader">
                <table width="100%" style="font-weight: bold; text-align: center;">
                    <tbody>
                        <tr>
                            <td align="center">
                                <img src="images/ajax-loader.gif" alt="" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="color: #0D76B8;">
                                <asp:Localize ID="Localize_Admin_Product_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Product_PleaseWait %>"
                                    EnableViewState="false"></asp:Localize>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_Products%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 240px;">
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtProductsHeadTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #PRODUCT_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>H1
        </td>
        <td>
            <asp:TextBox ID="txtProductsH1" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #PRODUCT_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost rowsPostBig row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtProductsMetaKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #PRODUCT_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost rowsPostBig row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtProductsMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #PRODUCT_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost rowsPostBig row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_AdditionalDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtProductsAdditionalDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="" /><br />
            <span style="color: Gray; font-size: 11px; display: block;">
                <%= Resources.Resource.Admin_m_Product_UseGlobalVariables%>
            </span>
        </td>
    </tr>

    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <a href="javascript:void(0)" onclick="resetMeta('<%= MetaType.Product %>')">
                <%= Resources.Resource.Admin_CommonSettings_SEO_ResetProductsMeta %>
            </a>
        </td>
    </tr>

    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <div class="dvSubHelp">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
                <a href="https://www.advantshop.net/help/pages/settings_of_seo_module" target="_blank">Инструкция. Настройка мета по умолчанию для магазина</a>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_Categories%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtCategoriesHeadTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #CATEGORY_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>H1
        </td>
        <td>
            <asp:TextBox ID="txtCategoriesMetaH1" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #CATEGORY_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtCategoriesMetaKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #CATEGORY_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtCategoriesMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #CATEGORY_NAME#" /><br />
            <span style="color: Gray; font-size: 11px; display: block;">
                <%= Resources.Resource.Admin_m_Category_UseGlobalVariables%>
            </span>
        </td>
    </tr>
    
    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <a href="javascript:void(0)" onclick="resetMeta('<%= MetaType.Category %>')">
                <%= Resources.Resource.Admin_CommonSettings_SEO_ResetCategoriesMeta %>
            </a>
        </td>
    </tr>

    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <div class="dvSubHelp">
                <asp:Image ID="Image4" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
                <a href="http://www.advantshop.net/help/pages/settings_of_seo_module" target="_blank">Инструкция. Настройка мета по умолчанию для магазина</a>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadNewsSEO%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtNewsHeadTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #NEWS_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>H1
        </td>
        <td>
            <asp:TextBox ID="txtNewsH1" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #NEWS_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtNewsMetaKeywords" runat="server" CssClass="niceTextArea textArea2Lines" TextMode="MultiLine" Text="#STORE_NAME# - #NEWS_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtNewsMetaDescription" runat="server" CssClass="niceTextArea textArea2Lines" TextMode="MultiLine" Text="#STORE_NAME# - #NEWS_NAME#" /><br />
            <span style="color: Gray; font-size: 11px; display: block;">
                <%= Resources.Resource.Admin_m_News_UseGlobalVariables%>
            </span>
        </td>
    </tr>
    
    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <a href="javascript:void(0)" onclick="resetMeta('<%= MetaType.News %>')">
                <%= Resources.Resource.Admin_CommonSettings_SEO_ResetNewsMeta %>
            </a>
        </td>
    </tr>
    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <div class="dvSubHelp">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
                <a href="http://www.advantshop.net/help/pages/settings_of_seo_module" target="_blank">Инструкция. Настройка мета по умолчанию для магазина</a>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_StaticPages%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtStaticPageHeadTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #PAGENAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>H1
        </td>
        <td>
            <asp:TextBox ID="txtStaticPageH1" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #PAGENAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtStaticPageMetaKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #PAGENAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtStaticPageMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #PAGENAME#" /><br />
            <span style="color: Gray; font-size: 11px; display: block;">
                <%= Resources.Resource.Admin_StaticPage_UseGlobalVariables%>
            </span>
        </td>
    </tr>
    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <a href="javascript:void(0)" onclick="resetMeta('<%= MetaType.StaticPage %>')">
                <%= Resources.Resource.Admin_CommonSettings_SEO_ResetStaticPagesMeta %>
            </a>
        </td>
    </tr>

    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <div class="dvSubHelp">
                <asp:Image ID="Image6" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
                <a href="http://www.advantshop.net/help/pages/settings_of_seo_module" target="_blank">Инструкция. Настройка мета по умолчанию для магазина</a>
            </div>
        </td>
    </tr>
    
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_Tags%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtTagsTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #TAG_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>H1
        </td>
        <td>
            <asp:TextBox ID="txtTagsH1" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #TAG_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtTagsMKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #TAG_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtTagsMDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME# - #TAG_NAME#" /><br />
            <span style="color: Gray; font-size: 11px; display: block;">
                <%= Resources.Resource.Admin_Tags_UseGlobalVariables%>
            </span>
        </td>
    </tr>
    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <a href="javascript:void(0)" onclick="resetMeta('<%= MetaType.Tag %>')">
                <%= Resources.Resource.Admin_CommonSettings_SEO_ResetTagsMeta %>
            </a>
        </td>
    </tr>

    <tr class="rowsPost rowsPostBig">
        <td colspan="2">
            <div class="dvSubHelp">
                <asp:Image ID="Image7" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
                <a href="http://www.advantshop.net/help/pages/settings_of_seo_module" target="_blank">Инструкция. Настройка мета по умолчанию для магазина</a>
            </div>
        </td>
    </tr>

    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                Производители (мета по умолчанию):
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtBrandItemMetaTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #BRAND_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            H1
        </td>
        <td>
            <asp:TextBox ID="txtBrandItemH1" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME# - #BRAND_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtBrandItemMetaKeywords" runat="server" CssClass="niceTextArea textArea2Lines" TextMode="MultiLine" Text="#STORE_NAME# - #BRAND_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtBrandItemMetaDescription" runat="server" CssClass="niceTextArea textArea2Lines" TextMode="MultiLine" Text="#STORE_NAME# - #BRAND_NAME#" /><br />
            <span style="color: Gray; font-size: 11px; display: block;">
                Доступные переменные: #STORE_NAME#, #BRAND_NAME#
            </span>
        </td>
    </tr>
    

    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadAnotherPages%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtMetaKeys" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME#" /><br />
            <span class="subParamNotify">
                <%= Resources.Resource.Admin_UseGlobalVariables%>
            </span>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_SEO_BrandsPage %>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtBrandTitle" runat="server" CssClass="niceTextBox textBoxClass" Text="#STORE_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtBrandMetaKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME#" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtBrandMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" Text="#STORE_NAME#" /><br />
            <span class="subParamNotify">
                <%= Resources.Resource.Admin_UseGlobalVariables%>
            </span>
        </td>
    </tr>
</table>
<div class="dvSubHelp">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/settings_of_seo_module" target="_blank">Инструкция. Настройка мета по умолчанию для магазина</a>
</div>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_OtherSEO%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 240px;">
            <%= Resources.Resource.Admin_SettingsSEO_CustomMetaString%>
        </td>
        <td>
            <asp:TextBox ID="txtCustomMetaString" runat="server" CssClass="niceTextBox textBoxLong" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Тег в Head 
                    </header>
                    <div class="help-content">
                        Дополнительный тег в Head.<br />
                        Используется для подтверждения владения сайтом.
                        <br />
                        <br />
                        Например: <b>&lt;meta name=&quot;generator&quot; content=&quot;AdVantShop.NET&quot;&gt;</b><br />
                        ! Не стоит использовать для вставки счётчиков посещаемости, для этого используйте статические блоки.
                    </div>
                </article>
            </div>
            <br />
            <span class="subParamNotify">Не следует использовать данное поле для вставки счетчиков.
            </span>
        </td>
    </tr>
</table>
<div class="dvSubHelp">
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
    <a href="http://www.advantshop.net/help/pages/add-tag-head" target="_blank">Инструкция. Дополнительный тег в Head</a>
</div>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_MetaData%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_MetaData_OpenGraph%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 240px;">
            <%= Resources.Resource.Admin_CommonSettings_OpenGraph_Enabled%>
        </td>
        <td>
            <asp:CheckBox ID="chkOpenGraphEnabled" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            fb:admins
        </td>
        <td>
            <asp:TextBox ID="txtOpenGraphFbAdmins" runat="server" CssClass="niceTextBox textBoxClass" />
        </td>
    </tr>
</table>

<div>
    <a href="#" class="adv-seo-options">Доп опции</a>
</div>
<div class="adv-seo-options-block" style="display: none">
    <table class="info-tb">
        <tr class="rowsPost row-interactive">
            <td style="width: 240px;">
                Поддержка кирилических символов в url адресах
            </td>
            <td>
                <asp:CheckBox ID="chkEnableCyrillicUrl" runat="server" /> 
                <div>
                    Внимание! Не рекомендуется использовать, на свой страх и риск.
                </div>
            </td>
        </tr>
    </table>    
</div>
<script type="text/javascript">
    $(function () {
        $("body").on("click", ".adv-seo-options", function(e) {
            $(".adv-seo-options-block").toggle();
            e.preventDefault();
        });
    });
</script>
