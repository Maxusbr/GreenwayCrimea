<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.BuyInTime.BuyInTimeModuleAddEdit" CodeBehind="BuyInTimeModuleAddEdit.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            width: 200px;
        }

        .desc {
            color: gray;
            font-size: 13px;
        }

        p {
            margin: 0;
            padding: 6px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="Server" ID="scriptManager" ScriptMode="Release" />
        <div style="text-align: center;">
            <asp:Label ID="lblCustomer" CssClass="head" runat="server" Text="<%$ Resources: BuyInTime_AddEdit_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="subHead" runat="server" Text="<%$ Resources: BuyInTime_AddEdit_SubHeader %>"></asp:Label>
        </div>
        <br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlShowMode" EventName="SelectedIndexChanged" />
                </triggers>
            <contenttemplate>
                    <div>
                        <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="red" />
                    </div>

                    <table class="tableview" style="width: 100%" cellpadding="3px">
                        <tr>
                            <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_ArtNo%>" />:
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductArtNo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_Discount%>" />:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDiscount" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_DateStart%>" />:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDateStart" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_DateExpired%>" />:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDateExpired" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdt">
                        <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: BuyInTime_AddEdit_SortOrder%>" />:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSortOrder" />
                    </td>
                </tr>
                <tr>
                    <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_IsRepeat%>" />:
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkIsRepeat" />
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: BuyInTime_AddEdit_By%>" />
                                <asp:TextBox runat="server" ID="txtDaysRepeat" Text="1" /> <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: BuyInTime_AddEdit_Days%>" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_ShowMode%>" />:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlShowMode">
                                    <asp:ListItem Text="<%$ Resources: BuyInTime_AddEdit_ShowMode_None%>" Value="0" />
                                    <asp:ListItem Text="<%$ Resources: BuyInTime_AddEdit_ShowMode_Horizontal%>" Value="1" />
                                    <asp:ListItem Text="<%$ Resources: BuyInTime_AddEdit_ShowMode_Vertical%>" Value="2" Selected="True" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_ProductPicture%>" />: <br/>
                                <span class="desc">
                                    (<asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_ProductPicture_Description%>" />)
                                </span>
                            </td>
                            <td>
                                <asp:Panel ID="pnlPicture" runat="server" Width="100%" Visible="False">
                                    <div>
                                        <asp:Literal runat="server" ID="liPicture" />
                                        <asp:Literal runat="server" ID="liPictureName" Visible="false"/>
                                    </div>
                                    <asp:Button ID="DeletePicture" runat="server" Text="<%$ Resources: Admin_Delete%>"
                                                OnClick="DeletePicture_Click" />
                                </asp:Panel>
                                <div id="fuPuctureDiv" runat="server">
                                    <asp:FileUpload runat="server" ID="fuPucture" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_Action%>" />
                                <br/><br/>
                                 <asp:TextBox ID="ckeActionText" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" Width="100%" />
                                <br/>
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_ActionText_Hint%>" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdt">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_ShowInMobile%>" />:
                            </td>
                            <td>
                                <asp:Checkbox ID="chkShowInMobile" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_MobileAction%>" />
                                <br/><br/>
                                 <asp:TextBox ID="ckeMobileActionText" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" Width="100%" />
                                <br/>
                                <asp:Localize runat="server" Text="<%$ Resources: BuyInTime_AddEdit_ActionText_Hint%>" />
                            </td>
                        </tr>
                    </table>
                </contenttemplate>
        </asp:UpdatePanel>

        <div style="margin: 10px 0 50px 0; text-align: center">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" EnableViewState="false" Text="<%$ Resources: BuyInTime_AddEdit_Save %>" />
        </div>
    </form>
    <script type="text/javascript">
        window.CKEDITOR_BASEPATH = '<%=(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + (!Request.ApplicationPath.EndsWith("/") ? "/" : string.Empty)) %>vendors/ckeditor/';
    </script>
    <script src="../../vendors/ckeditor/ckeditor.js?update=5.0"></script>
    <script src="../../admin/js/ckeditorInit.js?update=5.0"></script>
</body>
</html>
