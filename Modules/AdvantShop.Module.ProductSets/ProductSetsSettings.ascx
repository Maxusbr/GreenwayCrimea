<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.ProductSets.Admin_ProductSetsSettings" Codebehind="ProductSetsSettings.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: ProductSets_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize runat="server" Text="<%$ Resources: ProductSets_Title %>" />
            </td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr color="#C2C2C4" size="1px" />
                <p><asp:Localize runat="server" Text="<%$ Resources: ProductSets_Message%>" /></p>
                <p style="color: red;"><asp:Localize runat="server" Text="<%$ Resources: ProductSets_Warning%>" /></p>
                <p></p>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr color="#C2C2C4" size="1px" />
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: ProductSets_Save%>" />
            </td>
        </tr>
    </table>
</div>