<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.EditManagerTask" CodeBehind="ManagerTask.aspx.cs" %>

<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/UserControls/PopupGridManagers.ascx" TagName="PopupGridManagers" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridCustomers.ascx" TagName="PopupGridCustomers" TagPrefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("body").on("click", ".aSelectClientCustomerId", function () {
                ShowModalPopupCustomers();
            });
            $("body").on("click", ".aSelectAssignedManager", function () {
                ShowModalPopupManagers();
            });

            $("body").on("click", ".radiolist input[type='radio']", function () {
                showHidePanels(this.value);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;
        </div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td align="center">
                            <img src="images/ajax-loader.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="color: #0D76B8;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item dropdown-menu-parent">
                <a href="Managers.aspx"><%= Resource.Admin_MasterPageAdmin_Managers%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Managers.aspx">
                            <%= Resource.Admin_MasterPageAdmin_Managers %></a>
                        </li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Departments.aspx">
                            <%= Resource.Admin_MasterPageAdmin_Departments %></a>
                        </li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item selected">
                <a href="ManagersTasks.aspx"><%= Resource.Admin_MasterPageAdmin_ManagersTasks%></a>
            </li>
            <li class="neighbor-menu-item">
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
            <a href="CreateCustomer.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_Add %> <%= Resource.Admin_MasterPageAdmin_Customer %></a>,
            <a href="ManagerTask.aspx" class="panel-add-lnk"><%= Resource.Admin_ManagersTasks_Task %></a>, 
            <a href="EditLead.aspx" class="panel-add-lnk"><%= Resource.Admin_Leads_AddTop %></a>
        </div>
    </div>


    <ul class="two-column">
        <li class="two-column-item"></li>
        <li class="two-column-item">
            <ul class="justify" style="padding: 20px 0 0 0">
                <li class="justify-item">
                    <div class="order-main-number">
                        <asp:Label runat="server" ID="lblTitle" />
                    </div>
                    <div class="select-order-status">
                    </div>
                    <div style="color: red">
                        <asp:Literal ID="lblMessage" runat="server" Visible="False" Mode="PassThrough" />
                    </div>
                    <div style="color: blue">
                        <asp:Literal ID="lblMessageSuccess" runat="server" Visible="False" Mode="PassThrough" />
                    </div>
                </li>
                <li class="justify-item">
                    <asp:LinkButton runat="server" ID="lbCreateOrder" OnClick="lbCreateOrder_Click" Visible="False"
                        Text="<%$ Resources:Resource, Admin_ManagersTasks_CreateOrder %>" CssClass="btn btn-middle btn-action" />

                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" OnClick="btnSave_Click" onmousedown="window.onbeforeunload=null;"
                        Text="<%$ Resources:Resource, Admin_Save %>" />
                </li>
            </ul>

            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                <ContentTemplate>
                    <adv:PopupGridManagers ID="PopupGridManagers" runat="server" />
                    <adv:PopupGridCustomers ID="PopupGridCustomers" runat="server" />
                    <table class="info-tb">
                        <tr class="rowsPost" style='<%= TaskId == 0 ? "display: none;": string.Empty %>'>
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_DateCreated %></span>
                            </td>
                            <td>
                                <asp:Label ID="lblDateCreated" runat="server" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_DueDate %></span> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <span class="dp">
                                    <asp:TextBox ID="txtDueDate" runat="server" Width="100px" /></span>
                                <asp:TextBox ID="txtDueMinutes" runat="server" Width="50px" />

                                <asp:Image ID="popupDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 150px;">
                                <span><%= Resource.Admin_ManagersTasks_Status %></span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlStatus" DataSourceID="edsManagerTaskStatus"
                                    DataTextField="LocalizedName" DataValueField="Value" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_TaskName %></span> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" Width="400px"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagerTask_Description %></span> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <div class="textarea-wrap">
                                    <asp:TextBox ID="txtDescription" runat="server" Width="400px" TextMode="MultiLine" Rows="7"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_AssignedManager %></span> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hfAssignedManagerId" />
                                <asp:HyperLink runat="server" ID="hlAssignedManager" Target="_blank"></asp:HyperLink>
                                <a href="javascript:void(0);" class="editlink aSelectAssignedManager" id="aSelectAssignedManager" runat="server">
                                    <%= Resource.Admin_ManagerTask_Select %>
                                </a>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_AppointedManager %></span>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hlAppointedManager" Target="_blank"></asp:HyperLink>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td colspan="2" style="height: 34px;">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_ManagerTask_Additional%>"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost" id="trOrderId">
                            <td>
                                <span><%= Resource.Admin_ManagerTask_OrderId %></span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderId" runat="server" Width="200px" CssClass="autocompleteOrder" />
                            </td>
                        </tr>
                        <tr class="rowsPost" id="trClient">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_Client %></span>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hfClientCustomerId" />
                                <asp:Label ID="lblClientNotSelected" runat="server" Text="<%$ Resources:Resource, Admin_ManagerTask_ClientNotSelect %>" Visible="False"></asp:Label>
                                <asp:HyperLink runat="server" ID="hlClientCustomer" Target="_blank" Visible="False"></asp:HyperLink>
                                <asp:Literal runat="server" ID="pnlPhone" Mode="PassThrough" Visible="False" />
                                <asp:ImageButton runat="server" ID="ibRemoveClientCustomer" ImageUrl="~/Admin/images/remove.jpg" Visible="False"
                                    Style="margin: 0 2px;" OnClick="ibRemoveClientCustomer_Click" />
                                <a href="javascript:void(0);" class="editlink aSelectClientCustomerId" id="aSelectClientCustomerId" runat="server">
                                    <%= Resource.Admin_ManagerTask_Select %>
                                </a>
                            </td>
                        </tr>
                        <tr id="leadDiv" runat="server" visible="False" class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_Lead %></span>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hlLead" Target="_blank" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_ResultShort %></span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResultShort" runat="server" Width="400px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <span><%= Resource.Admin_ManagersTasks_ResultFull %></span>
                            </td>
                            <td>
                                <div class="textarea-wrap">
                                    <asp:TextBox ID="txtResultFull" runat="server" Width="400px" Rows="7" TextMode="MultiLine" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        </li>
    </ul>

    <adv:EnumDataSource runat="server" ID="edsManagerTaskStatus" EnumTypeName="AdvantShop.Customers.ManagerTaskStatus" />
</asp:Content>
