; (function () {
    'use strict';

    var process = function () {
        var wysiwygElements = document.querySelectorAll('.js-wysiwyg');
        var height = 0;

        for (var i = 0, len = wysiwygElements.length; i < len; i++) {
            var instance = CKEDITOR.instances[wysiwygElements[i].id];
            if (instance != null) {
                continue;
            }

            height = wysiwygElements[i].offsetHeight;

            CKEDITOR.replace(wysiwygElements[i], {
                height: height,
                autoParagraph: false,
                forceSimpleAmpersand: true,
                toolbar: [
                   { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'Templates'] },
                   { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
                   { name: 'clipboard', groups: ['clipboard', 'undo'], items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
                   '/',
                   { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                   { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
                   { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'] },
                   '/',
                   { name: 'styles', items: ['Format', 'Font', 'FontSize', 'lineheight'] },
                   { name: 'colors', items: ['TextColor', 'BGColor'] },
                   { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
                   { name: 'others', items: ['-'] },
                   { name: 'about', items: ['About'] }

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

            CKEDITOR.on('dialogDefinition', function (ev) {
                var dialogName = ev.data.name;
                var dialogDefinition = ev.data.definition;

                if (dialogName == 'table') {
                    var info = dialogDefinition.getContents('info');

                    info.get('txtWidth')['default'] = null;
                    info.get('txtBorder')['default'] = null;
                    info.get('txtCellPad')['default'] = null;
                    info.get('txtCellSpace')['default'] = null;
                }
            });
        }
    }

    window.addEventListener('load', function load() {

        window.removeEventListener('load', load);

        process();

        try {
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(process);
        } catch (err) {
            console.log(err);
        }

    });
})();