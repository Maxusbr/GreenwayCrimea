<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Products.RelatedProducts" Codebehind="RelatedProducts.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Register TagPrefix="adv" TagName="PopupTree" Src="~/Admin/UserControls/PopupTreeView.ascx" %>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="TreeNodeSelected" />
        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="Hiding" />
    </Triggers>
    <ContentTemplate>
        <adv:PopupTree runat="server" ID="popTree" OnTreeNodeSelected="popTree_Selected"
            Type="CategoryProduct" HeaderText="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_lblMain %>" />
        <asp:Panel runat="server" DefaultButton="lbAddRelatedProductByArtNo">
            <table class="table-p">
                <tr>
                    <td class="formheader">
                        <h2>
                            <span style="margin-left: 3px;">
                                <%=SettingsCatalog.GetRelatedProductName(RelatedType)%>
                            </span>
                        </h2>
                    </td>
                </tr>
                <tr style="height: 40px;">
                    <td>
                        <table style="width: 100%; margin-top: 10px;">
                            <tr>
                                <td style="width: 100px; vertical-align: middle;">
                                    <asp:Label ID="lRelatedProductsMessage" runat="server" CssClass="mProductLabelInfo"
                                        EnableViewState="False" Font-Names="Verdana" Font-Size="14px" ForeColor="Red"
                                        Visible="False" />
                                    <asp:LinkButton ID="lbAddRelatedProduct" runat="server" OnClientClick="document.body.style.overflowX='hidden';_TreePostBack=true;removeunloadhandler();"
                                        OnClick="lbAddRelatedProduct_Click" EnableViewState="false"><%=Resources.Resource.Admin_Product_AddRelatedProduct%></asp:LinkButton>
                                </td>
                                <td style="width: 50px; text-align: center; vertical-align: middle;">
                                    |
                                </td>
                                <td style="vertical-align: middle;">
                                    <%=Resources.Resource.Admin_Product_AddRelatedProductByArtNo%> 
                                    <asp:TextBox ID="txtProductArtNo" runat="server" CssClass="niceTextBox shortTextBoxClass" style="margin:0px 7px; width:145px;" />
                                    <asp:Button ID="lbAddRelatedProductByArtNo" runat="server" OnClick="lbAddRelatedProductByArtNo_Click" CssClass="btn btn-middle btn-add" 
                                        EnableViewState="false" Text="<%$ Resources:Resource,Admin_Product_Add %>"></asp:Button>
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
                        <span style="font-size: 14px; font-weight: bold;"><%=Resources.Resource.Admin_Product_CurrentRelatedProducts%></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Repeater ID="rRelatedProducts" runat="server" OnItemCommand="rRelatedProducts_ItemCommand">
                            <HeaderTemplate>
                                <ol style="list-style: none; padding: 0; margin: 0;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <%# Container.ItemIndex +1 %>.&nbsp;<a href='<%#"Product.aspx?ProductID=" + Eval("ProductID")%>'
                                        class="Link"><%# Eval("ArtNo") + " - " + Eval("Name")%></a>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%#Eval("ProductID")%>'
                                        OnClientClick="removeunloadhandler();" CommandName="DeleteRelatedProduct">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/remove.jpg" EnableViewState="false" />
                                    </asp:LinkButton>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ol>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td>
                        <% if (RelatedType == 0) { %>
                            <div class="dvSubHelp">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                                <a href="http://www.advantshop.net/help/pages/with-product-buy" target="_blank">Инструкция. Перекрестный маркетинг: «С этим товаром покупают»</a>
                            </div>
                        <% } %>
                        <% if (RelatedType == 1) { %>
                            <div class="dvSubHelp">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                                <a href="http://www.advantshop.net/help/pages/similar-goods" target="_blank">Инструкция. Перекрестный маркетинг: опция «Похожие товары»</a>
                            </div>
                        <% } %>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>