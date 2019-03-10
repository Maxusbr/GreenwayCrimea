<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.ExportFeedUc.ExportFeedSettingsTab" CodeBehind="ExportFeedSettingsTab.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Scheduler" %>

<script>

    var type = "<%=TimeIntervalType.Days.ToString() %>";
    function ChangeDL(obj, tr) {
        if ($(obj).val() == type) {
            $('#' + tr).show();
        }
        else {
            $('#' + tr).hide();
        }
    }

    $(function () {

        ChangeDL('#<% = ddlIntervalType.ClientID  %>', 'trStartTime');
    });

</script>
<table style="width: 100%;" class="export-settings">
    <%--class="info-tb"--%>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory"><%= Resources.Resource.Admin_ExportFeed_MainSettings %>
            </span>
            <%--<asp:Label ID="saveSuccess" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_SettingsSaved %>' Visible="False" Style="float: right; color: blue;" />--%>
            <asp:Label ID="lblSaveMessage" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_WrongData %>' Visible="False" Style="float: right;" />
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 300px;">
            <asp:Literal ID="Literal13" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_Name %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtExportFeedName" runat="server" CssClass="niceTextBox textBoxClass"></asp:TextBox>
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal14" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_Description %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtExportFeedDescription" runat="server" CssClass="niceTextArea textArea3Lines" TextMode="MultiLine"></asp:TextBox>
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal15" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_JobActive %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox ID="ckbExportFeedActive" runat="server" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <label class="form-lbl" for="<%= txtTimeInterval.ClientID %>" class="Label"><%= Resources.Resource.Admin_CommonSettings_StartInterval%></label>
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox runat="server" ID="txtTimeInterval" CssClass="niceTextBox shortTextBoxClass3" Text="1" />&nbsp;
                                <asp:DropDownList runat="server" ID="ddlIntervalType" onchange="javascript:ChangeDL(this,'trStartTime')"></asp:DropDownList>
            </span>
        </td>
    </tr>
    <tr id="trStartTime" class="rowsPost">
        <td>
            <label class="form-lbl" for="<%= txtHours.ClientID %>" class="Label"><%= Resources.Resource.Admin_CommonSettings_StartTime%></label>
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox runat="server" ID="txtHours" CssClass="niceTextBox shortTextBoxClass3" Text="0" /><span class="paramUnit"><%= Resources.Resource.Admin_CommonSettings_Hours%></span>&nbsp;
                                <asp:TextBox runat="server" ID="txtMinutes" CssClass="niceTextBox shortTextBoxClass3" Text="0" /><span class="paramUnit"><%= Resources.Resource.Admin_CommonSettings_Minutes%></span>
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedFileName %>' /><br />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:Label ID="lShopUrl" runat="Server" Style="font-size: 14px;" />&nbsp;<asp:TextBox Width="150px" ID="txtFileName" runat="Server" CssClass="niceTextBox textBoxClass" />.<asp:Literal
                    ID="FileNameExtLiteral" runat="Server" Visible="False" />
                <asp:DropDownList ID="ddlFileExtention" runat="server" />&nbsp;<asp:Label ID="lblFileNameMessage" runat="server" Visible="false" Text="Имя файла занято" ForeColor="Red"></asp:Label>
                <span class="warning">
                    <br />
                    <asp:Literal ID="Literal4" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_Note %>' />
                    <asp:Literal ID="ltrlFileName" runat="Server" />. </span>
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_Extracharge %>'></asp:Literal>
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtPriceMargin" runat="server" CssClass="niceTextBox shortTextBoxClass2"></asp:TextBox>
                <asp:RangeValidator ID="rvPriceMargin" runat="server" ControlToValidate="txtPriceMargin"
                    ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                    MaximumValue="100000" MinimumValue="-100000" Type="Double"> </asp:RangeValidator>
            </span>
        </td>
    </tr>
   <%-- <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal11" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_ExportNotAmount %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="ckbExportNotAmount" CssClass="checkly-align" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal12" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_ExportNotActive %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="ckbExportNotActive" CssClass="checkly-align" />
            </span>
        </td>
    </tr>--%>
</table>

<asp:Panel ID="pnlAdditionalSettings" runat="server"></asp:Panel>
<asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-middle btn-add" OnClick="btnSave_Click" Style="margin-top: 10px; margin-right: 10px;" Text="<%$Resources:Resource,Admin_Update %>"></asp:LinkButton>
