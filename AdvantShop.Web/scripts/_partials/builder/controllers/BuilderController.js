; (function (ng) {
    'use strict';

    var BuilderCtrl = function ($window, builderService, builderTypes) {
        var ctrl = this;

        ctrl.colors = [];

        ctrl.showDialog = function () {

            if (ctrl.design == null) {
                builderService.getDesignVariants().then(function (data) {
                    ctrl.design = data;

                    var colorsStorage = ctrl.prepareColors(ctrl.design.Colors, ctrl.design.DesignCurrent.ColorScheme);

                    ctrl.colors = ctrl.colors.concat(colorsStorage.colors);
                    ctrl.colorSelected = colorsStorage.colorSelected;
                });
            }

            builderService.dialogOpen(ctrl);

        };

        ctrl.prepareColors = function (colors, colorSelectedName) {

            var colorsLength = colors.length,
                colorSelected,
                colorsArray = [];

            for (var i = colorsLength - 1; i >= 0; i--) {
                colorsArray[i] = {
                    Color: colors[i].Name,
                    ColorName: colors[i].Title,
                    ColorId: colors[i].Color,
                    ColorCode: '#' + colors[i].Color
                };

                if (colorsArray[i].Color == colorSelectedName) {
                    colorSelected = colorsArray[i];
                }

            }

            return {
                colors: colorsArray.reverse(),
                colorSelected: colorSelected
            };
        };

        ctrl.changeBackground = function (name) {
            builderService.apply(builderTypes.background, name);

            //reset theme
            ctrl.design.DesignCurrent.Theme = ctrl.design.Themes[0].Name;
            builderService.apply(builderTypes.theme, ctrl.design.Themes[0].Name);
        };

        ctrl.changeTheme = function (name) {
            builderService.apply(builderTypes.theme, name);

            //reset background
            ctrl.design.DesignCurrent.Background = ctrl.design.Backgrounds[0].Name;
            builderService.apply(builderTypes.background, ctrl.design.Backgrounds[0].Name);
        };

        ctrl.changeColor = function (color) {
            builderService.apply(builderTypes.colorScheme, color.Color);
        };

        ctrl.cancel = function () {
            var oldDesign = builderService.return();


            for (var i = ctrl.colors.length - 1; i >= 0; i--){
                if (oldDesign.DesignCurrent.ColorScheme == ctrl.colors[i].ColorName) {
                    ctrl.colorSelected = ctrl.colors[i];
                    break;
                }
            }

            
            builderService.dialogClose();
        };

        ctrl.save = function () {
            builderService.save().then(function (design) {
                if (design.old.DesignCurrent.Structure != design.new.DesignCurrent.Structure) {
                    $window.location.reload();
                } else {
                    builderService.dialogClose();
                }
            });

        };
    };

    ng.module('builder')
    .controller('BuilderCtrl', BuilderCtrl);

    BuilderCtrl.$inject = ['$window','builderService', 'builderTypes'];

})(window.angular);