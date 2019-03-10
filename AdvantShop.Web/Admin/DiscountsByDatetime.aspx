<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="False"
    AutoEventWireup="true" Inherits="Admin.DiscountsByDatetime"
    Title="" Codebehind="DiscountsByDatetime.aspx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="Discount_PriceRange.aspx">
                <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
            <li class="neighbor-menu-item"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_Voting %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_VotingHistory %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div>
            <table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px;">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/settings_ico.gif" alt="" />
                        </td>
                        <td class="style1">
                            <asp:Label ID="lbl" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_DiscountsByDatetime_Header %>"></asp:Label><br />
                            <asp:Label ID="lblName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_DiscountsByDatetime_SubHeader %>"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>
        </div>
        <div style="text-align: center;">
            <asp:Label ID="lblError" runat="server" ForeColor="Blue" Visible="False"></asp:Label>
            <br />

            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td style="width:70px">
                        <%= Resource.Admin_DiscountsByDatetime_Enabled%>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnabled" runat="server" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width:70px">
                        <%= Resource.Admin_DiscountsByDatetime_Time%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDateTime" runat="server" Width="50px" /> - <asp:TextBox ID="txtToDateTime" runat="server" Width="50px" />
                    </td>
                     <td></td>
                </tr>
                <tr>
                    <td style="width:70px">
                        <%= Resource.Admin_DiscountsByDatetime_Discount%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDiscountByTime" runat="server" Text="0" Width="50px" />%
                    </td>
                     <td></td>
                </tr>
                <tr>
                     <td colspan="3">
                        <%= Resource.Admin_DiscountsByDatetime_ShowPopup%> <asp:CheckBox ID="chkShowPopup" runat="server" />
                     </td>
                </tr>
                <tr>
                     <td colspan="3">
                        <div>
                            <%= Resource.Admin_DiscountsByDatetime_PopupText%>
                        </div>
                          <asp:TextBox ID="FCKDiscountPopupText" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="700px" />
                     </td>
                </tr>                
                <tr>
                    <td colspan="3" style="padding:20px 0 0 0;">
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Resource, Admin_DiscountsByDatetime_Save %>" OnClick="btnSave_OnClick" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
