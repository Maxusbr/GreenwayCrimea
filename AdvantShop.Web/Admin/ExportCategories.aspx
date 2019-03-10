<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.ExportCategories" CodeBehind="ExportCategories.aspx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.ExportImport" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        function confirmDelete() {
            return confirm(localize("Admin_ExportFeed_ConfirmDelete"));
        }
        function ChangeBtState() {
            $(".export-tb select").each(function (index) {
                $(this).val(index + 1);
            });
        }
    </script>
    <style>
        .exp-c-sts td {
            padding: 3px 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;
        </div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #0D76B8; text-align: center;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Catalog.aspx">
                <%= Resource.Admin_MasterPageAdmin_CategoryAndProducts %></a> </li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="ProductsOnMain.aspx?type=New"
                class="neighbor-menu-lnk">
                <%= Resource.Admin_MasterPageAdminCatalog_FirstPageProducts%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=Best">
                            <%= Resource.Admin_MasterPageAdminCatalog_BestSellers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=New">
                            <%= Resource.Admin_MasterPageAdminCatalog_New %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=Sale">
                            <%= Resource.Admin_MasterPageAdminCatalog_Discount %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductLists.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ProductLists %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Properties.aspx">
                <%= Resource.Admin_MasterPageAdmin_Directory%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Properties.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ProductProperties%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Colors.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ColorDictionary%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Sizes.aspx">
                            <%= Resource.Admin_MasterPageAdmin_SizeDictionary%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Tags.aspx">
                            <%= Resource.Admin_Tags_Header %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item selected"><a href="ExportFeed.aspx">
                <%= Resource.Admin_MasterPageAdmin_Export%></a></li>
            <li class="neighbor-menu-item"><a href="ImportCSV.aspx">
                <%= Resource.Admin_MasterPageAdmin_Import%></a></li>
            <li class="neighbor-menu-item"><a href="Brands.aspx">
                <%= Resource.Admin_MasterPageAdmin_Brands%></a></li>
            <li class="neighbor-menu-item"><a href="Reviews.aspx">
                <%= Resource.Admin_MasterPageAdmin_Reviews%></a></li>
        </menu>
    </div>
    
    <div style="padding: 0 10px;">
        <h2 class="products-header">
            <%= Resource.Admin_ExportCategories_ExportCategories%>
        </h2>
        
        <div id="divAction" runat="server" style="margin:10px 0">
            <div style="font-weight:bold; margin-bottom:5px; font-size:14px;">
                <%= Resource.Admin_ExportCategories_ExportParams%>
            </div>
            <table class="exp-c-sts">
                <tr>
                    <td>
                        <span><%=Resource.Admin_ImportCsv_ChoseSeparator%>:</span>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSeparators" style="display: inline-block" />
                        &nbsp;
                        <asp:TextBox ID="txtCustomSeparator" runat="server" class="niceTextBox shortTextBoxClass2"
                            MaxLength="5" Style="display:none;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span><%=Resource.Admin_ImportCsv_ChoseEncoding%>:</span>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlEncoding" />
                    </td>
                </tr>
            </table>
            <div style="font-weight:bold; margin:15px 0 0 0; font-size:14px;">
                <%=Resource.Admin_ExportCsv_ChoseFields%> 
                <input type="button" class="btn btn-middle btn-action" value="Установить по умолчанию" onclick="ChangeBtState('select')" style="margin: 0 0 0 20px">
            </div>
        </div>
        <div id="choseDiv" runat="server" class="overflow" style="width: 600px;">
        </div>
        <div id="OutDiv" runat="server" visible="false" style="width: 610px; margin: auto;">
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
                        <%=Resource.Admin_ImportXLS_ProductsWithError %> : <span id="errorBlock" class=""></span>
                        <br/>
                        <%=Resource.Admin_CommonStatictic_CurrentProcess%> : <a id="lCurrentProcess"></a>
                    </td>
                </tr>
            </table>
            <div id="divScript" runat="server" style="width: 650px">
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
        
            
        <div id="divbtnAction" runat="server" style="margin-top:25px; margin-left:25px;">
            <table>
                <tr>
                    <td style="width:200px;">
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
                    <a id="downloadFile" style="display: none" class='Link' href='../content/price_temp/<%= fileName %>.csv'>
                        <%= Resource.Admin_ExportExcel_DownloadFile%></a><br />
                    <a id="hlStart" style="display: none" class='Link' href="ExportCategories.aspx">
                        <%= Resource.Admin_ExportCsv_ExportAgain%></a><br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lError" runat="server" ForeColor="Red" Font-Bold="true" Visible="false" EnableViewState="false"></asp:Label>
        
        <asp:DropDownList runat="server" ID="ddlProduct" Style="display: none" />
        <script>
            var temp = '<%= SeparatorsEnum.Custom.StrName()%>';

            function Change() {
                if ($("#<%= ddlSeparators.ClientID %> option:selected'").val() == temp) {
                    $('#<%= txtCustomSeparator.ClientID %>').show();
                } else {
                    $('#<%= txtCustomSeparator.ClientID %>').hide();
                }
            }

            $(document).ready(function () {
                Change();
                $("#<%= ddlSeparators.ClientID %>").on('change', Change);
            });
        </script>
    </div>

</asp:Content>
