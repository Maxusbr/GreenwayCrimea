<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ResellerSettings.ascx.cs"
    Inherits="AdvantShop.Module.Resellers.ResellerSettings" %>

<%@ Import Namespace="AdvantShop.Configuration" %>

<div>
    <span class="spanSettCategory">
        <asp:Localize ID="Localize9" runat="server" Text="Настройки импорта каталога" /></span>
    <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
    <hr color="#C2C2C4" size="1px" />
</div>

<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td>
            <div id="mainDiv" runat="server">
                <asp:Panel ID="pUploadExcel" runat="server">
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label7" runat="server" Text="Адрес сайта поставщика"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierSite" runat="server" Width="300px" />
                            </td>
                        </tr>

                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label1" runat="server" Text="Идентификатор дропшиппера"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResellerID" runat="server" Width="300px" />
                            </td>
                        </tr>

                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label3" runat="server" Text="Деактивировать товары, которых нет в прайсе"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbDeactivateProducts" runat="server" />
                            </td>
                        </tr>                        
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label2" runat="server" Text="Наценка на рекомендуемую розничную цену"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMargin" runat="server" />
                            </td>
                        </tr>

                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label4" runat="server" Text="Обнулить количество у товаров, которых нет в прайсе"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbAmountNulling" runat="server" />
                            </td>
                        </tr>
                        
                         <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label6" runat="server" Text="Автоматическое обновление каталога (каждые 6ч)"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbAutoUpdate" runat="server" />
                            </td>
                        </tr>

                         <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label5" runat="server" Text="Дата и время последнего обновления"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLastUpdate" runat="server" />
                                <asp:Button runat="server" OnClick="OnClick" Text="Обновить сейчас" Width="130px"/>
                            </td>
                        </tr>
                        
                        <tr class="rowsPost">
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="Сохранить"
                                    OnClick="btnSave_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <div style="text-align: left; width: 600px;">
        <div style="text-align: center;">
            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
        </div>
        <span id="lProgress" style="display: none">/</span><br />
        <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
            <asp:Label ID="Lbl2" runat="server" Text="Всего товаров : "></asp:Label><asp:Label
                ID="lblOfferRows" runat="server"></asp:Label>
            <div class="progressDiv">
                <div class="progressbarDiv" id="textBlock">
                </div>
                <div id="InDiv" class="progressInDiv">
                    &nbsp;
                </div>
            </div>
            <br />
            <div style="width: 600px; margin: auto; text-align: left; margin-top: 20px;">
                <div style="width: 300px; float: left;">
                    <asp:Label runat="server" Text="Добавлено: "></asp:Label><span
                        id="addBlock"></span>
                    <br />
                    <asp:Label runat="server" Text="Обновленно: "></asp:Label><span
                        id="updateBlock"></span>
                    <br />
                    <asp:Label runat="server" Text="Ошибок: "></asp:Label>
                    <span id="errorBlock"></span>
                    <br />
                    <asp:Label runat="server" Text="Текущий процесс: "></asp:Label><a
                        id="lCurrentProcess"></a>
                </div>
                <div style="width: 300px; float: left;" runat="server" id="divSaasPlanProducts" visible="false" class="divSaasPlanProducts">
                    <input type="hidden" id="hfProductsCount" runat="server" class="hfProductsCount" />
                    Товаров в магазине: <span id="lTotalProducts" style="color: red;"></span>
                    <br>
                    Доступно товаров на тарифе:
                                    <asp:Label ID="lTotalSaasPlanProducts" runat="server" Style="color: red;" CssClass="lTotalSaasPlanProducts"></asp:Label>
                    (<a href="http://www.advantshop.net/myaccount.aspx?mode=changeSaas&shopId=<%= SettingsLic.LicKey %>" target="_blank">Сменить тариф</a>)<br>
                </div>
            </div>
            <div class="clear"></div>
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

                                    $("#<%= lblOfferRows.ClientID %>").html(data.Total);
                                    $("#addBlock").html(data.Add);
                                    $("#updateBlock").html(data.Update);
                                    $("#errorBlock").html(data.Error);
                                    $("#lCurrentProcess").html(data.CurrentProcess);

                                    if ($(".divSaasPlanProducts").length && (".hfProductsCount").length) {
                                        var productsCount = parseInt($(".hfProductsCount").val()) + parseInt(data.Add);
                                        var productsCountInSaasPlan = parseInt($(".lTotalSaasPlanProducts").html());
                                        //$("#lTotalProducts").html(productsCount);                                                        
                                        if ($("#lTotalProducts").html() == "") {
                                            $("#lTotalProducts").html(parseInt($(".hfProductsCount").val()))
                                        }
                                        if (productsCount <= productsCountInSaasPlan) {
                                            $("#lTotalProducts").html(productsCount);
                                        }
                                        else {
                                        }
                                    }

                                    if (data.Processed == data.Total) {
                                        stopTimer();
                                        $("#<%= hlDownloadImportLog.ClientID %>").css("display", "inline");
                                        $("#<%= lblRes.ClientID %>").css("display", "inline");
                                        if (data.Error == 0) {
                                            $("#<%= lblRes.ClientID %>").html("Загрузка каталога завершена");
                                        }
                                        else {
                                            $("#<%= lblRes.ClientID %>").html("Загрузка завершена с ошибками");
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
                    <asp:LinkButton ID="linkCancel" runat="server" Text="Прервать операцию"
                        OnClick="linkCancel_Click"></asp:LinkButton><br />
                    <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                    <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                        Text="Скачать лог импорта (txt)" NavigateUrl="~/Admin/HttpHandlers/DownloadLog.ashx" />
                </contenttemplate>
            </asp:UpdatePanel>
        </div>
    </div>
            </div>
        </td>
    </tr>
</table>