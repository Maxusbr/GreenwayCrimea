; (function (ng) {
    'use strict';

    ng.module('bonus')
      .directive('bonusWhatToDo', function () {
          return {
              restrict: 'A',
              scope: {
                  page: '@',
                  autorizeBonus: '&',
                  changeBonus: '&',
                  email: '=',
                  city: '=',
                  outsideName: '=?',
                  outsideSurname: '=?',
                  outsidePhone: '=?',
                  outsidePatronymic: '=?',
                  isShowPatronymic: '&',
                  isApply: '&',
                  bonusPlus: '@'
              },
              controller: 'BonusWhatToDoCtrl',
              controllerAs: 'bonusWhatToDo',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/bonus/templates/whatToDo.html'
          };
      });

    ng.module('bonus')
      .directive('bonusAuth', function () {
          return {
              restrict: 'A',
              scope: {
                  page: '=',
                  callbackSuccess: '&',
                  outsidePhone: '=?'
              },
              controller: 'BonusAuthCtrl',
              controllerAs: 'bonusAuth',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/bonus/templates/auth.html'
          };
      });

    ng.module('bonus')
        .directive('bonusReg', function () {
            return {
                restrict: 'A',
                scope: {
                    email: '=',
                    city: '=',
                    page: '=',
                    outsideName: '=?',
                    outsideSurname: '=?',
                    outsidePhone: '=?',
                    outsidePatronymic: '=?',
                    isShowPatronymic: '=',
                    callbackSuccess: '&'
                },
                controller: 'BonusRegCtrl',
                controllerAs: 'bonusReg',
                bindToController: true,
                replace: true,
                templateUrl: '/scripts/_partials/bonus/templates/reg.html'
            };
        });

    ng.module('bonus')
    .directive('bonusApply', function () {
        return {
            restrict: 'A',
            replace: true,
            scope: {
                bonusText: '=',
                changeBonus: '&',
                isApply:'='
            },
            controller: 'BonusApplyCtrl',
            controllerAs: 'bonusApply',
            bindToController: true,
            templateUrl: '/scripts/_partials/bonus/templates/apply.html'
        };
    });

    ng.module('bonus')
    .directive('bonusInfo', function () {
        return {
            restrict: 'A',
            replace: true,
            scope: {
                bonusData: '=',
                isShowPatronymic: '='
            },
            controller: 'BonusInfoCtrl',
            controllerAs: 'bonusInfo',
            bindToController: true,
            templateUrl: '/scripts/_partials/bonus/templates/info.html'
        };
    });


    ng.module('bonus')
    .directive('bonusCode', function () {
        return {
            restrict: 'A',
            replace: true,
            scope: {},
            controller: 'BonusCodeCtrl',
            controllerAs: 'bonusCode',
            bindToController: true,
            templateUrl: '/scripts/_partials/bonus/templates/code.html'
        };
    });

})(window.angular);