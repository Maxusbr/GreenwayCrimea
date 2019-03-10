; (function (ng) {
    'use strict';

    var DesignCtrl = function ($window, $http, toaster, Upload, designService, SweetAlert) {

        var ctrl = this;

        ctrl.templatesProgress = [];

        ctrl.$onInit = function () {
            ctrl.getData();
        };

        ctrl.getData = function () {
            return designService.getDesigns(ctrl.urlParametr()).then(function (designData) {
                return ctrl.designData = designData;
            });
        };

        ctrl.changeDesign = function (designType, name) {

            designService.saveDesign(designType, name).then(function (result) {
                if (result === true) {
                    ctrl.getData().then(function () {
                        toaster.pop('success', '', 'Изменения сохранены');
                    })
                } else {
                    toaster.pop('error', '', 'Ошибка при сохранении дизайна');
                }
            });
        };

        ctrl.addDesign = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event, designType) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                designService.uploadDesign($file, designType)
                    .then(function (result) {

                        if (result === true) {
                            ctrl.getData().then(function () {
                                toaster.pop('success', '', 'Архив успешно загружен');
                            });
                        } else {
                            toaster.pop('error', '', 'Ошибка при загрузке');
                        }
                    })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
            }
        };

        ctrl.deleteDesign = function (designType, designName) {
            SweetAlert.confirm("Вы действительно хотите удалить?", { title: "" }).then(function (result) {
                if (result === true) {
                    designService.deleteDesign(designName, designType).then(function (result) {

                        if (result === true) {
                            ctrl.getData().then(function () {
                                toaster.pop('success', '', 'Успешно удалено');
                            });
                        } else {
                            toaster.pop('error', '', 'Ошибка при удалении');
                        }

                    }, function () {
                        toaster.pop('error', '', 'Ошибка при удалении');
                    });
                }
            });
        };

        ctrl.preview = function (id, previewTemplateId, shopUrl) {

            ctrl.templatesProgress[previewTemplateId] = true;

            designService.previewTemplate(id, previewTemplateId).then(function (result) {
                designService.checkPage(shopUrl).then(function () {
                    if (result === true) {
                        SweetAlert.success('Шаблон готов для предпросмотра', { title: 'Предпросмотр шаблона', showCancelButton: true, confirmButtonText: 'Перейти', cancelButtonText: 'Закрыть' }).then(function (result) {
                            if (result === true) {
                                $window.open(shopUrl);
                            }

                            $window.location.reload(true);
                        });
                    } else {
                        toaster.pop('error', '', 'Ошибка при активации предпросмотра');
                    }

                    ctrl.templatesProgress[previewTemplateId] = false;
                });
            });
        };

        ctrl.resizePictures = function () {
            SweetAlert.confirm("Вы действительно хотите пережать фотографии всех товаров (может занять длительное время)?", { title: "Пережатие фотографий товаров" }).then(function (result) {
                if (result === true) {
                    designService.resizePictures();
                }
            });
        }

        ctrl.installTemplate = function (stringId, id, version) {
            designService.installTemplate(stringId, id, version).then(function (response) {
                if (response.result === true) {
                    toaster.pop('success', '', 'Шаблон установлен');
                    $window.location.reload(true);
                }
                else {
                    toaster.pop('error', '', 'Ошибка при установке шаблона');
                }
            });
        }

        ctrl.updateTemplate = function (id) {
            SweetAlert.confirm("Вы действительно хотите обновить шаблон?", { title: "Обновление шаблона" }).then(function (result) {
                if (result === true) {
                    designService.updateTemplate(id).then(function (response) {
                        if (response.result === true) {
                            toaster.pop('success', '', 'Шаблон обновлен');
                            $window.location.reload(true);
                        }
                        else {
                            toaster.pop('error', '', 'Ошибка при обновлении шаблона');
                        }
                    });
                }
            });
        }

        ctrl.deleteTemplate = function (stringid) {
            SweetAlert.confirm("Вы действительно хотите удалить шаблон?", { title: "Удаление шаблона" }).then(function (result) {
                if (result === true) {
                    designService.deleteTemplate(stringid).then(function (response) {
                        if (response.result === true) {
                            toaster.pop('success', '', 'Шаблон удален');
                            $window.location.reload(true);
                        }
                        else {
                            toaster.pop('error', '', 'Ошибка при удалении шаблона');
                        }
                    });
                }
            });
        }

        ctrl.urlParametr = function() {
            return location.search.split(/[?&]/).slice(1).map(function (paramPair) {
                return paramPair.split(/=(.+)?/).slice(0, 2);
            }).reduce(function (obj, pairArray) {
                obj[pairArray[0]] = pairArray[1];
                return obj;
            }, {});
        }


        // Theme

        ctrl.initTheme = function(show, theme, design) {
            ctrl.show = show;
            ctrl.theme = theme;
            ctrl.design = design;
            ctrl.getFiles();
        }

        ctrl.getFiles = function() {
            $http.post("design/themeFiles", { theme: ctrl.theme, design: ctrl.design, action: 'getfiles' }).then(function (response) {
                ctrl.themeFiles = response.data.files;
            });
        }

        ctrl.removeFile = function(item) {
            $http.post("design/themeFiles", { theme: ctrl.theme, design: ctrl.design, action: 'remove', removeFile: item.Name })
                 .then(ctrl.getFiles)
                 .then(function () {
                    toaster.pop('success', '', 'Изменения сохранены');
                 });
        }

        ctrl.addThemeFile = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {

                Upload.upload({
                    url: 'design/themeFiles',
                    data: {
                        theme: ctrl.theme,
                        design: ctrl.design,
                        themeCss: ctrl.themeCss,
                        action: 'upload',
                        file: $file
                    }
                }).then(function (result) {
                    ctrl.getFiles();
                    if (result.data.result === true) {
                        toaster.pop('success', '', 'Архив успешно загружен');
                    } else {
                        toaster.pop('error', '', 'Ошибка при загрузке');
                    }
                });
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
            }
        };

        ctrl.saveTheme = function() {
            $http.post("design/saveTheme", { theme: ctrl.theme, design: ctrl.design, themeCss: ctrl.themeCss }).then(function (response) {
                if (response.data.result) {
                    toaster.pop('success', '', 'Изменения сохранены');
                } else {
                    toaster.pop('error', '', 'Изменения не сохранены');
                }
            });
        }
        
        $(window).bind('keydown', function (event) {
            if (event.ctrlKey || event.metaKey) {
                switch (String.fromCharCode(event.which).toLowerCase()) {
                    case 's':
                        event.preventDefault();
                        ctrl.saveTheme();
                        break;
                }
            }
        });
    };

    DesignCtrl.$inject = ['$window', '$http', 'toaster', 'Upload', 'designService', 'SweetAlert'];
    ng.module('design', ['magnificPopup']).controller('DesignCtrl', DesignCtrl);

})(window.angular);