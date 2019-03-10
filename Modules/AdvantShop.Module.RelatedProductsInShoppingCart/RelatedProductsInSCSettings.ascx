<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.RelatedProductsInShoppingCart.Admin_RelatedProductsInSCSettings" Codebehind="RelatedProductsInSCSettings.ascx.cs" %>
<table>
    <tr class="rowsPost">
        <td style="height: 34px;" colspan="2">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <%-- <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblDisplayBuyButton" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_DisplayBuyButton%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbDisplayBuyButton" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblDisplayMoreButton" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_DisplayMoreButton%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbDisplayMoreButton" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblDisplayPreOrderButton" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_DisplayPreOrderButton%>"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ckbDisplayPreOrderButton" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblBuyButtonText" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_BuyButtonText%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtBuyButtonText" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblMoreButtonText" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_MoreButtonText%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtMoreButtonText" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblPreOrderButtonText" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_PreOrderButtonText%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPreOrderButtonText" runat="server"></asp:TextBox>
        </td>
    </tr>--%>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Label ID="lblRelatedType" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_RelatedType%>"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlRelatedType" runat="server">
                <Items>
                    <asp:ListItem Text="<%$ Resources: RelatedProductsInSCSettings_RelatedType_Alternate%>"
                                  Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: RelatedProductsInSCSettings_RelatedType_Relate%>"
                                  Value="1"></asp:ListItem>
                </Items>
            </asp:DropDownList>
        </td>
    </tr>
    <%--<tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblImageMaxWidth" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_ImageMaxWidth%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtImageMaxWidth" runat="server"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtImageMaxWidth"
                Type="Integer" ErrorMessage="*" EnableClientScript="True" MinimumValue="1" MaximumValue="9999999"></asp:RangeValidator>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Label ID="lblImageMaxHeight" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_ImageMaxHeight%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtImageMaxHeight" runat="server"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtImageMaxHeight"
                Type="Integer" ErrorMessage="*" EnableClientScript="True" MinimumValue="1" MaximumValue="9999999"></asp:RangeValidator>
        </td>
    </tr>--%>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_TopHtml%>"></asp:Label>
        </td>
        <td>
             <asp:TextBox ID="txtTopHtml" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" Width="700px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Label ID="Label2" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_BottomHtml%>"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtBottomHtml" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" Width="700px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources: RelatedProductsInSCSettings_Save%>"
                        OnClick="btnSave_Click" />
        </td>
        <td>
        </td>
    </tr>
</table>