<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.StaticPageRightNavigation" Codebehind="StaticPageRightNavigation.ascx.cs" %>
<asp:UpdatePanel ID="UpdateCboCategory" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <div class="rightPanelHeader">
                <table border="0" cellspacing="0" cellpadding="0" class="catalog_part catelog_list">
	                <tbody>
		                <tr style="height: 28px;">
			                <td style="width: 25px; padding-top: 3px; padding-left: 3px;">
				                <a href="StaticPages.aspx">
					                <img src="images/folder.gif" alt="" style="border-style: none">
				                </a>
			                </td>
			                <td class="catalog_label">
				                <asp:Label ID="lblBigHead" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_lblSubMain %>" Font-Bold="true" />
			                </td>
			                <td style="width: 22px; padding-top: 5px;" align="right">
				                <asp:HyperLink ID="hlStaticPage" NavigateUrl="../StaticPage.aspx" runat="server"><img style="border:none;" 
                                    class="showtooltip" src="images/gplus.gif" 
                                    title="<%=Resources.Resource.Admin_UserControl_StaticPageRightNavigation_AddStaticPage%>"
                                    onmouseover="this.src='images/bplus.gif';"
                                    onmouseout="this.src='images/gplus.gif';" /></asp:HyperLink>
			                </td>
		                </tr>
	                </tbody>
                </table>
            </div>
            <div style="clear: both">
                <div class="admin_product_categoryListBlock">
                    <adv:CommandTreeView ID="tree" runat="server" NodeWrap="True" ShowLines="true" CssClass="AdminTree_MainClass"
                        OnTreeNodePopulate="tree_TreeNodePopulate">
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle ForeColor="#027dc2" Font-Bold="true" HorizontalPadding="0px" VerticalPadding="0px" />
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                            NodeSpacing="0px" VerticalPadding="0px" />
                    </adv:CommandTreeView>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
