(function () {

    var scriptsManager = function () { };

    scriptsManager.isExistLinkJS = function (url) {
        return $('script').filter('[src$="' + url + '"]').length;
    };

    scriptsManager.isExistLinkCSS = function (url) {
        return $('link').filter('[href$="' + url + '"]').length;
    };

    Advantshop.NamespaceRequire('Advantshop');
    Advantshop.ScriptsManager = scriptsManager;
})();
