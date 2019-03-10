<%@ Page AutoEventWireup="true" CodeBehind="TemplateSettings.aspx.cs" Inherits="Admin.TemplateSettings" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="DesignConstructor.aspx">
                <%= Resource.Admin_MasterPageAdmin_DesignConstructor%></a></li>
            <li class="neighbor-menu-item selected"><a href="TemplateSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_TemplateSettings%></a></li>
            <li class="neighbor-menu-item"><a href="StylesEditor.aspx">
                <%= Resource.Admin_MasterPageAdmin_StylesEditor%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div style="text-align: center;">
            <asp:Label ID="lblCustomer" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_TemplateSettings_Title %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_TemplateSettings_SubHeader %>"></asp:Label>&nbsp;
            <br />
            <br />
        </div>
        <div style="text-align: center;">
            <asp:Label ID="lblInfo" runat="server" ForeColor="Blue" CssClass="tpl-settings-result" />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            <br />
        </div>
        <div style="width:520px; margin: 10px auto;">
            <asp:Panel ID="pnlDesignSettings" runat="server">
            </asp:Panel>

            <div style="padding:0 0 10px 0">
                <asp:Label ID="lblInfo1" runat="server" ForeColor="Blue" CssClass="tpl-settings-result" />
            </div>
            <div style="text-align:center; margin-top:10px;">
                <asp:HyperLink ID="btnSave" runat="server" CssClass="tpl-save-btn btn btn-middle btn-add" 
                    Text="<%$ Resources:Resource, Admin_TemplateSettings_SaveSettings %>" NavigateUrl="javascript:void(0)" />
                
                <asp:LinkButton ID="btnSetDefaultSettings" runat="server" CssClass="btn btn-middle btn-action" 
                    Text="<%$ Resources:Resource, Admin_TemplateSettings_SetDefaultSettings %>" OnClick="btnSetDefaultSettings_Click" />
            </div>
            <div>
                <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
                    <div class="progressDiv">
                        <div class="progressbarDiv" id="textBlock">
                        </div>
                        <div id="InDiv" class="progressInDiv">
                            &nbsp;
                        </div>
                    </div>
                    <div id="Div4">
                        <%=Resource. Admin_CommonStatictic_CurrentProcess%>: <a id="lCurrentProcess"></a>
                    </div>
                    <br />
                    <div id="divScript" runat="server">
                        <script type="text/javascript">
                            var _timerId = -1;
                            var _stopLinkId = "#<%= linkCancel.ClientID %>";

                            $(document).ready(function () {
                                $.fjTimer({
                                    interval: 500,
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
                                                $("#lCurrentProcess").html(data.CurrentProcessName);
                                                $("#lCurrentProcess").attr("href", data.CurrentProcess);

                                                if (data.Processed == data.Total) {
                                                    stopTimer();
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
                                $(_stopLinkId).hide();
                            }
                        </script>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:LinkButton ID="linkCancel" runat="server" 
                            Text="<%$ Resources:Resource, Admin_ResizePhoto_Cancel%>" OnClick="linkCancel_Click"></asp:LinkButton><br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="lError" runat="server" ForeColor="Blue" Font-Bold="true" Visible="false" EnableViewState="false"></asp:Label>            
            </div>
            <br />
            <div class="tpl-settings-row">
		        <div class="tpl-setting-section">
			        <span><%=Resource.Admin_ResizePhoto_Resize%></span>
		        </div>
		        <asp:Button ID="btnResizePhoto" runat="server" CssClass="btn btn-middle btn-action"
                    Text="<%$ Resources:Resource, Admin_ResizePhoto_Resize %>" OnClick="btnResizePhoto_Click"/>
	        </div>
        </div>
    </div>
</asp:Content>
