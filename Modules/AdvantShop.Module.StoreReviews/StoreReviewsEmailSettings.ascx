<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.StoreReviews.Admin_StoreReviewsEmailSettings" Codebehind="StoreReviewsEmailSettings.ascx.cs" %>
<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: StoreReviewsMails_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right; margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 200px; text-align: left;">
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: StoreReviewsMails_Active%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="ckbEnableSendMails" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: StoreReviewsMails_Email %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: StoreReviewsMails_Subject %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtSubject" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: StoreReviewsMails_Format %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtFormat" runat="server" TextMode="MultiLine" Width="200px" Height="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources:StoreReviews_Save %>" />
            </td>
        </tr>
    </table>
</div>