<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterControl.ascx.cs"
    Inherits="Admin.UserControls.ShippingMethods.MasterControl" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Extensions" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<input type="hidden" id="hfShipping<%= Method == null ? "" : Method.ShippingMethodId.ToString()  %>" />
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px;" class="AllNice">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethods_HeadGeneral%>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize14" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Name %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td style="vertical-align: top">
            <asp:TextBox runat="server" ID="txtName" Width="400px" MaxLength="250"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Название метода доставки
                    </header>
                    <div class="help-content">
                        Это название будет показано в клиентской части магазина.<br />
                        <br />
                        Например: Самовывоз.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgName" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_MethodAvailibleFor %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <div id="GEO" style="font-weight:bold; display:inline;"></div>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Города и страны
                    </header>
                    <div class="help-content">
                        Тут вы можете задать список стран и городов, для которых будет доступен данный метод доставки.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;"></td>
        <td style="vertical-align: top">
            <div style="width: 70px; float: left;">
                <%= Resources.Resource.Admin_ShippingMethod_Countries%>:
            </div>
            <asp:TextBox runat="server" ID="txtCountry" Width="177"></asp:TextBox>
            <asp:LinkButton ID="btnAddCountry" runat="server" ValidationGroup="1" Text="<%$ Resources:Resource, Admin_ShippingMethod_AddCountry%>"
                OnClick="btnAddCountry_Click"></asp:LinkButton>
            <asp:Label runat="server" ID="msgCountry" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;"></td>
        <td style="vertical-align: top">
            <div style="width: 70px; float: left;">
                <%= Resources.Resource.Admin_ShippingMethod_Cities%>:
            </div>
            <asp:TextBox runat="server" ID="txtCity" Width="177"></asp:TextBox>
            <asp:LinkButton ID="btnAddCity" runat="server" ValidationGroup="1" Text="<%$ Resources:Resource, Admin_ShippingMethod_AddCity%>"
                OnClick="btnAddCity_Click"></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_MethodExcludedFor %>"/>:
        </td>
        <td style="vertical-align: top">
            <div id="GEOExcluded" style="font-weight:bold; display:inline;"></div>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Города исключения
                    </header>
                    <div class="help-content">
                        Тут вы можете задать только города исключения. Для них метод доставки не будет показан.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;"></td>
        <td style="vertical-align: top">
            <div style="width: 70px; float: left;">
                <%= Resources.Resource.Admin_ShippingMethod_Cities%>:
            </div>
            <asp:TextBox runat="server" ID="txtCityExcluded" Width="177"></asp:TextBox>
            <asp:LinkButton ID="lbAddExcludedCity" runat="server" ValidationGroup="1" Text="<%$ Resources:Resource, Admin_ShippingMethod_AddCity%>"
                OnClick="btnAddCityExcluded_Click"></asp:LinkButton>
        </td>
    </tr>


    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Type %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
           <%-- <%= ShippingType %>--%>
            <%= string.Format("{0} ({1})", HtmlExtensions.GetShippingLocalize(ShippingType), ShippingType)%>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize15" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Description %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;padding-bottom: 5px;">
            <asp:TextBox runat="server" ID="txtDescription" Width="400" TextMode="MultiLine"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Описание
                    </header>
                    <div class="help-content">
                        Описание метода доставки, которое выводится в клиентской части магазина.<br />
                        <br />
                        Например: Курьер, доставка до двери.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ZeroPriceMessageHeader %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:TextBox runat="server" ID="txtZeroPriceMessage" Width="250"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Текст при нулевой стоимости 
                    </header>
                    <div class="help-content">
                        Текст, который будет показан, если стоимость доставки равна нулю.<br />
                        <br />
                        Например: Бесплатно.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize16" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_SortOrder %>"></asp:Localize>
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
                        Очерёдность вывода метода доставки. <br />
                        Действует правило "ноль вверху"
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgSortOrder" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Enabled %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
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
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_DisplayCustomFields%>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:CheckBox runat="server" ID="chkDisplayCustomFields" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Дополнительные поля
                    </header>
                    <div class="help-content">
                        Показывать ли для данного метода доставки дополнительные поля.<br />
                        <br />
                        Дополнительные поля для доставки вы можете назначить в "Настройки - Поля в оформлении заказов"
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize9" runat="server" Text="Показывать индекс"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:CheckBox runat="server" ID="chkDisplayIndex" />
        </td>
    </tr>

    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShowInDetails%>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:CheckBox runat="server" ID="ckbShowInDetails" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображать в карточке товара
                    </header>
                    <div class="help-content">
                        Отображать или нет данный способ доставки в блоке "доставка" в карточке товара.<br />
                        <br />
                        Этой опцией вы можете регулировать, какие методы отображать, какие нет.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Icon %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <table>
                <tr>
                    <td>
                        <asp:Image runat="server" ID="imgIcon" />
                    </td>
                    <td>
                        <asp:FileUpload runat="server" ID="fuIcon" />
                        <asp:Button runat="server" ID="btnUploadIcon" Text="<%$ Resources:Resource, Admin_ShippingMethod_UploadIcon %>" CssClass="btn btn-middle btn-action" style="margin-right:5px;"
                            OnClick="btnUploadIcon_Click" CausesValidation="false" UseSubmitBehavior="False" />
                        <asp:Button runat="server" ID="btnDeleteIcon" Text="<%$ Resources:Resource, Admin_ShippingMethod_DeleteIcon %>" CssClass="btn btn-middle btn-action"
                            OnClick="btnDeleteIcon_Click" CausesValidation="false" UseSubmitBehavior="False" />
                        <div class="subSaveNotify" style="margin-bottom: 7px; margin-top: 0px;">Рекомендуемый размер 60 x 32 px <br>Формат может быть только *.gif, *.png или *.jpg</div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ActiveForPayments %>"></asp:Localize>
        </td>
        <td style="vertical-align: top">
            <asp:Repeater ID="rptrPayments" runat="server">
                <ItemTemplate>
                    <asp:HiddenField ID="hfPaymentId" runat="server" Value='<%# Eval("PaymentMethodID")%>' />
                    <asp:CheckBox ID="ckbUsePayment" runat="server" Text='<%#Eval("Name") %>' Checked='<%# SQLDataHelper.GetInt(Eval("Use")) == 0 %>' /><br />
                </ItemTemplate>
            </asp:Repeater>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top; padding-top: 15px">
            Налог
        </td>
        <td style="vertical-align: top; padding-top: 15px">
            <asp:DropDownList runat="server" ID="ddlTaxType" DataTextField="Text" DataValueField="Value" />
        </td>
    </tr>
</table>
<asp:Panel ID="pnlSpecific" Width="100%" runat="server" CssClass="AllNice">
</asp:Panel>
<div style="height:40px; margin-top:15px;">
    <asp:LinkButton runat="server" CssClass="btn btn-middle btn-action valid-confirm" ID="btnDelete"
        Text="<%$ Resources: Resource, Admin_ShippingMethod_Delete %>" OnClick="btnDelete_Click" CausesValidation="false"/> 
    <asp:LinkButton runat="server" CssClass="btn btn-middle btn-add" ID="btnSave" style="margin-left:5px;" 
        Text="<%$ Resources: Resource, Admin_ShippingMethod_Save %>" OnClick="btnSave_Click" ValidationGroup="5" />
</div>
<script type="text/javascript">

    function DelCity(cityID, methodId) {
        DelCityHandler(cityID, methodId, '<% = UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/DeleteCity.ashx") %>');
    }

    function DelCityHandler(cityID, methodId, url) {
        $.ajax({
            data: { cityID: cityID, methodId: methodId, subject: 'shipping' },
            url: url,
            dataType: "html",
            cache: false,
            success: function (result) {
                GetResult('<% =UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + ShippingMethodId) %>');
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
                data: { countryId: countryId, methodId: methodId, subject: 'shipping' },
                url: url,
                dataType: "html",
                cache: false,
                success: function (result) {
                    GetResult('<% = UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + ShippingMethodId)%>');
                },
                error: function (result) {
                    alert(result);
                }
            });
            }

            function GetResult(url) {
                $.ajax({
                    url: url,
                    data: { subject: 'shipping' },
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
                GetResult('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + ShippingMethodId) %>');
            });

</script>

<script type="text/javascript">

    function DelCityExcluded(cityID, methodId) {
        DelCityHandlerExcluded(cityID, methodId, '<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/DeleteCityExcluded.ashx")%>');
    }

    function DelCityHandlerExcluded(cityID, methodId, url) {
        $.ajax({
            data: { cityID: cityID, methodId: methodId, subject: 'shipping' },
            url: url,
            dataType: "html",
            cache: false,
            success: function (result) {
                GetResultExcluded('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCityExcluded.ashx?methodId=" + ShippingMethodId)%>');
            },
            error: function (result) {
                alert(result);
            }
        });
        }

        function GetResultExcluded(url) {
            $.ajax({
                url: url,
                data: { subject: 'shipping' },
                dataType: "html",
                async: false,
                cache: false,
                success: function (result) {
                    $("#GEOExcluded").html(result);
                },
                error: function (result) {
                    alert(result);
                }
            });
        }

        $(document).ready(function () {
            GetResultExcluded('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCityExcluded.ashx?methodId=" + ShippingMethodId)%>');
        });

</script>

<script type="text/javascript">
    $(function () {
        window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            GetResult('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCity.ashx?methodId=" + ShippingMethodId) %>');
            GetResultExcluded('<%= UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetAdminCountryCityExcluded.ashx?methodId=" + ShippingMethodId) %>');

            $("#<%=txtCountry.ClientID%>").autocomplete('<%=UrlService.GetAbsoluteLink("HttpHandlers/Location/GetCountries.ashx") %>', {
                delay: 300,
                minChars: 1,
                matchSubset: 1,
                autoFill: false,
                matchContains: 1,
                cacheLength: 0,
                selectFirst: false,
                maxItemsToShow: 10
            });

            $("#<%=txtCity.ClientID%>, #<%=txtCityExcluded.ClientID%>").autocomplete('<%=UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetCities.ashx") %>', {
                delay: 300,
                minChars: 1,
                matchSubset: 1,
                autoFill: false,
                matchContains: 1,
                cacheLength: 0,
                selectFirst: false,
                maxItemsToShow: 10
            });

            $("img[src='images/messagebox_info.png']").hide().filter("[title]").show();
        });
    });
    function ConfirmDelete() {
        return;
    }
</script>
