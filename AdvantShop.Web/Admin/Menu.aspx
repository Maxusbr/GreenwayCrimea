<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Menu" CodeBehind="Menu.aspx.cs" %>

<%@ Import Namespace="AdvantShop.CMS" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/UserControls/Menu/MenuItemsTree.ascx" TagPrefix="adv" TagName="MenuItems" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
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
                        <td style="color: #0D76B8; text-align: center;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="Menu.aspx">
                <%= Resource.Admin_MasterPageAdmin_MainMenu%></a></li>
            <li class="neighbor-menu-item"><a href="NewsAdmin.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsMenuRoot%></a></li>
            <li class="neighbor-menu-item"><a href="NewsCategory.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsCategory%></a></li>
            <li class="neighbor-menu-item"><a href="Carousel.aspx">
                <%= Resource.Admin_MasterPageAdmin_Carousel%></a></li>
            <li class="neighbor-menu-item"><a href="StaticPages.aspx">
                <%= Resource.Admin_MasterPageAdmin_AuxPagesMenuItem%></a></li>
            <li class="neighbor-menu-item"><a href="StaticBlocks.aspx">
                <%= Resource.Admin_MasterPageAdmin_PageParts%></a></li>
        </menu>
        <div class="panel-add">
            <%= Resource.Admin_MasterPageAdmin_Add %>
            <a href="EditNews.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_News %></a>, 
            <a href="StaticPage.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_StaticPage %></a>
        </div>
    </div>
    <ul class="two-column">
        <li class="two-column-item menu-trees">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="grid" EventName="RowCommand" />
                </Triggers>
                <ContentTemplate>
                    <adv:MenuItems runat="server" ID="MenuTopItems" MenuType="Top" />
                    <adv:MenuItems runat="server" ID="MenuBottomItems" MenuType="Bottom" />
                    <adv:MenuItems runat="server" ID="MenuMobileItems" MenuType="Mobile" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </li>
        <li class="two-column-item">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td colspan="2">
                            <div>
                                <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource,  Admin_MenuManager_TopMenu %>"></asp:Label>
                                <div style="display: inline-block; padding-left: 10px;" id="pnlLinks" runat="server">
                                    <asp:HyperLink ID="hlEditMenuItem" CssClass="lnk-edit" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_FPanel_EditMenu %>" />
                                    <span class="cat-separator"></span>
                                    <asp:LinkButton ID="hlDeleteMenuItem" CssClass="lnk-remove valid-confirm" runat="server"
                                        Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_FPanel_DeleteMenu %>"
                                        OnClick="hlDeleteMenuItem_Click"></asp:LinkButton>
                                </div>
                            </div>
                            <div style="float: left; margin-top: 10px;">
                                <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_MenuManager_SubHeader %>"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="btns-main">
                                <asp:Button CssClass="btn btn-middle btn-add" ID="btnAdd" runat="server" Text="<%$ Resources:Resource, Admin_HeadCmdInsertMenu %>" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="width: 100%">
                <div>
                    <table style="width: 100%;" class="massaction">
                        <tr>
                            <td>
                                <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                    <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                                </span><span style="display: inline-block">
                                    <select id="commandSelect">
                                        <option value="selectAll">
                                            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                        </option>
                                        <option value="unselectAll">
                                            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                        </option>
                                        <option value="selectVisible">
                                            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                        </option>
                                        <option value="unselectVisible">
                                            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                        </option>
                                        <option value="deleteSelected">
                                            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                        </option>
                                    </select>
                                    <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                    <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbDeleteSelected_Click" />
                                </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected%></span></span>
                                </span>
                                <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td class="selecteditems" style="text-align: right;">
                                <asp:UpdatePanel ID="upCounts" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <%=Resource.Admin_Catalog_Total%>
                                        <span class="bold">
                                            <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resource.Admin_Catalog_RecordsFound%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                            <table class="filter" cellpadding="0" cellspacing="0">
                                <tr style="height: 5px;">
                                    <td colspan="6"></td>
                                </tr>
                                <tr>
                                    <td style="width: 60px; text-align: center;">
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server" Width="55">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtNameFilter" Width="99%" runat="server"
                                            TabIndex="12" />
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList ID="ddlEnabled" TabIndex="10" CssClass="dropdownselect" runat="server">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList ID="ddlBlank" TabIndex="10" CssClass="dropdownselect" runat="server">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 160px; padding-right: 0px; text-align: right; white-space: nowrap">
                                        <div style="width: 160px; height: 0px; font-size: 0px;">
                                        </div>
                                    </td>
                                    <td style="width: 100px; text-align: center;">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                            TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
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
                                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_NewsAdmin_Confirmation %>"
                                    CssClass="tableview" Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand"
                                    OnSorting="grid_Sorting" ShowFooter="false" ShowFooterWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="60" ItemStyle-Width="60px">
                                            <HeaderTemplate>
                                                <div style="width: 60px; height: 0px; font-size: 0px">
                                                </div>
                                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="MenuItemName" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lblMenuName" runat="server" CommandName="Sort" CommandArgument="MenuItemName">
                                                    <%= Resource.Admin_MenuManager_Name %>
                                                    <asp:Image ID="arrowMenuName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMenuName" runat="server" Text='<%# Eval("MenuItemName") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Enabled" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150" ItemStyle-Width="150px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lblEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                                    <%= Resource.Admin_MenuManager_Enabled %>
                                                    <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="cbEnabledEdit" runat="server" Checked='<%# Eval("Enabled") %>'></asp:CheckBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Blank" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150" ItemStyle-Width="150px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lblBlank" runat="server" CommandName="Sort" CommandArgument="Blank">
                                                    <%= Resource.Admin_MenuManager_InNewTab %>
                                                    <asp:Image ID="arrowBlank" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="cbBlankEdit" runat="server" Checked='<%# Eval("Blank") %>'></asp:CheckBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="SortOrder" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="160px" ItemStyle-Width="160px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lblSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                    <%= Resource.Admin_MenuManager_SortOrder %>
                                                    <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtSortOrderEdit" runat="server" Text='<%# Eval("SortOrder") %>'>
                                                </asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                            <EditItemTemplate>
                                                <%# "<a href=\"javascript:open_window('m_Menu.aspx?MenuID=" + Eval("ID") + "',750,700);\" class = 'editbtn showtooltip' title=\"" + Resource.Admin_MasterPageAdminMenu_EditLinks + "\" ><img src='images/editbtn.gif' style='border: none;' /></a>"%>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                    style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                                <asp:LinkButton ID="buttonDelete" runat="server"
                                                    CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteItem" CommandArgument='<%# Eval("ID")%>'
                                                    data-confirm="<%$Resources:Resource, Admin_MenuManager_DeleteItem %>"
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                    style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                    <EmptyDataTemplate>
                                        <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                            <%=Resource.Admin_Catalog_NoRecords%>
                                        </div>
                                    </EmptyDataTemplate>
                                </adv:AdvGridView>
                                <div style="border-top: 1px #c9c9c7 solid;">
                                </div>
                                <table class="results2">
                                    <tr>
                                        <td style="width: 157px; padding-left: 6px;">
                                            <%=Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage" Style="display: inline-block;"
                                                runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>20</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>100</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                                UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
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
                    <input type="hidden" id="SelectedIds" name="SelectedIds" />
                </div>
            </div>
        </li>
    </ul>
    <script type="text/javascript">
        var base$TreeView_PopulateNodeDoCallBack = this.TreeView_PopulateNodeDoCallBack;
        var base$TreeView_ProcessNodeData = this.TreeView_ProcessNodeData;
        this.TreeView_ProcessNodeData = function (result, context) {
            //alert( "after load " );
            hide_wait_for_node(context.node);
            var r = base$TreeView_ProcessNodeData(result, context);
            <% foreach (EMenuType menuType in Enum.GetValues(typeof(EMenuType)))
        { %>
            setup<%=menuType.ToString() %>HoverPanel();
            <% } %>
            return r;
        };
        this.TreeView_PopulateNodeDoCallBack = function (context, param) {
            //alert( "before load " );
            show_wait_for_node(context.node);
            return base$TreeView_PopulateNodeDoCallBack(context, param);
        };

        function hide_wait_for_node(node) {
            if (node.wait_img) {
                node.removeChild(node.wait_img);
            }
        }

        function show_wait_for_node(node) {
            var wait_img = document.createElement("IMG");
            wait_img.src = "images/loader.gif";
            wait_img.border = 0;
            node.wait_img = wait_img;
            node.appendChild(wait_img);
        }

        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
            
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });

        function esc_press(D) {
            D = D || window.event;
            var A = D.keyCode;
            if (A == 27) {
                HideModalPopupNews();
                HideModalPopupProduct();
                HideModalPopupCategory();
                HideModalPopupAux();
            }
        }
        document.onkeydown = esc_press;

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
                        if (confirm("<%= Resource.Admin_MenuManager_Confirm%>"))
                            document.getElementById('<%=lbDeleteSelected.ClientID%>').click();
                        break;
                }
            });
        });

        var timeOut;
        function Darken() {
            timeOut = setTimeout('document.getElementById("inprogress").style.display = "block";', 1000);
        }

        function Clear() {
            clearTimeout(timeOut);

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
        });
    </script>
</asp:Content>
