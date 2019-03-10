; (function (ng) {
    'use strict';

    var cartService = function ($q, $http, cartConfig) {
        var service = this,
            isRequestProcess = false,
            isInitilaze = false,
            deferes = [],
            cart = {},
            callbacks = {},
            minicart;

        service.getData = function (cache) {

            var defer = $q.defer();

            if (cache == null) {
                cache = true;
            }

            if (isInitilaze === true && cache === true) {
                defer.resolve(cart);
            } else {

                if (isRequestProcess === true) {
                    deferes.push(defer);
                } else {
                    isRequestProcess = true;

                    deferes.push(defer);

                    $http.post('/Cart/GetCart',  { rnd: Math.random() }).then(function (response) {

                        ng.extend(cart, response.data);

                        isInitilaze = true;
                        isRequestProcess = false;

                        for (var i = deferes.length - 1; i >= 0; i--) {
                            deferes[i].resolve(cart);
                        }

                        deferes.length = 0;
                    });
                }
            }

            return defer.promise.then(function (result) {
                service.processCallback('get', result);
                return result;
            });
        };

        service.getModuleData = function () {
            return $http.get('/BuyMore/MiniCartMessage', { rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.updateAmount = function (items) {
            return $http.post('/Cart/UpdateCart', { items: items, rnd: Math.random() }).then(function (response) {
                service.processCallback('update', response.data);
                return service.getData(false);
            });
        };

        service.removeItem = function (shoppingCartItemId) {
            return $http.post('/Cart/RemoveFromCart', { itemId: shoppingCartItemId }).then(function (response) {
                service.processCallback('remove', response.data);
                service.getData(false);
                return response.data;
            });
        };

        service.addItem = function (offerId, productId, amount, attributesXml, payment) {
            if (offerId == null)
            {
                return;
            }
            return $http.post('/cart/addtocart', { offerId: offerId, productId: productId, amount: amount, attributesXml: attributesXml, payment: payment }).then(function (response) {

                var result = response.data,
                    defer,
                    promise;

                if (response.data.status !== 'redirect') {
                    return service.getData(false).then(function () {
                        service.processCallback('add', [result]);
                        return [result];
                    });
                } else {
                    defer = $q.defer();
                    promise = defer.promise;
                    defer.resolve([result]);
                    return promise;
                }
            });
        };

        service.addItems = function (items) {

            if (items == null || items.length == 0)
                return;

            $http.post('/cart/addCartItems', { items: items }).then(function (response) {
                var result = response.data;
                result.addedCount = items.length;
                if (result.status === 'success') {
                    return service.getData(false).then(function () {
                        service.processCallback('add', result);
                        return [result];
                    });
                }
            });
        };

        service.clear = function () {
            return $http.post('/Cart/ClearCart').then(function () {
                service.processCallback('clear');
                return service.getData(false);
            });
        };

        service.addCallback = function (name, func, targetName) {

            callbacks[name] = callbacks[name] || [];

            if (func == null) {
                throw Error('Callback for cart equal null');
            };

            callbacks[name].push({ callback: func, targetName: targetName || null });
        };

        service.processCallback = function (name, params, targetName) {
            if (callbacks[name] == null) {
                return;
            };

            for (var i = callbacks[name].length - 1; i >= 0; i--) {
                if (targetName != null && callbacks[name][i].targetName === targetName) {
                    callbacks[name][i].callback(cart, params);
                } else {
                    callbacks[name][i].callback(cart, params);
                }
            };
        };

        service.removeCallback = function (name, targetName) {
            var arrayCallbacksByName = callbacks[name],
                index;

            if (arrayCallbacksByName == null) {
                return;
            }

            if (targetName != null) {
                for (var i = 0, len = arrayCallbacksByName.length; i < len; i++) {
                    if (arrayCallbacksByName[i].targetName === targetName) {
                        arrayCallbacksByName.splice(i, 1);
                    }
                }
            } else {
                index = callbacks.indexOf(callbacks[name]);

                if (index !== -1) {
                    callbacks.splice(index, 1);
                }
            }
        };
    };

    ng.module('cart')
      .service('cartService', cartService);

    cartService.$inject = ['$q', '$http', 'cartConfig'];

})(angular);