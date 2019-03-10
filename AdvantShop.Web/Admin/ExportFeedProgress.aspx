<%@ Page Title="ExportFeed" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.ExportFeedProgress" CodeBehind="ExportFeedProgress.aspx.cs" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
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
            <li class="neighbor-menu-item"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>

    <div style="margin-left: 20px; margin-right: 20px">
        <div class="pageHeader">
            <span class="AdminHead">
                <asp:Literal ID="ltrlHead" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_PageHeader %>' /></span>
            <span id="PageSubheader" visible="false" runat="Server">
                <br />
                <span class="AdminSubHead">
                    <asp:Literal ID="ltrlSubHead" runat="Server" /></span>
                <br />
                <br />
            </span>
        </div>
        <br />
        <div class="ui-tabs">
            <div id="tabs-1" class="ui-tabs-panel" style="text-align: center;">
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
                    <script type="text/javascript">
                        $(function () {

                            window.progressbar($("#InDiv"), 'HttpHandlers/CommonStatisticData.ashx', null, function (obj, data) {
                                var total = data.Total;
                                var current = data.Processed;

                                $(".progressDiv").show();
                                $("#linkDiv").hide();

                                var processed;
                                if (total != 0) {
                                    processed = Math.round(current / total * 100);
                                } else {
                                    processed = 0;
                                }

                                if (!data.IsRun)
                                    processed = 100;

                                $("#textBlock").html(processed + "% (" + current + "/" + total + ")");
                                obj.css("width", processed + "%");

                                $("#lCurrentProcess").html(data.CurrentProcessName);
                                $("#lCurrentProcess").attr("href", data.CurrentProcess);
                                
                                if (!data.IsRun) {

                                    if (total > 0) {
                                        
                                        $("#exportLink").html("Скачать файл");

                                        var filename = data.FileName.indexOf('?') != -1 ? data.FileName : data.FileName + "?rnd=" + Math.random();

                                        $("#exportLink").attr('href', filename);
                                        $("#linkDiv").show();
                                    }
                                    return true; // finish progressbar
                                }
                                else {
                                    return false; //process progressbar
                                }
                            });
                        });
                    </script>
                </div>
                <br>
                <div id="linkDiv" style="display: none">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ExportFeed_SuccessfulExport %>" />.
                        <br>
                    <br>
                    <a class="btn btn-middle btn-add" id="exportLink" target="blank" href="javascript:void(0)"></a>
                    <br>
                    <br>
                </div>
                <a href="ExportFeed.aspx?feedid=<% = ExportFeedId %>"><%=  Resource.Admin_ExportFeed_Back%></a>
                <br>
            </div>
        </div>
    </div>

</asp:Content>
