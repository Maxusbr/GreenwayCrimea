<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.LogViewerDetailed" Codebehind="LogViewerDetailed.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
        <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="CommonSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
            <li class="neighbor-menu-item"><a href="CheckoutFields.aspx">
                <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
            <li class="neighbor-menu-item"><a href="PaymentMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
            <li class="neighbor-menu-item"><a href="ShippingMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
            <li class="neighbor-menu-item"><a href="Country.aspx">
                <%= Resource.Admin_MasterPageAdmin_Countries%></a></li>
            <li class="neighbor-menu-item"><a href="Currencies.aspx">
                <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
            <li class="neighbor-menu-item"><a href="Taxes.aspx">
                <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
            <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
            <li class="neighbor-menu-item"><a href="LogError404.aspx">
                <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
            <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div style="text-align: center;">
            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_HeaderDetails %>"></asp:Label>
            <br />
            <br />
            /&nbsp;<asp:HyperLink ID="hfBack" runat="server" CssClass="Link"><%= Resources.Resource.Admin_MasterPageAdmin_BugTracker_GoBack%></asp:HyperLink>&nbsp;/
            <br />
            <br />
            <asp:Label runat="server" ID="lblErr" ForeColor="Red"></asp:Label>
            <br />
            <table class="subform" id="tabs">
                <tr>
                    <td style="width: 225px;">
                        <ul id="tabs-headers">
                            <li id="commoninfo">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_CommonInfo %>" />
                            </li>
                            <li id="request">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_Request %>" />
                            </li>
                            <li id="browser">
                                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_Browser %>" /></li>
                            <li id="session">
                                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_Session %>" />
                            </li>
                        </ul>
                    </td>
                    <td id="tabs-contents">
                        <%=OutHtml.ToString() %>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
