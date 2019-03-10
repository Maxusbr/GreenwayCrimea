<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.ProductTabs.Modules_DetailsCommonTabs_AddEditTab" CodeBehind="AddEditTab.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        }

        .divAltRow {
            background-color: #eff0f1;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal runat="server" ID="lBase" />
        <div style="text-align: center;">
            <asp:Label ID="lblCustomer" CssClass="head" runat="server" Text="<%$ Resources: DetailsCommonTabsAddEdit_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="subHead" runat="server" Text="<%$ Resources: DetailsCommonTabsAddEdit_SubHeader %>"></asp:Label>
        </div>
        <br />
        <br />
        <div>
            <div class="divRow">
                <asp:Label ID="lblModerated" runat="server" Text='<%$ Resources: DetailsCommonTabsAddEdit_TabTitle %>'></asp:Label><br />
                <asp:TextBox ID="txtTabTitle" runat="server" Width="600px"></asp:TextBox>
            </div>
        </div>
        <div class="divAltRow">
            <div class="divRow">
                <asp:Label ID="Label1" runat="server" Text='<%$ Resources: DetailsCommonTabsAddEdit_TabBody %>'></asp:Label><br />
                <asp:TextBox ID="txtTabBody" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="300px" Width="600px" />
            </div>
        </div>
        <div>
            <div class="divRow">
                <asp:Label ID="Label3" runat="server" Text='<%$ Resources: DetailsCommonTabsAddEdit_Active %>'></asp:Label>
                <asp:CheckBox ID="ckbActive" runat="server" Checked="True" />
            </div>
        </div>
        <div class="divAltRow">
            <div class="divRow">
                <asp:Label ID="Label2" runat="server" Text='<%$ Resources: DetailsCommonTabsAddEdit_SortOrder %>'></asp:Label><br />
                <asp:TextBox ID="txtSortOrder" runat="server"></asp:TextBox>
            </div>
        </div>
        <div style="margin-top: 10px; text-align: center;">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSaveClick" Text='<%$ Resources: DetailsCommonTabsAddEdit_Save %>'
                Width="115px" Height="32px" />
        </div>
        <asp:Label ID="lblError" runat="server" ForeColor="red" Text="<%$ Resources: DetailsCommonTabsAddEdit_Error %>"
            Visible="false" />
    </form>

    <script type="text/javascript">
        window.CKEDITOR_BASEPATH = '<%=(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + (!Request.ApplicationPath.EndsWith("/") ? "/" : string.Empty)) %>vendors/ckeditor/';
    </script>
    <script src="../../vendors/ckeditor/ckeditor.js?update=5.0"></script>
    <script src="../../admin/js/ckeditorInit.js"></script>
</body>
</html>
