<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    Codebehind="PaymentMethod.aspx.cs" Inherits="Admin.EditPaymentMethod" %>

<%@ Reference Control="~/Admin/UserControls/PaymentMethods/MasterControl.ascx" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/UserControls/PaymentMethods/MasterControl.ascx" TagName="PaymentMethod"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $(".paymentType").hover(
                function () {
                    $(this).addClass("ptHovered");
                },
                function () {
                    $(this).removeClass("ptHovered");
                });

            $.advModal({
                title: "<%= Resources.Resource.Admin_PaymentMethod_Adding %>",
                control: $("#<%= btnAddMethod.ClientID%>"),
                htmlContent: $("#<%= modal.ClientID%>"),
                beforeOpen: clearModal,
                clickOut: false
            });

        });

        function clearModal() {
            $("#<%= txtName.ClientID %>").val("");
            $("#<%= txtDescription.ClientID %>").val("");
            $("#<%= txtSortOrder.ClientID %>").val("");
            $("#<%= txtName.ClientID %>").focus();
        }
        function showElement(span) {
            var method_id = $("input:hidden", $(span)).val();
            if ($("hfPayment" + method_id).length == 0)
                location = "PaymentMethod.aspx?PaymentMethodID=" + method_id;
        }
        function hoverElement(span) {
            if (span.className == "selected") {
                span.className = "selected_hovered";
            } else {
                span.className = "hovered";
            }
        }

        function outElement(span) {
            if (span.className == "selected_hovered" || span.className == "selected") {
                span.className = "selected";
            } else {
                span.className = "free";
            }
        }    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div style="<%= IsIframe ? "display: none;" : "" %>">
        <div id="divModal" style="display: none;">
            <asp:Panel ID="modal" runat="server">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnOk" />
                    </Triggers>
                    <ContentTemplate>
                        <div style="background-color: white; padding-top: 10px; padding-bottom: 10px;">
                            <div style="text-align: center;">
                                <table class="form-payment-shipping">
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="Label1" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethods_Name %>"></asp:Label><span
                                                class="required">*</span>
                                        </td>
                                        <td style="height: 36px">
                                            <asp:TextBox runat="server" ID="txtName" Width="300"></asp:TextBox>
                                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txtName" EnableClientScript="true" Style="display: inline;"
                                                ErrorMessage='<%$ Resources: Resource, Admin_PaymentMethod_NameRequired %>'></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethods_Type %>"></asp:Label><span
                                                class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlType" DataTextField="Text" DataValueField="Value" Width="300px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethod_Description %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="300"
                                                Height="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethod_SortOrder %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtSortOrder" Width="300"></asp:TextBox>
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1"
                                                runat="server" ControlToValidate="txtSortOrder" EnableClientScript="true" ValidationExpression="[0-9]*"
                                                ErrorMessage="<%$ Resources: Resource, Admin_SortOrder_MustBeNumeric %>"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: right;">
                                            <asp:LinkButton ID="btnOk" CssClass="btn btn-middle btn-action" runat="server" OnClientClick="return Page_ClientValidate();"
                                                OnClick="btnOk_Click" Text="<%$ Resources: Resource, Admin_PaymentMethod_Create %>"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div class="content-top">
            <menu class="neighbor-menu neighbor-catalog">
                <li class="neighbor-menu-item"><a href="CommonSettings.aspx">
                    <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
                <li class="neighbor-menu-item"><a href="CheckoutFields.aspx">
                    <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
                <li class="neighbor-menu-item selected"><a href="PaymentMethod.aspx">
                    <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
                <li class="neighbor-menu-item"><a href="ShippingMethod.aspx">
                    <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
                <li class="neighbor-menu-item"><a href="Country.aspx">
                    <%= Resource.Admin_MasterPageAdmin_Countries%></a></li>
                <li class="neighbor-menu-item"><a href="Currencies.aspx">
                    <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
                <li class="neighbor-menu-item"><a href="Taxes.aspx">
                    <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
                <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                    <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
                <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                    <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
                <li class="neighbor-menu-item"><a href="LogError404.aspx">
                    <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
                <li class="neighbor-menu-item"><a href="Localizations.aspx">
                    <%= Resource.Admin_Localizations_Header%></a></li>
            </menu>
        </div>
    </div>
    <div class="content-own">
        <table style="width: 100%; table-layout: fixed; <%= IsIframe ? "display: none;" : "" %>" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/customers_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Header %>"></asp:Label><br />
                        <asp:Label ID="lblPaymentMethod" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_SubHeader %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblMessage" Visible="false" ForeColor="Blue"></asp:Label>
                    </td>
                    <td>
                        <div class="btns-main">
                            <asp:LinkButton CssClass="btn btn-middle btn-add" ID="btnAddMethod" runat="server" Text="<%$ Resources:Resource, Admin_Add %>" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        
        <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
            <tr>
                <td style="vertical-align: top; width: 225px; <%= IsIframe ? "display: none;" : "" %>">
                    <ul class="tabs" id="tabs-headers">
                        <asp:Repeater runat="server" ID="rptTabs">
                            <ItemTemplate>
                                <li runat="server" onclick="javascript:showElement(this)" class='<%# (int)Eval("PaymentMethodID") == PaymentMethodId ? "selected" : "" %>'>
                                    <asp:HiddenField runat="server" Value='<%# Eval("PaymentMethodID") %>' />
                                    <asp:Label ForeColor='<%# (bool)Eval("Enabled") ? Color.Black : Color.Gray %>' ID="Literal4"
                                        runat="server" Text='<%# Eval("Name") %>' />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </td>
                <td class="tabContainer" id="<%= IsIframe ? "" : "tabs-contents" %>" style="vertical-align: top;">
                    <asp:Label ID="lblError" runat="server" CssClass="mProductLabelInfo" ForeColor="Red"
                        Visible="False" Font-Names="Verdana" Font-Size="14px" EnableViewState="false"></asp:Label>
                    <asp:Panel runat="server" ID="pnMethods">
                        <asp:Panel runat="server" ID="pnEmpty" Visible="False">
                            <span>
                                <% = Resource.Admin_PaymentMethod_EmptyPayment %></span>
                        </asp:Panel>
                        <adv:PaymentMethod runat="server" ID="ucSberBank" PaymentType="SberBank" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucBill" PaymentType="Bill" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucCash" PaymentType="Cash" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucMailRu" PaymentType="MailRu" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucWebMoney" PaymentType="WebMoney" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucRobokassa" PaymentType="Robokassa" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucYandexMoney" PaymentType="YandexMoney" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucYandexKassa" PaymentType="YandexKassa" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucAuthorizeNet" PaymentType="AuthorizeNet"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucGoogleCheckout" PaymentType="GoogleCheckout"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="uceWAY" PaymentType="eWAY" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucCheck" PaymentType="Check" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPayPal" PaymentType="PayPal" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucTwoCheckout" PaymentType="TwoCheckout" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucAssist" PaymentType="Assist" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucZPayment" PaymentType="ZPayment" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPlatron" PaymentType="Platron" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucRbkmoney" PaymentType="Rbkmoney" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucCyberPlat" PaymentType="CyberPlat" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucAmazonSimplePay" PaymentType="AmazonSimplePay"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucMoneybookers" PaymentType="Moneybookers"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucChronoPay" PaymentType="ChronoPay" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPayOnline" PaymentType="PayOnline" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPSIGate" PaymentType="PSIGate" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPayPoint" PaymentType="PayPoint" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucSagePay" PaymentType="SagePay" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucWorldPay" PaymentType="WorldPay" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucOnPay" PaymentType="OnPay" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPickPoint" PaymentType="PickPoint" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucCashOnDelivery" PaymentType="CashOnDelivery"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucGiftCertificate" PaymentType="GiftCertificate"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucMasterBank" PaymentType="MasterBank" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucWalletOneCheckout" PaymentType="WalletOneCheckout"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucQiwi" PaymentType="QIWI" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucKupivkredit" PaymentType="Kupivkredit" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucYesCredit" PaymentType="YesCredit" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucInterkassa" PaymentType="Interkassa" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucInterkassa2" PaymentType="Interkassa2" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucLiqPay" PaymentType="LiqPay" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucBillUa" PaymentType="BillUa" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucMoscowBank" PaymentType="MoscowBank" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucGateLine" PaymentType="GateLine" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucQppi" PaymentType="Qppi" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucBitPay" PaymentType="BitPay" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucIntellectMoney" PaymentType="IntellectMoney"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucIntellectMoneyMainProtocol" PaymentType="IntellectMoneyMainProtocol"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucAvangard" PaymentType="Avangard" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucDibs" PaymentType="Dibs" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucRsbCredit" PaymentType="RsbCredit" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucDirectCredit" PaymentType="DirectCredit"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPayAnyWay" PaymentType="PayAnyWay" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucPayPalExpressCheckout" PaymentType="PayPalExpressCheckout"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucMoneXy" PaymentType="MoneXy"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucNetPay" PaymentType="NetPay"
                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucAlfabankUa" PaymentType="AlfabankUa" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucCloudPayments" PaymentType="CloudPayments" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucPaymaster" PaymentType="Paymaster" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucAlfabank" PaymentType="Alfabank" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                        <adv:PaymentMethod runat="server" ID="ucTinkoff" PaymentType="Tinkoff" OnSaved="PaymentMethod_Saved"
                            OnErr="PaymentMethod_Error" />
                    </asp:Panel>
                </td>
            </tr>
        </table>

    </div>
</asp:Content>
