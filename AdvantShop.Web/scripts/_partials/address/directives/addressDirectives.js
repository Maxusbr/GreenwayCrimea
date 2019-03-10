; (function (ng) {
    'use strict';

    ng.module('address')
      .directive('addressList', function () {
          return {
              restrict: 'A',
              scope: {
                  type: '@', //change or view
                  initAddressFn: '&',
                  changeAddressFn: '&',
                  saveAddressFn: '&'
              },
              controller: 'AddressListCtrl',
              controllerAs: 'addressList',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/address/templates/list.html'
          };
      });
})(window.angular);