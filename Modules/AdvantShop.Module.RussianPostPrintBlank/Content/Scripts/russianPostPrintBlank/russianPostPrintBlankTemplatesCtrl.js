; (function (ng) {
    'use strict';

    var russianPostPrintBlankTemplatesCtrl = function ($http, toaster, $uibModal, SweetAlert, russianPostPrintBlankService) {
        var ctrl = this;

        ctrl.ViewType = "templatesList";

        ctrl.currentTemplate = {};

        ctrl.availableTemplateTypes = {};

        ctrl.editMode = 0;

        ctrl.currentTemplateType = [{
            Name: "Адресный ярлык Ф.7",
            Type: "F7"
        }];

        ctrl.TemplatesList = {};

        ctrl.getTemplates = function () {
            russianPostPrintBlankService.getTemplates().then(function (response) {
                ctrl.TemplatesList = response;

                ctrl.ViewType = "templatesList";
                if (ctrl.TemplatesList.length < 1)
                {
                    ctrl.ViewType = "templatesListEmpty";
                }
            });

        };

        ctrl.$onInit = function () {
            ctrl.getTemplates();
            //var tabs = document.getElementsByClassName('aside-menu-inner aside-menu-name');
            var tabs = document.getElementsByClassName('link-invert');
            [].forEach.call(tabs, function (tab) {
                tab.onclick = function () {
                    ctrl.cancelEdit();
                }
            });
        };

        ctrl.deleteTemplate = function (templateId, name)
        {
            SweetAlert.confirm("Вы уверены, что хотите удалить шаблон бланка?", { title: "Удаление шаблона бланка" }).then(function (result) {
                if (result === true) {
                    if (templateId < 1) {
                        toaster.pop('error', '', 'Не удалось удалить шаблон');
                        return;
                    }

                    russianPostPrintBlankService.deleteTemplate(templateId).then(function (response) {
                        toaster.pop(response == true ? "success" : "error", "", response == true ? "Шаблон '" + name + "' удален" : "Не удалось удалить шаблон");
                        ctrl.getTemplates();
                    });
                }
            });
        }

        ctrl.changeTemplate = function (templateId) {
            russianPostPrintBlankService.getTemplate(templateId).then(function (response) {
                ctrl.currentTemplate = response;
                ctrl.ViewType = "editTemplate";
            });
        };

        ctrl.editTemplate = function (templateId) {
            if (ctrl.currentTemplate.TemplateName == null || ctrl.currentTemplate.TemplateName == undefined || ctrl.currentTemplate.TemplateName == '') {
                toaster.pop('error', '', 'Укажите название шаблона');
                return;
            }

            SweetAlert.confirm("Вы уверены, что хотите изменить шаблон бланка?", { title: "Изменение шаблона бланка" }).then(function (result) {
                if (result === true) {
                    if (templateId < 1) {
                        toaster.pop('error', '', 'Не удалось изменить шаблон');
                        return;
                    }

                    russianPostPrintBlankService.editTemplate(templateId, ctrl.currentTemplate.TemplateName, ctrl.currentTemplate.Content).then(function (response) {
                        toaster.pop(response == true ? "success" : "error", "", response == true ? "Шаблон '" + ctrl.currentTemplate.TemplateName + "' изменен" : "Не удалось изменить шаблон");

                        ctrl.getTemplates();
                        ctrl.setDefault();
                    });
                }
            });
        };

        ctrl.showCreatingTemplate = function () {
            russianPostPrintBlankService.getAvailableTemplateTypes().then(function (response) {
                ctrl.availableTemplateTypes = response;
                ctrl.ViewType = "editTemplate";
                ctrl.editMode = 1;
            });
        };

        ctrl.changeTemplateTypeOfCreate = function () {
            var templateType = ctrl.currentTemplateType;
            if (templateType == null || templateType.Type == null || templateType.Type == "-1") {
                ctrl.setDefault();
                ctrl.editMode = 1;
                return;
            }

            russianPostPrintBlankService.getTemplateType(ctrl.currentTemplateType.Type).then(function (response) {
                ctrl.currentTemplate = response;
            });
        };

        ctrl.createTemplate = function ()
        {
            if (!ctrl.Validate(ctrl.currentTemplateType) || ctrl.currentTemplate.TemplateName == null || ctrl.currentTemplate.TemplateName == undefined || ctrl.currentTemplate.TemplateName == '') {
                toaster.pop('error', '', 'Укажите название создаваемого шаблона');
                return;
            }

            SweetAlert.confirm("Вы уверены, что хотите создать шаблон бланка?", { title: "Создание шаблона бланка" }).then(function (result) {
                if (result === true) {
                    if (ctrl.currentTemplateType == null) {
                        toaster.pop('error', '', 'Не удалось создать шаблон');
                        return;
                    }

                    russianPostPrintBlankService.createTemplate(ctrl.currentTemplateType.Type, ctrl.currentTemplate.TemplateName, ctrl.currentTemplate.Content)
                        .then(function (response) {
                        toaster.pop(response == true ? "success" : "error", "", response == true ? "Шаблон '" + ctrl.currentTemplate.TemplateName + "' создан" : "Не удалось создан шаблон");
                        ctrl.cancelEdit();
                    });
                }
            });
        }
        
        ctrl.Validate = function (templateType) {
            if (templateType == null || templateType.Type == null || templateType.Type == "-1") {
                toaster.pop('error', '', 'Выберите тип создаваемого шаблона');
                return false;
            }
            
            return true;
        };

        ctrl.cancelEdit = function () {
            ctrl.getTemplates();
            ctrl.setDefault();
        };

        ctrl.setDefault = function () {
            ctrl.currentTemplate = {};

            ctrl.editMode = 0;
        };
    };
    
    ng.module('russianPostPrintBlankTemplates', [])
        .controller('russianPostPrintBlankTemplatesCtrl', russianPostPrintBlankTemplatesCtrl)
        .component('russianPostPrintBlankTemplates', {
            templateUrl: '../modules/RussianPostPrintBlank/content/scripts/russianPostPrintBlank/templates/russianPostPrintBlankTemplates.html',
            controller: 'russianPostPrintBlankTemplatesCtrl'
        });
    
    russianPostPrintBlankTemplatesCtrl.$inject = ['$http', 'toaster', '$uibModal', 'SweetAlert', 'russianPostPrintBlankService'];

})(window.angular);