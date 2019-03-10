; (function (ng) {
    'use strict';

    var builderService = function ($http, $q, modalService, builderTypes, $translate) {
        var service = this,
            isRenderDialog = false,
            builderLinks = {
                'background': {
                    element: undefined,
                    pattern: undefined
                },
                'theme': {
                    element: undefined,
                    pattern: undefined
                },
                'colorScheme': {
                    element: undefined,
                    pattern: undefined
                }
            },
            designVariantsBackup = {},
            designVariants;

        service.getDesignVariants = function () {
            return ng.isDefined(designVariants) ? $q.when(designVariants) : service.requestDesignVariants();
        };

        service.requestDesignVariants = function () {
            return $http.get('common/getdesign').then(function (response) {

                designVariants = response.data;
                designVariantsBackup = ng.copy(designVariants);

                return designVariants;
            });
        };

        service.dialogOpen = function (ctrl) {
            if (isRenderDialog === false) {

                modalService.renderModal('modalBuilder', $translate.instant('Js.Builder.DesignTransformer'), '<div ng-include="\'scripts/_partials/builder/templates/body.html\'"></div>', '<div ng-include="\'scripts/_partials/builder/templates/footer.html\'"></div>', { 'isOpen': true, 'modalOverlayClass': 'builder-dialog', 'backgroundEnable': false, 'isFloating': true }, { builder: ctrl });

                isRenderDialog = true;

            } else {
                modalService.open('modalBuilder');
            }
        };

        service.dialogClose = function () {
            modalService.close('modalBuilder');
        };

        service.apply = function (type, param) {
            switch (type) {
                case builderTypes.background:
                    builderLinks.background.element.attr('href', builderLinks.background.pattern.replace('{name}', param));
                    break;
                case builderTypes.theme:
                    builderLinks.theme.element.attr('href', builderLinks.theme.pattern.replace('{name}', param));
                    break;
                case builderTypes.colorScheme:
                    builderLinks.colorScheme.element.attr('href', builderLinks.colorScheme.pattern.replace('{name}', param));
                    designVariants.DesignCurrent.ColorScheme = param;
                    break;
            }

            return designVariants;
        };

        service.save = function () {

            var response = {
                    'old': ng.copy(designVariantsBackup),
                    'new': ng.copy(designVariants)
            };

            designVariantsBackup = ng.copy(designVariants);

            return $http.post('common/savedesign', {
                background: designVariants.DesignCurrent.Background,
                theme: designVariants.DesignCurrent.Theme,
                colorscheme: designVariants.DesignCurrent.ColorScheme,
                structure: designVariants.DesignCurrent.Structure
            }).then(function () {
                return response;
            });
        };

        service.return = function () {
            ng.extend(designVariants, ng.copy(designVariantsBackup));

            service.apply(builderTypes.background, designVariants.DesignCurrent.Background);
            service.apply(builderTypes.theme, designVariants.DesignCurrent.Theme);
            service.apply(builderTypes.colorScheme, designVariants.DesignCurrent.ColorScheme);

            return designVariants;
        };

        service.memoryStylesheet = function (type, element) {
            switch (type) {
                case builderTypes.background:
                    builderLinks.background.element = element;
                    builderLinks.background.pattern = element[0].href.replace(/\/backgrounds\/([\d\w\s_-]*)\//, function (str, group, offset, source) {
                        return str.replace(group, '{name}');
                    });
                    break;
                case builderTypes.theme:
                    builderLinks.theme.element = element;
                    builderLinks.theme.pattern = element[0].href.replace(/\/themes\/([\d\w\s_-]*)\//, function (str, group, offset, source) {
                        return str.replace(group, '{name}');
                    });
                    break;
                case builderTypes.colorScheme:
                    builderLinks.colorScheme.element = element;
                    builderLinks.colorScheme.pattern = element[0].href.replace(/\/colors\/([\d\w\s_-]*)\//, function (str, group, offset, source) {
                        return str.replace(group, '{name}');
                    });
                    break;
            }
        };
    };

    ng.module('builder')
      .service('builderService', builderService);

    builderService.$inject = ['$http', '$q', 'modalService', 'builderTypes', '$translate'];
})(window.angular);