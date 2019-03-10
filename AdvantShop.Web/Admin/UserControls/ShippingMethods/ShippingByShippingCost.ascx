<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.ShippingMethods.ShippingByShippingCostControl" Codebehind="ShippingByShippingCost.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px;">
    <tr class="rowPost">
        <td>
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Label ID="lblShippingCost" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByShippingCost_Choose %>" />
        </td>
    </tr>
    <tr>
        <td class="columnName" style="padding:0 0 0 15px">
            <asp:RadioButton ID="RadioButtonByMax" runat="server" GroupName="ShippingCost" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByShippingCost_ByMax %>" Checked="true" />
        </td>
    </tr>
    <tr>
        <td class="columnName" style="padding:0 0 0 15px">
            <asp:RadioButton ID="RadioButtonBySum" runat="server" GroupName="ShippingCost" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByShippingCost_BySum %>" />
        </td>
    </tr>
    <tr>
        <td class="columnName" class="columnName">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByShippingCost_UseAmount %>" />
        </td>
    </tr>
    <tr>
        <td class="columnName"  style="padding:0 0 0 15px">
            <asp:RadioButton ID="RadioButtonUseAmount" runat="server" GroupName="ShippingCostAmount" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByShippingCost_UseAmount_Yes %>" />
        </td>
    </tr>
    <tr>
        <td class="columnName" style="padding:0 0 20px 15px">
            <asp:RadioButton ID="RadioButtonDontUseAmount" runat="server" GroupName="ShippingCostAmount" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByShippingCost_UseAmount_No %>" />
        </td>
    </tr>
</table>