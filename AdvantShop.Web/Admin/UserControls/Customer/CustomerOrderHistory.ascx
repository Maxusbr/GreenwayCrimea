<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.CustomerOrderHistory" CodeBehind="CustomerOrderHistory.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>

<asp:UpdatePanel ID="upHistory" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0" style="width: 98%">
            <tr>
                <td class="formheader" colspan="2">
                    <h4 style="display: inline; font-size: 10pt;">
                        <%=Resources.Resource.Admin_ViewCustomer_History%>
                    </h4>
                </td>
            </tr>
            <tr class="formheaderfooter">
                <td></td>
            </tr>
            <tr>
                <td>
                    <div style="border: 1px #c9c9c7 solid; width: 100%">
                        <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                            <table class="filter" cellpadding="0" cellspacing="0">
                                <tr style="height: 5px;">
                                    <td colspan="6"></td>
                                </tr>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtOrderId" Width="100px" runat="server"
                                            TabIndex="12" />
                                    </td>
                                    <td style="width: 100px; text-align: center;">
                                        <asp:DropDownList ID="ddlOrderStatus" TabIndex="13" CssClass="dropdownselect" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <div>
                                        </div>
                                    </td>
                                    <td style="width: 55px;">
                                        <center>
                                            <asp:Button ID="btnFilter" runat="server" CssClass="btn" Width="50" OnClientClick="javascript:FilterClick();"
                                                TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        </center>
                                    </td>
                                    <td style="width: 55px;">
                                        <center>
                                            <asp:Button ID="btnReset" runat="server" CssClass="btn" Width="50" OnClientClick="javascript:ResetFilter();"
                                                TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;" colspan="6"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <adv:AdvGridView ID="grid" runat="server" AutoGenerateColumns="False" CellPadding="0"
                            CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                            CssClass="tableview" GridLines="None" ReadOnlyGrid="True">
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 readonlyrow" />
                            <EmptyDataTemplate>
                                <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_NoRecords %>"></asp:Localize>
                                </center>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:HyperLinkField HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_OrderNum %>"
                                    DataNavigateUrlFields="OrderID" DataTextField="Number" DataNavigateUrlFormatString="~/admin/vieworder.aspx?OrderID={0}"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100" ItemStyle-Width="100px" />
                                <asp:BoundField HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_Status %>"
                                    DataField="OrderStatusID" Visible="false" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField AccessibleHeaderText="StatusName" HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_Status %>"
                                    HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="StatusName" runat="server" Text='<%# Eval("StatusName") %>' Width="99%"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_Buyer %>"
                                    HeaderStyle-Width="150" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <a href="<%# "ViewCustomer.aspx?CustomerID=" + (Guid )Eval("CustomerID") %>" class="Link">
                                            <%#Eval("FirstName") + " " + Eval("LastName")%></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_PaymenType %>"
                                    DataField="PaymentMethod" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_ShippingType %>"
                                    DataField="ShippingMethod" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_Sum %>"
                                    HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# PriceFormatService.FormatPrice(SQLDataHelper.GetFloat(Eval("Sum")),Convert.ToSingle(Eval("CurrencyValue")), (string)Eval("CurrencySymbol"), (string)Eval("CurrencyCode"), (bool)Eval("IsCodeBefore")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_ViewCustomer_OrderSearch_OrderDate %>"
                                    HeaderStyle-Width="150" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#AdvantShop.Localization .Culture.ConvertDate((DateTime )Eval("OrderDate"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <center style="margin-top: 20px; margin-bottom: 20px;">
                                                                    <%=Resources.Resource.Admin_Catalog_NoRecords%>
                                                                </center>
                            </EmptyDataTemplate>
                        </adv:AdvGridView>
                        <div style="border-top: 1px #c9c9c7 solid;">
                        </div>
                        <table class="results2">
                            <tr>
                                <td style="width: 157px; padding-left: 6px;">
                                    <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                        runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                        UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                </td>
                                <td style="width: 157px; text-align: right; padding-right: 12px">
                                    <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                        <span style="color: #494949">
                                            <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                Width="30" /></span>
                                        <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                            OnClick="linkGO_Click" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
