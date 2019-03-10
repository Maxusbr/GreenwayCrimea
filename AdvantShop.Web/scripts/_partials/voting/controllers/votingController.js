; (function (ng) {

    'use strict';

    var VotingCtrl = function ($http, $element, $q, CacheFactory) {
        var ctrl = this,
            cache;
        
        if (!CacheFactory.get('votingCache')) {
            cache = CacheFactory('votingCache', {
                maxAge: 60 * 1000, // 1 minute
                deleteOnExpire: 'aggressive',
                storageMode: 'localStorage'
            });
        }

        ctrl.fetch = function (cache) {
            (cache === true ? ctrl.getData() : ctrl.getDataFromDB()).then(function (response) {
                if (response != null) {
                    ctrl.data = response;
                    $element.removeClass('ng-hide');
                    ctrl.isShowResult = ctrl.data.isVoted === true;
                }
            });
        }

        ctrl.getData = function () {
            return ctrl.getDataFromCache().then(function (response) {
                if (response == null) {
                    return ctrl.getDataFromDB();
                }
                return response;
            });
        };

        ctrl.getDataFromCache = function () {
            var data = cache.get('votingData');
            return $q.when(data);
        }

        ctrl.getDataFromDB = function () {
            return $http.post('/Voting/GetVotingData').then(function (response) {
                if (response.data != null) {
                    cache.put('votingData', response.data);
                }

                return response.data;
            });
        }

        ctrl.sendAnswer = function(answerId) {
            return $http.post('/Voting/Vote', { answerId: answerId });
        }

        ctrl.send = function (answerId) {
            return ctrl.sendAnswer(answerId).then(function () {
                ctrl.isShowResult = true;
                ctrl.fetch();
            });
        };

        ctrl.resultShow = function () {
            ctrl.isShowResult = true;
            ctrl.sendAnswer(0).then(function () {
                ctrl.fetch();
            });
        };

        ctrl.fetch(true);
    };

    ng.module('voting')
      .controller('VotingCtrl', VotingCtrl);

    VotingCtrl.$inject = ['$http', '$element', '$q', 'CacheFactory'];

})(window.angular);