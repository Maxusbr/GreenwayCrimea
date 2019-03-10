<%@ Page Language="C#" MasterPageFile="~/Admin/CatalogLayout.master" AutoEventWireup="true"
    Inherits="Admin.Catalog" EnableEventValidation="false" CodeBehind="Catalog.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/UserControls/Catalog/SiteNavigation.ascx" TagName="SiteNavigation" TagPrefix="adv" %>
<%@ Register Src="~/admin/UserControls/Catalog/CategoryView.ascx" TagName="AdminCategoryView" TagPrefix="adv" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script type="text/javascript">
        var base$TreeView_ProcessNodeData;
        var base$TreeView_PopulateNodeDoCallBack;

        function updatetree() {
            var win = document.parentWindow || document.defaultView;
            if (win.TreeView_ProcessNodeData != ProcessNodeData) {
                base$TreeView_ProcessNodeData = win.TreeView_ProcessNodeData;
                win.TreeView_ProcessNodeData = ProcessNodeData;
            }
            if (win.TreeView_PopulateNodeDoCallBack != PopulateNodeDoCallBack) {
                base$TreeView_PopulateNodeDoCallBack = win.TreeView_PopulateNodeDoCallBack;
                win.TreeView_PopulateNodeDoCallBack = PopulateNodeDoCallBack;
            }
        }

        function ProcessNodeData(result, context) {
            hide_wait_for_node(context.node);
            return base$TreeView_ProcessNodeData(result, context);
        }

        function PopulateNodeDoCallBack(context, param) {
            show_wait_for_node(context.node);
            return base$TreeView_PopulateNodeDoCallBack(context, param);
        }

        function hide_wait_for_node(node) {
            if (node.wait_img) {
                node.removeChild(node.wait_img);
            }
        }

        function show_wait_for_node(node) {
            var waitImg = document.createElement("IMG");
            waitImg.src = "images/loading.gif";
            waitImg.border = 0;
            node.wait_img = waitImg;
            node.appendChild(waitImg);
        }

        var _TreePostBack = false;

        function endRequest() {
            if (_TreePostBack) {
                updatetree();
                document.getElementById('mpeBehavior_backgroundElement').onclick = function () { $find('mpeBehavior').hide(); };
            }
            else {
                $(".photoinput").val("");
            }
        }

        function ATreeView_Select(sender, arg) {
            $("a.selectedtreenode").removeClass("selectedtreenode");
            $(sender).addClass("selectedtreenode");
            document.getElementById("TreeView_SelectedValue").value = arg;
            document.getElementById("TreeView_SelectedNodeText").value = sender.innerHTML;
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphContent" runat="Server">
    <h2 class="header-edit cat-name">
        <asp:Literal ID="lblCategoryName" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_lblMain %>" />
    </h2>
    <asp:HyperLink ID="hlEditCategory" runat="server" CssClass="lnk-edit" Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_FPanel_EditCategory %>" />
    <asp:Label runat="server" ID="lblSeparator" CssClass="cat-separator" Text="" />
    <asp:LinkButton ID="hlDeleteCategory" CssClass="lnk-remove valid-confirm" runat="server"
        Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_FPanel_DeleteCategory %>"
        OnClick="hlDeleteCategory_Click" />
    <adv:SiteNavigation ID="sn" runat="server" />
    <br />
    <asp:Label ID="lMessage" Style="float: left;" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
    <adv:AdminCategoryView runat="server" ID="adminCategoryView" />
    <asp:Panel runat="server" ID="pnlProducts">
        <div style="font-size: 14px; margin-top: 5px;" runat="server" id="pnlAddProduct">
            <a href="javascript:void(0)" class="panel-add-lnk" data-add-product-call><%= Resource.Admin_Catalog_CreateProduct %></a>
        </div>
        <h2 runat="server" id="productsHeader">
            <%= Resource.Admin_Catalog_Products %></h2>
        <ul class="justify panel-do-grid">
            <li class="justify-item panel-do-grid-item">
                <select id="commandSelect" onchange="ChangeSelect()">
                    <option value="selectAll">
                        <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                    </option>
                    <option value="unselectAll">
                        <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                    </option>
                    <option value="selectVisible">
                        <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                    </option>
                    <option value="unselectVisible">
                        <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                    </option>
                    <option value="deleteSelected">
                        <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                    </option>
                    <%if (ShowMethod == EShowMethod.Normal)
                      {%>
                    <option value="deleteSelectedFromCategory">
                        <%= Resource.Admin_Catalog_DeleteSelectedFromCategory%>
                    </option>
                    <%}%>
                    <option value="changeCategory">
                        <%= Resource.Admin_Catalog_ChangeCategory%>
                    </option>
                    <option value="activateSelected">
                        <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ActivateSelected %>"></asp:Localize>
                    </option>
                    <option value="disableSelected">
                        <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DisableSelected %>"></asp:Localize>
                    </option>
                </select>
                <label id="lblChangeStatus" style="display: none">
                    <input type="checkbox" id="chkChangeStatus" runat="server" checked="True" />
                    <%= Resource.Admin_Catalog_ChangeCategoryStatus %>
                </label>
                <a href="javascript:void(0)" class="btn btn-middle btn-action btn-do-grid" id="commandButton">
                    <%= Resource.Admin_Catalog_GO %></a>
                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" OnClick="lbDeleteSelected1_Click" />
                <asp:LinkButton ID="lbDeleteSelectedFromCategory" Style="display: none" runat="server" OnClick="lbDeleteSelectedFromCategory_Click" />
                <asp:LinkButton ID="lbActivateSelected" Style="display: none" runat="server" OnClick="lbActivateSelected_Click" />
                <asp:LinkButton ID="lbDisableSelected" Style="display: none" runat="server" OnClick="lbDisableSelected_Click" />
                <span class="panel-do-grid-selected-rows"><span id="selectedIdsCount" class="panel-do-grid-count"></span>
                    <%=Resource.Admin_Catalog_ItemsSelected%>
                </span></li>
            <li class="justify-item panel-do-grid-item">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <span class="subcategories-count-wrap">
                            <%= Resource.Admin_Catalog_ProductsFound %>:
                            <asp:Label ID="lblProducts" CssClass="foundrecords panel-do-grid-count" runat="server" Text="" />
                        </span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
        </ul>
        <div style="width: 100%; clear: both;">
            <div style="width: 100%">
                <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                    <table class="filter" cellpadding="0" cellspacing="0">
                        <tr style="height: 5px;">
                            <td colspan="9"></td>
                        </tr>
                        <tr>
                            <td style="width: 50px; text-align: center; font-size: 13px" rowspan="2">
                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server" Width="35px">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 40px; text-align: center; font-size: 13px" rowspan="2">
                                <asp:DropDownList ID="ddPhoto" TabIndex="10" CssClass="dropdownselect" runat="server" Width="35px">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_WithPhoto %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_WithoutPhoto %>" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 100px;" rowspan="2">
                                <div style="width: 100px; height: 0px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtArtNo" Width="99%" runat="server" TabIndex="11" />
                            </td>
                            <td rowspan="2">
                                <div style="width: 134px; height: 0px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="99%" runat="server" TabIndex="12" />
                            </td>
                            <td style="width: 130px; text-align: right; white-space: nowrap">
                                <span><span class="textfromto">
                                    <asp:Localize ID="Localize_Admin_Catalog_From0" Text="<%$ Resources:Resource, Admin_Catalog_From %>"
                                        runat="server"></asp:Localize>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtPriceFrom" runat="server"
                                            TabIndex="13" />
                                </span>
                            </td>
                            <td style="width: 110px; text-align: right; white-space: nowrap">
                                <span class="textfromto">
                                    <asp:Localize ID="Localize_Admin_Catalog_From1" Text="<%$ Resources:Resource, Admin_Catalog_From %>"
                                        runat="server"></asp:Localize>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtQtyFrom" runat="server"
                                            TabIndex="15" />
                            </td>
                            <td style="width: 80px; text-align: center; padding-left: 10px" rowspan="2">
                                <asp:DropDownList ID="ddlEnabled" TabIndex="17" CssClass="dropdownselect" runat="server">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <%if (ShowMethod == EShowMethod.Normal)
                              {%>
                            <td style="width: 105px; text-align: right; white-space: nowrap;">
                                <span class="textfromto">
                                    <%=Resource.Admin_Catalog_From%>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtSortOrderFrom"
                                        runat="server" TabIndex="19" />
                            </td>
                            <%
                                }%>
                            <td style="width: 100px; text-align: center;" rowspan="2">
                                <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();" TabIndex="23"
                                    Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();" TabIndex="24"
                                    Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px; text-align: right; white-space: nowrap">
                                <span class="textfromto">
                                    <%=Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtPriceTo"
                                        runat="server" Font-Size="10px" Width="50" TabIndex="14" />
                            </td>
                            <td style="width: 110px; text-align: right; white-space: nowrap">
                                <span class="textfromto">
                                    <%=Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtQtyTo" runat="server"
                                        Font-Size="10px" Width="50" TabIndex="16" />
                            </td>
                            <% if (ShowMethod == EShowMethod.Normal)
                               {%>
                            <td style="width: 105px; text-align: right; white-space: nowrap">
                                <span class="textfromto">
                                    <%=Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtSortOrderTo"
                                        runat="server" Font-Size="10px" Width="50" TabIndex="20" />
                            </td>
                            <%
                                }%>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="9"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="grid" EventName="DataBinding" />
                    </Triggers>
                    <ContentTemplate>
                        <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                            CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>" CssClass="tableview"
                            DataFieldForEditURLParam="ID" DataFieldForImageDescription="BriefDescription" DataFieldForImagePath="PhotoName"
                            EditURL="Product.aspx?productid={0}" GridLines="None" TooltipImgCellIndex="2" TooltipTextCellIndex="5"
                            ReadOnlyGrid="True" OnSorting="grid_Sorting" OnRowCommand="grid_RowCommand" OnRowDataBound="grid_RowDataBound">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="Id" Visible="false" HeaderStyle-Width="50px">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label0" runat="server" Text='<%# "Product_" + Eval("ID") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#(bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />"%>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# "Product_" + Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Photo" HeaderStyle-Width="30px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# GetImageItem(SQLDataHelper.GetString(Eval("PhotoName")))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="ArtNo" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbOrderId" runat="server" CommandName="Sort" CommandArgument="ProductArtNo">
                                            <%=Resource.Admin_Catalog_StockNumber%>
                                            <asp:Image ID="arrowArtNo" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("ProductArtNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbOrderCategory" runat="server" CommandName="Sort" CommandArgument="Name">
                                            <%=Resource.Admin_Catalog_Name%>
                                            <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <div style="min-width: 250px;">
                                            <asp:TextBox ID="txtCatalogName" runat="server" Text='<%# Eval("Name") %>' Width="99%" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Categories" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=Resource.Admin_Catalog_Categories%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#  SQLDataHelper.GetInt(Eval("ProductCategoriesCount")) > 0 ? "<img src='images/category_ico.gif' class='txttooltip' abbr=\"" + AdvantShop.Catalog.ProductService.CreateTooltipContent(SQLDataHelper.GetInt(Eval("ID"))) + "\"/>" : ""%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Price" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbPrice" runat="server" CommandName="Sort" CommandArgument="Price">
                                            <%=Resource.Admin_Catalog_Price%>
                                            <asp:Image ID="arrowPrice" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrice" runat="server" Visible='<%# (int)Eval("OffersCount") == 1 %>' Text='<%# String.Format("{0:##,##0.00}", Eval("Price")) %>' Width="80%"  Style="text-align: center;" />
                                        <asp:Label ID="Label1" runat="server" Visible='<%# (int)Eval("OffersCount") != 1 %>' Text='<%# Eval("CurrencyIso3")!= DBNull.Value &&  Eval("Price")!= DBNull.Value ? PriceFormatService.FormatPrice(Convert.ToSingle(Eval("Price")),Convert.ToSingle(Eval("CurrencyValue")),Convert.ToString(Eval("Code")),Convert.ToString(Eval("CurrencyIso3")),Convert.ToBoolean(Eval("IsCodeBefore"))):String.Format("{0:##,##0.00}", Eval("Price")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbOrderInternalName" runat="server" CommandName="Sort" CommandArgument="Amount">
                                            <%=Resource.Admin_Catalog_Qty%>
                                            <asp:Image ID="arrowAmount" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" runat="server" Text='<%# SQLDataHelper.GetFloat(Eval("Amount")) %>' Visible='<%# (int)Eval("OffersCount") == 1 %>' Width="80%" Style="text-align: center;" />
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# SQLDataHelper.GetFloat(Eval("Amount")) %>' Visible='<%# (int)Eval("OffersCount") != 1 %>' Width="80%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Enabled" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                            <%=Resource.Admin_CatalogLinks_Active%>
                                            <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Enabled") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Enabled") %>' Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="SortOrder" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="95px">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                            <%=Resource.Admin_Catalog_SortOrder%>
                                            <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Bind("SortOrder") %>' size="2" Style="text-align: center;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lSortOrder" runat="server" Text='<%# Bind("SortOrder") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Buttons" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <a id="cmdlink" runat="server" href='' class="editbtn showtooltip" title='<%$ Resources:Resource,Admin_MasterPageAdminCatalog_Edit%>'>
                                            <img src="images/editbtn.gif" style="border: none;" alt="" /></a>
                                        <%if (ShowMethod == EShowMethod.Normal)
                                          {%>
                                        <asp:ImageButton ID="buttonDeleteLink" runat="server" ToolTip="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_DeleteFromCategory%>"
                                            data-confirm="<%$ Resources:Resource, Admin_Product_ConfirmDeletingProductFormCategory%>"
                                            ImageUrl="images/excludebtn.png" CssClass="deletebtn showtooltip valid-confirm" CommandName="Deletelink" CommandArgument='<%# Eval("ID") %>' />
                                        <%}%>
                                        <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm" ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>'
                                            CommandArgument='<%# Eval("ID") %>'>
                                            <asp:Image ID="Image1" ImageUrl="images/deletebtn.png" runat="server" />
                                        </asp:LinkButton>
                                        <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image" src="images/updatebtn.png"
                                            onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                            style="display: none" title='<%= Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                        <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image" src="images/cancelbtn.png"
                                            onclick="row_canceledit($(this).parent().parent()[0]); return false;" style="display: none"
                                            title='<%=Resource.Admin_MasterPageAdminCatalog_Cancel%>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                    </ItemTemplate>
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
                        <table class="results2">
                            <tr>
                                <td style="width: 157px; padding-left: 6px;">
                                    <asp:Localize ID="Localize_Admin_Catalog_ResultPerPage" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ResultPerPage %>"></asp:Localize>:&nbsp;<asp:DropDownList
                                        ID="ddRowsPerPage" runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: center;">
                                    <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7" UseHref="false"
                                        OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
                                </td>
                                <td style="width: 157px; text-align: right; padding-right: 12px">
                                    <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                        <span style="color: #494949">
                                            <%=Resource.Admin_Catalog_PageNum%>&nbsp; <span class="input-wrap">
                                                <asp:TextBox ID="txtPageNum" runat="server" Width="30" /></span></span>
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
    </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnChangeProductCategory" />
        </Triggers>
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="mpeTree2" runat="server" PopupControlID="pTree2" TargetControlID="hhl2"
                BackgroundCssClass="blackopacitybackground" CancelControlID="btnCancelParent2" BehaviorID="ModalBehaviour2">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HyperLink ID="hhl2" runat="server" Style="display: none;" />
            <asp:Panel runat="server" ID="pTree2" CssClass="modal-admin">
                <div style="text-align: center;">
                    <table style="margin-top: 13px;">
                        <tbody>
                            <tr>
                                <td>
                                    <span style="font-size: 11pt;">
                                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>"></asp:Localize></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="tree2" />
                                            <asp:AsyncPostBackTrigger ControlID="lbChangeCategory" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div style="height: 360px; width: 450px; overflow: scroll; background-color: White; text-align: left">
                                                <asp:TreeView ID="tree2" ForeColor="Black" SelectedNodeStyle-BackColor="Blue" PopulateNodesFromClient="true"
                                                    OnSelectedNodeChanged="OnSelectedNodeChanged2" runat="server" ShowLines="True" ExpandImageUrl="images/loading.gif"
                                                    BackColor="White" OnTreeNodePopulate="PopulateNode2" >
                                                    <SelectedNodeStyle BackColor="Yellow" />
                                                </asp:TreeView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:LinkButton ID="lbChangeCategory" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelectedFromCategory %>"
                                        OnClick="lbChangeCategory_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 36px; text-align: right; vertical-align: bottom;">
                                    <asp:Button ID="btnChangeProductCategory" runat="server" Text="<%$ Resources: Resource, Admin_Catalog_SaveChangeCategory%>"
                                        OnClick="btnChangeProductCategory_Click" />
                                    <span></span>
                                    <asp:Button ID="btnCancelParent2" runat="server" Text="<%$ Resources: Resource, Admin_Cancel %>" Width="67" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupAdvantGrid(); });

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

        function setupAdvantGrid() {
            $(".imgtooltip").tooltip({
                delay: 10,
                showURL: false,
                bodyHandler: function () {
                    var imagePath = $(this).attr("abbr");
                    if (imagePath.length == 0) {
                        return "<div><span><%= Resource.Admin_Catalog_NoMiniPicture %></span></div>";
                    }
                    else {
                        return $("<img/>").attr("src", imagePath);
                    }
                }
            });

            $("tr[rowType='category']").click(function (a) {
                window.location = "Catalog.aspx?CategoryID=" + $(this).attr("element_id");
            });

            $("tr[rowType='goToUpperLevel']").click(function (a) {
                window.location = "Catalog.aspx?CategoryID=" + $(this).attr("element_id");
            });

            $("tr[rowType='category'] input[type='image']").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();
            });

            $("tr[rowType='category'] a.editbtn").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();

                window.location = 'm_Category.aspx?CategoryID=' + $(this).parent().parent().attr("element_id");
            });

            $("tr[rowType='category'] a.deletebtn").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();
            });

            $("tr[rowType='category'] td.checkboxcolumn").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();
            });

            $("tr[rowType='category'] input").css("cursor", "pointer");
        }

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
                        var r = confirm("<%= Resource.Admin_Catalog_Confirm%>");
                        if (r) { window.__doPostBack('<%=lbDeleteSelected.UniqueID%>', ''); $("#selectedIdsCount").text("0"); }
                        break;
                    case "deleteSelectedFromCategory":
                        var r = confirm("<%=Resource.Admin_Catalog_ConfirmDeleteFromCategory%>");
                        if (r) window.__doPostBack('<%=lbDeleteSelectedFromCategory.UniqueID%>', '');
                        break;
                    case "changeCategory":
                        if ($("#SelectedIds").val() != "") {
                            window.__doPostBack('<%=lbChangeCategory.UniqueID%>', '');
                            $("#selectedIdsCount").text("0");
                        } else {
                            alert("<%=Resource.Admin_Catalog_NoSelectedPositions%>");
                        }
                        break;
                    case "activateSelected":
                        window.__doPostBack('<%=lbActivateSelected.UniqueID%>', ''); 
                        break;
                    case "disableSelected":
                        window.__doPostBack('<%=lbDisableSelected.UniqueID%>', '');
                        break;
                }
            });
        });

        function ATreeView_Select(sender, arg) {
            $("a.selectedtreenode").removeClass("selectedtreenode");
            $(sender).addClass("selectedtreenode");
            document.getElementById("TreeView_SelectedValue").value = arg;
            document.getElementById("TreeView_SelectedNodeText").value = sender.innerHTML;
            return false;
        }

        function ChangeSelect() {
            var index = document.getElementById("commandSelect").selectedIndex;

            if (document.getElementById("commandSelect").options[index].text == '<%=Resource.Admin_Catalog_ChangeCategory%>') {
                document.getElementById('lblChangeStatus').style.display = "inline";
            } else {
                document.getElementById('lblChangeStatus').style.display = "none";
            }
        }

    </script>
</asp:Content>
