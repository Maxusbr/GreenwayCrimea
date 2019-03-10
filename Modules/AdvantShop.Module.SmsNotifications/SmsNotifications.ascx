<%@ Control Language="C#" AutoEventWireup="true" Inherits="Advantshop.Modules.UserControls.Modules_SmsNotifications_SmsNotifications" CodeBehind="SmsNotifications.ascx.cs" %>

<%@ Register TagPrefix="adv" Namespace="AdvantShop.Core.Controls" Assembly="AdvantShop.Core" %>
<script type="text/javascript">
    $(document).ready(function () {
        initgrid();
    });
</script>
<div style="padding: 0px 10px;">
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: SmsNotifications_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <asp:Label runat="server" ID="lblErr" Visible="False" ForeColor="Red" EnableViewState="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize15" runat="server" Text="<%$ Resources: SmsNotifications_SmsService%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList runat="server" ID="ddlSmsService" Width="220" onchange="ServiceSettings();">
                    <asp:ListItem Text="unisender.com" Value="WwwUnisenderCom"></asp:ListItem>
                    <asp:ListItem Text="epochta.ru" Value="WwwEpochtaRu"></asp:ListItem>
                    <asp:ListItem Text="Stream-Telecom.ru" Value="StreamTelecom"></asp:ListItem>
                    <asp:ListItem Text="sms4b.ru" Value="WwwSms4BRu"></asp:ListItem>
                    <asp:ListItem Text="smslab.ru" Value="SmslabRu"></asp:ListItem>
                    <asp:ListItem Text="smsimple.ru" Value="WwwSmsimpleRu"></asp:ListItem>
                    <asp:ListItem Text="gsm-inform.ru" Value="GsmInformRu"></asp:ListItem>
                    <asp:ListItem Text="iqsms.ru" Value="WwwIqsmsRu"></asp:ListItem>
                    <asp:ListItem Text="smspilot.ru" Value="WwwSmspilotRu"></asp:ListItem>
                    <asp:ListItem Text="sendex.ru (sms-online.com)" Value="RuSmsOnlineCom"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td></td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="WwwEpochtaRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;"></td>
            <td style="text-align: left; vertical-align: top;">
                <a href="https://www.epochta.ru/products/sms/register.php#a_aid=advantshop" target="_blank">Зарегистрироваться</a>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize45" runat="server" Text="<%$ Resources: SmsNotifications_PublicKey%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwEpochtaRuApiKey" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize47" runat="server" Text="<%$ Resources: SmsNotifications_PrivateKey%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwEpochtaRuPrivatKey" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize46" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwEpochtaRuSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="WwwUnisenderCom" class="servicesettings">
        <tr class="rowsPost">
            <td colspan="2">
                <hr color="#C2C2C4" size="1px" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">Новый аккаунт
            </td>
            <td style="text-align: left; vertical-align: top;">

                <table style="margin-top: 20px">
                    <tr class="rowsPost">
                        <td style="text-align: left; width: 250px;">E-mail
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox runat="server" ID="txtUniSenderEmail" Width="220"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="rowsPost">
                        <td style="text-align: left; width: 250px;">Логин
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox runat="server" ID="txtUnisenderLogin" Width="220"></asp:TextBox>
                        </td>
                    </tr>

                    <tr class="rowsPost">
                        <td style="text-align: left; width: 250px;">Пароль
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox runat="server" ID="txtUnisenderPassword" Width="220"></asp:TextBox>
                        </td>
                    </tr>

                    <tr class="rowsPost">
                        <td style="text-align: left; width: 250px;"></td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:Button runat="server" ID="btnUnisenderReg" Text="Зарегистрироваться" OnClick="btnUnisenderReg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2">
                <hr color="#C2C2C4" size="1px" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">Уже есть аккаунт
            </td>
            <td style="text-align: left; vertical-align: top;">
                <table>
                    <tr class="rowsPost">
                        <td style="text-align: left; width: 250px;">
                            <asp:Localize ID="Localize40" runat="server" Text="<%$ Resources: SmsNotifications_ApiKey%>"></asp:Localize>
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox runat="server" ID="txtWwwUnisenderComApiKey" Width="220"></asp:TextBox>
                        </td>
                    </tr>

                    <tr class="rowsPost">
                        <td style="text-align: left; width: 250px;">
                            <asp:Localize ID="Localize41" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox runat="server" ID="txtWwwUnisenderComSender" Width="220"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="StreamTelecom" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtStreamTelecomLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize runat="server" Text="<%$ Resources: SmsNotifications_Password%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtStreamTelecomPassword" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtStreamTelecomSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>

    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="WwwSms4BRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize16" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSms4BRuLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources: SmsNotifications_Password%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSms4BRuPassword" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize18" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSms4BRuSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="SmslabRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize19" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtSmslabRuLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize20" runat="server" Text="<%$ Resources: SmsNotifications_Password%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtSmslabRuPassword" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtSmslabRuSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="WwwSmsimpleRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize22" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSmsimpleRuLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize23" runat="server" Text="<%$ Resources: SmsNotifications_Password%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSmsimpleRuPassword" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize24" runat="server" Text="<%$ Resources: SmsNotifications_Origin_id%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSmsimpleRuOriginId" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="GsmInformRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize25" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtGsmInformRuLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize26" runat="server" Text="<%$ Resources: SmsNotifications_Password%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtGsmInformRuPassword" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize27" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtGsmInformRuSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="WwwIqsmsRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize28" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwIqsmsRuLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize29" runat="server" Text="<%$ Resources: SmsNotifications_Password%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwIqsmsRuPassword" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize30" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwIqsmsRuSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="LeninsmsRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize31" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtLeninsmsRuLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize32" runat="server" Text="<%$ Resources: SmsNotifications_ApiKey%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtLeninsmsRuApiKey" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize33" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtLeninsmsRuSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="WwwSmspilotRu" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize34" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSmspilotRuLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize35" runat="server" Text="<%$ Resources: SmsNotifications_Password%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSmspilotRuPassword" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize36" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtWwwSmspilotRuSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" style="display: none; width: 100%;" id="RuSmsOnlineCom" class="servicesettings">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize37" runat="server" Text="<%$ Resources: SmsNotifications_Login%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtRuSmsOnlineComLogin" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize38" runat="server" Text="<%$ Resources: SmsNotifications_SecretKey%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtRuSmsOnlineComSecretKey" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize39" runat="server" Text="<%$ Resources: SmsNotifications_Sender%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtRuSmsOnlineComSender" Width="220"></asp:TextBox>
            </td>
        </tr>
    </table>

    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2">
                <hr color="#C2C2C4" size="1px" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; vertical-align: top; width: 250px;">
                <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: SmsNotifications_PhoneAdmin%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <table style="width: 250px">
                    <tr>
                        <td rowspan="3" style="vertical-align: top;">+</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="txtNumberPhoneAdmin" Width="140"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources: SmsNotifications_NumberPhoneAdmin%>"></asp:Localize></td>
                    </tr>
                </table>
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: SmsNotifications_SendSms%>"></asp:Localize>
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; vertical-align: top; width: 250px;">
                <asp:Localize ID="Localize43" runat="server" Text="<%$ Resources: SmsNotifications_SendSmsOrderPhone%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:CheckBox runat="server" ID="ckbSendSmsOrderPhone" />
                <span style="color: gray">
                    <asp:Localize ID="Localize44" runat="server" Text="<%$ Resources: SmsNotificationsWarning_SendSmsOrderPhone%>"></asp:Localize></span>
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2">
                <hr color="#C2C2C4" size="1px" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; vertical-align: top; width: 250px;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: SmsNotifications_NewOrder%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="ckbSendSmsNewOrder" Text="<%$ Resources: SmsNotifications_SendNewOrderUser%>" /><br />
                <asp:CheckBox runat="server" ID="ckbSendNewOrderAdmin" Text="<%$ Resources: SmsNotifications_SendNewOrderAdmin%>" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; vertical-align: top; width: 250px;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: SmsNotifications_TextSmsNewOrder%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txtTextSmsNewOrder" Width="430px" Height="80px" TextMode="MultiLine"></asp:TextBox>
                <br />
                <span style="color: gray">#ORDERNUMBER# -
                    <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: SmsNotifications_IdOrder%>"></asp:Localize></span>
                <br />
                <span style="color: gray">#ORDER_SUM# -
                    <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: SmsNotifications_ORDER_SUM%>"></asp:Localize></span>
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2">
                <hr color="#C2C2C4" size="1px" />
            </td>
            <td></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; vertical-align: top; width: 250px;">
                <asp:Localize ID="Localize42" runat="server" Text="<%$ Resources: SmsNotifications_ChangeStatus%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="ckbSendSmsChangeStatus" Text="<%$ Resources: SmsNotifications_SendNewOrderUser%>" /><br />
                <asp:CheckBox runat="server" ID="ckbSendChangeStatusAdmin" Text="<%$ Resources: SmsNotifications_SendNewOrderAdmin%>" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="text-align: left; vertical-align: top; width: 200px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: SmsNotifications_SendForStatus%>"></asp:Localize>
            </td>
            <td style="text-align: left; vertical-align: top; width: 800px;">
                <asp:Panel runat="server" ID="pnlProducts">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <triggers>
                        <asp:AsyncPostBackTrigger ControlID="grid" EventName="DataBinding" />
                             </triggers>
                        <contenttemplate>
                         <ul class="justify panel-do-grid">
                        <li class="justify-item panel-do-grid-item">
                            <select id="commandSelect" onchange="ChangeSelect()">
                                <option value="selectAll">
                                    <asp:Localize ID="Localize10" runat="server" Text="Выделить все"></asp:Localize>
                                </option>
                                <option value="unselectAll">
                                    <asp:Localize ID="Localize12" runat="server" Text="Снять выделение"></asp:Localize>
                                </option>
                                <option value="deleteSelected">
                                    <asp:Localize ID="Localize48" runat="server" Text="Удалить выделенные"></asp:Localize>
                                </option>
                                <option value="activateSelected">
                                    <asp:Localize ID="Localize49" runat="server" Text="Активировать выделенные"></asp:Localize>
                                </option>
                                <option value="deactivateSelected">
                                    <asp:Localize ID="Localize50" runat="server" Text="Деактивировать выделенные"></asp:Localize>
                                </option>
                            </select>
                            <a href="javascript:void(0)" class="btn btn-middle btn-action btn-do-grid" id="commandButton">GO</a>
                            <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" OnClick="lbDeleteSelected_Click" />
                            <asp:LinkButton ID="lbActivateSelected" Style="display: none" runat="server" OnClick="lbActivateSelected_Click" />
                            <asp:LinkButton ID="lbDeactivateSelected" Style="display: none" runat="server" OnClick="lbDectivateSelected_Click" />
                            <span class="panel-do-grid-selected-rows"><span id="selectedIdsCount" class="panel-do-grid-count"></span>
                                позиций выделено 
                            </span></li>
                        <li class="justify-item panel-do-grid-item">
                        <span class="subcategories-count-wrap">
                            Шаблонов:
                            <asp:Label ID="lblProducts" CssClass="foundrecords panel-do-grid-count" runat="server" Text="" />
                        </span>
                        </li>
                        <li class="justify-item panel-do-grid-item">
                            <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddStatus" runat="server" Text="Добавить"
                                ValidationGroup="0" OnClick="btnAddResource_Click" />
                        </li>
                    </ul>
                            <div style="width: 100%; clear: both;">
                            <div style="width: 100%">
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                                            CellSpacing="0" Confirmation="Вы действительно хотите удалить шаблон?" CssClass="tableview"
                                            GridLines="None" TooltipImgCellIndex="2" TooltipTextCellIndex="5"
                                            ReadOnlyGrid="False" OnRowCommand="grid_RowCommand" OnRowDataBound="grid_RowDataBound" >
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="id" Visible="False" HeaderStyle-Width="50px">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn">
                                        <ItemTemplate>
                                            <%# Eval("IsSelected") != DBNull.Value && (bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("Status") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Status" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                                Статус
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStatusName" Text='<%# _statusList.First(x => x.StatusID == Convert.ToInt32(Eval("Status"))).StatusName %>'> 
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlStatus" Width="100%" OnDataBound="ddlStatus_DataBound"/>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Content" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="450px">
                                        <HeaderTemplate>
                                                Шаблон
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="fckContentEdit" Width="430px" Height="80px" TextMode="MultiLine" Text='<%# Eval("Content") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblContent" Width="430px" Height="80px" TextMode="MultiLine" Text='<%# Eval("Content") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="fckContent" Width="430px" Height="80px" TextMode="MultiLine" Text=""></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Enabled" ItemStyle-CssClass="colid" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                                Активность
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox runat="server" style="text-align: center;" ID="chkEnabledEdit" Checked='<%#Eval("Enabled") %>'/>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" style="text-align: center;" ID="chkEnabledRO" Checked='<%#Eval("Enabled") %>' Enabled="False"/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox runat="server" ID="chkEnabled" Checked="True"/>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Buttons" ItemStyle-CssClass="colid" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                CommandName="deleteStatus" CommandArgument='<%#Eval("Status") %>'>
                                                <asp:Image ID="Image4" ImageUrl="images/deletebtn.png" runat="server" />
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnUpdate" runat="server" CssClass="updatebtn showtooltip" style="display: none;"
                                                CommandName="updateStatus" CommandArgument='<%#Eval("Status") %>'>
                                                <asp:Image ID="Image1" ImageUrl="images/updatebtn.png" runat="server" />
                                            </asp:LinkButton>
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image" src="images/cancelbtn.png"
                                                onclick="row_canceledit($(this).parent().parent()[0]); return false;" style="display: none"
                                                title='Отменить' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibAddStatus" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddStatus"
                                                ToolTip="Добавить" />
                                            <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                CommandName="CancelAdd" ToolTip="Отмена" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 readonlyrow" />
                            </adv:AdvGridView><br>
                            <asp:Localize runat="server" ID="ErrorsGrid"/>
                        </div>
                            <input type="hidden" id="SelectedIds" name="SelectedIds" />
                            </contenttemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <script type="text/javascript">

                    $(document).ready(function () {
                        $("#commandButton").click(function () {
                            var command = $("#commandSelect").val();

                            switch (command) {
                                case "selectAll":
                                    SelectAll(true);
                                    UpdateSelectedRow(true);
                                    break;
                                case "unselectAll":
                                    SelectAll(false);
                                    UpdateSelectedRow(false);
                                    break;
                                case "deleteSelected":
                                    window.__doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                                    break;
                                case "activateSelected":
                                    window.__doPostBack('<%=lbActivateSelected.UniqueID%>', '');
                                    break;
                                case "deactivateSelected":
                                    window.__doPostBack('<%=lbDeactivateSelected.UniqueID%>', '');
                                    break;
                            }
                        });
                    });

                    function UpdateSelectedRow(change) {
                        var items = $("input.sel");
                        if (items.length > 0) {
                            for (var i = 0; i < items.length; i++) {
                                items[i].checked = change;
                            }
                        }
                    }

                </script>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; vertical-align: top; width: 250px;"></td>
            <td>
                <span style="color: gray">#ORDERNUMBER# -
                    <asp:Localize ID="Localize53" runat="server" Text="<%$ Resources: SmsNotifications_IdOrder%>"></asp:Localize></span>
                <br />
                <span style="color: gray">#STATUS# -
                    <asp:Localize ID="Localize54" runat="server" Text="<%$ Resources: SmsNotifications_Status%>"></asp:Localize></span>
                <br />
                <span style="color: gray">#STATUSCOMMENT# -
                    <asp:Localize ID="Localize55" runat="server" Text="<%$ Resources: SmsNotifications_StatusComment%>"></asp:Localize></span>
                <br />
                <span style="color: gray">#TRACKNUMBER# -
                    <asp:Localize ID="Localize56" runat="server" Text="<%$ Resources: SmsNotifications_TrackNumber%>"></asp:Localize></span>
                <br />
                <span style="color: gray">#ORDER_SUM# -
                    <asp:Localize ID="Localize57" runat="server" Text="<%$ Resources: SmsNotifications_ORDER_SUM%>"></asp:Localize></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: SmsNotifications_Save%>" />
            </td>
            <td></td>
        </tr>
    </table>

    <script type="text/javascript">
        $(function () {
            ServiceSettings();
        });

        function ServiceSettings() {
            var s = $("#<%= ddlSmsService.ClientID %> :selected").val();
            $("table.servicesettings").hide();
            $("#" + s).show();
        }
    </script>
</div>
