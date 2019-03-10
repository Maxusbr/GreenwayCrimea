; (function (ng) {
    'use strict';

    var ModalClearDataCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve != null ? ctrl.$resolve.params || {} : {};
            switch (params.mode) {
                case 'catalog':
                    ctrl.deleteCategoties = true;
                    ctrl.deleteProducts = true;
                    ctrl.deleteBrands = true;
                    break;
                default:
                    break;
            }
            
            ctrl.formInited = true;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.delete = function () {
            if (!ctrl.confirm) return;

            var isSelectSome = ctrl.deleteCategoties ||
                ctrl.deleteProducts ||
                ctrl.deleteProperty ||
                ctrl.deleteBrands ||
                ctrl.deleteOrder ||
                ctrl.deleteCustomers ||
                ctrl.deleteSubscription ||
                ctrl.deleteMenu ||
                ctrl.deletePage ||
                ctrl.deleteNews ||
                ctrl.deleteCarosel ||
                ctrl.deleteShippings ||
                ctrl.deletePayments ||
                ctrl.deleteUsers ||
                ctrl.deleteTasks ||
                ctrl.deleteCrm;

            if (!isSelectSome) {
                toaster.pop("info", "", "Выберите элементы");
                return;
            }          

            ctrl.btnSleep = true;

            var params = {
                deleteCategoties: ctrl.deleteCategoties,
                deleteProducts: ctrl.deleteProducts,
                deleteProperty: ctrl.deleteProperty,
                deleteBrands: ctrl.deleteBrands,
                deleteOrder: ctrl.deleteOrder,
                deleteCustomers: ctrl.deleteCustomers,
                deleteSubscription: ctrl.deleteSubscription,
                deleteMenu: ctrl.deleteMenu,
                deletePage: ctrl.deletePage,
                deleteNews: ctrl.deleteNews,
                deleteCarosel: ctrl.deleteCarosel,
                deleteShippings: ctrl.deleteShippings,
                deletePayments: ctrl.deletePayments,
                deleteUsers: ctrl.deleteUsers,
                deleteTasks: ctrl.deleteTasks,
                deleteCrm: ctrl.deleteCrm,
                rnd: Math.random()
            };

            var url = 'settings/clearData';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "",  "Указанные данные удалены");
                    $uibModalInstance.close();
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при удаление данных");
                    ctrl.btnSleep = false;
                }
            }, function (respose) {
                toaster.pop("error", "Ошибка", "Ошибка при удаление данных");
            });
        }
    };

    ModalClearDataCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalClearDataCtrl', ModalClearDataCtrl);

})(window.angular);