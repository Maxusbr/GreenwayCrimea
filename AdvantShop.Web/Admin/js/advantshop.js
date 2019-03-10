var Advantshop = Advantshop || {};

(function ($) {
    Advantshop.NamespaceRequire = function(namespaceString) {
        var parts = namespaceString.split('.'),
            parent = window,
            currentPart = '';

        for (var i = 0, length = parts.length; i < length; i++) {
            currentPart = parts[i];
            parent[currentPart] = parent[currentPart] || {};
            parent = parent[currentPart];
        }

        return parent;
    };

    Advantshop.GetJQueryObject = function (selector) {
        var obj;

        if ((selector instanceof $) === false) {
            obj = $(selector);
        } else{
			obj = selector;
		}
		
        return obj;
    };
} (jQuery));
