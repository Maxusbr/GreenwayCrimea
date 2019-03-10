<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Dashboard.BigOrdersChart" Codebehind="BigOrdersChart.ascx.cs" %>
<article class="chart-block">
    <div class="clearfix">
        <h2 class="chart-orders-title">
            <%= Resources.Resource.Admin_Default_Orders %></h2>
        <div class="chart-orders-period">
            <div data-plugin="radiolist" class="radiolist">
                <label>
                    <input type="radio" checked="checked" id="gr-chart0" name="gr-chart" value="#chartWeek" />
                    <%= Resources.Resource.Admin_Charts_Week %></label>
                <label>
                    <input type="radio" id="gr-chart1" name="gr-chart" value="#chartMounth" />
                    <%= Resources.Resource.Admin_Charts_Mounth %></label>
                <label>
                    <input type="radio" id="gr-chart2" name="gr-chart" value="#chartYear" />
                    <%= Resources.Resource.Admin_Charts_Year%></label>
            </div>
        </div>
    </div>
    <div id="chartWeek" style="width: 99%; height: 190px;" data-plugin="chart" data-chart="<%= RenderDataByDays(Now.AddDays(-7)) %>"
        data-chart-options="{xaxis : { mode: 'time', timeformat: '%d %b', min: <%= GetTimestamp(Now.AddDays(-7)) %> , max: <%= GetTimestamp(Now.AddDays(1)) %>} }">
    </div>
    <div id="chartMounth" style="display: none; width: 99%; height: 190px;" data-plugin="chart" data-chart="<%= RenderDataByDays(Now.AddDays(-30)) %>"
        data-chart-options="{xaxis : { mode: 'time', timeformat: '%d %b', min: <%= GetTimestamp(Now.AddDays(-30)) %> , max: <%= GetTimestamp(Now.AddDays(1)) %>}}">
    </div>
    <div id="chartYear" style="display: none; width: 99%; height: 190px;" data-plugin="chart" data-chart="<%= RenderDataByMonths(Now.AddYears(-1)) %>"
        data-chart-options="{xaxis : { mode: 'time', timeformat: '%b %y', min: <%= GetTimestamp(Now.AddYears(-1).AddMonths(-1)) %> ,  max: <%= GetTimestamp(Now.AddMonths(1)) %> }}">
    </div>
</article>