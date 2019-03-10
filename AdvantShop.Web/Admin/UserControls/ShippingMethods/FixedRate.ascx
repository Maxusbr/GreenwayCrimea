<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.ShippingMethods.FixedRateControl" Codebehind="FixedRate.ascx.cs" %>
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
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_FixeRateShipping_ShippingPrice %>"></asp:Localize><span class="required">&nbsp;*</span>
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
            <asp:Label runat="server" ID="msgShippingPrice" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <%--<tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_FixeRateShipping_Extracharge %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtExtracharge" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Наценка на метод доставки
                    </header>
                    <div class="help-content">
                        Тут вы указываете наценку на метод доставки, если она есть. Цена указывается в базовой валюте.<br>
                        <br>
                        Например: 100 (руб.)<br />
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgExtracharge" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>--%>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingTerm %>"></asp:Localize><span class="required">&nbsp;*</span>
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
                        Например: 24 часа, или 1-2 дня.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
