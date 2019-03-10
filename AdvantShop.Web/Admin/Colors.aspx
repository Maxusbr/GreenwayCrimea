<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Colors" CodeBehind="Colors.aspx.cs" %>

<%@ Import Namespace="AdvantShop" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/admin/UserControls/Catalog/AddProduct.ascx" TagName="AddProduct" TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript" src="js/ajaxfileupload.js"></script>
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <adv:AddProduct ID="addProduct" runat="server" />
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
            <li class="neighbor-menu-item"><a href="Catalog.aspx">
                <%= Resource.Admin_MasterPageAdmin_CategoryAndProducts %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="ProductsOnMain.aspx?type=New"
                class="neighbor-menu-lnk">
                <%= Resource.Admin_MasterPageAdminCatalog_FirstPageProducts%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=Best">
                            <%= Resource.Admin_MasterPageAdminCatalog_BestSellers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=New">
                            <%= Resource.Admin_MasterPageAdminCatalog_New %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=Sale">
                            <%= Resource.Admin_MasterPageAdminCatalog_Discount %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductLists.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ProductLists %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item dropdown-menu-parent selected"><a href="Properties.aspx">
                <%= Resources.Resource.Admin_MasterPageAdmin_Directory%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Properties.aspx"><%= Resource.Admin_MasterPageAdmin_ProductProperties%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Colors.aspx"><%= Resource.Admin_MasterPageAdmin_ColorDictionary%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Sizes.aspx"><%= Resource.Admin_MasterPageAdmin_SizeDictionary%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Tags.aspx">
                            <%= Resource.Admin_Tags_Header %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx">
                <%= Resource.Admin_MasterPageAdmin_Export%></a></li>
            <li class="neighbor-menu-item"><a href="ImportCSV.aspx">
                <%= Resource.Admin_MasterPageAdmin_Import%></a></li>
            <li class="neighbor-menu-item"><a href="Brands.aspx">
                <%= Resource.Admin_MasterPageAdmin_Brands%></a></li>
            <li class="neighbor-menu-item"><a href="Reviews.aspx">
                <%= Resource.Admin_MasterPageAdmin_Reviews%></a></li>
        </menu>
        <div class="panel-add">
            <%= Resource.Admin_MasterPageAdmin_Add %>
            <a href="javascript:void(0);" class="panel-add-lnk" data-add-product-call>
                <%= Resource.Admin_MasterPageAdmin_Product %></a>, 
            <a href="m_Category.aspx?ParentCategoryID=<%=Request["categoryid"] ?? "0" %>" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Category %></a>
        </div>
    </div>
    <div class="content-own">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_ColorsDictionary_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ColorsDictionary_ListColors %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="Label1" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ColorsDictionary_CatalogImageSize %>"></asp:Label>: <%= SettingsPictureSize.ColorIconWidthCatalog + "x" + SettingsPictureSize.ColorIconHeightCatalog + " px" %></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="Label2" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ColorsDictionary_DetailsImageSize %>"></asp:Label>: <%= SettingsPictureSize.ColorIconWidthDetails + "x" + SettingsPictureSize.ColorIconHeightDetails + " px" %></td>
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
                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddColor" runat="server" Text="<%$ Resources:Resource, Admin_ColorsDictionary_AddColor %>" ValidationGroup="0" OnClick="btnAddColor_Click" />
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
                                    <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="55">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtNameFilter" Width="99%" runat="server" TabIndex="11" />
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtColorCodeFilter" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 300px;">
                                    <div style="width: 120px; font-size: 0; line-height: 0"></div>
                                </td>
                                <td style="width: 190px;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSortOrderFilter" Width="99%" runat="server"
                                        TabIndex="13" />
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
                            <asp:AsyncPostBackTrigger ControlID="btnAddColor" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_ColorsDictionary_Confirmation %>"
                                CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                                DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_RowCommand"
                                OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting" ShowFooter="false">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="60px" HeaderStyle-Width="60px"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <div style="width: 60px; font-size: 0px">
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ColorName" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="ColorName">
                                                <%=Resources.Resource.Admin_Catalog_Name %>
                                                <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("ColorName") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewName" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ColorCode" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="200" HeaderStyle-Width="200">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbColorCode" runat="server" CommandName="Sort" CommandArgument="ColorCode">
                                                <%=Resources.Resource.Admin_ColorsDictionary_ColorCode %>
                                                <asp:Image ID="arrowColorCode" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtColorCode" runat="server" Text='<%# Eval("ColorCode") %>' MaxLength="7" Style="display: none;"></asp:TextBox>
                                            <span class="jPicker">
                                                <span class="Icon">
                                                    <span class="Color" style="background: <%# Eval("ColorCode") %>">&nbsp;</span>
                                                    <span class="Alpha">&nbsp;</span>
                                                    <span class="Image" title="Click To Open Color Picker" style="background-image: url(js/plugins/jpicker/images/picker.gif);">&nbsp;</span>
                                                    <span class="Container">&nbsp;</span>
                                                </span>
                                            </span>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewColorCode" runat="server" Text="#ffffff" MaxLength="7" Style="display: none;"></asp:TextBox>
                                            <span class="jPicker">
                                                <span class="Icon">
                                                    <span class="Color" style="background: #ffffff">&nbsp;</span>
                                                    <span class="Alpha">&nbsp;</span>
                                                    <span class="Image" title="Click To Open Color Picker" style="background-image: url(js/plugins/jpicker/images/picker.gif);">&nbsp;</span>
                                                    <span class="Container">&nbsp;</span>
                                                </span>
                                            </span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ColorPicture" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="300" HeaderStyle-Width="300">
                                        <HeaderTemplate>
                                            <%=Resources.Resource.Admin_ColorsDictionary_ColorIcon %>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# SQLDataHelper.GetString(Eval("PhotoName")).IsNotEmpty() ? ("<img src='" + FoldersHelper.GetImageColorPath(ColorImageType.Details, Eval("PhotoName").ToString(), true) +  "'/>") : string.Empty%>
                                            <input type="file" id="<%# "newPicture_" + Eval("ID") %>" name="<%# "newPicture_" + Eval("ID") %>">
                                            <a href="javascript:void(0);" onclick="<%# "uploadPicture(" + Eval("ID") + ")" %>"><%=Resource.Admin_ColorsDictionary_Upload %></a>
                                            <a href="javascript:void(0);" onclick="<%# "deletePicture(" + Eval("ID") + ")" %>"><%=Resource.Admin_ColorsDictionary_Delete %></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="SortOrder" ItemStyle-Width="200" HeaderStyle-Width="200">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                <%=Resources.Resource.Admin_Properties_SortOrder%>
                                                <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtSortOrder" Width="95%" Text='<%# Bind("SortOrder") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="txtNewSortOrder" Width="95%" CssClass="add" Text=''></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="25px" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <img src="images/editbtn.gif" class="editbtn showtooltip" style="border: none;" alt='' />
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibAddColor" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddColor"
                                                ToolTip="<%$ Resources:Resource, Admin_Add  %>" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="25px" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Left">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                                CssClass="deletebtn showtooltip" CommandName="DeleteColor" CommandArgument='<%# Eval("ID")%>'
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' Visible='<%# CanDelete(Convert.ToInt32(Eval("ID"))) %>' />
                                               <%# !CanDelete((int)Eval("ID")) ? "<img src=\"images/deletebtn_disabled.png\" title=\"Удаление невозможно, используется в товарах\"/>": string.Empty %>
                                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                                ConfirmText="<%$ Resources:Resource, Admin_ColorsDictionary_Confirm %>">
                                            </ajaxToolkit:ConfirmButtonExtender>
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_Cancel %>" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Cancel  %>" />
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
                                        <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                            runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem Selected="True">20</asp:ListItem>
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
                                                <%=Resources.Resource.Admin_Catalog_PageNum %>&nbsp;<asp:TextBox ID="txtPageNum"
                                                    runat="server" Width="30" /></span>
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
            var picker;

            $(function () {

                var spanCurrent, inpCurrent;

                var inpHide = $('<input type="text" value="ffffff" style="display:none;" />');

                var funcUpdate = function (color, context) {
                    if (color.val() == null) {
                        inpCurrent.val('transparent');
                        spanCurrent.find('.Color').css('backgroundColor', 'transparent');
                    }
                    else {
                        inpCurrent.val('#' + color.val().hex);
                        spanCurrent.find('.Color').css('backgroundColor', '#' + color.val().hex);
                    }
                };

                $(document.body).append(inpHide);

                picker = inpHide.jPicker({}, funcUpdate, funcUpdate, null);

                inpHide.next('span.jPicker').hide();

                $(document.body).on('click', 'span.jPicker', function () {
                    spanCurrent = $(this);
                    inpCurrent = spanCurrent.prevAll('input[type="text"]');
                    $.jPicker.List[0].color.active.val('hex', inpCurrent.val(), inpCurrent);
                    $.jPicker.List[0].show();


                    var spanPos = spanCurrent.offset(),
                        _picker = $('.jPicker.Container'),
                        pos = {};

                    if (spanPos.top + _picker.outerHeight() > $(window).height()) {
                        pos.top = spanPos.top - _picker.outerHeight();
                    } else {
                        pos.top = spanPos.top + spanCurrent.outerHeight();
                    }

                    if (spanPos.left + _picker.outerWidth() > $(window).width()) {
                        pos.left = spanPos.left - _picker.outerWidth();
                    } else {
                        pos.left = spanPos.left + spanCurrent.outerWidth();
                    }

                    _picker.css(pos);

                    $('span.jPicker').removeClass('picker-selected');

                    spanCurrent.addClass('picker-selected');

                });
            });

            function setupTooltips() {
                $(".showtooltip").tooltip({
                    showURL: false
                });
            }

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });


            function uploadPicture(colorid) {

                var arrId = [];
                arrId.push("newPicture_" + colorid);

                $.ajaxFileUpload({
                    url: '../admin/HttpHandlers/Catalog/UploadColorPicture.ashx?ColorId=' + colorid,
                    secureuri: false,
                    fileElementId: arrId,
                    dataType: 'json',
                    success: function (data, status) {
                        if (data != null && data.msg == "error") {
                            alert(data.error);
                        } else {
                            window.location.reload();
                        }
                    },
                    error: function (data, status, e) {
                        alert(e);
                    }
                });
            }


            function deletePicture(colorid) {

                $.ajax({
                    url: 'HttpHandlers/Catalog/DeleteColorPicture.ashx?ColorId=' + colorid,
                    dataType: 'json',
                    cache: false,
                    success: function (data, status) {
                        if (data != null && data.msg == "error") {
                            alert(data.error);
                        } else {
                            window.location.reload();
                        }
                    },
                    error: function (data, status, e) {
                        alert(e);
                    }
                });
            }


        </script>
    </div>
</asp:Content>
