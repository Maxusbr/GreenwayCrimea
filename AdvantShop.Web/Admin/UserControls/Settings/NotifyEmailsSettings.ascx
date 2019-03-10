<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.NotifyEmailsSettings" CodeBehind="NotifyEmailsSettings.ascx.cs" %>
<table class="info-tb" style="width: 650px;">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_NotifyEmails%>
            </span>
            <br />
            <span class="subTitleNotify">
                <%= Resources.Resource.Admin_CommonSettings_EmailTitleNotify%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width:230px;">
            <label class="form-lbl" for="<%= txtEmailRegReport.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EmailForReports%></label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailRegReport" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOrderEmail.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EmailForOrders%></label>
        </td>
        <td>
            <asp:TextBox ID="txtOrderEmail" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtEmailProductDiscuss.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EmailForComment%></label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailProductDiscuss" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtFeedbackEmail.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EmailForFeedBack%></label>
        </td>
        <td>
            <asp:TextBox ID="txtFeedbackEmail" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
      <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtFeedbackEmail.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EmailForLead%></label>
        </td>
        <td>
            <asp:TextBox ID="txtLeadEmail" runat="server" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
</table>
<span class="subSaveNotify"><%= Resources.Resource.Admin_CommonSettings_EmailExample%>
</span>
<br />
<table class="info-tb" style="width: 650px;">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_FreshdeskIntegration%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <label class="form-lbl" for="<%= txtFreshdeskDomain.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_FreshdeskDomain%></label>
        </td>
        <td>https:// <asp:TextBox ID="txtFreshdeskDomain" runat="server" class="textBoxClass" placeholder="<%$ Resources:Resource, Admin_CommonSettings_FreshdeskDomainPlaceHolder %>" Width="350px"></asp:TextBox>
        </td>
    </tr>
    
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <label class="form-lbl" for="<%= txtFreshdeskWidgetCode.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_FreshdeskWidgetCode%></label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtFreshdeskWidgetCode" TextMode="MultiLine" ReadOnly="true" Width="390px" Height="100px" />
        </td>
    </tr>
</table>
