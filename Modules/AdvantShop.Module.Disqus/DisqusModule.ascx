<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.Disqus.Admin_DisqusModule" Codebehind="DisqusModule.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Disqus_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                Перейти на сайт <a href="https://disqus.com/" target="_blank">disqus.com</a>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Disqus_WebsiteShortname %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtShortname"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Disqus_Save%>" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</div>