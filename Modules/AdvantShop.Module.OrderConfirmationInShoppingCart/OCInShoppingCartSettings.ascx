<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.OrderConfirmationInShoppingCart.OCInShoppingCartSettings" Codebehind="OCInShoppingCartSettings.ascx.cs" %>
<table>
    <tr class="rowsPost">
        <td style="height: 34px;" colspan="2">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <%--<tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblIsShowName" runat="server" Text="<%$ Resources: IsShowName%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbIsShowName" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblIsShowEmail" runat="server" Text="<%$ Resources: IsShowEmail%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbIsShowEmail" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblIsShowPhone" runat="server" Text="<%$ Resources: IsShowPhone%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbIsShowPhone" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblIsShowComment" runat="server" Text="<%$ Resources: IsShowComment%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbIsShowComment" runat="server" />
        </td>
    </tr>
     <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblIsRequiredName" runat="server" Text="<%$ Resources: IsRequiredName%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbIsRequiredName" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblIsRequiredEmail" runat="server" Text="<%$ Resources: IsRequiredEmail%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbIsRequiredEmail" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblIsRequiredPhone" runat="server" Text="<%$ Resources: IsRequiredPhone%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbIsRequiredPhone" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblValueOfNameBlock" runat="server" Text="<%$ Resources: ValueNameBlock%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtValueOfNameBlock" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblValueOfEmailBlock" runat="server" Text="<%$ Resources: ValueEmailBlock%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtValueOfEmailBlock" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblValueOfPhoneBlock" runat="server" Text="<%$ Resources: ValuePhoneBlock%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtValueOfPhoneBlock" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblValueOfCommentBlock" runat="server" Text="<%$ Resources: ValueCommentBlock%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtValueOfCommentBlock" runat="server"></asp:TextBox>
        </td>
    </tr>
    
      <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblMinimalOrderPrice" runat="server" Text="<%$ Resources: MinimalOrderPrice%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtMinimalOrderPrice" runat="server"></asp:TextBox>
        </td>
    </tr>
      <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblEmailForOrders" runat="server" Text="<%$ Resources: EmailForOrders%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailForOrders" runat="server"></asp:TextBox>
        </td>
    </tr>
--%>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Label ID="lblFirstText" runat="server" Text="<%$ Resources: FirstText%>"></asp:Label>
        </td>
        <td>
             <asp:TextBox ID="ckFirstText" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="100px" Width="700px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Label ID="lblFinalText" runat="server" Text="<%$ Resources: FinalText%>"></asp:Label>
        </td>
        <td>
             <asp:TextBox ID="ckFinalText" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" Width="700px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources: Save%>" OnClick="btnSave_Click" />
        </td>
        <td>
        </td>
    </tr>
</table>