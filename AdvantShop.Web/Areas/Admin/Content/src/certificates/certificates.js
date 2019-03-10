; (function (ng) {
    'use strict';

    var CertificatesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'CertificateCode',
                    displayName: 'Код сертификата',
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="text-align:left">' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditCertificatesCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/certificates/modal/addEditCertificates/addEditCertificates.html" ' +
                                    'data-resolve="{\'id\': row.entity.CertificateId}" ' +
                                    'data-on-close="grid.appScope.$ctrl.fetchData()" ' +
                                    'data-size="xs-9"> ' +
                                    '<a href="">{{COL_FIELD}}</a>' +
                            '</ui-modal-trigger></div></div>',
                    filter: {
                        placeholder: 'Код сертификата',
                        type: uiGridConstants.filter.INPUT,
                        name: 'CertificateCode'
                    }
                },
                {
                    name: 'OrderId',
                    displayName: 'Номер заказа (приобретен)',
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><a class="link-invert" ng-href="orders/edit/{{row.entity.OrderId}}">{{COL_FIELD}}</a></div>',
                    filter: {
                        placeholder: 'Номер заказа (приобретен)',
                        type: uiGridConstants.filter.INPUT,
                        name: 'OrderId'
                    }
                },
                {
                    name: 'ApplyOrderNumber',
                    displayName: 'Номер заказа (использован)',
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><a class="link-invert" ng-href="orders/edit/{{row.entity.ApplyOrderNumber}}">{{COL_FIELD}}</a></div>',
                    filter: {
                        placeholder: 'Номер заказа (использован)',
                        name: 'ApplyOrderNumber',
                        type: uiGridConstants.filter.INPUT
                    }
                },
                {
                    name: 'FullSum',
                    displayName: 'Сумма',
                    enableCellEdit: false,
                    width:150,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="text-align:left">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Сумма',
                        name: 'Sum',
                        type: uiGridConstants.filter.INPUT
                    }
                },
                {
                    name: 'Paid',
                    displayName: 'Оплачен',
                    enableCellEdit: false,
                    width: 80,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;"><label class="adv-checkbox-label">' +
                        '<input type="checkbox" ng-model="row.entity.Paid" class="adv-checkbox-input control-checkbox" />' +
                        '<span class="adv-checkbox-emul"></span></label></div></div>',
                    filter: {
                        placeholder: 'Оплачен',
                        name: 'Payed',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'Enable',
                    displayName: 'Доступен',
                    enableCellEdit: false,
                    width:80,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;"><label class="adv-checkbox-label">' +
                        '<input type="checkbox" ng-model="row.entity.Enable" class="adv-checkbox-input control-checkbox" />' +
                        '<span class="adv-checkbox-emul"></span></label></div></div>',
                    filter: {
                        placeholder: 'Доступен',
                        name: 'Enable',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'Used',
                    displayName: 'Использован',
                    enableCellEdit: false,
                    width:100,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;"><label class="adv-checkbox-label">' +
                        '<input type="checkbox" ng-model="row.entity.Used" class="adv-checkbox-input control-checkbox" />' +
                        '<span class="adv-checkbox-emul"></span></label></div></div>',
                    filter: {
                        placeholder: 'Использован',
                        name: 'Used',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'CreationDates',
                    displayName: 'Дата создания',
                    width: 120,
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Дата создания',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'CreationDateFrom'
                            },
                            to: {
                                name: 'CreationDateTo'
                            }
                        }
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 90,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditCertificatesCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/certificates/modal/addEditCertificates/addEditCertificates.html" ' +
                                    'data-resolve="{\'id\': row.entity.CertificateId}" ' +
                                    'data-on-close="grid.appScope.$ctrl.fetchData()" ' +
                                    'data-size="xs-9"> ' +
                                    '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil" style="cursor:pointer"></a>' +
                            '</ui-modal-trigger>' +
                            '<a class="link-invert" target="_blank" ng-href="../giftcertificate/print?code={{row.entity.CertificateCode}}" style="cursor:pointer"><span class="fa fa-print"></span></a>' +
                            '<ui-grid-custom-delete url="Certificates/DeleteCertificates" params="{\'Ids\': row.entity.CertificateId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'Certificates/DeleteCertificates',
                        field: 'CertificateId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
    };

    CertificatesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('certificates', ['uiGridCustom', 'urlHelper'])
      .controller('CertificatesCtrl', CertificatesCtrl);

})(window.angular);