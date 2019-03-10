<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.Snowfall.Admin_SnowfallModule" Codebehind="SnowfallModule.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Snowfall_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <%--            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">
                    <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: Snowfall_Active%>"></asp:Localize>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="ckbEnableSnowfall" />
                </td>
            </tr>--%>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Snowfall_Color%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSnowfallColor" data-plugin="jpicker" data-jpicker-options="{window: {position: {y: 'center'}}}"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Snowfall_MinSize%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSnowfallMinSize"></asp:TextBox>
            </td>
        </tr>            
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Snowfall_MaxSize%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSnowfallMaxSize"></asp:TextBox>
            </td>
        </tr>              
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: Snowfall_NewOn%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSnowfallNewOn"></asp:TextBox>
            </td>
        </tr>   
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Snowfall_Save%>" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</div>