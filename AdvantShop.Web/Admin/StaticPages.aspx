<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.StaticPages" Codebehind="StaticPages.aspx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
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
                        var r = confirm("<%= Resources.Resource.Admin_StaticPage_Confirm%>");
                        if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                    case "setActive":
                        var r = confirm("<%= Resources.Resource.Admin_StaticPage_ConfirmEnable%>");
                        if (r) document.getElementById('<%=lbSetActive.ClientID%>').click();
                        break;
                    case "setDeactive":
                        var r = confirm("<%= Resources.Resource.Admin_StaticPage_ConfirmDisable%>");
                        if (r) document.getElementById('<%=lbSetDeactive.ClientID%>').click();
                        break;
                    case "changeParent":
                        var r = confirm("<%= Resources.Resource.Admin_StaticPage_ConfirmChangeParent%>");
                        if (r) {
                            document.getElementById('<%=lbChangeParent.ClientID%>').click();
                        }
                        break;
                }
            });
        });

        function ChangeParent() {
            if ($("#commandSelect option:selected").val() == "changeParent") {
                $("#<%= ddlParentPages.ClientID %>").show();
            } else {
                $("#<%= ddlParentPages.ClientID %>").hide();
            }
        }  

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        
            <div id="curtain" class="opacitybackground">
                &nbsp;</div>
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
                                <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
            <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Menu.aspx">
                <%= Resource.Admin_MasterPageAdmin_MainMenu%></a></li>
            <li class="neighbor-menu-item"><a href="NewsAdmin.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsMenuRoot%></a></li>
            <li class="neighbor-menu-item"><a href="NewsCategory.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsCategory%></a></li>
            <li class="neighbor-menu-item"><a href="Carousel.aspx">
                <%= Resource.Admin_MasterPageAdmin_Carousel%></a></li>
            <li class="neighbor-menu-item selected"><a href="StaticPages.aspx">
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
        <div class="content-own">
        <%--<div style="padding-left: 10px; padding-right: 10px">--%>
        <div style="width: 100%">
            <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td style="vertical-align: top;">
                        <div id="leftPanel" class="leftPanel dvLeftPanel">
                            <table border="0" cellspacing="0" cellpadding="0" class="catalog_part catelog_list">
                                <tr style="height: 28px;">
                                    <td style="width: 25px;">
                                        <a href="StaticPages.aspx">
                                            <img src="images/folder.gif" alt="" style="border-style: none" />
                                        </a>
                                    </td>
                                    <td style="width: 194px;" class="catalog_label">
                                        <asp:Localize ID="Localize7" runat="server" Text='<%$ Resources:Resource, Admin_StaticPage_lblSubMain %>'></asp:Localize>
                                    </td>
                                    <td style="width: 22px;" align="right">
                                        <asp:ImageButton ID="ibtnAddInRoot" runat="server" ImageUrl="images/gplus.gif" onmouseover="this.src='images/bplus.gif'"
                                            onmouseout="this.src='images/gplus.gif';" ToolTip="<%$ Resources:Resource, Admin_StaticPage_AddPage %>"
                                            OnClick="ibtnAddInRoot_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div class="catalog_part catelog_listContent">
                                <asp:UpdatePanel ID="UpdatePanelTree" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <adv:CommandTreeView ID="tree" runat="server" NodeWrap="True" ShowLines="true" CssClass="AdminTree_MainClass"
                                            OnTreeNodePopulate="tree_TreeNodePopulate" OnTreeNodeCommand="tree_TreeNodeCommand">
                                            <ParentNodeStyle Font-Bold="False" />
                                            <SelectedNodeStyle ForeColor="#027dc2" Font-Bold="true" HorizontalPadding="0px" VerticalPadding="0px" />
                                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                NodeSpacing="0px" VerticalPadding="0px" />
                                        </adv:CommandTreeView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </td>
                    <%=RenderSplitter()%>
                    <td style="width: 100%; vertical-align: top; padding: 0px 0 0 10px;">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tbody>
                                <tr>
                                    <td style="width: 72px;">
                                        <img src="images/orders_ico.gif" alt="" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_lblMain %>"></asp:Label><br />
                                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_lblSubMain %>"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div>
                            <div class="btns-main">
                                <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddPage" runat="server" Text="<%$ Resources:Resource, Admin_HeadCmdInsert %>" OnClick="btnAddPage_Click" />
                            </div>
                            <div style="height: 10px">
                            </div>
                            <table style="width: 99%;" class="massaction">
                                <tr>
                                    <td>
                                        <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                                        </span><span style="display: inline-block">
                                            <select id="commandSelect" onchange="ChangeParent();">
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
                                                <option value="setActive">
                                                    <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetActive %>"></asp:Localize>
                                                </option>
                                                <option value="setDeactive">
                                                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"></asp:Localize>
                                                </option>
                                                <option value="changeParent">
                                                    <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ChangeParent %>"></asp:Localize>
                                                </option>
                                            </select>
                                            <asp:DropDownList ID="ddlParentPages" runat="server" DataTextField="PageName" DataValueField="StaticPageID"
                                                OnDataBound="ddlParentPagesOnDataBound" DataSourceID="sdsGroup" Style="display: none;
                                                width: 120px;">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="sdsGroup" runat="server" OnInit="sds_Init" SelectCommand="SELECT PageName, StaticPageID FROM [CMS].[StaticPage]">
                                            </asp:SqlDataSource>
                                            <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                                height: 20px;" />
                                            <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                                OnClick="lbDeleteSelected_Click" />
                                            <asp:LinkButton ID="lbSetActive" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetActive %>"
                                                OnClick="lbSetActive_Click" />
                                            <asp:LinkButton ID="lbSetDeactive" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"
                                                OnClick="lbSetDeactive_Click" />
                                            <asp:LinkButton ID="lbChangeParent" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"
                                                OnClick="lbChangeParent_Click" />
                                        </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                                            |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span>
                                        </span>
                                        <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td class="selecteditems" style="text-align: center;">
                                        <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
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
                                    <td style="width: 8px;">
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                                    <table class="filter" cellpadding="0" cellspacing="0">
                                        <tr style="height: 5px;">
                                            <td colspan="6">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60px; text-align: center;">
                                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                    Width="55">
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="filtertxtbox" ID="txtPageName" Width="99%" runat="server" />
                                            </td>
                                            <td style="text-align: center; width: 120px;">
                                                <asp:DropDownList ID="ddlEnabled" TabIndex="18" CssClass="dropdownselect" runat="server">
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                        Value="any" />
                                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 100px;">
                                                <asp:TextBox CssClass="filtertxtbox" ID="txtSortOrder" Width="99%" runat="server"
                                                    TabIndex="12" />
                                            </td>
                                            <td style="width: 100px;">
                                                <!--TODO date filter-->
                                            </td>
                                            <td style="width: 60px; text-align: center;">
                                                <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                                    TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                                <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                                    TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;" colspan="6">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_StaticPage_Confirmation %>"
                                            CssClass="tableview" Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand"
                                            OnSorting="grid_Sorting" ShowFooter="false">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="60px" HeaderStyle-Width="60px"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <div style="width: 60px; height: 0px; font-size: 0px">
                                                        </div>
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
                                                <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbOrderStaticPageID" runat="server" CommandName="Sort" CommandArgument="ID">
                                                            <%=Resources.Resource.Admin_StaticPage_AuxPageID%>
                                                            <asp:Image ID="arrowStaticPageID" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lStaticPageID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="PageName" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbOrderPageName" runat="server" CommandName="Sort" CommandArgument="PageName">
                                                            <%=Resources.Resource.Admin_StaticPage_PageName%>
                                                            <asp:Image ID="arrowPageName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtPageNameBind" runat="server" Text='<%# Eval("PageName") %>' Width="99%"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lPageName" runat="server" Text='<%# Bind("PageName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewPageName" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="Enabled" ItemStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbOrderEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                                            <div style="float: left; width: 90px; text-align: center;">
                                                                <%=Resources.Resource.Admin_StaticPage_Enabled%>
                                                            </div>
                                                            <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif"
                                                                Style="margin-top: 7px; margin-left: 0px;" /></asp:LinkButton>
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
                                                <asp:TemplateField AccessibleHeaderText="SortOrder" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Width="90px">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbOrderSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                            <%=Resources.Resource.Admin_StaticPage_SortOrder%>
                                                            <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSortOrderBind" runat="server" Text='<%# Eval("SortOrder") %>' Width="99%"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lSortOrder" runat="server" Text='<%# Bind("SortOrder") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewSortOrder" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ModifyDate" HeaderText="<%$ Resources:Resource, Admin_ModifyDate %>">
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    <ControlStyle CssClass="Link" />
                                                    <HeaderStyle ForeColor="White" CssClass="GridView_HeaderStyle_BoundField" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbOrderModifyDate" runat="server" CommandName="Sort" CommandArgument="ModifyDate">
                                                            <%=Resources.Resource.Admin_ModifyDate%>
                                                            <asp:Image ID="arrowModifyDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#((DateTime)Eval("ModifyDate")).ToShortDateTime()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                                    <EditItemTemplate>
                                                        <a id="cmdlink" href='StaticPage.aspx?PageID=<%# Eval("ID") %>' class="editbtn showtooltip"
                                                            title='<%=Resources.Resource.Admin_MasterPageAdminCatalog_Edit%>'>
                                                            <img src="images/editbtn.gif" style="border: none;" /></a>
                                                        <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                            src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                            style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                                        <asp:LinkButton ID="buttonDelete" runat="server" 
                                                            CssClass="deletebtn showtooltip valid-confirm" CommandName="DeletePage" CommandArgument='<%# Eval("ID")%>'
                                                            data-confirm="<%$ Resources:Resource, Admin_StaticPage_Confirmation %>" 
                                                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                                        <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                            src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                            style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
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
                                                <td style="width: 157px; padding-left: 6px;">
                                                    <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                                        runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                                        <asp:ListItem>10</asp:ListItem>
                                                        <asp:ListItem>20</asp:ListItem>
                                                        <asp:ListItem>50</asp:ListItem>
                                                        <asp:ListItem>100</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center">
                                                    <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                                        UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                                </td>
                                                <td style="width: 157px; text-align: right; padding-right: 12px">
                                                    <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                                        <span style="color: #494949">
                                                            <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
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
                    </td>
                </tr>
            </table>
        </div>
        <%--</div>--%>
        <script type="text/javascript">
            function togglePanel() {
                if ($.cookie("isVisiblePanel") == "true" || $.cookie("isVisiblePanel") == "") {
                    $("div:.leftPanel").hide("fast");
                    $("div:#divHide").hide("fast");
                    $("div:#divShow").show("fast");
                    $.cookie("isVisiblePanel", "false", { expires: 7 });
                } else {
                    $("div:.leftPanel").show("fast");
                    $("div:#divHide").show("fast");
                    $("div:#divShow").hide("fast");
                    $.cookie("isVisiblePanel", "true", { expires: 7 });
                }
            }

            $(document).ready(function () {
                if ($.cookie("isVisiblePanel") != "false") {
                    showPanel();
                }
                setupTooltips();
            });

            function showPanel() {
                document.getElementById("leftPanel").style.display = "block";
                document.getElementById("divHide").style.display = "block";
                document.getElementById("divShow").style.display = "none";
            }
        </script>
    </div>
</asp:Content>
