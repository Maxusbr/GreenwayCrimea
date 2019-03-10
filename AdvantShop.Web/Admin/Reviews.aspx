<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Reviews" CodeBehind="Reviews.aspx.cs" %>

<%@ Import Namespace="AdvantShop.CMS" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<%@ Register Src="~/admin/UserControls/Catalog/AddProduct.ascx" TagName="AddProduct" TagPrefix="adv" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript" src="js/ajaxfileupload.js"></script>
    <style type="text/css">
        .tableview input[type=file] {
            font-size: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <adv:AddProduct ID="addProduct" runat="server" />
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
            <li class="neighbor-menu-item"><a href="Brands.aspx">
                <%= Resource.Admin_MasterPageAdmin_Brands%></a></li>
            <li class="neighbor-menu-item selected"><a href="Reviews.aspx">
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
        <div style="text-align: center;">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblReview" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_Reviews %>" />
                            <br />
                            <asp:Label ID="lblReviewName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_ReviewsView %>" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div>
            <div id="gridTable" runat="server">
                <div>
                    <div class="btns-main">
                        <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddReview" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_AddReview %>"
                            OnClick="btnAddReview_Click" />
                    </div>
                    <div style="height: 10px">
                    </div>
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
                                        <option value="setChecked">
                                            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetChecked %>"></asp:Localize>
                                        </option>
                                        <option value="setNotChecked">
                                            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetNotChecked %>"></asp:Localize>
                                        </option>
                                    </select>
                                    <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                    <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" OnClick="lbDeleteSelected_Click" />
                                    <asp:LinkButton ID="lbSetChecked" Style="display: none" runat="server" OnClick="lbSetChecked_Click" />
                                    <asp:LinkButton ID="lbSetNotChecked" Style="display: none" runat="server" OnClick="lbSetNotChecked_Click" />
                                    <asp:LinkButton ID="lbChangeStatus" Style="display: none" runat="server" />
                                </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected %></span></span></span>
                            </td>
                            <td align="right" class="selecteditems">
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
                                    <td colspan="10"></td>
                                </tr>
                                <tr>
                                    <td style="width: 60px; text-align: center;">
                                        <div style="height: 0px; font-size: 0px; width: 60px">
                                        </div>
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="99%">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 220px; text-align: center;">
                                        <div style="height: 0; font-size: 0; width: 220px"></div>
                                        <asp:DropDownList ID="ddlPhoto" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="99%">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                            <asp:ListItem Text="С фото" Value="True" />
                                            <asp:ListItem Text="Без фото" Value="False" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 180px;">
                                        <div style="height: 0; font-size: 0; width: 180px"></div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtEntityName" Width="95%" runat="server" TabIndex="11" />
                                    </td>
                                    <td style="width: 110px;">
                                        <div style="height: 0; font-size: 0; width: 110px"></div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtArtNo" Width="95%" runat="server" TabIndex="11" />
                                    </td>
                                    <td>
                                        <div style="height: 0; font-size: 0; width: 100px"></div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="95%" runat="server" TabIndex="11" />
                                    </td>
                                    <td>
                                        <div style="height: 0; font-size: 0; width: 100px"></div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtEmail" Width="95%" runat="server" TabIndex="11" />
                                    </td>
                                    <td>
                                        <div style="height: 0; font-size: 0; width: 100px"></div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtText" Width="95%" runat="server" TabIndex="11" />
                                    </td>
                                    <td style="width: 130px; vertical-align: middle">
                                        <div style="height: 0; font-size: 0; width: 130px"></div>
                                        <table cellpadding="1px;" cellspacing="0px;" style="margin-left: 5px;">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%=Resource.Admin_Catalog_From%>:
                                                </td>
                                                <td style="width: 110px;">
                                                    <div class="dp">
                                                        <asp:TextBox CssClass="filtertxtbox" ID="txtDateFrom" Width="70" runat="server" TabIndex="10" />
                                                        <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%=Resource.Admin_Catalog_To%>:
                                                </td>
                                                <td>
                                                    <div class="dp">
                                                        <asp:TextBox CssClass="filtertxtbox" ID="txtDateTo" Width="70" runat="server" TabIndex="10" />
                                                        <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 70px; text-align: center;">
                                        <div style="height: 0; font-size: 0; width: 70px">
                                        </div>
                                        <asp:DropDownList ID="ddlChecked" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="95%">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="-1" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 60px; text-align: center;">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                            TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                            TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr style="height: 5px;">
                                    <td colspan="10"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                                <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnAddReview" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <div style="display: none;" id="pnlErrors" runat="server">
                                </div>
                                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="5" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Reviews_Confirmation %>"
                                    CssClass="tableview" GridLines="None" TooltipTextCellIndex="5" DataKeyNames=""
                                    OnSorting="grid_Sorting" OnRowCommand="grid_RowCommand" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall headerCb" runat="server"
                                                    onclick="javascript:SelectVisible(this.checked);" Style="margin-left: 0px;" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# (bool) Eval("IsSelected")? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="50" CssClass="checkboxcolumnheader" />
                                            <ItemStyle HorizontalAlign="Center" CssClass="checkboxcolumn" />
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ProductPhoto">
                                            <HeaderTemplate>
                                                <div style="width: 210px; height: 0; font-size: 0;"></div>
                                                Изображение
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# GetImageItem(SQLDataHelper.GetString(Eval("PhotoName"))) %>

                                                <input type="file" id="<%# "newPicture_" + Eval("ID") %>" name="<%# "newPicture_" + Eval("ID") %>" <%# string.IsNullOrEmpty( SQLDataHelper.GetString(Eval("PhotoName")) )? string.Empty : "style=\"display:none;\"" %>>
                                                <div>
                                                    <a href="javascript:void(0);" onclick="<%# "uploadPicture(" + Eval("ID") + ")" %>" <%# string.IsNullOrEmpty( SQLDataHelper.GetString(Eval("PhotoName")) )? string.Empty : "style=\"display:none;\"" %>><%=Resource.Admin_ColorsDictionary_Upload %></a>
                                                    <a href="javascript:void(0);" onclick="<%# "deletePicture(" + Eval("ID") + ")" %>" <%# string.IsNullOrEmpty( SQLDataHelper.GetString(Eval("PhotoName")) )? "style=\"display:none;\"" :string.Empty  %>><%=Resource.Admin_ColorsDictionary_Delete %></a>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="210px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ProductName">
                                            <HeaderTemplate>
                                                <div style="width: 170px; height: 0; font-size: 0;"></div>
                                                <asp:LinkButton ID="lbProductName" runat="server" CommandName="Sort" CommandArgument="ProductName">
                                                    <%=Resource.Admin_Reviews_Name%>
                                                    <asp:Image ID="arrowProductName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lblName" runat="server" Text='<%# string.IsNullOrEmpty(Eval("ProductName").ToString()) ? GetEntityName((int)Eval("ID")) : Eval("ProductName") %>'
                                                    NavigateUrl='<%# GetEntityAdminUrl((int)Eval("EntityId"), (EntityType)SQLDataHelper.GetInt(Eval("Type"))) %>' Target="_blank" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="170px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ArtNo">
                                            <HeaderTemplate>
                                                <div style="width: 100px; height: 0; font-size: 0;"></div>
                                                <asp:LinkButton ID="lbArtNo" runat="server" CommandName="Sort" CommandArgument="ArtNo">
                                                    Артикул
                                                    <asp:Image ID="arrowArtNo" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblArtNo" runat="server" Text='<%# SQLDataHelper.GetString(Eval("ArtNo")) %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewArtNo" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Name">
                                            <HeaderTemplate>
                                                <div style="width: 100px; height: 0; font-size: 0;"></div>
                                                <asp:LinkButton ID="lbOrderName" runat="server" CommandName="Sort" CommandArgument="Name">
                                                    <%= Resource.Admin_ReviewsBlock_UserName%>
                                                    <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNameBind" runat="server" Text='<%# Eval("Name") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewName" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Email">
                                            <HeaderTemplate>
                                                <div style="width: 100px; height: 0; font-size: 0;"></div>
                                                <asp:LinkButton ID="lbOrderEmail" runat="server" CommandName="Sort" CommandArgument="Email">
                                                    <%= Resource.Admin_ReviewsBlock_Email%>
                                                    <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEmailBind" runat="server" Text='<%# Eval("Email") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewEmail" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="[Text]">
                                            <HeaderTemplate>
                                                <div style="width: 100px; height: 0; font-size: 0;"></div>
                                                <%= Resource.Admin_Reviews_Text%>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTextBind" runat="server" Text='<%# Eval("[Text]") %>' Width="99%"
                                                    TextMode="MultiLine" Rows="4" Wrap="true" Style="overflow: auto"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewText" runat="server" Width="99%" TextMode="MultiLine" Rows="4" Wrap="true" Style="overflow: auto" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="AddDate">
                                            <HeaderTemplate>
                                                <div style="width: 120px; height: 0; font-size: 0;"></div>
                                                <asp:LinkButton ID="lbAddDate" runat="server" CommandName="Sort" CommandArgument="AddDate">
                                                    <%=Resource.Admin_Reviews_AddDate%>
                                                    <asp:Image ID="arrowAddDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtAddDate" runat="server" Text='<%#  AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("AddDate")) %>'
                                                    Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewAddDate" runat="server" CssClass="add" Text='<%# AdvantShop.Localization.Culture.ConvertDate(DateTime.Now) %>' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                            <HeaderStyle Width="120px" />
                                            <ItemStyle CssClass="colmodify"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Checked">
                                            <HeaderTemplate>
                                                <div style="width: 60px; height: 0; font-size: 0;"></div>
                                                <asp:LinkButton ID="lbOrderChecked" runat="server" CommandName="Sort" CommandArgument="Checked">
                                                    <%= Resource.Admin_Reviews_Checked%>
                                                    <asp:Image ID="arrowChecked" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbChecked" runat="server" Checked='<%# Eval("Checked")%>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="cbNewChecked" runat="server" Checked="True" CssClass="add" />
                                            </FooterTemplate>
                                            <HeaderStyle Width="60px" />
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Buttons">
                                            <HeaderTemplate>
                                                <div style="width: 50px; height: 0; font-size: 0;"></div>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' CommandName="DeleteReview"
                                                    data-confirm="<%$ Resources:Resource, Admin_Reviews_Confirmation %>" CommandArgument='<%# Eval("ID") %>' />
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                    style="display: none" title='<%= Resource.Admin_MasterPageAdminCatalog_Update %>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                    style="display: none" title='<%=Resource.Admin_MasterPageAdminCatalog_Cancel %>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="ibAddColor" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddReview"
                                                    ToolTip="<%$ Resources:Resource, Admin_Add  %>" />
                                                <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                    CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Cancel  %>" />
                                            </FooterTemplate>
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 readonlyrow" />
                                    <EmptyDataTemplate>
                                        <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                            <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_EmptyDataPage %>"></asp:Localize>
                                        </div>
                                    </EmptyDataTemplate>
                                </adv:AdvGridView>
                                <div style="border-top: 1px #c9c9c7 solid;">
                                </div>
                                <table class="results2">
                                    <tr>
                                        <td style="width: 157px; padding-left: 6px;">
                                            <asp:Localize ID="Localize_Admin_Catalog_ResultPerPage" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ResultPerPage %>"></asp:Localize>:&nbsp;
                                            <asp:DropDownList ID="ddRowsPerPage" runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist"
                                                AutoPostBack="true" Style="display: inline-block;">
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
                                                    <%=Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
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
                </div>
            </div>
        </div>
        <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
        <script type="text/javascript">

            //$(document).ready(function () {
            //    $("input.showtooltip").tooltip({
            //        showURL: false
            //    });
            //});

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

            function setupAdvantGrid() {
                $("img.imgtooltip").tooltip({
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
            }

            $(document).ready(function () {
                document.onkeydown = keyboard_navigation;
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(Darken);
                prm.add_endRequest(Clear);
                initgrid();
                $("ineditcategory").tooltip();
                setupAdvantGrid();
                showNotify();
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
                            var r = confirm("<%= Resource.Admin_Reviews_Confirm%>");
                            if (r) __doPostBack('<%= lbDeleteSelected.UniqueID %>', '');
                            break;
                        case "setChecked":
                            document.getElementById('<%=lbSetChecked.ClientID%>').click();
                        break;
                    case "setNotChecked":
                        document.getElementById('<%=lbSetNotChecked.ClientID%>').click();
                        break;
                }
                });
        }

        function showNotify() {
            if ($("#<%=pnlErrors.ClientID%>").html().length) {
                notify($("#<%=pnlErrors.ClientID%>").html(), notifyType.error, false, { timer: 10000 });
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            selectCange();
            setupAdvantGrid();
            showNotify();
        });

        function uploadPicture(reviewId) {

            var arrId = [];
            arrId.push("newPicture_" + reviewId);

            $.ajaxFileUpload({
                url: '../admin/HttpHandlers/Review/UploadReviewImage.ashx?ReviewId=' + reviewId,
                secureuri: false,
                fileElementId: arrId,
                dataType: 'json',
                success: function (data, status) {
                    if (data != null && data.msg == "error") {
                        notify(data.error, notifyType.error, false, { timer: 10000 });
                    } else {
                        window.location.reload();
                    }
                },
                error: function (data, status, e) {
                    notify(e, notifyType.error, false, { timer: 10000 });
                }
            });
        }

        function deletePicture(reviewId) {

            $.ajax({
                url: 'HttpHandlers/Review/DeleteReviewImage.ashx?ReviewId=' + reviewId,
                dataType: 'json',
                cache: false,
                success: function (data, status) {
                    if (data != null && data.msg == "error") {
                        notify(data.error, notifyType.error, false, { timer: 10000 });
                    } else {
                        window.location.reload();
                    }
                },
                error: function (data, status, e) {
                    notify(e, notifyType.error, false, { timer: 10000 });
                }
            });
        }
        </script>
    </div>
</asp:Content>
