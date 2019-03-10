<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.ShippingMethods.EdostControl" CodeBehind="Edost.ascx.cs" %>
<%@ Import Namespace="System.Globalization" %>
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
            <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_ShopId %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtShopId" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_ShopId_Description %>" /><asp:Label
                    runat="server" ID="msgShopId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Password %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250" ValidationGroup="5"></asp:TextBox>
            <asp:Literal runat="server" ID="lPassword" Text="******" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Password_Description %>" /><asp:Label
                    runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Rate %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtRate" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <%-- <asp:CustomValidator ID="cvValidateRate" runat="server" EnableClientScript="True"
                Display="Static" ControlToValidate="txtRate" ClientValidationFunction="validateFloat" ErrorMessage=""></asp:CustomValidator>--%>
            <asp:RangeValidator ID="rvRate" Type="Double" MinimumValue="0,001" MaximumValue="1000000" Text="Значение должно быть от 0,001 до 1 000 000"
                runat="server" ControlToValidate="txtRate" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image6" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Rate_Description %>" /><asp:Label
                    runat="server" ID="msgRate" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreateCash %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox ID="chbcreateCOD" runat="server" ValidationGroup="5" /><asp:HiddenField
                ID="hfCod" runat="server" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image4" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreateCash %>" /><asp:Label
                    runat="server" ID="Label2" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreatePickPoint %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox ID="chbcreatePickPoint" runat="server" ValidationGroup="5" /><asp:HiddenField
                ID="hfPickPoint" runat="server" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image5" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreatePickPoint %>" /><asp:Label
                    runat="server" ID="Label3" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 10px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Weight %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWeight" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" Type="Double" MinimumValue="0,001" MaximumValue="1000000"
                runat="server" ControlToValidate="txtWeight" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Вес товара
                    </header>
                    <div class="help-content">
                        Вес товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в кг, возможно указать дробное значение.<br />
                        <br />
                        Например: 1 или 0.2 (кг.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Length %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtLength" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator2" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtLength" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Длина товара
                    </header>
                    <div class="help-content">
                        Длина товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в мм.<br />
                        <br />
                        Например: 120 (мм.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label4" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Width %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWidth" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator3" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtWidth" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Ширина товара
                    </header>
                    <div class="help-content">
                        Ширина товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в мм.<br />
                        <br />
                        Например: 120 (мм.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label5" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Height %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtHeight" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator4" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtHeight" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Высота товара
                    </header>
                    <div class="help-content">
                        Высота товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в мм.<br />
                        <br />
                        Например: 120 (мм.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label6" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image123" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/install-edost" target="_blank">Инструкция. Подключение модуля доставки eDost</a>
</div>