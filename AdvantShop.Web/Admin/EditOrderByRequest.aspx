<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.EditOrderByRequest" Codebehind="EditOrderByRequest.aspx.cs" %>
<%@ Import Namespace="Resources" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 150px;
            text-align: left;
            vertical-align: top;
            height: 30px;
            padding: 7px 0px 0px 15px;
        }
        
        .style2
        {
            background-color: #F0F0F0;
            padding: 3px 0px 3px 0px;
        }
        
        .style3
        {
            width: 850px;
            padding: 7px 0px 3px 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="OrderSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
            <li class="neighbor-menu-item"><a href="OrderStatuses.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderStatuses%></a></li>
            <li class="neighbor-menu-item selected"><a href="OrderByRequest.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderByRequest%></a></li>
            <li class="neighbor-menu-item"><a href="ExportOrdersExcel.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrdersExcelExport%></a></li>
            <li class="neighbor-menu-item"><a href="Export1C.aspx">
                <%= Resource.Admin_MasterPageAdmin_1CExport%></a></li>
        </menu>
        <div class="panel-add">
            <%= Resource.Admin_MasterPageAdmin_Add %>
            <a href="EditOrder.aspx" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Order %></a>
        </div>
    </div>
    <div style="margin: 15px 10px 0px 20px;">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_Header %>" /><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_RequestDate %>" />
                        <asp:Literal ID="lOrderDate" runat="server" /><br />
                        <asp:CheckBox ID="chkIsComplete" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_IsComplete %>"
                            TextAlign="Left" />
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td>
                    <asp:HyperLink ID="Hyperlink1" NavigateUrl="~/Admin/OrderByRequest.aspx" Text='<%$ Resources: Resource, Admin_Back %>'
                        runat="server" CssClass="Link"></asp:HyperLink>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Visible="false"></asp:Label>
                    <asp:Label ID="Message" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                </td>
                <td style="width: 105px;">
                    <asp:Button CssClass="btn btn-middle btn-add" CausesValidation="false" ID="btnSave" runat="server" Text='<%$ Resources:Resource, Admin_Update %>'
                        OnClick="btnSave_Click" />
                </td>
                <td style="width: 150px;">
                    <asp:Button CssClass="btn btn-middle btn-add" CausesValidation="false" ID="btnDeleteOrder" runat="server"
                        Text='<%$ Resources:Resource, Admin_OrderByRequest_DeleteOrder %>' 
                        onclick="btnDeleteOrder_Click" />
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td class="formheader_order" colspan="2">
                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="12pt" Text="<%$ Resources: Resource, Admin_OrderByRequest_Customer %>"></asp:Label>
                </td>
            </tr>
            <tr class="style2">
                <td class="style1">
                    <asp:Label ID="lblUserName" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_UserName %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" Width="300px" />
                    <asp:Label ID="lUserNameError" runat="server" Text="*" Visible="false" Font-Bold="true"
                        ForeColor="Red" />
                </td>
            </tr>
            <tr class="style2">
                <td class="style1">
                    <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_Email %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" />
                    <asp:Label ID="lEmailError" runat="server" Text="*" Visible="false" Font-Bold="true"
                        ForeColor="Red" />
                </td>
            </tr>
            <tr class="style2">
                <td class="style1">
                    <asp:Label ID="lblPhone" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_Phone %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server" Width="300px" />
                    <asp:Label ID="lPhoneError" runat="server" Text="*" Visible="false" Font-Bold="true"
                        ForeColor="Red" />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td class="formheader_order" colspan="2">
                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="12pt" Text="<%$ Resources: Resource, Admin_OrderByRequest_ProductName %>"></asp:Label>
                                     
                </td>
            </tr>
            <tr class="style2">
                <td class="style1">
                    <asp:Label ID="lblArtNo" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_ArtNo %>" />
                </td>
                <td>
                    <asp:Label ID="lArtNo" runat="server" />
                </td>
            </tr>
            <tr class="style2">
                <td class="style1">
                    <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_ProductName %>" />
                </td>
                <td>
                    <asp:Label ID="lProductName" runat="server" /> <asp:Label ID="lblNewProductName" runat="server"></asp:Label>   
                </td>
            </tr>
            <tr class="style2">
                <td class="style1">
                    <asp:Label ID="lblQuantity" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_Quantity %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtQuantity" runat="server" Width="300px" />
                    <asp:Label ID="lQuantityError" runat="server" Text="*" Visible="false" Font-Bold="true"
                        ForeColor="Red" />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td class="formheader_order" colspan="2">
                    <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="12pt" Text="<%$ Resources: Resource, Admin_OrderByRequest_Different %>"></asp:Label>
                </td>
            </tr>
            <tr class="style2">
                <td class="style1">
                    <asp:Label ID="lblComment" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_Comment %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Width="600px" Height="100px" />
                </td>
            </tr>
        </table>
        <div class="style3">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, Admin_OrderByRequest_SendConfirmationNote %>" />
        </div>
        <div class="style3">
            <asp:Label runat="server" Text="<%$ Resources: Resource, Admin_OrderByRequest_AddComment %>"
                Font-Bold="true" />
        </div>
        <div class="style3">
            <asp:TextBox ID="txtLetterComment" runat="server" TextMode="MultiLine" Width="600px"
                Height="100px" />
        </div>
        <div class="style3">
            <div style="width: 260px; float: left;">
                <asp:Button CssClass="btn btn-middle btn-add" CausesValidation="false" ID="btnSendLink" runat="server"
                    Text='<%$ Resources:Resource, Admin_OrderByRequest_SendLink %>' OnClick="btnSendLink_Click" />
            </div>
            <div style="float: left;">
                &nbsp;<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, Admin_OrderByRequest_And %>" />
                <asp:CheckBox ID="chkCloseAfterConfirmation" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_CloseOrder %>"
                    TextAlign="Right" Checked="false" />
            </div>
            <div style="clear: both;">
            </div>
        </div>
        <div class="style3">
            <asp:Label ID="Label4" runat="server" Text="<%$ Resources: Resource, Admin_OrderByRequest_SendFailureNote %>" />
        </div>
        <div class="style3">
            <div style="width: 415px; float: left;">
                <asp:Button CssClass="btn btn-middle btn-add" CausesValidation="false" ID="btnSentFailure" runat="server"
                    Text='<%$ Resources:Resource, Admin_OrderByRequest_SendFailure %>' onclick="btnSentFailure_Click" />
            </div>
            <div style="float: left;">
                &nbsp;<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: Resource, Admin_OrderByRequest_And %>" />
                <asp:CheckBox ID="chkCloseAfterFailure" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_CloseOrder %>"
                    TextAlign="Right" Checked="true" />
            </div>
            <div style="clear: both;">
            </div>
        </div>
    </div>
</asp:Content>