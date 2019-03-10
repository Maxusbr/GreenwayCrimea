<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Dashboard.LastTasks" CodeBehind="LastTasks.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Customers" %>
<div class="table-ui-wrap">
    <h2 class="header-title"><%= Resources.Resource.Admin_ManagersTasks_MyTasks %></h2>
    <a href="ManagersTasks.aspx?manager=<%= ManagerId%>"><%= Resources.Resource.Admin_ManagersTasks_MyTasksAll%></a>
    <div class="clear"></div>
    <div>
        <asp:ListView ID="lvLastTasks" runat="server" ItemPlaceholderID="itemPlaceholderID" OnItemCommand="lvLastTasks_ItemCommand">
            <LayoutTemplate>
                <table class="table-ui table-last-orders">
                    <thead>
                        <tr>
                            <th class="table-ui-align-left">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_ManagersTasks_Status %>'></asp:Label>
                            </th>
                            <th class="table-ui-align-left">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_ManagersTasks_TaskName %>'></asp:Label>
                            </th>
                            <th class="table-ui-align-left">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_ManagersTasks_AppointedManager %>'></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_ManagersTasks_OrderNumber %>'></asp:Label>
                            </th>
                            <th class="table-ui-cost">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_ManagersTasks_DueDate %>'></asp:Label>
                            </th>
                            <th class="table-ui-cost">
                                <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_ManagersTasks_DateCreated %>'></asp:Label>
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
                <tr data-redirect='<%# "ManagerTask.aspx?TaskId=" + Eval("TaskId") %>'>
                    <td>
                        <%# ((ManagerTaskStatus)Eval("Status")).Localize() %>
                    </td>
                    <td>
                        <%# Eval("Name") %>
                    </td>
                    <td>
                        <a href='<%#"ViewCustomer.aspx?CustomerID=" + Eval("AppointedManager.CustomerId")%>'>
                            <asp:Label ID="lblAppointedName" runat="server" Text='<%# Eval("AppointedManager.FirstName") + " " + Eval("AppointedManager.LastName") %>'></asp:Label></a>
                    </td>
                    <td class="table-ui-align-center">
                        <a href='<%# UrlService.GetAdminAbsoluteLink("ViewOrder.aspx?OrderId=" + Eval("OrderId")) %>' runat='server' Visible='<%# Eval("OrderId") != DBNull.Value %>'>
                            <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("OrderId") %>'></asp:Label>
                        </a>
                    </td>
                    <td>
                        <%# AdvantShop.Localization.Culture.ConvertShortDate((DateTime)Eval("DueDate")) %>
                    </td>
                    <td class="table-ui-align-center">
                        <%# AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("DateCreated")) %>
                    </td>
                    <td class="table-ui-align-center">
                        <a href='<%# "ManagerTask.aspx?TaskId=" + Eval("TaskId") %>'>
                            <img src="images/editbtn.gif" alt="" /></a>
                        <asp:LinkButton runat="server" ID="buttonDelete" runat="server" CommandName="DeleteTask" CommandArgument='<%# Eval("TaskId") %>'
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
