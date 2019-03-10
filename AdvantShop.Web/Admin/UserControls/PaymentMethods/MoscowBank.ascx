<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.MoscowBankControl" Codebehind="MoscowBank.ascx.cs" %>
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
            <asp:Localize ID="LocalizeMerchant" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Merchant %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchant" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageMerchant" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Merchant_Description %>" /><asp:Label runat="server" ID="msgMerchant" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="LocalizeTerminal" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Terminal %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtTerminal" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageTerminal" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Terminal_Description %>" /><asp:Label runat="server" ID="msgTerminal" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="LocalizeMerchName" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_MerchName %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchName" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageMerchName" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_MerchName_Description %>" /><asp:Label runat="server" ID="msgMerchName" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="LocalizeEmail" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Email %>"></asp:Localize>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtEmail" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageEmail" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Email_Description %>" /><asp:Label runat="server" ID="msgEmail" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="LocalizeKey" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Key %>"></asp:Localize>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtKey" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageKey" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_Key_Description %>" /><asp:Label runat="server" ID="msgKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="LocalizeCurrencyLabel" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_CurrencyLabel %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyLabel" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageCurrencyLabel" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_MoscowBank_CurrencyLabel_Description %>" /><asp:Label runat="server" ID="msgCurrencyLabel" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyValue" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="Image4" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue_Description %>" /><asp:Label runat="server" ID="msgCurrencyValue" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
