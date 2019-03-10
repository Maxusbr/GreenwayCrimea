<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="False"
    AutoEventWireup="true" Inherits="Admin.DesignConstructor"
    Title="" Codebehind="DesignConstructor.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<%@ Register Src="~/Admin/UserControls/Design/ThemesSettings.ascx" TagName="ThemesSettings"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Design/TemplatesSettings.ascx" TagName="TemplatesSettings"
    TagPrefix="adv" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="DesignConstructor.aspx">
                <%= Resource.Admin_MasterPageAdmin_DesignConstructor%></a></li>
            <li class="neighbor-menu-item"><a href="TemplateSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_TemplateSettings%></a></li>           
             <li class="neighbor-menu-item"><a href="StylesEditor.aspx">
                <%= Resource.Admin_MasterPageAdmin_StylesEditor%></a></li>  
        </menu>
    </div>
    <div class="content-own">
        <style type="text/css">
            #tabs-contents td { vertical-align: middle; }
        </style>
        <div>
            <table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px;">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/settings_ico.gif" alt="" />
                        </td>
                        <td class="style1">
                            <asp:Label ID="lbl" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Header %>"></asp:Label><br />
                            <asp:Label ID="lblName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_DesignSettings %>"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>
        </div>
        <div style="text-align: center;">
            <asp:Label ID="lblError" runat="server" ForeColor="Blue" Visible="False"></asp:Label>
            <br />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="themesUP">
                <ProgressTemplate>
                    <div id="inprogress">
                        <div id="curtain" class="opacitybackground">
                            &nbsp;</div>
                        <div class="loader">
                            <table width="100%" style="font-weight: bold; text-align: center;">
                                <tbody>
                                    <tr>
                                        <td align="center">
                                            <img src="images/ajax-loader.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="color: #0D76B8;">
                                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="themesUP" runat="server">
                <ContentTemplate>
                    <table cellpadding="0px" cellspacing="0px" style="width: 100%;" id="tabs2">
                        <tr>
                            <td style="vertical-align: top; width: 225px;">
                                <ul id="tabs-headers">
                                    <li id="templates">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Templates %>" />
                                    </li>
                                    <li id="themes">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Themes %>" />
                                    </li>
                                    <li id="backgrounds">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Backgrounds %>" />
                                    </li>
                                    <li id="colors">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Colors  %>" />
                                    </li>
                                </ul>
                                <input type="hidden" runat="server" id="tabid" class="tabid" />
                            </td>
                            <td id="tabs-contents">
                                <div class="tab-content">
                                    <adv:TemplatesSettings ID="TemplatesSettings" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:ThemesSettings ID="ThemesSettings" runat="server" DesignType="Theme" />
                                </div>
                                <div class="tab-content">
                                    <adv:ThemesSettings ID="BackgroundsSettings" runat="server" DesignType="Background" />
                                </div>
                                <div class="tab-content">
                                    <adv:ThemesSettings ID="ColorSettings" runat="server" DesignType="Color" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <script type="text/javascript">
                        function setupTooltips() {
                            $(".imgtooltip").tooltip({
                                showURL: false,
                                bodyHandler: function () {
                                    var imagePath = $(this).attr("abbr");
                                    if (imagePath.length == 0) {
                                        return "<div><span><%= Resource.Admin_Catalog_NoMiniPicture %></span></div>";
                                    }
                                    else {
                                        return $("<img/>").attr("src", imagePath);
                                    }
                                }
                            });
                            $(".showtooltip").tooltip({
                                showURL: false
                            });
                        }
                        function tabInit2() {
                            if ($("#tabs2").length) {
                                $("#tabs2").advTabs({
                                    headers: "#tabs-headers > li",
                                    contents: "#tabs-contents  div.tab-content"
                                });
                            }
                        }
                        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); tabInit2(); });
                    </script>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
