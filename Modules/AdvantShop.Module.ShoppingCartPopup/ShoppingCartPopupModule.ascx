<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.ShoppingCartPopup.Admin_ShoppingCartPopupModule" Codebehind="ShoppingCartPopupModule.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: ShoppingCartPopup_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: ShoppingCartPopup_ShowMode_Title%>"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlShowMode">
                    <asp:ListItem Text="<%$ Resources: ShoppingCartPopup_ShowMode_None %>" Value="none" />
                    <asp:ListItem Text="<%$ Resources: ShoppingCartPopup_ShowMode_Related %>" Value="related" />
                    <asp:ListItem Text="<%$ Resources: ShoppingCartPopup_ShowMode_Alternative %>" Value="alternative" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize2" runat="server" Text='Кнопка "Оформить заказ" ведет на оформление заказа (по умолчанию ведет в корзину)'></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkGoToCheckout"/>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: ShoppingCartPopup_Save%>" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</div>