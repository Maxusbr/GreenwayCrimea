<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.MonexyControl" Codebehind="Monexy.ascx.cs" %>

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
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoneyXy_MerchantId %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantId" Width="250"></asp:TextBox>
            <asp:Label runat="server" ID="msgMerchantId" Visible="false"></asp:Label>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoneyXy_ShopName %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtShopName" Width="250"></asp:TextBox>
            <asp:Label runat="server" ID="msgShopName" Visible="false"></asp:Label>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoneyXy_Currency %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlCurrency">
                <asp:ListItem Text="UAH" Value="UAH" />
                <asp:ListItem Text="RUR" Value="RUR" />
                <asp:ListItem Text="USD" Value="USD" />
                <asp:ListItem Text="EUR" Value="EUR" />
            </asp:DropDownList>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <%--<tr>
         <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
             <asp:TextBox runat="server" ID="txtCurrencyValue" Width="250" Text="1" />
        </td>
          <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue_Description %>" /><asp:Label runat="server" ID="msgCurrencyValue" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>--%>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoneyXy_IsCheckHash %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="chkIsCheckHash"/>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MoneyXy_SecretKey %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
</table>