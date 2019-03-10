; (function (ng) {
    'use strict';

    var ReadmoreCtrl = function ($element, readmoreConfig) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.maxHeight = ctrl.maxHeight || readmoreConfig.maxHeight;
            ctrl.moreText = ctrl.moreText || readmoreConfig.moreText;
            ctrl.lessText = ctrl.lessText || readmoreConfig.lessText;
            ctrl.speed = ctrl.speed || readmoreConfig.speed;
        }; 

        ctrl.$onChanges = function (changesObj) {

            ctrl.isActive = ctrl.checkSizes($element);

            ctrl.expanded = ctrl.isActive === true ? readmoreConfig.expanded : true;

            ctrl.text = ctrl.expanded === true ? ctrl.lessText : ctrl.moreText;
        };

        ctrl.switch = function (expanded) {
            if (expanded === true) {
                ctrl.expanded = false;
                ctrl.text = ctrl.moreText;
            } else {
                ctrl.expanded = true;
                ctrl.text = ctrl.lessText;
            }
        };

        ctrl.checkSizes = function ($el) {
            var clone = $el.clone(),
                result = false;

            clone.addClass('readmore-unvisible');

            $el.after(clone);

            result = ctrl.maxHeight < clone[0].querySelector('.readmore-content').offsetHeight;

            clone.remove();

            return result;
        }
    };

    ReadmoreCtrl.$inject = ['$element', 'readmoreConfig']

    ng.module('readmore')
      .controller('ReadmoreCtrl', ReadmoreCtrl);

})(window.angular);