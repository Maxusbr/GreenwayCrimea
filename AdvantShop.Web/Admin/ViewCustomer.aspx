<%@ Page AutoEventWireup="true" Inherits="Admin.ViewCustomer" Language="C#"
    MasterPageFile="~/Admin/MasterPageAdmin.master" CodeBehind="ViewCustomer.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.Services.Loging.Emails" %>
<%@ Import Namespace="AdvantShop.Core.Services.Loging.Smses" %>
<%@ Import Namespace="Resources" %>

<%@ Register Src="~/Admin/UserControls/Customer/FindCustomers.ascx" TagName="FindCustomers" TagPrefix="adv" %>
<%--<%@ Register Src="~/Admin/UserControls/Customer/CustomerEmailCall.ascx" TagName="CustomerEmailCall" TagPrefix="adv" %>--%>
<%@ Register Src="~/Admin/UserControls/Customer/ActivitiLog.ascx" TagName="ActivitiLog" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Customer/CallLog.ascx" TagName="CallLog" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Customer/Leads.ascx" TagName="Leads" TagPrefix="adv" %>

<asp:Content ID="ContentViewCustomer" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="CustomerSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Buyers%></a></li>
            <li class="neighbor-menu-item"><a href="CustomersGroups.aspx">
                <%= Resource.Admin_MasterPageAdmin_CustomersGroups%></a></li>
            <li class="neighbor-menu-item"><a href="Subscription.aspx">
                <%= Resource.Admin_MasterPageAdmin_SubscribeList%></a></li>
            <li class="neighbor-menu-item"><a href="Managers.aspx">
                <%= Resource.Admin_MasterPageAdmin_Managers%></a></li>
            <li class="neighbor-menu-item"><a href="Departments.aspx">
                <%= Resource.Admin_MasterPageAdmin_Departments%></a></li>
            <li class="neighbor-menu-item"><a href="ManagersTasks.aspx">
                <%= Resource.Admin_MasterPageAdmin_ManagersTasks%></a></li>
        </menu>
        <div class="panel-add">
            <a href="CreateCustomer.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_Add %> <%= Resource.Admin_MasterPageAdmin_Customer %></a>
        </div>
    </div>

    <ul class="two-column">
        <li class="two-column-item">
            <adv:FindCustomers ID="FindCustomers" runat="server" />
        </li>
        <li class="two-column-item">
            <% if (CurrentCustomer.RegistredUser)
               { %>
            <ul class="justify order-dashboard-row">
                <li class="justify-item">
                    <div class="order-main">
                        <div class="order-main-number">
                            <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                        </div>
                        <%= Resource.Admin_ViewCustomer_RegistrationDate %>
                        <asp:Label ID="lblRegistrationDate" runat="server"></asp:Label>
                        <div class="rating">
                            <div id="rating_<%= CustomerId %>">
                            </div>
                            <input type="hidden" value="<%= Rating %>" id="rating_hidden_<%= CustomerId %>" />
                        </div>

                    </div>
                </li>
                <li class="justify-item">
                    <a href="editcustomer.aspx?customerid= <%= CustomerId %>" class="btn btn-middle btn-action" id="btnEditCustomer"><%= Resource.Admin_Edit %></a>
                    <a href="editorder.aspx?OrderID=addnew&customerid=<%= CustomerId %>" class="btn btn-middle btn-action"><% = Resource.Admin_ViewCustomer_AddOrder %></a>
                    <a href="editlead.aspx?customerid=<%= CustomerId %>" class="btn btn-middle btn-action"><% = Resource.Admin_ViewCustomer_AddLid %></a>
                </li>
            </ul>

            <ul class="order-list-address">
                <li class="order-list-address-item">
                    <div class="list-order-data-caption">
                        <%= Resource.Admin_ViewCustomer_CustomerInfo %>
                    </div>

                    <ul class="list-order-data">
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                E-mail
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerEmail" runat="server"></asp:Label>
                            </div>
                        </li>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_Phone %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Literal ID="lblCustomerPhone" runat="server"></asp:Literal>
                            </div>
                        </li>
                        <% if (CurrentCustomer.Contacts.Any())
                           { %>
                        <% if (!string.IsNullOrEmpty(CurrentCustomer.Contacts[0].Country))
                           { %>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_ContactCountry %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerContactCountry" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(CurrentCustomer.Contacts[0].Region))
                           { %>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_ContactZone %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerContactZone" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(CurrentCustomer.Contacts[0].City))
                           { %>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_ContactCity %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerContactCity" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(CurrentCustomer.Contacts[0].Street))
                           { %>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_ContactAddress %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerContactAddress" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(CurrentCustomer.Contacts[0].Zip))
                           { %>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_ContactZip %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerContactZip" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                        <% } %>
                    </ul>
                </li>
                <li class="order-list-address-item">
                    <div id="blockManagers" runat="server" class="list-order-data-caption" visible="false">
                        <%= Resource.Admin_ViewCustomer_Manager %>
                    </div>
                    <div>
                        <asp:DropDownList ID="ddlViewCustomerManager" runat="server" CssClass="ddlViewCustomerManager" Visible="False" />
                        <asp:Label runat="server" ID="lblCustomerManager"></asp:Label>
                    </div>
                    <br />
                    <div class="list-order-data-caption">
                        <%= Resource.Admin_ViewCustomer_CustomerGroup %>
                    </div>
                    <div>
                        <asp:DropDownList ID="ddlCustomerGroup" runat="server" CssClass="ddlCustomerGroup"></asp:DropDownList>
                    </div>
                    <br />
                    <div class="list-order-data-caption viewcustomer__newsSubscribe">
                        <span><%= Resource.Admin_ViewCustomer_Subscribed4News %></span>
                        <input type="checkbox" id="ckbSubscribed4News" runat="server" class="ckbSubscribed4News" />
                    </div>
                    <br />
                    <% if (!string.IsNullOrEmpty(BonusCardNumber) || !string.IsNullOrEmpty(BonusCardAmount) || !string.IsNullOrEmpty(BonusCardGrade))
                       { %>
                    <div class="list-order-data-caption">
                        <%= Resource.Admin_ViewCustomer_BonusCard %>
                    </div>
                    <% } %>
                    <ul class="list-order-data">
                        <% if (!string.IsNullOrEmpty(BonusCardNumber))
                           { %>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_BonusCardNumber %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblBonusCardNumber" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(BonusCardAmount))
                           { %>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_Grade %>
                            </div>
                            <div class="list-order-data-value">

                                <asp:Label ID="lblBonusCardGrade" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(BonusCardGrade))
                           { %>
                        <li id="liBonusAmount" runat="server" class="list-order-data-row" i>
                            <div class="list-order-data-name">
                                <%= Resource.Admin_ViewCustomer_BonusCardAmount %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblBonusCardAmount" runat="server"></asp:Label>
                            </div>
                        </li>
                        <% } %>
                    </ul>

                </li>
                <li class="order-list-address-item" id="indicatorsBlock" runat="server">
                    <div class="list-order-data-caption">
                        <%= Resource.Admin_ViewCustomer_BonusIndicators %>
                    </div>
                    <ul class="list-order-data">
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_Last_Activity_Date %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblLastActiveDate" runat="server"></asp:Label>
                            </div>
                        </li>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_Statuses %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerStatuses" runat="server"></asp:Label>
                            </div>
                        </li>
                        <li class="list-order-data-row">
                            <div class="list-order-data-name">
                                <%= Resource.Admin_Segment %>
                            </div>
                            <div class="list-order-data-value">
                                <asp:Label ID="lblCustomerSegment" runat="server"></asp:Label>
                            </div>
                        </li>
                    </ul>

                </li>

            </ul>
            <div>
                <strong>
                    <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_AdminComment %>"></asp:Label></strong>
                <div>
                    <asp:TextBox ID="txtAdminComment" runat="server" CssClass="niceTextArea textArea3Lines editableTextBoxInViewCustomer" TextMode="MultiLine" data-field-type="comment"></asp:TextBox>
                </div>
            </div>
            <div style="margin-bottom: 20px;">
                <asp:ListView ID="lvCustomerOrders" runat="server" ItemPlaceholderID="itemPlaceholderID">
                    <EmptyDataTemplate>
                        <div class="cutomers-table-header">
                            <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_History %>"></asp:Label>
                        </div>
                        <table class="table-data empty-table">
                            <tr>
                                <td>
                                    <%= Resource.Admin_No_Entries %>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table class="table-ui-simple">
                            <div class="cutomers-table-header">
                                <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_History %>"></asp:Label>
                            </div>
                            <thead>
                                <tr>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label runat="server" Text="№ Заказа"></asp:Label>
                                    </th>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label ID="Label2" runat="server" Text="Статус"></asp:Label>
                                    </th>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label runat="server" Text="Оплачен"></asp:Label>
                                    </th>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label runat="server" Text="Оплата"></asp:Label>
                                    </th>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label runat="server" Text="Доставка"></asp:Label>
                                    </th>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label runat="server" Text="Сумма"></asp:Label>
                                    </th>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label runat="server" Text="Дата"></asp:Label>
                                    </th>
                                    <th class="table-ui-simple-align-center">
                                        <asp:Label runat="server" Text="Менеджер"></asp:Label>
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
                                <a href="ViewOrder.aspx?orderid=<%# Eval("OrderId") %>"><%# Eval("OrderNumber") %></a>
                            </td>
                            <td class="table-ui-simple-align-center">
                                <%# Eval("Status") %>
                            </td>
                            <td class="table-ui-simple-align-center">
                                <%# Convert.ToBoolean(Eval("Payed")) ? Resource.Admin_Yes : Resource.Admin_No %>
                            </td>
                            <td class="table-ui-simple-align-center ">
                                <%# Eval("ArchivedPaymentName") %>
                            </td>
                            <td class="table-ui-simple-align-center ">
                                <%# Eval("ShippingMethodName") %>
                            </td>
                            <td class="table-ui-simple-align-center table-ui-simple-bold">
                                <%# PriceFormatService.FormatPrice(Convert.ToSingle(Eval("Sum")))%>
                            </td>
                            <td class="table-ui-simple-align-center">
                                <%# Eval("OrderDate") %>
                            </td>
                            <td style="width: 100px;">
                                <a href="<%# "ordersearch.aspx?manager=" + Eval("ManagerId") %>"><%# Eval("ManagerName") %></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="3"
                    UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged"
                    Width="250" />
            </div>
            <% }
               else
               { %>
            <ul class="customer-unregistred">
                <li class="order-list-address-item">
                    <div class="list-order-data-caption">
                        <%= Resource.Admin_ViewCustomer_CustomerInfo %>
                    </div>
                    <div>
                        <%= Resource.Admin_ViewCustomer_UnRegistered %>
                    </div>
                    <div>
                        <%= Resource.Admin_ViewCustomer_UserId %>: <%= Code %>
                    </div>
                </li>
                <li style="display: inline-block; float: right;">
                    <div style="margin: 10px 0;">
                        <a href="createcustomer.aspx?customerid=<%= CustomerId %>" class="btn btn-middle btn-action"><% = Resource.Admin_ViewCustomer_AddCustomer %></a>
                        <a href="editorder.aspx?OrderID=addnew&customerid=<%= CustomerId %>" class="btn btn-middle btn-action"><% = Resource.Admin_ViewCustomer_AddOrder %></a>
                        <a href="editlead.aspx?customerid=<%= CustomerId %>" class="btn btn-middle btn-action"><% = Resource.Admin_ViewCustomer_AddLid %></a>
                    </div>
                </li>
            </ul>
            <% } %>

            <adv:Leads ID="CustomerLeads" runat="server" />

            <asp:Panel ID="pnlCustomerActivity" runat="server">
                <table class="customer-activity-table">
                    <tr>
                        <td style="width: 49%; padding-right: 1%; vertical-align: top;">
                            <div class="cutomers-table-header">
                                <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ActiveLog %>"></asp:Label>
                            </div>
                            <adv:ActivitiLog runat="server" ID="ActivitiLog" />
                        </td>
                        <td style="vertical-align: top;">
                            <div class="cutomers-table-header">
                                <asp:Label runat="server">E-mail отправленные сайтом</asp:Label>
                                <%--<asp:Label runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_EmailCallLog %>"></asp:Label>--%>
                            </div>
                            <asp:ListView runat="server" ID="lvEmailLog" ItemPlaceholderID="trItem">
                                <EmptyDataTemplate>
                                    <table class="table-data empty-table">
                                        <tr>
                                            <td>Записи о Email отсутствуют
                                            <%--<%= Resource.Admin_No_Entries_Email_Rings %>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <table class="table-data full-table">
                                        <tr runat="server" id="trItem" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="createOn"><%# Eval("CreateOn") %></td>
                                        <td class="emailAddress"><%# Eval("EmailAddress") %></td>
                                        <td class="subject"><%# Eval("Subject") %></td>
                                        <td class="status"><%# Eval("Status").ToString().TryParseEnum<EmailStatus>().Localize() %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>

                            <div class="cutomers-table-header">
                                <asp:Label runat="server">Звонки</asp:Label>
                            </div>
                            <adv:CallLog ID="CallLog" runat="server" />

                            <div class="cutomers-table-header">SMS</div>
                            <asp:ListView runat="server" ID="lvSms" ItemPlaceholderID="trItem">
                                <EmptyDataTemplate>
                                    <table class="table-data empty-table">
                                        <tr>
                                            <td>
                                                <%= Resource.Admin_No_Entries_SMS %>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <table class="table-data full-table">
                                        <tr runat="server" id="trItem" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="createOn"><%# Eval("CreateOn") %></td>
                                        <td class="emailAddress"><%# Eval("Phone") %></td>
                                        <td class="subject"><%# Eval("Body") %></td>
                                        <td class="status"><%# Eval("Status").ToString().TryParseEnum<SmsStatus>().Localize() %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label runat="server" ID="LogingNotAvailable" Visible="false" Style="text-align: center; display: block; margin: 20px" />
        </li>
    </ul>

    <div style="text-align: center;">
        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False" />
    </div>


    <br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            &nbsp; &nbsp;
            <asp:Image ID="imgUpdating" runat="server" AlternateText="<%$ Resources:Resource, Admin_Loading %>" ImageUrl="images/loading.gif" />
            <asp:Label ID="lblUpdating" runat="server" Text="<%$ Resources:Resource, Admin_Loading %>" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">

        function toggleRightPanel() {
            if ($.cookie("isVisibleRightPanel") == "true") {
                $("div:.rightPanel").hide("fast");
                $("div:.right_hide_rus").hide("fast");
                $("div:.right_show_rus").show("fast");
                $("div:.right_hide_en").hide("fast");
                $("div:.right_show_en").show("fast");
                $.cookie("isVisibleRightPanel", "false", { expires: 7 });
            } else {
                $("div:.rightPanel").show("fast");
                $("div:.right_show_rus").hide("fast");
                $("div:.right_hide_rus").show("fast");
                $("div:.right_show_en").hide("fast");
                $("div:.right_hide_en").show("fast");
                $.cookie("isVisibleRightPanel", "true", { expires: 7 });
            }
        }
    </script>
</asp:Content>
