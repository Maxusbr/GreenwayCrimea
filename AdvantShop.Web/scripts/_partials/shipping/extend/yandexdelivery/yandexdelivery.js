; (function (ng) {
    'use strict';

    ng.module('yandexdelivery', [])
      .controller('YandexDeliveryCtrl', ['$scope', function ($scope) {

          var ctrl = this;

          ctrl.init = function () {
              if (!window.isYandexDeliveryLoaded) {

                  jQuery.getScript("https://api-maps.yandex.ru/2.1/?lang=ru-RU", function () {

                      jQuery.getScript(ctrl.yandexDeliveryWidgetCodeYa, function () {

                          ydwidget.ready(function () {
                              yd$('body').prepend('<div id="ydwidget" class="yd-widget-modal"></div>');

                              ydwidget.initCartWidget({
                                  'getCity': function() {
                                      var contact = ctrl.yandexDeliveryContact;
                                      if (contact != null && contact.City != null) {
                                          return { value: contact.City };
                                      }
                                      return false;
                                  },

                                  //id элемента-контейнера
                                  'el': 'ydwidget',
                                  //общее количество товаров в корзине
                                  'totalItemsQuantity': function() { return ctrl.yandexDeliveryAmount; },
                                  //общий вес товаров в корзине
                                  'weight': function() { return ctrl.yandexDeliveryWeight; },
                                  //общая стоимость товаров в корзине
                                  'cost': function() { return ctrl.yandexDeliveryCost; },
                                  //габариты и количество по каждому товару в корзине
                                  'itemsDimensions': function() {
                                      return eval(ctrl.yandexDeliveryDimensions);
                                  },
                                  //обработка смены варианта доставки
                                  'onDeliveryChange': function(delivery) {
                                      //если выбран вариант доставки, выводим его описание и закрываем виджет, иначе произошел сброс варианта,
                                      //очищаем описание
                                      if (delivery) {
                                          ctrl.setYaDeliveryAnswer(delivery);
                                          ydwidget.cartWidget.close();
                                      }
                                  },
                                  // Объявленная ценность заказа. Влияет на расчет стоимости в предлагаемых вариантах доставки.
                                  'assessed_value': ctrl.yandexDeliveryShowAssessedValue ? ctrl.yandexDeliveryCost : 0,
                                  //'onlyPickuppoints': true, //old param
                                  'onlyDeliveryTypes': function(){return ['pickup'];},
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
                      });

                      window.isYandexDeliveryLoaded = true;
                  });
              }
          }

          ctrl.setYaDeliveryAnswer = function (delivery) {
              if (typeof (delivery) === 'string') {
                  delivery = (new Function("return " + delivery))();
              }

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
                  description += ', ' + delivery.full_address;
              }
              if (delivery.days != null && delivery.days != "") {
                  description += ", " + delivery.days + " дн";
              }

              if (delivery.deliveryIntervalFormatted != null && delivery.deliveryIntervalFormatted != "") {
                  description += ", " + delivery.deliveryIntervalFormatted;
              }
                            

              ctrl.yandexDeliveryShipping.PickpointId = delivery.pickuppointId;
              ctrl.yandexDeliveryShipping.PickpointAddress = description;
              ctrl.yandexDeliveryShipping.PickpointAdditionalData = JSON.stringify(additionalData);
              ctrl.yandexDeliveryShipping.TariffId = delivery.tariffId;

              ctrl.yandexDeliveryCallback({ event: 'yandexDelivery', field: ctrl.yandexDeliveryShipping.PickpointId || 0, shipping:ctrl.yandexDeliveryShipping});

              $scope.$digest();
          }
      }])
      .directive('yandexDelivery', ['urlHelper', function (urlHelper) {
          return {
              scope: {
                  yandexDeliveryShipping: '=',
                  yandexDeliveryCallback: '&',
                  yandexDeliveryWidgetCodeYa: '=',
                  yandexDeliveryShowAssessedValue: '=',
                  yandexDeliveryAmount: '=',
                  yandexDeliveryWeight: '=',
                  yandexDeliveryCost: '=',
                  yandexDeliveryDimensions: '=',
                  yandexDeliveryIsSelected: '=',
                  yandexDeliveryContact: '='
              },
              controller: 'YandexDeliveryCtrl',
              controllerAs: 'yandexDelivery',
              bindToController: true,
              templateUrl: function () {
                  return urlHelper.getAbsUrl('scripts/_partials/shipping/extend/yandexdelivery/yandexdelivery.tpl.html', true);
              },
              link: function (scope, element, attrs, ctrl) {
                  ctrl.init();
              }
          }
      }])

})(window.angular);