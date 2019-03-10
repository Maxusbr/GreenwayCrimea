<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.StatisticsFilter" CodeBehind="StatisticsFilter.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;
        </div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #0D76B8; text-align: center;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Discount_PriceRange.aspx">
                <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
            <li class="neighbor-menu-item"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resource.Admin_MasterPageAdmin_Voting %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resource.Admin_MasterPageAdmin_VotingHistory %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item selected"><a href="Statistics.aspx">
                <%= Resource.Admin_Statistics_Header %></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">

        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width: 72px;">
                    <img src="images/orders_ico.gif" alt="" />
                </td>
                <td>
                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Statistics_Header %>"></asp:Label><br />
                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Statistics_SubHeader %>"></asp:Label>
                </td>
            </tr>
        </table>
        <div style="padding: 15px 0; font-size: 15px;">
            <asp:Literal ID="lblTitle" runat="server" Mode="PassThrough" />
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                <asp:AsyncPostBackTrigger ControlID="grid" EventName="DataBinding" />
            </Triggers>
            <ContentTemplate>
                <div id="tbFound" runat="server" visible="False">
                    <table style="width: 100%">
                        <td class="style1" style="text-align: right; padding: 5px 0;">
                            <%= Resource.Admin_Catalog_Total %>
                            <span class="bold">
                                <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resource.Admin_Catalog_RecordsFound%>
                        </td>
                    </table>
                </div>
                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                    CellSpacing="0" CssClass="tableview" GridLines="None" ReadOnlyGrid="True">
                    <Columns>
                        <asp:TemplateField AccessibleHeaderText="ArtNo" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                <%=Resource.Admin_Catalog_StockNumber%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("ArtNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                <%=Resource.Admin_Catalog_Name%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="Price" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                            <HeaderTemplate>
                                <%=Resource.Admin_Catalog_Price%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("CurrencyIso3")!= DBNull.Value &&  Eval("Price")!= DBNull.Value ? PriceFormatService.FormatPrice(Convert.ToSingle(Eval("Price")),Convert.ToSingle(Eval("CurrencyValue")),Convert.ToString(Eval("Code")),Convert.ToString(Eval("CurrencyIso3")),Convert.ToBoolean(Eval("IsCodeBefore"))):String.Format("{0:##,##0.00}", Eval("Price")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="header" />
                    <RowStyle CssClass="row1 readonlyrow" />
                    <AlternatingRowStyle CssClass="row2 readonlyrow" />
                    <EmptyDataTemplate>
                        <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                            <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_NoRecords %>"></asp:Localize>
                        </div>
                    </EmptyDataTemplate>
                </adv:AdvGridView>

                <adv:AdvGridView ID="gridCustomers" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                    CellSpacing="0" CssClass="tableview" GridLines="None" ReadOnlyGrid="True">
                    <Columns>
                        <asp:TemplateField Visible="false" AccessibleHeaderText="CustomerId">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("CustomerId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                Имя
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <%# Convert.ToString(Eval("CustomerId")) != Guid.Empty.ToString() ?  "<a href=\"ViewCustomer.aspx?CustomerID="+ Eval("CustomerId") + "\">" + Eval("Name") + "</a>" : Eval("Name").ToString()  %>
                                <%--<a href="ViewCustomer.aspx?CustomerID=<%# Eval("CustomerId") %>">
                                    <asp:Literal ID="txtCustomerName" runat="server" Text='<%# Eval("Name") %>' />
                                </a>--%>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="Email" HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                Email
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="txtEmail" runat="server" Text='<%# Eval("Email") %>' Width="99%"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="Phone" HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                Телефон
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="txtPhone" runat="server" Text='<%# Eval("Phone") %>' Width="99%"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="Phone" HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                Заказ
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <a href="ViewOrder.aspx?OrderID=<%# Eval("OrderId") %>">
                                    <asp:Literal ID="txtOrderId" runat="server" Text='<%# Eval("OrderId") %>' />
                                </a>                              
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="header" />
                    <RowStyle CssClass="row1 readonlyrow" />
                    <AlternatingRowStyle CssClass="row2 readonlyrow" />
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                </adv:AdvGridView>

                <table class="results2">
                    <tr>
                        <td style="width: 157px; padding-left: 6px;"></td>
                        <td style="text-align: center;">
                            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7" UseHref="false"
                                OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
                        </td>
                        <td style="width: 157px; text-align: right; padding-right: 12px"></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>




    </div>
</asp:Content>
