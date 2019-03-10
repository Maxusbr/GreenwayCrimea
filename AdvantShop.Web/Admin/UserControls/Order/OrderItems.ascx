<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderItems.ascx.cs" Inherits="Admin.UserControls.Order.OrderItemsControl" %>
<%@ Register Src="~/Admin/UserControls/PopupTreeView.ascx" TagName="PopupTree" TagPrefix="adv" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<adv:PopupTree runat="server" ID="pTreeProduct" HeaderText="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>"
    Type="CategoryProduct" ExceptId="0" OnTreeNodeSelected="pTreeProduct_NodeSelected"
    OnHiding="pTreeProduct_Hiding" />
<asp:Label runat="server" ID="lblError" Visible="false"></asp:Label>
<table style="width: 100%">
    <tr>
        <td>
            <asp:SqlDataSource runat="server" ID="sdsCurrs" OnInit="sds_Init" SelectCommand="SELECT * FROM [Catalog].[Currency]" />
            <span>
                <asp:Localize runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_ChoosingCurrency%>" />: 
            </span>
            <asp:DropDownList ID="ddlCurrs" runat="server" DataSourceID="sdsCurrs" DataTextField="Name"
                DataValueField="CurrencyIso3" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlCurrs_SelectedChanged" Visible="false">
            </asp:DropDownList>
            <asp:Label runat="server" ID="lcurrency" />
            <asp:HiddenField runat="server" ID="hfOldCurrencyValue" />
        </td>
        <td>
            <asp:Label runat="server" ID="lDiscount" Text="<%$ Resources: Resource, Admin_EditOrder_Discount%>"></asp:Label>:
            <asp:TextBox runat="server" ID="txtDiscount" Width="30px" />
            %
        </td>
        <td>
            <asp:Panel Style="float: right;" runat="server" DefaultButton="btnAddProductByArtNo">
                <div style="display: inline-block; vertical-align: top; padding: 3px 0 0 0;">
                    <asp:TextBox runat="server" ID="txtArtNo" CssClass="orderitems-search autocompleteSearch" Placeholder="<%$ Resources: Resource, Admin_OrderSearch_AddProductByArtNoPlaceholder%>" />
                    <input type="hidden" id="hfOffer" runat="server" class="acsearchhf" />
                </div>
                <div style="display: inline-block;">
                    <asp:Button CssClass="btn btn-middle btn-add" CausesValidation="false" ID="btnAddProductByArtNo" runat="server"
                        OnClick="btnAddProductByArtNo_Click" Text="<%$ Resources: Resource, Admin_OrderSearch_AddProductByArtNoOrName%>" />
                </div>
            </asp:Panel>
        </td>
        <td>
            <div style="float: right;">
                <asp:Button CssClass="btn btn-middle btn-add" CausesValidation="false" ID="btnAddProduct" runat="server" OnClick="btnAddProduct_Click" 
                    Text="<%$ Resources: Resource, Admin_OrderSearch_AddProduct%>" />

            </div>
        </td>
    </tr>
    <tr class="formheaderfooter">
        <td colspan="2"></td>
    </tr>
</table>
<div style="text-align: center;">
    <asp:ListView ID="lvOrderItems" runat="server" ItemPlaceholderID="itemPlaceholder"
        OnItemCommand="dlItems_ItemCommand" OnItemDataBound="dlItems_ItemDataBound">
        <LayoutTemplate>
            <table width="100%" class="grid-main orderitems-tb">
                <tr class="header">
                    <th style="width: 150px">
                        &nbsp;
                    </th>
                    <th style="width: 70px">
                        <b><asp:Literal ID="l1" runat="server" Text='<%$ Resources:Resource, Admin_ViewOrder_ArtNo %>'/></b>
                    </th>
                    <th>
                        <b><asp:Literal ID="l2" runat="server" Text='<%$ Resources:Resource, Admin_ViewOrder_ItemName %>'/></b>
                    </th>
                    <th style="width: 100px; text-align: center;">
                        <b><asp:Literal ID="l3" runat="server" Text='<%$ Resources:Resource, Admin_ViewOrder_Price %>'/></b>
                    </th>
                    <th style="width: 100px; text-align: center;">
                        <b><asp:Literal ID="l4" runat="server" Text='<%$ Resources:Resource, Admin_ViewOrder_ItemAmount %>'/></b>
                    </th>
                    <th style="width: 100px;">
                        <asp:Literal ID="l5" runat="server" Text='<%$ Resources:Resource, Admin_ViewOrder_AvailableAmount %>'/>
                    </th>
                    <th style="width: 120px; text-align: center;">
                        <b><asp:Literal ID="l6" runat="server" Text='<%$ Resources:Resource, Admin_ViewOrder_ItemCost %>'/></b>
                    </th>
                    <td style="width: 50px"></td>
                </tr>
                <tr runat="server" id="itemPlaceholder"></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class="row1" style="height: 35px;">
                <td class="product-picture">
                    <%# Eval("ProductID") != null ? RenderPicture((int)Eval("ProductID"), SQLDataHelper.GetNullableInt(Eval("PhotoID"))) : ""%>
                </td>
                <td>
                    <asp:Literal runat="server" ID="ltArtNo" Text='<%#Eval("ArtNo")%>'></asp:Literal>
                </td>
                <td>
                    <div><%# Eval("Name")%></div>
                    <%# RenderSelectedOptions((IList<EvaluatedCustomOptions>)Eval("SelectedOptions"))%>
                    <%# !string.IsNullOrEmpty(SQLDataHelper.GetString(Eval("Size"))) ? "<div>"+ SettingsCatalog.SizesHeader +": <b>" + SQLDataHelper.GetString(Eval("Size")) + "</b></div>" : "" %>
                    <%# !string.IsNullOrEmpty(SQLDataHelper.GetString(Eval("Color"))) ? "<div>"+ SettingsCatalog.ColorsHeader +": <b>" + SQLDataHelper.GetString(Eval("Color")) + "</b></div>" : "" %>

                </td>
                <td style="text-align: center;">
                    <asp:TextBox runat="server" ID="txtPrice" Text='<%#Eval("Price") %>' Width="70px" />
                </td>
                <td style="text-align: center;">
                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Amount") %>' Width="35" />
                </td>
                <td style="width: 100px;">
                    <asp:Label ID="lbMaxCount" runat="server" Text="Label" ForeColor="Red" CssClass="lbMaxCount" Font-Bold="true" />
                </td>
                <td style="text-align: center;">
                    <%# (SQLDataHelper.GetFloat(Eval("Price")) * SQLDataHelper.GetFloat(Eval("Amount"))).FormatPrice(Currency)%>
                </td>
                <td>
                    <div style="display: inline-block">
                        <asp:ImageButton CausesValidation="false" ID="btnQuantUp" ImageUrl="~/Admin/images/refresh.png"
                            runat="server" CommandArgument='<%# Eval("OrderItemID") %>' CommandName="SaveQuantity" />
                    </div>
                    <div style="display: inline-block">
                        <asp:LinkButton ID="buttonDelete" CssClass="deletebtn showtooltip valid-confirm" 
                            CommandName="DeleteItem" CommandArgument='<%# Eval("OrderItemID") %>'
                            runat="server" Enabled="True" CausesValidation="false" ToolTip='<%$ Resources:Resource, Admin_OrderSearch_Delete%>' />
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
