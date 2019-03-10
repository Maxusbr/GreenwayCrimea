<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivitiLog.ascx.cs" Inherits="AdvantShop.Admin.UserControls.Customer.ActivitiLog" %>
<%@ Import Namespace="Resources" %>

<% if (ActivityEvents != null && ActivityEvents.Any())
   { %>

<table class="table-data full-table activiti">

    <% foreach (var itemgroup in ActivityEvents.GroupBy(x => new {x.CreateOn.Date}).Select(x => x.Key.Date).OrderByDescending(x => x.Date))
       { %>
    <tr>
        <td class="activiti-date">
            <%= itemgroup.ToString("dd.MM.yyyy") %>
        </td>
    </tr>
    <tr class="activiti-info">
        <td class="activiti-info-cell">
            <table>
                <tr>
                    <th class="activiti-header">
                        <%= Resource.Admin_Activity_Event %>
                    </th>
                    <th class="activiti-header">
                        <%= Resource.Admin_Activity_Date %>
                    </th>
                </tr>
                <% foreach (var item in ActivityEvents.Where(x => x.CreateOn.Date == itemgroup).OrderByDescending(x => x.CreateOn))
                   { %>
                <tr>
                    <td>
                        <span><% = RenderEventType(item.EvenType) %></span> <%= RenderEventLink(item) %>
                    </td>
                    <td>
                        <%= RenderTime(item.CreateOn) %>
                    </td>
                </tr>
                <% } %>
            </table>
        </td>
    </tr>
    <% } %>
</table>

<% }
   else
   { %>

<table class="table-data empty-table">
    <tr>
        <td>
            <%= Resource.Admin_No_Entries %>
        </td>
    </tr>
</table>
<% } %>

        
