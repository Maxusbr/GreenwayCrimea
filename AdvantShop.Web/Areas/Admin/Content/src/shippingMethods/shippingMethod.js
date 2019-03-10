; (function (ng) {
    'use strict';

    var ShippingMethodCtrl = function ($location, $window, toaster, SweetAlert, $http, Upload) {

        var ctrl = this;

        ctrl.init = function(methodId, icon) {
            ctrl.methodId = methodId;
            ctrl.icon = icon;

            ctrl.getAvailableLocations();
            ctrl.getExcludedLocations();
            ctrl.getPayments();
        }

        ctrl.getAvailableLocations = function() {
            $http.get('shippingMethods/getAvailableLocations', { params: { methodId: ctrl.methodId } })
                .then(function(response) {

                    var data = response.data;
                    ctrl.AvailableCountries = data.countries;
                    ctrl.AvailableCities = data.cities;
                });
        }

        ctrl.getExcludedLocations = function () {
            $http.get('shippingMethods/getExcludedLocations', { params: { methodId: ctrl.methodId } })
                .then(function (response) {

                    var data = response.data;
                    ctrl.ExcludedCities = data.cities;
                    ctrl.ExcludedCountry = data.country;
                });
        }


        ctrl.deleteAvailableCountry = function(countryId) {
            $http.post('shippingMethods/deleteAvailableCountry', { methodId: ctrl.methodId, countryId: countryId })
                .then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    }
                }).then(ctrl.getAvailableLocations);
        }

        ctrl.deleteAvailableCity = function (cityId) {
            $http.post('shippingMethods/deleteAvailableCity', { methodId: ctrl.methodId, cityId: cityId })
                .then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    }
                }).then(ctrl.getAvailableLocations);
        }

        ctrl.deleteExcludedCity = function (cityId) {
            $http.post('shippingMethods/deleteExcludedCity', { methodId: ctrl.methodId, cityId: cityId })
                .then(function (response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    }
                }).then(ctrl.getExcludedLocations);
        }

        ctrl.deleteExcludedCountry = function (CountryID) {
            $http.post('shippingMethods/DeleteExcludedCountry', { methodId: ctrl.methodId, CountryId: CountryID })
                .then(function (response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    }
                }).then(ctrl.getExcludedLocations);
        }


        ctrl.addAvailableCountry = function () {
            $http.post('shippingMethods/addAvailableCountry', { methodId: ctrl.methodId, countryName: ctrl.newAvailableCountry })
                .then(function (response) {
                    if (response.data.result === true) {
                        ctrl.newAvailableCountry = '';
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    } else {
                        toaster.pop('error', '', 'Невозможно добавить "' + ctrl.newAvailableCountry + '" ');
                    }
                }).then(ctrl.getAvailableLocations);
        }

        ctrl.addAvailableCity = function () {
            $http.post('shippingMethods/addAvailableCity', { methodId: ctrl.methodId, cityName: ctrl.newAvailableCity })
                .then(function (response) {
                    if (response.data.result === true) {
                        ctrl.newAvailableCity = '';
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    } else {
                        toaster.pop('error', '', 'Невозможно добавить "' + ctrl.newAvailableCity + '" ');
                    }
                }).then(ctrl.getAvailableLocations);
        }

        ctrl.addExcludedCity = function () {
            $http.post('shippingMethods/AddExcludedCity', { methodId: ctrl.methodId, cityName: ctrl.newExcludedCity })
                .then(function (response) {
                    if (response.data.result === true) {
                        ctrl.newExcludedCity = '';
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    } else {
                        toaster.pop('error', '', 'Невозможно добавить "' + ctrl.newExcludedCity + '" ');
                    }
                }).then(ctrl.getExcludedLocations);
        }

        ctrl.addExcludedCountry = function () {
            $http.post('shippingMethods/AddExcludedCountry', { methodId: ctrl.methodId, countryName: ctrl.newExcludedCountry })
                .then(function (response) {
                    if (response.data.result === true) {
                        ctrl.newExcludedCountry = '';
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    } else {
                        toaster.pop('error', '', 'Невозможно добавить "' + ctrl.newExcludedCountry + '" ');
                    }
                }).then(ctrl.getExcludedLocations);
        }


        ctrl.getPayments = function () {
            $http.get('shippingMethods/getPayments', { params: { methodId: ctrl.methodId } })
                .then(function (response) {
                    ctrl.payments = response.data;
                    ctrl.selectedPaymentMethods =
                        ctrl.payments != null
                            ? ctrl.payments.filter(function (x) { return x.Active === true; }).map(function(x) { return x.PaymentMethodId; })
                            : null;
                });
        }


        ctrl.uploadIcon = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                ctrl.sendIcon($file);
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
            }
        };

        ctrl.deleteIcon = function () {

            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" })
                .then(function (result) {
                    if (result === true) {
                        return $http.post('shippingMethods/deleteIcon', { methodId: ctrl.methodId }).then(function (response) {
                            var data = response.data;
                            if (data.result === true) {
                                ctrl.icon = null;
                                toaster.pop('success', '', 'Изображение удалено');
                            } else {
                                toaster.pop('error', 'Ошибка при удалении', data.error);
                            }
                        });
                    }
                });
        };

        ctrl.sendIcon = function (file) {
            return Upload.upload({
                url: 'shippingMethods/uploadIcon',
                data: {
                    file: file,
                    methodId: ctrl.methodId,
                    rnd: Math.random(),
                }
            }).then(function (response) {
                var data = response.data;

                if (data.Result === true) {
                    ctrl.icon = data.Picture;
                    toaster.pop('success', '', 'Изображение сохранено');
                } else {
                    toaster.pop('error', 'Ошибка при загрузке', data.error);
                }
            });
        }


        ctrl.deleteMethod = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('shippingMethods/deleteMethod', { methodId: ctrl.methodId }).then(function (response) {
                        $window.location.assign('settings/shippingMethods');
                    });
                }
            });
        }


        /* sdek */
        ctrl.callSdekCourier = function () {

            var params = ctrl.sdek;
            $http.post('shippingMethods/callSdekCourier', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.msg);
                } else {
                    if (typeof data.msg === 'string') {
                        toaster.pop('error', '', data.msg);
                    } else if (Object.prototype.toString.call(data.msg) === '[object Array]') {
                        for (var i = 0; i < data.msg.length; i++) {
                            toaster.pop('error', '', data.msg[i]);
                        }
                    }
                }
            });
        }


    };

    ShippingMethodCtrl.$inject = ['$location', '$window', 'toaster', 'SweetAlert', '$http', 'Upload'];


    ng.module('shippingMethod', ['checklist-model', 'angular-inview'])
      .controller('ShippingMethodCtrl', ShippingMethodCtrl);

})(window.angular);