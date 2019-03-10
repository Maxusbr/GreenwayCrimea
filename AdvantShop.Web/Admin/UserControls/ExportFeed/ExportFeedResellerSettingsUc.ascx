<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.ExportFeedUc.ExportFeedResellerSettingsUc" CodeBehind="ExportFeedResellerSettingsUc.ascx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.ExportImport" %>
<%@ Import Namespace="Resources" %>

<div>
    <div id="divAction" runat="server" style="margin-bottom: 10px">
        <table style="width: 100%;" class="export-settings">
            <tr>
                <td style="width: 300px;">
                    <span><%=Resource.Admin_Resellers_ID%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:Label ID="lblResellerCode" runat="server" MaxLength="5" Text=""></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <span><%=Resource.Admin_ImportCsv_RecommendedPriceMargin%>:</span>
                </td>
                <td>
                    <span class="parametrValueString">
                        <asp:TextBox ID="txtRecommendedPriceMargin" runat="server" class="niceTextBox shortTextBoxClass2" MaxLength="5"></asp:TextBox>
                        <asp:RangeValidator ID="rvRecommendedPriceMargin" runat="server" ControlToValidate="txtRecommendedPriceMargin"
                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                            MaximumValue="100000" MinimumValue="-100000" Type="Double"> </asp:RangeValidator>
                    </span>
                </td>
            </tr>
        </table>
    </div>   
</div>

