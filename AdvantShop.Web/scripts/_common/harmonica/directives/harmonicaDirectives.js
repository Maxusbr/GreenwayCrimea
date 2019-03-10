; (function (ng) {
    'use strict';

    ng.module('harmonica')
    .directive('harmonica', [function () {
        return {
            restrict: 'EA',
            scope: true,
            controller: 'HarmonicaCtrl',
            controllerAs: 'harmonica',
            bindToController: true,
            compile: function (cElement, cAttrs) {
                cElement.append('<li data-harmonica-tile></li>');

                return function (scope, element, attrs, ctrl) {
                    element.addClass('harmonica-initialized');
                }
            }
        }
    }]);

    ng.module('harmonica')
    .directive('harmonicaItem', function () {
        return {
            require: '^harmonica',
            restrict: 'EA',
            scope: true,
            link: function (scope, element, attrs, ctrl) {
                ctrl.addItem(element, scope);

                scope.$watch('isVisibleInMenu', function (newValue, oldValue) {
                    element[newValue === false ? 'addClass' : 'removeClass']('ng-hide');
                });
            }
        }
    });

    ng.module('harmonica')
    .directive('harmonicaLink', function () {
        return {
            require: '^harmonica',
            restrict: 'EA',
            scope: true,
            link: function (scope, element, attrs, ctrl) {
                ctrl.addLink(element.attr('href'), element.text(), scope);
            }
        }
    });

    ng.module('harmonica')
    .directive('harmonicaTile', ['$window', 'domService', function ($window, domService) {
        return {
            require: ['harmonicaTile', '^harmonica'],
            restrict: 'EA',
            scope: {},
            replace: true,
            controller: 'HarmonicaTileCtrl',
            controllerAs: 'harmonicaTile',
            bindToController: true,
            templateUrl: '/scripts/_common/harmonica/templates/tile.html',
            link: function (scope, element, attrs, ctrls) {

                var harmonicaTile = ctrls[0],
                    harmonica = ctrls[1],
                    index;

                harmonicaTile.links = harmonica.getLinks();

                harmonicaTile.cssClasses = harmonica.getCssClassesForTile();

                harmonica.saveTileScope(harmonicaTile);

                harmonicaTile.isVisibleTile = harmonica.getVisibleTile();

                index = harmonica.calc();

                harmonica.setVisible(index);

                element[0].addEventListener('mouseenter', function(event){
                    harmonicaTile.tileActive(event);
                    scope.$digest();
                });

                element[0].addEventListener('mouseleave', function(event){
                    harmonicaTile.tileDeactive(event);
                    scope.$digest();
                });

                element[0].addEventListener('click', function(event){
                    harmonicaTile.tileClick(event)
                    scope.$digest();
                });

                $window.addEventListener('resize', function () {
                    scope.$apply(function () {
                        var index = harmonica.calc();

                        harmonica.setVisible(index);
                    });
                });
            }
        }
    }]);

})(angular);