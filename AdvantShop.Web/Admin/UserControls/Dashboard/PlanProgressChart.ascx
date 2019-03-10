<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Dashboard.PlanProgressChart" Codebehind="PlanProgressChart.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="Resources" %>
<article class="chart-block">
    <h2>
        <%= Resource.Admin_Charts_ProgressChart %>
    </h2>
    <figure>
        <div class="chart-progress-bar">
            <div class="chart-progress-complete" style="width:<%= planPercent %>%"></div>
            <div class="chart-progress-value">
                <%= planPercent %>% (<%= PriceFormatService.FormatPrice(sales) %>)
            </div>
        </div>
        <figcaption class="chart-progress-legend">
            <span class="chart-progress-legend-complete">
                <%= Resource.Admin_Charts_ProgressDone %></span><span class="chart-progress-legend-left">
                    <%=Resource.Admin_Charts_ProgressPlan%></span>
        </figcaption>
    </figure>
</article>