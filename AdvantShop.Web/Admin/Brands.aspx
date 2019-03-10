<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Codebehind="Brands.aspx.cs" Inherits="Admin.Brands" %>

<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/admin/UserControls/Catalog/AddProduct.ascx" TagName="AddProduct" TagPrefix="adv" %>
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
                        var r = confirm("<%= Resource.Admin_NewsAdmin_Confirm%>");
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
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
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
             <li class="neighbor-menu-item dropdown-menu-parent"><a href="Properties.aspx">
                <%= Resource.Admin_MasterPageAdmin_Directory%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Properties.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ProductProperties%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Colors.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ColorDictionary%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Sizes.aspx">
                            <%= Resource.Admin_MasterPageAdmin_SizeDictionary%>
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
            <li class="neighbor-menu-item selected"><a href="Brands.aspx">
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
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Brands_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Brands_SubHeader %>"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="width: 100%">
            <div>
                <div class="btns-main">
                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnAdd" runat="server" Text="<%$ Resources:Resource, Admin_HeadCmdInsert %>" OnClientClick="javascript:open_window('m_Brand.aspx',750,600);return false;" />
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
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                    height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                    OnClick="lbDeleteSelected_Click" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                                |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected%></span></span>
                            </span>
                            <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                        </td>
                        <td align="right" class="selecteditems">
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
                        <td style="width: 8px;">
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                        <table class="filter" cellpadding="0" cellspacing="0">
                            <tr style="height: 5px;">
                                <td colspan="7">
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
                                <td style="width: 150px; text-align: center;">
                                    <asp:DropDownList ID="ddlLogo" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="140">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                            Value="any" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <div style="width: 200px; height: 0px; font-size: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtTitle" Width="99%" runat="server" TabIndex="12" />
                                </td>
                                <td style="width: 200px;">
                                    <div style="width: 200px; height: 0px; font-size: 0px;">
                                    </div>
                                </td>
                                <td style="width: 140px; text-align: center;">
                                    <div style="width: 140px; height: 0px; font-size: 0px;">
                                    </div>
                                    <asp:DropDownList ID="ddlShowOnMainPage" TabIndex="18" CssClass="dropdownselect"
                                        runat="server">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                            Value="any" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 200px;">
                                    <div style="width: 200px; height: 0px; font-size: 0px;">
                                    </div>
                                </td>
                                <td style="width: 85px;">
                                    <div style="width: 85px; height: 0px; font-size: 0px;">
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
                                <td style="height: 5px;" colspan="7">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_NewsAdmin_Confirmation %>"
                                CssClass="tableview" Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand"
                                OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting" ShowFooter="false"
                                ShowFooterWhenEmpty="true" OnRowDataBound="grid_RowDataBound">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="BrandLogo" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <%= Resource.Admin_Brands_Logo %>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Image ID="imgLogo" runat="server" ImageUrl='<%# FoldersHelper.GetPath(FolderType.BrandLogo, Eval("BrandLogo") as string, true) %>'
                                                Visible='<%# GetImageVisible(Eval("BrandLogo")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="BrandName" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderBrandName" runat="server" CommandName="Sort" CommandArgument="BrandName">
                                                <%= Resource.Admin_Brands_Name %>
                                                <asp:Image ID="arrowBrandName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtBrandName" runat="server" Text='<%# Eval("BrandName") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lBrandName" runat="server" Text='<%# Bind("BrandName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ProductsCount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderNewsCategoryID" runat="server" CommandName="Sort" CommandArgument="ProductsCount">
                                                <%= Resource.Admin_Brands_ProductsCount %>
                                                <asp:Image ID="arrowProductsCount" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lProductsCount" runat="server" Text='<%# Eval("ProductsCount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Enabled" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                                <%= Resource.Admin_Brands_Enabled %>
                                                <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Bind("Enabled") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkShowOnMainPage" runat="server" Checked='<%# Bind("Enabled") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="SortOrder" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                <%= Resource.Admin_Brands_SortOrder %>
                                                <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Eval("SortOrder") %>' size="2"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lSortOrder" runat="server" Text='<%# Bind("SortOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <%# "<a href=\"javascript:open_window('m_brand.aspx?ID=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(Eval("ID").ToString())) + "',750,600);\" class = 'editbtn showtooltip'><img src='images/editbtn.gif' style='border: none;'  title="
                                                 + Resource.Admin_MasterPageAdminCatalog_Edit + " /></a>"%>
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                            <asp:LinkButton ID="buttonDelete" runat="server"
                                                CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteBrand" CommandArgument='<%# Eval("ID")%>'
                                                data-confirm="<%$ Resources:Resource, Admin_Brands_DeleteConfirm%>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 readonlyrow brandrow" />
                                <AlternatingRowStyle CssClass="row2 readonlyrow brandrow" />
                                <EmptyDataTemplate>
                                    <center style="margin-top: 20px; margin-bottom: 20px;">
                                        <%=Resource.Admin_Catalog_NoRecords%>
                                    </center>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%=Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
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
    </div>
</asp:Content>
