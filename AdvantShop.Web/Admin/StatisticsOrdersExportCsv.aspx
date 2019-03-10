<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.StatisticsOrdersExportCsv" CodeBehind="StatisticsOrdersExportCsv.aspx.cs" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="OrderSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
            <li class="neighbor-menu-item"><a href="OrderStatuses.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderStatuses%></a></li>
            <li class="neighbor-menu-item"><a href="OrderByRequest.aspx">
                <%= Resource.Admin_MasterPageAdmin_OrderByRequest%></a></li>
            <li class="neighbor-menu-item selected"><a href="StatisticsOrdersExportCsv.aspx">
                <%= Resource.Admin_Statistics_ExportOrdersHeader%></a></li>
        </menu>
        <div class="panel-add">
            <a href="EditOrder.aspx?OrderID=addnew" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Add %>
                <%= Resource.Admin_MasterPageAdmin_Order %></a>
        </div>
    </div>
    <div class="content-own">
        <div id="mainDiv" runat="server">
            <div>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblAdminHead" CssClass="AdminHead" runat="server" Text="По заказам"></asp:Label><br />
                            <asp:Label ID="lblAdminSubHead" CssClass="AdminSubHead" runat="server" Text="Выгрузка данных по заказам в csv"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>

            <br />
            <br />
            <div style="text-align: center">
                <asp:SqlDataSource ID="sdsStatus" SelectCommand="SELECT OrderStatusId,StatusName  FROM [Order].[OrderStatus]"
                    runat="server" OnInit="sdsStatus_Init"></asp:SqlDataSource>
                <asp:Panel ID="pnSearch" runat="server">

                    <table class="export-settings">
                        <tr>
                            <td>
                                <span><%= Resource. Admin_ExportOrdersExcel_CheckStatus %></span>
                            </td>
                            <td>
                                <div style="float: left;">
                                    <asp:CheckBox ID="chkStatus" runat="server"
                                        Checked="false" />
                                </div>
                                <div style="float: left;">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" DataSourceID="sdsStatus" DataValueField="OrderStatusId"
                                        DataTextField="StatusName">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <span><%= Resource. Admin_ExportOrdersExcel_CheckDate %></span>
                            </td>
                            <td>
                                <div>
                                    <asp:CheckBox ID="chkDate" runat="server" Checked="false" />
                                </div>
                                <div class="dp" style="width: 150px; float: left;" id="divfrom">
                                    от
                                    <asp:TextBox ID="txtDateFrom" runat="server" Width="80"></asp:TextBox><img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                </div>
                                <div class="dp" style="width: 150px; float: left;" id="divto">
                                    до
                                    <asp:TextBox ID="txtDateTo" runat="server" Width="80"></asp:TextBox><img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>
                                    <% = Resource.Admin_ImportCsv_ChoseEncoding%>:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlEncoding" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div id="divbtnAction" runat="server" style="margin-top: 20px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Button CssClass="btn btn-middle btn-add" runat="server" ID="btnDownload" OnClick="btnDownload_Click"
                                    Text="<%$ Resources: Resource, Admin_ExportExcel_Export %>" />
                            </td>
                            <td>&nbsp;<asp:Literal ID="ltLink" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <script type="text/javascript">
                    function switchDate() {
                        if ($("#<%= chkDate.ClientID %>").is(":checked")) {
                            $("#divfrom").removeAttr("disabled");
                            $("#divto").removeAttr("disabled");

                            $("#<%= txtDateTo.ClientID %>").removeAttr("disabled");
                            $("#<%= txtDateFrom.ClientID %>").removeAttr("disabled");
                        }
                        else {
                            $("#divfrom").attr("disabled", "disabled");
                            $("#divto").attr("disabled", "disabled");

                            $("#<%= txtDateTo.ClientID %>").attr("disabled", "disabled");
                            $("#<%= txtDateFrom.ClientID %>").attr("disabled", "disabled");

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
                                            url: "HttpHandlers/ExportOrdersStatisticData.ashx",
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

                                                <%--if ((data.Processed == data.Total) || (!data.IsRun)) {
                                                    stopTimer();
                                                    if ($("#NotDoPost").val() != "true") {
                                                        window.__doPostBack('<%=btnAsyncLoad.UniqueID%>', '');
                                                    }
                                                }--%>

                                                if (!data.IsRun) {
                                                    stopTimer();
                                                    if (data.Error != 0)
                                                        $("#hlDownloadImportLog").css("display", "inline");
                                                    $("#hlStart").css("display", "inline");
                                                    $("#lblRes").css("display", "inline");
                                                    $("#downloadFile").css("display", "inline");
                                                    if (data.Error == 0) {
                                                        $("#lblRes").html("<% =  Resource.Admin_ImportXLS_UpdoadingSuccessfullyCompleted %>");
                                                    }
                                                    else {
                                                        $("#lblRes").html("<% =  Resource.Admin_ImportXLS_UpdoadingCompletedWithErrors %>");
                                                        $("#lblRes").css("color", "red");
                                                    }
                                                    $("#<%= linkCancel.ClientID %>").css("display", "none");
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
                            OnClick="linkCancel_Click"></asp:LinkButton>
                        <span id="lblRes" style="display: none; font-weight: bold; color: blue"></span>
                        <br />
                        <a id="hlDownloadImportLog" style="display: none" class='Link' href="HttpHandlers/DownloadLog.ashx">
                            <%= Resource.Admin_ImportXLS_DownloadImportLog%></a><br />
                        <a id="downloadFile" style="display: none" class='Link' href='../content/price_temp/<% = ExtStrFileName %> '>
                            <%= Resource.Admin_ExportExcel_DownloadFile%></a><br />
                        <a id="hlStart" style="display: none" class='Link' href="StatisticsOrdersExportCsv.aspx">
                            <%= Resource.Admin_ExportCsv_ExportAgain%></a><br />
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
