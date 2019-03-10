<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Order.OrderCertificates" Codebehind="OrderCertificates.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Import Namespace="Resources" %>
<asp:Label runat="server" ID="lblError" Visible="false"></asp:Label>
<div style="text-align: center;">
    <asp:ListView ID="lvOrderCertificates" runat="server" ItemPlaceholderID="itemPlaceholderID">
        <LayoutTemplate>
            <table class="table-ui-simple">
                <caption>
                    <asp:Label runat="server" Text="<%$ Resources:Resource,Admin_EditOrder_OrderDetails %>"></asp:Label>
                </caption>
                <thead>
                    <tr>
                        <th>
                            <asp:Label runat="server" Text=" <%$ Resources:Resource,Admin_EditOrder_OrderCertificate_GiftCertificateCode%>"></asp:Label>
                        </th>
                        <th class="table-ui-simple-align-center">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource,Admin_EditOrder_OrderCertificate_ApplyOrderNumber%>"></asp:Label>
                        </th>
                        <th class="table-ui-simple-align-center">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource,Admin_EditOrder_OrderCertificate_Sum%>"></asp:Label>
                        </th>
                        <th class="table-ui-simple-align-center">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource,Admin_EditOrder_OrderCertificate_Used%>"></asp:Label>
                        </th>
                        <th class="table-ui-simple-align-center">
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource,Admin_EditOrder_OrderCertificate_Enable%>"></asp:Label>
                        </th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr id="itemPlaceholderID" runat="server">
                    </tr>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td class="table-ui-simple-align-center table-ui-simple-bold">
                    <%# Eval("CertificateCode") %>
                </td>
                <td class="table-ui-simple-align-center">
                    <%# Eval("ApplyOrderNumber") %>
                </td>
                <td class="table-ui-simple-align-center table-ui-simple-bold">
                    <%# PriceFormatService.FormatPrice(Convert.ToSingle(Eval("Sum")), OrderCurrency)%>
                </td>
                <td class="table-ui-simple-align-center">
                    <asp:CheckBox ID="ckbCertificateUsed" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean(Eval("Used")) %>' />
                </td>
                <td class="table-ui-simple-align-center">
                    <asp:CheckBox ID="ckbCertificateEnabled" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean(Eval("Enable")) %>' />
                </td>
                <td class="table-ui-simple-align-center">
                    <%# "<a href=\"javascript:open_window('m_Certificate.aspx?ID=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(Eval("CertificateId").ToString())) + "',750,600);\" class='editbtn showtooltip' title=" + Resource.Admin_MasterPageAdminCatalog_Edit + "><img src='images/editbtn.gif' style='border: none;' /></a>"%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
