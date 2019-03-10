; (function (ng) {
    'use strict';

    ng.module('uiGridCustom')
        .config(['$provide', 'uiGridConstants', function ($provide, uiGridConstants) {
            $provide.decorator('GridOptions', ['$delegate', function ($delegate) {
                var gridOptions;
                gridOptions = angular.copy($delegate);
                gridOptions.initialize = function (options) {
                    var initOptions,
                        directions = [uiGridConstants.ASC, uiGridConstants.DESC]; //remove state null

                    initOptions = $delegate.initialize(options);


                    for (var i = 0, len = initOptions.columnDefs.length; i < len; i++) {
                        initOptions.columnDefs[i].sortDirectionCycle = directions;
                    }

                    return initOptions;
                };
                return gridOptions;
            }]);

            $provide.decorator('Grid', ['$delegate', '$timeout', function ($delegate, $timeout) {
                $delegate.prototype.renderingComplete = function () {
                    if (angular.isFunction(this.options.onRegisterApi)) {
                        this.options.onRegisterApi(this.api);
                    }
                    this.api.core.raise.renderingComplete(this.api);
                    $timeout(function () {
                        var $viewport = $('.ui-grid-render-container');
                        ['touchstart', 'touchmove', 'touchend', 'keydown', 'wheel', 'mousewheel', 'DomMouseScroll', 'MozMousePixelScroll'].forEach(function (eventName) {
                            $viewport.unbind(eventName);
                        });
                    }.bind(this));
                };
                return $delegate;
            }]);
        }]);

})(window.angular);

