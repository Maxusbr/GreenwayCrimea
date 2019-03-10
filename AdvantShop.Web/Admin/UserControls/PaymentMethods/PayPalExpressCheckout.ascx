<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.PaymentMethods.PayPalExpressCheckoutControl" Codebehind="PayPalExpressCheckout.ascx.cs" %>
<%@ Import Namespace="System.ComponentModel" %>
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
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_PayPalExpressCheckout_User %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtUserId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_PayPalExpressCheckout_User_Description %>" /><asp:Label
                    runat="server" ID="msgUserId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_PayPalExpressCheckout_Password %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_PayPalExpressCheckout_Password_Description %>" /><asp:Label
                    runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_PayPalExpressCheckout_Signature %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSignature" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image7" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_PayPalExpressCheckout_Signature_Description %>" /><asp:Label
                    runat="server" ID="msgSignature" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyCode %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:DropDownList ID="ddlCurrencyCode" runat="server" Width="250">
            </asp:DropDownList>
        </td>
        <td class="columnDescr">
           
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyValue" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image4" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue_Description %>" /><asp:Label
                    runat="server" ID="msgCurrencyValue" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Sandbox %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="chkSandbox" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image6" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Sandbox_Description %>" /><asp:Label
                    runat="server" ID="msgSandbox" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
