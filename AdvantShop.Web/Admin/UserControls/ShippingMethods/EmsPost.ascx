<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.ShippingMethods.EmsPostControl" Codebehind="EmsPost.ascx.cs" %>
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
            <asp:Localize ID="Localize2" runat="server" Text="Город РФ, из которого осуществляется доставка"></asp:Localize>
            <span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlShippingCityFrom" Width="250"></asp:DropDownList>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Город РФ, из которого осуществляется доставка. Проверьте город на сайте emspost.ru" /><asp:Label
                    runat="server" ID="msgShippingCityFrom" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="Наценка к цене доставки"></asp:Localize>
            <span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtExtraPrice" Width="250" ValidationGroup="5" Text="0"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Наценка к цене доставки" /><asp:Label runat="server" ID="msgExtraPrice" Visible="false"
                    ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="Вес заказа по умолчанию"></asp:Localize>
            <span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtShippingWeight" Width="250" ValidationGroup="5" Text="1"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image3" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Вес заказа по умолчанию" /><asp:Label runat="server" ID="msgShippingWeight" Visible="false"
                    ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="Максимальный вес для доставки"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:Label runat="server" ID="lblMaxWeight" />
        </td>
        <td class="columnDescr">
        </td>
    </tr>
</table>
