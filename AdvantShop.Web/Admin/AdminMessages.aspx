<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master"
    Inherits="Admin.AdminMessages" Codebehind="AdminMessages.aspx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>

<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-own">
        <div id="inprogress" style="display: none;">
            <div id="curtain" class="opacitybackground">
                &nbsp;
            </div>
            <div class="loader">
                <table style="font-weight: bold; text-align: center; width: 100%;">
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #0D76B8; text-align: center;">
                            <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="text-align: center;">
            <table style="width: 100%; padding: 0px; margin: 0px;">
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblReview" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_AdminMessages_MessagesAdvantshop %>" />
                        <br />
                        <asp:Label ID="lblReviewName" CssClass="AdminSubHead" runat="server" Text="" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="height: 10px;">
        </div>
        <div>
            <input type="button" id="btnCheckViewed" value="<%= Resources.Resource.Admin_AdminMessages_MarkAsRead %>" onclick="setViewedCheckedAdminMessages()" />
            <input type="button" id="btnCheckNotViewed" value="<%= Resources.Resource.Admin_AdminMessages_MarkAsUnread %>" onclick="setNotViewedCheckedAdminMessages()" />
            <br />
            <br />
            <asp:ListView ID="lvAdminMessages" runat="server" ItemPlaceholderID="itemPlaceholderID">
                <LayoutTemplate>
                    <table class="table-ui">
                        <thead>
                            <tr>
                                <th class="table-adminmessages-checkbox">
                                    <input type="checkbox" id="checkAll" onclick="checkAllAdminMessages(this)" />
                                </th>
                                <th class="table-ui-align-left table-adminmessages-date">
                                    <asp:Literal ID="lblDate"  runat="server" Text="<%$ Resources:Resource, Admin_AdminMessages_Date %>"></asp:Literal>
                                </th>
                                <th class="table-ui-align-left">
                                    <asp:Literal ID="lblSubject" runat="server" Text="<%$ Resources:Resource, Admin_AdminMessages_Subject %>"></asp:Literal>
                                </th>
                                <th class="table-ui-align-left">
                                    <asp:Literal ID="lblType" runat="server" Text="<%$ Resources:Resource, Admin_AdminMessages_Type %>"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr runat="server" id="itemPlaceholderID">
                            </tr>
                        </tbody>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr <%# "class=\"" + (Convert.ToBoolean( Eval("Viewed")) ? "adminMessageRowViewed": "adminMessageRowNotViewed" ) + "\"" %>>
                        <td class="table-ui-align-center">
                            <input type="checkbox" id="ckbCheck" class="adminMessageCheckBox" data-amid="<%# Eval("ID") %>" />
                        </td>
                        <td class="modalShowRow">
                            <%# ((DateTime)Eval("DateChange")).ToShortDateTime()%>
                        </td>
                        <td class="modalShowRow">
                            <%#Eval("Subject") %>
                        </td>
                        <td class="modalShowRow">
                            <%# Eval("MessageTypeString")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div style="text-align: center; font-size: 15px; padding: 20px; width: 100%;">
                        Сообщений нет
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>
    <div style="display: none;">
        <div id="modalAdminMessage">
        </div>
    </div>
</asp:Content>
