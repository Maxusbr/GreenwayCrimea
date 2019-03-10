<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.StatisticsProductsExportCsv" CodeBehind="StatisticsProductsExportCsv.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.ExportImport" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Discount_PriceRange.aspx">
                <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
            <li class="neighbor-menu-item"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resources.Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_Voting %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_VotingHistory %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item selected"><a href="Statistics.aspx">
                <%= Resources.Resource.Admin_Statistics_Header %></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tree" EventName="TreeNodePopulate" />
        </Triggers>
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="mpeTree" runat="server" PopupControlID="pTree"
                TargetControlID="hhl" BackgroundCssClass="blackopacitybackground" CancelControlID="btnCancelParent"
                BehaviorID="ModalBehaviour">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HyperLink ID="hhl" runat="server" Style="display: none;" />
            <asp:Panel runat="server" ID="pTree" CssClass="modal-admin">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <span style="font-size: 11pt;">
                                    <asp:Localize ID="Localize_Admin_CatalogLinks_ParentCategory" runat="server" Text="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>"></asp:Localize>:</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height: 360px; width: 450px; overflow: scroll; background-color: White; text-align: left">
                                    <asp:TreeView ID="tree" ForeColor="Black" PopulateNodesFromClient="true" runat="server"
                                        ShowLines="True" ExpandImageUrl="images/loading.gif" BackColor="White" OnTreeNodePopulate="PopulateNode"
                                        AutoPostBack="false" OnSelectedNodeChanged="Select_change" SelectedNodeStyle-BackColor="Yellow" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom" style="height: 36px;">
                                <asp:Button ID="btnUpdateParent" runat="server" Text="<%$ Resources:Resource,Admin_CatalogLinks_UpdateCategory %>"
                                    OnClick="btnUpdateParent_Click" />
                                <asp:Button ID="btnCancelParent" runat="server" Text="<%$ Resources: Resource, Admin_Cancel %>"
                                    Width="67" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="content-own">
        <div id="mainDiv" runat="server">
            <div>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource,Admin_Statistics_ExportProductsHeader %>"></asp:Label><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="Выгрузка данных по товарам в csv"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <div id="divAction" runat="server">
                    <table class="export-settings">
                        <tr>
                            <td>
                                <span>Категория:</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnUpdateParent" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Label ID="lParent" runat="server" Text=""></asp:Label>
                                        <asp:LinkButton ID="lbParentChange" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_ChangeParent %>"
                                            OnClick="lbParentChange_Click"></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hfCatId" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Артикул товара:</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtArtNo" runat="server" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Временной интервал:</span>
                            </td>
                            <td>
                                <div class="dp" style="float:left;width: 150px;">от
                                    <asp:TextBox ID="txtDateFrom" runat="server" Width="80"></asp:TextBox><img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" /></div>
                                
                                <div class="dp" style="float:left;width: 150px;">до
                                    <asp:TextBox ID="txtDateTo" runat="server" Width="80"></asp:TextBox><img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>
                                    <% = Resource.Admin_ImportCsv_ChoseSeparator%>:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSeparetors" Style="display: inline-block" Width="150px"/>
                                &nbsp;
                                <asp:TextBox ID="txtCustomSeparator" runat="server" MaxLength="5" Style="display: none" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>
                                    <% = Resource.Admin_ImportCsv_ChoseEncoding%>:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlEncoding" Width="150px"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <center>
                <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
                    <table>
                        <tr>
                            <td>
                                <div class="progressDiv">
                                    <div class="progressbarDiv" id="textBlock">
                                    </div>
                                    <div id="InDiv" class="progressInDiv">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <br />
                                <% = Resource. Admin_ImportXLS_ProductsWithError %> : <span id="errorBlock" class=""></span>
                                <br/>
                                <% = Resource. Admin_CommonStatictic_CurrentProcess%> : <a id="lCurrentProcess"></a>
                            </td>
                        </tr>
                    </table>
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

                                                $("#errorBlock").html(data.Error);
                                                $("#lCurrentProcess").html(data.CurrentProcessName);
                                                $("#lCurrentProcess").attr("href", data.CurrentProcess);

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
            </center>
                <div id="divbtnAction" runat="server" style="margin-top: 20px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Button CssClass="btn btn-middle btn-add" runat="server" ID="btnDownload" OnClick="btnDownload_Click"
                                    Text="<%$ Resources: Resource, Admin_ExportExcel_Export %>" />
                            </td>
                            <td>&nbsp;<asp:Literal ID="ltLink" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <div style="text-align: center;">
                            <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_CancelImport%>"
                                OnClick="linkCancel_Click"></asp:LinkButton>
                            <span id="lblRes" style="display: none; font-weight: bold; color: blue"></span>
                            <br />
                            <a id="hlDownloadImportLog" style="display: none" class='Link' href="HttpHandlers/DownloadLog.ashx">
                                <%= Resource.Admin_ImportXLS_DownloadImportLog%></a><br />
                            <a id="downloadFile" style="display: none" class='Link' href='../content/price_temp/<% = ExtStrFileName %> '>
                                <%= Resource.Admin_ExportExcel_DownloadFile%></a><br />
                            <a id="hlStart" style="display: none" class='Link' href="StatisticsProductsExportCsv.aspx">
                                <%= Resource.Admin_ExportCsv_ExportAgain%></a><br />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lError" runat="server" ForeColor="Red" Font-Bold="true" Visible="false"
                    EnableViewState="false"></asp:Label>
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
    <script>
        var temp = '<% =  SeparatorsEnum.Custom.StrName()%>';

        function Change() {
            if ($("#<% = ddlSeparetors.ClientID %> option:selected'").val() == temp) {
                $('#<% = txtCustomSeparator.ClientID %>').show();
            } else {
                $('#<% = txtCustomSeparator.ClientID %>').hide();
            }
        }

        $(document).ready(function () {
            Change();
            $("#<% = ddlSeparetors.ClientID %>").on('change', Change);
        });
    </script>
</asp:Content>
