<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.StoreReviews.Modules_StoreReviews_EditReview" CodeBehind="EditReview.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        .reviewEditTable {
            width: 100%;
            border-collapse: collapse;
        }

            .reviewEditTable td {
                height: 30px;
                width: 50%;
            }

            .reviewEditTable tr:nth-of-type(odd) {
                background-color: #eff0f1;
            }

            .reviewEditTable .tdRight {
                text-align: right;
                padding-right: 10px;
            }

            .reviewEditTable .tdLeft {
                text-align: left;
                padding-left: 10px;
            }

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center;">
            <asp:Label ID="lblCustomer" CssClass="head" runat="server" Text="<%$ Resources: StoreReviews_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="subHead" runat="server" Text="<%$ Resources: StoreReviews_SubHeader %>"></asp:Label>
        </div>
        <br />
        <br />
        <table class="reviewEditTable">
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblDate" runat="server" Text='<%$ Resources: StoreReviews_DateAdded %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtDateAdded" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblEmail" runat="server" Text='<%$ Resources: StoreReviews_ReviewerEmail %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtEmail" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="Label3" runat="server" Text='<%$ Resources: StoreReviews_ReviewerName %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtReviewerName" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblModerated" runat="server" Text='<%$ Resources: StoreReviews_Moderated %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:CheckBox ID="ckbModerated" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources: StoreReviews_Rate %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:RadioButtonList ID="rblRating" runat="server" RepeatDirection="Horizontal" Width="100px">
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="Label2" runat="server" Text='<%$ Resources: StoreReviews_Review %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtReview" runat="server" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources: StoreReviews_Image %>"></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:Panel ID="pnlReviewerImage" runat="server" Visible="False">
                        <asp:Image ID="imgReviewerImage" runat="server" />
                        <br />
                        <asp:Button ID="btnDeleteReviewerImage" runat="server" Text="<%$ Resources: StoreReviews_Delete %>" OnClick="btnDeleteReviewerImage_Click"/>
                    </asp:Panel>
                    <asp:FileUpload ID="fuReviewerImage" runat="server" />
                </td>
            </tr>
        </table>
        <div style="text-align: center; margin-top: 10px;">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSaveClick" Text='<%$ Resources: StoreReviews_Save %>' />
        </div>
        <asp:Label ID="lError" runat="server" ForeColor="red" Text="<%$ Resources: StoreReviews_WrongDateFormat %>" Visible="false" />
    </form>
</body>
</html>
