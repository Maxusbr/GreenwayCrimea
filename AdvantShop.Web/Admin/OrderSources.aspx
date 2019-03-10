<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.OrderSources" EnableEventValidation="false" CodeBehind="OrderSources.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="OrderSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
            <li class="neighbor-menu-item"><a href="OrderStatuses.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderStatuses%></a></li>
            <li class="neighbor-menu-item selected"><a href="OrderSources.aspx">
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
        <div class="panel-add">
            <a href="CreateCustomer.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_Add %> <%= Resource.Admin_MasterPageAdmin_Customer %></a>,
            <a href="ManagerTask.aspx" class="panel-add-lnk"><%= Resource.Admin_ManagersTasks_Task %></a>, 
            <a href="EditLead.aspx" class="panel-add-lnk"><%= Resource.Admin_Leads_AddTop %></a>
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
                                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text='<%$ Resources:Resource, Admin_OrderSource_OrederSources %>'></asp:Label><br />
                                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text='<%$ Resources:Resource, Admin_OrderSource_OrederSources %>'></asp:Label>
                                </td>
                                <td class="btns-main">
                                    <asp:Button CssClass="btn btn-middle btn-add" runat="server" ID="btnAddOrder" OnClick="btnAddOrderSource_Click"
                                        Text='<%$ Resources:Resource, Admin_OrderSource_AddOrederSource %>'/>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div style="height: 10px;">
                </div>

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
                                    </select>
                                    <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                    <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbDeleteSelected_Click" />
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 8px;"></td>
                        </tr>
                    </table>
                    <div>
                        <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                            <table class="filter" cellpadding="0" cellspacing="0">
                                <tr style="height: 5px;">
                                    <td colspan="7"></td>
                                </tr>
                                <tr>
                                    <td style="width: 50px; text-align: center;" rowspan="2">
                                        <div style="height: 0px; font-size: 0px; width: 50px">
                                        </div>
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="99%">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <%--<td style="width: 50px;" rowspan="2">
                                        <div style="height: 0px; font-size: 0px; width: 50px">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtFilterOrderSourceId" Width="50px" runat="server" TabIndex="11" />
                                    </td>--%>
                                    <td rowspan="2">
                                        <div style="font-size: 0; line-height: 0; width: 90px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtFilterOrderSourceName" Width="99%" runat="server" TabIndex="11" />
                                    </td>
                                    <td style="width: 120px;" rowspan="2">
                                        <div style="height: 0px; font-size: 0px; width: 120px">
                                        </div>
                                        <asp:DropDownList ID="ddlFilterOrderSourceMain" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="99%">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="yes" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="no" />
                                        </asp:DropDownList>
                                        <%--<asp:CheckBox ID="ckbFilterOrderSourceMain" Width="110px" runat="server" TabIndex="13" />--%>
                                    </td>
                                    <td style="width: 240px;" rowspan="2">
                                        <div style="font-size: 0; line-height: 0; width: 240px;">
                                        </div>
                                        <asp:DropDownList ID="ddlFilterOrderSourceType" runat="server" Width="99%"></asp:DropDownList>
                                    </td>
                                    <td style="width: 160px;" rowspan="2">
                                        <div style="height: 0px; font-size: 0px; width: 120px">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtFilterOrderSourceSortOrder" Width="99%" runat="server" TabIndex="13" />
                                    </td>
                                    <td style="width: 150px;" rowspan="2">
                                        <div style="height: 0px; font-size: 0px; width: 120px">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtFilterOrdersCount" Width="99%" runat="server" TabIndex="13" />
                                    </td>
                                    <td style="width: 150px;" rowspan="2">
                                        <div style="height: 0px; font-size: 0px; width: 120px">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtFilterLeadsCount" Width="99%" runat="server" TabIndex="13" />
                                    </td>
                                    <td style="width: 60px; padding-right: 10px; text-align: center;" rowspan="2">
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
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                    CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                                    CssClass="tableview" GridLines="None" EnableModelValidation="True" EnableEdit="True"
                                    OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting" OnRowDataBound="grid_OnDataBound" ShowFooterWhenEmpty="True">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ID" Visible="false" HeaderStyle-Width="70px">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="40px">
                                            <HeaderTemplate>
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                                </div>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <%# ((bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />")%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                                    <%= Resource.Admin_OrderSource_Name %>

                                                    <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>

                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtOrderSourceName" runat="server" Text='<%# Eval("Name")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtOrderSourceNewName" runat="server" CssClass="add"></asp:TextBox>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="Main" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="110px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbMain" runat="server" CommandName="Sort" CommandArgument="Main">
                                                    <%= Resource.Admin_OrderSource_Main %>
                                                    <asp:Image ID="arrowMain" CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="ckbOrderSourceMain" runat="server" Checked='<%#Convert.ToBoolean( Eval("Main") )%>'></asp:CheckBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="ckbOrderSourceNewMain" runat="server" CssClass="add"></asp:CheckBox>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="Type" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="230px" ItemStyle-Width="230px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbType" runat="server" CommandName="Sort" CommandArgument="Type">
                                                    <%= Resource.Admin_OrderSource_Type %>
                                                    <asp:Image ID="arrowType" CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlOrderSourceType" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlOrderSourceNewType" runat="server" CssClass="add"></asp:DropDownList>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SortOrder" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="150px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                    <%= Resource.Admin_OrderSource_SortOrder %>
                                                    <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtOrderSourceSortOrder" runat="server" Text='<%#Eval("SortOrder") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtOrderSourceNewSortOrder" runat="server" CssClass="add"></asp:TextBox>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OrdersCount" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="140px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbOrdersCount" runat="server" CommandName="Sort" CommandArgument="OrdersCount">
                                                    <%= Resource.Admin_OrderSource_OrdersCount %>
                                                    <asp:Image ID="arrowOrdersCount" CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblOrdersCount" runat="server" Text='<%#Eval("OrdersCount") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="LeadsCount" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="140px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbLeadsCount" runat="server" CommandName="Sort" CommandArgument="LeadsCount">
                                                    <%= Resource.Admin_OrderSource_LeadsCount %>
                                                    <asp:Image ID="arrowLeadsCount" CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblLeadsCount" runat="server" Text='<%#Eval("LeadsCount") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center">
                                            <EditItemTemplate>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                    style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                <asp:LinkButton ID="buttonDelete" runat="server"
                                                    CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteOrderSource" CommandArgument='<%# Eval("ID")%>'
                                                    Visible='<%# Convert.ToInt32(Eval("OrdersCount")) == 0 && Convert.ToInt32(Eval("LeadsCount")) == 0 %>'
                                                    data-confirm="deleteOrderSource"
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                                <%# Convert.ToInt32(Eval("OrdersCount")) >0 || Convert.ToInt32(Eval("LeadsCount")) > 0  ? "<img src=\"images/deletebtn_disabled.png\" title=\"Удаление невозможно, есть назначенные заказы, лиды\"/>": string.Empty %>
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                    style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="ibAddOrderSource" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddOrderSource"
                                                    ToolTip="AddOrderSource" />
                                                <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                    CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Currencies_CancelAdd  %>" />
                                            </FooterTemplate>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    <br />
                </div>

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
                    }
                });
            }

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { selectCange(); });
        </script>
    </div>
</asp:Content>
