<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.EditLead" CodeBehind="EditLead.aspx.cs" %>

<%@ Register Src="~/Admin/UserControls/Order/OrderItems.ascx" TagName="OrderItems" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridCustomers.ascx" TagName="PopupGridCustomers" TagPrefix="adv" %>

<%@ Register TagPrefix="adv" TagName="LeadPanel" Src="~/admin/UserControls/Leads/LeadPanel.ascx" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item dropdown-menu-parent">
                <a href="Managers.aspx"><%= Resource.Admin_MasterPageAdmin_Managers%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Managers.aspx">
                            <%= Resource.Admin_MasterPageAdmin_Managers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Departments.aspx">
                            <%= Resource.Admin_MasterPageAdmin_Departments %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item">
                <a href="ManagersTasks.aspx"><%= Resource.Admin_MasterPageAdmin_ManagersTasks%></a>
            </li>
            <li class="neighbor-menu-item selected">
                <a href="Leads.aspx"><%= Resource.Admin_MasterPageAdmin_Leads%></a>
            </li>
            <li class="neighbor-menu-item">
                <a href="Calls.aspx"><%= Resource.Admin_MasterPageAdmin_Calls%></a>
            </li>
            <li class="neighbor-menu-item">
                <a href="Calls.aspx?Type=Missed"><%= Resource.Admin_MasterPageAdmin_MissedCalls%></a>
            </li>
        </menu>
        <div class="panel-add">
            <a href="ManagerTask.aspx" class="panel-add-lnk"><%= Resource.Admin_ManagersTasks_AddTask %></a>, <a href="EditLead.aspx" class="panel-add-lnk"><%= Resource.Admin_Leads_AddTop %></a>
        </div>
    </div>
    <ul class="two-column">
        <li class="two-column-item">
            <adv:LeadPanel ID="LeadPanel" runat="server" />
        </li>
        <li class="two-column-item" style="padding: 0 20px 20px 20px;">
            <ul class="justify order-dashboard-row" style="position: relative">
                <li class="justify-item">
                    <asp:Panel ID="pnlLeadInfo" runat="server" CssClass="order-main" Visible="False">

                        <div class="order-main-number">
                            <%= Resource.Admin_Leads_ItemNum %>
                            <asp:Label ID="lblOrderId" runat="server" />
                        </div>
                        <div style="padding: 5px 0 0 0;">
                            <%=Resource.Admin_Leads_CreatedTime %>:
                            <asp:Label ID="lblOrderDate" runat="server" />
                        </div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>

                        <div id="createOrderBlock" runat="server" style="position: absolute; right: 230px; top: 0;" visible="false">
                            <asp:LinkButton runat="server" ID="lbCreateOrder" CssClass="btn btn-middle btn-add" Text="<%$ Resources: Resource, Admin_Leads_CreateOrder %>"
                                OnClick="lbCreateOrder_Click" />
                        </div>

                        <div id="createTaskBlock" runat="server" style="position: absolute; right: 0; top: 0;" visible="false">
                            <a class="btn btn-middle btn-add create-new-task" href="javascript:void(0)"><%=Resource.Admin_Leads_InWork %></a>
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        <%=Resource.Admin_Leads_InWorkHintTitle %>
                                    </header>
                                    <div class="help-content">
                                        <%=Resource.Admin_Leads_InWorkHintText %>
                                    </div>
                                </article>
                            </div>

                            <asp:Panel runat="server" ID="pnlTasks" Visible="False">
                                <div class="lead-tasks-title"><%=Resource.Admin_Leads_Tasks %>:</div>
                                <div>
                                    <asp:ListView runat="server" ID="lvTasks">
                                        <ItemTemplate>
                                            <div class="lead-tasks-item">
                                                <a target="_blank" href="ManagerTask.aspx?TaskId=<%#Eval("TaskId")%>"><%#Eval("Name")%></a>
                                            </div>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlNewLead" runat="server" CssClass="order-main" Visible="False">
                        <div class="order-main-number">
                            <%=Resource.Admin_Leads_AddNewLead %>
                        </div>
                    </asp:Panel>
                </li>
            </ul>



            <ul class="list-order-data" style="padding: 0 0 15px 0">
                <%--<li class="list-order-data-row">
                    <div class="list-order-data-name">
                        <%= Resource.Admin_Leads_Status%>:
                    </div>
                    <div class="list-order-data-value">
                        <asp:DropDownList runat="server" ID="ddlLeadStatus" CssClass="lead-status" />
                    </div>
                </li>--%>
                <li id="blockManagers" runat="server" class="list-order-data-row">
                    <div class="list-order-data-name">
                        <%= Resource.Admin_Leads_Manager%>:
                    </div>
                    <div class="list-order-data-value">
                        <asp:DropDownList ID="ddlManager" runat="server" CssClass="customer-manager" Width="300" />
                    </div>
                </li>
                <li class="list-order-data-row">
                    <div class="list-order-data-name">
                        <%= Resource.Admin_Leads_OrderType%>:
                    </div>
                    <div class="list-order-data-value">
                        <asp:DropDownList runat="server" ID="ddlOrderType" DataTextField="Name" DataValueField="Id" />
                    </div>
                </li>
            </ul>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="PopupGridCustomers" />
                </Triggers>
                <ContentTemplate>

                    <div class="order-buyer-name">
                        <div class="list-order-data-caption">
                            <%= Resource.Admin_ViewOrder_Customer %> 
                            (<a class="editlink" style="color: #017DC1 !important" href="javascript:void(0)"><%= Resource.Admin_OrderSearch_ChooseUser%></a>)
                            <asp:HyperLink runat="server" ID="hlCustomer" Target="_blank" />
                        </div>
                        <ul class="list-order-data">
                            <li class="list-order-data-row">
                                <div class="list-order-data-name">
                                    <%= Resource.Admin_ViewOrder_CustomerName%>:
                                </div>
                                <div class="list-order-data-value">
                                    <asp:TextBox runat="server" ID="txtCustomerName" Width="300px" CssClass="customer-name-field" />
                                </div>
                            </li>
                            <li class="list-order-data-row">
                                <div class="list-order-data-name">
                                    <%= Resource.Admin_ViewOrder_Email %>:
                                </div>
                                <div class="list-order-data-value">
                                    <asp:TextBox runat="server" ID="txtCustomerEmail" Width="300px" CssClass="customer-email-field" />
                                </div>
                            </li>
                            <li class="list-order-data-row">
                                <div class="list-order-data-name">
                                    <%= Resource.Admin_ViewOrder_Phone %>:
                                </div>
                                <div class="list-order-data-value">
                                    <asp:TextBox runat="server" ID="txtPhone" Width="300px" CssClass="customer-phone-field" />
                                </div>
                            </li>
                        </ul>
                    </div>
                    <input type="hidden" id="hfCustomerId" runat="server" class="customer-id" />

                </ContentTemplate>
            </asp:UpdatePanel>


            <div class="order-table-wrap">

                <div class="list-order-data-caption">
                    <%=Resource.Admin_Leads_Products %>
                </div>

                <asp:UpdatePanel ID="upItems" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="orderItems" EventName="ItemsUpdated" />
                    </Triggers>
                    <ContentTemplate>
                        <adv:OrderItems runat="server" ID="orderItems" OnItemsUpdated="orderItems_Updated" IsLead="True" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <ul class="justify order-comment-wrap">
                <li class="justify-item order-comment-item-wrap">
                    <div class="order-comment-title">
                        <%= Resource.Admin_Leads_UserComment %>:
                    </div>
                    <div class="textarea-wrap order-comment-text">
                        <asp:TextBox runat="server" ID="txtUserComment" TextMode="MultiLine" />
                    </div>
                </li>
                <li class="justify-item order-comment-item-wrap">
                    <div class="order-comment-title">
                        <%= Resource.Admin_Leads_AdminComment %>
                    </div>
                    <div class="textarea-wrap order-comment-text">
                        <asp:TextBox runat="server" ID="txtAdminComment" TextMode="MultiLine" />
                    </div>
                </li>
            </ul>

            <asp:Button runat="server" ID="btnAddOrSaveLead" OnClick="btnAddOrSaveLead_Click" CssClass="btn btn-middle btn-add" />

        </li>
    </ul>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            SearchProduct();
        });

        $(document).ready(function () {
            $(".editlink").live("click", function () {
                ShowModalPopupCustomers();
            });
        });
    </script>

    <%--popup for choose customer--%>
    <adv:PopupGridCustomers ID="PopupGridCustomers" runat="server" />

    <div class="new-task-block" style="display: none">
        <div class="new-task-header"><%=Resource.Admin_Leads_InWorkTitle %></div>
        <ul class="list-order-data">
            <li class="task-customer-taskname-field list-order-data-row">
                <div class="list-order-data-name"><%=Resource.Admin_Leads_Task %>: <span class="valid-star">*</span></div>
                <div class="list-order-data-value">
                    <asp:TextBox runat="server" ID="txtTaskName" Width="300px" />
                </div>
            </li>
            <li class="task-customer-fields task-customer-name-field list-order-data-row">
                <div class="list-order-data-name"><%= Resource.Admin_ViewOrder_CustomerName%>:</div>
                <div class="list-order-data-value">
                    <asp:TextBox runat="server" ID="txtTaskCustomerName" Width="300px" />
                </div>
            </li>
            <li class="task-customer-fields task-customer-email-field list-order-data-row">
                <div class="list-order-data-name"><%= Resource.Admin_ViewOrder_Email%>: </div>
                <div class="list-order-data-value">
                    <asp:TextBox runat="server" ID="txtTaskEmail" Width="300px" CssClass="valid-email group-newtask" />
                </div>
            </li>
            <li class="task-customer-fields task-customer-phone-field list-order-data-row">
                <div class="list-order-data-name"><%= Resource.Admin_ViewOrder_Phone%>:</div>
                <div class="list-order-data-value">
                    <asp:TextBox runat="server" ID="txtTaskPhone" Width="300px" />
                </div>
            </li>
        </ul>
        <div class="task-error"></div>

        <input type="hidden" runat="server" id="hfTaskCustomerId" class="task-customer-id" />
        <input type="hidden" runat="server" id="hfTaskLeadId" class="task-leadid" />

        <div class="task-existing-customer">
            <ul class="list-order-data">
                <li class="list-order-data-row">
                    <div class="list-order-data-name"><%=Resource.Admin_Leads_Client %>:</div>
                    <div class="list-order-data-value">
                        <div class="task-existing-customer-info"></div>
                        <div class="task-existing-customer-other"><a href="javascript:void(0)"><%=Resource.Admin_Leads_OtherClient %></a></div>
                    </div>
                </li>
            </ul>
        </div>

        <div class="task-customers-search">
            <div class="task-customers-search-title"><%=Resource.Admin_Leads_FindRelatedCustomers %>:</div>
            <div class="task-customers-search-results"></div>
        </div>

        <ul class="list-order-data">
            <li class="list-order-data-row">
                <div class="list-order-data-name"><%=Resource.Admin_Leads_Manager %> <span class="valid-star">*</span></div>
                <div class="list-order-data-value">
                    <asp:DropDownList runat="server" ID="ddlTaskManager" CssClass="task-customer-manager" />
                </div>
            </li>
        </ul>
    </div>

</asp:Content>
