; (function (ng) {
    'use strict';

    ng.module('uiAceTextarea')
        .constant('uiAceDefaultOptions', {
            require: ['ace/ext/language_tools', 'ace/ext/searchbox'],
            advanced: {
                enableSnippets: true,
                enableBasicAutocompletion: true,
                enableLiveAutocompletion: true
            }
        });

})(window.angular);