<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.ProductSets.Admin_AdminProductSets" Codebehind="AdminProductSets.ascx.cs" %>
<asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel runat="server" DefaultButton="btnAddOfferByArtNo">
            <table class="table-p">
                <tr>
                    <td class="formheader">
                        <h2>
                            <asp:Label runat="server" ID="lblHead" />
                        </h2>
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="text-align: left; padding-top: 10px;">
                        <asp:Label runat="server" Text="<%$ Resources: AddProductToSetByArtNo %>" EnableViewState="false" />
                        <asp:TextBox ID="txtOfferArtNo" runat="server" ValidationGroup="ProductSets" CssClass="niceTextBox shortTextBoxClass" style="margin:0 7px; width:145px;"></asp:TextBox>
                        <asp:Button ID="btnAddOfferByArtNo" runat="server" OnClick="btnAddOfferByArtNo_Click" ValidationGroup="ProductSets" CssClass="btn btn-middle btn-add" 
                            EnableViewState="false" Text="<%$ Resources:Add %>"></asp:Button>
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td>
                        <span style="font-size: 14px; font-weight: bold;">
                            <asp:Localize runat="server" Text="<%$ Resources: CurrentProductsInSet %>" />
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Repeater ID="rProductsInSet" runat="server" OnItemCommand="rProductsInSet_ItemCommand">
                            <HeaderTemplate>
                                <ol style="list-style: none; padding: 0; margin: 0;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <%# Container.ItemIndex +1 %>.&nbsp;<a href='<%#"Product.aspx?ProductID=" + Eval("ProductId")%>'
                                        class="Link"><%# Eval("ArtNo") + " - " + Eval("Product.Name") + 
                                            (Eval("Size") != null ? ", " + Eval("Size.SizeName") : string.Empty) +
                                            (Eval("Color") != null ? ", " + Eval("Color.ColorName") : string.Empty ) %></a>
                                    <asp:LinkButton ID="lbRemoveOffer" runat="server" CommandArgument='<%#Eval("OfferId")%>' 
                                        CommandName="DeleteOfferFromSet" ValidationGroup="ProductSets">
                                        <asp:Image runat="server" ImageUrl="~/Modules/ProductSets/images/remove.jpg" EnableViewState="false" />
                                    </asp:LinkButton>
                                    <%# CheckOffer((AdvantShop.Catalog.Offer)Container.DataItem) %>
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
                        <asp:Label ID="lblMessage" runat="server" Visible="false" />
                        <div class="dvSubHelp">
	                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
	                        <a href="http://www.advantshop.net/help/pages/module-kit-products" 
                                target="_blank">Инструкция. Модуль "Комплекты товаров"</a>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>