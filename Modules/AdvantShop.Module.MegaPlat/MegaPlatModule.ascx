<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.MegaPlat.Admin_MegaPlatModule" CodeBehind="MegaPlatModule.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">Настройки</span>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">ApiKey:
            </td>
            <td>
                <asp:Label runat="server" ID="lblApiKey" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">URL для проверки ApiKey:
            </td>
            <td>
                <asp:Label runat="server" ID="lblCheckApiKey" />
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="linkCheckApiKey" Text="Перейти..." />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">URL для выгрузки каталога:
            </td>
            <td>
                <asp:Label runat="server" ID="lblDownloadCatalog" />
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="linkDownloadCatalog" Text="Перейти..." />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">URL для выгрузки категорий:
            </td>
            <td>
                <asp:Label runat="server" ID="lblDownloadCategories" />
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="linkDownloadCategories" Text="Перейти..." />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">URL для загрузки каталога:
            </td>
            <td>
                <asp:Label runat="server" ID="lblUploadCatalog" />
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="linkUploadCatalog" Text="Перейти..." />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">URL для выгрузки заказов:
            </td>
            <td>
                <asp:Label runat="server" ID="lblDownloadOrders" />
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="linkDownloadOrders" Text="Перейти..." />
            </td>
        </tr>

    </table>
</div>
