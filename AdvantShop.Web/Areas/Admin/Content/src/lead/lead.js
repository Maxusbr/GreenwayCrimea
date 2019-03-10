; (function (ng) {
    'use strict';
    var LeadCtrl = function (uiGridCustomConfig, $http, toaster, $timeout, $uibModal, $q, SweetAlert, $window, Upload) {

        var ctrl = this;

        ctrl.init = function (leadId) {
            ctrl.leadId = leadId;

            ctrl.getAttachments();
        }

        ctrl.gridLeadItemsOptions = ng.extend({}, uiGridCustomConfig, {
            rowHeight: 90,
            columnDefs: [
                {
                    name: 'ImageSrc',
                    displayName: '',
                    cellTemplate: '<div class="ui-grid-cell-contents"><img class="ui-grid-custom-col-img" ng-src="{{row.entity.ImageSrc}}"></div>',
                    width: 100,
                    enableSorting: false,
                },
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents ui-grid-cell-contents-order-items">' +
                            '<div ng-if="row.entity.ProductLink != null"><a href="{{row.entity.ProductLink}}" target="_blank">{{row.entity.Name}}</a></div> ' +
                            '<div ng-if="row.entity.ProductLink == null">{{row.entity.Name}}</div> ' +

                            '<div class="order-item-artno">Артикул: {{row.entity.ArtNo}}</div> ' +
                            '<div ng-if="row.entity.Color != null && row.entity.Color.length > 0">{{row.entity.Color}}</div>' +
                            '<div ng-if="row.entity.Size != null && row.entity.Size.length > 0">{{row.entity.Size}}</div>' +
                        '</div>',
                },
                {
                    name: 'Price',
                    displayName: 'Цена',
                    enableCellEdit: true,
                    width: 100,
                },
                {
                    name: 'Amount',
                    displayName: 'Кол-во',
                    enableCellEdit: true,
                    width: 80,
                },
                {
                    name: 'Cost',
                    displayName: 'Стоимость',
                    width: 110,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 35,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-grid-custom-delete url="leads/deleteLeadItem" params="{\'LeadId\': row.entity.LeadId, \'leadItemId\': row.entity.LeadItemId }"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ],
        });


        ctrl.gridLeadItemsOnInit = function (grid) {
            ctrl.gridLeadItems = grid;
        };


        ctrl.addLeadItems = function (result) {

            if (result == null || result.ids == null || result.ids.length == 0)
                return;

            $http.post("leads/addLeadItems", { leadId: ctrl.leadId, offerIds: result.ids })
                    .then(function (response) {
                        if (response.data.result === true) {
                            toaster.pop('success', '', 'Товар успешно добавлен');
                        }
                    })
                    .then(ctrl.gridLeadItemUpdate);
        }

        ctrl.gridLeadItemUpdate = function () {
            ctrl.gridLeadItems.fetchData();
            ctrl.leadItemsSummaryUpdate();
        }

        ctrl.gridLeadItemDelete = function () {
            ctrl.leadItemsSummaryUpdate();
        }

        ctrl.initLeadItemsSummary = function (leadItemsSummary) {
            ctrl.leadItemsSummary = leadItemsSummary;
            ctrl.leadItemsSummaryUpdate();
        }

        ctrl.leadItemsSummaryUpdate = function () {
            if (ctrl.leadItemsSummary != null) {
                ctrl.leadItemsSummary.getLeadItemsSummary().then(function(data) {
                    if (data != null) {
                        ctrl.hasProducts = data.ProductsCost > 0;
                        if (ctrl.hasProducts || (data.ProductsCost === 0 && ctrl.ProductsCost !== 0 && ctrl.ProductsCost != null)) {
                            ctrl.sum = data.SumValueFormat;
                            ctrl.ProductsCost = data.ProductsCost;
                        }
                    }
                });
            }
        }

        ctrl.createOrder = function () {
            $http.post('leads/createOrder', { leadId: ctrl.leadId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Заказ успешно создан');
                    $window.location.assign('orders/edit/' + data.orderId);
                } else {
                    toaster.pop('error', '', 'Не удалось создать заказ');
                }
            });
        }

        ctrl.deleteLead = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('leads/deleteLead', { leadId: ctrl.leadId }).then(function (response) {
                        $window.location.assign('leads');
                    });
                }
            });
        }

        /* attachments */
        ctrl.getAttachments = function () {
            $http.get("leadsExt/getAttachments", { params: { leadId: ctrl.leadId } }).then(function (response) {
                ctrl.attachments = response.data;
            });
        }

        ctrl.uploadAttachment = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            ctrl.loadingFiles = true;
            if (($event.type === 'change' || $event.type === 'drop') && $files != null && $files.length > 0) {

                Upload.upload({
                    url: 'leadsExt/uploadAttachments',
                    data: {
                        leadId: ctrl.leadId,
                    },
                    file: $files,
                }).then(function (response) {
                    var data = response.data;
                    for (var i in response.data) {
                        if (data[i].Result === true) {
                            ctrl.attachments.push(data[i].Attachment);
                            toaster.pop('success', '', 'Файл "' + data[i].Attachment.FileName + '" добавлен');
                        }
                        else {
                            toaster.pop('error', 'Ошибка при загрузке', (data[i].Attachment != null ? data[i].Attachment.FileName + ": " : "") + data[i].Error);
                        }
                    }
                    ctrl.loadingFiles = false;
                });
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
                ctrl.loadingFiles = false;
            }
            else {
                ctrl.loadingFiles = false;
            }
        };

        ctrl.deleteAttachment = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить файл?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post("leadsExt/deleteAttachment", { id: id, leadId: ctrl.leadId }).then(function (response) {
                        ctrl.getAttachments();
                    });
                }
            });
        }
        /* end attachments */

        ctrl.leadEventsOnInit = function (leadEvents) {
            ctrl.leadEvents = leadEvents;
        }

    }

    LeadCtrl.$inject = ['uiGridCustomConfig', '$http', 'toaster', '$timeout', '$uibModal', '$q', 'SweetAlert', '$window', 'Upload'];

    ng.module('lead', ['uiGridCustom', 'leadItemsSummary', 'leadEvents'])
      .controller('LeadCtrl', LeadCtrl);

})(window.angular);