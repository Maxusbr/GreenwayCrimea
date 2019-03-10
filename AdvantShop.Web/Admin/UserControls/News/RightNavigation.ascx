<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.News.RightNavigation" Codebehind="RightNavigation.ascx.cs" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="Resources" %>
<asp:UpdatePanel ID="UpdateCboCategory" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <div class="rightPanelHeader">
                <div style="width: 30px; float: left">
                    <img src="images/folder.gif" alt="" />
                </div>
                <div style="width: 185px; float: left">
                    <asp:Label ID="lblBigHead" runat="server" Text="<%$ Resources:Resource, Admin_NewsAdmin_Header %>"
                        Font-Bold="true" style="font-family: verdana;font-size: 10pt;font-weight: bold;" EnableViewState="false" />
                </div>
                <div style="float: left">
                    <a href="EditNews.aspx"  class="link-pictograph-add-product">
                        <img style="border:none;" class="showtooltip" src="images/gplus.gif" title="<%=Resource.Admn_EditNews_AddNews%>" 
                            onmouseover="this.src='images/bplus.gif';" onmouseout="this.src='images/gplus.gif';" />
                    </a>
                </div>
            </div>
            <div style="clear: both">
                <div style="text-align: center; padding: 5px 5px 0 5px;">
                    <asp:DropDownList ID="ddlNewsCategory" runat="server" Width="231px" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                        DataSourceID="sdsNewsCategories" DataTextField="Name" DataValueField="NewsCategoryID" />
                </div>
                <div class="admin_product_categoryListBlock">
                    <asp:Repeater runat="server" OnItemCommand="rptNews_ItemCommand" ID="rptNews">
                        <HeaderTemplate>
                            <table style="width: 100%">
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 14px;">
                                    <img src='images/blank.gif' alt="" />
                                </td>
                                <td style="width: 203px;">
                                    <%# ((int)Eval("ID") == NewsId ? "<b>" : "")%>
                                    <%# "<a href='EditNews.aspx?NewsId=" + Eval("ID") + "&pn=" + Paging.CurrentPageIndex + "' class='blueLink imgtooltip' " +
                                        ((Eval("PhotoName") != DBNull.Value) ? ("abbr='" + FoldersHelper.GetPath(FolderType.News, (string)Eval("PhotoName"), true)) + "'" : string.Empty) + 
                                        ">" + Eval("Title") + "</a>" %>
                                    <%# ((int)Eval("ID") == NewsId) ? "</b>" : ""%>
                                </td>
                                <td style="width: 14px;">
                                    <asp:LinkButton runat="server" ID="ibDelete" CssClass="showtooltip valid-confirm"
                                        CommandName="DeleteNews" CommandArgument='<%# Eval("ID")%>' ToolTip="<%$Resources:Resource, Admin_Delete%>">
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
                <% } %>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:SqlDataSource ID="sdsNewsCategories" runat="server" SelectCommand="SELECT [Name], [NewsCategoryID] FROM [Settings].[NewsCategory] ORDER BY SortOrder"
    OnInit="sds_Init"></asp:SqlDataSource>
