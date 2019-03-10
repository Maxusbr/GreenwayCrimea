; (function (ng) {
    'use strict';

    var PaymentMethodCtrl = function ($location, $window, toaster, SweetAlert, $http, Upload) {

        var ctrl = this;

        ctrl.init = function(methodId, icon) {
            ctrl.methodId = methodId;
            ctrl.icon = icon;

            ctrl.getAvailableLocations();
        }

        ctrl.getAvailableLocations = function() {
            $http.get('paymentMethods/getAvailableLocations', { params: { methodId: ctrl.methodId } })
                .then(function(response) {

                    var data = response.data;
                    ctrl.AvailableCountries = data.countries;
                    ctrl.AvailableCities = data.cities;
                });
        }
        

        ctrl.deleteAvailableCountry = function(countryId) {
            $http.post('paymentMethods/deleteAvailableCountry', { methodId: ctrl.methodId, countryId: countryId })
                .then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    }
                }).then(ctrl.getAvailableLocations);
        }

        ctrl.deleteAvailableCity = function (cityId) {
            $http.post('paymentMethods/deleteAvailableCity', { methodId: ctrl.methodId, cityId: cityId })
                .then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    }
                }).then(ctrl.getAvailableLocations);
        }
        

        ctrl.addAvailableCountry = function () {
            $http.post('paymentMethods/addAvailableCountry', { methodId: ctrl.methodId, countryName: ctrl.newAvailableCountry })
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
            $http.post('paymentMethods/addAvailableCity', { methodId: ctrl.methodId, cityName: ctrl.newAvailableCity })
                .then(function (response) {
                    if (response.data.result === true) {
                        ctrl.newAvailableCity = '';
                        toaster.pop('success', '', 'Изменения успешно сохранены');
                    } else {
                        toaster.pop('error', '', 'Невозможно добавить "' + ctrl.newAvailableCity + '" ');
                    }
                }).then(ctrl.getAvailableLocations);
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
                        return $http.post('paymentMethods/deleteIcon', { methodId: ctrl.methodId }).then(function (response) {
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
                url: 'paymentMethods/uploadIcon',
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
                    $http.post('paymentMethods/deleteMethod', { methodId: ctrl.methodId }).then(function (response) {
                        $window.location.assign('settings/paymentMethods');
                    });
                }
            });
        }
    };

    PaymentMethodCtrl.$inject = ['$location', '$window', 'toaster', 'SweetAlert', '$http', 'Upload'];


    ng.module('paymentMethod', ['checklist-model'])
      .controller('PaymentMethodCtrl', PaymentMethodCtrl);

})(window.angular);