<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CallLog.ascx.cs" Inherits="AdvantShop.Admin.UserControls.Customer.CallLog" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>

<% if (Calls != null && Calls.Any())
   { %>
<table class="table-data full-table activiti">
    <% foreach (var itemgroup in Calls.GroupBy(x => new {x.CallDate.Date}).Select(x => x.Key.Date).OrderByDescending(x => x.Date))
       { %>
    <tr>
        <td class="activiti-date" colspan="5">
            <%= itemgroup.ToString("dd.MM.yyyy") %>
        </td>
    </tr>
    <% foreach (var item in Calls.Where(x => x.CallDate.Date == itemgroup).OrderByDescending(x => x.CallDate))
       { %>
    <tr>
        <td><%= item.CallDate.ToShortTimeString() %></td>
        <td><%= item.SrcNum %></td>
        <td><%= item.DstNum %></td>
        <td style="width: 200px;">
            <% if (item.CallAnswerDate.HasValue)
                { %>
                <a href="javascript:void(0);" data-recordlink-callid='<%= item.Id %>' data-recordlink-type='<%= item.OperatorType.ToString() %>'>Прослушать</a>
            <% } %>
        </td>
        <td class="status">
            <span class="icon-call calltype-<%= item.Type.ToString().ToLower() %>" title="<%= item.Type.Localize() %>"></span>
        </td>
    </tr>
    <% } %>
    <% } %>
</table>
<% }
   else
   { %>
<table class="table-data empty-table">
    <tr>
        <td>Записи о звонках отсутствуют
        </td>
    </tr>
</table>
<% } %>
