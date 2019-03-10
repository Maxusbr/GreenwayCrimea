<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.YaBuying.Admin_YaMarketBuyingHistory" Codebehind="YaMarketBuyingHistory.ascx.cs" %>

<div style="padding: 10px 0; font-weight: bold; text-align: left">История закаказов со статусами</div>
<asp:ListView ID="lvHistory" runat="server" ItemPlaceholderID="trPlaceholderID">
    <LayoutTemplate>
        <table class="table-ui">
            <thead>
                <th>№ заказа в Яндекс.Маркете</th>
                <th>№ заказа в магазине</th>
                <th>Статус</th>
                <th>Дата создания</th>
                <th>Сумма</th>
                <th></th>
            </thead>
            <tbody>
                <tr id="trPlaceholderID" runat="server"></tr>
            </tbody>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <%# Eval("MarketOrderId")%>
            </td>
            <td>
                <%# Eval("OrderId") is DBNull ? "Заказ не найден" : Eval("OrderId") %>
            </td>
            <td>
                <pre><%# Eval("Status") %></pre>
            </td>
            <td>
                <%# Eval("OrderDate") %>
            </td>
            <td>
                <%# Eval("Sum") %>
            </td>
            <td>
                <%# Eval("OrderId") is DBNull ? "" : "<a href='/admin/vieworder.aspx?orderId=" + Eval("OrderId") + "'>Посмотреть заказ</a>" %>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
