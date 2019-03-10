<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.Order.OrdersSearchUc" CodeBehind="OrdersSearchUc.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>

<script type="text/javascript">
    function setCurrentStatus() {

    }
</script>
<div class="panel-toggle">
    <h2>
        <%= Resources.Resource.Admin_OrdersSearch_Orders %></h2>
    <asp:HiddenField ID="hfCurrentStatus" runat="server" />
    <asp:SqlDataSource ID="sdsStatuses" runat="server" SelectCommand="SELECT OrderStatusID, StatusName, Color, (Select Count(OrderID) From [Order].[Order] Where [OrderStatusID] = OrderStatus.[OrderStatusID]) as OrdersCount FROM [Order].OrderStatus order by SortOrder"
        OnInit="sdsStatuses_Init"></asp:SqlDataSource>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True" ID="upSearchOrders">
        <ContentTemplate>
            <div class="list-order-status-item">
                <div class="list-order-status-name">
                    <asp:LinkButton ID="lbtnAllOrders" CssClass="list-order-status-lnk" runat="server"
                        OnClick="lbtnAllOrders_Click"><span class=" <% = StatusId == 0 ? "bold selectedAll" : ""%>" ><%= Resources.Resource.Admin_OrdersSearch_AllOrders %></asp:LinkButton>
                </div>
                <div class="list-order-status-count">
                    <asp:Label ID="lblTotalOrdersCount" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <asp:ListView ID="lvOrderStatuses" runat="server" ItemPlaceholderID="itemPlaceholderID"
                DataSourceID="sdsStatuses" OnItemCommand="lvOrderStatuses_OnItemCommand">
                <LayoutTemplate>
                    <ul class="list-order-status">
                        <li id="itemPlaceholderID" runat="server"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="list-order-status-item">
                        <div class="list-order-status-name" style='<%# "border-left-color: #" + Eval("Color") %>' data-status-color="<%# Eval("Color") %>">
                            <asp:LinkButton ID="lb" CssClass="list-order-status-lnk" runat="server" CommandArgument='<%# Eval("OrderStatusID") %>' CommandName="ShowOrdersByStatus">
                     <span class="<%# SQLDataHelper.GetInt(Eval("OrderStatusID")) == StatusId ? "bold" : string.Empty %>" onclick=""> <%# Eval("StatusName") %></span></asp:LinkButton>
                        </div>
                        <div class="list-order-status-count">
                            <%# Eval("OrdersCount")%>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <div style="border-bottom: 1px solid #cbcbcb; margin-bottom: 20px;">
            </div>

            <asp:ListView ID="lvOrders" runat="server" ItemPlaceholderID="itemPlaceHolderId">
                <LayoutTemplate>
                    <ul class="orders-list">
                        <li runat="server" id="itemPlaceHolderId"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="orders-list-row" style='<%# "border-left-color: #" + Eval("Color") + ";" + ( Request["orderid"] == Eval("OrderID").ToString() ? "background-color:rgb(239, 240, 241);": string.Empty) %>'
                        onclick='<%# "window.location=\"" + RenderOrederLink(Convert.ToInt32(Eval("OrderID")), Convert.ToInt32(_paging.CurrentPageIndex)) + "\"" %>'
                        <%# Request["orderid"] == Eval("OrderID").ToString() ?  "data-current-order=\"1\"": string.Empty %>
                        title='<%#  Eval("PaymentDate") == DBNull.Value ? string.Empty : Resources.Resource.Admin_ViewOrder_Paid %>'>
                        <div class='<%# SQLDataHelper.GetDateTime( Eval("PaymentDate")) == DateTime.MinValue ? "orders-list-name orders-list" : "orders-list-name orders-list-ok" %>'
                            <%# Request["orderid"] == Eval("OrderID").ToString() ?  "data-current-order=\"1\"": string.Empty %>>
                            <a href='<%# RenderOrederLink(Convert.ToInt32(Eval("OrderID")), Convert.ToInt32(_paging.CurrentPageIndex)) %>'
                                class="orders-list-lnk">
                                <%#"№ " + Eval("Number") + " - " + Eval("CustomerName") %></a>
                        </div>
                        <div><%#Eval("OrderDate") %></div>
                        <div>
                            <%# PriceFormatService.FormatPrice(SQLDataHelper.GetFloat(Eval("Sum")), SQLDataHelper.GetFloat(Eval("CurrencyValue")), SQLDataHelper.GetString(Eval("CurrencySymbol")), SQLDataHelper.GetString(Eval("CurrencyCode")), SQLDataHelper.GetBoolean(Eval("IsCodeBefore")))%>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="3"
                UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged"
                Width="250" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
