; (function (ng) {
    'use strict';

    var MenuItemActionsCtrl = function ($http, toaster, SweetAlert) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
        };

        ctrl.editMenuItem = function () {
            toaster.pop('success', '', 'Изменения успешно сохранены');
            ctrl.menuTreeviewCtrl.treeRefresh();
        }

        ctrl.deleteMenuItem = function () {

            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {

                    $http.post('menus/deleteMenuItem', { menuItemId: ctrl.id, menuType: ctrl.type }).then(function (response) {
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                        ctrl.menuTreeviewCtrl.treeRefresh();
                    });
                }
            });
        }

    };

    MenuItemActionsCtrl.$inject = ['$http', 'toaster', 'SweetAlert'];
    
    ng.module('menus')
        .controller('MenuItemActionsCtrl', MenuItemActionsCtrl)
        .component('menuItemActions', {
            require: {
                menuTreeviewCtrl: '^menuTreeview'
            },
            templateUrl: '../areas/admin/content/src/menus/components/menu-item-actions/templates/menuItemActions.html',
            controller: 'MenuItemActionsCtrl',
            bindings: {
                id: '@',
                type: '@'
            }
        });

})(window.angular);
