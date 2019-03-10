<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Dashboard.Admin_UserControls_GoogleAnalyticStatistic" Codebehind="GoogleAnalyticStatistic.ascx.cs" %>
<article class="chart-block">
    <h2>
        <%= Resources.Resource.Admin_Dashboard_GAStatistic_Attendance %></h2>
    <div style="width: 100%; height: 190px;" data-plugin="chart" data-chart="<%= chartData %>"
        data-chart-options="{xaxis : { 
            mode: 'time', 
            timeformat: '%d %b', 
            min: <%= GetTimestamp(DateTime.Now.AddDays(-7)) %> ,    
            max: <%= GetTimestamp(DateTime.Now) %>,
            decimal: 0
            }
        }">
    </div>
</article>
<ul class="statistic-visitors">
    <li class="statistic-visitors-row">
        <div class="statistic-visitors-item">
            <p class="statistic-data-coefficient">
                <asp:Literal ID="lblViewPagesYesterday" runat="server" Text="N/A" />
            </p>
            <p class="statistic-data-desrc">
                <%= Resources.Resource.Admin_Dashboard_GAStatistic_PagesYesterday %>
            </p>
        </div>
        <div class="statistic-visitors-item">
            <p class="statistic-data-coefficient">
                <asp:Literal ID="lblViewPagesToday" runat="server" Text="N/A" />
            </p>
            <p class="statistic-data-desrc">
                <%= Resources.Resource.Admin_Dashboard_GAStatistic_PagesToday %>
            </p>
        </div>
    </li>
    <li class="statistic-visitors-row">
        <div class="statistic-visitors-item">
            <p class="statistic-data-coefficient">
                <asp:Literal ID="lblVisitsYesterday" runat="server" Text="N/A" />
            </p>
            <p class="statistic-data-desrc">
                <%= Resources.Resource.Admin_Dashboard_GAStatistic_VisitsYesterday %>
            </p>
        </div>
        <div class="statistic-visitors-item">
            <p class="statistic-data-coefficient">
                <asp:Literal ID="lblVisitsToday" runat="server" Text="N/A" />
            </p>
            <p class="statistic-data-desrc">
                <%= Resources.Resource.Admin_Dashboard_GAStatistic_VisitsToday %>
            </p>
        </div>
    </li>
    <li class="statistic-visitors-row">
        <div class="statistic-visitors-item">
            <p class="statistic-data-coefficient">
                <asp:Literal ID="lblVisitorsYesterday" runat="server" Text="N/A" />
            </p>
            <p class="statistic-data-desrc">
                <%= Resources.Resource.Admin_Dashboard_GAStatistic_VisitorsYesterday %>
            </p>
        </div>
        <div class="statistic-visitors-item">
            <p class="statistic-data-coefficient">
                <asp:Literal ID="lblVisitorsToday" runat="server" Text="N/A" />
            </p>
            <p class="statistic-data-desrc">
                <%= Resources.Resource.Admin_Dashboard_GAStatistic_VisitorsToday %>
            </p>
        </div>
    </li>
</ul>
<asp:HyperLink ID="lnkGoSettings" runat="server" NavigateUrl="~/admin/CommonSettings.aspx#tabid=seo"
    Target="_blank" Text='<%$ Resources:Resource,Admin_Dashboard_GAStatistic_GoToSettings %>'
    Visible="False" ForeColor="Red"></asp:HyperLink>
<asp:Label runat="server" ID="lDate" CssClass="statistic-comment"></asp:Label>

