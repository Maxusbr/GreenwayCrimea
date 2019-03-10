; (function (ng) {
    'use strict';

    var ModalTemplateSettingsCtrl = function ($uibModalInstance, $http, $q, toaster) {
        var ctrl = this;

        function clone(obj) {
            var copy;

            // Handle the 3 simple types, and null or undefined
            if (null == obj || "object" != typeof obj) return obj;

            // Handle Date
            if (obj instanceof Date) {
                copy = new Date();
                copy.setTime(obj.getTime());
                return copy;
            }

            // Handle Array
            if (obj instanceof Array) {
                copy = [];
                for (var i = 0, len = obj.length; i < len; i++) {
                    copy[i] = clone(obj[i]);
                }
                return copy;
            }

            // Handle Object
            if (obj instanceof Object) {
                copy = {};
                for (var attr in obj) {
                    if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
                }
                return copy;
            }

            throw new Error("Unable to copy obj! Its type isn't supported.");
        }

        ctrl.$onInit = function () {
            $http.get('design/templatesettings', { params: { rnd: Math.random() } }).then(function (response) {

                console.log(clone(response));
                console.log(clone(response.data));
                ctrl.templateSettingsData = response.data;
            });
        };

        ctrl.save = function (settings) {
            $http.post('design/savetemplatesettings', { settings: ctrl.serialize(settings).join(',') }).then(function (response) {
                if (response.data === true) {

                    $uibModalInstance.close('saveTemplateSettings');

                    toaster.pop('success', 'Настройки шаблона успешно сохранены');

                    return response.data;
                } else {
                    return $q.reject();
                }
            }).catch(function () {
                toaster.pop('error', 'Ошибка при сохранении настроек шаблона');
            });
        };

        ctrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.changeSectionName = function (name) {
            ctrl.sectionName = name;
        };

        ctrl.serialize = function (settings) {
            var result = [];

            for (var i = 0, len = settings.Sections.length; i < len; i++){
                for (var j = 0, lenJ = settings.Sections[i].Settings.length; j < lenJ; j++) {
                    result.push(settings.Sections[i].Settings[j].Name + '~' + settings.Sections[i].Settings[j].Value + '~' + settings.Sections[i].Settings[j].DataType);
                }
            }

            return result;
        };
    };

    ModalTemplateSettingsCtrl.$inject = ['$uibModalInstance', '$http', '$q', 'toaster'];

    ng.module('uiModal')
        .controller('ModalTemplateSettingsCtrl', ModalTemplateSettingsCtrl);

})(window.angular);