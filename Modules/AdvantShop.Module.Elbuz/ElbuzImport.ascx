<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ElbuzImport.ascx.cs" Inherits="Advantshop.Modules.UserControls.Admin_ElbuzImportModule" %>

<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td k style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: ElbuzImport_Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <div id="mainDiv" runat="server">
                <asp:Panel ID="pUploadExcel" runat="server">
                    <asp:CheckBox ID="ckbDisableProducts" runat="server" Text="Деактивировать товары, которых нет в прайсе"/><br/>
                    <asp:CheckBox ID="ckbDisableCategories" runat="server" Text=" Деактивировать категории, которых нет в прайсе"/><br/>
                    <asp:Label runat="server" Text="Брать артикул товара из выбранного поля"/>
                    <asp:DropDownList runat="server" ID="ddlTypeArtNo">
                        <asp:ListItem Value="Code" Text="Код товара"></asp:ListItem>
                        <asp:ListItem Value="ArtNo" Text="Артикул товара" Selected="True"></asp:ListItem>
                    </asp:DropDownList><br/>
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources: ElbuzImport_CsvFilePath%>" />
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="btnLoad" runat="server" Height="22px" Text="<%$ Resources: ElbuzImport_Upload %>"
                                OnClick="btnLoad_Click" />
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
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzImport_AddProducts%>"></asp:Label><span
                                                                                                                     id="addBlock"></span>
                            <br />
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzImport_UpdateProducts%>"></asp:Label><span
                                                                                                                        id="updateBlock"></span>
                            <br />
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzImport_ProductsWithError%>"></asp:Label>
                            <span id="errorBlock"></span>
                            <br />
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzImport_CurrentProcess%>"></asp:Label><a id="lCurrentProcess"></a>
                        </div>
                        <script type="text/javascript">
                            var _timerId = -1;
                            var _stopLinkId = "#<%= linkCancel.ClientID %>";

                            $(document).ready(function() {
                                $("#lProgress").css("display", "inline");
                                $.fjTimer({
                                    interval: 500,
                                    repeat: true,
                                    tick: function(counter, timerId) {
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
                                            success: function(data) {
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
                                                        $("#<%= lblRes.ClientID %>").html("<%= GetLocalResourceObject("ElbuzImport_UpdoadingSuccessfullyCompleted") %>");
                                                    } else {
                                                        $("#<%= lblRes.ClientID %>").html("<%= GetLocalResourceObject("ElbuzImport_UpdoadingCompletedWithErrors") %>");
                                                        $("#<%= lblRes.ClientID %>").css("color", "red");
                                                    }
                                                    $("#<%= linkCancel.ClientID %>").css("display", "none");
                                                }
                                            }
                                        });
                                    }
                                });

                                $(_stopLinkId).click(function() {
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
                                <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources: ElbuzImport_CancelImport%>"
                                                OnClick="linkCancel_Click"></asp:LinkButton><br />
                                <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                                <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                                               Text="<%$ Resources:ElbuzImport_DownloadImportLog%>" NavigateUrl="~/Admin/HttpHandlers/DownloadLog.ashx" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>