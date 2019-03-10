<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.ShippingMethods.ShippingNovaPoshtaControl" CodeBehind="NovaPoshta.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_APIKey %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtAPIKey" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_APIKey_Description %>" /><asp:Label
                    runat="server" ID="msgAPIKey" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_CityFrom %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCityFrom" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_CityFrom_Description %>" /><asp:Label
                    runat="server" ID="msgCityFrom" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_DeliveryType %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <adv:EnumDataSource runat="server" ID="edsDeliveryType" EnumTypeName="AdvantShop.Shipping.NovaPoshta.enNovaPoshtaDeliveryType"></adv:EnumDataSource>
            <asp:DropDownList runat="server" ID="ddlDeliveryType" Width="250" DataTextField="LocalizedName" DataValueField="Value" DataSourceID="edsDeliveryType">
            </asp:DropDownList>
        </td>
        <td class="columnDescr"></td>
    </tr>
    <%--<tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_EnabledInsurance %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox ID="cbEnabledInsurance" runat="server" ValidationGroup="5" />
        </td>
        <td class="columnDescr">
        </td>
    </tr>--%>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_Rate %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtRate" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="rvRate" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtRate" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image6" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_Rate_Description %>" /><asp:Label
                    runat="server" ID="msgRate" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_Weight %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWeight" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtRate" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image3" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_Weight_Description %>" /><asp:Label
                    runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <%--<tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_CreateCash %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox ID="chbcreateCOD" runat="server" ValidationGroup="5" /><asp:HiddenField
                ID="hfCod" runat="server" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image4" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_NovaPoshta_CreateCash %>" /><asp:Label
                    runat="server" ID="Label2" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>--%>
</table>
