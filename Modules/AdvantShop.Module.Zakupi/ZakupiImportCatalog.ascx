<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ZakupiImportCatalog.ascx.cs"
    Inherits="AdvantShop.Module.ZakupiImport.ZakupiImportCatalog" %>
<div>
    <span class="spanSettCategory">
        <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: YandexMarketImport_Header%>" /></span>
    <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
    <hr color="#C2C2C4" size="1px" />
</div>


<div id="mainDiv" runat="server">
    <asp:Panel ID="pUploadExcel" runat="server">
        <%-- <div style="display: table-cell; vertical-align: middle;height: 30px; ">
            <input type="radio" name="fileType" value="fileupload" style="margin: 3px;" checked="checked"/><asp:Label runat="server" Text="<%$ Resources: YandexMarketImport_YmlFilePath%>"></asp:Label>
            <input type="radio" name="fileType" value="filelink" style="margin: 3px;" /><asp:Label runat="server" Text="<%$ Resources: YandexMarketImport_YmlFileUrlPath%>"></asp:Label>
        </div>--%>
        <table border="0" cellpadding="2" cellspacing="0">
            <tr class="rowsPost" runat="server" visible="true">
                <td style="width: 200px; text-align: left;">
                    <asp:Label ID="Label2" runat="server" Text="Импорт каталога из Zakupi.net" />
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            </tr>


            <tr class="rowsPost">
                <td style="width: 200px; text-align: left;">
                    <asp:Label ID="Label5" runat="server" Text="Обновлять названия"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="ckbUpdateName" runat="server" Width="400px"></asp:CheckBox>
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 200px; text-align: left;">
                    <asp:Label ID="Label3" runat="server" Text="Обновлять описания"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="ckbUpdateDescription" runat="server" Width="400px"></asp:CheckBox>
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 200px; text-align: left;">
                    <asp:Label ID="Label4" runat="server" Text="Обновлять картинки"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="ckbUpdatePhotos" runat="server" Width="400px"></asp:CheckBox>
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 200px; text-align: left;">
                    <asp:Label ID="Label6" runat="server" Text="Обновлять характеристики"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="ckbUpdateParams" runat="server" Width="400px"></asp:CheckBox>
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 200px; text-align: left;">
                    <asp:Label ID="Label1" runat="server" Text="Ссылка на xml файл"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFileUrlPath" runat="server" Text="1" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr class="rowsPost">
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Сохранить"
                        OnClick="btnSave_Click" Width="150px" />
                </td>
                <td></td>
            </tr>
            <tr class="rowPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory"></span>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>

            <tr class="rowsPost">
                <td>
                    <asp:Button ID="btnLoad" runat="server" Text="Загрузить контент"
                        OnClick="btnLoad_Click" Width="150px" />
                </td>
                <td>
                       <asp:Button ID="btnPartialImport" runat="server" Text="Обновить наличие и цены"
                        OnClick="btnPartialImport_Click" Width="200px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div style="text-align: left; width: 600px;">
        <div style="text-align: center;">
            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
        </div>
        <span id="lProgress" style="display: none">/</span><br />
        <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
            <asp:Label ID="lbl1" runat="server" Text="<%$ Resources:YandexMarketImport_CategoryRows%>"></asp:Label><asp:Label
                ID="lblCategoryRows" runat="server"></asp:Label><br />
            <asp:Label ID="Lbl2" runat="server" Text="<%$ Resources:YandexMarketImport_OfferRows%>"></asp:Label><asp:Label
                ID="lblOfferRows" runat="server"></asp:Label>
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
                                    if (data.Processed == data.Total) {
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
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources: YandexMarketImport_CancelImport%>"
                        OnClick="linkCancel_Click"></asp:LinkButton><br />
                    <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                    <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                        Text="<%$ Resources:YandexMarketImport_DownloadImportLog%>" NavigateUrl="~/Admin/HttpHandlers/DownloadLog.ashx" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
