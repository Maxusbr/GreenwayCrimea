<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.FindCustomers" CodeBehind="FindCustomers.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="Resources" %>

<div class="panel-toggle">
    <h2>
        <%= Resources.Resource.Admin_ViewCustomer_Header %></h2>
</div>

<asp:UpdatePanel ID="upCustomers" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel runat="server" DefaultButton="btnFindCustomer">
            <div style="clear: both">
                <div class="customersearch">
                    <asp:TextBox ID="txtSEmail" runat="server" CssClass="niceTextBox textBoxClass" placeholder="Email" />
                    <asp:TextBox ID="txtSFirstName" runat="server" CssClass="niceTextBox textBoxClass" placeholder="<%$ Resources:Resource, Admin_ViewCustomer_Name %>" />
                    <asp:TextBox ID="txtSLastName" runat="server" CssClass="niceTextBox textBoxClass" placeholder="<%$ Resources:Resource, Admin_ViewCustomer_LastName %>" />
                    <asp:TextBox ID="txtSPhone" runat="server" CssClass="niceTextBox textBoxClass" placeholder="<%$ Resources:Resource, Admin_ViewCustomer_Phone %>" />
                </div>
                <div style="margin: 10px 0;">
                    <asp:Button CssClass="btn btn-middle btn-action" ID="btnClear" runat="server" EnableViewState="false"
                        OnClick="btnClear_Click" onmousedown="window.onbeforeunload=null;" Text="<%$ Resources:Resource, Admin_ViewCustomer_Clear %>" style="width: 95px; padding: 8px 0px;" />
                    <asp:Button CssClass="btn btn-middle btn-action" ID="btnFindCustomer" runat="server" EnableViewState="false"
                        OnClick="btnFindCustomer_Click" onmousedown="window.onbeforeunload=null;" Text="<%$ Resources:Resource, Admin_ViewCustomer_Find %>" style="width: 95px; padding: 8px 0px;" />
                </div>

                <div style="width: 100%; height: 1px; border-top: 1px solid #EAEAEA; margin-bottom: 10px;">
                </div>

                <asp:HiddenField ID="currentPage" runat="server" Value="1" />
                <asp:ListView ID="lvCustomers" runat="server" ItemPlaceholderID="itemPlaceHolderId">
                    <LayoutTemplate>
                        <ul class="orders-list">
                            <li runat="server" id="itemPlaceHolderId"></li>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="orders-list-row" onclick='<%# "window.location=\"viewcustomer.aspx?customerid=" + Eval("ID")+ "\"" %>'>
                            <div class="orders-list-name orders-list">
                                <a href='<%# "viewcustomer.aspx?customerid=" + Eval("ID")%>'
                                    class="orders-list-lnk">
                                    <%# Eval("FirstName") + " " + Eval("LastName")%></a>
                            </div>
                            <div class="justify">
                                <span class="justify-item">
                                    <%#Convert.ToDateTime(Eval("RegistrationDateTime")).ToShortDateString() %></span>
                            </div>
                            <div class="justify">
                                <span class="justify-item">Заказов на: 
                                    <%# PriceFormatService.FormatPrice(Convert.ToSingle(Eval("OrdersSum"))) %>
                                </span>
                            </div>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul class="orders-list">
                            <li class="orders-list-row">
                                <div class="orders-list-name orders-list">
                                    <%= Resource.Admin_ViewCustomer_CustomerNotFound %>
                                </div>
                                <div class="justify">
                                </div>
                            </li>
                        </ul>
                    </EmptyDataTemplate>
                </asp:ListView>
                <adv:PageNumberer CssClass="PageNumberer findCustomers" ID="pageNumberer" runat="server" DisplayedPages="3"
                    UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged"
                     />
            </div>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
