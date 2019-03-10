; (function (ng) {
    'use strict';

    ng.module('boxberry', [])
      .controller('BoxberryCtrl', ['$scope', function ($scope) {

          var ctrl = this;

          ctrl.init = function () {
              if (!window.isBoxberryLoaded) {
                  jQuery.getScript((window.location.protocol === "https:" ? "https://points.boxberry.de/js/boxberry.js" : "http://points.boxberry.ru/js/boxberry.js"), function () {

                      var divgWidgetContainer = $('<div id="pm" style="height:500px;"></div>');
                      Object.keys(ctrl.boxberryWidgetConfigData).map(function (objectKey) {
                          divgWidgetContainer.attr(objectKey, ctrl.boxberryWidgetConfigData[objectKey]);
                      });
                      window.isBoxberryLoaded = true;
                  });

              } else {

              }
          };

          ctrl.boxberryCallback = function (points) {
              if (typeof (delivery) === 'string') {
                  delivery = (new Function("return " + delivery))();
              }

              /*
              address:"432005, Ульяновск г, Московское ш, д.100"
              id:"73041"
              loadlimit:"15"
              name:"Ульяновск"
              period:"3"
              phone:"8-800-222-80-00"
              prepaid:"0"
              price:"540.28"
              workschedule:"пн-пт:09.00-19.00, сб-вс:10.00-16.00"
              zip:"432005"
              */

              var additionalData = {
                  address: points.address,
                  code: points.id,
                  name: points.name,
                  deliveryPeriod: points.period,
                  phone: points.phone,
                  price: points.price,
                  workSchedule: points.workschedule

              };

              ctrl.boxberryDeliveryShipping.PickpointId = points.id;
              ctrl.boxberryDeliveryShipping.PickpointAddress = points.address;
              ctrl.boxberryDeliveryShipping.PickpointAdditionalData = JSON.stringify(additionalData);
              ctrl.boxberryDeliveryShipping.Rate = points.price;
              ctrl.boxberryDeliveryShipping.DeliveryTime = points.period == "" ? "" : points.period + "дн.";
              ctrl.boxberryDeliveryCost = points.price;

              ctrl.boxberryDeliveryCallback({ event: 'boxberryDelivery', field: ctrl.boxberryDeliveryShipping.PickpointId || 0, shipping: ctrl.boxberryDeliveryShipping });

              $scope.$digest();
          };

          ctrl.boxberryOpenWidget = function () {
              boxberry.open(ctrl.boxberryCallback,
                  ctrl.boxberryWidgetConfigData.api_token,
                  ctrl.boxberryWidgetConfigData.custom_city,
                  ctrl.boxberryWidgetConfigData.target_start,
                  ctrl.boxberryWidgetConfigData.ordersum,
                  ctrl.boxberryWidgetConfigData.weight,
                  ctrl.boxberryWidgetConfigData.paysum,
                  ctrl.boxberryWidgetConfigData.height,
                  ctrl.boxberryWidgetConfigData.width,
                  ctrl.boxberryWidgetConfigData.depth);
          };
      }])

      .directive('boxberry', ['urlHelper', function (urlHelper) {
          return {
              scope: {
                  boxberryDeliveryShipping: '=',
                  boxberryCallback: '&',
                  boxberryDeliveryCallback: '&',
                  boxberryOpenWidget: '&',
                  boxberryWidgetConfigData: '=',
                  boxberryIsSelected: '=',
                  boxberryContact: '=',

                  boxberryDeliveryAmount: '=',
                  boxberryDeliveryWeight: '=',
                  boxberryDeliveryCost: '=',
                  boxberryDeliveryDimensions: '=',

              },
              controller: 'BoxberryCtrl',
              controllerAs: 'boxberry',
              bindToController: true,
              templateUrl: function () {
                  return urlHelper.getAbsUrl('scripts/_partials/shipping/extend/boxberry/boxberry.tpl.html', true);
              },
              link: function (scope, element, attrs, ctrl) {
                  ctrl.init();
              }
          }
      }])

})(window.angular);