<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.Convead.Admin_ConveadSettings" Codebehind="ConveadSettings.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Convead_Header %>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 150px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Convead_AppKey%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtAppKey" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 10px 0 0 0">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Convead_Description%>"></asp:Localize>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Convead_Save%>" />
            </td>
        </tr>
    </table>
</div>