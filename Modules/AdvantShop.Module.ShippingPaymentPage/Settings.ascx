<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.ShippingPaymentPage.Settings" CodeBehind="Settings.ascx.cs" %>
<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right; margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources: DefaultWeight %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtDefaultWeight" runat="server" Width="50px"></asp:TextBox>
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: kg %>"></asp:Localize>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: DefaultWidth %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtDefaultWidth" runat="server" Width="50px"></asp:TextBox>
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: mm %>"></asp:Localize>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: DefaultHeight %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtDefaultHeight" runat="server" Width="50px"></asp:TextBox>
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: mm %>"></asp:Localize>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: DefaultLength %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtDefaultLength" runat="server" Width="50px"></asp:TextBox>
                <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: mm %>"></asp:Localize>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: DefaultPrice %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtDefaultPrice" runat="server" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: DefaultShippingPrice %>"></asp:Localize>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtDefaultShippingPrice" runat="server" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources: ShippingTextBlock %>"></asp:Localize>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtShippingTextBlock" runat="server" Width="200px" TextMode="MultiLine" CssClass="js-wysiwyg"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources: ShippingTextBlockBottom %>"></asp:Localize>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtShippingTextBlockBottom" runat="server" Width="200px" TextMode="MultiLine" CssClass="js-wysiwyg"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize14" runat="server" Text="Title"></asp:Localize>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize15" runat="server" Text="Keywords"></asp:Localize>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtMetaKeywords" runat="server" Width="400px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize16" runat="server" Text="Decription"></asp:Localize>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtMetaDescription" runat="server" Width="400px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <%--<asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: MaxHeight %>"></asp:Localize>--%>
            </td>
            <td>
                <asp:HyperLink ID="lnkGoToModule" runat="server" Text="<%$ Resources: ModuleUrl %>" Target="_blank"></asp:HyperLink>
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Save %>" />
            </td>
        </tr>
    </table>
</div>
