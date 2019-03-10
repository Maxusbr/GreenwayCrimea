<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Dashboard.SearchStatistic" Codebehind="SearchStatistic.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<article class="statistic-search-block">
    <h2>
       <%= Resources.Resource.Admin_Dashboard_SearchTerms %></h2>
    <asp:ListView runat="server" ID="lvSearchStatistic" ItemPlaceholderID="itemPlaceHolderId">
        <LayoutTemplate>
            <ul class="statistic-search-request">
                <li id="itemPlaceHolderId" runat="server"></li>
                <li><a href="searchstatistic.aspx"><asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_Dashboard_AllSearchTerms %>" ></asp:Literal></a></li>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="statistic-search-request-row"><a class="statistic-search-lnk" href='<%# UrlService.GetAbsoluteLink(HttpUtility.HtmlEncode(Convert.ToString(Eval("Request"))) + "&ignorelog=1") %>'
                target="_blank">
                <%# HttpUtility.HtmlEncode(Eval("SearchTerm"))%></a> <span>
                    <%# " ("+ Eval("ResultCount") +")"%></span> </li>
        </ItemTemplate>
        <EmptyDataTemplate>
            <%= Resources.Resource.Admin_Dashboard_NoSearchTerms %>
        </EmptyDataTemplate>
    </asp:ListView>
</article>