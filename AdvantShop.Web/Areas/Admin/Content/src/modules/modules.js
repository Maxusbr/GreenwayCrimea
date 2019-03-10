; (function (ng) {
    'use strict';
    var ModulesCtrl = function (modulesService, $http, toaster, $window) {

        var ctrl = this;

        ctrl.gridParams = {};

        ctrl.columns = [];

        ctrl.dataLoaded = false;

        ctrl.onInit = function () {
            ctrl.getModules().then(function () {
                ctrl.filterApply([{ name: 'search', value: ctrl.filterStart }]);

                ctrl.needUpdateModules = ctrl.modulesData.some(function(item){
                    return item.Version != item.CurrentVersion;
                })

            });
            ctrl.filterParams = {};
        };

        ctrl.gridSearchPlaceholder = ctrl.gridSearchPlaceholder || 'Введите текст для поиска';

        ctrl.filterModal = function (value) {

            var result = true;

            for (var key in ctrl.filterParams) {

                if (ctrl.filterParams.hasOwnProperty(key)) {

                    var term = ctrl.filterParams[key].filter.term;

                    if (term != null) {

                        if (ctrl.filterParams[key].filter.type === 'range') {

                            if (term.from != null && term.from > value[key]) {
                                result = false;
                                break;
                            }

                            if (term.to != null && term.to < value[key]) {
                                result = false;
                                break;
                            }

                        } else if (ctrl.filterParams[key].filter.type === 'input') {

                            if (term != null && term !== "") {
                                if (value[key].toLowerCase().indexOf(term.toLowerCase()) === -1) {

                                    if (value["StringId"].toLowerCase().indexOf(term.toLowerCase()) === -1) {
                                        result = false;
                                        break;
                                    }
                                }
                            }

                        } else if (ctrl.filterParams[key].filter.type === 'select') {

                            if (term != null && value[key] !== term) {
                                result = false;
                                break;
                            }
                        }

                    }

                }
            }

            console.log('result');
            console.log(result);

            return result;
        };

        ctrl.filterColumnDefs = [
            {
                filter: {
                    placeholder: 'Название',
                    type: 'input',
                    name: 'Name',
                }
            },
            {
                filter: {
                    name: 'Enabled',
                    placeholder: 'Активность',
                    type: 'select',
                    selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                }
            }
        ];

        ctrl.filterApply = function (params, item) {
            if (params == null) return;

            var obj = params.filter(function (x) { return x.name === "search"; })[0];

            if (obj != null && obj.name === 'search') {
                ctrl.filterParams['Name'] = {
                    filter: {
                        placeholder: 'Название',
                        type: 'input',
                        term: obj.value,
                        name: 'Name',
                    }
                }
            } else if (item != null) {
                //ctrl.filterParams[item.filter.type === 'range' ? item.filter.name : name] = item;
                ctrl.filterParams[item.filter.name] = item;
            }

            ctrl.modulesData = ctrl.modulesMaster.filter(ctrl.filterModal);
        };

        ctrl.filterRemove = function (name, item) {

            if (item.filter.type === 'range') {
                delete ctrl.gridParams[item.filter.rangeOptions.from.name];
                delete ctrl.gridParams[item.filter.rangeOptions.to.name];
            } if (item.filter.type === 'datetime') {
                delete ctrl.gridParams[item.filter.datetimeOptions.from.name];
                delete ctrl.gridParams[item.filter.datetimeOptions.to.name];
            } else {
                delete ctrl.gridParams[name];
            }
        };

        ctrl.getModules = function () {
            return modulesService[ctrl.pageType ? 'getLocalModules' : 'getMarketModules']().then(function (modulesData) {
                ctrl.dataLoaded = true;
                ctrl.modulesMaster = ng.copy(modulesData);
                ctrl.modulesData = ng.copy(modulesData);

                return modulesData;
            });
        };

        ctrl.installModule = function (module) {

            module.isInstalling = true;

            $http.post('modules/installModule', { stringId: module.StringId, id: module.Id, version: module.Version })
                .then(function (response) {
                    if (response.data.result === true) {
                        $window.location = response.data.url;
                        toaster.pop('success', '', 'Модуль установлен');
                    } else {
                        toaster.pop('error', '', 'Ошибка при установке модуля');
                    }
                })
                .finally(function () {
                    module.isInstalling = false;
                });
        }

        ctrl.updateModule = function (module) {

            module.isUpdating = true;

            $http.post('modules/updateModule', { stringId: module.StringId, id: module.Id, version: module.Version })
                .then(function (response) {
                    $window.location.reload(true);
                })
                .finally(function () {
                    module.isUpdating = false;
                });
        }

        ctrl.updateAllModules = function () {
            $http.post('modules/updateAllModules', { modules: ctrl.modulesData })
            .then(function (response) {
                $window.location.reload(true);
            })
            .finally(function () {
                toaster.pop('success', '', 'Модули обновлены');
            });
        }

        ctrl.changeEnabled = function (module) {

            $http.post('modules/changeEnabled', { stringId: module.StringId, enabled: module.Enabled }).then(function (response) {

                if (response.data.result === true) {
                    toaster.pop('success', '', module.Enabled ? 'Модуль активирован' : 'Модуль не активен');
                } else {
                    toaster.pop('error', '', 'Ошибка при изменении активности');
                }
            });
        }

        ctrl.uninstallModule = function (module) {

            $http.post('modules/uninstallModule', { stringId: module.StringId })
                .then(function (response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Модуль удален');

                        $window.location.reload(true);
                    }
                })
        }
    }


    ModulesCtrl.$inject = ['modulesService', '$http', 'toaster', '$window'];

    ng.module('modules', [])
      .controller('ModulesCtrl', ModulesCtrl);

})(window.angular);