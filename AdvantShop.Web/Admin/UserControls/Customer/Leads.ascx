<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Leads.ascx.cs" Inherits="AdvantShop.Admin.UserControls.Customer.Leads" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>

<div class="cutomers-table-header">
    <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_Leads_Header %>"></asp:Label>
</div>
<asp:ListView ID="lvLeads" runat="server" ItemPlaceholderID="itemPlaceholderID">
    <EmptyDataTemplate>
        <table class="table-data empty-table">
            <tr><td><%= Resource.Admin_No_Entries %></td></tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table class="table-ui-simple">
            <thead>
                <tr>
                    <th class="table-ui-simple-align-center">
                        <asp:Label runat="server" Text="№"></asp:Label>
                    </th>
                    <th class="table-ui-simple-align-center">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_Leads_Status %>"></asp:Label>
                    </th>
                    <th class="table-ui-simple-align-center">
                        <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_Leads_Name %>"></asp:Label>
                    </th>
                    <th class="table-ui-simple-align-center">
                        <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_Sum %>"></asp:Label>
                    </th>
                    <th class="table-ui-simple-align-center">
                        <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_Leads_CreatedTime %>"></asp:Label>
                    </th>
                    <th class="table-ui-simple-align-center">
                        <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_Manager %>"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr runat="server" id="itemPlaceholderID">
                </tr>
            </tbody>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td class="table-ui-simple-align-center">
                <a href='<%# "EditLead.aspx?id=" + Eval("ID") %>' target="_blank">
                    <asp:Literal ID="lLeadId" runat="server" Text='<%# Bind("ID") %>' />
                </a>
            </td>
            <td class="table-ui-simple-align-center">
                <%# RenderLeadStatus(Eval("LeadStatus").ToString()) %>
            </td>
            <td class="table-ui-simple-align-center">
                <%# Eval("Name") %>
            </td>
            <td class="table-ui-simple-align-center table-ui-simple-bold">
                <%# PriceFormatService.FormatPrice(SQLDataHelper.GetFloat(Eval("Sum")), SQLDataHelper.GetFloat(Eval("CurrencyValue")), SQLDataHelper.GetString(Eval("CurrencySymbol")), SQLDataHelper.GetString(Eval("CurrencyCode")), SQLDataHelper.GetBoolean(Eval("IsCodeBefore"))) %>
            </td>
            <td class="table-ui-simple-align-center">
                <%# AdvantShop.Localization.Culture.ConvertDate(SQLDataHelper.GetDateTime( Eval("CreatedDate"))) %>
            </td>
            <td style="width: 100px;">
                <%# Eval("ManagerName") %>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
