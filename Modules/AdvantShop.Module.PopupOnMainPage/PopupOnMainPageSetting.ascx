<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.PopupOnMainPage.Admin_PopupOnMainPageSetting" CodeBehind="PopupOnMainPageSetting.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: PopupOnMainPage_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: PopupOnMainPage_ShowOnMain%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowOnMain" runat="server" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: PopupOnMainPage_ShowInDetails%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowInDetails" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: PopupOnMainPage_OtherPages%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowInOtherPages" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources: PopupOnMainPage_BlocksBackground%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbBlocksBackground" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: PopupOnMainPage_ShowInMobile%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowInMobile" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: PopupOnMainPage_DelayShowPopup%>"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList ID="ddlDelayShowPopup" runat="server">
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_Now%>" Value="8" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_ThirtySeconds%>" Value="9"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_OneMinute%>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_FiveMinute%>" Value="7"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_TenMinute%>" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: PopupOnMainPage_TimeSpan%>"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList ID="ddlPopupOnMainPageTimeSpan" runat="server">
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_OneTime%>" Value="0" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_TenMinute%>" Value="2"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_HalfAnHour%>" Value="3"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_OneDay%>" Value="4"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_OneMounth%>" Value="5"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_Never%>" Value="6"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: PopupOnMainPage_TimeSpan_EveryTime%>" Value="10"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: PopupOnMainPage_Title%>"></asp:Localize>
            </td>
            <td>
                 <asp:TextBox ID="txtPopupOnMainPageTitle" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="100%" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px;">&nbsp;</td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;" colspan="2">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: PopupOnMainPage_Html%>"></asp:Localize>
                <br />
                <br />
                 <asp:TextBox ID="txtPopupOnMainPageHtml" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="100%" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: PopupOnMainPage_Save%>" />
            </td>
        </tr>
    </table>
</div>
