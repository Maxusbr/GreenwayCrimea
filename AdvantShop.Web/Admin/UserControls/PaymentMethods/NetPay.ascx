<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.NetPayControl" Codebehind="NetPay.ascx.cs" %>
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
            <asp:Localize ID="LocalizeSite" runat="server" Text="Api key"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtApiKey" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageApiKey" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_NetPay_ApiKeyDescription %>" /><asp:Label runat="server" ID="msgApiKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="LocalizePassword" runat="server" Text="Auth signature"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtAuthSign" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageAuthSign" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_NetPay_AuthSignDescription %>" /><asp:Label runat="server" ID="msgAuthSign" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="LocalizeTestMode" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_SandBox %>"></asp:Localize>
        </td>
         <td class="columnVal">
            <asp:CheckBox runat="server" ID="chkTestMode" Checked="False" />
        </td>
          <td class="columnDescr">
            <asp:Image ID="ImageTestMode" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" 
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Sandbox_Description %>" /><asp:Label runat="server" ID="msgTestMode" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
