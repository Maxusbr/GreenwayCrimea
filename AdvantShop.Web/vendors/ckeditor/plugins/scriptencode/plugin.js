;(function(){

    var ICON_HTML = '<img class="ckeditor-scriptencode-icon" src="./vendors/ckeditor/plugins/scriptencode/images/icon.png">',
        REGEXP_REMOVE_ICON = new RegExp('(<img.*class="ckeditor-scriptencode-icon".*>)<script', 'g');

    CKEDITOR.plugins.add('scriptencode', {
        init: function (editor) {

            editor.on('getData', function (event) {
                event.data.dataValue = event.data.dataValue.replace(REGEXP_REMOVE_ICON, function (str, group, offset, source) {
                    return str.replace(group, '');
                });
            });

            editor.on('setData', function (event) {
                event.data.dataValue = event.data.dataValue.replace(/<script/g, ICON_HTML + '<script');
            });
        }
    });
})();

