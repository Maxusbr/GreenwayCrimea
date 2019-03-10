<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.OnPayControl" Codebehind="OnPay.ascx.cs" %>
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
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_OnPay_FormPay %>"></asp:Localize>
            <span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtformpay" Width="250"></asp:TextBox>
            <asp:Label runat="server" ID="lbformpay" Width="250"></asp:Label>
                    <br />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_OnPay_FormPay %>" />
            <asp:Label runat="server" ID="msgformpay" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_OnPay_SecretKey %>"></asp:Localize>
            <span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_OnPay_SecretKey %>" /> <asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
            <asp:Label runat="server" ID="msgSecretKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
     <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$Resources:Resource, Admin_PaymentMethod_OnPay_MethodSend %>"></asp:Localize>
            <span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
      <asp:RadioButton runat="server" ID="rbdSendGet" Text="GET "  GroupName="RadioGroup1"  Width="250" />
          <br />
        <asp:RadioButton runat="server" ID="rbdSendPost" Text="POST " GroupName="RadioGroup1"  Width="250" />
    
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image4" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$Resources:Resource, Admin_PaymentMethod_OnPay_MethodSend %>" />
            <asp:Label runat="server" ID="msgMethodSend" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
  
   <tr>
         <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Robocassa_CurrencyLabel %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyLabel" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="Image5" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Robocassa_CurrencyLabel_Description %>" /><asp:Label runat="server" ID="msgCurrencyLabel" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>   
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyValue" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image3" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_OnPay_Course %>" />
            <asp:Label runat="server" ID="msgCurrencyValue" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>

     <tr>
        <td class="columnName">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MD5 %>"></asp:Localize><span
                class="required"></span>
        </td>
        <td class="columnVal">
        <asp:CheckBox runat="server" ID="chbMd5" Text="<%$ Resources:Resource, Admin_PaymentMethod_nomd5%>" value="no" ForeColor="Blue"/>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image6" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_OnPay_Md5Description %>" />
        </td>
    </tr>

</table>
