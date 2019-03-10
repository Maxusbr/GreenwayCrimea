<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterControl.ascx.cs"
    Inherits="Admin.UserControls.PaymentMethods.MasterControl" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Extensions" %>
<input type="hidden" id="hfPayment<%= Method == null ? "" : Method.PaymentMethodId.ToString(CultureInfo.InvariantCulture)  %>" />
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px;" class="AllNice">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadGeneral%>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize14" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Name %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td style="vertical-align: top">
            <asp:TextBox runat="server" ID="txtName" Width="400px" MaxLength="250"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Название метода оплаты
                    </header>
                    <div class="help-content">
                        Это название будет показано в клиентской части магазина.<br />
                        <br />
                        Например: Оплата пластиковой картой.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgName" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_MethodIsAvailableFor %>" />:
        </td>
        <td style="vertical-align: top">
            <div id="GEO" style="font-weight: bold; display: inline;"></div>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Города и страны
                    </header>
                    <div class="help-content">
                        Тут вы можете задать список стран и городов, для которых будет доступен данный метод оплаты.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;"></td>
        <td style="vertical-align: top">
            <div style="width: 70px; float: left;">
                <%= Resources.Resource.Admin_PaymentMethod_Coutries %>:
            </div>
            <asp:TextBox runat="server" ID="txtCountry" Width="177"></asp:TextBox>
            <asp:LinkButton ID="lbtnAddCountry" runat="server" ValidationGroup="1" Text="<%$ Resources:Resource, Admin_PaymentMethod_AddCountry %>"
                OnClick="btnAddCountry_Click"></asp:LinkButton>
            <asp:Label runat="server" ID="msgCountry" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;"></td>
        <td style="vertical-align: top">
            <div style="width: 70px; float: left;">
                <%= Resources.Resource.Admin_PaymentMethod_Cities %>:
            </div>
            <asp:TextBox runat="server" ID="txtCity" Width="177"></asp:TextBox>
            <asp:LinkButton ID="lbtnAddCity" runat="server" ValidationGroup="1" Text="<%$ Resources:Resource, Admin_PaymentMethod_AddCity %>"
                OnClick="btnAddCity_Click"></asp:LinkButton>
        </td>
    </tr>
    <tr style="height: 40px;">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Type %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; padding-top: 5px;">
           <%-- <%= PaymentType%>--%>
            <%= string.Format("{0} ({1})", HtmlExtensions.GetPaymentLocalize(PaymentType), PaymentType)%>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize15" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Description %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:TextBox runat="server" ID="txtDescription" Width="250px" Height="32px" TextMode="MultiLine"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Описание
                    </header>
                    <div class="help-content">
                        Описание метода оплаты, которое выводится в клиентской части магазина.<br />
                        <br />
                        Например: Оплата наличными курьеру при получении посылки.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize16" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_SortOrder %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td style="vertical-align: top">
            <asp:TextBox runat="server" ID="txtSortOrder" Width="75"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Порядок сортировки
                    </header>
                    <div class="help-content">
                        Очерёдность вывода метода оплаты.
                        <br />
                        Действует правило "ноль вверху"
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgSortOrder" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr style="height: 40px;">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Enabled %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; padding-top: 5px;">
            <asp:CheckBox runat="server" ID="chkEnabled" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Активен
                    </header>
                    <div class="help-content">
                        Включен или выключен метод в данный момент.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Icon %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <table>
                <tr>
                    <td>
                        <asp:Image runat="server" ID="imgIcon" />
                    </td>
                    <td>
                        <asp:FileUpload runat="server" ID="fuIcon" />
                        <asp:Button runat="server" ID="btnUpload" Text="<%$ Resources:Resource, Admin_PaymentMethod_Upload %>" CssClass="btn btn-middle btn-action" Style="margin-right: 5px;"
                            OnClick="btnUpload_Click" CausesValidation="false" UseSubmitBehavior="false" />
                        <asp:Button runat="server" ID="btnDeleteIcon" Text="<%$ Resources:Resource, Admin_ShippingMethod_DeleteIcon %>" CssClass="btn btn-middle btn-action"
                            OnClick="btnDeleteIcon_Click" CausesValidation="false" UseSubmitBehavior="false" />
                        <div class="subSaveNotify" style="margin-bottom: 7px; margin-top: 0px;">Рекомендуемый размер 60 x 32 px
                            <br>
                            Формат может быть только *.gif, *.png или *.jpg</div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr style="height: 40px;">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_TypeExtraCharge %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <adv:EnumDataSource runat="server" ID="edsTypes" EnumTypeName="AdvantShop.Payment.ExtrachargeType" />
            <asp:DropDownList runat="server" ID="ddlExtrachargeType" DataSourceID="edsTypes" DataTextField="LocalizedName" DataValueField="Value" Style="display: inline;" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Тип наценки
                    </header>
                    <div class="help-content">
                        Тут вы указываете тип наценки на метод оплаты, если она есть.
                        <br />
                        <br />
                        Фиксированная - это фиксированное число, скажем 100 руб.<br />
                        <br />
                        Процентная - это процент от суммы к оплате, скажем 3%.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr style="height: 40px;">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_ExtraCharge %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:TextBox runat="server" ID="txtExtracharge" Width="75"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Тип наценки
                    </header>
                    <div class="help-content">
                        Тут вы указываете саму наценку на метод оплаты, если она есть.
                        <br />
                        <br />
                        Фиксированная - это фиксированное число, скажем "100" руб.<br />
                        <br />
                        Процентная - это процент от суммы к оплате, скажем "3" %.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr id="paymentCurrency" runat="server">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize10" runat="server" Text="Валюта, в которой производится оплата"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:DropDownList runat="server" ID="ddlPayCurrency" Style="display: inline;" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Валюта, в которой производится оплата
                    </header>
                    <div class="help-content">
                        Данная опция отвечает за назначение валюты, в которой работает данный метод оплаты.<br />
                        <br />
                        Например, если у вас каталог в долларах США, а счет вам нужно выставить в рублях.<br />
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlSpecific" Width="100%" runat="server" CssClass="AllNice">
</asp:Panel>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px;" class="AllNice">
    <tr runat="server" id="trReturnUrl">
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_ReturnUrl %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:Literal ID="litReturnUrl" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr runat="server" id="trFailUrl">
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_FailUrl %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:Literal ID="litFailUrl" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr runat="server" id="trCancelUrl">
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_CancelUrl %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:Literal ID="litCancelUrl" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr runat="server" id="trNotificationUrl">
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_NotificationUrl %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:Literal ID="litNotificationUrl" runat="server"></asp:Literal>
        </td>
    </tr>
</table>
<div style="height: 40px; margin-top: 15px;">
    <asp:LinkButton runat="server" CssClass="btn btn-middle btn-action valid-confirm" ID="btnDelete"
        Text="<%$ Resources: Resource, Admin_PaymentMethod_Delete %>" OnClick="btnDelete_Click" CausesValidation="false" />
    <asp:LinkButton runat="server" CssClass="btn btn-middle btn-add" ID="btnSave" Style="margin-left: 5px;"
        Text="<%$ Resources: Resource, Admin_PaymentMethod_Save %>" OnClick="btnSave_Click" CausesValidation="false" />
</div>
<input type="hidden" id="methodId" value="<%=PaymentMethodID %>" />
<script type="text/javascript">

    function DelCity(cityID, methodId) {
        DelCityHandler(cityID, methodId, '<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/DeleteCity.ashx") %>');
    }

    function DelCityHandler(cityID, methodId, url) {
        $.ajax({
            data: { cityID: cityID, methodId: methodId, subject: 'payment' },
            url: url,
            dataType: "html",
            cache: false,
            success: function (result) {
                GetResult('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + PaymentMethodID) %>');
            },
            error: function (result) {
                alert(result);
            }
        });
        }

        function DelCountry(countryId, methodId) {
            DelCountryHandler(countryId, methodId, '<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/DeleteCountry.ashx") %>');
        }

        function DelCountryHandler(countryId, methodId, url) {
            $.ajax({
                data: { countryId: countryId, methodId: methodId, subject: 'payment' },
                url: url,
                dataType: "html",
                cache: false,
                success: function (result) {
                    GetResult('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + PaymentMethodID) %>');
            },
            error: function (result) {
                alert(result);
            }
        });
        }

        function GetResult(url) {
            $.ajax({
                url: url,
                data: { subject: 'payment' },
                dataType: "html",
                async: false,
                cache: false,
                success: function (result) {
                    $("#GEO").html(result);
                },
                error: function (result) {
                    alert(result);
                }
            });
        }

        $(document).ready(function () {
            GetResult('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + PaymentMethodID) %>');
        });

</script>
<script type="text/javascript">
    $(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            GetResult('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + PaymentMethodID) %>');

            $("#<%=txtCountry.ClientID%>").autocomplete('<%=UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetCountries.ashx") %>', {
                delay: 300,
                minChars: 1,
                matchSubset: 1,
                autoFill: false,
                matchContains: 1,
                cacheLength: 0,
                selectFirst: false,
                maxItemsToShow: 10,
            });

            $("#<%=txtCity.ClientID%>").autocomplete('<%=UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetCities.ashx") %>', {
                delay: 300,
                minChars: 1,
                matchSubset: 1,
                autoFill: false,
                matchContains: 1,
                cacheLength: 0,
                selectFirst: false,
                maxItemsToShow: 10,
            });

            $("img[src='images/messagebox_info.png']").hide().filter("[title]").show();
        });
    });
    function ConfirmDelete() {
        return;
    }
</script>
