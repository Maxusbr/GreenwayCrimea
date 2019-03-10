<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.CertificatesOptions" Codebehind="CertificatesOptions.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;</div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td align="center">
                            <img src="~/admin/images/ajax-loader.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="color: #0D76B8;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="content-own">
        <div style="margin: 0 auto; width: 470px">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Client_GiftCertificate_Header %>"></asp:Label><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Client_GiftCertificate_Options_SubHead %>"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="margin: 20px auto; width: 400px; text-align: center;">
            <a href="Certificates.aspx"><%= Resources.Resource.Admin_Certificates_BackToList %></a>
        </div>
        <div style="margin: 20px auto; width: 400px">
            <asp:ListView ID="lvTaxes" runat="server" ItemPlaceholderID="itemPlaceHolderTaxID">
                <LayoutTemplate>
                    <div style="text-align:center; font-weight:bold; margin-bottom:5px; font-size:14px;">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_Certificates_Taxes %>" />
                    </div>
                    <table cellpadding="3px">
                        <tr runat="server" id="itemPlaceHolderTaxID">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfTaxId" runat="server" Value='<%# Eval("TaxId")%>' />
                            <asp:CheckBox ID="ckbActive" runat="server" CssClass="checkly-align"
                                Checked='<%# GiftCertificateTaxes.Any(item=>item == Convert.ToInt32(Eval("TaxId"))) %>' />
                        </td>
                        <td style="padding-left:5px;">
                            <%# Eval("Name") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div style="margin: 20px auto; width: 400px;">
            <asp:ListView ID="lvPaymentMethods" runat="server" ItemPlaceholderID="itemPlaceHolderID">
                <LayoutTemplate>
                    <div style="text-align: center; font-weight: bold; margin-bottom:5px; font-size:14px;">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_Certificates_PaymentMethods %>" />
                    </div>
                    <table cellpadding="3px">
                        <tr runat="server" id="itemPlaceHolderID">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ckbActive" runat="server" CssClass="checkly-align"
                                Checked='<%# GiftCertificatePaymentMethods.Any(item=>item == Convert.ToInt32(Eval("PaymentMethodId"))) %>' />
                            <asp:HiddenField ID="hfPaymentId" runat="server" Value='<%#Eval("PaymentMethodId") %>' />
                        </td>
                        <td style="padding-left:5px;">
                            <%# Eval("Name") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div style="text-align: center">
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-middle btn-add"
                Text="<%$ Resources:Resource, Admin_Certificates_Save %>" OnClick="btnSaveClick" />
        </div>
        <div style="text-align:center; margin:0 auto; width: 500px; margin-top:25px;">
            <asp:Label ID="lblMessage" runat="server" Text="" />
        </div>
    </div>
</asp:Content>
