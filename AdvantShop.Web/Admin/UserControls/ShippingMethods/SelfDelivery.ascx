<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.ShippingMethods.SelfDeliveryControl" Codebehind="SelfDelivery.ascx.cs" %>
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
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_FixeRateShipping_ShippingPrice %>"></asp:Localize> 
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtShippingPrice" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_ShippingMethod_FixeRateShipping_ShippingPrice_Description%>
                    </header>
                    <div class="help-content">
                        Стомость доставки. Цена указывается в базовой валюте магазина, только числом.<br />
                        <br />
                        Например: 100 (руб.)<br />
                        <br />
                        Если самовывоз в вашем случае бесплатный, укажите стоимость 0.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingTerm %>"></asp:Localize> 
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtDeliveryTime" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingTerm%>
                    </header>
                    <div class="help-content">
                        Срок доставки, как отрезок времени, в который данный метод доставки сможет доставить товар.<br />
                        <br />
                        Например: 1-2 дня.
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
