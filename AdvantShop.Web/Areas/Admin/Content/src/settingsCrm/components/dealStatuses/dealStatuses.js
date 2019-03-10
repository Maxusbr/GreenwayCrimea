; (function (ng) {
    'use strict';

    var DealStatusesCtrl = function ($http, toaster, SweetAlert) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.fetch();

            if (ctrl.onInit != null) {
                ctrl.onInit({ items: ctrl });
            }
        };

        ctrl.fetch = function () {
            $http.get('leads/getDealStatuses').then(function (response) {
                ctrl.items = response.data.items || [];
                ctrl.items.push({ Id: null, Name: '', SortOrder: 100 });
            });
        };


        ctrl.sortableOptions = {
            orderChanged: function(event) {
                var id = event.source.itemScope.item.Id,
                    prev = ctrl.items[event.dest.index - 1],
                    next = ctrl.items[event.dest.index + 1];
                
                $http.post('leads/changeDealStatusSorting', {
                    Id: id,
                    prevId: prev != null ? prev.Id : null,
                    nextId: next != null ? next.Id : null
                }).then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения сохранены');
                    }
                });
            }
        };

        
        ctrl.deleteItem = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('leads/deleteDealStatus', { id: id }).then(function (response) {
                        if (response.data.result === true) {
                            ctrl.fetch();
                            toaster.pop('success', '', 'Изменения сохранены');
                        }
                    });
                }
            });
        }

        ctrl.addItem = function() {
            $http.post('leads/addDealStatus', { name: ctrl.newName }).then(function (response) {
                if (response.data.result === true) {
                    ctrl.fetch();
                    ctrl.newName = '';
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        }
    };

    DealStatusesCtrl.$inject = ['$http', 'toaster', 'SweetAlert'];

    ng.module('dealStatuses', ['as.sortable'])
        .controller('DealStatusesCtrl', DealStatusesCtrl)
        .component('dealStatuses', {
            templateUrl: '../areas/admin/content/src/settingsCrm/components/dealStatuses/dealStatuses.html',
            controller: 'DealStatusesCtrl',
            transclude: true,
            bindings: {
                onInit: '&'
            }
        });

})(window.angular);