; (function (ng) {

    'use strict';

    var BlocksConstructorCtrl = function ($window, modalService, blocksConstructorService, Upload) {

        var ctrl = this;

        ctrl.modalSettingsBlockData = {};
        ctrl.modalAddSubblockData = {};

        ctrl.addBlock = function () {

            var parentData = {
                modalData: {
                    landingpageId: ctrl.landingpageId,
                    blockId: ctrl.blockId,
                    name: ctrl.name,
                    type: ctrl.type,
                    sortOrder: ctrl.sortOrder,
                    settings: ctrl.settings,
                    onApplyNewBlock: ctrl.onApplyNewBlock
                }
            };

            if (modalService.hasModal('modalNewBlock') === false) {
                modalService.renderModal('modalNewBlock', 'Добавить новый блок', '<blocks-constructor-modal-new-block data-modal-data="modalData" data-on-apply="modalData.onApplyNewBlock(blockName, sortOrder)" />', null, { modalClass: 'blocks-constructor-modal' }, parentData);
            }

            modalService.getModal('modalNewBlock').then(function (modal) {
                modal.modalScope.open();
            });
        };

        ctrl.onApplyNewBlock = function (blockName, sortOrder) {
            blocksConstructorService.addBlock(ctrl.landingpageId, blockName, sortOrder).then(function (response) {
                if (response.result === true) {
                    $window.location.reload();
                } else {
                    alert('Ошибка при добавлении блока!');
                }
            });
        };

        ctrl.addElement = function () {
            var modalId = 'modalAddSubblock_' + ctrl.blockId;

            ctrl.modalAddSubblockData.data = {
                modalData: {
                    landingpageId: ctrl.landingpageId,
                    blockId: ctrl.blockId,
                    name: ctrl.name,
                    type: ctrl.type,
                    sortOrder: ctrl.sortOrder,
                    settings: ctrl.settings,
                    templateUrlByType: 'areas/landing/frontend/blocks/blocks-constructor/templates/subblock/' + ctrl.type + '.html'
                }
            };

            if (modalService.hasModal(modalId) === false) {
                modalService.renderModal(modalId,
                    'Добавить элемент',
                    '<blocks-constructor-modal-add-subblock data-modal-data="modalData" data-on-apply="modalData.onApplySettings(blockId, settings)" data-on-cancel="modalData.onCancel()" />',
                    null,
                    { modalClass: 'blocks-constructor-modal', modalOverlayClass: 'blocks-constructor-modal-floating-wrap', isFloating: true, backgroundEnable: false },
                    ctrl.modalAddSubblockData.data);
            }

            modalService.getModal(modalId).then(function (modal) {
                modal.modalScope.open();

                ctrl.modalAddSubblockData.backup = ng.copy(ctrl.modalAddSubblockData.data);
            });

        };

        ctrl.showOptionsBlock = function () {

            var modalId = 'modalSettingsBlock_' + ctrl.blockId;

            ctrl.modalSettingsBlockData.data = {
                modalData: {
                    landingpageId: ctrl.landingpageId,
                    blockId: ctrl.blockId,
                    name: ctrl.name,
                    type: ctrl.type,
                    sortOrder: ctrl.sortOrder,
                    settings: ctrl.settings,
                    templateUrlByType: 'areas/landing/frontend/blocks/blocks-constructor/templates/blocks/' + ctrl.type + '.html',
                    uploadFileBackground: ctrl.uploadFileBackground,
                    onApplySettings: ctrl.onApplySettings,
                    onUpdateBackground: ctrl.onUpdateBackground,
                    onUploadFileBackground: ctrl.onUploadFileBackground,
                    onUpdateFileBackground: ctrl.onUpdateFileBackground,
                    onDeleteFileBackground: ctrl.onDeleteFileBackground,
                    onCancel: ctrl.modalSettingsBlockCancel
                }
            };

            if (modalService.hasModal(modalId) === false) {
                modalService.renderModal(modalId,
                    'Настройки блока',
                    '<blocks-constructor-modal-settings-block data-modal-data="modalData" data-on-apply="modalData.onApplySettings(blockId, settings)" data-on-upload-file-background="modalData.onUploadFileBackground($file)" data-on-update-file-background="modalData.onUpdateFileBackground($file)" data-on-delete-file-background="modalData.onDeleteFileBackground()" data-on-cancel="modalData.onCancel()" />',
                    null,
                    { modalClass: 'blocks-constructor-modal', modalOverlayClass: 'blocks-constructor-modal-floating-wrap', isFloating: true, backgroundEnable: false },
                    ctrl.modalSettingsBlockData.data);
            }

            modalService.getModal(modalId).then(function (modal) {
                modal.modalScope.open();

                ctrl.modalSettingsBlockData.backup = ng.copy(ctrl.modalSettingsBlockData.data);
            });
        };

        ctrl.onApplySettings = function (blockId, settings) {
            blocksConstructorService.saveBlockSettings(blockId, JSON.stringify(settings)).then(function (response) {
                if (response.result === true) {
                    $window.location.reload();
                } else {
                    alert('Ошибка при применении настроек!');
                }
            });
        };

        ctrl.onUpdateBackground = function (cssString) {

            ctrl.settings.background_color = cssString;

            ctrl.settings.style.background = blocksConstructorService.getBackgroundString(cssString, ctrl.settings.background_image);

            ctrl.blocksConstructorContainerCtrl.updateBackgroundContainer(ctrl.settings.style.background);
        }

        ctrl.onUploadFileBackground = function ($file) {

            if ($file == null) {
                return;
            }

            blocksConstructorService.uploadPicture(ctrl.landingpageId, ctrl.blockId, $file).then(function (result) {
                if (result.Result === false) {
                    alert('Ошибка при загрузки картинки: ' + result.Error);
                } else {
                    ctrl.settings.background_image = result.Picture;

                    ctrl.settings.style.background = blocksConstructorService.getBackgroundString(ctrl.settings.background_color, ctrl.settings.background_image);

                    ctrl.blocksConstructorContainerCtrl.updateBackgroundContainer(ctrl.settings.style.background);

                    blocksConstructorService.saveBlockSettings(ctrl.blockId, JSON.stringify(ctrl.settings)).then(function (response) {
                        if (response.result === false) {
                            alert('Ошибка при применении настроек!');
                        }
                    });
                }
            });
        };

        ctrl.onUpdateFileBackground = function ($file) {

            if ($file == null) {
                return;
            }

            blocksConstructorService.updatePicture(ctrl.landingpageId, ctrl.blockId, $file, ctrl.settings.background_image).then(function (result) {
                if (result.Result === false) {
                    alert('Ошибка при загрузки картинки: ' + result.Error);
                } else {
                    ctrl.settings.background_image = result.Picture;

                    ctrl.settings.style.background = blocksConstructorService.getBackgroundString(ctrl.settings.background_color, ctrl.settings.background_image);

                    ctrl.blocksConstructorContainerCtrl.updateBackgroundContainer(ctrl.settings.style.background);

                    blocksConstructorService.saveBlockSettings(ctrl.blockId, JSON.stringify(ctrl.settings)).then(function (response) {
                        if (response.result === false) {
                            alert('Ошибка при применении настроек!');
                        }
                    });
                }
            });
        };

        ctrl.onDeleteFileBackground = function () {
            blocksConstructorService.removePicture(ctrl.landingpageId, ctrl.blockId, ctrl.settings.background_image).then(function (result) {
                if (result.Result === false) {
                    alert('Ошибка при удалении картинки: ' + result.Error);
                } else {
                    ctrl.settings.background_image = null;

                    ctrl.settings.style.background = blocksConstructorService.getBackgroundString(ctrl.settings.background_color, ctrl.settings.background_image);

                    ctrl.blocksConstructorContainerCtrl.updateBackgroundContainer(ctrl.settings.style.background);

                    blocksConstructorService.saveBlockSettings(ctrl.blockId, JSON.stringify(ctrl.settings)).then(function (response) {
                        if (response.result === false) {
                            alert('Ошибка при применении настроек!');
                        }
                    });
                }
            });
        };

        ctrl.modalSettingsBlockCancel = function () {
            ctrl.modalSettingsBlockData.data.modalData.settings = ng.extend(ctrl.modalSettingsBlockData.data.modalData.settings, ctrl.modalSettingsBlockData.backup.modalData.settings);
            ctrl.onUpdateBackground(ctrl.modalSettingsBlockData.data.modalData.settings.style.background);
        };

        ctrl.moveUpBlock = function () {
            blocksConstructorService.saveBlockSortOrder(ctrl.blockId, true).then(function (response) {
                if (response.result === true) {
                    $window.location.reload();
                } else {
                    alert('Ошибка при перемещении блока вверх!');
                }
            });
        };

        ctrl.moveDownBlock = function () {
            blocksConstructorService.saveBlockSortOrder(ctrl.blockId, false).then(function (response) {
                if (response.result === true) {
                    $window.location.reload();
                } else {
                    alert('Ошибка при перемещении блока вниз!');
                }
            });
        };

        ctrl.removeBlock = function () {
            blocksConstructorService.removeBlock(ctrl.blockId).then(function (response) {
                if (response.result === true) {
                    $window.location.reload();
                } else {
                    alert('Ошибка при удалении блока!');
                }
            });
        };
    };

    ng.module('blocksConstructor')
      .controller('BlocksConstructorCtrl', BlocksConstructorCtrl);

    BlocksConstructorCtrl.$inject = ['$window', 'modalService', 'blocksConstructorService', 'Upload'];

})(window.angular);