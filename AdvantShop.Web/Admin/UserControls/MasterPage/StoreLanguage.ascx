<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.MasterPage.StoreLanguage" Codebehind="StoreLanguage.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<div class="top-part-right dropdown-menu-parent dropdown-arrow-light">
    <img src="images/new_admin/lang/<%= SettingsMain.Language.Split('-')[0] %>.jpg" class="lang-selected" />
    <div class="dropdown-menu-wrap">
        <ul class="dropdown-menu">
            <asp:Repeater runat="server" ID="rprLanguages">
                <ItemTemplate>
                    <li class="dropdown-menu-item">
                        <a class="js-lang" data-lang-id="<%# Eval("LanguageId") %>" href="#">
                            <img src="images/new_admin/lang/<%# Eval("LanguageCode").ToString().Split('-')[0] %>.jpg" alt="<%# Eval("Name") %>" class="flag"/>
                            <%# Eval("Name") %>
                        </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</div>
