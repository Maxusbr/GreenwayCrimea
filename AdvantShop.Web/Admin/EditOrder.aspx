<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeBehind="EditOrder.aspx.cs" Inherits="Admin.EditOrder" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>
<%--<%@ Register Src="UserControls/Order/OrdersSearch.ascx" TagName="OrdersSearch" TagPrefix="adv" %>--%>
<%@ Register Src="~/Admin/UserControls/Order/OrderItems.ascx" TagName="OrderItems"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Order/OrderCertificates.ascx" TagName="OrderCertificates"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Order/ShippingRates.ascx" TagName="ShippingRates"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridCustomers.ascx" TagName="PopupGridCustomers"
    TagPrefix="adv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            if ($.cookie("isVisiblePanel") != "false") {
                //showPanel();
            }
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
        });
        var timeOut;
        function Darken() {
            timeOut = setTimeout('document.getElementById("inprogress").style.display = "block";', 1000);
        }

        function Clear() {
            clearTimeout(timeOut);
            document.getElementById("inprogress").style.display = "none";

            $("input.sel").each(function (i) {
                if (this.checked) $(this).parent().parent().addClass("selectedrow");
            });
        }
        function removeunloadhandler(a) {

            window.onbeforeunload = null;

        }

        var skip = false;
        var dirty = false;
        function addbeforeunloadhandler() {
            window.onbeforeunload = beforeunload;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#findOrder").qtip({
                delay: 10,
                showURL: false,
                content: "<%= Resource.Admin_OrderSearch_Qtip %>",
                position: {
                    corner: {
                        target: 'bottomMiddle',
                        tooltip: 'topRight'
                    }
                }

            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(endRequest);

        });

        function endRequest() {
            window.onbeforeunload = beforeunload;
        }

        function beforeunload(e) {
            if (!skip) {
            } else {
                skip = false;
            }
        }

        //        function showPanel() {
        //            $("#rightPanel").show();
        //            document.getElementById("divHide").style.display = "block";
        //            document.getElementById("divShow").style.display = "none";
        //        }

        function togglePanel() {
            if ($.cookie("isVisiblePanel") == "true" || $.cookie("isVisiblePanel") == "") {
                $("#rightPanel").hide("fast");
                $("div:.hide_rus").hide("fast");
                $("div:.show_rus").show("fast");
                $("div:.hide_en").hide("fast");
                $("div:.show_en").show("fast");
                $.cookie("isVisiblePanel", "false", { expires: 7 });
            } else {
                $("#rightPanel").show("fast");
                $("div:.show_rus").hide("fast");
                $("div:.hide_rus").show("fast");
                $("div:.show_en").hide("fast");
                $("div:.hide_en").show("fast");
                $.cookie("isVisiblePanel", "true", { expires: 7 });
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            SearchProduct();
        });
    </script>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="OrderSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
            <li class="neighbor-menu-item"><a href="OrderStatuses.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderStatuses%></a></li>
            <li class="neighbor-menu-item"><a href="OrderSources.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderSources %></a></li>
            <li class="neighbor-menu-item"><a href="OrderByRequest.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderByRequest%></a></li>
            <li class="neighbor-menu-item"><a href="StatisticsOrdersExportCsv.aspx">
                <%= Resource.Admin_Statistics_ExportOrdersHeader%></a></li>
        </menu>
        <div class="panel-add">
            <a href="EditOrder.aspx?OrderID=addnew" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Add %>
                <%= Resource.Admin_MasterPageAdmin_Order %></a>
        </div>
    </div>
    <div>
        <table style="width: 100%; height: 359px;" cellspacing="0">
            <tr>
                <td style="width: 99%; vertical-align: top; padding: 0px 10px;">
                    <div id="inprogress" style="display: none;">
                        <div id="curtain" class="opacitybackground">
                            &nbsp;
                        </div>
                        <div class="loader">
                            <table width="100%" style="font-weight: bold; text-align: center;">
                                <tbody>
                                    <tr>
                                        <td align="center">
                                            <img src="images/ajax-loader.gif" alt="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="color: #0D76B8;">
                                            <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>">
                                            </asp:Localize>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="pnOrder">
                        <center>
                        <asp:Panel ID="pnlMsgErr" runat="server" Visible="false">
                            <asp:Label ID="MsgErr" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                            <br />
                        </asp:Panel>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tbody>
                                <tr>
                                    <td rowspan="3" style="width: 72px;">
                                        <img src="images/orders_ico.gif" alt="" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOrderID" CssClass="AdminHead" runat="server" />
                                        <asp:HiddenField ID="hiddenIdOrder" runat="server" />
                                        <br />
                                    </td>
                                    <td align="right" runat="server" id="cellPrint1" style="width: 20px">
                                        <a href="javascript:open_printable_version('../printorder/<%= OrderCode %>?showmap=true');">
                                            <img id="printerImg" src="images/printer.png" alt="<%=Resource.Admin_ViewOrder_PrintOrder%>"
                                                style="border: none;" />
                                        </a>
                                    </td>
                                    <td runat="server" id="cellPrint2" style="width: 100px">
                                        <a href="javascript:open_printable_version('../printorder/<%= OrderCode %>?showmap=true');">
                                        <%=Resource.Admin_ViewOrder_PrintOrder%></a>
                                    </td>
                                    <td align="right" runat="server" id="cellPrint3" style="width: 20px">
                                        <asp:HyperLink runat="server" ID="hlExport" ImageUrl="images/xls.jpg" ToolTip="<%$ Resources: Resource, Admin_ViewOrder_ExportToExcel %>" />
                                    </td>
                                    <td runat="server" id="cellPrint4" style="width: 150px">
                                        <asp:HyperLink CssClass="Link" runat="server" ID="hlExport2" Text="<%$ Resources: Resource, Admin_ViewOrder_ExportToExcel %>">
                                        </asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOrderStatus" CssClass="AdminSubHead" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label Font-Bold="true" ID="Localize_Admin_ViewOrder_IP" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_IP %>">
                                        </asp:Label>
                                        <asp:Label ID="lCustomerIP" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <div class="dp" style="width:250px;display:inline-block">
                                        <b>
                                            <asp:Label ID="Label35" runat="server" Text='<%$ Resources: Resource,Admin_ViewOrder_Date%>'/>
                                        </b>
                                        <asp:TextBox ID="lOrderDate" runat="server" Width="130" Style="margin-top: 0px">
                                          </asp:TextBox>
                                        <asp:Image ID="popupDateBuy" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png"
                                           Style="height: 16px" CssClass="icon-calendar" />
                                        </div>
                                        <div style="width:200px;display:inline-block">
                                             <b>
                                                <asp:Label ID="Label43" runat="server" Text='<%$ Resources: Resource,Admin_ViewOrder_Time%>'/>
                                            </b>
                                             <asp:TextBox ID="txtOrderTime" runat="server" Width="50" Style="margin-top: 0px" />
                                        </div>
                                        <div id="divUseIn1c" runat="server" style="width:200px;display:inline-block">
                                            <b>
                                                <asp:Label runat="server" AssociatedControlID="chkUseIn1C" Text='<%$ Resources: Resource, Admin_ViewOrder_UseIn1C%>'/>
                                            </b>
                                             <asp:CheckBox ID="chkUseIn1C" runat="server" />
                                        </div>
                                        <div id="divManagerConfirm" runat="server" style="width:250px;display:inline-block">
                                            <b>
                                                <asp:Label runat="server" AssociatedControlID="chkUseIn1C" Text='Подтвердить заказ, разрешить оплату'/>
                                            </b>
                                             <asp:CheckBox ID="ckbManagerConfirm" runat="server" />
                                        </div>
                                    </td>
                                    <td colspan="4" align="right">
                                        <div style="width: 390px">
                                            <a <%= "href=\"vieworder.aspx?orderid=" + OrderID + "\"" %> class="btn btn-middle btn-action"><%= Resource.Admin_BackToBrowse %></a>
                                            <asp:Button CssClass="btn btn-middle btn-add" CausesValidation="false" UseSubmitBehavior="False"  ID="btnSave" runat="server" Text='<%$ Resources:Resource,Admin_OrderSearch_Save %>'
                                                OnClick="btnSave_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </center>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="PopupGridCustomers" />
                                <asp:AsyncPostBackTrigger ControlID="orderItems" EventName="ItemsUpdated" />
                                <asp:AsyncPostBackTrigger ControlID="btnShippingOK" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlShippingCountry" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlBillingCountry" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="printPaymentDetails" Visible="false">
                                    <asp:Panel runat="server" ID="paymentDetails" Visible="false">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                            <tbody>
                                                <tr>
                                                    <td class="formheader_order" colspan="2">
                                                        <asp:Label runat="server" Font-Bold="true" Font-Size="12pt" Text="<%$ Resources: Resource, Admin_ViewOrder_PrintPaymentDetails %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #F0F0F0;">
                                                    <td style="width: 150px;">
                                                        <asp:Localize ID="LocalizeClient_OrderConfirmation_OrganizationName" runat="server"
                                                            Text="<%$ Resources:Resource, Client_OrderConfirmation_OrganizationName %>"></asp:Localize>:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCompanyName" />
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #F0F0F0;">
                                                    <td style="width: 150px;">
                                                        <asp:Localize ID="LocalizeClient_OrderConfirmation_INN" runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_INN %>"></asp:Localize>:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtINN" />
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #F0F0F0;">
                                                    <td colspan="2">
                                                        <input type="button" runat="server" id="btnPrintPaymentDetails" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="qiwiPanel" Visible="false">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                            <tbody>
                                                <tr>
                                                    <td class="formheader_order" colspan="2">
                                                        <asp:Label ID="Label44" runat="server" Font-Bold="true" Font-Size="12pt" Text="Оплата через QIWI"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #F0F0F0;">
                                                    <td style="width: 150px;">
                                                        <asp:Localize ID="Localize1" runat="server"
                                                            Text="<%$ Resources:Resource, Client_OrderConfirmation_Phone %>"></asp:Localize>:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPhoneQiwi" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>

                                </asp:Panel>
                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td class="formheader_order">
                                            <span style="font-size: 12pt; font-weight: bold">
                                                <%= Resource.Admin_OrderSearch_User %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #F0F0F0; padding: 5px;">
                                            <table class="info-tb">
                                                <tr>
                                                    <td style="width: 130px">
                                                        <asp:Label runat="server" ID="lblSelectCustomer" Text="<%$ Resources: Resource, Admin_OrderSearch_SelectCustomer %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hfContactID" runat="server" />
                                                        <asp:HyperLink runat="server" Font-Bold="true" ID="hlCustomer" CssClass="blueLink"
                                                            NavigateUrl="CustomerSearch.aspx" Visible="false"></asp:HyperLink>
                                                        <asp:Label runat="server" Font-Bold="true" ID="lblCustomer" Visible="false"></asp:Label>
                                                        <asp:Label runat="server" Font-Bold="true" ID="lblChosingCustomer" Text="<%$ Resources:Resource,Admin_OrderSearch_UserNotChosen %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel runat="server" ID="pnCustomerSelect" Style="margin-left: 10px">
                                                            <a class="editlink" style="color: #017DC1 !important" href="javascript:void(0)">
                                                                <%= Resource.Admin_OrderSearch_ChooseUser%>...</a>
                                                            <%-- <span>
                                                                    <%= Resource.Admin_OrderSearch_Or%></span> <a class="editcreate" style="color: #017DC1 !important"
                                                                        href="javascript:void(0)">
                                                                        <%= Resource.Admin_OrderSearch_CreateUser%></a>--%>
                                                        </asp:Panel>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td><span><%= Resource.Admin_CustomerGroup_Header%>&nbsp;</span></td>
                                                    <td colspan="3">
                                                        <asp:Label runat="server" ID="lblGroupDiscount" Text="0" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><span>Тип заказа&nbsp;</span></td>
                                                    <td colspan="3">
                                                        <asp:DropDownList runat="server" ID="ddlOrdertype" DataTextField="Name" DataValueField="Id" Width="250px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Font-Bold="true" ID="Label25" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_OrderLastName %>"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtOrderLastName" runat="server" Width="250px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Font-Bold="true" ID="Label32" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_OrderFirstName %>"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtOrderFirstName" runat="server" Width="250px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Font-Bold="true" ID="Label33" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_OrderEmail %>"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtOrderEmail" runat="server" Width="250px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Font-Bold="true" ID="Label34" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_OrderMobilePhone %>"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtOrderMobilePhone" runat="server" CssClass="phoneSource" Width="250px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Font-Bold="true" ID="Label45" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_OrderStandardPhone %>"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtStandardPhone" runat="server" CssClass="phoneDest" Width="250px" />
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <center>
                                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="formheader_order" colspan="2">
                                            <span style="font-size: 12pt; font-weight: bold">
                                                <%= Resource.Admin_OrderSearch_OrderInfo%></span>
                                        </td>
                                    </tr>
                                    <tr style="background-color: #F0F0F0;">
                                        <td style="width: 50%; vertical-align: top; padding: 5px;">
                                            <table class="info-tb">
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Font-Bold="true" ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Shipping %>"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <a href="javascript:ShowModalChooseAddress('shipping');" style="color: #017DC1 !important"
                                                            runat="server" id="lnkChooseAddress">
                                                            <%= Resource.Admin_OrderSearch_ChooseUser%>...</a>
                                                        <asp:HiddenField ID="hfShippingID" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Name %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingName" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Country %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlShippingCountry" runat="server" DataSourceID="sdsCountry"
                                                            DataTextField="CountryName" Style="width: 320px" DataValueField="CountryID" CausesValidation="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Zone %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingZone" Width="100%" runat="server" CssClass="autocompleteRegion"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_City %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingCity" Width="100%" runat="server" CssClass="ms-city autocompleteCity"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Zip %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingZip" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Address %>"></asp:Label>
                                                        <asp:HyperLink ID="lnkMap" runat="server" Target="_blank" ><img src="images/map.png" alt="map" style="border:none;"/></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingAddress" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%= SettingsCheckout.CustomShippingField1  %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingCustomField1" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%= SettingsCheckout.CustomShippingField2  %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingCustomField2" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%= SettingsCheckout.CustomShippingField3  %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShippingCustomField3" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; padding: 5px;">
                                            <table class="info-tb">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox runat="server" ID="chkCopyAddress" Text="<%$ Resources: Resource ,Admin_ViewOrder_CopyAddress %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Font-Bold="true" ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Billing %>"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <a href="javascript:ShowModalChooseAddress('billing');" style="color: #017DC1 !important"
                                                            runat="server" id="lnkChooseBilling">
                                                            <%= Resource.Admin_OrderSearch_ChooseUser%>...</a>
                                                        <asp:HiddenField ID="hfBillingID" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Name %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingName" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Country %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBillingCountry" runat="server" DataSourceID="sdsCountry"
                                                            DataTextField="CountryName" Style="width: 320px" DataValueField="CountryID" CausesValidation="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Zone %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingZone" Width="100%" runat="server" CssClass="autocompleteRegion"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_City %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingCity" Width="100%" runat="server" CssClass="autocompleteCity"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Zip %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingZip" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources: Resource, Admin_ViewOrder_Address %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingAddress" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td>
                                                        <%= SettingsCheckout.CustomShippingField1  %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingCustomField1" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%= SettingsCheckout.CustomShippingField2  %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingCustomField2" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%= SettingsCheckout.CustomShippingField3  %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingCustomField3" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formheader_order" colspan="2">
                                            <span style="font-size: 12pt; font-weight: bold">
                                                <% = Resource.Admin_EditOrder_ShippingPayment%></span>
                                        </td>
                                    </tr>
                                    <tr style="background-color: #F0F0F0;">
                                        <td style="width: 50%; vertical-align: top; padding: 5px;">
                                            <table class="info-tb">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lbChangeShipping" CssClass="blueLink" OnClick="lbChangeShipping_Click"
                                                            Text="<%$ Resources:Resource, Admin_ViewOrder_ChangeShippingMethod %>" CausesValidation="false"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ShippingType %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:TextBox ID="txtShippingMethod" runat="server" Text="" Width="333px"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfOrderShippingId" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ShippingPrice %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:TextBox ID="txtShippingPrice" runat="server" AutoPostBack="true" Text="0"></asp:TextBox>
                                                        <asp:Label ID="lblCurrencySymbol" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Literal ID="ltPickPointID" runat="server" />
                                                        <asp:Literal ID="ltPickPointAddress" runat="server" />
                                                        <asp:HiddenField runat="server" ID="hfPickpointAdditional"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; padding: 5px;">
                                            <table class="info-tb">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_PaymentType %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="100%" ID="txtPaymentMethod" Visible="false"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlPaymentMethod" runat="server" DataTextField="Name" DataValueField="PaymentMethodID"
                                                            Visible="true">
                                                        </asp:DropDownList>
                                                    </td>

                                                </tr>
                                                                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_PaymentExtracharge %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:TextBox ID="txtPaymentPrice" runat="server" Text="0"></asp:TextBox>
                                                        <asp:Label ID="Label14" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                                <%--                                <script type="text/javascript">
                                    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                                        if ($("input.autocompleteRegion").length) {
                                            $("input.autocompleteRegion").autocomplete("../HttpHandlers/GetRegions.ashx", {
                                                delay: 300,
                                                minChars: 1,
                                                matchSubset: 1,
                                                autoFill: true,
                                                matchContains: 1,
                                                cacheLength: 10,
                                                selectFirst: false,
                                                //formatItem: liFormat,
                                                maxItemsToShow: 10
                                            });
                                        }

                                        if ($("input.autocompleteCity").length) {
                                            $("input.autocompleteCity").autocomplete('../HttpHandlers/GetCities.ashx', {
                                                delay: 300,
                                                minChars: 1,
                                                matchSubset: 1,
                                                autoFill: true,
                                                matchContains: 1,
                                                cacheLength: 10,
                                                selectFirst: false,
                                                //formatItem: liFormat,
                                                maxItemsToShow: 10
                                            });
                                        }
                                    });
                                </script>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <table cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td class="formheader_order">
                                    <span style="font-size: 12pt; font-weight: bold">
                                        <% =Resource.Admin_ViewOrder_OrderItem %></span>
                                    <asp:Label ID="lOrderContent" runat="server" Text="<%$ Resources:Resource, Admin_EditOrder_OrderCantBeEdited %>"
                                        ForeColor="Red" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel runat="server" ID="pnlOderContent">
                                        <asp:UpdatePanel ID="upItems" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="orderItems" EventName="ItemsUpdated" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <adv:OrderItems runat="server" ID="orderItems" OnItemsUpdated="orderItems_Updated"></adv:OrderItems>
                                                <adv:OrderCertificates runat="server" ID="orderCertificates"></adv:OrderCertificates>
                                                <asp:Panel ID="pnlSummary" runat="server">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemCost2 %>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblTotalOrderPrice" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trDiscount">
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemDiscount %>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblOrderDiscount" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trBonuses" visible="False">
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Bonuses %>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblOrderBonuses" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trCertificatePrice" visible="false">
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Certificate %>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblCertificatePrice" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trCoupon" visible="false">
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Coupon%>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblCoupon" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ShippingPrice %>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblShippingPrice" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <asp:Literal ID="literalTaxCost" runat="server"></asp:Literal>
                                                        <tr>
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_PaymentPrice %>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblPaymentPrice" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <b>
                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_TotalPrice %>"></asp:Label>:&nbsp;</b>
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <b>
                                                                    <asp:Label ID="lblTotalPrice" runat="server"></asp:Label></b>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trBonusesPlus" visible="False">
                                                            <td style="background-color: #FFFFFF; text-align: right">
                                                                <asp:Label ID="Label46" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_BonusCardAmount %>"></asp:Label>:&nbsp;
                                                            </td>
                                                            <td style="background-color: #FFFFFF; width: 150px">
                                                                <asp:Label ID="lblOrderBonusesPlus" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:HiddenField ID="hforderShipName" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="notBonusCard" runat="server" visible="True">
                                        <b>Бонусной карты нет</b>
                                        <br />
                                        <br />
                                    </div>
                                    <div id="bonusPurchaise" runat="server" visible="False">
                                        <asp:CheckBox runat="server" ID="chkMakePurchaise" Text="<%$ Resources:Resource, Admin_EditOrder_MakePurchase %>"
                                            AutoPostBack="True" OnCheckedChanged="chkUseBonuses_CheckedChanged" />
                                    </div>
                                    <div id="useBonuses" runat="server" visible="False">
                                        <asp:CheckBox runat="server" ID="chkUseBonuses" Text="<%$ Resources:Resource, Admin_EditOrder_UseBonuses %>"
                                            AutoPostBack="True" OnCheckedChanged="chkUseBonuses_CheckedChanged" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formheader_order">
                                    <span style="font-size: 12pt; font-weight: bold">
                                        <% =Resource.Admin_ViewOrder_CommentsAndStatus %></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" style="background-color: #F0F0F0;">
                                        <tr>
                                            <td style="padding: 5px;">
                                                <%--<div>
                                                    <b>
                                                        <%=Resource.Admin_ViewOrder_Number%></b>
                                                    <asp:Label ID="lNumber" runat="server" />
                                                </div>--%>
                                                <div id="bonusCardBlock" runat="server" visible="False" style="margin: 15px 0">
                                                    <b>
                                                        <%= Resource.Admin_ViewOrder_BonusCard %>:</b>
                                                    <div style="margin: 5px 0 0 10px">
                                                        <%= Resource.Admin_ViewOrder_BonusCardNumber%>:
                                                        <asp:Label ID="lblBonusCardNumber" runat="server" />
                                                    </div>
                                                    <div style="margin: 5px 0 0 10px">
                                                        <%= Resource.Admin_ViewOrder_BonusCardAmount %>:
                                                        <asp:Label ID="lblBonusCardAmount" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div id="pnlCertificateCode" runat="server" visible="False" style="margin: 15px 0">
                                                    <b>
                                                        <%=Resource.Admin_ViewOrder_CertificateCode%>:</b>
                                                    <asp:Label ID="lCertificateCode" runat="server" />
                                                </div>
                                                <div style="margin: 15px 0">
                                                    <b>
                                                        <%=Resource.Admin_ViewOrder_UserComment%></b>
                                                    <div style="margin: 5px 0 0 10px">
                                                        <asp:Label ID="lblUserComment" runat="server" Text="<%$ Resources: Resource, Admin_OrderSearch_NoComment %>" />
                                                    </div>
                                                </div>

                                                <div style="margin: 15px 0">
                                                    <b>Доп. информация</b>
                                                    <div style="margin: 5px 0 0 10px">
                                                        <asp:Label ID="lblCustomData" runat="server" Text="" />
                                                    </div>
                                                </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="width: 50%; padding: 5px">
                                                <b>
                                                    <asp:Label ID="lblStatusComment" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_StatusComment %>">
                                                    </asp:Label></b>
                                                <br />
                                                <div class="textarea-wrap" style="height: 150px">
                                                    <asp:TextBox ID="txtStatusComment" runat="server" TextMode="MultiLine" />
                                                </div>
                                                <br />
                                                <br />
                                                <asp:CheckBox ID="chkSendConfirmation" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_SendConfirmation %>" />
                                                <br />
                                            </td>
                                            <td valign="top" style="width: 50%; padding: 5px;">
                                                <b>
                                                    <asp:Label ID="lblAdminOrderComment" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_AdminOrderComment %>">
                                                    </asp:Label></b>
                                                <br />
                                                <div class="textarea-wrap" style="height: 150px">
                                                    <asp:TextBox ID="txtAdminOrderComment" Width="100%" runat="server" TextMode="MultiLine" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="float: right; margin: 10px;">
                                        <asp:Button CssClass="btn btn-middle btn-add" ID="btnSaveBottom" CausesValidation="false" UseSubmitBehavior="False" runat="server"
                                            Text='<%$ Resources:Resource, Admin_OrderSearch_Save %>' OnClick="btnSave_Click" />
                                    </div>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT OrderStatusID, StatusName FROM [Order].OrderStatus"
                                        OnInit="SqlDataSource1_Init"></asp:SqlDataSource>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnEmpty" runat="server" BackColor="#c0c0c0" Height="45" Visible="false">
                        <asp:Label ID="lblNotFound" runat="server" Text='<%$ Resources: Resource, Admin_OrderSearch_Empty %>'>
                        </asp:Label>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="sdsCountry" OnInit="sds_Init" runat="server" SelectCommand="SELECT CountryID,CountryName FROM [Customers].[Country] ORDER BY CountryName"></asp:SqlDataSource>
        <center>
        <asp:Label ID="lblInfo" runat="server" Text="" ForeColor="Blue"></asp:Label></center>
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Blue"></asp:Label>
        <asp:HiddenField ID="hfTypeBindAddress" runat="server" />

        <%-- popup for shipping method --%>
        <asp:LinkButton ID="lbShipping" runat="server" Style="display: none;" />
        <ajaxToolkit:ModalPopupExtender ID="modalShipping" runat="server" TargetControlID="lbShipping"
            PopupControlID="modalShippingMethod" BackgroundCssClass="blackopacitybackground"
            BehaviorID="ModalShippingBehaviour" CancelControlID="btnShippingCancel">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="modalShippingMethod" CssClass="modal-admin" runat="server">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:PostBackTrigger ControlID="lbChangeShipping" />
                    <asp:AsyncPostBackTrigger ControlID="btnShippingOk" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <div class="title">
                        <%= Resource.Admin_MasterPageAdmin_ShippingMethod%>
                    </div>
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    <br />
                    <div style="height: 350px; width: 500px; overflow: auto;">
                        <adv:ShippingRates ID="ShippingRates" runat="server" />
                    </div>
                    <br />
                    <asp:Button ID="btnShippingOk" CausesValidation="false" runat="server" Text="<%$ Resources:Resource,Admin_OK %>"
                        OnClick="btnSelectShipping_Click" CssClass="btnShippinOK" />
                    &nbsp;
                    <asp:Button ID="btnShippingCancel" runat="server" OnClientClick="HideShippingModal();"
                        Text="<%$ Resources:Resource,Admin_Cancel%>" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <%-- popup for recheck shipping data --%>
        <asp:LinkButton ID="lbtnRecheckShipping" runat="server" Style="display: none;" />
        <ajaxToolkit:ModalPopupExtender ID="modalRecheckShipping" runat="server" TargetControlID="lbtnRecheckShipping"
            PopupControlID="modalPanelRecheckShipping" BackgroundCssClass="blackopacitybackground"
            BehaviorID="ModalRecheckShippingBehaviour">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="modalPanelRecheckShipping" CssClass="modal-admin" runat="server">
            <div style="width: 90%; white-space: normal; font-weight: normal;">
                <asp:Literal ID="lChangeShipping" Text="<%$ Resources: Resource, Admin_EditOrder_RefreshShipping %>"
                    runat="server" />
            </div>
            <br />
            <asp:Button runat="server" ID="btnChangeShipping" OnClick="lbChangeShipping_Click"
                OnClientClick="HideRecheckShippingModal();" Text="<%$ Resources:Resource, Admin_ViewOrder_ChangeShippingMethod %>"
                CausesValidation="false"></asp:Button>
            &nbsp;
            <asp:Button runat="server" ID="btnHideChange" OnClientClick="HideRecheckShippingModal();"
                Text="<%$ Resources:Resource,Admin_Cancel%>" CausesValidation="false"></asp:Button>
        </asp:Panel>
        <%-- popup for choose address --%>
        <asp:LinkButton ID="lbtnChooseAddress" runat="server" Style="display: none;" />
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="lbtnChooseAddress"
            PopupControlID="modalChooseAddress" BackgroundCssClass="blackopacitybackground"
            BehaviorID="ModalAddressBehaviour">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="modalChooseAddress" CssClass="modal-admin" runat="server">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSelectAddress" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <span style="font-size: 12pt">
                        <%= Resource.Admin_OrderSearch_ChooseAddress%></span><br />
                    <asp:Label ID="ErrMes" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label><br />
                    <div style="height: 350px; width: 500px; overflow: scroll;">
                        <asp:RadioButtonList ID="CustomerContacts" runat="server" Width="100%">
                        </asp:RadioButtonList>
                    </div>
                    <br />
                    <asp:Button ID="btnSelectAddress" CausesValidation="false" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_ChooseAddressBtn%>"
                        OnClientClick="HideModalChooseAddress();" OnClick="btnSelectAddress_Click" />
                    &nbsp;
                    <asp:Button ID="btnHideAddress" OnClientClick="HideModalChooseAddress();" runat="server"
                        Text="<%$ Resources:Resource,Admin_OrderSearch_Cancel%>" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <%-- popup for create user --%>
        <asp:LinkButton ID="lbtnCreatUser" runat="server" Style="display: none;" />
        <ajaxToolkit:ModalPopupExtender ID="modalPopupCreateUser" runat="server" TargetControlID="lbtnCreatUser"
            PopupControlID="modalCreateUser" BackgroundCssClass="blackopacitybackground"
            BehaviorID="ModalUserBehaviour">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="modalCreateUser" CssClass="modal-admin" runat="server">
            <asp:UpdatePanel ID="upCreateUser" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <span style="font-size: 12pt">
                        <%= Resource.Admin_OrderSearch_CreateUser%></span>
                    <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    <br />
                    <ul runat="server" id="ulUserRegistarionValidation">
                    </ul>
                    <table border="0" width="100%" cellspacing="0" cellpadding="2" style="margin-top: 5px">
                        <tr>
                            <td class="td_property_alt" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Email %>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2_alt">
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="<%$ Resources:Resource, Admin_ViewCustomer_NotValidEmail %>" ValidationExpression="\w+([-+.'])*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_EnterPassword%>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2" style="height: 25px">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property_alt" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_ConfirmPassword%>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2_alt" style="height: 25px">
                                <asp:TextBox ID="txtPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPasswordConfirm"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_LastName %>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2" style="height: 25px">
                                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property_alt" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Name %>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2_alt" style="vertical-align: top;">
                                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_WWW %>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2">
                                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Subscribed4News %>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2">
                                <asp:CheckBox ID="chkSubscribed4News" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property_alt" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerGroup %>" />
                                </b>
                            </td>
                            <td class="td_property2_alt">
                                <asp:DropDownList ID="ddlCustomerGroup" runat="server" Style="width: 200px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_property" style="width: 50%;">
                                <b>
                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerRole %>"></asp:Label>
                                </b>
                                <br />
                            </td>
                            <td class="td_property2">
                                <asp:DropDownList ID="ddlCustomerRole" runat="server" Style="width: 155px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center; padding-top: 5px">
                                <asp:Button ID="btnCreateUser" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_CreateUser%>"
                                    OnClick="btnCreateUser_Click" />
                                &nbsp;
                                <asp:Button ID="btnHideCreate" OnClientClick="HideModalUserPopup();" runat="server"
                                    Text="<%$ Resources:Resource,Admin_OrderSearch_Cancel%>" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--popup for choose customer--%>
        <asp:LinkButton ID="lbPopup" runat="server" Style="display: none;" />
        <adv:PopupGridCustomers ID="PopupGridCustomers" runat="server" />


        <script type="text/javascript">

            function esc_press(D) {
                D = D || window.event;
                var A = D.keyCode;
                if (A == 27) {
                    HideModalPopupCustomers();
                    HideModalUserPopup();
                    HideModalChooseAddress();
                }
            }

            document.onkeydown = esc_press;

            function HideModalUserPopup() {
                $find("ModalUserBehaviour").hide();
                $('select', 'object', 'embed').each(function () {
                    $(this).show(); /*.style.visibility = 'visible'*/
                });
            }

            $(document).ready(function () {
                $(".editlink").live("click", function () {
                    ShowModalPopupCustomers();
                });
            });

            $(document).ready(function () {
                $(".editcreate").live("click", function () {
                    document.body.style.overflowX = 'hidden';
                    $find('ModalUserBehaviour').show();
                    document.getElementById('ModalUserBehaviour_backgroundElement').onclick = HideModalUserPopup;
                });
            });

            $(document).ready(function () {
                $("#ModalShippingBehaviour_backgroundElement").live("click", HideShippingModal);
                $("#<%= chkCopyAddress.ClientID %>").live("click", function () {
                    if ($('#<%= chkCopyAddress.ClientID %>').attr('checked')) {
                        $("#<% = txtBillingName.ClientID %>").val($("#<% = txtShippingName.ClientID %>").val()).attr('disabled', "disabled");
                        $("#<% = txtBillingCity.ClientID %>").val($("#<% = txtShippingCity.ClientID %>").val()).attr('disabled', "disabled");
                        $("#<% = txtBillingZip.ClientID %>").val($("#<% = txtShippingZip.ClientID %>").val()).attr('disabled', "disabled");
                        $("#<% = txtBillingAddress.ClientID %>").val($("#<% = txtShippingAddress.ClientID %>").val()).attr('disabled', "disabled");
                        $("#<% = ddlBillingCountry.ClientID %>").attr('disabled', "disabled").find("[value='" + $("#<% = ddlShippingCountry.ClientID %> :selected").val() + "']").attr("selected", "selected");
                        $("#<% = txtBillingZone.ClientID %>").attr('disabled', "disabled").val($("#<% = txtShippingZone.ClientID %> ").val());

                        $("#<% = txtBillingCustomField1.ClientID %>").val($("#<% = txtShippingCustomField1.ClientID %>").val()).attr('disabled', "disabled");
                        $("#<% = txtBillingCustomField2.ClientID %>").val($("#<% = txtShippingCustomField2.ClientID %>").val()).attr('disabled', "disabled");
                        $("#<% = txtBillingCustomField3.ClientID %>").val($("#<% = txtShippingCustomField3.ClientID %>").val()).attr('disabled', "disabled");
                    }
                    else {
                        $("#<% = txtBillingName.ClientID %>").val("").removeAttr('disabled');
                        $("#<% = txtBillingCity.ClientID %>").val("").removeAttr('disabled');
                        $("#<% = txtBillingZip.ClientID %>").val("").removeAttr('disabled');
                        $("#<% = txtBillingAddress.ClientID %>").val("").removeAttr('disabled');
                        $("#<% = ddlBillingCountry.ClientID %>").removeAttr('disabled');
                        $("#<% = txtBillingZone.ClientID %>").removeAttr('disabled');

                        $("#<% = txtBillingCustomField1.ClientID %>").removeAttr('disabled');
                        $("#<% = txtBillingCustomField2.ClientID %>").removeAttr('disabled');
                        $("#<% = txtBillingCustomField3.ClientID %>").removeAttr('disabled');
                    }

                    //TODO make textboxes work!!!!!!!!!!!!!!!
                    //!!!!!!!!!
                    $("#<%= hfBillingID.ClientID %>").val($("#<%= hfShippingID.ClientID %>").val());
                });

                $("#ModalRecheckShippingBehaviour_backgroundElement").live("click", function () {
                    $find("ModalRecheckShippingBehaviour").hide();
                });
            });

            function ShowModalChooseAddress(typeBindAddress) {
                document.body.style.overflowX = 'hidden';
                $find('ModalAddressBehaviour').show();
                document.getElementById('ModalAddressBehaviour_backgroundElement').onclick = HideModalChooseAddress;
                document.getElementById('<% =hfTypeBindAddress.ClientID %>').value = typeBindAddress;
                $("#<% = CustomerContacts.ClientID %> input").removeAttr("checked");
                if (typeBindAddress == "shipping") {
                    $("#<% = CustomerContacts.ClientID %> [value='" + $("#<%= hfShippingID.ClientID %>").val() + "']").attr("checked", "checked");
                }
                else {
                    $("#<% = CustomerContacts.ClientID %> [value='" + $("#<%= hfBillingID.ClientID %>").val() + "']").attr("checked", "checked");
                }
            }

            function HideModalChooseAddress() {
                $find("ModalAddressBehaviour").hide();
                $('select', 'object', 'embed').each(function () {
                    $(this).show(); /*.style.visibility = 'visible'*/
                });
            }

            function HideShippingModal() {
                $find("ModalShippingBehaviour").hide();
            }

            function HideRecheckShippingModal() {
                $find("ModalRecheckShippingBehaviour").hide();
            }

            $(".phoneSource").blur(function () {
                if ($(".phoneDest").val() == "") {
                    $(".phoneDest").val(convertPhone($(".phoneSource").val()));
                }
            });

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="ContentPlaceHolder_Head">
    <style type="text/css">
        .style1 {
            width: 67px;
        }
    </style>
</asp:Content>
