<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Advantshop.Modules.UserControls.Admin_RitmzSettings" Codebehind="RitmzSettings.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Ritmz_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Ritmz_Login%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtLogin"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Ritmz_Password%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPassword"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: Ritmz_SiteName%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSiteName"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: Ritmz_SiteUrl%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSiteUrl"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Ritmz_Information%>"></asp:Localize>
            </td>
            <td>
                <asp:Label runat="server">/modules/ritmz/exportritmz.ashx</asp:Label><br />
                <asp:Label runat="server">/modules/ritmz/exportritmzproducts.ashx</asp:Label><br />
                <asp:Label runat="server">/modules/ritmz/importordersritmz.ashx</asp:Label><br />
                <asp:Label runat="server">/modules/ritmz/importremainsritmz.ashx</asp:Label>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <br/>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Ritmz_Save%>"/>
            </td>
        </tr>
    </table>
</div>
