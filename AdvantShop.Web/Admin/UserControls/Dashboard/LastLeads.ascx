<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Dashboard.LastLeads" CodeBehind="LastLeads.ascx.cs" %>
<div class="table-ui-wrap">
    <h2 class="header-title"><%= Resources.Resource.Admin_Leads_MyLeads %></h2>
    <a href="Leads.aspx?manager=<%= ManagerId %>"><%= Resources.Resource.Admin_Leads_MyLeadsAll %></a>
    <div class="clear"></div>
    <div>
        <asp:ListView ID="lvLastLeads" runat="server" ItemPlaceholderID="itemPlaceholderID" OnItemCommand="lvLastLeads_ItemCommand">
            <LayoutTemplate>
                <table class="table-ui table-last-orders">
                    <thead>
                        <tr>
                            <th class="table-ui-align-left">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_Leads_Status %>'></asp:Label>
                            </th>
                            <th class="table-ui-align-left">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_Leads_Name %>'></asp:Label>
                            </th>
                            <th class="table-ui-align-left">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_OrderSearch_Phone %>'></asp:Label>
                            </th>
                            <th class="table-ui-cost">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_Leads_CreatedTime %>'></asp:Label>
                            </th>
                            <th class="table-ui-row-btn">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_Dashboard_OrderCommands %>'></asp:Label>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholderID" runat="server">
                        </tr>
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr data-redirect='<%# "EditLead.aspx?Id=" + Eval("Id") %>'>
                    <td>
                        <%# GetLeadStatus(Eval("LeadStatus").ToString()) %>
                    </td>
                    <td>
                        <%# Eval("FirstName") %> <%# Eval("LastName") %>
                    </td>
                    <td>
                        <%# Eval("Phone") %>
                    </td>
                    <td class="table-ui-align-center">
                        <%# AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("CreatedDate")) %>
                    </td>
                    <td class="table-ui-align-center">
                        <a href='<%# "EditLead.aspx?Id=" + Eval("Id") %>'>
                            <img src="images/editbtn.gif" alt="" />
                        </a>
                        <asp:LinkButton runat="server" ID="buttonDelete" runat="server" CommandName="DeleteLead" CommandArgument='<%# Eval("Id") %>'
                            CssClass="showtooltip lnk-del-order" ToolTip='<%$ Resources:Resource, Admin_OrderSearch_Delete%>'
                            data-message='<%# Resources.Resource.Admin_ConfirmDeleting %>'>
                            <img src="images/deletebtn.png" alt="" />
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                    <%=Resources.Resource.Admin_Catalog_NoRecords %>
                </div>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
</div>
