<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.Order.OrdersSearch" Codebehind="OrdersStatuses.ascx.cs" %>
<div class="panel-toggle">
    <h2>
        <%= Resources.Resource.Admin_OrdersSearch_Orders %></h2>
    <asp:SqlDataSource ID="sdsStatuses1" runat="server" 
                       SelectCommand="SELECT OrderStatusID, StatusName, Color, (Select Count(OrderID) From [Order].[Order] Where [OrderStatusID] = OrderStatus.[OrderStatusID]) as OrdersCount FROM [Order].OrderStatus order by SortOrder"
                       OnInit="sdsStatuses_Init" />

    <div class="list-order-status-item">
        <div class="list-order-status-name">
            <a class="list-order-status-lnk" href="ordersearch.aspx"><%= Resources.Resource.Admin_OrdersSearch_AllOrders %></a>
        </div>
        <div class="list-order-status-count">
            <asp:Label ID="lblTotalOrdersCount" runat="server" />
        </div>
    </div>    
    <asp:ListView ID="lvOrderStatuses" runat="server" ItemPlaceholderID="itemPlaceholderID"
        DataSourceID="sdsStatuses1">
        <LayoutTemplate>
            <ul class="list-order-status">
                <li id="itemPlaceholderID" runat="server"></li>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="list-order-status-item">
                <div class="list-order-status-name" style='<%# "border-left-color: #" + Eval("Color") %>' data-status-color="<%# Eval("Color") %>">
                    <a class="list-order-status-lnk" href="ordersearch.aspx?status=<%# Eval("OrderStatusID") %>">
                        <%# Eval("StatusName") %>
                    </a>
                </div>
                <div class="list-order-status-count">
                    <%# Eval("OrdersCount")%>
                </div>
            </li>
        </ItemTemplate>
    </asp:ListView>
</div>
