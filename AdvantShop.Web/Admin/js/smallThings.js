(function ($, $body) {
    $(function () {
        $body.on('click.redirect', '[data-redirect]', function () {
            var loc = this.getAttribute('data-redirect');
            window.location.assign(loc);
        });
    });



    window.progressbar = function (obj, url, params, funcProgress, funcError, interval) {
    	/// <summary>
    	/// Progressbar
    	/// </summary>
        /// <param name="obj">Element - progressbar track</param>
        /// <param name="url">[required] Request url</param>
    	/// <param name="params">Data for send</param>
        /// <param name="funcProgress">[required] Callback success. Parameters (element, responseData, timerId). Need return true for stop.</param>
        /// <param name="funcError">Callback error</param>
    	/// <param name="interval">(deafult:1000) Interval send request</param>

        params = params || {};
        interval = interval || 1000;

        var progressbarFunc = function (_obj, _url, _params, _funcProgress, _funcError, _interval) {
            $.ajax({
                type: 'GET',
                dataType: 'json',
                data: _params || {},
                url: url,
                cache: false,
                async: false,
                success: function (data) {
                    var timerId = setTimeout(function () {

                        var isProcess = _funcProgress(_obj, data, timerId);

                        if (isProcess === true) {
                            clearTimeout(timerId);
                        } else if (isProcess === false) {
                            progressbarFunc(_obj, _url, _params, _funcProgress, _funcError, _interval);
                        }

                    }, _interval);
                },
                error: function (data) {

                    if (_funcError != null) {
                        _funcError(data);
                    } else {
                        throw Error(data.statusText);
                    }
                }
            });
        };

        setTimeout(function () {
            progressbarFunc(obj, url, params, funcProgress, funcError, interval);
        }, interval);
    };

})(jQuery, $(document.body));