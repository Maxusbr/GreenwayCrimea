; (function (ng, body) {

	'use strict';

	/*убираем BFCache в FF*/
	window.addEventListener('unload', function () { });

	ng.module('app', [
		'ngSanitize',
		'ngCookies',
		'angular-cache',
		'oc.lazyLoad',
		'cookiesPolicy',
		'feedback',
		'validation',
		'dom',
		'videos',
		'tabs',
		'urlHelper',
		'address',
		'input',
		'modal',
		'module',
		'toaster',
		'pascalprecht.translate',
	])
		.config(['$provide', '$compileProvider', '$cookiesProvider', '$httpProvider', '$localeProvider', '$locationProvider', function ($provide, $compileProvider, $cookiesProvider, $httpProvider, $localeProvider, $locationProvider) {

			var date = new Date(),
				currentYear = date.getFullYear();

			date.setFullYear(currentYear + 1);

			//#region set cookie expires
			$cookiesProvider.defaults.expires = date;
			$cookiesProvider.defaults.path = '/';

			if (window.location.hostname !== 'localhost' && window.location.hostname !== 'server' &&
				!/^(?!0)(?!.*\.$)((1?\d?\d|25[0-5]|2[0-4]\d)(\.|$)){4}$/.test(window.location.hostname)) {
				$cookiesProvider.defaults.domain = '.' + window.location.hostname.replace('www.', '');
			}

			//#endregion

			//#region ie10 bug validation

			$provide.decorator('$sniffer', ['$delegate', function ($sniffer) {
				var msie = parseInt((/msie (\d+)/.exec(angular.lowercase(navigator.userAgent)) || [])[1], 10);
				var _hasEvent = $sniffer.hasEvent;
				$sniffer.hasEvent = function (event) {
					if (event === 'input' && msie === 10) {
						return false;
					}
					_hasEvent.call(this, event);
				}
				return $sniffer;
			}]);

			//#endregion

			//#region prepera ajax url in absolute path
			var basePath = document.getElementsByTagName('base')[0].getAttribute('href'),
				regex = new RegExp('^(?:[a-z]+:)?//', 'i');


			$httpProvider.useApplyAsync(true);

			$httpProvider.interceptors.push(function () {
				return {
					'request': function (config) {

						var urlOld = config.url,
							template;

						if (regex.test(config.url) === false) {

							if (config.url.charAt(0) === '/') {
								config.url = config.url.substring(1);
							}

							config.url = basePath + config.url;
						}

						//for templates
						if (urlOld != config.url && ng.isObject(config.cache) && config.cache.get(urlOld) !== null) {
							template = config.cache.get(urlOld);
							config.cache.remove(urlOld);
							config.cache.put(config.url, template);
						}

						return config;
					}
				};
			});

			//#endregion

			$locationProvider.html5Mode({
				enabled: true,
				requireBase: true,
				rewriteLinks: false
			});

			$locationProvider.hashPrefix('#');

		}])
		.run(function () {

			var timerScroll,
				linkWithAnchors = document.querySelectorAll('a[href*="#"]'),
				linkHref,
				linkHashIndex;

			window.addEventListener('scroll', function () {

				clearTimeout(timerScroll);

				if (!body.classList.contains('disable-hover')) {
					body.classList.add('disable-hover');
				}

				timerScroll = setTimeout(function () {
					body.classList.remove('disable-hover');
				}, 500);
			}, false);

			//materialClickInit();
		})
		.controller('AppCtrl', function () {
		})
		.filter("sanitize", ['$sce', function ($sce) {
			return function (htmlCode) {
				return $sce.trustAsHtml(htmlCode);
			}
		}]);

})(window.angular, document.body);


