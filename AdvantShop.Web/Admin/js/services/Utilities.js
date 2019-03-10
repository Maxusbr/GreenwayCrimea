Advantshop.Utilities = {};

 /*file*/
(function ($) {
    /*----*/
    var fileClass = function () { };

    fileClass.IsExist = function (url) {
        var result = false;
        $.ajax({
            url: url,
            type: 'HEAD',
            cache: false,
            async: false,
            error: function () {
                result = false;
            },
            success: function () {
                result = true;
            }
        });
        return result;
    };

    Advantshop.Utilities.File = fileClass;
    /*----*/
})(jQuery);
/*events*/
(function($) {
/*----*/
    var eventsClass = function() { };
    
    eventsClass.list = function ($element) {
    // В разных версиях jQuery список событий получается по-разному
    var events = $element.data('events');
    if (events !== undefined) return events;

    events = $.data($element, 'events');
    if (events !== undefined) return events;

    events = $._data($element, 'events');
    if (events !== undefined) return events;

    events = $._data($element[0], 'events');
    if (events !== undefined) return events;

    return null;
    };

    eventsClass.isExistEvent = function ($element, eventName) {

        if ($element.length === 0) {
            return false;
        }

        var events, isExist = false;
        events = eventsClass.list($element);

        if (!events) {
            isExist = false;
        } else if (eventName.indexOf('.') == -1) {
            isExist = eventName in events;
        } else {
            var eventType = /^(\w+)\.?/g.exec(eventName)[1];
            var namespace = eventName.replace(eventType + ".", "");


            namespace = namespace.split(".").reverse().join(".");

            if(!events[eventType]) {
                isExist = false;
            }else {
                 for (var i = 0; i < events[eventType].length; i++ ) {
                    if (events[eventType][i].namespace == namespace) {
                        isExist = true;
                        break;
                    } 
                }    
            }
        }

        return isExist; 
    };
    
    Advantshop.Utilities.Events = eventsClass;
/*----*/
})(jQuery);

/*text*/
(function() {
/*----*/    
    var textObj = function() {};
    textObj.toFirstUpperSimbol = function(str) {
        var simbol = str[0] || str.charAt(0);
        var result = simbol.toUpperCase() + str.substring(1, str.length);
        return result;
    };
    
    Advantshop.Utilities.Text = textObj; 
/*----*/ 
})();

/*others*/
(function() {
/*----*/
    var evalObj = function (code) {
        if (typeof (code) === 'string' && code.length === 0) {
            return null;
        }
        return (new Function('return ' + code))();
   };
    
   Advantshop.Utilities.Eval = evalObj;
/*----*/    
})();

