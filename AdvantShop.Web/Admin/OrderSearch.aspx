<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.OrderSearch" EnableEventValidation="false" CodeBehind="OrderSearch.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="OrderSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
            <li class="neighbor-menu-item"><a href="OrderStatuses.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderStatuses%></a></li>
            <li class="neighbor-menu-item"><a href="OrderSources.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderSources %></a></li>
            <li class="neighbor-menu-item"><a href="OrderByRequest.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderByRequest%></a></li>
            <li class="neighbor-menu-item"><a href="StatisticsOrdersExportCsv.aspx">
                <%= Resource.Admin_Statistics_ExportOrdersHeader%></a></li>
        </menu>
        <div class="panel-add">
            <a href="EditOrder.aspx?OrderID=addnew" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Add %>
                <%= Resource.Admin_MasterPageAdmin_Order %></a>
        </div>
    </div>
    <div class="content-own">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="inprogress" style="display: none;">
                    <div id="curtain" class="opacitybackground">
                        &nbsp;
                    </div>
                    <div class="loader">
                        <table width="100%" style="font-weight: bold; text-align: center;">
                            <tbody>
                                <tr>
                                    <td style="text-align: center;">
                                        <img src="images/ajax-loader.gif" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; color: #0D76B8;">
                                        <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div style="text-align: center;">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                            <tr>
                                <td style="width: 72px;">
                                    <img src="images/orders_ico.gif" alt="" />
                                </td>
                                <td>
                                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_Header %>"></asp:Label><br />
                                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_SubHeader %>"></asp:Label>
                                </td>
                                <td class="btns-main">
                                    <asp:Button CssClass="btn btn-middle btn-add" runat="server" ID="btnAddOrder" OnClientClick="location='EditOrder.aspx?OrderID=addnew'; return false;"
                                        Text="<%$ Resources: Resource, Admin_OrderSearch_AddOrder %>" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div style="height: 10px;">
                </div>
                <div>
                    <div id="gridTable" runat="server" style="text-align: center;">
                        <table style="width: 99%;" class="massaction">
                            <tr>
                                <td>
                                    <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                        <asp:Localize ID="Localize_Admin_Catalog_Command" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                                    </span><span style="display: inline-block">
                                        <select id="commandSelect" onchange="ChangeSelect()">
                                            <option value="selectAll">
                                                <asp:Localize ID="Localize_Admin_Catalog_SelectAll" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                            </option>
                                            <option value="unselectAll">
                                                <asp:Localize ID="Localize_Admin_Catalog_UnselectAll" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                            </option>
                                            <option value="selectVisible">
                                                <asp:Localize ID="Localize_Admin_Catalog_SelectVisible" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                            </option>
                                            <option value="unselectVisible">
                                                <asp:Localize ID="Localize_Admin_Catalog_UnselectVisible" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                            </option>
                                            <option value="deleteSelected">
                                                <asp:Localize ID="Localize_Admin_Catalog_DeleteSelected" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                            </option>
                                            <option value="changeStatus">
                                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_ChangeStatus %>"></asp:Localize>
                                            </option>
                                            <option value="setPay">
                                                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_MarkPayed %>"></asp:Localize>
                                            </option>
                                            <option value="setNotPay">
                                                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_MarkNotPayed %>"></asp:Localize>
                                            </option>

                                            <option value="setManagerConfirmedTrue" runat="server" id="setManagerConfirmedTrue" visible="False">
                                                <asp:Localize ID="Localize3" runat="server" Text="Разрешить оплату"></asp:Localize>
                                            </option>
                                            <option value="setManagerConfirmedFalse" runat="server" id="setManagerConfirmedFalse" visible="False">
                                                <asp:Localize ID="Localize4" runat="server" Text="Запретить оплату"></asp:Localize>
                                            </option>
                                        </select>
                                        <asp:DropDownList ID="ddlStatus" CssClass="NoneDisplay" runat="server" DataSourceID="DSStatus"
                                            DataTextField="StatusName" DataValueField="OrderStatusID">
                                        </asp:DropDownList>
                                        <asp:TextBox runat="server" ID="txtStatusBasis" CssClass="NoneDisplay" placeholder="Основание для смены статуса" Width="300px" />
                                        <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                        <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                            OnClick="lbDeleteSelected_Click" />
                                        <asp:LinkButton ID="lbChangeStatus" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_ChangeStatus %>"
                                            OnClick="lbChangeStatus_Click" />
                                        <asp:LinkButton ID="lbSetPay" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetActive %>"
                                            OnClick="lbSetPay_Click" />
                                        <asp:LinkButton ID="lbSetNotPay" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"
                                            OnClick="lbSetNotPay_Click" />

                                        <asp:LinkButton ID="lblSetManagerConfirmedTrue" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetActive %>"
                                            OnClick="lblSetManagerConfirmedTrue_Click" />
                                        <asp:LinkButton ID="lblSetManagerConfirmedFalse" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"
                                            OnClick="lblSetManagerConfirmedFalse_Click" />

                                    </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold"><%= SelectedCount() %></span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected%></span></span></span></td>
                                <td style="text-align: right;" class="selecteditems">
                                    <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Localize ID="Localize_Admin_Catalog_Total" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Total %>"></asp:Localize>
                                            <span class="bold">
                                                <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" />
                                            </span>
                                            <asp:Localize ID="Localize_Admin_Catalog_RecordsFound" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_RecordsFound %>"></asp:Localize>
                                            <br />
                                            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_Total %>"></asp:Localize>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="bold" Text="0" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 8px;"></td>
                            </tr>
                        </table>
                        <div>
                            <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                                <table class="filter" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr style="height: 5px;">
                                        <td colspan="7"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40px; text-align: center;" rowspan="2">
                                            <div style="height: 0px; font-size: 0px; width: 59px">
                                            </div>
                                            <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                Width="99%">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 86px;" rowspan="2">
                                            <div style="height: 0px; font-size: 0px; width: 86px">
                                            </div>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtNumber" Width="99%" runat="server" TabIndex="11" />
                                        </td>
                                        <td style="width: 64px;" rowspan="2">
                                            <div style="font-size: 0; line-height: 0; width: 64px;">
                                            </div>
                                            <asp:DropDownList ID="ddlStatusName" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                Width="99%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="" rowspan="2">
                                            <div style="height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtBuyerName" Width="99%" runat="server"
                                                TabIndex="13" />
                                        </td>
                                        <td style="width: 150px;" rowspan="2">
                                            <div style="height: 0px; font-size: 0px; width: 150px">
                                            </div>
                                            <asp:DropDownList ID="ddlPaymentMethod" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                Width="99%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 70px;" rowspan="2">
                                            <div style="height: 0px; font-size: 0px; width: 70px">
                                            </div>
                                            <asp:DropDownList ID="ddlPayed" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                Width="99%">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                    Value="any" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="yes" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="no" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 135px;" rowspan="2">
                                            <div style="height: 0px; font-size: 0px; width: 135px">
                                            </div>
                                            <asp:DropDownList ID="ddlShippingMethod" TabIndex="10" CssClass="dropdownselect"
                                                runat="server" Width="99%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 130px;" rowspan="2" id="tdManager" runat="server" visible="False">
                                            <div style="height: 0px; font-size: 0px; width: 130px">
                                            </div>
                                            <asp:DropDownList ID="ddlManagers" TabIndex="10" CssClass="dropdownselect"
                                                runat="server" Width="99%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 130px;" rowspan="2" id="tdManagerConfirmed" runat="server" visible="False">
                                            <div style="height: 0px; font-size: 0px; width: 130px">
                                            </div>
                                            <asp:DropDownList ID="ddlManagerConfirmed" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                Width="99%">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                    Value="any" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="yes" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="no" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 121px;">
                                            <div style="height: 0px; width: 120px; font-size: 0px;">
                                            </div>
                                            <table cellpadding="1px;" cellspacing="0px;" style="margin-left: 5px;">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <%=Resource.Admin_Catalog_From%>:
                                                    </td>
                                                    <td style="width: 110px;">
                                                        <div>
                                                            <asp:TextBox CssClass="filtertxtbox" ID="txtSumFrom" runat="server" TabIndex="19" Width="80" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <%=Resource.Admin_Catalog_To%>:
                                                    </td>
                                                    <td style="width: 88px;">
                                                        <div>
                                                            <asp:TextBox CssClass="filtertxtbox" ID="txtSumTo" runat="server" TabIndex="19" Width="80" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 135px;">
                                            <div style="height: 0px; width: 135px; font-size: 0px;">
                                            </div>
                                            <table cellpadding="1px;" cellspacing="0px;" style="margin-left: 5px;">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <%=Resource.Admin_Catalog_From%>:
                                                    </td>
                                                    <td style="width: 110px;">
                                                        <div class="dp">
                                                            <asp:TextBox CssClass="filtertxtbox" ID="txtDateFrom" Width="80" runat="server" TabIndex="12" />
                                                            <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <%=Resource.Admin_Catalog_To%>:
                                                    </td>
                                                    <td style="width: 88px;">
                                                        <div class="dp">
                                                            <asp:TextBox CssClass="filtertxtbox" ID="txtDateTo" Width="80" runat="server" TabIndex="12" />
                                                            <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50px; padding-right: 10px; text-align: center;" rowspan="2">
                                            <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                                TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                            <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                                TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px;" colspan="7"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <adv:AdvGridView ID="grid" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                        CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                                        CssClass="tableview" GridLines="None" EnableModelValidation="True" EnableEdit="False"
                                        OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting" OnRowDataBound="grid_OnDataBound" ReadOnlyGrid="True">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="ID" Visible="false" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="40px">
                                                <HeaderTemplate>
                                                    <div style="text-align: center;">
                                                        <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# ((bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />")%>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5px" ItemStyle-CssClass="statusColor"
                                                HeaderStyle-CssClass="statusColor">
                                                <HeaderTemplate>
                                                    <div style="width: 5px;">
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div <%# "style=\" width: 5px; float:left; height:40px; background-color:#" +  Eval("Color") + "\"" %>>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Number" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lbNumber" runat="server" CommandName="Sort" CommandArgument="SortNumber">
                                                        <%=Resource.Admin_OrderSearch_OrderNum%><asp:Image ID="arrowNumber" CssClass="arrow"
                                                            runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <a href='<%# "vieworder.aspx?orderid=" + Eval("ID") %>'>
                                                        <asp:Literal ID="lOrderId" runat="server" Text='<%# Bind("Number") %>'></asp:Literal>
                                                    </a>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="StatusName" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lbStatusName" runat="server" CommandName="Sort" CommandArgument="SortStatus">
                                                        <%=Resource.Admin_OrderSearch_Status%><asp:Image ID="arrowStatusName" CssClass="arrow"
                                                            runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lStatusName" runat="server" Text='<%# Eval("StatusName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="BuyerName" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                                <HeaderTemplate>
                                                    <%=Resource.Admin_OrderSearch_BuyerName%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("BuyerName") %>
                                                    <%# SQLDataHelper.GetInt(Eval("Rating")) != 0 ? "<span class='users-rate'>" +Eval("Rating") + "</span>" : string.Empty %>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="PaymentMethod" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lbPaymentMethod" runat="server" CommandName="Sort" CommandArgument="PaymentMethod">
                                                        <%=Resource.Admin_OrderSearch_Payment%><asp:Image ID="arrowPaymentMethod"
                                                            CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span style="white-space: normal; width: 170px;">
                                                        <asp:Label ID="lPayment" runat="server" Text='<%# (Eval("PaymentMethod") != DBNull.Value ? Eval("PaymentMethod") : Eval("PaymentMethodName")) %>'></asp:Label>
                                                    </span>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="PaymentDate" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="65">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lbPay" runat="server" CommandName="Sort" CommandArgument="PaymentDate">
                                                        <%= Resource.Admin_OrderSearch_Payed%><asp:Image ID="arrowPay" CssClass="arrow"
                                                            runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: center">
                                                        <asp:CheckBox ID="chkPay" runat="server" Checked='<%# Eval("PaymentDate") != DBNull.Value  %>' Enabled="False" />
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ShippingMethod" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lbShippingMethod" runat="server" CommandName="Sort" CommandArgument="ShippingMethodName">
                                                        <%=Resource.Admin_OrderSearch_Shipping%><asp:Image ID="arrowShippingMethod"
                                                            CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lShipping" runat="server" Text='<%# Bind("ShippingMethodName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ManagerName" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" Visible="False" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=Resource.Admin_OrderSearch_Manager%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("ManagerName")%>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ManagerConfirmed" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" Visible="False" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    Доступен к оплате
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbManagerConfirmed" runat="server" Checked='<%# Eval("ManagerConfirmed")  %>' Enabled="False" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Sum" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="Sum" runat="server" CommandName="Sort" CommandArgument="Sum">
                                                        <%=Resource.Admin_OrderSearch_Sum%><asp:Image ID="arrowSum" CssClass="arrow"
                                                            runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lSum" runat="server" Text='<%#PriceFormatService.FormatPrice(SQLDataHelper.GetFloat(Eval("Sum")), SQLDataHelper.GetFloat(Eval("CurrencyValue")), SQLDataHelper.GetString(Eval("CurrencySymbol")), SQLDataHelper.GetString(Eval("CurrencyCode")), SQLDataHelper.GetBoolean(Eval("IsCodeBefore")), null)%>'> </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_OrderSearch_OrderTime %>"
                                                ItemStyle-CssClass="coladdDate" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                ItemStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lbOrderDate" runat="server" CommandName="Sort" CommandArgument="OrderDate">
                                                        <%=Resource.Admin_OrderSearch_OrderTime%><asp:Image ID="arrowOrderDate"
                                                            CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertDate(SQLDataHelper.GetDateTime( Eval("OrderDate"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle CssClass="colmodify"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href='<%# "vieworder.aspx?orderid=" + Eval("ID") %>' class="showtooltip" title="<%=Resource.Admin_OrderSearch_Edit%>">
                                                        <img src="images/editbtn.gif" style="border: none;" alt="" /></a>
                                                    <asp:LinkButton ID="buttonDelete" runat="server"
                                                        CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' CssClass="deletebtn showtooltip valid-confirm"
                                                        Enabled="True" ToolTip='<%$ Resources:Resource, Admin_OrderSearch_Delete%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="header" />
                                        <RowStyle CssClass="row1 readonlyrow" />
                                        <AlternatingRowStyle CssClass="row2 readonlyrow" />
                                        <EmptyDataTemplate>
                                            <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                                <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_NoRecords %>"></asp:Localize>
                                            </div>
                                        </EmptyDataTemplate>
                                    </adv:AdvGridView>
                                    <div style="border-top: 1px #c9c9c7 solid;">
                                    </div>
                                    <table class="results2">
                                        <tr>
                                            <td style="width: 157px; padding-left: 6px;">
                                                <asp:Localize ID="Localize_Admin_Catalog_ResultPerPage" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ResultPerPage %>"></asp:Localize>:&nbsp;<asp:DropDownList
                                                    ID="ddRowsPerPage" runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist"
                                                    AutoPostBack="true">
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                    <asp:ListItem>100</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: center;">
                                                <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                                    UseHref="false" OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
                                            </td>
                                            <td style="width: 157px; text-align: right; padding-right: 12px">
                                                <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                                    <span style="color: #494949">
                                                        <%=Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                            Width="30" /></span>
                                                    <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                        OnClick="linkGO_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                        <br />
                    </div>
                </div>
                <asp:SqlDataSource ID="DSStatus" runat="server" SelectCommand="SELECT * FROM [Order].[OrderStatus]"
                    OnInit="DSStatus_Init"></asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
        <script type="text/javascript">

            $(document).ready(function () {
                $("input.showtooltip").tooltip({
                    showURL: false
                });
            });

            function hide_wait_for_node(node) {
                if (node.wait_img) {
                    node.removeChild(node.wait_img);
                }
            }

            function CreateHistory(hist) {
                $.historyLoad(hist);
            }

            var timeOut;
            function Darken() {
                timeOut = setTimeout('document.getElementById("inprogress").style.display = "block";', 1000);
            }

            function Clear() {
                clearTimeout(timeOut);
                document.getElementById("inprogress").style.display = "none";

                $("input.sel").each(function (i) {
                    if (this.checked) $(this).parent().parent().addClass("selectedrow");
                });

                initgrid();
            }

            $(document).ready(function () {
                document.onkeydown = keyboard_navigation;
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(Darken);
                prm.add_endRequest(Clear);
                initgrid();
                $("ineditcategory").tooltip();
            });

            function removeunloadhandler() {
                window.onbeforeunload = null;
            }


            function beforeunload(e) {
                if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                    var evt = window.event || e;
                    evt.returnValue = '<%=Resource.Admin_OrderSearch_LostChanges%>';
                }
            }

            function addbeforeunloadhandler() {
                window.onbeforeunload = beforeunload;
            }

            function selectCange() {
                $("#commandButton").click(function () {
                    var command = $("#commandSelect").val();

                    switch (command) {
                        case "selectAll":
                            SelectAll(true);
                            break;
                        case "unselectAll":
                            SelectAll(false);
                            break;
                        case "selectVisible":
                            SelectVisible(true);
                            break;
                        case "unselectVisible":
                            SelectVisible(false);
                            break;
                        case "deleteSelected":
                            var r = confirm("<%= Resource.Admin_OrderSearch_Confirm%>");
                            if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                            break;
                        case "changeStatus":
                            __doPostBack('<%=lbChangeStatus.UniqueID%>', '');
                            break;
                        case "setPay":
                            document.getElementById('<%=lbSetPay.ClientID%>').click();
                            break;
                        case "setNotPay":
                            document.getElementById('<%=lbSetNotPay.ClientID%>').click();
                            break;


                        case "setManagerConfirmedTrue":
                            document.getElementById('<%=lblSetManagerConfirmedTrue.ClientID%>').click();
                            break;
                        case "setManagerConfirmedFalse":
                            document.getElementById('<%=lblSetManagerConfirmedFalse.ClientID%>').click();
                            break;
                    }
                });
            }

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { selectCange(); });

            function ChangeSelect() {
                var index = document.getElementById("commandSelect").selectedIndex;

                if (document.getElementById("commandSelect").options[index].text == '<%=Resource.Admin_OrderSearch_ChangeStatus%>') {
                    document.getElementById('<%=ddlStatus.ClientID%>').style.display = "inline";
                    document.getElementById('<%=txtStatusBasis.ClientID%>').style.display = "inline";
                } else {
                    document.getElementById('<%=ddlStatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtStatusBasis.ClientID%>').style.display = "none";
                }
            }

        </script>
    </div>
</asp:Content>
