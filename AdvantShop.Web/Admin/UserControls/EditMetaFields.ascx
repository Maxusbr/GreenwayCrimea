<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EditMetaFields.ascx.cs" Inherits="Admin.UserControls.EditMetaFields" %>
<table border="0" cellpadding="2" cellspacing="0" class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_HeadSeo%>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td>
            <span><%=Resources.Resource.Admin_MetaTitle%></span>
        </td>
        <td>
            <asp:TextBox ID="txtPageTitle" runat="server" CssClass="niceTextBox textBoxClass" />
        </td>
    </tr>
    <tr>
        <td>
            <span>H1</span>
        </td>
        <td>
            <asp:TextBox ID="txtH1" runat="server" CssClass="niceTextBox textBoxClass" />
        </td>
    </tr>
    <tr>
        <td>
            <span><%=Resources.Resource.Admin_MetaKeyWords%></span>
        </td>
        <td>
            <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" />
        </td>
    </tr>
    <tr>
        <td>
            <span><%=Resources.Resource.Admin_MetaDescription%></span>
        </td>
        <td>
            <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" />
        </td>
    </tr>
    <tr>
        <td></td>
        <td valign="top" width="400">
            <asp:Label ID="lblHint" Text="<%$ Resources: Resource, Admin_UseGlobalVariables %>" runat="server" CssClass="info-hint-text" />
        </td>
    </tr>
</table>
