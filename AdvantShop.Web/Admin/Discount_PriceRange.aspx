<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="false"
    Inherits="Admin.DiscountsPriceRange" Codebehind="Discount_PriceRange.aspx.cs" %>

<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
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
                                <td align="center">
                                    <img src="images/ajax-loader.gif" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="color: #0D76B8;">
                                    <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="content-top">
                <menu class="neighbor-menu neighbor-catalog">
                    <li class="neighbor-menu-item selected"><a href="Discount_PriceRange.aspx">
                        <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
                    <li class="neighbor-menu-item"><a href="Coupons.aspx">
                        <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
                    <li class="neighbor-menu-item"><a href="Certificates.aspx">
                        <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
                    <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                        <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
                    <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                        <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
                    <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                        <%= Resources.Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
                    <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
                        <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                        <div class="dropdown-menu-wrap">
                            <ul class="dropdown-menu">
                                <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_Voting %>
                                </a></li>
                                <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_VotingHistory %>
                                </a></li>
                            </ul>
                        </div>
                    </li>
                    <li class="neighbor-menu-item"><a href="Statistics.aspx">
                        <%= Resources.Resource.Admin_Statistics_Header %></a></li>
                    <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                        <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
                    <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                        <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
                </menu>
            </div>
            <div class="content-own">
                <center>
                <table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px; padding-right: 7px;">
                    <tbody>
                        <tr>
                            <td style="width: 72px;">
                                <img src="images/orders_ico.gif" alt="" />
                            </td>
                            <td>
                                <asp:Label ID="lblPriceRange" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Discount_PriceRange_Header %>"></asp:Label>
                                <br />
                                <asp:Label ID="lblPriceRangeName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Discount_PriceRange_SubHeader %>"></asp:Label>
                                <div style="margin: 10px 0px">
                                    <asp:CheckBox ID="chkModuleEnabled" runat="server" Text="<%$ Resources:Resource, Admin_Discount_PriceRange_PluginModule %>"
                                        OnCheckedChanged="chkModuleEnabled_CheckedChanged" AutoPostBack="True" />
                                    <br />
                                    <asp:Localize ID="Localize1" runat="server" Text="<%$Resources:Resource, Admin_Discount_PriceRange_CustomerGroupMessage %>"></asp:Localize>
                                </div>
                                <div style="margin:10px 0">
                                    <a href="DiscountsByDatetime.aspx"><%= Resources.Resource.Admin_Discount_PriceRange_DiscountByTimeMessage %></a>
                                </div>
                            </td>
                            <td>
                                <div class="btns-main">
                                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnAdd" runat="server" Text="<%$ Resources:Resource, Admin_PriceRange_Add %>"  ValidationGroup="0" OnClick="btnAdd_Click" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </center>
                <div>
                    <div id="gridTable" runat="server">
                        <center>
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
                                        <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                            height: 20px;" />
                                        <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                            OnClick="lbDeleteSelected_Click" />
                                        <asp:LinkButton ID="lbChangeStatus" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_ChangeStatus %>" />
                                    </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                                        |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected %></span></span></span>
                                </td>
                                <td align="right" class="selecteditems">
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
                                <td style="width: 8px;">
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                                <table class="filter" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td colspan="7">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 74px; text-align: center;" rowspan="2">
                                            <div style="height: 0px; font-size: 0px; width: 74px">
                                            </div>
                                            <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                                Width="65px">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 45%" rowspan="2">
                                            <div style="height: 0px; font-size: 0px; width: 134px">
                                            </div>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtUserName" Width="100%" runat="server"
                                                TabIndex="11" />
                                        </td>
                                        <td rowspan="2" style="width: 45%">
                                            <div style="height: 0px; font-size: 0px; width: 30%">
                                            </div>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtText" Width="100%" runat="server" TabIndex="11" />
                                        </td>
                                        <td style="width: 100px; padding-right: 10px;">
                                            <div style="height: 0px; font-size: 0px; width: 100px;">
                                            </div>
                                            <center>
                                                <asp:Button ID="btnFilter" runat="server" CssClass="btn" Width="43px" OnClientClick="javascript:FilterClick();"
                                                    TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                                <asp:Button ID="btnReset" runat="server" CssClass="btn" Width="43px" OnClientClick="javascript:ResetFilter();"
                                                    TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                        </td>
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
                                    <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Discount_PriceRange_Confirmation %>"
                                        CssClass="tableview" GridLines="None" DataKeyNames="" OnSorting="grid_Sorting"
                                        OnRowCommand="grid_RowCommand" ShowFooterWhenEmpty="true" ShowHeaderWhenEmpty="true">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="ID" Visible="false" HeaderStyle-Width="100%">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 70px; font-size: 0px;">
                                                    </div>
                                                    <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall headerCb" runat="server"
                                                        onclick="javascript:SelectVisible(this.checked);" Style="margin-left: 0px;" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# (bool) Eval("IsSelected")? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="PriceRange" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="45%">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 130px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbOrderPriceRange" runat="server" CommandName="Sort" CommandArgument="PriceRange">
                                                        <%= Resources.Resource.Admin_Discount_PriceRange_Discount_PriceRange%>
                                                        <asp:Image ID="arrowPriceRange" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriceRangeBind" runat="server" Text='<%# AdvantShop.Helpers.SQLDataHelper.GetFloat(Eval("PriceRange")).ToString("F2") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPriceRange" runat="server" Text='<%# AdvantShop.Helpers.SQLDataHelper.GetFloat(Eval("PriceRange")).ToString("F2") %>'
                                                        Width="99%"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewPriceRange" CssClass="add" runat="server" Width="99%"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="PercentDiscount" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="45%">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 130px; font-size: 0px;">
                                                    </div>
                                                    <asp:LinkButton ID="lbOrderPercentDiscount" runat="server" CommandName="Sort" CommandArgument="PercentDiscount">
                                                        <%= Resources.Resource.Admin_Discount_PriceRange_PercentDiscount %>
                                                        <asp:Image ID="arrowPercentDiscount" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPercentDiscount" runat="server" Text='<%# Eval("PercentDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPercentDiscount" runat="server" Text='<%# Eval("PercentDiscount") %>'
                                                        Width="99%"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewPercentDiscount" CssClass="add" runat="server" Width="99%"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="100" AccessibleHeaderText="Buttons" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="height: 0px; width: 100px; font-size: 0px;">
                                                    </div>
                                                </HeaderTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' 
                                                                    data-confirm="<%$ Resources: Resource, Admin_Discount_PriceRange_QDelete %>"
                                                                    CommandName="DeleteDiscount" CommandArgument='<%# Eval("ID") %>' />
                                                    <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                        src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                        style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update %>' />
                                                    <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                        src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                        style="display: none" title='<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>' />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="ibAdd" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddRange"
                                                        ToolTip="<%$ Resources:Resource, Admin_Property_Add  %>" />
                                                    <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                        CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Property_CancelAdd  %>" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="header" />
                                        <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                        <AlternatingRowStyle CssClass="row2 propertiesRow_25 readonlyrow" />
                                        <EmptyDataTemplate>
                                            <center style="margin-top: 20px; margin-bottom: 20px;">
                                                <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_EmptyDataPage %>"></asp:Localize>
                                            </center>
                                        </EmptyDataTemplate>
                                    </adv:AdvGridView>
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
                                            <td align="center">
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
                        <input type="hidden" id="SelectedIds" name="SelectedIds" />
                        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    </center>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        $(document).ready(function () {
            $("input.showtooltip").tooltip({
                showURL: false
            });
        })

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
        })

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
                        var r = confirm("<%= Resources.Resource.Admin_Discount_PriceRange_Confirmation%>");
                        if (r) __doPostBack('<%= lbDeleteSelected.UniqueID %>', '');
                        break;
                }
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { selectCange() });


    </script>
</asp:Content>
