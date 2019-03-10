<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.ProfitSettings" Codebehind="ProfitSettings.ascx.cs" %>
<table class="info-tb" style="width:400px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadProfitability%>
            </span>
            <br />
            <span class="subTitleNotify">
                Настройки для графика плана продаж
            </span>
            <hr style="color: #C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width:180px;">
            <label class="form-lbl" for="<%= txtSalesPlan.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_SalesPlan%></label>
        </td>
        <td>
            <asp:TextBox ID="txtSalesPlan" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtProfitPlan.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ProfitPlan%></label>
        </td>
        <td>
            <asp:TextBox ID="txtProfitPlan" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
        </td>
    </tr>
</table>
<span class="subSaveNotify">
    Значения указываются в валюте магазина. Например: 100 000<br />
    <br />
    Чтобы сохранить изменения, нажмите оранжевую кнопку "Сохранить" вверху.
</span>
