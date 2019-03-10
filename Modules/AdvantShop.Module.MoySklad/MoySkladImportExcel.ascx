<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.MoySklad.Modules_MoySklad_MoySkladImportExcel" Codebehind="MoySkladImportExcel.ascx.cs" %>
<div style="padding-left: 10px;">
    <asp:Panel ID="pUpload" runat="server">
        <table>
            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">
                        <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: MoySklad_Header%>" /></span>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>
            <tr>
                <td><asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: MoySklad_Header%>" />:</td>
                <td><asp:FileUpload ID="FileUpload" runat="server" Width="220" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnNext" runat="server" Text="<%$ Resources: MoySklad_Next%>" OnClick="btnNext_Click" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSelectCol">
        <table>
            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">
                        <asp:Localize ID="Localize99" runat="server" Text="<%$ Resources: MoySklad_Header%>" /></span>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>
            <tr>
                <td><asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: MoySklad_DeletePropertyNotFile%>" />:</td>
                <td><asp:CheckBox runat="server" ID="cbDeletePropertyNotFile"/></td>
            </tr>
            <tr>
                <td><asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: MoySklad_SelectKey%>" />:</td>
                <td><asp:DropDownList runat="server" ID="ddlKey" Width="220"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: MoySklad_SelectPropertyes%>" />:</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBoxList runat="server" ID="cblColumnst" Width="100%"></asp:CheckBoxList>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnRun" runat="server" Text="<%$ Resources: MoySklad_Next%>" OnClick="btnRun_Click" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlProcessing">
        <center>
            <div style="text-align: left; width: 600px;">
                <center>
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                </center>
                <span id="lProgress" style="display: none">/</span><br />
                <div id="OutDiv" runat="server" style="margin-bottom: 5px">
                    <div class="progressDiv">
                        <div class="progressbarDiv" id="textBlock">
                        </div>
                        <div id="InDiv" class="progressInDiv">
                            &nbsp;
                        </div>
                    </div>
                    <br />
                    <div>
                        <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: MoySklad_LoadValues%>" />:<span id="rowBlock" class="">
                                                                                                                   </span>
                        <br />
                        <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: MoySklad_CountError%>" />:<span id="errorBlock"
                                                                                                                         class=""></span>
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
                                        url: "../Modules/MoySklad/ImportDataExcel.ashx",
                                        dataType: "json",
                                        cache: false,
                                        success: function(data) {
                                            if (data.Processed != 0) {
                                                $("#lProgress").css("display", "none");
                                            }
                                            var processed;
                                            if (data.Total != 0) {
                                                processed = Math.round(data.Processed / data.Total * 100);
                                            } else {
                                                processed = 0;
                                            }


                                            $("#textBlock").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                            $("#InDiv").css("width", processed + "%");

                                            $("#rowBlock").html(data.Update);
                                            $("#errorBlock").html(data.Error);

                                            if ((data.Processed == data.Total && data.Total != 0) || (!data.IsRun)) {
                                                stopTimer();
                                                $("#<%= hlDownloadImportLog.ClientID %>").css("display", "inline");
                                                $("#<%= lblRes.ClientID %>").css("display", "inline");
                                                if (data.Error == 0) {
                                                    $("#<%= lblRes.ClientID %>").html("<%= GetLocalResourceObject("MoySklad_LoadSuccess") %>");
                                                } else {
                                                    $("#<%= lblRes.ClientID %>").html("<%= GetLocalResourceObject("MoySklad_LoadSuccessErrors") %>");
                                                    $("#<%= lblRes.ClientID %>").css("color", "red");
                                                }
                                                $("#<%= linkCancel.ClientID %>").css("display", "none");
                                                $("#<%= pnlLog.ClientID %>").show();
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
                <center>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources: MoySklad_CencelLoad%>"
                                            OnClick="linkCancel_Click"></asp:LinkButton><br />
                            <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
            </div>
        </center>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlLog">
        <div style="padding-top: 30px;">
            <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server"
                           Text="<%$ Resources: MoySklad_DownloadImportLog%>" />
        </div>
    </asp:Panel>
</div>