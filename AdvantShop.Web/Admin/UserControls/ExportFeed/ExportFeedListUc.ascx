<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.ExportFeedUc.ExportFeedListUc" CodeBehind="ExportFeedListUc.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.ExportImport" %>

<script>
    $(function () {
        $.advModal({
            title: localize("Admin_ExportFeed_AddFeedTitle"),
            control: $(".newExportFeed"),
            htmlContent: $("#modalAddExportFeed"),
            beforeOpen: clearModal,
            afterOpen: function () {
                $("#<%= txtExportFeedName.ClientID %>").focus();
            },
            clickOut: false
        });
    });
    function clearModal() {
        $("#<%= txtExportFeedName.ClientID %>").val("");
        $("#<%= txtExportFeedDescription.ClientID %>").val("");
    }
</script>
<div class="panel-toggle">
    <h2 class="justify-item products-header">
        <asp:Literal ID="lblLeftHead" runat="server" Text='<%$ Resources:Resource, Admin_ExportFeed_PageHeader %>' />
    </h2>

    <div id="divModal" style="display: none;">
        <div id="modalAddExportFeed">

            <asp:Panel Style="background-color: white; padding-top: 10px; padding-bottom: 10px; text-align: center;" ID="pnlInnerModal" runat="server" DefaultButton="btnAddFeed">
                <table width="430px;">
                    <tr>
                        <td style="width: 130px; vertical-align: top;">
                            <asp:Label ID="Label1" runat="server" Text="<%$  Resources:Resource, Admin_ShippingMethods_Name %>"></asp:Label><span class="required">*</span>
                        </td>
                        <td style="padding: 3px 0">
                            <asp:TextBox ID="txtExportFeedName" runat="server" ValidationGroup="modalAddFeed" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtExportFeedName" EnableClientScript="true" Style="display: inline;"
                                ErrorMessage='Введите название фида' ValidationGroup="modalAddFeed"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethods_Type %>"></asp:Label><span
                                class="required">*</span>
                        </td>
                        <td style="padding: 3px 0">
                            <asp:DropDownList ID="ddlExportFeedType" runat="server" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Description %>"></asp:Label>
                        </td>
                        <td style="padding: 3px 0">
                            <asp:TextBox runat="server" ID="txtExportFeedDescription" TextMode="MultiLine" Width="298" Height="100" ValidationGroup="modalAddFeed" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right; padding: 3px 0;">
                            <asp:LinkButton runat="server" ID="btnAddFeed" Text="<%$ Resources: Resource, Admin_ShippingMethod_Create %>" CssClass="btn btn-middle btn-action" OnClick="btnAddFeed_Click" ValidationGroup="modalAddFeed" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>

    <asp:ListView ID="lvExportFeeds" runat="server" ItemPlaceholderID="itemPlaceHolderId">
        <LayoutTemplate>
            <ul class="orders-list" style="width: 205px;">
                <li runat="server" id="itemPlaceHolderId"></li>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="orders-list-row" onclick="<%# "window.location='exportfeed.aspx?feedid=" + Eval("Id") + TypeParametr() + "'"  %>"
                style='<%# RenderExportFeedBorderColor(Convert.ToString(Eval("Type"))) + ( Request["feedid"] == Eval("Id").ToString() ? "background-color:rgb(239, 240, 241);": string.Empty) %>'>
                <a href="<%# "exportfeed.aspx?feedid=" + Eval("Id") + TypeParametr() %>"><%#Eval("Name") %></a>
                <br>
                <%# ((EExportFeedType)Eval("Type")).Localize() %>
                <br>
                <%# GetJobActiveImage(Convert.ToInt32(Eval("Id")))%>
                <%# Eval("LastExport") ?? Resources.Resource.Admin_ExportFeed_NotExports %>
            </li>
        </ItemTemplate>
    </asp:ListView>
    <div style="margin: 10px 0">
        <a class="btn btn-middle btn-action newExportFeed" id="btnAddExportFeed" href="javascript:void(0)" style="padding: 8px 20px 8px 20px;"><%= Resources.Resource. Admin_ExportFeed_NewFile %></a>
    </div>
</div>
