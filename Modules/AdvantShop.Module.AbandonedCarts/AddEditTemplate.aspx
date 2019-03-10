<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.AbandonedCarts.AddEditTemplate" CodeBehind="AddEditTemplate.aspx.cs" %>

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

        .tableview tbody tr:nth-child(odd) {
            background: #fdfdfd;
        }

        .tableview tbody tr:nth-child(even) {
            background: #f4f4f4;
        }

        .tdt {
            width: 150px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center;">
            <asp:Label ID="lblCustomer" CssClass="head" runat="server" Text="<%$ Resources: AddEdit_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="subHead" runat="server" Text="<%$ Resources: AddEdit_SubHeader %>"></asp:Label>
        </div>
        <br />
        <br />
        <div>
            <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="red" />
        </div>

        <table class="tableview" style="width: 100%" cellpadding="3px">
            <tr>
                <td class="tdt">
                    <asp:Localize runat="server" Text="<%$ Resources: Name%>" />:
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="400px" />
                </td>
            </tr>
            <tr>
                <td class="tdt">
                    <asp:Localize runat="server" Text="<%$ Resources: Active%>" />:
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="chkActive" />
                </td>
            </tr>
            <tr>
                <td class="tdt">
                    <asp:Localize runat="server" Text="<%$ Resources: SendingTime%>" />:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSendingTime" Width="100px" Text="1" />
                </td>
            </tr>
            <tr>
                <td class="tdt">
                    <asp:Localize runat="server" Text="<%$ Resources: Subject%>" />:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSubject" Width="430px" />
                </td>
            </tr>
            <tr>
                <td class="tdt">
                    <asp:Localize runat="server" Text="<%$ Resources: Body%>" />:
                </td>
                <td>
                    <asp:TextBox ID="ckeBody" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400px" Width="100%" />
                </td>
            </tr>
        </table>

        <div style="margin: 10px 0 50px 0; text-align: center">
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
