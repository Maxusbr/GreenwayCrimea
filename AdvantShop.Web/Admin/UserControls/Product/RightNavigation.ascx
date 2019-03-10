<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Products.RightNavigation" Codebehind="RightNavigation.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<asp:UpdatePanel ID="UpdateCboCategory" runat="server" ChildrenAsTriggers="true"
    UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <div class="rightPanelHeader">
                <div style="width: 30px; float: left">
                    <img src="images/folder.gif" alt="" />
                </div>
                <div style="width: 164px; float: left">
                    <asp:Label ID="lblBigHead" runat="server" Text="<%$ Resources:Resource, Admin_Product_Catalog %>"
                        Font-Bold="true" style="font-family: verdana;font-size: 10pt;font-weight: bold;" EnableViewState="false" />
                </div>
                <div style="width: 37px; float: left">
                    <a href="javascript:void(0)" data-add-product-call class="link-pictograph-add-product">
                        <img style="border:none;" class="showtooltip" src="images/gplus.gif" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_AddNewProduct%>" 
                            onmouseover="this.src='images/bplus.gif';" onmouseout="this.src='images/gplus.gif';" />
                    </a>
                    <input type="image" class="showtooltip" src="images/gudarrow.gif" onclick="open_window('m_CategorySortOrder.aspx', 750, 640);return false;"
                        title="<%= Resources.Resource.Admin_MasterPageAdminCatalog_SortOrder %>" onmouseover="this.src='images/budarrow.gif';"
                        onmouseout="this.src='images/gudarrow.gif';" />
                </div>
            </div>
            <div style="clear: both">
                <div style="text-align: center; padding: 5px 5px 0 5px;">
                    <asp:DropDownList ID="ddlCategory" runat="server" Width="231px"
                        AutoPostBack="True" DataTextField="Name" DataValueField="CategoryID" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" />
                </div>
                <div class="admin_product_categoryListBlock">
                    <asp:Repeater runat="server" OnItemCommand="rCategoryContent_ItemCommand" ID="rCategoryContent">
                        <HeaderTemplate>
                            <table style="width: 100%">
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 14px;">
                                    <img src='<%# ((int)Eval("ItemType")==0) ? "images/lfolder.gif": "images/blank.gif" %>'
                                        alt="" />
                                </td>
                                <td style="width: 189px;">
                                    <asp:LinkButton ID="LinkButton2" CssClass="blueLink" runat="server" Text='<%# Eval("Name")%>'
                                        OnClientClick="removeunloadhandler()" CommandName="Select" CommandArgument='<%# Eval("ID")%>'
                                        Visible='<%# (int)Eval("ItemType") == 0 %>' />
                                    <%# ((int)Eval("ID") == ProductID ? "<b>" : "")%>
                                    <%#((int)Eval("ItemType") == 1 && Eval("PhotoName") != DBNull.Value) ? "<a href=Product.aspx?ProductID=" + Eval("ID") + "&CategoryID=" + CategoryID + "&pn=" + Paging.CurrentPageIndex + " class='blueLink middlePictureSrc imgtooltip' abbr='" + FoldersHelper.GetImageProductPath(ProductImageType.XSmall, (string)Eval("PhotoName"), true) + "'>" + Eval("Name") + "</a>" : String.Empty%>
                                    <%#((int)Eval("ItemType") == 1 && Eval("PhotoName") == DBNull.Value) ? "<a href=Product.aspx?ProductID=" + Eval("ID") + "&CategoryID=" + CategoryID + "&pn=" + Paging.CurrentPageIndex + " class='blueLink imgtooltip'>" + Eval("Name") + "</a>" : String.Empty%>
                                    <%# ((int)Eval("ID") == ProductID) ? "</b>" : ""%>
                                </td>
                                <td>
                                    <td style="width: 14px;">
                                        <asp:LinkButton runat="server" ID="ibDelFrmCategory" CssClass="showtooltip"
                                            CommandName="DeleteFromCategory" CommandArgument='<%# Eval("ID")%>' Visible='<%# ((int)Eval("ItemType")==1 && CategoryID != CategoryService.DefaultNonCategoryId) %>'
                                            data-confirm="<%$ Resources:Resource, Admin_Product_ConfirmDeletingProductFormCategory %>" 
                                            ToolTip="<%$Resources:Resource, Admin_Product_ConfirmDeletingProductFormCategory%>">
                                            <img src="images/excludebtn.png"/>
                                        </asp:LinkButton>
                                    </td>
                                </td>
                                <td style="width: 14px;">
                                    <asp:LinkButton runat="server" ID="ibDelete" CssClass="showtooltip valid-confirm"
                                        CommandName='<%# ((int)Eval("ItemType")==0)? "DeleteCategory" : "DeleteProduct" %>'
                                        CommandArgument='<%# Eval("ID")%>' Visible='<%# !((int)Eval("ItemType") == 0 && (int)Eval("ID")==0) %>'
                                        ToolTip="<%$Resources:Resource, Admin_Delete%>">
                                        <img src="images/gcross.gif" alt=""/>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <% if (Paging.PageCount > 1)
                   { %>
                <table id="pager" style="width: 100%; background-color: #EFF0F1; text-align: center;">
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbPreviousPage" CssClass="Link" runat="server" Enabled="false"
                                EnableViewState="false" OnClientClick="removeunloadhandler()" Text="<%$Resources:Resource, Admin_Product_Prev%>"
                                OnClick="lbPreviousPage_Click"> </asp:LinkButton>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCurrentPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrentPage_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbNextPage" CssClass="Link" runat="server" Enabled="false" EnableViewState="false"
                                OnClientClick="removeunloadhandler()" Text="<%$Resources:Resource, Admin_Product_Next %>"
                                OnClick="lbNextPage_Click">
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <% }  %>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
