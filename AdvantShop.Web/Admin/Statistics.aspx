<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Statistics" CodeBehind="Statistics.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;
        </div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #0D76B8; text-align: center;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Discount_PriceRange.aspx">
                <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
            <li class="neighbor-menu-item"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resource.Admin_MasterPageAdmin_Voting %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resource.Admin_MasterPageAdmin_VotingHistory %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item selected"><a href="Statistics.aspx">
                <%= Resource.Admin_Statistics_Header %></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">

        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width: 72px;">
                    <img src="images/orders_ico.gif" alt="" />
                </td>
                <td>
                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Statistics_Header %>"></asp:Label><br />
                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Statistics_SubHeader %>"></asp:Label>
                </td>
            </tr>
        </table>

        <div style="padding: 15px 0;">
            <table class="info-tb AllNice">
                <tr>
                    <td><%= Resource.Admin_Statistics_DateFrom %></td>
                    <td></td>
                    <td><%= Resource.Admin_Statistics_DateTo %></td>

                    <td class="statistic-order-option"><%= Resource.Admin_Statistics_Paid %></td>
                    <td class="statistic-order-option"><%= Resource.Admin_Statistics_Status %></td>
                </tr>
                <tr>
                    <td>
                        <div class="dp">
                            <asp:TextBox runat="server" ID="txtDateFrom" CssClass="datefrom" /></div>
                    </td>
                    <td style="padding: 3px 7px 3px 6px;">-</td>
                    <td style="padding-right: 15px;">
                        <div class="dp">
                            <asp:TextBox runat="server" ID="txtDateTo" CssClass="dateto" /></div>
                    </td>
                    <td class="statistic-order-option" style="padding-right: 15px;">
                        <asp:DropDownList runat="server" ID="ddlPayed" CssClass="paied">
                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" Selected="True" />
                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="yes" />
                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="no" />
                        </asp:DropDownList>
                    </td>
                    <td class="statistic-order-option" style="padding-right: 15px;">
                        <asp:DropDownList runat="server" ID="ddlStatuses" CssClass="statuses" DataTextField="StatusName" DataValueField="StatusID" />
                    </td>
                </tr>
            </table>
        </div>

        <ul id="tabs" class="two-column">
            <li class="two-column-item">
                <ul id="tabs-headers">
                    <li id="sales">Воронка продаж</li>
                    <li id="profit">Доход</li>
                    <% if (ShowCrm)
                       { %>
                    <li id="leads">Лиды</li>
                    <% } %>
                    <li id="check">Средний чек</li>
                    <li id="payments">Заказы</li>
                    <li id="goods">Товары</li>
                    <li id="buyers">Покупатели</li>
                    <% if (ShowCrm)
                       { %>
                    <li id="telephony">Телефония</li>
                    <li id="managers">Менеджеры</li>
                    <% } %>
                </ul>
            </li>
            <li class="two-column-item">

                <div id="tabs-contents">

                    <div class="tab-content">
                        <div id="vortex"></div>
                        <% if (!SettingsSEO.GoogleAnalyticsApiEnabled)
                           { %>
                        <div style="padding: 10px 0; color: red;">
                            Необходимо настроить <a href="CommonSettings.aspx#tabid=counters" target="_blank">Google Analytics Api</a>
                        </div>
                        <% } %>
                    </div>
                    <div class="tab-content">
                        <div class="statistics-sales statistics-tab-content">
                            <div>
                                <label>Учитывать стоимость доставки
                                    <input type="checkbox" class="useshippings" checked="checked" /></label>
                            </div>

                            <div class="statistics-orderby-head clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Default_Orders %></h2>
                                <div class="chart-orders-period">
                                    <div data-plugin="radiolist" class="radiolist">
                                        <label>
                                            <input type="radio" id="groupbyDay" value="dd" checked="checked" name="groupOrders" /><%= Resource.Admin_Statistics_Day %>
                                        </label>
                                        <label>
                                            <input type="radio" id="groupbyWeek" value="wk" name="groupOrders" /><%= Resource.Admin_Statistics_Week %>
                                        </label>
                                        <label>
                                            <input type="radio" id="groupbyMounth" value="mm" name="groupOrders" /><%= Resource.Admin_Statistics_Month %>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <article class="chart-block">
                                <div id="orderGraph"
                                    data-plugin="chart"
                                    data-statistics="sales"
                                    data-chart-url="httphandlers/statistic/salesgraph.ashx?type=sum"
                                    class="statistics-chart">
                                </div>
                            </article>

                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_OrdersByCount %></h2>
                            </div>
                            <article class="chart-block">
                                <div id="orderCountGraph"
                                    data-plugin="chart"
                                    data-statistics="sales"
                                    data-chart-url="httphandlers/statistic/salesgraph.ashx?type=count"
                                    class="statistics-chart">
                                </div>
                            </article>
                        </div>
                    </div>
                    <% if (ShowCrm)
                       { %>
                    <div class="tab-content">
                        <div class="statistics-leads statistics-tab-content">
                            <div class="statistics-orderby-head clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_LeadsByCount %></h2>
                                <div class="chart-orders-period">
                                    <div data-plugin="radiolist" class="radiolist">
                                        <label>
                                            <input type="radio" value="dd" checked="checked" name="groupLeadsBy" /><%= Resource.Admin_Statistics_Day %>
                                        </label>
                                        <label>
                                            <input type="radio" value="wk" name="groupLeadsBy" /><%= Resource.Admin_Statistics_Week %>
                                        </label>
                                        <label>
                                            <input type="radio" value="mm" name="groupLeadsBy" /><%= Resource.Admin_Statistics_Month %>
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <article class="chart-block">
                                <div id="leadsCountGraph"
                                    data-plugin="chart"
                                    data-statistics="leads"
                                    data-chart-url="httphandlers/statistic/leadsgraph.ashx?type=count"
                                    class="statistics-chart">
                                </div>
                            </article>
                        </div>

                        <div class="clearfix">
                            <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_LeadsByStatus %></h2>
                        </div>

                        <article class="chart-block">
                            <div id="leadsStatusGraph"
                                data-plugin="d3graphHorizontal"
                                data-statistics="leads"
                                data-chart-url="httphandlers/statistic/leadsgraph.ashx?type=status"
                                class="statistics-chart-d3">
                                <svg xmlns="http://www.w3.org/2000/svg"></svg>
                            </div>
                        </article>
                    </div>
                    <% } %>

                    <div class="tab-content">
                        <div class="statistics-avgcheck statistics-tab-content">
                            <div class="statistics-orderby-head clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_AvgCheck %></h2>

                                <div class="avg-value"></div>
                                <div class="chart-orders-period">
                                    <div data-plugin="radiolist" class="radiolist">
                                        <label>
                                            <input type="radio" value="dd" checked="checked" name="groupAvgCheckBy" /><%= Resource.Admin_Statistics_Day %>
                                        </label>
                                        <label>
                                            <input type="radio" value="wk" name="groupAvgCheckBy" /><%= Resource.Admin_Statistics_Week %>
                                        </label>
                                        <label>
                                            <input type="radio" value="mm" name="groupAvgCheckBy" /><%= Resource.Admin_Statistics_Month %>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <article class="chart-block">
                                <div id="avgCheckGraph"
                                    data-plugin="chart"
                                    data-statistics="avgcheck"
                                    data-chart-url="httphandlers/statistic/avgcheckgraph.ashx?type=avg"
                                    class="statistics-chart">
                                </div>
                            </article>


                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_AvgCheckByCity %></h2>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="d3graphHorizontal"
                                    data-statistics="avgcheck"
                                    data-chart-url="httphandlers/statistic/avgcheckgraph.ashx?type=city"
                                    class="statistics-chart-d3">
                                    <svg xmlns="http://www.w3.org/2000/svg"></svg>
                                </div>
                            </article>
                        </div>
                    </div>

                    <div class="tab-content">
                        <div class="statistics-ordersby statistics-tab-content">
                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_Payments %></h2>
                            </div>

                            <article class="chart-block">
                                <div
                                    data-plugin="d3graphHorizontal"
                                    data-statistics="ordersby"
                                    data-chart-url="httphandlers/statistic/ordersbygraph.ashx?type=payment"
                                    class="statistics-chart-d3">
                                    <svg xmlns="http://www.w3.org/2000/svg"></svg>
                                </div>
                            </article>


                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_Shippings %></h2>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="d3graphHorizontal"
                                    data-statistics="ordersby"
                                    data-chart-url="httphandlers/statistic/ordersbygraph.ashx?type=shipping"
                                    class="statistics-chart-d3">
                                    <svg xmlns="http://www.w3.org/2000/svg"></svg>
                                </div>
                            </article>

                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_OrderStatuses %></h2>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="d3graphHorizontal"
                                    data-statistics="ordersby"
                                    data-chart-url="httphandlers/statistic/ordersbygraph.ashx?type=status"
                                    class="statistics-chart-d3">
                                    <svg xmlns="http://www.w3.org/2000/svg"></svg>
                                </div>
                            </article>

                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_OrderTypes %></h2>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="d3graphHorizontal"
                                    data-statistics="ordersby"
                                    data-chart-url="httphandlers/statistic/ordersbygraph.ashx?type=ordertype"
                                    class="statistics-chart-d3">
                                    <svg xmlns="http://www.w3.org/2000/svg"></svg>
                                </div>
                            </article>

                            <div class="statistics-orderby-head clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_RepeatOrders %></h2>
                                <div class="chart-orders-period">
                                    <div data-plugin="radiolist" class="radiolist">
                                        <label>
                                            <input type="radio" value="dd" checked="checked" name="groupOrdersBy" /><%= Resource.Admin_Statistics_Day %>
                                        </label>
                                        <label>
                                            <input type="radio" value="wk" name="groupOrdersBy" /><%= Resource.Admin_Statistics_Week %>
                                        </label>
                                        <label>
                                            <input type="radio" value="mm" name="groupOrdersBy" /><%= Resource.Admin_Statistics_Month %>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <article class="chart-block">
                                <div id="ordersByRepeatOrdersGraph"
                                    data-plugin="chart"
                                    data-statistics="ordersby"
                                    data-chart-url="httphandlers/statistic/ordersbygraph.ashx?type=repeatorders"
                                    class="statistics-chart">
                                </div>
                            </article>
                        </div>
                    </div>

                    <div class="tab-content">
                        <div class="clearfix">
                            <h2 class="chart-orders-title">ABC/XYZ анализ</h2>
                        </div>
                        <div id="abcxyz"></div>
                        <div class="abc-hint"></div>
                        <div class="abc-hint-msg">
                            <div data-hint-type="ax">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_AX %>" /></div>
                            <div data-hint-type="ay">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_AY %>" /></div>
                            <div data-hint-type="az">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_AZ %>" /></div>

                            <div data-hint-type="bx">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_BX %>" /></div>
                            <div data-hint-type="by">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_BY %>" /></div>
                            <div data-hint-type="bz">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_BZ %>" /></div>

                            <div data-hint-type="cx">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_CX %>" /></div>
                            <div data-hint-type="cy">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_CY %>" /></div>
                            <div data-hint-type="cz">
                                <asp:Literal runat="server" Mode="PassThrough" Text="<%$ Resources: Resource, StatisticsFilter_AbcXyzAnalysis_CZ %>" /></div>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div class="tab-content">
                        <div id="rfm"></div>
                    </div>

                    <% if (ShowCrm)
                       { %>
                    <div class="tab-content">
                        <div class="statistics-telephony statistics-tab-content">

                            <div class="statistics-orderby-head clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_InCalls %></h2>
                                <div class="chart-orders-period">
                                    <div data-plugin="radiolist" class="radiolist">
                                        <label>
                                            <input type="radio" value="dd" checked="checked" name="groupTelephony" /><%= Resource.Admin_Statistics_Day %>
                                        </label>
                                        <label>
                                            <input type="radio" value="wk" name="groupTelephony" /><%= Resource.Admin_Statistics_Week %>
                                        </label>
                                        <label>
                                            <input type="radio" value="mm" name="groupTelephony" /><%= Resource.Admin_Statistics_Month %>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="chart"
                                    data-statistics="telephony"
                                    data-chart-url="httphandlers/statistic/telephonygraph.ashx?type=in"
                                    class="statistics-chart">
                                </div>
                            </article>
                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_MissedCallls %></h2>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="chart"
                                    data-statistics="telephony"
                                    data-chart-url="httphandlers/statistic/telephonygraph.ashx?type=missed"
                                    class="statistics-chart">
                                </div>
                            </article>
                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_OutCalls %></h2>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="chart"
                                    data-statistics="telephony"
                                    data-chart-url="httphandlers/statistic/telephonygraph.ashx?type=out"
                                    class="statistics-chart">
                                </div>
                            </article>
                            <div class="clearfix">
                                <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_AvgDurationgCalls %></h2>
                            </div>
                            <article class="chart-block">
                                <div
                                    data-plugin="chart"
                                    data-statistics="telephony"
                                    data-chart-url="httphandlers/statistic/telephonygraph.ashx?type=avgtime"
                                    class="statistics-chart">
                                </div>
                            </article>
                        </div>
                    </div>

                    <div class="tab-content">
                        <div class="managers"></div>
                    </div>
                    <% } %>
                </div>

            </li>
        </ul>

    </div>
</asp:Content>
