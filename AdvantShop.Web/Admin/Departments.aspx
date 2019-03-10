<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="Admin.Departments" %>

<%@ Import Namespace="Resources" %>

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
                        var r = confirm("<%= Resources.Resource.Admin_Catalog_Confirm%>");
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
            <menu class="neighbor-menu neighbor-catalog">
                <li class="neighbor-menu-item selected dropdown-menu-parent">
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
                <li class="neighbor-menu-item ">
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
        </menu>
        <div class="panel-add">
            <a href="CreateCustomer.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_Add %> <%= Resource.Admin_MasterPageAdmin_Customer %></a>
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
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Departments_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Departments_SubHeader %>"></asp:Label>
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
                <div class="btns-main">
                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddDepartment" runat="server" Text="<%$ Resources:Resource, Admin_Departments_AddDepartment %>" ValidationGroup="0" OnClick="btnAddDepartment_Click" />
                </div>
                <div style="height: 10px">
                </div>
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
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected %></span></span>
                            </span>
                        </td>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <%=Resources.Resource.Admin_Catalog_Total %>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resources.Resource.Admin_Catalog_RecordsFound %>
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
                                <td colspan="6"></td>
                            </tr>
                            <tr>
                                <td style="width: 60px; text-align: center;">
                                    <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server" Width="55">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="99%" runat="server" TabIndex="12" />
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSortOrder" Width="99%" runat="server" TabIndex="14" />
                                </td>
                                <td style="text-align: center; width: 120px;">
                                    <asp:DropDownList ID="ddlEnabled" TabIndex="18" CssClass="dropdownselect" runat="server">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                            Value="any" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                    </asp:DropDownList>
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
                                <td style="height: 5px;" colspan="6"></td>
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
                            <asp:AsyncPostBackTrigger ControlID="btnAddDepartment" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                                CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Departments_Confirmation %>" CssClass="tableview"
                                Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription="" DataFieldForImagePath=""
                                EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting"
                                ShowFooter="false">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="60px" HeaderStyle-Width="60px"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="width: 60px; font-size: 0px">
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderName" runat="server" CommandName="Sort" CommandArgument="Name">
                                                <%=Resources.Resource.Admin_Catalog_Name %>
                                                <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewName" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Sort" ItemStyle-Width="200" HeaderStyle-Width="200">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbSort" runat="server" CommandName="Sort" CommandArgument="Sort">
                                                <%=Resources.Resource.Admin_Properties_SortOrder%>
                                                <asp:Image ID="arrowSort" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSort" Text='<%# Bind("Sort") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtSortBind" Width="95%" Text='<%# Bind("Sort") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="txtNewSort" Width="95%" CssClass="add" Text=''></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Enabled" ItemStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                                <div style="float: left; width: 90px; text-align: center;">
                                                    <%=Resources.Resource.Admin_Departments_Enabled%>
                                                </div>
                                                <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif"
                                                    Style="margin-top: 7px; margin-left: 0px;" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEnabledBind" runat="server" Checked='<%# Bind("Enabled") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEnabledBind2" runat="server" Checked='<%# Bind("Enabled") %>' Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkNewEnabled" runat="server" CssClass="add" Checked="True" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="25px" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <img src="images/editbtn.gif" class="editbtn showtooltip" style="border: none;" alt='' />
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image" src="images/updatebtn.png"
                                                onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibAddDepartment" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddDepartment" ToolTip="<%$ Resources:Resource, Admin_Add  %>" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="25px" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                CommandName="DeleteDepartment" CommandArgument='<%# Eval("ID")%>'
                                                data-confirm="<%$ Resources:Resource, Admin_Departments_Confirmation %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image" src="images/cancelbtn.png"
                                                onclick="row_canceledit($(this).parent().parent()[0]); return false;" style="display: none" title="<%=Resources.Resource.Admin_Cancel %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png" CommandName="CancelAdd"
                                                ToolTip="<%$ Resources:Resource, Admin_Cancel  %>" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#ccffcc" />
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                        <%=Resources.Resource.Admin_Catalog_NoRecords %>
                                    </div>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage" runat="server"
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
                                                <%=Resources.Resource.Admin_Catalog_PageNum %>&nbsp;<asp:TextBox ID="txtPageNum" runat="server" Width="30" /></span>
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
