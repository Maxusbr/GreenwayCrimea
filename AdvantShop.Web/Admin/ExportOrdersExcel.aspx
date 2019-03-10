<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    Codebehind="ExportOrdersExcel.aspx.cs" Inherits="Admin.Admin_ExportOrdersExcel" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="OrderSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
            <li class="neighbor-menu-item"><a href="OrderStatuses.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderStatuses%></a></li>
            <li class="neighbor-menu-item"><a href="OrderByRequest.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderByRequest%></a></li>
            <li class="neighbor-menu-item selected"><a href="ExportOrdersExcel.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrdersExcelExport%></a></li>
        </menu>
        <div class="panel-add">
            <a href="EditOrder.aspx?OrderID=addnew" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Add %>
                <%= Resource.Admin_MasterPageAdmin_Order %></a>
        </div>
    </div>
    <div class="content-own">
        <div id="mainDiv" runat="server">
            <center>
            <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_ExportOrdersExcel_Orders %>"></asp:Label><br />
            <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_ExportOrdersExcel_DownloadOrders %>"></asp:Label>
            <br />
        </center>
            <br />
            <br />
            <div style="text-align: center">
                <asp:SqlDataSource ID="sdsStatus" SelectCommand="SELECT OrderStatusId,StatusName  FROM [Order].[OrderStatus]"
                    runat="server" OnInit="sdsStatus_Init"></asp:SqlDataSource>
                <asp:Panel ID="pnSearch" runat="server">
                    <asp:CheckBox ID="chkStatus" runat="server" Text="<%$ Resources: Resource, Admin_ExportOrdersExcel_CheckStatus %>"
                        Checked="false" />
                    <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" DataSourceID="sdsStatus" DataValueField="OrderStatusId"
                        DataTextField="StatusName">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <center>
                    <table>
                        <tr style="text-align: center;">
                            <td colspan="2">
                                <asp:CheckBox ID="chkDate" runat="server" Text="<%$ Resources: Resource, Admin_ExportOrdersExcel_CheckDate %>"
                                    Checked="false" />
                            </td>
                        </tr>
                        <tr class="dp">
                            <td style="text-align: right; width: 90px;">
                                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Resource, Admin_ExportOrdersExcel_DateFrom %>"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateFrom" runat="server" Font-Size="10px" Width="65" Enabled="false" ></asp:TextBox>
                                <asp:Image ID="popupDateFrom" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png"
                                    Style="display: none;" class="icon-calendar"/>
                                <asp:Image ID="popupDateFromDisabled" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleBW.png" />
                            </td>
                        </tr>
                        <tr class="dp">
                            <td style="text-align: right; width: 90px;">
                                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Resource, Admin_ExportOrdersExcel_DateTo %>"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateTo" runat="server" Font-Size="10px" Width="65" Enabled="false"></asp:TextBox>
                                <asp:Image ID="popupDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png"
                                    Style="display: none;" class="icon-calendar" />
                                <asp:Image ID="popupDateToDisabled" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleBW.png" />
                            </td>
                        </tr>
                    </table>
                </center>
                </asp:Panel>
                <asp:Button ID="btnAsyncLoad" runat="server" Text="" Visible="false" OnClick="btnAsyncLoad_Click" />
                <input id="NotDoPost" type="hidden" value="<% =  NotDoPost%>" />
                <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Resource, Admin_ExportOrdersExcel_Export %>"
                    OnClick="btnDownload_Click" />
                <br />
                <script type="text/javascript">
                    function switchDate() {
                        if ($("#<%= chkDate.ClientID %>").is(":checked")) {
                            $("#<%= txtDateTo.ClientID %>").removeAttr("disabled");
                            $("#<%= txtDateFrom.ClientID %>").removeAttr("disabled");
                            $("#<%= popupDateTo.ClientID %>").show();
                            $("#<%= popupDateFrom.ClientID %>").show();
                            $("#<%= popupDateToDisabled.ClientID %>").hide();
                            $("#<%= popupDateFromDisabled.ClientID %>").hide();
                        }
                        else {
                            $("#<%= txtDateTo.ClientID %>").attr("disabled", "disabled");
                            $("#<%= txtDateFrom.ClientID %>").attr("disabled", "disabled");
                            $("#<%= popupDateTo.ClientID %>").hide();
                            $("#<%= popupDateFrom.ClientID %>").hide();
                            $("#<%= popupDateToDisabled.ClientID %>").show();
                            $("#<%= popupDateFromDisabled.ClientID %>").show();
                        }
                    }
                    function switchStatus() {
                        if ($("#<%= chkStatus.ClientID %>").is(":checked")) {
                            $("#<%= ddlStatus.ClientID %>").removeAttr("disabled");
                        }
                        else {
                            $("#<%= ddlStatus.ClientID %>").attr("disabled", "disabled");
                        }
                    }
                    $(document).ready(function () {
                        $("#<%= chkStatus.ClientID %>").click(function () {
                            switchStatus();
                        });
                        $("#<%= chkDate.ClientID %>").click(function () {
                            switchDate();
                        });
                        switchDate();
                        switchStatus();
                    });
                </script>
                <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
                    <div class="progressDiv">
                        <div class="progressbarDiv" id="textBlock">
                        </div>
                        <div id="InDiv" class="progressInDiv">
                            &nbsp;
                        </div>
                    </div>
                    <div id="Div4">
                        <% = Resource. Admin_CommonStatictic_CurrentProcess%> : <a id="lCurrentProcess"></a>
                    </div>
                    <br />
                    <div id="divScript" runat="server">
                        <script type="text/javascript">
                            var _timerId = -1;
                            var _stopLinkId = "#<%= linkCancel.ClientID %>";

                        $(document).ready(function () {

                            $.fjTimer({
                                interval: 100,
                                repeat: true,
                                tick: function (counter, timerId) {
                                    _timerId = timerId;
                                    jQuery.ajax({
                                        url: "HttpHandlers/CommonStatisticData.ashx",
                                        dataType: "json",
                                        cache: false,
                                        success: function (data) {
                                            var processed;
                                            if (data.Total != 0) {
                                                processed = Math.round(data.Processed / data.Total * 100);
                                            } else {
                                                processed = 0;
                                            }

                                            $("#textBlock").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                            $("#InDiv").css("width", processed + "%");

                                            $("#lCurrentProcess").html(data.CurrentProcessName);
                                            $("#lCurrentProcess").attr("href", data.CurrentProcess);

                                            if ((data.Processed == data.Total) || (!data.IsRun)) {
                                                stopTimer();
                                                if ($("#NotDoPost").val() != "true") {
                                                    window.__doPostBack('<%=btnAsyncLoad.UniqueID%>', '');
                                                    }
                                                }
                                            }
                                        });
                                    }
                                });

                                $(_stopLinkId).click(function () {
                                    if (_timerId != -1) {
                                        stopTimer();
                                    }
                                });
                            });

                            function stopTimer() {
                                clearInterval(_timerId);
                            }
                        </script>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_CancelImport%>"
                            OnClick="linkCancel_Click"></asp:LinkButton><br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <% =Link%>
                <br />
                <asp:Label ID="lError" runat="server" ForeColor="Blue" Font-Bold="true" Visible="false" EnableViewState="false"></asp:Label>
            </div>
        </div>
        <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
            <center>
            <h2>
                <%=  Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </center>
        </div>
    </div>
</asp:Content>
