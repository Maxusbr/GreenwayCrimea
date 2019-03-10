<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Order.ShippingRatesControl" CodeBehind="ShippingRates.ascx.cs" %>
<div id="divScripts" runat="server" visible="true">
    <script type="text/javascript" src="//pickpoint.ru/select/postamat.js"></script>
    <script type="text/javascript">
        function SetPickPointAnswer(result) {
            $('#<%= pickpointId.ClientID  %>').val(result['id']);
            $('#address').html(result['name'] + '<br />' + result['address']);
            $('#<%= pickAddress.ClientID  %>').val(result['name'] + '<br />' + result['address']);
        }
    </script>
</div>
<script type="text/javascript">

    //Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(RefreshHiddenInputs());

    function RefreshHiddenInputs(conteineritem) {
        var radio = $(conteineritem).children("input[type=radio]");
        $("._selectedID").val(radio.val());

        var elShipping = $(conteineritem).find(".shipping-type");
        $(".hfType").val(elShipping.attr("data-type"));
        $(".hfMethodId").val(elShipping.attr("data-methodId"));
        $(".hfName").val(elShipping.attr("data-name"));

        var select = $(conteineritem).find(".shipping-points-sdek, .shipping-points");

        var selectCheckout = $(conteineritem).find(".shipping-points-checkout, .shipping-points");
        if (select.length) {
            $(".pickpointId").val(select.val());
            $(".pickAddress").val(select.children("option:selected").text());

            $(".pickAdditional").val($(conteineritem).find(".hiddenSdekTariff").val());
        }
        else if (selectCheckout.length || $(conteineritem).find(".hiddenCheckoutInfo").length) {

            if (selectCheckout.length) {
                var selectOption = selectCheckout.children("option:selected");

                $(".pickpointId").val(selectOption.val());
                $(".pickAddress").val(selectOption.attr("data-checkout-address"));
                $(".pickAdditional").val(selectOption.attr("data-additional"));
            }
            else if ($(conteineritem).find(".hiddenCheckoutInfo").length) {
                $(".pickAdditional").val($(conteineritem).find(".hiddenCheckoutInfo").val());
            }
        } else {
            //$(".pickpointId").val("");
            //$(".pickAddress").val("");
            //$(".pickAdditional").val($(this).find(".hiddenSdekTariff").val());                
        }
    };

    $(function () {
        $("body").on("change", ".shipping-points-sdek, .shipping-points", function (e) {

            var radio = $(this).closest(".radio-shipping").children("input[type=radio]");
            $("._selectedID").val(radio.val());
            radio.attr("checked", "checked");

            $(".pickpointId").val($(this).val());
            $(".pickAddress").val($(this).children("option:selected").text());
            $(".pickAdditional").val($(this).closest(".radio-shipping").find(".hiddenSdekTariff").val());
        });

        $("body").on("click", ".radio-shipping", function () {
            RefreshHiddenInputs(this)
        });

        $("body").on("click", ".btnShippinOK", function () {
            var inputChecked = jQuery(".radio-shipping input[type=radio]:checked");
            RefreshHiddenInputs(inputChecked.closest(".radio-shipping")[0])
        });

        $("body").on("blur", ".tDistance", function () {
            $(".hfDistance").val($(this).val());
        });
    });

</script>
<div id="divMultiShip" runat="server" visible="false">
    <script type="text/javascript" src="https://api-maps.yandex.ru/2.1/?lang=ru-RU"></script>
    <script type="text/javascript" src="<%=WidgetCode%>"></script>
    <script type="text/javascript">
        $(window).load(function () {
            <%--jQuery.getScript("https://api-maps.yandex.ru/2.1/?lang=ru-RU", function () {

                jQuery.getScript("<%=WidgetCode%>", function () {--%>
            
                    ydwidget.ready(function () {

                        yd$('body').prepend('<div id="ydwidget" class="yd-widget-modal"></div>');

                        ydwidget.initCartWidget({
                            'getCity': function () {
                                var city = $(".ms-city").val();
                                if (city != "") {
                                    return { value: city };
                                } else {
                                    return false;
                                }
                            },

                            //id элемента-контейнера
                            'el': 'ydwidget',
                            //общее количество товаров в корзине
                            'totalItemsQuantity': function () { return <%= Amount%>; },
                            //общий вес товаров в корзине
                            'weight': function () { return <%= Weight%>; },
                            //общая стоимость товаров в корзине
                            'cost': function () { return <%= Cost%>; },
                            //габариты и количество по каждому товару в корзине
                            'itemsDimensions': function () {
                                return <%= Dimensions%>;
                            },
                            //обработка смены варианта доставки
                            'onDeliveryChange': function (delivery) {
                                //если выбран вариант доставки, выводим его описание и закрываем виджет, иначе произошел сброс варианта,
                                //очищаем описание
                                if (delivery) {
                                    setYaDeliveryAnswer(delivery);
                                    ydwidget.cartWidget.close();
                                }
                            },
                            // Объявленная ценность заказа. Влияет на расчет стоимости в предлагаемых вариантах доставки.
                            'assessed_value': <%= ShowAssessedValue%> ? <%= Cost%> : 0,

                            'onlyDeliveryTypes': function () { return ['pickup']; },
                            'createOrderFlag': function () { return false; },
                            'order': {
                                //имя, фамилия, телефон, улица, дом, индекс
                                'recipient_first_name': function () { return ""; },
                                'recipient_last_name': function () { return ""; },
                                'recipient_phone': function () { return ""; },
                                'deliverypoint_street': function () { return ""; },
                                'deliverypoint_house': function () { return ""; },
                                'deliverypoint_index': function () { return ""; }
                            },

                        });
                    });
            //    });
            //});

            function setYaDeliveryAnswer(delivery) {
                
                var additionalData = {
                    direction: delivery.direction,
                    delivery: delivery.delivery_id,
                    price: delivery.costWithRules,
                    tariffId: delivery.tariffId
                };

                if (delivery.settings != null && delivery.settings.to_yd_warehouse != null) {
                    additionalData.to_ms_warehouse = parseInt(delivery.settings.to_yd_warehouse);
                }

                var description = delivery.delivery.name ;

                if (delivery.full_address != null) {
                    description +=  ', ' + delivery.full_address;
                }
                if (delivery.days != null && delivery.days != "") {
                    description += ", " + delivery.days + " дн";
                }

                if (delivery.deliveryIntervalFormatted != null && delivery.deliveryIntervalFormatted != "") {
                    description += ", " + delivery.deliveryIntervalFormatted;
                }

                $('.pickpointId').val(delivery.pickuppointId);
                $('.pickAddress').val(description);
                $('.yandexdelivery-address').val(description);
                $('.pickAdditional').val(JSON.stringify(additionalData));

                $('.yandexdelivery-address').parent('label').find('.price').val(delivery.costWithRules + " руб");
            }
        });
    </script>
    <div id="mswidget" class="ms-widget-modal"></div>
</div>
<input type="hidden" id="_selectedID" class="_selectedID" runat="server" value="" />
<input type="hidden" id="pickpointId" class="pickpointId" runat="server" value="" />
<input type="hidden" id="pickAddress" class="pickAddress" runat="server" value="" />
<input type="hidden" id="pickAdditional" class="pickAdditional" runat="server" value="" />
<input type="hidden" id="hfDistance" class="hfDistance" runat="server" value="0" />
<input type="hidden" id="hfType" class="hfType" runat="server" value="" />
<input type="hidden" id="hfMethodId" class="hfMethodId" runat="server" value="" />
<input type="hidden" id="hfName" class="hfName" runat="server" value="" />

<div id="RadioList" runat="server" class="RadioList" visible="true">
</div>
<asp:Label ID="lblNoShipping" runat="server" Style="color: red" Visible="false" Text="<%$ Resources:Resource, Client_ShippingRates_NoShipping %>"></asp:Label>