; (function (scriptTag, urlFormat, urlTemplate, urlStyles) {

    'use strict';

    var service = {},
        cache = {};

    service.ajax = function (method, url, success, error, isJSON) {
        var xhr = new XMLHttpRequest();
        if ("withCredentials" in xhr) {
            xhr.open(method, url, true);
        } else {
            alert('Cors not supported')
        }
        xhr.withCredentials = true;


        xhr.onreadystatechange = function (oEvent) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    var result = xhr.responseText;

                    if (isJSON === true) {
                        result = JSON.parse(result);
                    }

                    success(result);
                }
                else {
                    error(xhr, xhr.status);
                } 
            }
        };

        xhr.send();
    };

    service.getScriptData = function () {
        return JSON.parse(scriptTag.getAttribute('data-options'));
    }

    service.render = function (data, domain) {

        if (data.ErrorMessage != null) {
            service.errorRender(data.ErrorMessage);
        }else{
            service.ajax('GET', urlTemplate.replace('{0}', domain), function (template) {

                var scriptTemplate, html, domHtml, styleTag;

                scriptTemplate = document.createElement('script');
                scriptTemplate.type = 'text/html';
                scriptTemplate.id = 'customerTemplate';
                scriptTemplate.innerHTML = template;
                scriptTag.parentNode.appendChild(scriptTemplate);

                html = service.tmpl('customerTemplate', data);
                domHtml = document.createElement('div');
                domHtml.innerHTML = html;

                scriptTag.parentNode.appendChild(domHtml);

                styleTag = document.createElement('link');
                styleTag.href = urlStyles.replace('{0}', domain);
                styleTag.rel = 'stylesheet';

                document.getElementsByTagName('head')[0].appendChild(styleTag);
            });
        }
    };

    service.tmpl = function tmpl(str, data){
        // Figure out if we're getting a template, or if we need to
        // load the template - and be sure to cache the result.
        var fn = !/\W/.test(str) ?
          cache[str] = cache[str] ||
            tmpl(document.getElementById(str).innerHTML) :
     
          // Generate a reusable function that will serve as a template
          // generator (and which will be cached).
          new Function("obj",
            "var p=[],print=function(){p.push.apply(p,arguments);};" +
       
            // Introduce the data as local variables using with(){}
            "with(obj){p.push('" +
       
            // Convert the template into pure JavaScript
            str
              .replace(/[\r\t\n]/g, " ")
              .split("<%").join("\t")
              .replace(/((^|%>)[^\t]*)'/g, "$1\r")
              .replace(/\t=(.*?)%>/g, "',$1,'")
              .split("\t").join("');")
              .split("%>").join("p.push('")
              .split("\r").join("\\'")
          + "');}return p.join('');");
   
        // Provide some basic currying to the user
        return data ? fn( data ) : fn;
    };

    service.errorRender = function (message) {
        var dom = document.createElement('div');
        dom.className = 'aw-customer-error';
        dom.innerHTML = message;

        scriptTag.parentNode.appendChild(dom);
    };

    //initilaze

    var errorCallback = function () {
        service.errorRender('Ошибка при получении данных');
    };

    var successCallback = function(data){
        service.render(data, widgetData.domain);
    };

    var widgetData = service.getScriptData(),
        url = urlFormat.replace('{0}', widgetData.domain).replace('{1}', widgetData.email);

    service.ajax('GET', url, successCallback, errorCallback, true);

})(document.querySelector('[data-advantshop-widget="customer"]'), '{0}/admin/httphandlers/customer/getdata.ashx?email={1}', '{0}/admin/httphandlers/customer/getdata.ashx?type=html', '{0}/widget/widget.css');