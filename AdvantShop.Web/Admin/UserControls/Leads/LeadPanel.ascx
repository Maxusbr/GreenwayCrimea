<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Admin.UserControls.Leads.LeadPanel" Codebehind="LeadPanel.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<div class="panel-toggle">
    <h2>
        <%= Resources.Resource.Admin_Leads_Header %>
    </h2>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True" ID="upSearchOrders">
        <ContentTemplate>
            <div class="justify list-order-status-item">
                <div class="justify-item list-order-status-name">
                    <asp:LinkButton ID="lbtnAllOrders" CssClass="list-order-status-lnk" runat="server"
                        OnClick="lbtnAllOrders_Click"><%= Resources.Resource.Admin_Leads_AllLeads %></asp:LinkButton>
                </div>
                <div class="justify-item list-order-status-count">
                    <asp:Label ID="lblTotalOrdersCount" runat="server" Text=""></asp:Label>
                </div>
            </div>
            
            <asp:ListView ID="lvStatuses" runat="server" ItemPlaceholderID="itemPlaceholderID" OnItemCommand="lvOrderStatuses_OnItemCommand">
                <LayoutTemplate>
                    <ul class="list-order-status">
                        <li id="itemPlaceholderID" runat="server"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="justify list-order-status-item">
                        <div class="justify-item list-order-status-name" style='<%# "border-left-color: " + Eval("Color") %>' data-status-color="<%# Eval("Color") %>">
                            <asp:LinkButton ID="lb" CssClass="list-order-status-lnk" runat="server" CommandArgument='<%# Eval("Value") %>' CommandName="ShowByStatus">
                                <span class="<%# SQLDataHelper.GetString(Eval("Value")) == Status ? "bold" : string.Empty %>"><%# Eval("Name") %></span>
                            </asp:LinkButton>
                        </div>
                        <div class="justify-item list-order-status-count">
                            <%# Eval("LeadsCount")%>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <div style="border-bottom: 1px solid #cbcbcb; margin-bottom: 20px;"></div>

            <asp:ListView ID="lvOrders" runat="server" ItemPlaceholderID="itemPlaceHolderId">
                <LayoutTemplate>
                    <ul class="orders-list">
                        <li runat="server" id="itemPlaceHolderId"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="orders-list-row">
                        <div>
                            <a href='<%# "editlead.aspx?id=" + Eval("Id") %>' class="orders-list-lnk">
                                <%#"№ " + Eval("Id") + " - " + Eval("Name") %></a>
                        </div>
                        <div><%#Eval("CreatedDate") %></div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="5"
                UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged"
                Width="250" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
