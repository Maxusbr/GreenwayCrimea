; (function (ng) {
    'use strict';

    ng.module('pickpoint', [])
      .controller('PickpointCtrl', ['$scope', function ($scope) {

          var ctrl = this,
              _callback;

          var pickpointCallback = function (result) {
              ctrl.shipping.PickpointId = result.id;
              ctrl.shipping.PickpointAddress = result.name + ', ' + result.address;
              //эта функция вызывается вне контекста ангуляра, поэтому нужно вызвать $digest
              _callback('pickpointSelect', ctrl.shipping.PickpointId);
              $scope.$digest();
          };

          ctrl.open = function (shipping, callback) {
              _callback = callback;
              ctrl.shipping = shipping;
              PickPoint.open(pickpointCallback, { city: shipping.Pickpointmap, ids: null });
          };
      }])

})(window.angular);