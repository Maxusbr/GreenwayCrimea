<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Design.TemplatesSettings" CodeBehind="TemplatesSettings.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<asp:Label ID="lblError" runat="server" Text="" Style="color: blue; padding: 10px; display: block; border: solid 1px blue; width: 300px; margin: 0 0 10px 0;"
    Visible="false" />

<asp:Label ID="lTrialMode" runat="server" Text="<%$ Resources:Resource, Admin_Design_TrialMode %>" ForeColor="Red"></asp:Label><br />
<br />
<asp:ListView ID="DataListTemplates" runat="server" OnItemCommand="dlItems_ItemCommand"
    ItemPlaceholderID="itemPlaceholderId">
    <LayoutTemplate>
        <div class="admin-templates">
            <div runat="server" id="itemPlaceholderId" />
        </div>
    </LayoutTemplate>
    <ItemTemplate>
        <div class="template-admin-item <%# (Convert.ToBoolean(Eval("IsInstall")) && SettingsDesign.Template == Convert.ToString(Eval("StringId"))) ? "current" : "" %>">
            <%# RenderTemplatePicture((string)Eval("Icon"), (string)Eval("StringId"))%>
            <div class="title">
                <%#Eval("Name")%>
            </div>
            <asp:Literal ID="lblCost" runat="server" Text='<%# Convert.ToBoolean(Eval("IsInstall")) ? "" : (Convert.ToDecimal(Eval("Price")) != 0 ? String.Format("{0:##,##0.##}", Eval("Price")) + " " + Eval("Currency") : Resources.Resource.Admin_Modules_FreeCost)%>' />
            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("StringId") %>' />
            <div>
                <div class="left">
                    <asp:LinkButton CssClass="template-admin-apply" ID="btnActivate" runat="server" CommandArgument='<%# Eval("StringId") %>'
                        CommandName="ApplyTheme" Visible='<%# Convert.ToBoolean(Eval("IsInstall")) && Convert.ToBoolean(Eval("Active")) && SettingsDesign.Template != Convert.ToString(Eval("StringId")) %>'>
                            <%= Resources.Resource.Admin_ThemesSettings_Apply %> 
                            <span class="triangle"></span>
                    </asp:LinkButton>
                    <asp:HyperLink ID="btnSettings" runat="server" NavigateUrl="~/Admin/TemplateSettings.aspx"
                        Text='<%$ Resources:Resource, Admin_ThemesSettings_Settings %>' Visible='<%# Convert.ToBoolean(Eval("IsInstall")) && SettingsDesign.Template == Convert.ToString(Eval("StringId")) %>' />
                    <asp:HyperLink runat="server" ID="lbBuy" CssClass="addbtn showtooltip" NavigateUrl='<%# Eval("DetailsLink") %>'
                        Target="_blank" Text='<%$ Resources:Resource, Admin_ThemesSettings_BuyTheme %>'
                        ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_BuyTheme %>' Visible='<%# !Convert.ToBoolean(Eval("IsInstall")) && !Convert.ToBoolean(Eval("Active")) %>' />
                </div>
                <div class="right">
                    <asp:LinkButton runat="server" ID="btnAdd" CausesValidation="false" 
                        CommandArgument='<%# Eval("id") != null && Eval("id").ToString() != "0" ? Eval("Id") : Eval("StringId") %>'
                        CommandName="Add" CssClass="addbtn showtooltip themeadd" Text='<%$ Resources:Resource, Admin_ThemesSettings_Add %>'
                        ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Add %>' 
                        Visible='<%# !Convert.ToBoolean(Eval("IsInstall")) && Convert.ToBoolean(Eval("Active")) && _default != Convert.ToString(Eval("StringId")) && !AdvantShop.Trial.TrialService.IsTrialEnabled %>' />

                    <asp:LinkButton runat="server" ID="btnDelete" CausesValidation="false" 
                        CommandArgument='<%# Eval("StringId") %>'
                        CommandName="Delete" CssClass="valid-confirm showtooltip themedel" Text='<%$ Resources:Resource, Admin_ThemesSettings_Delete %>'
                        data-confirm="<%$ Resources:Resource, Admin_ThemesSettings_Confirmation %>"
                        ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>'
                        Visible='<%# Convert.ToBoolean(Eval("IsInstall")) && Convert.ToBoolean(Eval("Active")) && _default != Convert.ToString(Eval("StringId"))&& !AdvantShop.Trial.TrialService.IsTrialEnabled %>' />

                    <asp:LinkButton runat="server" ID="btnUpdate" CausesValidation="false" 
                        CommandArgument='<%# Eval("Id") %>'
                        CommandName="InstallLastVersion" CssClass="valid-confirm showtooltip themeupdate" Text='<%$ Resources:Resource, Admin_Modules_Update %>'
                        data-confirm="<%$ Resources:Resource, Admin_ThemesSettings_Update %>"
                        ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Update %>'
                        Visible='<%# Convert.ToBoolean(Eval("IsInstall")) && Convert.ToBoolean(Eval("Active")) && _default != Convert.ToString(Eval("StringId"))  && !Convert.ToString(Eval("Version")).Equals(Convert.ToString(Eval("CurrentVersion"))) && !AdvantShop.Trial.TrialService.IsTrialEnabled %>' />
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
    </ItemTemplate>
</asp:ListView>
