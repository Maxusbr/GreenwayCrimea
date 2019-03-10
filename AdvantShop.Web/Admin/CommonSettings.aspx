<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="False" AutoEventWireup="true" Inherits="Admin.CommonSettings" EnableViewStateMac="false" CodeBehind="CommonSettings.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Trial" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/UserControls/EditRobotsTxt.ascx" TagName="EditRobotsTxt" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/OAuthSettings.ascx" TagName="OAuthSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/SocialSettings.ascx" TagName="SocialSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/NewsSettings.ascx" TagName="NewsSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/GeneralSettings.ascx" TagName="GeneralSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/SEOSettings.ascx" TagName="SEOSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/CountersSettings.ascx" TagName="CountersSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/OrderConfirmationSettings.ascx" TagName="OrderConfirmationSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/NotifyEmailsSettings.ascx" TagName="NotifyEmailsSettings" TagPrefix="adv" %>
<%--<%@ Register Src="~/Admin/UserControls/Settings/BankSettings.ascx" TagName="BankSettings" TagPrefix="adv" %>--%>
<%--<%@ Register Src="~/Admin/UserControls/Settings/TaskSettings.ascx" TagName="TaskSettings" TagPrefix="adv" %>--%>
<%@ Register Src="~/Admin/UserControls/Settings/CatalogSettings.ascx" TagName="CatalogSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/ProfitSettings.ascx" TagName="ProfitSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/LicSettings.ascx" TagName="LicSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/DetailsSettings.ascx" TagName="DetailsSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/MailSettings.ascx" TagName="MailSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/ApiSettings.ascx" TagName="ApiSettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/TelephonySettings.ascx" TagName="TelephonySettings" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Settings/MobileSettings.ascx" TagName="MobileSettings" TagPrefix="adv" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="CommonSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
            <li class="neighbor-menu-item"><a href="CheckoutFields.aspx">
                <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
            <li class="neighbor-menu-item"><a href="PaymentMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
            <li class="neighbor-menu-item"><a href="ShippingMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
            <li class="neighbor-menu-item"><a href="Country.aspx">
                <%= Resource.Admin_MasterPageAdmin_Countries%></a></li>
            <li class="neighbor-menu-item"><a href="Currencies.aspx">
                <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
            <li class="neighbor-menu-item"><a href="Taxes.aspx">
                <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
            <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
            <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
            <li class="neighbor-menu-item"><a href="LogError404.aspx">
                <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
            <li class="neighbor-menu-item"><a href="Localizations.aspx">
                <%= Resource.Admin_Localizations_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/settings_ico.gif" alt="" />
                        </td>
                        <td class="style1">
                            <asp:Label ID="lbl" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Header %>"></asp:Label><br />
                            <asp:Label ID="lblName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_CommonSettings %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="label-box-admin good js-error-message" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <div style="float: right; margin-left: 7px; position: relative; text-align: right;">
                                <div data-plugin="transformer" data-transformer-options="{classes: [null, 'btn-save-fixed'], point: 'top' }">
                                    <img src="images/ajax-loader_charts.gif" alt="" class="ajax-inline ajax-inline-hidden js-ajax-inline"><asp:Button CssClass="btn btn-middle btn-add btn-admin-save js-ajax-button" ID="btnSave" runat="server" Text="<%$ Resources:Resource, Admin_Update %>" OnClick="btnSave_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>
        </div>
        <div style="text-align: center;">
            <br />
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GeneralSettings" />
                    <%--<asp:PostBackTrigger ControlID="BankSettings" />--%>
                </Triggers>
                <ContentTemplate>
                    <table cellpadding="0px" cellspacing="0px" style="width: 100%;" id="tabs">
                        <tr>
                            <td style="vertical-align: top; width: 225px;">
                                <ul id="tabs-headers">
                                    <li id="general">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabGeneral%>" />
                                        <img class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="seo">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabSeo  %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="catalog">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabCatalog %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="details">
                                        <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ProductDetails %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="oc">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabOrderConfirmation %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="counters">
                                        <asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabCounters  %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="news">
                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_News %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="profitability">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabProfitability  %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <%--<li id="bankset" runat="server">
                                        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, Admin_UserControl_BankSettings_Head  %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>--%>
                                    <li id="mailset">
                                        <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabMail  %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="mailserver">
                                        <asp:Literal ID="Literal13" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabMailServer  %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="social">
                                        <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_TabSocialNet %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="oauth">
                                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, Admin_UserControl_OAuthSettings_Head %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                <%--    <li id="task">
                                        <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, Admin_UserControl_TaskSettings_Head %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>--%>
                                    <li id="robot">
                                        <asp:Literal ID="Literal2" runat="server" Text="Robots.txt" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <% if (!TrialService.IsTrialEnabled)
                                        { %>
                                    <li id="licence">
                                        <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, Admin_UserControl_License_Head %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <% } %>
                                    <li id="api" runat="server">
                                        <asp:Literal ID="Literal9" runat="server" Text="API" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="telephony">
                                        <asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_IpTelephonySettings %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>
                                    <li id="mobile">
                                        <asp:Literal ID="Literal14" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_MobileVersion %>" />
                                        <img class="floppy" src="images/floppy.gif" alt="" />
                                    </li>

                                </ul>
                                <input type="hidden" runat="server" id="tabid" class="tabid" />
                            </td>
                            <td id="tabs-contents">
                                <div class="tab-content">
                                    <adv:GeneralSettings ID="GeneralSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:SEOSettings ID="SEOSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:CatalogSettings ID="CatalogSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:DetailsSettings ID="DetailsSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:OrderConfirmationSettings ID="OrderConfirmationSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:CountersSettings ID="CountersSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:NewsSettings ID="NewsSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:ProfitSettings ID="ProfitSettings" runat="server" />
                                </div>
                                <%--<div class="tab-content" runat="server" id="tabBankSettings">
                                    <adv:BankSettings ID="BankSettings" runat="server" />
                                </div>--%>
                                <div class="tab-content">
                                    <adv:NotifyEmailsSettings ID="NotifyEmailsSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:MailSettings ID="MailSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:SocialSettings ID="SocialSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:OAuthSettings ID="OAuthSettings" runat="server" />
                                </div>
                             <%--   <div class="tab-content">
                                    <adv:TaskSettings ID="TaskSettings" runat="server" />
                                </div>--%>
                                <div class="tab-content">
                                    <adv:EditRobotsTxt ID="EditRobotsTxt" runat="server" />
                                </div>
                                <% if (!TrialService.IsTrialEnabled)
                                    { %>
                                <div class="tab-content">
                                    <adv:LicSettings ID="LicSettings" runat="server" />
                                </div>
                                <% } %>
                                <div class="tab-content" runat="server">
                                    <adv:ApiSettings ID="ApiSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:TelephonySettings ID="TelephonySettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:MobileSettings ID="MobileSettings" runat="server" />
                                </div>

                            </td>
                        </tr>
                    </table>

                    <script>
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                            tabInit();
                        });
                    </script>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
