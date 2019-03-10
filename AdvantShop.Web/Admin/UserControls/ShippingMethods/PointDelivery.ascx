<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.ShippingMethods.PointDeliveryControl" Codebehind="PointDelivery.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
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
            <asp:Localize ID="Localize1" runat="server" Text="Пункты самовывоза"></asp:Localize>
            <span class="required">*</span>
        </td>
        <td class="columnVal" style="width: 320px">
            <asp:TextBox runat="server" ID="txtNewPoint" Width="250" ValidationGroup="byPrice" placeholder="Адрес" style="margin-bottom:10px;"/>
            <%--<asp:TextBox runat="server" ID="txtNewPointPrice" Width="200" ValidationGroup="byPrice" placeholder="Цена" style="margin-bottom:10px;"/>
            <asp:TextBox runat="server" ID="txtNewPointTimeSpan" Width="200" ValidationGroup="byPrice" placeholder="Срок доставки" style="margin-bottom:10px;"/>--%>
            <br />
            <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByOrderPrice_Add %>"
                ValidationGroup="byPrice" />
            <br />
            <br />
            <asp:Repeater runat="server" ID="rPoints" OnItemCommand="rPoints_Delete">
                <ItemTemplate>
                    <%# Container.DataItem %>
                    <%--<%# Eval("Address") %>
                    <%# Eval("TimeSpan") %>
                    <%# Eval("Rate") %>--%>
                    <asp:ImageButton runat="server" ImageUrl="../../images/remove.jpg" ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByOrderPrice_Delete %>"
                        CommandName="DeletePoints" CommandArgument='<%# Container.DataItem %>' CausesValidation="false" />
                    <%--<asp:ImageButton runat="server" ImageUrl="../../images/remove.jpg" ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByOrderPrice_Delete %>"
                        CommandName="DeletePoints" CommandArgument='<%# Eval("Address") %>' CausesValidation="false" />--%>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
            <br />
        </td>
        <td class="columnDescr">
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
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingTerm %>"></asp:Localize> 
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
