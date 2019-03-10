<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ElbuzPropertiesImport.ascx.cs"
    Inherits="Advantshop.Modules.UserControls.Admin_ElbuzPropertiesImportModule" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td k style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: ElbuzPropertiesImport_Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <div id="mainDiv" runat="server">
                <asp:Panel ID="pUploadExcel" runat="server">
                    <asp:Label runat="server" Text="Брать артикул товара из выбранного поля"/>
                    <asp:DropDownList runat="server" ID="ddlTypeArtNo">
                        <asp:ListItem Value="Code" Text="Код товара"></asp:ListItem>
                        <asp:ListItem Value="ArtNo" Text="Артикул товара" Selected="True"></asp:ListItem>
                    </asp:DropDownList><br/>
                    <table style="width: 420px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources: ElbuzPropertiesImport_ChooseZipFile%>"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="fupZipPhotos" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources: ElbuzPropertiesImport_ChooseFile%>"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Height="22px" Text="<%$ Resources: ElbuzPropertiesImport_Upload %>"
                                    OnClick="btnLoad_Click" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdatePanel ID="upExceptions" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lvProperties" />
                        <asp:AsyncPostBackTrigger ControlID="lvExceptions" />
                        <asp:PostBackTrigger ControlID="btnStartProcess" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlExceptions" runat="server" Visible="False">
                            <div style="width: 280px; height: 30px; float: left; font-weight: bold;">
                                <asp:Label ID="lblProperties" runat="server" Text="<%$ Resources:ElbuzPropertiesImport_Properties %>"></asp:Label></div>
                            <div style="width: 280px; height: 30px; float: right; font-weight: bold;">
                                <asp:Label ID="lblExceptions" runat="server" Text="<%$ Resources:ElbuzPropertiesImport_Exceptions %>"></asp:Label></div>
                            <div style="width: 280px; height: 300px; overflow: scroll; float: left; border: 1px solid grey;">
                                <asp:ListView ID="lvProperties" runat="server" ItemPlaceholderID="itemPlaceholderID"
                                    OnItemCommand="lvProperties_OnItemCommand">
                                    <LayoutTemplate>
                                        <table style="width: 260px;">
                                            <tr runat="server" id="itemPlaceholderID">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Property")%>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Property") %>' CommandName="AddException"
                                                    Text="<%$ Resources:ElbuzPropertiesImport_AddException %>" Visible='<%# !Convert.ToBoolean(Eval("IsUsed")) %>'></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                            <div style="width: 280px; height: 300px; overflow: scroll; float: right; border: 1px solid grey;">
                                <asp:ListView ID="lvExceptions" runat="server" ItemPlaceholderID="itemPlaceholderID"
                                    OnItemCommand="lvExceptions_OnItemCommand">
                                    <LayoutTemplate>
                                        <table style="width: 260px;">
                                            <tr runat="server" id="itemPlaceholderID">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# (string)Container.DataItem %>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" CommandArgument="<%# (string)Container.DataItem %>"
                                                    CommandName="RemoveException" Text="<%$ Resources:ElbuzPropertiesImport_RemoveException %>"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                            <div class="clear" style="margin-bottom: 10px;">
                            </div>
                           <%-- <div style="margin-bottom: 20px; margin-top: 20px;">
                                <table>
                                    <tr>
                                        <td style="width: 200px;">
                                            <asp:Label runat="server" ID="Label5" Text="Использовать для идентификации поле артикла"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="ckbUseArtNo" runat="server" Text="" />
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                         <%--   <div style="font-weight: bold; margin-bottom: 10px; margin-top: 20px;">
                                <asp:Label runat="server" ID="Label3" Text="<%$ Resources:ElbuzPropertiesImport_SizeSettings %>"> </asp:Label></div>
                            <div style="margin-bottom: 20px;">
                                <table>
                                    <tr>
                                        <td style="width: 200px;">
                                            <asp:Label runat="server" ID="lblTextSizeSettings" Text="<%$ Resources:ElbuzPropertiesImport_SizeField %>"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPropertySize" runat="server" Width="200">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblTextSizeSplitter" Text="<%$ Resources:ElbuzPropertiesImport_SizeSpliter %>"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSizeSpliter" runat="server" Width="50" Text="-"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                            <asp:Button ID="btnStartProcess" runat="server" OnClick="btnStartProcess_Click" Text="Начать импорт" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="text-align: left; width: 600px;">
                    <div style="text-align: center;">
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    </div>
                    <span id="lProgressProperties" style="display: none">/</span><br />
                    <div id="OutPropertiesDiv" runat="server" visible="false" style="margin-bottom: 5px">
                        <div class="progressDiv">
                            <div class="progressbarDiv" id="textBlockProperties">
                            </div>
                            <div id="InPropertiesDiv" class="progressInDiv">
                                &nbsp;
                            </div>
                        </div>
                        <br />
                        <div>
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzPropertiesImport_AddProducts%>"></asp:Label>
                            <span id="addPropertiesBlock"></span>
                            <br />
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzPropertiesImport_UpdateProducts%>"></asp:Label>
                            <span id="updatePropertiesBlock"></span>
                            <br />
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzPropertiesImport_ProductsWithError%>"></asp:Label>
                            <span id="errorPropertiesBlock"></span>
                            <br />
                            <asp:Label runat="server" Text="<%$ Resources:ElbuzPropertiesImport_CurrentProcess%>"></asp:Label>
                            <a id="lPropertiesCurrentProcess"></a>
                        </div>
                        <script type="text/javascript">
                            var _timerPropertiesId = -1;
                            var _stopPropertiesLinkId = "#<%= linkPropertiesCancel.ClientID %>";

                            $(document).ready(function () {
                                $("#lProgressProperties").show();
                                $.fjTimer({
                                    interval: 500,
                                    repeat: true,
                                    tick: function (counter, timerId) {
                                        _timerPropertiesId = timerId;

                                        switch ($("#lProgressProperties").html()) {
                                            case "\\":
                                                $("#lProgressProperties").html("|");
                                                break;
                                            case "|":
                                                $("#lProgressProperties").html("/");
                                                break;
                                            case "/":
                                                $("#lProgressProperties").html("--");
                                                break;
                                            case "-":
                                                $("#lProgressProperties").html("\\");
                                                break;
                                        }

                                        jQuery.ajax({
                                            url: "HttpHandlers/CommonStatisticData.ashx",
                                            dataType: "json",
                                            cache: false,
                                            success: function (data) {
                                                if (data.Processed != 0) {
                                                    $("#lProgressProperties").hide();
                                                }
                                                var processed = 0 ;
                                                if (data.Total != 0) {
                                                    processed = Math.round(data.Processed / data.Total * 100);
                                                } else {
                                                    processed = 0;
                                                }

                                                $("#textBlockProperties").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                                $("#InPropertiesDiv").css("width", processed + "%");

                                                $("#addPropertiesBlock").html(data.Add);
                                                $("#updatePropertiesBlock").html(data.Update);
                                                $("#errorPropertiesBlock").html(data.Error);
                                                $("#lPropertiesCurrentProcess").html(data.CurrentProcess);
                                                $("#lPropertiesCurrentProcess").attr("href", data.CurrentProcess);

                                                if (data.Processed == data.Total && data.Total != 0) {
                                                    stopPropertiesTimer();
                                                    $("#<%= hlDownloadImportLog.ClientID %>").show();
                                                    $("#<%= lblPropertiesRes.ClientID %>").show();
                                                    if (data.Error == 0) {
                                                        $("#<%= lblPropertiesRes.ClientID %>").html("<%= (string)GetLocalResourceObject("ElbuzPropertiesImport_UpdoadingSuccessfullyCompleted") %>");
                                                    }
                                                    else {
                                                        $("#<%= lblPropertiesRes.ClientID %>").html("<%= (string)GetLocalResourceObject("ElbuzPropertiesImport_UpdoadingCompletedWithErrors") %>");
                                                        $("#<%= lblPropertiesRes.ClientID %>").css("color", "red");
                                                    }
                                                    $("#<%= linkPropertiesCancel.ClientID %>").hide();
                                                }
                                            }
                                        });
                                    }
                                });

                                $(_stopPropertiesLinkId).click(function () {
                                    if (_timerPropertiesId != -1) {
                                        stopPropertiesTimer();
                                    }
                                });
                            });

                            function stopPropertiesTimer() {
                                clearInterval(_timerPropertiesId);
                            }
                        </script>
                    </div>
                    <asp:UpdatePanel ID="upProperties" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="linkPropertiesCancel" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:LinkButton ID="linkPropertiesCancel" runat="server" Text="<%$ Resources: ElbuzPropertiesImport_CancelImport%>"
                                OnClick="linkCancel_Click"></asp:LinkButton><br />
                            <asp:Label ID="lblPropertiesRes" runat="server" Font-Bold="True" ForeColor="Blue"
                                Style="display: none" /><br />
                            <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                                Text="<%$ Resources: ElbuzPropertiesImport_DownloadImportLog%>" NavigateUrl="~/Admin/HttpHandlers/DownloadLog.ashx" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </td>
    </tr>
</table>
