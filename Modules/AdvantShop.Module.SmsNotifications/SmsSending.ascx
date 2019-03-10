<%@ Control Language="C#" AutoEventWireup="true" Inherits="Advantshop.Modules.UserControls.Modules_SmsNotifications_SmsSending" Codebehind="SmsSending.ascx.cs" %>
<table style="width: 100%;">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: SmsNotifications_Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="padding-left: 30px;" />
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 100px;">
            <asp:Localize runat="server" Text="<%$ Resources: SmsNotifications_Recipient%>" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlRecipientType" onchange="RecipientTypeChange();" Style="display: inline-block;">
                <Items>
                    <asp:ListItem Value="Subscriber" Text="<%$ Resources: SmsNotifications_Recipient_Subscriber%>" />
                    <asp:ListItem Value="Customer" Text="<%$ Resources: SmsNotifications_Recipient_Customer%>" />
                    <asp:ListItem Value="OrderCustomer" Text="<%$ Resources: SmsNotifications_Recipient_OrderCustomer%>" />
                    <asp:ListItem Value="All" Text="<%$ Resources: SmsNotifications_Recipient_All%>" />
                    <asp:ListItem Value="One" Text="<%$ Resources: SmsNotifications_Recipient_One%>" />
                </Items>
            </asp:DropDownList>&nbsp;
            <div style="display: none;">
                +<asp:TextBox runat="server" ID="txtPhone" Width="120px" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2">
            <asp:Localize ID="Localize42" runat="server" Text="<%$ Resources: SmsNotifications_Message%>"></asp:Localize>
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2">
            <asp:TextBox runat="server" ID="txtMessage" Width="470px" Height="100px" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" style="height: 30px; width: 150px;" Text="<%$ Resources: SmsNotifications_Send%>" />
        </td>
        <td style="padding: 5px 0 0 10px;">
            <asp:UpdatePanel ID="upPhonesList" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:PostBackTrigger ControlID="lnkPhonesList" />
                </Triggers>
                <ContentTemplate>
                    <asp:LinkButton runat="server" ID="lnkPhonesList" Text="Список номеров" OnClick="lnkPhonesList_Click"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<script type="text/javascript">
    $(function() {
        RecipientTypeChange();
    });

    function RecipientTypeChange() {
        if ($("#<%= ddlRecipientType.ClientID %> :selected").val() == "One") {
            var txtPhone = $("#<%= txtPhone.ClientID%>");
            txtPhone.closest("div").css("display", "inline-block");
            if (txtPhone.val().length)
                $("#<%= txtMessage.ClientID%>").focus();
            else
                txtPhone.focus();
        } else
            $("#<%= txtPhone.ClientID%>").closest("div").css("display", "none");
    }
</script>
