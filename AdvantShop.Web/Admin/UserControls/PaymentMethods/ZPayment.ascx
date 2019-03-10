<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.ZPaymentControl" Codebehind="ZPayment.ascx.cs" %>
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
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_ZPayment_Purse %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPurse" Width="250"></asp:TextBox>
        </td>
       <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_ZPayment_Purse_Description %>" /><asp:Label runat="server" ID="msgPurse" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_ZPayment_Password %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_ZPayment_Password_Description %>" /><asp:Label runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_ZPayment_SecretKey %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_ZPayment_SecretKey_Description %>" /><asp:Label runat="server" ID="msgSecretKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
