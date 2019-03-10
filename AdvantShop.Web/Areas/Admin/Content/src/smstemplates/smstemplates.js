; (function (ng) {
    'use strict';

    var SmsTemplatesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, $http, $q, SweetAlert) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'SmsType',
                    displayName: 'Тип шалона',
                    cellTemplate: '<ui-modal-trigger data-controller="\'ModalAddEditSmsTemplateCtrl\'" controller-as="ctrl" size="md" backdrop="static"' +
                              'template-url="../areas/admin/content/src/_shared/modal/bonus/smstemplate/addeditsmstemplate.html"' +
                              'resolve="{params:{ SmsTypeId:row.entity.SmsTypeId}}"><div class="ui-grid-cell-contents ui-grid-custom-pointer"><a ng-href="">{{COL_FIELD}}</a></div></ui-modal-trigger>'
                },
                {
                    name: 'SmsBody',
                    displayName: 'Сообщение',
                    enableCellEdit: false,
                    cellTemplate: '<ui-modal-trigger data-controller="\'ModalAddEditSmsTemplateCtrl\'" controller-as="ctrl" size="md" backdrop="static"' +
                              'template-url="../areas/admin/content/src/_shared/modal/bonus/smstemplate/addeditsmstemplate.html"' +
                              'resolve="{params:{ SmsTypeId:row.entity.SmsTypeId}}"><div class="ui-grid-cell-contents ui-grid-custom-pointer">{{COL_FIELD}}' + '</div></ui-modal-trigger>'
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger on-close="grid.appScope.$ctrl.gridExtendCtrl.gridUpdate();" data-controller="\'ModalAddEditSmsTemplateCtrl\'" controller-as="ctrl" size="md" backdrop="static"' +
                              'template-url="../areas/admin/content/src/_shared/modal/bonus/smstemplate/addeditsmstemplate.html"' +
                              'resolve="{params:{ SmsTypeId:row.entity.SmsTypeId}}"><a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a></ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="smstemplates/deletesmstemplate" params="{\'id\': row.entity.SmsTypeId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            enableSorting: false,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'smstemplates/DeleteSmsTemplateMass',
                        field: 'SmsTypeId',
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

        ctrl.gridUpdate = function () {
            ctrl.grid.fetchData()
        }

        var columnlogDefs = [
                {
                    name: 'Phone',
                    displayName: 'Номер',
                    enableSorting: false,
                    filter: {
                        placeholder: 'Номер',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Phone'
                    }
                },
                {
                    name: 'Body',
                    displayName: 'Сообщение',
                    enableCellEdit: false,
                    enableSorting: false,
                    filter: {
                        placeholder: 'Сообщение',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Body'
                    }
                },
                 {
                     name: 'State',
                     displayName: 'Статус',
                     enableCellEdit: false
                 },
                  {
                      name: 'Created_Str',
                      displayName: 'Создано',
                      enableCellEdit: false
                  },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    //cellTemplate:
                    //    '<div class="ui-grid-cell-contents"><div>' +
                    //        '<ui-modal-trigger data-controller="\'ModalAddEditSmsTemplateCtrl\'" controller-as="ctrl" size="md" backdrop="static"' +
                    //          'template-url="../areas/admin/content/src/_shared/modal/bonus/smstemplate/addeditsmstemplate.html"' +
                    //          'resolve="{params:{ SmsTypeId:row.entity.SmsTypeId}}"><a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a></ui-modal-trigger>' +
                    //        '<ui-grid-custom-delete url="smstemplates/deletesmstemplate" params="{\'id\': row.entity.SmsTypeId}"></ui-grid-custom-delete>' +
                    //    '</div></div>'
                }
        ];

        ctrl.gridlogOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnlogDefs,
            rowHeight: 100
        });

        ctrl.gridlogOnInit = function (gridlog) {
            ctrl.gridlog = gridlog;
        };

        ctrl.deleteSmsTemplate = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('smstemplates/deleteSmsTemplate', { id: id }).then(function (response) {
                        ctrl.gridUpdate();
                    });
                }
            });
        }

    };

    SmsTemplatesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', '$http', '$q', 'SweetAlert'];


    ng.module('smstemplates', ['uiGridCustom', 'urlHelper'])
      .controller('SmsTemplatesCtrl', SmsTemplatesCtrl);

})(window.angular);