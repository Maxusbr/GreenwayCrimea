<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.BingImagesSearch.Admin_BingImagesSearchModule" Codebehind="BingImagesSearchModule.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize runat="server" Text="<%$ Resources: Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="Blue" Style="float: right;" />
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px;">
                <asp:Localize runat="server" Text="<%$ Resources: ApiKey %>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtApiKey" Width="300px" />
            </td>
        </tr>      
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Save%>" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</div>