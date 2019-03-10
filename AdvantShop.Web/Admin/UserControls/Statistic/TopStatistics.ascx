<%@ Control Language="C#" Inherits="Admin.UserControls.Statistic.TopStatistics" ClientIDMode="AutoID" Codebehind="TopStatistics.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<div class="stat-b-item" style="width: 330px;">
    <div class="clearfix">
        <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_TopCustomersBySum %></h2>
    </div>
    <asp:ListView runat="server" ID="lvCustomers">
        <LayoutTemplate>
            <table class="table-ui">
                <tr>
                    <td></td>
                    <td>
                        <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Resource, Admin_Statistics_CustomerName%>" />
                    </td>
                    <td>
                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Resource, Admin_Statistics_SummPrice%>" />
                    </td>
                </tr>
                <tr runat="server" id="itemPlaceholder"></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td><b><%# Container.DataItemIndex + 1%></b></td>
                <td><%# !string.IsNullOrEmpty((string)Eval("Email")) ? Eval("Email") : Eval("fio") %></td>
                <td style="width: 90px"><%# Convert.ToInt64(Eval("Summary")).ToString("F2") %></td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
                    
<div class="stat-b-item">
    <div class="clearfix">
        <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_TopProductsByAmount %></h2>
    </div>
    <asp:ListView runat="server" ID="lvProducts">
        <LayoutTemplate>
            <table class="table-ui">
                <tr>
                    <td></td>
                    <td>
                        <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Resource, Admin_Statistics_Name%>" />
                    </td>
                    <td style="width: 50px">
                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Resource, Admin_Statistics_ProductsAmount%>" />
                    </td>
                </tr>
                <tr runat="server" id="itemPlaceholder"></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td><b><%# Container.DataItemIndex + 1%></b></td>
                <td>
                    <%# RenderLink(SQLDataHelper.GetInt(Eval("ProductId")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("Name")), SQLDataHelper.GetString(Eval("ArtNo"))) %>
                </td>
                <td><%# Convert.ToInt32(Eval("Summary")) %></td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
                    
<div class="stat-b-item">
    <div class="clearfix">
        <h2 class="chart-orders-title"><%= Resource.Admin_Statistics_TopProductsByAmount %></h2>
    </div>
    <asp:ListView runat="server" ID="lvProductsBySum">
        <LayoutTemplate>
            <table class="table-ui">
                <tr>
                    <td></td>
                    <td>
                        <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Resource, Admin_Statistics_Name%>" />
                    </td>
                    <td>
                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Resource, Admin_Statistics_SummPrice%>" />
                    </td>
                </tr>
                <tr runat="server" id="itemPlaceholder"></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td><b><%# Container.DataItemIndex + 1%></b></td>
                <td>
                    <%# RenderLink(SQLDataHelper.GetInt(Eval("ProductId")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("Name")), SQLDataHelper.GetString(Eval("ArtNo"))) %>
                </td>
                <td><%# Convert.ToInt64(Eval("Summary")).ToString("F2") %></td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>