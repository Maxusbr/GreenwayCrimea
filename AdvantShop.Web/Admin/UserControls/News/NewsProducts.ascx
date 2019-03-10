<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.News.NewsProducts" Codebehind="NewsProducts.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="adv" TagName="PopupTree" Src="~/Admin/UserControls/PopupTreeView.ascx" %>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="TreeNodeSelected" />
        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="Hiding" />
    </Triggers>
    <ContentTemplate>
        <adv:PopupTree runat="server" ID="popTree" OnTreeNodeSelected="popTree_Selected"
            Type="CategoryProduct" HeaderText="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>" />
        <asp:Panel runat="server" DefaultButton="lbAddProductByArtNo">
            <table class="info-tb">
                <tr class="rowPost">
                    <td colspan="2" style="height: 34px;">
                        <span class="spanSettCategory">
                            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_EditNews_TabProducts %>" /></span>
                    </td>
                </tr>
                <tr class="rowPost">
                    <td>
                        <table style="width: 100%; margin-top: 10px;">
                            <tr>
                                <td style="width: 100px; vertical-align: middle;">
                                    <asp:Label ID="lRelatedProductsMessage" runat="server" CssClass="mProductLabelInfo"
                                        EnableViewState="False" Font-Names="Verdana" Font-Size="14px" ForeColor="Red"
                                        Visible="False" />
                                    <asp:LinkButton ID="lbAddRelatedProduct" runat="server" OnClientClick="document.body.style.overflowX='hidden';_TreePostBack=true;removeunloadhandler();"
                                        OnClick="lbAddRelatedProduct_Click" EnableViewState="false"><%=Resource.Admin_Product_AddRelatedProduct%></asp:LinkButton>
                                </td>
                                <td style="width: 50px; text-align: center; vertical-align: middle;">
                                    |
                                </td>
                                <td style="vertical-align: middle;">
                                    <%=Resource.Admin_Product_AddRelatedProductByArtNo%> 
                                    <asp:TextBox ID="txtProductArtNo" runat="server" />
                                    <asp:Button ID="lbAddProductByArtNo" runat="server" OnClick="lbAddProductByArtNo_Click"
                                                EnableViewState="false" Text="<%$ Resources:Resource, Admin_Product_Add %>" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                                <td>
                                    <asp:Label ID="txtError" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="height: 40px;">
                    <td>
                    </td>
                </tr>
                <tr style="height: 40px;">
                    <td>
                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Resource, Admin_Product_CurrentRelatedProducts %>"
                            EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Repeater ID="rNewsProducts" runat="server" OnItemCommand="rNewsProducts_ItemCommand">
                            <HeaderTemplate>
                                <ol style="list-style: none; padding: 0; margin: 0;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <%# Container.ItemIndex +1 %>.&nbsp;<a href='<%#"Product.aspx?ProductID=" + Eval("ProductID")%>'
                                        class="Link"><%# Eval("ArtNo") + " - " + Eval("Name")%></a>
                                    <asp:LinkButton runat="server" CommandArgument='<%#Eval("ProductID")%>' OnClientClick="removeunloadhandler();" CommandName="DeleteNewsProduct">
                                        <asp:Image runat="server" ImageUrl="~/Admin/images/remove.jpg" EnableViewState="false" />
                                    </asp:LinkButton>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ol>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
