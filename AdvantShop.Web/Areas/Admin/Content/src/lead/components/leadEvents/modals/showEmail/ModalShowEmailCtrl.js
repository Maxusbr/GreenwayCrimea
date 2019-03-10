; (function (ng) {
    'use strict';

    var ModalShowEmailCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve.params;
            ctrl.id = params != null ? params.id : null;
            ctrl.folder = params != null ? params.folder : null;
            ctrl.emailData = params != null ? params.emailData : null;
            ctrl.customerId = params != null ? params.customerId : null;

            if (ctrl.id != null) {
                ctrl.getEmail();

            } else if (ctrl.emailData != null) {

                ctrl.getSendedEmail();
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getEmail = function() {
            $http.get('leadsExt/getEmail', { params: { id: ctrl.id, folder: ctrl.folder } }).then(function (response) {
                ctrl.email = response.data;
                ctrl.addHtmlBody();
            });
        }

        ctrl.getSendedEmail = function () {
            $http.get('leadsExt/getSendedEmail', { params: ctrl.emailData }).then(function (response) {
                if (response.data != null) {
                    ctrl.email = response.data;
                    ctrl.addHtmlBody();
                }
            });
        }

        ctrl.addHtmlBody = function () {

            if (ctrl.email.HtmlBody == null) return;

            // add email in iframe
            var iframe = document.getElementById("emailBody");
            iframe.style.display = "block";

            var doc = iframe.document;
            if (iframe.contentDocument) {
                doc = iframe.contentDocument;
            }
            else if (iframe.contentWindow) {
                doc = iframe.contentWindow.document;
            }
            doc.open();
            doc.writeln(ctrl.email.HtmlBody);
            doc.close();

            // add style to break long words
            var css = '* {word-break: break-word;} html {font-family: arial, sans-serif;font-size: 14px;} pre {white-space: pre-wrap;}',
            head = doc.head || doc.getElementsByTagName('head')[0],
            style = doc.createElement('style');

            style.type = 'text/css';
            if (style.styleSheet) {
                style.styleSheet.cssText = css;
            } else {
                style.appendChild(document.createTextNode(css));
            }
            head.appendChild(style);

            // set iframe height
            setTimeout(function () {
                iframe.style.height = iframe.contentWindow.document.body.offsetHeight + 30 + 'px';
            },
                150);
        }
    };

    ModalShowEmailCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalShowEmailCtrl', ModalShowEmailCtrl);

})(window.angular);