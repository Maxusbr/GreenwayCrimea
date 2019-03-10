<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Settings.LicSettings" Codebehind="LicSettings.ascx.cs" %>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_UserControls_Settings_LicSettings_header%>
            </span>
            <br />
            <span class="subTitleNotify">
                Здесь вы можете проверить и указать лицензионный ключ.<br />
                Обратите внимание, что смена ключа повлечёт смену настроек, не меняйте ключ без надобности.
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <%= Resources.Resource.Admin_UserControls_Settings_LicSettings_LicStatus%>
        </td>
        <td>
            <asp:Label runat="server" ID="lblState"></asp:Label>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtLicKey.ClientID %>"><%= Resources.Resource.Admin_UserControls_Settings_LicSettings_LicKey%></label>
        </td>
        <td>
            <asp:TextBox ID="txtLicKey" runat="server" CssClass="niceTextBox textBoxClass" Text="" />
            <asp:Button runat="server" ID="btnCheckLic" OnClick="btnCheckLic_Click" Text="<%$ Resources:Resource, Admin_UserControls_Settings_LicSettings_Check %>" />
        </td>
    </tr>
</table>
