<%@ Control Language="C#" AutoEventWireup="true" Inherits="Advantshop.Modules.UserControls.FindCheaper.Admin_FindCheaperSettings" CodeBehind="FindCheaperSettings.ascx.cs" %>
<style>
    .reviewsTable {
        border-collapse: collapse;
        width: 100%;
    }

        .reviewsTable td, .reviewsTable th {
            border-bottom: 1px solid #000000;
            height: 30px;
            text-align: left;
        }
</style>
<div>
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Label ID="Localize9" runat="server" Text="Настройки модуля" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize2" runat="server" Text="Заголовок окна"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="Текст под заголовком"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtTopText" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize3" runat="server" Text="Финальный текст"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtFinalText" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize4" runat="server" Text="Получатель письма о запросе"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtEmailTo" Width="300px"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Сохранить" />
            </td>
        </tr>
    </table>

</div>
