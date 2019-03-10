<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.ShippingMethods.ShippingByProductAmountControl" Codebehind="ShippingByProductAmount.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByProductAmount_Amount %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal" style="width: 420px">
            <%=Resources.Resource.Admin_ShippingMethod_ShippingByProductAmount_From %>:<asp:TextBox
                runat="server" ID="txtAmount" Width="60" ValidationGroup="byPrice" />
            &nbsp;&nbsp;
            <%=Resources.Resource.Admin_ShippingMethod_ShippingByProductAmount_ShippingPrice %>:<asp:TextBox
                runat="server" ID="txtShippingPrice" Width="60" ValidationGroup="byPrice" />
            <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByProductAmount_Add %>"
                ValidationGroup="byPrice" />
            <br />
            <br />
            <asp:Repeater runat="server" ID="rRanges" OnItemCommand="rRanges_Delete">
                <ItemTemplate>
                    <%=Resources.Resource.Admin_ShippingMethod_ShippingByProductAmount_From %>:
                    <%# Eval("Amount") + Resources.Resource.Admin_ShippingMethod_ShippingByProductAmount_Unit %>
                    -
                    <%=Resources.Resource.Admin_ShippingMethod_ShippingByProductAmount_ShippingPrice %>:
                    <%# PriceFormatService.FormatPrice((float)Eval("ShippingPrice")) %>
                    <asp:ImageButton runat="server" ImageUrl="../../images/remove.jpg" ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByProductAmount_Delete %>"
                        CommandName="DeleteRange" CommandArgument='<%# Eval("Amount") %>' CausesValidation="false" />
                    <br />
                </ItemTemplate>
            </asp:Repeater>
            <br />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByWeight_PricePerKg_Description %>" /><asp:Label
                    runat="server" ID="msgPricePerKg" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
	<tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingTerm %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtDeliveryTime" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image3" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_ShippingTerm %>" /><asp:Label
                    runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
