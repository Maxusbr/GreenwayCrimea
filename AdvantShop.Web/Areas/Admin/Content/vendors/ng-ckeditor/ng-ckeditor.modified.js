(function (angular, factory) {
    if (typeof define === 'function' && define.amd) {
        define(['angular', 'ckeditor'], function (angular) {
            return factory(angular);
        });
    } else {
        return factory(angular);
    }
}(angular || null, function (angular) {
    var app = angular.module('ngCkeditor', ['oc.lazyLoad']);
    var $defer, loaded = false;

    //app.run(['$q', '$timeout', function ($q, $timeout) {
    //    $defer = $q.defer();

    //    if (angular.isUndefined(CKEDITOR)) {
    //        throw new Error('CKEDITOR not found');
    //    }
    //    CKEDITOR.disableAutoInline = true;
    //    function checkLoaded() {
    //        if (CKEDITOR.status == 'loaded') {
    //            loaded = true;
    //            $defer.resolve();
    //        } else {
    //            checkLoaded();
    //        }
    //    }
    //    CKEDITOR.on('loaded', checkLoaded);
    //    //$timeout(checkLoaded, 100);
    //}])

    app.directive('ckeditor', ['$timeout', '$q', '$parse', 'ngCkeditorOptions', '$ocLazyLoad', 'urlHelper', function ($timeout, $q, $parse, ngCkeditorOptions, $ocLazyLoad, urlHelper) {
        'use strict';

        return {
            restrict: 'AC',
            require: ['ngModel', '^?form'],
            scope: false,
            link: function (scope, element, attrs, ctrls) {
                var ngModel = ctrls[0];
                var form = ctrls[1] || null;
                var EMPTY_HTML = '',
                    isTextarea = element[0].tagName.toLowerCase() == 'textarea',
                    data = [],
                    isReady = false;

                if (!isTextarea) {
                    element.attr('contenteditable', true);
                }

                var onLoad = function () {
                    //var options = {
                    //    toolbar: 'full',
                    //    toolbar_full: [
                    //        { name: 'basicstyles',
                    //            items: [ 'Bold', 'Italic', 'Strike', 'Underline' ] },
                    //        { name: 'paragraph', items: [ 'BulletedList', 'NumberedList', 'Blockquote' ] },
                    //        { name: 'editing', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock' ] },
                    //        { name: 'links', items: [ 'Link', 'Unlink', 'Anchor' ] },
                    //        { name: 'tools', items: [ 'SpellChecker', 'Maximize' ] },
                    //        '/',
                    //        { name: 'styles', items: [ 'Format', 'FontSize', 'TextColor', 'PasteText', 'PasteFromWord', 'RemoveFormat' ] },
                    //        { name: 'insert', items: [ 'Image', 'Table', 'SpecialChar' ] },
                    //        { name: 'forms', items: [ 'Outdent', 'Indent' ] },
                    //        { name: 'clipboard', items: [ 'Undo', 'Redo' ] },
                    //        { name: 'document', items: [ 'PageBreak', 'Source' ] }
                    //    ],
                    //    disableNativeSpellChecker: false,
                    //    uiColor: '#FAFAFA',
                    //    height: '400px',
                    //    width: '100%'
                    //};

                    var options = angular.copy(ngCkeditorOptions);
                    var customOptions = $parse(attrs.ckeditor)(scope);


                    options = angular.extend(options, customOptions);

                    var instance = (isTextarea) ? CKEDITOR.replace(element[0], options) : CKEDITOR.inline(element[0], options),
                        configLoaderDef = $q.defer();

                    element[0].ckEditorInstance = instance;
                    
                    element.bind('$destroy', function () {
                        instance.destroy(
                            false //If the instance is replacing a DOM element, this parameter indicates whether or not to update the element with the instance contents.
                        );
                    });
                    var setModelData = function (setPristine, data) {
                        //var data = instance.getData();
                        if (data == '') {
                            data = null;
                        }
                        $timeout(function () { // for key up event
                            (setPristine !== true || data != ngModel.$viewValue) && ngModel.$setViewValue(data);
                            //(setPristine === true && form) && form.$setPristine();
                        }, 0);
                    }, onUpdateModelData = function (setPristine, value) {
                        if (!data.length) { return; }


                        var item = data.pop() || EMPTY_HTML;
                        isReady = false;
                        instance.setData(item, function () {
                            setModelData(setPristine, value);
                            isReady = true;
                        });
                    }

                    //instance.on('pasteState',   setModelData);
                    instance.on('change', function () {
                        setModelData(false, instance.getData());

                    });
                    instance.on('blur', function () {
                        setModelData(false, instance.getData())
                    });
                    //instance.on('key',          setModelData); // for source view

                    instance.on('instanceReady', function () {
                        scope.$broadcast("ckeditor.ready");
                        scope.$apply(function () {

                            var d = ngModel;
                            //onUpdateModelData(true, instance.element.$.value);
                            onUpdateModelData(true, ngModel.$modelValue);
                        });

                        instance.document.on("keyup", function () {
                            setModelData(false, instance.getData())
                        });
                    });
                    instance.on('customConfigLoaded', function () {
                        configLoaderDef.resolve();
                    });

                    ngModel.$render = function () {
                        data.push(ngModel.$viewValue);
                        if (isReady) {
                            onUpdateModelData(false, instance.getData());
                        }
                    };
                };

                $ocLazyLoad.load(urlHelper.getAbsUrl('./vendors/ckeditor/ckeditor.js?v=4.5.11', true)).then(function () {

                    CKEDITOR.disableAutoInline = true;

                    if (CKEDITOR.status == 'loaded') {
                        loaded = true;
                    }

                    if (loaded) {
                        onLoad();
                    } else {
                        $defer.promise.then(onLoad);
                    }
                });
            }
        };
    }]);


    app.constant('ngCkeditorOptions', {
        autoParagraph: false,
        forceSimpleAmpersand: true,
        height: '250px',
        uiColor: '#FAFAFA',
        toolbar: [
           { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'Templates'] },
           { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', '-', 'RemoveFormat'] },
           { name: 'clipboard', groups: ['clipboard', 'undo'], items: [ 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
           { name: 'colors', items: ['TextColor', 'BGColor'] },
           { name: 'tools', items: ['Maximize'] },
           '/',
           { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
           { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
           { name: 'insert', items: ['Image', 'Table'] },
           '/',
           { name: 'styles', items: ['Format', 'Font', 'FontSize', 'lineheight'] },

        ],
        toolbarGroups: [
            { name: 'document', groups: ['mode', 'document', 'doctools'] },
            { name: 'clipboard', groups: ['clipboard', 'undo'] },
            { name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
            { name: 'forms' },
            '/',
            { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
            { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
            { name: 'links' },
            { name: 'insert' },
            '/',
            { name: 'styles' },
            { name: 'colors' },
            { name: 'tools' },
            { name: 'others' },
            { name: 'about' }
        ]
    });

    return app;
}));
