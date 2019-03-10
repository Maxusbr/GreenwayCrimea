; (function (ng) {
    'use strict';

    var MailSettingsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, toaster) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'FormatName',
                    displayName: 'Название',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'FormatName',
                    }
                },
                {
                    name: 'TypeName',
                    displayName: 'Тип письма',
                    width: 200,
                    filter: {
                        placeholder: 'Тип письма',
                        type: uiGridConstants.filter.SELECT,
                        name: 'MailFormatTypeID',
                        fetch: 'settingsMail/GetMailFormatTypesSelectOptions'
                    }
                },
                {
                    name: 'Enable',
                    displayName: 'Активен',
                    enableCellEdit: false,
                    cellTemplate: '<ui-grid-custom-switch row="row" field-name="Enable" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                    width: 90,
                    filter: {
                        placeholder: 'Активность',
                        name: 'Enable',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    width: 120,
                    enableCellEdit: true,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditMailFormatCtrl\'" controller-as="ctrl" size="lg" ' +
                                        'template-url="../areas/admin/content/src/mailSettings/modal/addEditMailFormat/addEditMailFormat.html" ' +
                                        'data-resolve="{value:{\'MailFormatID\': row.entity.MailFormatID }}"' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +

                            '<ui-grid-custom-delete url="settingsMail/deleteMailFormat" params="{\'mailFormatID\': row.entity.MailFormatID}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'settingsMail/deleteMailFormats',
                        field: 'MailFormatID',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };


        ctrl.deleteMailFormat = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('settingsMail/DeleteMailFormat', { mailFormatID: id }).then(function (response) {
                        $window.location.assign('settings/notifyemails#?notifyTab=3');
                    });
                }
            });
        }


        ctrl.mailFormat = {};

        ctrl.getTypeDescription = function (mailFormatTypeId) {
            $http.get('settingsMail/getTypeDescription', { params: { mailFormatTypeId: mailFormatTypeId } }).then(function (response) {
                if (response.data.result) {
                    ctrl.mailFormat.Description = response.data.message;
                }
                else {
                    toaster.pop("error", "Ошибка", response.data.error);
                }
            });
        }

        ctrl.sendTestMessage = function (params) {
            ctrl.sendingProgress = true;
            $http.post('settingsMailTest/sendTestMessage', params).then(function (response) {
                var data = response.data;

                if (data.result === true) {
                    toaster.pop('success', '', 'Письмо успешно отправлено');

                    ctrl.notifyemails.emailsettings.To = null;
                    ctrl.notifyemails.emailsettings.Subject = null;
                    ctrl.notifyemails.emailsettings.Body = null;

                } else {
                    data.errors.forEach(function(error) {
                        toaster.pop('error', 'Ошибка при отправке письма', error);
                    });
                }
            }).finally(function () {
                ctrl.sendingProgress = false;
            });
        };
    };



    MailSettingsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', 'toaster'];


    ng.module('mailSettings', ['uiGridCustom'])
      .controller('MailSettingsCtrl', MailSettingsCtrl);

})(window.angular);