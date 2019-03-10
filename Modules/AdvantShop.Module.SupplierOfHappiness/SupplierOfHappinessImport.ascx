<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierOfHappinessImport.ascx.cs"
    Inherits="Advantshop.Module.SupplierOfHappiness.SupplierOfHappinessImport" %>
<%@ Import Namespace="AdvantShop.Module.SupplierOfHappiness" %>
<div>
    <span class="spanSettCategory">
        <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Import_Header%>" /></span>
    <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
    <hr color="#C2C2C4" size="1px" />
</div>


<asp:Panel ID="pUploadExcel" runat="server">
    <table border="0" cellpadding="2" cellspacing="0">

        <tr class="rowsPost">
            <td style="width: 300px; text-align: left; vertical-align: middle; height: 40px;">
                <asp:Label ID="Label1" runat="server" Text="Обновить каталог в ручном режиме"></asp:Label>
            </td>
            <td style="vertical-align: middle; height: 40px;">
                <asp:Button ID="btnStartFullImport" runat="server" Text="Запуск" OnClick="btnStartFullImport_Click" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="width: 300px; text-align: left; vertical-align: middle; height: 40px;">
                <asp:Label ID="Label2" runat="server" Text="Обновить остатки в ручном режиме"></asp:Label>
            </td>
            <td style="vertical-align: middle; height: 40px;">
                <asp:Button ID="btnStartQuickImport" runat="server" Text="Запуск" OnClick="btnStartQuickImport_Click" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 300px; text-align: left; vertical-align: middle; height: 40px;">
                <asp:Label ID="Label3" runat="server" Text="Обновить кеш изображений"></asp:Label>
            </td>
            <td style="vertical-align: middle; height: 40px;">
                <asp:Button ID="BtnUpdateImg" runat="server" Text="Запуск" OnClick="btnUpdateImg_Click" />
            </td>
        </tr>

    </table>
    <div>
        <h4 style="display: inline; font-size: 10pt;">
            <asp:Localize ID="Localize3" runat="server" Text="Логи импорта"></asp:Localize></h4>
        <hr color="#C2C2C4" size="1px" />

        <asp:ListView runat="server" ID="lvLogs" ItemPlaceholderID="itemPlaceHolder">
            <layouttemplate>
                    <ul style="list-style-type: none; padding: 0; margin: 0;">
                        <li id="itemPlaceHolder" runat="server"></li>
                    </ul>
                </layouttemplate>
            <itemtemplate>
                    <li>
                        <%# Container.DataItemIndex + 1 %>. 
                        <a href="<%# "../Modules/" + SupplierOfHappiness.ModuleID + "/log/" + Container.DataItem %>" target="_blank">
                            <%# Container.DataItem %>
                        </a>
                    </li>
                </itemtemplate>
            <emptydatatemplate>
                    Нет данных
                </emptydatatemplate>
        </asp:ListView>
    </div>
</asp:Panel>
<div style="text-align: left; width: 600px;">
    <div style="text-align: center;">
        <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
    </div>
    <span id="lProgress" style="display: none">/</span><br />
    <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
        <div class="progressDiv">
            <div class="progressbarDiv" id="textBlock">
            </div>
            <div id="InDiv" class="progressInDiv">
                &nbsp;
            </div>
        </div>
        <br />
        <div>
            <asp:Label runat="server" Text="<%$ Resources:YandexMarketImport_AddProducts%>"></asp:Label><span
                id="addBlock"></span>
            <br />
            <asp:Label runat="server" Text="<%$ Resources:YandexMarketImport_UpdateProducts%>"></asp:Label><span
                id="updateBlock"></span>
            <br />
            <asp:Label runat="server" Text="<%$ Resources:YandexMarketImport_ProductsWithError%>"></asp:Label>
            <span id="errorBlock"></span>
            <br />
            <asp:Label runat="server" Text="<%$ Resources:YandexMarketImport_CurrentProcess%>"></asp:Label><a
                id="lCurrentProcess"></a>
        </div>
        <script type="text/javascript">
            var _timerId = -1;
            var _stopLinkId = "#<%= linkCancel.ClientID %>";

                $(document).ready(function () {
                    $("#lProgress").css("display", "inline");
                    $.fjTimer({
                        interval: 500,
                        repeat: true,
                        tick: function (counter, timerId) {
                            _timerId = timerId;

                            switch ($("#lProgress").html()) {
                                case "\\":
                                    $("#lProgress").html("|");
                                    break;
                                case "|":
                                    $("#lProgress").html("/");
                                    break;
                                case "/":
                                    $("#lProgress").html("--");
                                    break;
                                case "-":
                                    $("#lProgress").html("\\");
                                    break;
                            }

                            jQuery.ajax({
                                url: "HttpHandlers/CommonStatisticData.ashx",
                                dataType: "json",
                                cache: false,
                                success: function (data) {
                                    if (data.Processed != 0) {
                                        $("#lProgress").css("display", "none");
                                    }
                                    if (data.Total != 0) {
                                        processed = Math.round(data.Processed / data.Total * 100);
                                    } else {
                                        processed = 0;
                                    }


                                    $("#textBlock").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                    $("#InDiv").css("width", processed + "%");

                                    $("#addBlock").html(data.Add);
                                    $("#updateBlock").html(data.Update);
                                    $("#errorBlock").html(data.Error);
                                    if (!data.IsRun) {
                                        stopTimer();
                                        $("#<%= hlDownloadImportLog.ClientID %>").css("display", "inline");
                                        $("#<%= lblRes.ClientID %>").css("display", "inline");
                                        if (data.Error == 0) {
                                            $("#<%= lblRes.ClientID %>").html("<%= (string)GetLocalResourceObject("YandexMarketImport_UpdoadingSuccessfullyCompleted")%>");
                                        }
                                        else {
                                            $("#<%= lblRes.ClientID %>").html("<%= (string)GetLocalResourceObject("YandexMarketImport_UpdoadingCompletedWithErrors")%>");
                                            $("#<%= lblRes.ClientID %>").css("color", "red");
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
    <div style="text-align: center;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <triggers>
                    <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                </triggers>
            <contenttemplate>
                    <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources: YandexMarketImport_CancelImport%>"
                        OnClick="linkCancel_Click"></asp:LinkButton><br />
                    <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                    <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                        Text="<%$ Resources:YandexMarketImport_DownloadImportLog%>" NavigateUrl="~/Admin/HttpHandlers/DownloadLog.ashx" />
                </contenttemplate>
        </asp:UpdatePanel>
    </div>
</div>
