<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.PostAffiliatePro.Admin_PostAffiliateProSettings" CodeBehind="PostAffiliateProSettings.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: PostAffiliatePro_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 120px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: PostAffiliatePro_Login%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtPostAffiliateProLogin" runat="server" Width="300px"></asp:TextBox>
                <div data-plugin="help" class="help-block">
                    <div class="help-icon js-help-icon"></div>
                    <article class="bubble help js-help">
                        <header class="help-header">
                            Url
                        </header>
                        <div class="help-content">
                            Адрес, на котором установлен Post Affiliate Pro
                        </div>
                    </article>
                </div>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 120px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: PostAffiliatePro_AccountId%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtPostAffiliateProAccount" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: PostAffiliatePro_Save%>" />
            </td>
        </tr>
    </table>
</div>
