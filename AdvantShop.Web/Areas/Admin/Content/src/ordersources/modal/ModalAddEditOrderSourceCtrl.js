; (function (ng) {
    'use strict';

    var ModalAddEditOrderSourceCtrl = function ($uibModalInstance, $http, $filter) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.mode = ctrl.id != 0 ? "edit" : "add";

            ctrl.getTypes().then(function () {

                if (ctrl.mode == "add") {
                    ctrl.type = ctrl.types[0];
                    ctrl.sortOrder = 0;
                } else {
                    ctrl.loadOrderSource(ctrl.id);
                }
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getTypes = function() {
            return $http.get('ordersources/getTypes').then(function (response) {
                ctrl.types = response.data;
            });
        }

        ctrl.loadOrderSource = function (id) {
            $http.get('ordersources/getOrderSource', { params: { id: id } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.name = data.Name;
                    ctrl.main = data.Main;
                    ctrl.type = $filter('filter')(ctrl.types, { value: data.Type }, true)[0];
                    ctrl.sortOrder = data.SortOrder;
                }
            });
        }
        
        ctrl.saveSource = function() {
            var params = {
                id: ctrl.id,
                name: ctrl.name,
                main: ctrl.main,
                type: ctrl.type.value,
                sortOrder: ctrl.sortOrder,
            }
            var url = ctrl.mode == "add" ? 'ordersources/addOrderSource' : 'ordersources/updateOrderSource';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {

                    $uibModalInstance.close();
                }
            });
        }
    };

    ModalAddEditOrderSourceCtrl.$inject = ['$uibModalInstance', '$http', '$filter'];

    ng.module('uiModal')
        .controller('ModalAddEditOrderSourceCtrl', ModalAddEditOrderSourceCtrl);

})(window.angular);