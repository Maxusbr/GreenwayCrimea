<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Blog.BlogEditCategory" CodeBehind="BlogEditCategory.aspx.cs" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        .reviewEditTable {width: 100%;border-collapse: collapse;}
        .reviewEditTable td {height: 30px;width: 50%;}
        .reviewEditTable tr.altRow {background-color: #eff0f1;}
        .reviewEditTable .tdRight {text-align: right;padding-right: 10px;}
        .reviewEditTable .tdLeft {text-align: left;padding-left: 10px;}
        .reviewEditTable input, .reviewEditTable textarea {width: 200px}
        .head {font-family: Verdana;font-size: 18pt;text-transform: uppercase;}
        .subHead {color: #666666;font-family: Verdana;font-size: 10pt;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal runat="server" ID="lBase" />
        <div style="text-align: center;">
            <asp:Label ID="lblHeader" CssClass="head" runat="server" Text="<%$ Resources: Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblSubHeader" CssClass="subHead" runat="server" Text="<%$ Resources: SubHeader %>"></asp:Label>
            <br />
            <asp:Label ID="lError" runat="server" ForeColor="red" Text="<%$ Resources: WrongDateFormat %>" Visible="false" />
        </div>
        <br />
        <br />
        <table class="reviewEditTable">
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblName" runat="server" Text='<%$ Resources: Title %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="altRow">
                <td class="tdRight">
                    <asp:Label ID="lblUrlPath" runat="server" Text='<%$ Resources: UrlPath %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtUrlPath" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources: SortOrder %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox runat="server" ID="txtSortOrder" />
                </td>
            </tr>
            <tr class="altRow">
                <td class="tdRight">
                    <asp:Label ID="lblMetaTitle" runat="server" Text='<%$ Resources: MetaTitle %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtMetaTitle" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblMetaKeywords" runat="server" Text='<%$ Resources: MetaKeywords %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" Height="50px" />
                </td>
            </tr>
            <tr class="altRow">
                <td class="tdRight">
                    <asp:Label ID="lblMetaDescription" runat="server" Text='<%$ Resources: MetaDescription %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Height="50px" />
                </td>
            </tr>
        </table>
        <div style="text-align: center; margin-top: 10px;">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSaveClick" Text='<%$ Resources: Save %>' />
        </div>
    </form>
</body>
</html>
