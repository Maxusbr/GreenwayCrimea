<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.RobokassaControl" Codebehind="Robokassa.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Robocassa_MerchantLogin %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantLogin" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_PaymentMethod_Robocassa_MerchantLogin%>
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_PaymentMethod_Robocassa_MerchantLogin_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgMerchantLogin" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Robocassa_Password %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_PaymentMethod_Robocassa_Password%>
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_PaymentMethod_Robocassa_Password_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Robocassa_PasswordNotify %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPasswordNotify" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_PaymentMethod_Robocassa_PasswordNotify%>
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_PaymentMethod_Robocassa_PasswordNotify_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgPasswordNotify" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Передавать данные для чека"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="cbSendReceiptData"/>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/connect-robokassa" target="_blank"><%= Resources.Resource.Admin_PaymentMethod_Robocassa_Instruction%></a>
</div>