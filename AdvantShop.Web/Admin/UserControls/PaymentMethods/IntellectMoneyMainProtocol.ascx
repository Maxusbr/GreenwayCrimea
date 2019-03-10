﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntellectMoneyMainProtocol.ascx.cs" Inherits="Admin.UserControls.PaymentMethods.IntellectMoneyMainProtocolControl" %>
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
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_EshopId %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtEshopId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_EshopId_Description %>" /><asp:Label runat="server" ID="msgEshopId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_Preference %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlPaymentSystem" DataTextField="Value" DataValueField="Key" Width="254" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_Preference_Description %>" /><asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>


    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="Секретный ключ (Secret Key)"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSecretKey" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image  runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="Строка символов, используемая для подписи данных передаваемых системой ИнтеллектМани магазину. Эта строка используется для повышения надежности идентификации высылаемого оповещения. Содержание строки известно только системе IntellectMoney и интернет-магазину!" /><asp:Label runat="server" ID="Label2" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>