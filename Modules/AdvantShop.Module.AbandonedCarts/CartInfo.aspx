<%@ Page Language="C#" AutoEventWireup="true" Inherits="Advantshop.Modules.UserControls.CartInfo" CodeBehind="CartInfo.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<asp:content contentplaceholderid="cphMain" runat="Server">
    <div style="padding: 15px;">
        <div style="padding: 10px 0;">
            <a href="Module.aspx?module=AbandonedCarts&MasterPageEmpty=<%= Request["MasterPageEmpty"] %>">
                <asp:Localize runat="server" Text="<%$ Resources: BackToCartList%>" /></a>
        </div>

        <div class="order-buyer-name">
            <div class="list-order-data-caption">
                <asp:Localize runat="server" Text="<%$ Resources: Customer%>" />:
            </div>
            <ul id="customerInfo" runat="server" class="list-order-data">
                <li class="list-order-data-row">
                    <div class="list-order-data-name">
                        <asp:Localize runat="server" Text="<%$ Resources: CustomerName%>" />:
                    </div>
                    <div class="list-order-data-value">
                        <asp:HyperLink ID="lnkCustomerName" runat="server" Target="_blank" />
                        <asp:Label ID="lblCustomerName" runat="server" />
                    </div>
                </li>
                <li class="list-order-data-row">
                    <div class="list-order-data-name">
                        E-mail
                    </div>
                    <div class="list-order-data-value">
                        <asp:HyperLink ID="lnkCustomerEmail" runat="server" CssClass="order-email" Target="_blank"></asp:HyperLink>
                        <asp:Label ID="lblCustomerEmail" runat="server" />
                    </div>
                </li>
                <li class="list-order-data-row">
                    <div class="list-order-data-name">
                        <asp:Localize runat="server" Text="<%$ Resources: Telephone%>" />
                    </div>
                    <div class="list-order-data-value">
                        <asp:Label ID="lblCustomerPhone" runat="server" />
                    </div>
                </li>
            </ul>
            <div id="customerInfoNotExist" runat="server" visible="False">
                <asp:Localize runat="server" Text="<%$ Resources: NoData%>" />
            </div>
        </div>
        <ul class="order-list-address">
            <li class="order-list-address-item">
                <% if (ShippingContact != null)
                    { %>
                <div class="list-order-data-caption">
                    <asp:Localize runat="server" Text="<%$ Resources: ShippingAddress%>" />
                </div>
                <ul class="list-order-data">
                    <li class="list-order-data-row">
                        <div class="list-order-data-name">
                            <asp:Localize runat="server" Text="<%$ Resources: Country%>" />:
                        </div>
                        <div class="list-order-data-value">
                            <asp:Label ID="lblShippingCountry" runat="server" />
                        </div>
                    </li>
                    <% if (ShippingContact.Region.IsNotEmpty())
                        { %>
                    <li class="list-order-data-row">
                        <div class="list-order-data-name">
                            <asp:Localize runat="server" Text="<%$ Resources: Zone%>" />:
                        </div>
                        <div class="list-order-data-value">
                            <asp:Label ID="lblShippingRegion" runat="server" />
                        </div>
                    </li>
                    <% }
                        if (ShippingContact.City.IsNotEmpty())
                        { %>
                    <li class="list-order-data-row">
                        <div class="list-order-data-name">
                            <asp:Localize runat="server" Text="<%$ Resources: City%>" />:
                        </div>
                        <div class="list-order-data-value">
                            <asp:Label ID="lblShippingCity" runat="server" />
                        </div>
                    </li>
                    <% }
                        if (ShippingContact.Zip.IsNotEmpty())
                        { %>
                    <li class="list-order-data-row">
                        <div class="list-order-data-name">
                            <asp:Localize runat="server" Text="<%$ Resources: Zip%>" />:
                        </div>
                        <div class="list-order-data-value">
                            <asp:Label ID="lblShippingZipCode" runat="server" />
                        </div>
                    </li>
                    <% }
                        if (ShippingContact.Street.IsNotEmpty())
                        { %>
                    <li class="list-order-data-row">
                        <div class="list-order-data-name">
                            <asp:Localize runat="server" Text="<%$ Resources: Address%>" />:
                        </div>
                        <div class="list-order-data-value">
                            <asp:Label ID="lblShippingAddress" runat="server" />
                        </div>
                    </li>
                    <% } %>
                </ul>
                <% } %>
            </li>
            <li id="divShipPayments" runat="server" class="order-list-address-item">
                <div class="list-order-data-caption">
                    <asp:Localize runat="server" Text="<%$ Resources: ShippingMethodName%>" />:
                </div>
                <div>
                    <asp:Label ID="lblShippingMethodName" runat="server" />
                </div>
                <div class="list-order-data-caption order-payment-method">
                    <asp:Localize runat="server" Text="<%$ Resources: PaymentMethodName%>" />:
                </div>
                <div>
                    <asp:Label ID="lblPaymentMethodName" runat="server" />
                </div>
            </li>
        </ul>


        <div class="order-table-wrap">
            <asp:ListView ID="lvOrderItems" runat="server" ItemPlaceholderID="itemPlaceholderID">
                <layouttemplate>
                    <table class="table-ui-simple">
                        <caption>
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources: OrderContent %>" /></caption>
                        <thead>
                            <tr>
                                <th>
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources: Product %>" />
                                </th>
                                <th class="table-ui-simple-align-center">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources: PriceForUnit %>" />
                                </th>
                                <th class="table-ui-simple-align-center">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources: ItemAmount %>" />
                                </th>
                                <th class="table-ui-simple-align-center">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources: ItemCost %>" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr runat="server" id="itemPlaceholderID">
                            </tr>
                        </tbody>
                    </table>
                </layouttemplate>
                <itemtemplate>
                    <tr>
                        <td>
                            <a href='<%# UrlService.GetAbsoluteLink("products/" + Eval("Offer.Product.UrlPath")) %>'
                               class="order-item-photo" target="_blank">
                                <%# Eval("Offer.ProductID") != null ? RenderPicture(SQLDataHelper.GetInt(Eval("Offer.ProductID")), SQLDataHelper.GetNullableInt(Eval("Offer.Photo") != null ? Eval("Offer.Photo.PhotoID") : null)) : "" %></a>
                            <div class="order-item-info">
                                <div class="order-item-name">
                                    <a href='<%# UrlService.GetAbsoluteLink("products/" + Eval("Offer.Product.UrlPath")) %>' class="order-item-lnk" target="_blank">
                                        <%#Eval("Offer.Product.Name") %></a>
                                </div>
                                <div>
                                    <%#Eval("Offer.ArtNo") %>
                                </div>
                                <div class="order-item-options">
                                    <div>
                                        <%# !string.IsNullOrEmpty(Convert.ToString(Eval("Offer.Color"))) ? "<span>" + SettingsCatalog.ColorsHeader + ":</span> " + Eval("Offer.Color") : string.Empty %>
                                    </div>
                                    <div>
                                        <%# !string.IsNullOrEmpty(Convert.ToString(Eval("Offer.Size"))) ? "<span>" + SettingsCatalog.SizesHeader + ":</span> " + Eval("Offer.Size") : string.Empty %>
                                    </div>
                                    <div>
                                        <%# RenderSelectedOptions(Convert.ToString(Eval("AttributesXml"))) %>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td class="table-ui-simple-align-center table-ui-simple-bold">
                            <%# PriceFormatService.FormatPrice(Convert.ToSingle(Eval("PriceWithDiscount"))) %>
                        </td>
                        <td class="table-ui-simple-align-center">
                            <%# Convert.ToSingle(Eval("Amount")) %>
                        </td>
                        <td class="table-ui-simple-align-center table-ui-simple-bold">
                            <%# PriceFormatService.FormatPrice(Convert.ToSingle(Eval("PriceWithDiscount")) * Convert.ToSingle(Eval("Amount"))) %>
                        </td>
                    </tr>
                </itemtemplate>
            </asp:ListView>
        </div>

        <div style="padding: 10px 0 0 0;" id="lbCreateLead" runat="server">
            <a href="javascript:void(0)" onclick="createlead()" class="btn btn-middle btn-add">Создать лид</a>

             <script>
                 function createlead() {
                     $.ajax({
                         url: '../modules/AbandonedCarts/createlead.ashx?customerid=<%=Request["Id"]%>&ShoppingCartType=<%=(int)shoppingCartType%>',
                         error: function () {
                             console.log("Произошла ошибка");
                         },
                         success: function (data) {
                             if (data.indexOf("error") == -1) {
                                 window.top.location = '../adminv2/leads/edit/' + data;
                             }
                             else {
                                 console.log(data);
                             }
                         },
                         type: 'POST'
                     });
                 }
        </script>
        </div>
       
    </div>
</asp:content>
