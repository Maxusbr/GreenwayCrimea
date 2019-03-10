<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.ExportFeedUc.ExportFeedCsvFieldsSettingsTab" CodeBehind="ExportFeedCsvFieldsSettingsTab.ascx.cs" %>

<%@ Import Namespace="Resources" %>

<script type="text/javascript">
    function Change(obj) {
        var tdCurrent = $(obj).closest("td");
        tdCurrent.next("td").find("span").text($("<% = '#' + ddlProduct.ClientID  %> [value='" + $(obj).val() + "']").text());
    }

    function ChangeState(obj) {
        window.location = 'exportfeed.aspx?feedid=' + <%= ExportFeedId %> +'&state=' + $(obj).val() + '#tabid=tab-csvfields';
    }
    function ChangeBtState(obj) {
        window.location = 'exportfeed.aspx?feedid=' + <%= ExportFeedId %> + '&state=' + obj + '&<% = CategorySort %>=' + '<% =CsvExportFeedSettings!= null && CsvExportFeedSettings.CsvCategorySort %>#tabid=tab-csvfields';
    }


</script>

<div>
    <div>
        <span class="spanSettCategory"><%= Resources.Resource.Admin_ExportCsv_ChoseFields %>
        </span>
        <asp:Label ID="saveSuccess" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_SettingsSaved %>' Visible="False" Style="float: right; color: blue;" />

        <%--<hr color="#C2C2C4" size="1px" />--%>
    </div>
    <div style="height: 45px;">
        &nbsp;<input type="button" class="btn btn-middle btn-add"
            value="<%= Resource.Admin_ExportCsv_Select %>" onclick="ChangeBtState('select')" style="float: right;" /><input type="button" class="btn btn-middle btn-action" style="margin-right: 5px; float: right;"
            value="<%=  Resource.Admin_ExportCsv_Deselect %>" onclick="ChangeBtState('deselect')" />

    </div>
    <div id="choseDiv" runat="server" class="overflow" EnableViewState="True" ViewStateMode="Enabled">
    </div>
</div>
<asp:DropDownList runat="server" ID="ddlProduct" Style="display: none" />

<asp:LinkButton ID="btnSave" runat="server" 
                CssClass="btn btn-middle btn-add" 
                OnClick="btnSave_Click" 
                Style="margin-top: 10px; margin-right: 10px;" 
                Text="<%$Resources:Resource,Admin_Update %>" />

<%--<div class="dvSubHelp" id="CsvHelp" runat="server">
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/import-csv" target="_blank">Инструкция. Импорт и экспорт данных в формате CSV (Excel)</a>
</div>--%>
