<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.YesCreditControl" Codebehind="YesCredit.ascx.cs" %>
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
            <asp:Localize runat="server" Text='<%$ Resources:Resource, Admin_PaymentMethod_YesCredit_StoreId %>'></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMerchantId" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip='<%$ Resources:Resource, Admin_PaymentMethod_YesCredit_StoreId_Description %>' /><asp:Label
                    runat="server" ID="msgMerchantId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="Минимальная цена"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtMinimumPrice" Width="250" Text="3000" ></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Кнопка &quot;Купить в кредит&quot; будет отображаться у товаров превышающих минимальную цену (желательно > 3000 руб.)" /><asp:Label
                    runat="server" ID="msgMinimumPrice" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="Сумма первого платежа(%)"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtFirstPayment" Width="250" Text="10" ></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="% от стоимости товара составит первый платеж" /><asp:Label
                    runat="server" ID="msgFirstPayment" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr> 
</table>
