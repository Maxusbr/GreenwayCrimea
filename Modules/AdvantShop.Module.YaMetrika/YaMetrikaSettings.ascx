<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.YaMetrika.Admin_YaMetrikaSettings" Codebehind="YaMetrikaSettings.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: YaMetrika_Header %>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: YaMetrika_CounterId%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtCounterId" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: YaMetrika_Counter%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtCounter" runat="server" Height="200px" Width="700px" TextMode="MultiLine" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize5" runat="server" Text="Отправлять заказы через событие 'Order'"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkOldApiEnabled" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize6" runat="server" Text="Использовать Ecommerce API"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkEcommerceApi" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize7" runat="server" Text="Собирать данные об ip-адресе посетителя"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkCollectIp" />
                <span style="color:gray">Необходимо добавить в код счетчика параметр - params: yaParams</span>
            </td>
        </tr>

        <tr>
            <td colspan="2" style="padding: 10px 0 0 0">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: YaMetrika_Events%>"></asp:Localize>
                <br />
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: YaMetrika_OrderEvent%>"></asp:Localize>
                
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: YaMetrika_Save%>" />
            </td>
        </tr>
    </table>
</div>