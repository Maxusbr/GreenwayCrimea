<%@ Page Language="C#" AutoEventWireup="true" Inherits="Advantshop.Modules.UserControls.BuyMore.BuyMoreModuleAddEdit" CodeBehind="BuyMoreModuleAddEdit.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <asp:Literal runat="server" ID="lBase" />
    <style>
        .head {
            font-family: Verdana;
            font-size: 18pt;
            text-transform: uppercase;
        }

        .subHead {
            color: #666666;
            font-family: Verdana;
            font-size: 10pt;
        }

        .divRow {
            margin: auto;
            padding: 10px 0px 10px 0px;
            width: 800px;
        }

        .divAltRow {
            background-color: #eff0f1;
        }

        .tableview {
            margin-top: 20px;
        }

            .tableview tbody tr:nth-child(odd) {
                background: #fdfdfd;
            }

            .tableview tbody tr:nth-child(even) {
                background: #f4f4f4;
            }

        .tdt {
            width: 50%;
        }

        p {
            margin: 0;
            padding: 6px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center;">
            <asp:Label ID="lblCustomer" CssClass="head" runat="server" Text="<%$ Resources: Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="subHead" runat="server" Text="<%$ Resources: SubHeader %>"></asp:Label>
        </div>

        <div>
            <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="red" />
        </div>

        <table class="tableview" style="width: 100%" cellpadding="3px">
            <tr>
                <td class="tdt">
                    <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: OrderPriceFrom%>" />:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtOrderPriceFrom" />
                </td>
            </tr>

            <tr>
                <td class="tdt">
                    <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: FreeShipping%>" />:
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbFreeShipping" />
                </td>
            </tr>
            <tr>
                <td class="tdt">
                    <asp:Localize runat="server" Text="<%$ Resources: GiftProductArtNo%>" />:
                </td>
                <td>
                    <asp:TextBox ID="txtProductArtNo" runat="server" /><asp:Button ID="btnAddOffer" runat="server" Text="Добавить" OnClick="btnAddOffer_Click"/><br>
                    <asp:ListView ID="lvProducts" runat="server" OnItemCommand="lvProducts_ItemCommand" ItemPlaceholderID="itemPlaceholderID">
                        <layouttemplate>
                            <div ID="itemPlaceholderID" runat="server"></div>
                        </layouttemplate>
                        <itemtemplate>
                            <div><%# Eval("ArtNo") %> - <%# Eval("Product") != null ? Eval("Product.Name") : "Продукт удален" %> <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteOffer" CommandArgument='<%# Eval("OfferId") %>'><img src="../../Modules/BuyMore/images/remove.jpg"/></asp:LinkButton> </div>
                        </itemtemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>

        <div style="margin: 30px 0 10px 0; text-align: center">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" EnableViewState="false" Text="<%$ Resources: Save %>" />
        </div>
    </form>
    <script type="text/javascript">
        window.CKEDITOR_BASEPATH = '<%=(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + (!Request.ApplicationPath.EndsWith("/") ? "/" : string.Empty)) %>vendors/ckeditor/';
    </script>
    <script src="../../vendors/ckeditor/ckeditor.js?update=5.0"></script>
    <script src="../../admin/js/ckeditorInit.js"></script>
</body>
</html>
