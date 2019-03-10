<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Products.ProductLandingPage" Codebehind="ProductLandingPage.ascx.cs" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>

<table class="table-p">
    <tbody>
        <tr>
            <td class="formheader" colspan="2">
                <h2>Посадочная страница</h2>
            </td>
        </tr>
        <tr>
            <td style="width:210px; padding:10px 10px 10px 0;">
                Ссылка на посадочную страницу:
            </td>
            <td style="padding:10px 0;">
                <asp:ListView runat="server" ID="lvCustomViews">
                    <itemtemplate>
                        <div>
                            <a target="_blank" href='<%# "../" + UrlService.GetLinkDB(ParamType.Product, ProductID) + "?v=" + Container.DataItem %>'>
                                <asp:Literal ID="Literal1" runat="server" Text='<%# SettingsMain.SiteUrl + "/" +  UrlService.GetLinkDB(ParamType.Product, ProductID) + "?v=" + Container.DataItem %>' /></a>
                        </div>
                    </itemtemplate>
                </asp:ListView>
            </td>
        </tr>
         <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="formheader" colspan="2">
                <h2>Дополнительное описание товара</h2>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="ckTabBody" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400px" Width="100%" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="dvSubHelp">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                    <a href="http://www.advantshop.net/help/pages/landing-page-5" target="_blank">Инструкция. Настройка страниц "Landing Page"</a>
                </div>
            </td>
        </tr>
    </tbody>
</table>