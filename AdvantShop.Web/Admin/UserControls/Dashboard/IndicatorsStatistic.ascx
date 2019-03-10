<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Dashboard.Admin_UserControls_IndicatorsStatistic" Codebehind="IndicatorsStatistic.ascx.cs" %>
<article>
    <h2>
        <%= Resources.Resource.Admin_Dashboard_Statistics%></h2>
    <div>
        <ul class="statistic-data">
            <li class="statistic-data-row">
                <div class="statistic-data-image">
                    <div class="statistic-image-wrap">
                        <img src="images/new_admin/cliparts/calendar-day.png"/>
                        <div class="statistic-data-datetime">
                            <div class="statistic-data-datetime-day"><%= DateTime.Now.Day %></div> 
                            <div class="statistic-data-datetime-month"><%= DateTime.Now.ToString("MM.yyyy") %></div>
                        </div>
                    </div>
                </div>
                <div class="statistic-data-info">
                    <p class="statistic-data-coefficient">
                        <asp:Label ID="lblSaleToday" runat="server" /> <span class="sum">(<asp:Literal runat="server" ID="lSumToday" />)</span></p>
                    <p class="statistic-data-desrc">
                        <%= Resources.Resource.Admin_Dashboard_OrdersToday%></p>
                </div>
            </li>
            <li class="statistic-data-row">
                <div class="statistic-data-image">
                    <img src="images/new_admin/cliparts/calendar-yesterday.png"/>
                </div>
                <div class="statistic-data-info">
                    <p class="statistic-data-coefficient">
                        <asp:Label ID="lblSaleYesterday" runat="server" /> <span class="sum">(<asp:Literal runat="server" ID="lSumYesterday" />)</span></p>
                    <p class="statistic-data-desrc">
                        <%= Resources.Resource.Admin_Dashboard_OrdersYesterday%></p>
                </div>
            </li>
            <li class="statistic-data-row">
                <div class="statistic-data-image">
                    <img src="images/new_admin/cliparts/calendar-week.png"/>
                </div>
                <div class="statistic-data-info">
                    <p class="statistic-data-coefficient">
                        <asp:Label ID="lblSaleWeek" runat="server" /> <span class="sum">(<asp:Literal runat="server" ID="lSumWeek" />)</span></p>
                    <p class="statistic-data-desrc">
                        <%= Resources.Resource.Admin_Dashboard_OrdersWeek%></p>
                </div>
            </li>
            <li class="statistic-data-row">
                <div class="statistic-data-image">
                    <img src="images/new_admin/cliparts/calendar-month.png"/>
                </div>
                <div class="statistic-data-info">
                    <p class="statistic-data-coefficient">
                        <asp:Label ID="lblSaleMounth" runat="server" /> <span class="sum">(<asp:Literal runat="server" ID="lSumMonth" />)</span></p>
                    <p class="statistic-data-desrc">
                         <%= Resources.Resource.Admin_Dashboard_OrdersMonth%></p>
                </div>
            </li>
            <li class="statistic-data-row">
                <div class="statistic-data-image">
                    <img src="images/new_admin/cliparts/clock.png"/>
                </div>
                <div class="statistic-data-info">
                    <p class="statistic-data-coefficient">
                        <asp:Label ID="lblSale" runat="server" /> <span class="sum">(<asp:Literal runat="server" ID="lSaleSum" />)</span></p>
                    <p class="statistic-data-desrc">
                        <%= Resources.Resource.Admin_Dashboard_OrdersAllTime%></p>
                </div>
            </li>
            <li class="statistic-data-row">
                <div class="statistic-data-image">
                    <img src="images/new_admin/cliparts/box.png"/>
                </div>
                <div class="statistic-data-info">
                    <p class="statistic-data-coefficient">
                        <asp:Label ID="lblTotalProducts" runat="server" /></p>
                    <p class="statistic-data-desrc">
                         <%= Resources.Resource.Admin_Dashboard_TotalProducts%></p>
                </div>
            </li>
            <%--<li class="statistic-data-row">
                <div class="statistic-data-image">
                    <img src="images/new_admin/cliparts/cash-order.png"/>
                </div>
                <div class="statistic-data-info">
                    <p class="statistic-data-coefficient">
                        <asp:Label ID="lblTotalOrders" runat="server"></asp:Label></p>
                    <p class="statistic-data-desrc">
                        <%= Resources.Resource.Admin_Dashboard_TotalOrders%></p>
                </div>
            </li>--%>
        </ul>
    </div>
</article>
