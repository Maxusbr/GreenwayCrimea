; (function (ng) {
    'use strict';

    ng.module('grastin', [])
      .controller('GrastinCtrl', ['$scope', 'modalService', function ($scope, modalService) {

          var ctrl = this;

          ctrl.init = function () {
              if (!window.isGrastinLoaded) {
                  jQuery.getScript((window.location.protocol === "https:" ? "https:" : "http:") + "//grastin.ru/js/gWidget.js", function () {

                      var divgWidgetContainer = $('<div id="gWidget" style="height:500px;"></div>');
                      Object.keys(ctrl.grastinWidgetConfigData).map(function (objectKey) {
                          divgWidgetContainer.attr(objectKey, ctrl.grastinWidgetConfigData[objectKey]);
                      });

                      modalService.renderModal(
                          "modalGrastinWidget", null, divgWidgetContainer.prop('outerHTML'), null,
                          { modalClass: 'grastin-widget-dialog', callbackInit: 'grastin.GrastinWidgetInitModal()' },
                          {
                              grastin: {
                                  GrastinWidgetInitModal: function () {
                                      gwClient.createWidgets();
                                  }
                              }
                          });

                      window.grastinPvzWidgetCallback = ctrl.setGrastinPvz;
                      gwClient.onMessage = function(event) {
                          if (event.origin.indexOf(this.o.host) != -1) {
                              var i = "object" == typeof event.data ? event.data : JSON.parse(event.data), a = Object.keys(i)[0];
                              switch (a) {
                              case "delivery":
                                  "function" == typeof window[this.o.callbackName] && window[this.o.callbackName](i[a]);
                              }
                          }
                      };
                      window.isGrastinLoaded = true;
                  });

              } else {

              }
          }

          ctrl.setGrastinPvz = function (delivery) {
              if (typeof (delivery) === 'string') {
                  delivery = (new Function("return " + delivery))();
              }

              var closeModal = true;
              //                                                Если почта, то не обязательно данные по точке самовыова (их нет)
              if (delivery.cost <= 0 || !delivery.partnerId || (delivery.partnerId !== 'post' && delivery.deliveryType === 'pvz' && (!delivery.currentId || !delivery.pvzData)))
                  return;

              var deliveryClone = JSON.parse(JSON.stringify(delivery));

              if (delivery.deliveryType === 'courier') {
                  deliveryClone.deliveryType = 1;
              } else if (delivery.deliveryType === 'pvz') {
                  deliveryClone.deliveryType = 2;
              }

              var company = '';
              if (delivery.partnerId === 'grastin') {
                  company = 'Grastin';
                  deliveryClone.partner = 1;

              } else if (delivery.partnerId === 'dpd') {
                  company = 'DPD';
                  deliveryClone.partner = 5;

              } else if (delivery.partnerId === 'hermes') {
                  company = 'Hermes';
                  deliveryClone.partner = 2;

              } else if (delivery.partnerId === 'boxberry') {
                  company = 'BoxBerry';
                  deliveryClone.partner = 4;

              } else if (delivery.partnerId === 'post') {
                  company = 'Почта РФ';
                  deliveryClone.partner = 3;

              } else if (delivery.partnerId) {
                  company = delivery.partnerId;
              }

              if (delivery.partnerId === 'post') {
                  // Особый случай с почтой
                  ctrl.grastinShipping.PickpointId = delivery.partnerId;
                  ctrl.grastinShipping.PickpointAddress = delivery.cityTo;
                  ctrl.grastinShipping.PickpointAdditionalData = JSON.stringify(deliveryClone);
                  ctrl.grastinShipping.NameRate = 'Самовывоз ' + company;
                  ctrl.grastinShipping.Rate = delivery.cost;
                  ctrl.grastinShipping.DeliveryTime = '';//(delivery.time && delivery.time > 0 ? delivery.time + " д." : "");
              }
              else if (delivery.deliveryType === 'pvz') {
                  ctrl.grastinShipping.PickpointId = delivery.currentId;
                  ctrl.grastinShipping.PickpointAddress = delivery.pvzData.name;
                  ctrl.grastinShipping.PickpointAdditionalData = JSON.stringify(deliveryClone);
                  ctrl.grastinShipping.NameRate = 'Самовывоз ' + company;
                  ctrl.grastinShipping.Rate = delivery.cost;
                  ctrl.grastinShipping.DeliveryTime = '';//(delivery.time && delivery.time > 0 ? delivery.time + " д." : "");
              } else {
                  ctrl.grastinShipping.PickpointId = delivery.partnerId;
                  ctrl.grastinShipping.PickpointAddress = delivery.cityTo;
                  ctrl.grastinShipping.PickpointAdditionalData = JSON.stringify(deliveryClone);
                  ctrl.grastinShipping.NameRate = 'Курьерская доставка ' + company;
                  ctrl.grastinShipping.Rate = delivery.cost;
                  ctrl.grastinShipping.DeliveryTime = '';//(delivery.time && delivery.time > 0 ? delivery.time + " д." : "");
              }

              ctrl.grastinCallback({ event: 'grastinWidget', field: ctrl.grastinShipping.PickpointId || 0 });
              if (closeModal) {
                  modalService.close("modalGrastinWidget");
              }

              $scope.$digest();
          }

      }])
      .directive('grastin', ['urlHelper', function (urlHelper) {
          return {
              scope: {
                  grastinShipping: '=',
                  grastinCallback: '&',
                  grastinWidgetConfigData: '=',
                  grastinIsSelected: '=',
                  grastinContact: '='
              },
              controller: 'GrastinCtrl',
              controllerAs: 'grastin',
              bindToController: true,
              templateUrl: function () {
                  return 'scripts/_partials/shipping/extend/grastin/grastin.tpl.html';
              },
              link: function (scope, element, attrs, ctrl) {
                  ctrl.init();
              }
          }
      }])

})(window.angular);