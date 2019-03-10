<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.AdwordsRemarketing.Admin_AdwordsRemarketingSettings" CodeBehind="AdwordsRemarketingSettings.ascx.cs" %>
<div style="text-align: center;" class="info-tb">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">AdWords Remarketing</span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 150px; text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="Индентификатор транзакций"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtConversionId" runat="server" Width="200px" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 150px; text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="Использовать особый тип"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkUseDynx" />
                <div data-plugin="help" class="help-block">
                    <div class="help-icon js-help-icon"></div>
                    <article class="bubble help js-help">
                        <header class="help-header">
                            Использовать особый тип
                        </header>
                        <div class="help-content text-floating">
                            Подробнее о специальных параметрах можно прочитать здесь <a target="_blank" href="https://support.google.com/adwords/answer/3103357?hl=ru#custom_tag">https://support.google.com/adwords/answer/3103357?hl=ru#custom_tag</a>
                        </div>
                    </article>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Сохранить" />
            </td>
        </tr>
    </table>
</div>
