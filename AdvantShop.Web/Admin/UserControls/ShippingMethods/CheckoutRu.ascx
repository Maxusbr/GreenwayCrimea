<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckoutRu.ascx.cs" Inherits="AdvantShop.Admin.UserControls.ShippingMethods.CheckoutRu" %>
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
            <asp:Localize ID="Localize5" runat="server" Text="Группировать по типу доставки"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="ckbGrouping" Width="250" ValidationGroup="5"></asp:CheckBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgGrouping" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="Идентификационный ключ клиента"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtClientId" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Идентификационный ключ клиента
                    </header>
                    <div class="help-content">
                        Идентификационный ключ - параметр, который отвечает за связку магазина и сервиса. <br /><br />
                        Ключ можно найти в личном кабинете Checkout, вкладка "Настройка - выбрать ваш магазин - Интеграция"
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgClientId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
  <%--  <tr>
        <td class="columnName">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_TypeExtraCharge %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <adv:EnumDataSource runat="server" ID="edsTypes" EnumTypeName="AdvantShop.Payment.ExtrachargeType" />
            <asp:DropDownList runat="server" ID="ddlExtrachargeType" DataSourceID="edsTypes" DataTextField="LocalizedName" DataValueField="Value" />
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_ExtraCharge %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtExtracharge" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
        </td>
    </tr>--%>
</table>
<asp:Label runat="server" ID="lError" ForeColor="Red" Visible="False" Style="display: block; margin-bottom: 10px;" />