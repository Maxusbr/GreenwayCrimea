<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.OrderStatuses" CodeBehind="OrderStatuses.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="OrderSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
            <li class="neighbor-menu-item selected"><a href="OrderStatuses.aspx">
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
        <adv:EnumDataSource runat="server" ID="edsOrderStatusCommand" EnumTypeName="AdvantShop.Orders.OrderStatusCommand">
        </adv:EnumDataSource>
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
                                    <asp:Label ID="lblOrderStatuses" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatuses_OrderStatuses %>"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblOrderStatusesName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatuses_OrderStatusesView %>"></asp:Label>
                                </td>
                                <td>
                                    <div class="btns-main">
                                        <asp:Button CssClass="btn btn-middle btn-add" runat="server" ID="btnAddStatus" OnClick="btnAddStatus_Click"
                                            Text="<%$ Resources: Resource, Admin_OrderStatuses_Add %>" />
                                    </div>
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
                                        <select id="commandSelect">
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
                                        </select>
                                        <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                        <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                            OnClick="lbDeleteSelected_Click" />
                                    </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold"><%= SelectedCount() %></span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected %></span></span></span></td>
                                <td style="text-align: right;" class="selecteditems">
                                    <asp:UpdatePanel ID="upCounts" runat="server">
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 8px;"></td>
                            </tr>
                        </table>
                        <div>
                            <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                                <table class="filter" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 74px; text-align: center;">
                                            <div style="height: 0px; font-size: 0px; width: 74px">
                                            </div>
                                            <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                Width="65px">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <div style="height: 0px; font-size: 0px; width: 140px">
                                            </div>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtStatusName" Width="98%" runat="server"
                                                TabIndex="11" />
                                        </td>
                                        <td style="width: 200px; text-align: center;">
                                            <asp:DropDownList ID="ddlIsDefaultFilter" TabIndex="10" CssClass="dropdownselect"
                                                runat="server" Width="65px">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                    Value="any" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px; text-align: center;">
                                            <asp:DropDownList ID="ddlCanceledFilter" TabIndex="10" CssClass="dropdownselect"
                                                runat="server" Width="65px">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                    Value="any" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px; text-align: center;">
                                            <asp:DropDownList ID="ddlCompletedFilter" TabIndex="10" CssClass="dropdownselect"
                                                runat="server" Width="65px">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                    Value="any" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px; text-align: center;">
                                            <asp:DropDownList ID="ddlHiddenFilter" TabIndex="10" CssClass="dropdownselect"
                                                runat="server" Width="65px">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                    Value="any" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 140px; text-align: center;">
                                            <div style="height: 0px; font-size: 0px; width: 140px">
                                            </div>
                                        </td>
                                        <td style="width: 100px; text-align: center;">
                                            <div style="height: 0px; font-size: 0px; width: 100px">
                                            </div>
                                        </td>
                                        <td style="width: 230px; text-align: center;">
                                            <asp:DropDownList ID="ddlCommandIDFilter" TabIndex="10" CssClass="dropdownselect"
                                                runat="server" DataTextField="LocalizedName" DataValueField="Value" DataSourceID="edsOrderStatusCommand"
                                                OnDataBound="ddlCommandIDFilter_Databound">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 100px; padding-right: 10px;">
                                            <div style="text-align: center;">
                                                <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                                    TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                                    TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:UpdatePanel ID="UpdatePanelGrid" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                        CellPadding="5" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_OrderStatuses_Confirmation %>"
                                        CssClass="tableview" GridLines="None" OnSorting="grid_Sorting" OnRowDataBound="grid_RowDataBound"
                                        OnRowCommand="grid_RowCommand" ShowFooterWhenEmpty="true">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="ID" Visible="false" HeaderStyle-Width="100%">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="Label02" runat="server" Text='0'></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="40" HeaderStyle-Width="40">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 40px; font-size: 0px;">
                                                    </div>
                                                    <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall headerCb" runat="server"
                                                        onclick="javascript:SelectVisible(this.checked);" Style="margin-left: 0px;" />
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <%# (bool) Eval("IsSelected")? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="StatusName" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Width="70">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 110px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbOrderStatusesName" runat="server" CommandName="Sort" CommandArgument="StatusName">
                                                        <%= Resources.Resource.Admin_OrderStatuses_StatusName%>
                                                        <asp:Image ID="arrowStatusName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtStatusNameBind" runat="server" Text='<%# Eval("StatusName") %>' Width="98%">
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtStatusNameAdd" CssClass="add" runat="server" Width="98%"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="IsDefault" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 60px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbOrderIsDefault" runat="server" CommandName="Sort" CommandArgument="IsDefault">
                                                        <%= Resources.Resource.Admin_OrderStatuses_IsDefault%>
                                                        <asp:Image ID="arrowIsDefault" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <adv:PlainCheckBox runat="server" ID="chkIsDefault" Checked='<%# Eval("IsDefault") %>'>
                                                    </adv:PlainCheckBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkIsDefaultAdd" CssClass="switchCheckbox add" runat="server"></asp:CheckBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="IsCanceled" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 70px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbOrderCanceled" runat="server" CommandName="Sort" CommandArgument="IsCanceled">
                                                        <%= Resources.Resource.Admin_OrderStatuses_Canceled%>
                                                        <asp:Image ID="arrowCanceled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <adv:PlainCheckBox runat="server" ID="chkCanceled" Checked='<%# Eval("IsCanceled") %>'>
                                                    </adv:PlainCheckBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkCanceledAdd" CssClass="switchCheckbox add" runat="server"></asp:CheckBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="IsCompleted" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 70px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbOrderCompleted" runat="server" CommandName="Sort" CommandArgument="IsCompleted">
                                                        <%= Resources.Resource.Admin_OrderStatuses_Completed%>
                                                        <asp:Image ID="arrowCompleted" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <adv:PlainCheckBox runat="server" ID="chkCompleted" Checked='<%# Eval("IsCompleted") %>'>
                                                    </adv:PlainCheckBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkCompletedAdd" CssClass="add" runat="server"></asp:CheckBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="Hidden" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 70px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbHidden" runat="server" CommandName="Sort" CommandArgument="Hidden">
                                                        Скрыт
                                                        <asp:Image ID="arrowHidden" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <adv:PlainCheckBox runat="server" ID="chkHidden" Checked='<%# Eval("Hidden") %>'>
                                                    </adv:PlainCheckBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkHiddenAdd" CssClass="add" runat="server"></asp:CheckBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SortOrder" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 80px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                        <%= Resources.Resource.Admin_OrderStatuses_SortOrder%>
                                                        <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtSortOrder" runat="server" Text='<%#Eval("SortOrder") %>' Width="80px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtAddSortOrder" CssClass="add" runat="server" Width="80px"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Color" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 100px; font-size: 0px;">
                                                    </div>
                                                    <%= Resources.Resource.Admin_OrderStatuses_Color%>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtColor" runat="server" data-plugin="jpicker" Width="1px" Style="display: none;"
                                                        Text='<%# Eval("Color") ?? string.Empty %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtColorAdd" runat="server" data-plugin="jpicker" Width="1px" Style="display: none;"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="CommandID" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="200"
                                                ItemStyle-Width="200">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 200px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbOrderCommandID" runat="server" CommandName="Sort" CommandArgument="CommandID"
                                                        Style="text-align: center;">
                                                        <%= Resources.Resource.Admin_OrderStatuses_CommandID%>
                                                        <asp:Image ID="arrowCommandID" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlCommandID" runat="server" DataTextField="LocalizedName"
                                                        DataValueField="Value" DataSourceID="edsOrderStatusCommand">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlCommandIDAdd" CssClass="add" runat="server" DataTextField="LocalizedName"
                                                        DataValueField="Value" DataSourceID="edsOrderStatusCommand">
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="100" HeaderStyle-Width="100" AccessibleHeaderText="Buttons"
                                                ItemStyle-HorizontalAlign="Center" FooterStyle-Width="100" FooterStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 60px; font-size: 0px;">
                                                    </div>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="buttonDelete" runat="server"
                                                        CssClass="deletebtn showtooltip valid-confirm" CommandName="StatusDelete" CommandArgument='<%# Eval("ID")%>'
                                                        ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>'
                                                        data-confirm="<%$ Resources: Resource, Admin_OrderStatuses_Confirmation %>"
                                                        Visible='<%# OrderStatusService.StatusCanBeDeleted((int)Eval("ID")) %>' />
                                                    <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                        src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                        style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update %>' />
                                                    <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                        src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                        style="display: none" title='<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="buttonAdd" ImageUrl="images/addbtn.gif" runat="server" ToolTip="<%$ Resources:Resource, Admin_OrderStatuses_Add  %>"
                                                        CommandName="Add" />
                                                    <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                        CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Currencies_CancelAdd  %>" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="footer" />
                                        <HeaderStyle CssClass="header" />
                                        <RowStyle CssClass="row1 readonlyrow" />
                                        <AlternatingRowStyle CssClass="row2 readonlyrow" />
                                        <EmptyDataTemplate>
                                            <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                                <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatuses_EmptyDataPage %>"></asp:Localize>
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
                                                        <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                            Width="30" /></span>
                                                    <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label><br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
        <script type="text/javascript">

            $(document).ready(function () {

                $(".showtooltip").tooltip({
                    showURL: false
                });

                $(".switchCheckbox").live("change", function () {
                    if ($(this).is(":checked")) {
                        $(this).closest("tr").find(".switchCheckbox").removeAttr("checked");
                        $(this).attr("checked", "checked");
                    }
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
                    evt.returnValue = '<%=Resources.Resource.Admin_OrderSearch_LostChanges%>';
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
                            var r = confirm("<%= Resources.Resource.Admin_OrderStatuses_Confirm%>");
                            if (r) __doPostBack('<%= lbDeleteSelected.UniqueID %>', '');
                            break;
                    }
                });
            }

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { selectCange(); });

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                //$('span.jPicker').remove();
                var objects = $('[data-plugin="jpicker"]');
                if ($('span.jPicker').length == 0) {
                    objects.jPicker();
                }
            });

        </script>
    </div>
</asp:Content>
