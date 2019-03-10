<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.SubscribePopup.Admin_SubscribePopupSetting" CodeBehind="SubscribePopupSetting.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: SubscribePopup_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: SubscribePopup_NotifyAdmin%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbNotifyAdmin" runat="server" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: SubscribePopup_ShowOnMain%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowOnMain" runat="server" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: SubscribePopup_ShowInDetails%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowInDetails" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: SubscribePopup_OtherPages%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowInOtherPages" runat="server" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: SubscribePopup_DelayShowPopup%>"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList ID="ddlDelayShowPopup" runat="server">
                    <asp:ListItem Text="<%$ Resources: SubscribePopup_TimeSpan_Now%>" Value="10" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: SubscribePopup_TimeSpan_ThirtySeconds%>" Value="11"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: SubscribePopup_TimeSpan_OneMinute%>" Value="7"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: SubscribePopup_TimeSpan_FiveMinute%>" Value="8"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: SubscribePopup_TimeSpan_TenMinute%>" Value="9"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: SubscribePopup_TimeSpan%>"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList ID="ddlSubscribePopupTimeSpan" runat="server">
                    <asp:ListItem Text="Один раз" Value="0" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="Раз в месяц" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Раз в неделю" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Раз в день" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Каждые 6 часов" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Раз в час" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Каждые 30 минут" Value="6"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: SubscribePopup_TimeSpan_EveryTime%>" Value="12"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px;">&nbsp;</td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: SubscribePopup_Title%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtPopupTitle" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px;">&nbsp;</td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;" colspan="2">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: SubscribePopup_TopHtml%>"></asp:Localize>
                <br />
                <br />
                 <asp:TextBox ID="txtSubscribePopupTopHtml" TextMode="MultiLine" runat="server" Height="300px" Width="700px" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px;">&nbsp;</td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;" colspan="2">
                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources: SubscribePopup_BottomHtml%>"></asp:Localize>
                <br />
                <br />
                 <asp:TextBox ID="txtSubscribePopupBottomHtml" TextMode="MultiLine" runat="server"  Height="300px" Width="700px" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px;">&nbsp;</td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;" colspan="2">
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: SubscribePopup_FinalHtml%>"></asp:Localize>
                <br />
                <br />
                <asp:TextBox ID="txtSubscribePopupFinalHtml" TextMode="MultiLine" runat="server" Height="300px" Width="700px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: SubscribePopup_Save%>" />
            </td>
        </tr>
    </table>
</div>
