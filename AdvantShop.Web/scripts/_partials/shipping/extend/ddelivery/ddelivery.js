; (function (ng) {
    'use strict';

    ng.module('ddelivery', [])
      .controller('DdeliveryCtrl', ['$http', '$scope', 'modalService', function ($http, $scope, modalService) {

          var ctrl = this;

          ctrl.init = function () {
              if (!window.isDdeliveryLoaded) {
                  jQuery.getScript("//sdk.ddelivery.ru/assets/ddelivery.js", function () {

                      var divWidgetContainer = $('<div id="widgetDdelivery" style="height:600px;width:600px;"></div>');

                      modalService.renderModal(
                         "modalDdeliveryWidget", null, divWidgetContainer.prop('outerHTML'), null,
                         {
                             modalClass: 'ddelivery-widget-dialog', callbackOpen: 'ddelivery.DDeliveryWidgetInitModal()'
                         },
                         {
                             ddelivery: {
                                 DDeliveryWidgetInitModal: function () {
                                     ctrl.ddeliveryInitWidget();
                                 }
                             }
                         });
                      window.isDdeliveryLoaded = true;
                  });
              } else {

              }
          };

          /*
          
            city:425
            city_name:"Ульяновск"
            client_price:2611.55
            client_token:"d37226d9b2d72ee45d5371e826da2ced"
            comment:null

            company_name:"DPD e-Book"
            ddelivery_id:null
            id:180402
            info:"Самовывоз, ID пункта:30361, адрес:432001, Россия, г. Ульяновск, ул Крымова, дом 25, компания: DPD e-Book, Ульяновск"
            local_status:null
            npp_option:null
            order_id:180402
            packing_message:null
            packing_paid:false
            packing_price:null
            packing_required:false
            payment_availability:true
            payment_variant:null
            pickup_warehouse:null
            point:"30361"
            shop_refnum:null
            to_email:""
            to_flat:""
            to_house:""
            to_name:""
            to_phone:""
            to_street:""
            type:1
          */
          ctrl.ddeliveryCallback = function (point) {
              if (typeof (delivery) === 'string') {
                  delivery = (new Function("return " + delivery))();
              }
              var additionalData = {
                  Code: point.point,
                  DeliveryTypeId: point.type,
                  Address: point.info,
                  Rate: point.company_info.client_price,
                  DeliveryDate: point.company_info.pickup_date,
                  DeliveryCompanyId: point.company_info.delivery_company                  
              };

              ctrl.ddeliveryDeliveryShipping.PickpointId = point.point;
              ctrl.ddeliveryDeliveryShipping.PickpointAddress = point.info;
              ctrl.ddeliveryDeliveryShipping.PickpointAdditionalData = JSON.stringify(additionalData);
              ctrl.ddeliveryDeliveryShipping.Rate = point.company_info.client_price;
              ctrl.ddeliveryDeliveryCost = point.company_info.client_price;

              ctrl.ddeliveryDeliveryCallback({ event: 'ddeliveryDelivery', field: ctrl.ddeliveryDeliveryShipping.PickpointId || 0, shipping: ctrl.ddeliveryDeliveryShipping });

              $scope.$digest();
          };

          ctrl.ddeliveryInitWidget = function () {              
              DDeliveryWidget.init(
                  "widgetDdelivery",
                  {
                      products: ctrl.ddeliveryWidgetConfigData.products,
                      id: ctrl.ddeliveryWidgetConfigData.id,
                      width: 600,
                      height: 700,
                      env: DDeliveryWidget.ENV_PROD
                  },
                  {
                      price: function (data) {
                          //console.log('price');
                          //console.log(data);
                      },
                      change: function (data) {
                          //console.log('change');
                          //console.log(data);

                          ctrl.ddeliveryCallback(data);
                      }
                  });
          };
      }])

      .directive('ddelivery', ['urlHelper', function (urlHelper) {
          return {
              scope: {
                  ddeliveryDeliveryShipping: '=',
                  ddeliveryCallback: '&',
                  ddeliveryDeliveryCallback: '&',
                  ddeliveryOpenWidget: '&',
                  ddeliveryWidgetConfigData: '=',
                  ddeliveryIsSelected: '=',
                  ddeliveryContact: '=',

                  ddeliveryDeliveryAmount: '=',
                  ddeliveryDeliveryWeight: '=',
                  ddeliveryDeliveryCost: '=',
                  ddeliveryDeliveryDimensions: '=',

              },
              controller: 'DdeliveryCtrl',
              controllerAs: 'ddelivery',
              bindToController: true,
              templateUrl: function () {
                  return urlHelper.getAbsUrl('scripts/_partials/shipping/extend/ddelivery/ddelivery.tpl.html', true);
              },
              link: function (scope, element, attrs, ctrl) {
                  ctrl.init();
              }
          }
      }])

})(window.angular);