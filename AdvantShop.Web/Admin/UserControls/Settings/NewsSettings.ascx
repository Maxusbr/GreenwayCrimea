<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.NewsSettings" CodeBehind="NewsSettings.ascx.cs" %>
<table class="info-tb" style="width: 550px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">Новости
            </span>
            <br />
            <span class="subTitleNotify">Прочие настройки для новостей
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadNewsOther%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= txtMainPageText.ClientID %>"><%= Resources.Resource.Admin_SettingsNews_MainPageText%></label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtMainPageText" class="niceTextBox textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 180px;">
            <label class="form-lbl" for="<%= txtNewsPerPage.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_NewsPerPage%></label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtNewsPerPage" class="niceTextBox shortTextBoxClass3"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtNewsMainPageCount.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_MainPageNews%></label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtNewsMainPageCount" class="niceTextBox shortTextBoxClass3"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtNewsMainPageCount.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_RssViewNews%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkViewRssNews" runat="server"/>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">SEO cтраницы новостей 
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_PageTitle%>
        </td>
        <td>
            <asp:TextBox ID="txtNewsHeadTitle" runat="server" CssClass="niceTextBox textBoxClass" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>H1
        </td>
        <td>
            <asp:TextBox ID="txtNewsH1" runat="server" CssClass="niceTextBox textBoxClass" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaKeywords%>
        </td>
        <td>
            <asp:TextBox ID="txtNewsMetaKeywords" runat="server" CssClass="niceTextArea textArea2Lines" TextMode="MultiLine" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_m_Default_MetaDescription%>
        </td>
        <td>
            <asp:TextBox ID="txtNewsMetaDescription" runat="server" CssClass="niceTextArea textArea2Lines" TextMode="MultiLine" /><br />
        </td>
    </tr>

</table>
<span class="subSaveNotify">Чтобы сохранить изменения, нажмите оранжевую кнопку "Сохранить" вверху.
</span>