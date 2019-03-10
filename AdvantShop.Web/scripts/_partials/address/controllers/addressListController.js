; (function (ng) {

    'use strict';

    var AddressListCtrl = function ($http, $q, addressService, zoneService, modalService) {

        var ctrl = this,
            isModalRender = false,
            timerChange;

        ctrl.items = [];
        ctrl.form = {};
        ctrl.fields = null;
        ctrl.isLoaded = false;

        modalService.getModal('modalAddress').then(function (modal) {
            ctrl.formModal = modal.modalScope._form;
        });

        addressService.getAddresses().then(function (response) {

            ctrl.items = [];

            if (response != null && response !== '' && response.length > 0) {
                ctrl.items.push(response[0]);
            }

            //ng.extend(ctrl.items, response);
            ctrl.addressSelected = ctrl.items[0];

            ctrl.initAddressFn({
                address: ctrl.addressSelected
            });

            ctrl.isLoaded = true;
        });

        ctrl.change = function (address) {

            if (timerChange != null) {
                clearTimeout(timerChange);
            }

            timerChange = setTimeout(function () {
                ctrl.changeAddressFn({
                    address: ctrl.addressSelected
                });
            }, 600);
        };

        ctrl.add = function () {
            ctrl.clearFormData();
            ctrl.buildModal();

            if (isModalRender === false) {
                addressService.dialogRender('addressList.modalCallbackClose(modalScope)', ctrl);
                isModalRender = true;
            } else {
                addressService.dialogOpen();
            }
        };

        ctrl.edit = function (item) {

            ctrl.form.contactId = item.ContactId;
            ctrl.form.fio = item.Name;
            ctrl.form.countryId = item.CountryId;
            //ctrl.form.country = item.Country; //set in ctrl.buildModal()
            ctrl.form.region = item.Region;
            ctrl.form.city = item.City;
            ctrl.form.zip = item.Zip;

            ctrl.form.isShowFullAddress = item.IsShowFullAddress;

            if (!ctrl.form.isShowFullAddress) {

                ctrl.form.street = item.Street;
                ctrl.form.house = '';
                ctrl.form.apartment = '';
                ctrl.form.structure = '';
                ctrl.form.entrance = '';
                ctrl.form.floor = '';
            } else {
                ctrl.form.street = item.Street;
                ctrl.form.house = item.House;
                ctrl.form.apartment = item.Apartment;
                ctrl.form.structure = item.Structure;
                ctrl.form.entrance = item.Entrance;
                ctrl.form.floor = item.Floor;
            }
            
            ctrl.buildModal().then(function () {
                if (isModalRender === false) {
                    addressService.dialogRender('addressList.modalCallbackClose', ctrl);
                    isModalRender = true;
                } else {
                    addressService.dialogOpen();
                }
            });
        };

        //ctrl.remove = function (contactId, index) {
        //    addressService.removeAddress(contactId).then(function (response) {
        //        if (response === true) {
        //            ctrl.items.splice(index, 1);
        //        }
        //    });
        //};

        ctrl.buildModal = function () {
            return addressService.getFields()
                .then(function(response) {
                    return ctrl.fields = ctrl.fields || response;
                })
                .then(function(fields) {
                    if (fields.IsShowCountry === true) {
                        return ctrl.getCountries();
                    }
                })
                .then(function(countries) {
                    if (countries != null) {
                        return ctrl.getSelectedCountry(countries);
                    }
                });
        };

        ctrl.getCountries = function () {

            var countriesDefer = $q.defer(),
                countriesPromise;

            if (ctrl.form.countries != null) {
                countriesPromise = countriesDefer.promise;
                countriesDefer.resolve(ctrl.form.countries);
            } else {
                countriesPromise = $http.get('location/GetCountries').then(function (response) { return ctrl.form.countries = response.data; });
            }

            return countriesPromise;
        };

        ctrl.getSelectedCountry = function (countries) {

            return $q.when(ctrl.form.countryId != null && ctrl.form.countryId !== 0 ? { CountryId: ctrl.form.countryId } : zoneService.getCurrentZone()).then(function (zone) {

                var country;

                for (var i = countries.length - 1; i >= 0; i--) {
                    if (countries[i].CountryId === zone.CountryId) {
                        country = countries[i];
                        break;
                    }
                }

                return ctrl.form.country = country;
            });
        };

        ctrl.save = function () {
            var obj = ctrl.getObjectForUpdate();

            addressService.addUpdateCustomerContact(obj).then(function (response) {
                var editContact, needSelect;

                if (response !== null) {

                    if (obj.ContactId != null) {
                        editContact = ctrl.items.filter(function (item) { return item.ContactId == response.ContactId })[0];
                        ng.extend(editContact, response);
                    } else {

                        needSelect = ctrl.items.length === 0;

                        ctrl.items.push(response);

                        if (needSelect === true) {
                            ctrl.addressSelected = ctrl.items[0];
                        }
                    }

                    addressService.getAddresses().then(function (response) {
                        ctrl.items[0] = response[0];
                        ctrl.addressSelected = ctrl.items[0];

                        addressService.dialogClose();
                        ctrl.clearFormData();

                        ctrl.saveAddressFn({
                            address: ctrl.addressSelected
                        });

                    });

                }
            });
        };

        ctrl.clearFormData = function () {
            ctrl.form.contactId = null;
            ctrl.form.fio = null;
            ctrl.form.countryId = null;
            ctrl.form.country = null;
            ctrl.form.region = null;
            ctrl.form.city = null;
            ctrl.form.street = null;
            ctrl.form.zip = null;
            if (ctrl.formModal != null) {
                ctrl.formModal.$setPristine();
            }
        };

        ctrl.modalCallbackClose = function (modal) {
            ctrl.clearFormData();
        };

        ctrl.getObjectForUpdate = function () {

            var form = ctrl.form,
                account = {};

            if (form.contactId) {
                account.ContactId = form.contactId;
            }

            if (form.fio) {
                account.Fio = form.fio;
            }

            if (form.country) {
                account.CountryId = form.country.CountryId;
                account.Country = form.country.Name;
            }

            if (form.region) {
                account.Region = form.region;
            }

            if (form.city) {
                account.City = form.city;
            }
            
            if (form.zip) {
                account.Zip = form.zip;
            }

            account.Street = form.street;
            account.House = form.house;
            account.Apartment = form.apartment;
            account.Structure = form.structure;
            account.Entrance = form.entrance;
            account.Floor = form.floor;

            return account;
        };
    };

    ng.module('address')
      .controller('AddressListCtrl', AddressListCtrl);

    AddressListCtrl.$inject = ['$http', '$q', 'addressService', 'zoneService', 'modalService'];

})(window.angular);
