<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="Admin.LandingPage" %>

<%@ Import Namespace="Resources" %>

<%@ Register Src="~/Admin/UserControls/Menu/MarketingNeighborMenu.ascx" TagName="MarketingNeighborMenu" TagPrefix="adv" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <adv:MarketingNeighborMenu ID="MarketingNeighborMenu" runat="server" />
    </div>
    <div class="content-own">
        <div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td style="vertical-align: top;">
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="Landing page"></asp:Label><br />
                            
                        </td>
                        <td style="text-align: right; vertical-align: bottom;">
                            <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" Text="<%$ Resources:Resource, Admin_Save %>" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="width: 800px; margin: auto; margin-top: 10px;">

                <table class="info-tb" style="width: 100%;">

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            Активность
                        </td>
                        <td>
                            <asp:CheckBox ID="chbLandingPageActive" runat="server" />
                        </td>
                    </tr>
                    <tr class="rowPost ">
                        <td style="text-align: left;" colspan="2">
                            <span style="font-weight: bold; font-size: 14px;">
                                <asp:Localize ID="Localize3" runat="server" Text="Дополнительное описание (не используется, если заполнено у товара)"></asp:Localize>
                            </span>
                            <br />
                            <br />
                            <div>
                                <asp:TextBox ID="txtProductLandingPageCommonStatic" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400px" Width="100%" />
                            </div>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>






</asp:Content>
