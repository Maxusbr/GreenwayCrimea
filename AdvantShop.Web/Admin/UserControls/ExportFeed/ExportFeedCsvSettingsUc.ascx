<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.ExportFeedUc.ExportFeedCsvSettingsUc" CodeBehind="ExportFeedCsvSettingsUc.ascx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.ExportImport" %>
<%@ Import Namespace="Resources" %>

<script type="text/javascript">
    var temp = '<%= SeparatorsEnum.Custom.StrName()%>';

    function ChangeSeparator() {
        if ($("#<% = ddlSeparators.ClientID %> option:selected'").val() == temp) {
            $('#<% = txtCustomSeparator.ClientID %>').show();
        } else {
            $('#<% = txtCustomSeparator.ClientID %>').hide();
        }
    }

    $(document).ready(function () {
        ChangeSeparator();
        $("#<% = ddlSeparators.ClientID %>").on('change', ChangeSeparator);
    });
</script>

<div>
    <div id="divAction" runat="server" style="margin-bottom: 10px">
        <table class="export-settings" style="width: 100%;">
            <tr>
                <td style="width: 300px;">
                    <span><% = Resource.Admin_Csv_ExportNoInCategory%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:CheckBox runat="server" ID="chbCsvExportNoInCategory" />
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <span><%=Resource.Admin_ImportCsv_ChoseSeparator%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:DropDownList runat="server" ID="ddlSeparators" Style="display: inline-block" />
                        &nbsp;
                                <asp:TextBox ID="txtCustomSeparator" runat="server" class="niceTextBox shortTextBoxClass2"
                                    MaxLength="5" Style="display: none;"></asp:TextBox>
                    </span>

                </td>
            </tr>
            <tr>
                <td>
                    <span><%=Resource.Admin_Csv_ColumSeparator%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:TextBox ID="txtColumSeparator" runat="server" MaxLength="5" class="niceTextBox shortTextBoxClass2" Text=";"></asp:TextBox>
                    </span>

                </td>
            </tr>
            <tr>
                <td>
                    <span><%=Resource.Admin_Csv_PropertySeparator%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:TextBox ID="txtPropertySeparator" runat="server" MaxLength="5" class="niceTextBox shortTextBoxClass2" Text=":"></asp:TextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <span><%=Resource.Admin_ImportCsv_ChoseEncoding%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:DropDownList runat="server" ID="ddlEncoding" />
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <span><%=Resource.Admin_ExportCsv_ExportCategorySort%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:CheckBox runat="server" ID="ChbCategorySort" />
                    </span>
                </td>
            </tr>
        </table>
       <%-- <div class="dvSubHelp" id="CsvHelp" runat="server">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
            <a href="http://www.advantshop.net/help/pages/import-csv" target="_blank">Инструкция. Импорт и экспорт данных в формате CSV (Excel)</a>
        </div>--%>
    </div>
</div>

