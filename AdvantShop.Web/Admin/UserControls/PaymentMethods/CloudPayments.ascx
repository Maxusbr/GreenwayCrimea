<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.CloudPaymentsControl" Codebehind="CloudPayments.ascx.cs" %>
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
            <asp:Localize ID="Localize2" runat="server" Text="Платежный шлюз"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlSite">
                <asp:ListItem Text="cloudpayments.ru" Value="cloudpayments.ru" />
                <asp:ListItem Text="cloudpayments.kz" Value="cloudpayments.kz" />
			</asp:DropDownList>
        </td>
        <td class="columnDescr">
        </td>
    </tr>

    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Идентификатор сайта (publicId)"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPublicId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgPublicId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>


     <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Секертный ключ (API Secret)"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtApiSecret" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgApiSecret" Visible="false" ForeColor="Red"></asp:Label>
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
            <asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>

    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Cистема налогообложения"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlTaxationSystem">
                <asp:ListItem Text="Общая система налогообложения" Value="0" />
                <asp:ListItem Text="Упрощенная система налогообложения (Доход)" Value="1" />
                <asp:ListItem Text="Упрощенная система налогообложения (Доход минус Расход)" Value="2" />
                <asp:ListItem Text="Единый налог на вмененный доход" Value="3" />
                <asp:ListItem Text="Единый сельскохозяйственный налог" Value="4" />
                <asp:ListItem Text="Патентная система налогообложения" Value="5" />
			</asp:DropDownList>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="Label2" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
   
</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="https://cloudpayments.ru/Docs/Connect" target="_blank">Подключение к сервису Cloud Payments</a>
</div>