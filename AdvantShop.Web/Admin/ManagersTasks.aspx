<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeBehind="ManagersTasks.aspx.cs" Inherits="Admin.ManagersTasks" %>

<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="server">
    <script type="text/javascript">
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

        $(document).ready(function () {
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
                        var r = confirm("<%= Resource.Admin_ManagersTasks_Confirmation %>");
                        if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;
        </div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td align="center">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="color: #0D76B8;">
                            <asp:Localize ID="Localize_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
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

    <div style="padding-left: 10px; padding-right: 10px">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_ManagersTasks %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ManagersTasks_SubHeader %>"></asp:Label>
                    </td>
                    <td>
                        <div class="btns-main">
                            <asp:HyperLink CssClass="btn btn-middle btn-add" ID="btnAddManagerTask" runat="server"
                                Text="<%$ Resources:Resource, Admin_ManagersTasks_AddTask %>" NavigateUrl="ManagerTask.aspx" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:UpdatePanel ID="upErrors" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblError" runat="server" CssClass="prop-errors" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div style="width: 100%">
            <div>
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td>
                            <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                            </span><span style="display: inline-block">
                                <select id="commandSelect">
                                    <option value="selectAll">
                                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                    </option>
                                    <option value="unselectAll">
                                        <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                    </option>
                                    <option value="selectVisible">
                                        <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                    </option>
                                    <option value="unselectVisible">
                                        <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                    </option>
                                    <option value="deleteSelected">
                                        <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                    </option>
                                </select>
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                    OnClick="lbDeleteSelected_Click" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected %></span></span>
                            </span>
                        </td>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <%=Resource.Admin_Catalog_Total %>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resource.Admin_Catalog_RecordsFound %>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 8px;"></td>
                    </tr>
                </table>
                <div style="border: 1px #c9c9c7 solid; width: 100%">
                    <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                        <table class="filter" cellpadding="0" cellspacing="0">
                            <tr style="height: 5px;">
                                <td colspan="10"></td>
                            </tr>
                            <tr>
                                <td style="width: 50px; text-align: center;">
                                    <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server" Width="60">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 40px">
                                    <div style="height: 0px; width: 40px; font-size: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtTaskId" Width="99%" runat="server" TabIndex="12" />
                                </td>
                                <td style="width: 110px; text-align: center; font-size: 13px">
                                    <div style="width: 120px; height: 0px; font-size: 0px;">
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlSearchStatus" DataSourceID="edsManagerTaskStatus" CssClass="dropdownselect"
                                        DataTextField="LocalizedName" DataValueField="Value" OnDataBound="ddlFilter_DataBound" Width="120" />
                                </td>
                                <td style="width: 160px;">
                                    <div style="height: 0px; width: 160px; font-size: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSearchName" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 190px;">
                                    <div style="height: 0px; width: 190px; font-size: 0px;">
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlSearchAssignedManager" DataSourceID="sdsManagers" OnDataBound="ddlFilter_DataBound"
                                        DataTextField="FullName" DataValueField="ManagerId" CssClass="dropdownselect" Width="200" />
                                </td>
                                <td style="width: 150px;">
                                    <div style="height: 0px; width: 150px; font-size: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSearchAppointedManager" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 90px;">
                                    <div style="height: 0px; width: 90px; font-size: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSearchOrderId" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 150px;">
                                    <div style="height: 0px; width: 150px; font-size: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSearchClientEmail" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 80px;">
                                    <div style="height: 0px; width: 70px; font-size: 0px;">
                                    </div>
                                </td>
                                <td style="width: 80px;">
                                    <div style="height: 0px; width: 70px; font-size: 0px;">
                                    </div>
                                </td>
                                <td style="width: 50px; text-align: center;">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" Width="50" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                </td>
                                <td style="width: 50px; text-align: center;">
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" Width="50" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;" colspan="10"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                                CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Managers_Confirmation %>" CssClass="tableview"
                                Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting"
                                OnRowDataBound="grid_RowDataBound">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="50px" HeaderStyle-Width="50px"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 50px; font-size: 0px;">
                                            </div>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="width: 50px; font-size: 0px">
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TaskId as ID" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 40px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lTaskId" runat="server" CommandName="Sort" CommandArgument="ID">
                                                <%=Resource.Admin_ManagersTasks_TaskId %>
                                                <asp:Image ID="arrowTaskId" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <a href='<%# "ManagerTask.aspx?TaskId=" + Eval("ID")%>'>
                                                <asp:Literal ID="lLeadId" runat="server" Text='<%# Bind("ID") %>'></asp:Literal>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="Status" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="110">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 110px; font-size: 0px;">
                                            </div>
                                            <%=Resource.Admin_ManagersTasks_Status %>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlStatus" DataSourceID="edsManagerTaskStatus"
                                                DataTextField="LocalizedName" DataValueField="Value" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Width="160">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 160px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                                <%=Resource.Admin_ManagersTasks_TaskName %>
                                                <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="AssignedManagerId" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="190">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 190px; font-size: 0px;">
                                            </div>
                                            <%=Resource.Admin_ManagersTasks_AssignedManager %>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlAssignedManager" DataSourceID="sdsManagers"
                                                DataTextField="FullName" DataValueField="ManagerId" Width="190" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="AppointedName" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="150">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 150px; font-size: 0px;">
                                            </div>
                                            <%=Resource.Admin_ManagersTasks_AppointedManager %>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <a href='<%#"ViewCustomer.aspx?CustomerID=" + Eval("AppointedCustomerId")%>'>
                                                <asp:Label ID="lblAppointedName" runat="server" Text='<%# Eval("AppointedName") %>'></asp:Label></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="OrderId" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="90px">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 90px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbOrderId" runat="server" CommandName="Sort" CommandArgument="OrderId">
                                                <%=Resource.Admin_ManagersTasks_OrderNumber %>
                                                <asp:Image ID="arrowOrderId" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <a href='<%# "ViewOrder.aspx?OrderId=" + Eval("OrderId") %>' runat='server' visible='<%# Eval("OrderId") != DBNull.Value %>'>
                                                <asp:Label ID="lblOrderId" runat="server" Text='<%# "#" + Eval("OrderNumber") %>'></asp:Label>
                                            </a>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ClientCustomerId" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="110px" ItemStyle-Width="150px">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 150px; font-size: 0px;">
                                            </div>
                                            <%=Resource.Admin_ManagersTasks_Client %>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <a href='<%#"ViewCustomer.aspx?CustomerID=" + Eval("ClientCustomerId")%>' runat='server' visible='<%# Eval("ClientCustomerId") != DBNull.Value %>'>
                                                <asp:Label ID="lblCustomerId" runat="server" Text='<%# Eval("Email") %>'></asp:Label></a>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DueDate" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="70px">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 70px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbDueDate" runat="server" CommandName="Sort" CommandArgument="DueDate">
                                                <%=Resource.Admin_ManagersTasks_DueDate %>
                                                <asp:Image ID="arrowDueDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblDueDate" runat="server" Text='<%# SQLDataHelper.GetDateTime(Eval("DueDate")).ToString("dd.MM.yyyy") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DateCreated" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="70px">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 70px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbDateCreated" runat="server" CommandName="Sort" CommandArgument="DateCreated">
                                                <%=Resource.Admin_ManagersTasks_DateCreated %>
                                                <asp:Image ID="arrowDateCreated" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblDateCreated" runat="server" Text='<%# SQLDataHelper.GetDateTime(Eval("DateCreated")).ToString("dd.MM.yyyy HH:mm") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <div style="height: 0px; width: 40px; font-size: 0px;">
                                            </div>
                                            <a href="<%# "ManagerTask.aspx?TaskId=" + Eval("ID")%>">
                                                <asp:Image ID="buttonEdit" runat="server" ImageUrl="images/editbtn.gif" CssClass="editbtn showtooltip"
                                                    title='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Edit %>' /></a>
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image" src="images/updatebtn.png"
                                                onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                        <EditItemTemplate>
                                            <div style="height: 0px; width: 40px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                CommandName="DeleteManagerTask" CommandArgument='<%# Eval("ID")%>'
                                                data-confirm="<%$ Resources:Resource, Admin_ManagersTasks_Confirm %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image" src="images/cancelbtn.png"
                                                onclick="row_canceledit($(this).parent().parent()[0]); return false;" style="display: none" title="<%=Resource.Admin_Cancel %>" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#ccffcc" />
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                        <%=Resource.Admin_Catalog_NoRecords %>
                                    </div>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%=Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage" runat="server"
                                            OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem Selected="True">20</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: center;">
                                        <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7" UseHref="false"
                                            UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                    </td>
                                    <td style="width: 157px; text-align: right; padding-right: 12px">
                                        <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                            <span style="color: #494949">
                                                <%=Resource.Admin_Catalog_PageNum %>&nbsp;<asp:TextBox ID="txtPageNum" runat="server" Width="30" /></span>
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                OnClick="linkGO_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <input type="hidden" id="SelectedIds" name="SelectedIds" />
            </div>
        </div>
        <adv:EnumDataSource runat="server" ID="edsManagerTaskStatus" EnumTypeName="AdvantShop.Customers.ManagerTaskStatus" />
        <asp:SqlDataSource runat="server" ID="sdsManagers" OnInit="sds_Init"
            SelectCommand="SELECT Managers.ManagerId, LastName + ' ' + FirstName as FullName FROM Customers.Managers INNER JOIN Customers.Customer ON Customer.CustomerId = Managers.CustomerId"></asp:SqlDataSource>
        <script type="text/javascript">
            function setupTooltips() {
                $(".showtooltip").tooltip({
                    showURL: false
                });
            }
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
        </script>
    </div>
</asp:Content>
