<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.RatingMailRu.Admin_RatingMailRuSettings" Codebehind="RatingMailRuSettings.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: RatingMailRu_Header %>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 150px; text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: RatingMailRu_Counter%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtCounter" runat="server" Height="200px" Width="700px" TextMode="MultiLine" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize runat="server" Text="<%$ Resources: RatingMailRu_RemarketingSettings %>" /></span>
                <asp:Label ID="Label1" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 150px; text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: RatingMailRu_PriceListID%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtPriceListID" runat="server" Width="200px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: RatingMailRu_Save%>" />
            </td>
        </tr>
    </table>
</div>
