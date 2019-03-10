<%@ Page AutoEventWireup="true" Inherits="Admin.SendMessage"
    Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="false" Codebehind="SendMessage.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Discount_PriceRange.aspx">
                <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
            <li class="neighbor-menu-item"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item selected"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resources.Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
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
            <li class="neighbor-menu-item"><a href="Statistics.aspx">
                <%= Resources.Resource.Admin_Statistics_Header %></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div style="text-align: center;">
            <asp:Label CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_Header %>"></asp:Label>
            <br />
            <asp:Label CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_SubHeader %>"></asp:Label>&nbsp;
            <br />
            <br />
        </div>
        <div style="text-align: center;">
            <asp:Label ID="lblInfo" runat="server" ForeColor="Blue"></asp:Label>
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            <br />
        </div>
        <div id="divMessage" runat="server" style="text-align: center; width: 800px; margin: auto;">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <br />
            <a href="<%= UrlService.GetAdminAbsoluteLink("SendMessage.aspx") %>">
                <%=Resources.Resource.Admin_m_News_SendAgain%></a>
        </div>
        <div id="divSend" runat="server" style="margin: 0px 5px 0px 5px">
            <div style="text-align: center;">
                <table id="table2" style="width: 100%; height: 50px; background-color: #eff0f1; margin: 0px; padding: 0px; border: none;">
                    <tr>
                        <td style="height: 27px; text-align: center;">
                            <div style="text-align: left; width: 800px; margin: auto;">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_TitleAnnotation %>"
                                    Font-Bold="True"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_SendMessage_Required %>"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtTitle" runat="server" Width="309px"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; margin: 0px; padding: 0px; border: none;">
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <div style="text-align: left; width: 800px; margin: auto;">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_Text %>"
                                    Font-Bold="True"></asp:Label>
                                <asp:Label ID="Label13" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_SendMessage_Required %>"></asp:Label>
                                <br />
                                 <asp:TextBox ID="fckMailContent" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg"  Width="800px" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div style="text-align: center;">
                <div style="width: 800px; margin: auto; text-align: left;">
                    <span style="font-weight: bold;">
                        <%=Resources.Resource.Admin_SendMessage_SendNews%></span>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 625px;">
                                <asp:CheckBox runat="server" ID="ckbSubscribers" Text="<%$ Resources:Resource, Admin_SendMessage_Subsribers %>" />
                            </td>
                            <td style="text-align: right; padding-right: 20px;">
                                <span><a href="Subscription.aspx" target="_blank" class="Link">
                                    <%=Resources.Resource.Admin_SendMessage_ViewAddresses%></a></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="ckbOrderedCustomers" Text="<%$ Resources:Resource, Admin_SendMessage_OrderedCusomers %>" />
                            </td>
                            <td style="text-align: right; padding-right: 20px;"></td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <div style="text-align: center;">
                <asp:Button ID="btnSend" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_Send %>"
                    Width="101px" OnClick="btnSend_Click" />
            </div>
            <br />
        </div>
    </div>
</asp:Content>
