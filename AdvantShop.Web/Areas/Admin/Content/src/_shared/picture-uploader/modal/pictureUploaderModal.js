; (function (ng) {
    'use strict';

    var ModalPictureUploaderCtrl = function ($uibModalInstance, $window, $timeout) {
        var ctrl = this;

        ctrl.save = function (url) {
            $uibModalInstance.close(url);
        };

        ctrl.dismiss = function () {
            $uibModalInstance.dismiss('cancel');
        };
        //to do: ngModel не обновляется если мы вставили текст в поле через контекстное меню мышкой
        ctrl.pasteUrl = function ($event) {
            var paste = $event.originalEvent.clipboardData && $event.originalEvent.clipboardData.getData ?
                $event.originalEvent.clipboardData.getData('text/plain') :                // Standard
                $window[0].clipboardData && $window[0].clipboardData.getData ?
                $window[0].clipboardData.getData('Text') :                 // MS
                null;

            $timeout(function () {
                ctrl.url = paste;
            });
        }
    };

    ModalPictureUploaderCtrl.$inject = ['$uibModalInstance', '$window', '$timeout'];

    ng.module('uiModal')
        .controller('ModalPictureUploaderCtrl', ModalPictureUploaderCtrl);

})(window.angular);