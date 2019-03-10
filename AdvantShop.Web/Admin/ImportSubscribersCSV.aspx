<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.ImportSubscribersCSV" Codebehind="ImportSubscribersCSV.aspx.cs" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="CustomerSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Buyers%></a></li>
            <li class="neighbor-menu-item"><a href="CustomersGroups.aspx">
                <%= Resource.Admin_MasterPageAdmin_CustomersGroups%></a></li>
            <li class="neighbor-menu-item selected"><a href="Subscription.aspx">
                <%= Resource.Admin_MasterPageAdmin_SubscribeList%></a></li>
        </menu>
        <div class="panel-add">
            <%= Resource.Admin_MasterPageAdmin_Add %>
            <a href="Product.aspx?categoryid=<%=Request["categoryid"] ?? "0"  %>" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Product %></a>, <a href="#" onclick="open_window('m_Category.aspx?CategoryID=<%=Request["categoryid"] ?? "0" %>&mode=create', 750, 640); return false;"
                    class="panel-add-lnk">
                    <%= Resource.Admin_MasterPageAdmin_Category %></a>
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
                            <asp:Label runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_Subscription_Unreg_Header %>"></asp:Label><br />
                            <asp:Label runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_ImportSubscribersCSV_Name %>"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:Panel ID="pUpload" runat="server">
                    <div id="divStart" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <span>
                                        <%= Resource.Admin_ImportCsv_CsvPath%>&nbsp;</span>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload" runat="server" Width="220" />
                                    <div data-plugin="help" class="help-block">
                                        <div class="help-icon js-help-icon"></div>
                                        <article class="bubble help js-help">
                                            <header class="help-header">
                                                <%= Resource.Admin_ImportCsv_CsvEncoding%>
                                            </header>
                                            <div class="help-content">
                                                <%= Resource.Admin_ImportCsv_CsvEncodingType_Utf8%>
                                            </div>
                                        </article>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnAction" runat="server" Text="<%$ Resources:Resource, Admin_ImportCsv_btnAction %>"
                            OnClick="btnAction_Click" />
                        <span id="fuPhotoError" style="color: Red; font-weight: bold; display: none;">
                            <%= Resource.Admin_ImportCsv_SelectFile %></span>
                    </div>
                </asp:Panel>
                <div style="padding-left: 10px;">
                    <div style="margin-bottom: 10px">
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    </div>
                    <div style="text-align: center;">
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
                                <% = Resource.Admin_ImportXLS_AddProducts %>
                                : <span id="addBlock" class=""></span>
                                <br />
                                <% =Resource.Admin_ImportXLS_UpdateProducts%>
                                : <span id="updateBlock" class=""></span>
                                <br />
                                <% = Resource. Admin_ImportXLS_ProductsWithError %>
                                : <span id="errorBlock" class=""></span>
                                <br />
                                <% = Resource. Admin_CommonStatictic_CurrentProcess%>
                                : <a id="lCurrentProcess"></a>
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
                                                    var processed;
                                                    if (data.Total != 0) {
                                                        processed = Math.round(data.Processed / data.Total * 100);
                                                    } else {
                                                        processed = 0;
                                                    }

                                                    //$("#textBlock").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                                    $("#textBlock").html(processed + "%");
                                                    $("#InDiv").css("width", processed + "%");

                                                    $("#addBlock").html(data.Add);
                                                    $("#updateBlock").html(data.Update);
                                                    $("#errorBlock").html(data.Error);
                                                    $("#lCurrentProcess").html(data.CurrentProcessName);
                                                    $("#lCurrentProcess").attr("href", data.CurrentProcess);

                                                    if ((!data.IsRun)) {
                                                        stopTimer();
                                                        if (data.Error != 0)
                                                            $("#<%= hlDownloadImportLog.ClientID %>").css("display", "inline");
                                                        $("#<%= hlStart.ClientID %>").css("display", "inline");
                                                        $("#<%= lblRes.ClientID %>").css("display", "inline");
                                                        if (data.Error == 0) {
                                                            $("#<%= lblRes.ClientID %>").html("<% =  Resource.Admin_ImportXLS_UpdoadingSuccessfullyCompleted %>");
                                                        }
                                                        else {
                                                            $("#<%= lblRes.ClientID %>").html("<% =  Resource.Admin_ImportXLS_UpdoadingCompletedWithErrors %>");
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
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_CancelImport%>"
                                    OnClick="linkCancel_Click"></asp:LinkButton><br />
                                <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                                <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                                    Text="<%$ Resources:Resource, Admin_ImportXLS_DownloadImportLog%>" NavigateUrl="~/Admin/HttpHandlers/DownloadLog.ashx" />
                                <asp:HyperLink CssClass="Link" ID="hlStart" runat="server" Style="display: none"
                                    Text="<%$ Resources:Resource, Admin_ImportCsv_StartLoad%>" NavigateUrl="ImportSubscribersCSV.aspx" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify" style="text-align: center;">
            <h2>
                <%= Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </div>
        <asp:UpdateProgress runat="server" ID="uprogress">
            <ProgressTemplate>
                <div id="inprogress">
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
                                        <asp:Localize ID="Localize_Admin_Product_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Product_PleaseWait %>"></asp:Localize>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <script type="text/javascript">
            $(function () {
                $('#file_upload').fileupload({
                    url: 'httphandlers/uploadzipphoto.ashx',
                    dataType: 'json',
                    always: function (e, data) {
                        var result = JSON.parse(data.jqXHR.responseText);
                        if (result.error != "") {
                            alert(result.msg);
                        }
                    }
                });
            });
        </script>
    </div>
</asp:Content>
