<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.CustomersGroups" CodeBehind="CustomersGroups.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Customers" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
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
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
        })

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
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="CustomerSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Users%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="CustomerSearch.aspx?role=User">
                            <%= Resource.Admin_MasterPageAdmin_Buyers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="CustomerSearch.aspx?role=Moderator">
                            <%= Resource.Admin_MasterPageAdmin_Moderators %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="CustomerSearch.aspx?role=Administrator">
                            <%= Resource.Admin_MasterPageAdmin_Administrators %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item selected"><a href="CustomersGroups.aspx">
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
    <div class="content-own">
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
                                <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_CustomerGroup_Header %>"></asp:Label><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_CustomerGroup_SubHeader %>"></asp:Label>
                        </td>
                        <td>
                            <div style="float: right;">
                                <div style="height: 41px" id="advButtonFiller" runat="server">
                                </div>
                                <div class="btns-main">
                                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddCustomerGroup" runat="server" Text="<%$ Resources:Resource, Admin_CustomerGroup_Add %>" ValidationGroup="0" OnClick="btnAddCustomerGroup_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="width: 100%">
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td colspan="2">
                            <center>
                            <asp:Label ID="lMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                        </center>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                            </span><span style="display: inline-block;">
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
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span></span>
                        </td>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <%=Resources.Resource.Admin_Catalog_Total%>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resources.Resource.Admin_Catalog_RecordsFound%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 8px;"></td>
                    </tr>
                </table>
                <div>
                    <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                        <table class="filter" cellpadding="2" cellspacing="0">
                            <tr style="height: 5px;">
                                <td colspan="5"></td>
                            </tr>
                            <tr>
                                <td style="width: 80px; text-align: center;">
                                    <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server" Width="65">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <div style="width: 130px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtGroupName" Width="99%" runat="server" TabIndex="12" />
                                </td>
                                <td style="width: 140px;">
                                    <div style="width: 140px; font-size: 0px; height: 0px;">
                                    </div>
                                </td>
                                <td style="width: 230px;">
                                    <div style="width: 230px; font-size: 0px; height: 0px;">
                                    </div>
                                </td>
                                <td style="width: 150px;">
                                    <div style="width: 150px; font-size: 0px; height: 0px;">
                                    </div>
                                </td>
                                <td style="width: 100px;">
                                    <div style="width: 100px; font-size: 0px; height: 0px;">
                                    </div>
                                    <center>
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </center>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;" colspan="5"></td>
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
                            <asp:AsyncPostBackTrigger ControlID="grid" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="grid" EventName="Sorting" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="2"
                                CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_CustomerGroup_QDelete %>" CssClass="tableview"
                                Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription="" DataFieldForImagePath=""
                                EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting" OnRowDataBound="grid_RowDataBound"
                                ShowFooter="false">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="Label02" runat="server" Text='0'></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 70px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="GroupName" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <div style="width: 130px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbGroupName" runat="server" CommandName="Sort" CommandArgument="GroupName">
                                                <%= Resources.Resource.Admin_Customer_GroupName %>
                                                <asp:Image ID="arrowGroupName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGroupNameBind" runat="server" Text='<%# Eval("GroupName") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lGroupName" runat="server" Text='<%# Bind("GroupName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewGroupName" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="GroupDiscount" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="130px">
                                        <HeaderTemplate>
                                            <div style="width: 130px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbGroupDiscount" runat="server" CommandName="Sort" CommandArgument="GroupDiscount">
                                                <%=Resources.Resource.Admin_Customer_GroupDiscount%>
                                                <asp:Image ID="arrowGroupDiscount" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lGroupDiscount" runat="server" Text='<%# Bind("GroupDiscount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGroupDiscount" runat="server" Text='<%# Eval("GroupDiscount") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewGroupDiscount" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="MinimumOrderPrice" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                        <HeaderTemplate>
                                            <div style="width: 200px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbMinimumOrderPrice" runat="server" CommandName="Sort" CommandArgument="MinimumOrderPrice">
                                                <%=Resources.Resource.Admin_Customer_MinimumOrderPrice%>
                                                <asp:Image ID="arrowMinimumOrderPrice" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lMinimumOrderPrice" runat="server" Text='<%# Bind("MinimumOrderPrice") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMinimumOrderPrice" runat="server" Text='<%# Eval("MinimumOrderPrice") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewMinimumOrderPrice" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="MinimumOrderPrice" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <a href="CustomerSearch.aspx?group=<%# Eval("ID")%>"><%= Resources.Resource.Admin_Customer_SeeCustomers %></a>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="90px">
                                        <EditItemTemplate>

                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image" src="images/updatebtn.png"
                                                onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                            <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                CommandName="DeleteCustomerGroup" CommandArgument='<%# Eval("ID")%>' Visible='<%# (SQLDataHelper.GetInt(Eval("ID")) != CustomerGroupService.DefaultCustomerGroup) %>'
                                                data-confirm="<%$ Resources:Resource, Admin_CustomerGroup_Confirmation %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image" src="images/cancelbtn.png"
                                                onclick="row_canceledit($(this).parent().parent()[0]); return false;" style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibAddCustomerGroup" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddCustomerGroup"
                                                ToolTip="<%$ Resources:Resource, Admin_CustomerGroup_Add  %>" />
                                            <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png" CommandName="CancelAdd"
                                                ToolTip="<%$ Resources:Resource, Admin_CustomerGroup_Add  %>" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#ccffcc" />
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resources.Resource.Admin_Catalog_NoRecords%>
                                </center>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 200px; padding-left: 6px;">
                                        <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage" runat="server"
                                            OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7" UseHref="false"
                                            UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                    </td>
                                    <td style="width: 157px; text-align: right; padding-right: 12px">
                                        <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                            <span style="color: #494949">
                                                <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server" Width="30" /></span>
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                OnClick="linkGO_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
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
