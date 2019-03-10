; (function (ng) {

    'use strict';

    var bonusService = function ($compile, $http, $q, $rootScope) {

        var service = this,
            _bonusCode,
            bonusCodeEl;

        bonusCodeEl = ng.element('<div data-bonus-code></div>');
        document.body.appendChild(bonusCodeEl[0]);
        $compile(bonusCodeEl)($rootScope.$new());

        service.addModalCode = function (bonusCode) {
            _bonusCode = bonusCode;
        };

        service.showModalCode = function (fn) {
            _bonusCode.setCallback(fn);
            _bonusCode.showModal();
        };

        service.successModal = function () {
            _bonusCode.successModal();
        };

        service.getBonus = function () {
            return $http.get('bonuses/bonusjson', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        service.createBonusCard = function () {
            return $http.post('bonuses/createbonuscard', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        service.autorize = function (cardnumber, phone) {
            phone = phone || '';

            return $http.post('bonuses/confirmcard', { cardnumber: cardnumber, phone: phone.replace(/\D+/g, ''), rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.register = function (phone) {
            return $http.post('bonuses/confirmnewcard', { phone: phone.replace(/\D+/g, ''), rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.checkCode = function (code, isCheckout) {
            return $http.post('bonuses/confirmcode', { code: code, isCheckout: isCheckout, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        //service.newCard = function (code, isCheckout, firstName, lastName, secondName, gender, birthDay, phone, email, city) {
        service.newCard = function (code, isCheckout) {
            return $http.post('bonuses/confirmcodefornewcard', {
                code: code,
                isCheckout: isCheckout,
                //firstName: firstName,
                //lastName: lastName,
                //secondName: secondName,
                ///gender: gender,
                //birthDay: birthDay,
                //phone: phone.replace(/\D+/g, ''),
                //email: email,
                //city: city,
                rnd: Math.random()
            }).then(function (response) {
                return response.data;
            });
        };
		
		service.getDataAgreement = function () {
            return $http.post('Checkout/CheckoutBuyInOneClickFields', {
                rnd: Math.random()
            }).then(function (response) {
                return response.data;
            });
        };

        //service.updateCard = function (bonus) {
        //    return $http.post('bonuses/updatecard', bonus)
        //        .then(function (response) {
        //            return response.data;
        //        });
        //};
    };

    ng.module('bonus')
      .service('bonusService', bonusService);

    bonusService.$inject = ['$compile', '$http', '$q', '$rootScope'];

})(window.angular);