; (function (ng) {
    'use strict';

    var AnswersListCtrl = function ($http, SweetAlert, toaster, urlHelper, $window) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.themeId = ctrl.parametr;
            ctrl.fetch();

            if (ctrl.onInit != null) {
                ctrl.onInit({ answers: ctrl });
            }
        };

        ctrl.fetch = function () {
            //$http.get('Voting/getVotingList').then(function (response) {
            //    ctrl.answers = response.data;
            //});
            $http.post('Voting/getVotingList', { ThemeId: ctrl.themeId }).then(function (response) {
                ctrl.answers = response.data;
                ctrl.answers.push({ 'AnswerId': null, 'FkidTheme': ctrl.themeId });
            });
        };


        ctrl.sortableOptions = {
            orderChanged: function(event) {
                var AnswerId = event.source.itemScope.item.AnswerId,
                    ThemeId = event.source.itemScope.item.FkidTheme,
                    prev = ctrl.answers[event.dest.index - 1],
                    next = ctrl.answers[event.dest.index + 1];
                
                $http.post('Voting/changeSorting', {
                    Id: AnswerId,
                    ThemeId: ThemeId,
                    prevId: prev != null ? prev.AnswerId : null,
                    nextId: next != null ? next.AnswerId : null
                }).then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения сохранены');
                    }
                });
            }
        };

        ctrl.setAnswer = function (text, themeId) {
            $http.post('Voting/setAnswer', { Text: text, ThemeId: themeId }).then(function (response) {
                if (response.data.result === true) {
                    ctrl.fetch();
                    toaster.pop('success', '', 'Изменения сохранены');
                }
                else {
                    toaster.pop('error', '', 'Ошибка добавления варианта ответа');
                }
            });
        }

        ctrl.deleteAnswer = function (answerId, themeId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('Voting/deleteAnswer', { Id: answerId, ThemeId: themeId }).then(function (response) {
                        if (response.data.result === true) {
                            ctrl.fetch();
                            toaster.pop('success', '', 'Вариант ответа успешно удален');
                        }
                        else {
                            toaster.pop('error', '', 'Ошибка удаления');
                        }
                    });
                }
            });
        }

    };

    AnswersListCtrl.$inject = ['$http', 'SweetAlert', 'toaster', 'urlHelper', '$window'];

    ng.module('answersList', ['as.sortable'])
        .controller('AnswersListCtrl', AnswersListCtrl)
        .component('answersList', {
            templateUrl: '../areas/admin/content/src/voting/modal/addEditVoting/components/AnswersList/templates/answersList.html',
            controller: 'AnswersListCtrl',
            transclude: true,
            bindings: {
                onInit: '&',
                parametr: '@'
            }
        });

})(window.angular);