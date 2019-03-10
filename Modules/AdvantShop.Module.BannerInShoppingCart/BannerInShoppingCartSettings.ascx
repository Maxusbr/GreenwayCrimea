<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.BannerInShoppingCart.BannerInShoppingCartSettings" CodeBehind="BannerInShoppingCartSettings.ascx.cs" %>

<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Header%>" />
                </span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right; margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <%--  <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: PageTitle %>"></asp:Localize>
            </td>
            <td>
                <asp:FileUpload ID="fuBanner" runat="server" />
            </td>
        </tr>--%>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: PageTitle %>"></asp:Localize>
            </td>
            <td>
                 <asp:TextBox ID="txtStaticBlock" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="100%" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Save %>" />
            </td>
        </tr>
    </table>
</div>
