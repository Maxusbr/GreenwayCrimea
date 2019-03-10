; (function (ng) {
    'use strict';

    var VotingCtrl = function (toaster, $location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this;

        ctrl.subject = { 'types': null, 'ThemeId': null };
        ctrl.changeSubject = function (type, id) {
            ctrl.subject['types'] = type;
            ctrl.subject['ThemeId'] = id;
            if (id == null) {
                ctrl.answers = null;
            }
        }

        ctrl.updateDiagram = function(){
            $http.get('Voting/GetDiagrem', { params: { ThemeId: ctrl.subject['ThemeId'], rnd: Math.random() } }).then(function (response) {
                if (response.data.result == null) {
                    ctrl.answers = response.data;
                }
            });
        }

        var columnDefsVoting = [
                {
                    name: 'Name',
                    displayName: 'Тема',
                    cellTemplate: '<div class="ui-grid-cell-contents news-category-link"><ui-modal-trigger data-controller="\'ModalAddEditVotingCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/voting/modal/addEditVoting/addEditVoting.html" ' +
                                    'data-resolve="{\'ID\': row.entity.ID}" ' +
                                    'data-on-close="grid.appScope.$ctrl.gridExtendCtrl.gridVoting.fetchData()"> ' +
                                    '<a ng-href="" class="ui-grid-custom-service-icon">{{COL_FIELD}}</a>' +
                                  '</ui-modal-trigger></div>',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Тема',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name'
                    }
                },
                {
                    name: 'ID',
                    displayName: ' ',
                    width: 150,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.changeSubject(\'answers\',row.entity.ID)">Варианты ответа</a></div>'
                },
                {
                    name: 'IsDefault',
                    displayName: 'Текущее',
                    enableCellEdit: false,
                    width:80,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;"><label class="adv-checkbox-label">' +
                        '<input type="checkbox" ng-model="row.entity.IsDefault" class="adv-checkbox-input control-checkbox" />' +
                        '<span class="adv-checkbox-emul"></span></label></div></div>',
                    filter: {
                        placeholder: 'Текущее',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsDefault',
                        selectOptions: [{ label: 'Текущие', value: true }, { label: 'Не текущие', value: false }]
                    }
                },
                {
                    name: 'IsHaveNullVoice',
                    displayName: 'Имеет пустой голос',
                    enableCellEdit: false,
                    width: 150,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;"><label class="adv-checkbox-label">' +
                        '<input type="checkbox" ng-model="row.entity.IsHaveNullVoice" class="adv-checkbox-input control-checkbox" />' +
                        '<span class="adv-checkbox-emul"></span></label></div></div>',
                    filter: {
                        placeholder: 'Имеет пустой голос',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsHaveNullVoice',
                        selectOptions: [{ label: 'Имеют пустые голоса', value: true }, { label: 'Не имеют пустые голоса', value: false }]
                    }
                },
                {
                    name: 'IsClose',
                    displayName: 'Закрытое',
                    enableCellEdit: false,
                    width: 80,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;"><label class="adv-checkbox-label">' +
                        '<input type="checkbox" ng-model="row.entity.IsClose" class="adv-checkbox-input control-checkbox" />' +
                        '<span class="adv-checkbox-emul"></span></label></div></div>',
                    filter: {
                        placeholder: 'Закрытое',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsClose',
                        selectOptions: [{ label: 'Закрытые', value: true }, { label: 'Не закрытые', value: false }]
                    }
                },
                {
                    name: 'CountAnswers',
                    displayName: 'Количество ответов',
                    enableSorting: false,
                    enableCellEdit: false,
                    width: 100,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Количество ответов',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'CountAnswersFrom'
                            },
                            to: {
                                name: 'CountAnswersTo'
                            }
                        }
                    }
                },
                {
                    name: 'DateAdded',
                    displayName: 'Дата создания',
                    width: 180,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="text-align:left;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Дата создания',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'DateAddedFrom'
                            },
                            to: {
                                name: 'DateAddedTo'
                            }
                        }
                    }
                },
                {
                    name: 'DateModify',
                    displayName: 'Дата модификации',
                    width: 180,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="text-align:left;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Дата модификации',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'DateModifyFrom'
                            },
                            to: {
                                name: 'DateModifyTo'
                            }
                        }
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditVotingCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/voting/modal/addEditVoting/addEditVoting.html" ' +
                                    'data-resolve="{\'ID\': row.entity.ID}" ' +
                                    'data-on-close="grid.appScope.$ctrl.gridExtendCtrl.gridVoting.fetchData()"> ' +
                                    '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil">{{COL_FIELD}}</a>' +
                            '</ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="Voting/DeleteVoting" params="{\'Ids\': row.entity.ID}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptionsVoting = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsVoting,
            uiGridCustom: {
                rowUrl: '',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'Voting/DeleteVoting',
                        field: 'ID',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridVotingOnInit = function (gridVoting) {
            ctrl.gridVoting = gridVoting;
        };


        var columnDefsAnswers = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents news-category-link"><ui-modal-trigger data-controller="\'ModalAddEditAnswersCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/voting/modal/addEditAnswers/addEditAnswers.html" ' +
                                    'data-resolve="{\'ID\': row.entity.ID, \'ThemeId\': grid.appScope.$ctrl.gridExtendCtrl.subject[\'ThemeId\']}" ' +
                                    'data-on-close="grid.appScope.$ctrl.gridExtendCtrl.gridAnswers.fetchData()"> ' +
                                    '<a ng-href="" class="ui-grid-custom-service-icon">{{COL_FIELD}}</a>' +
                                  '</ui-modal-trigger></div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name'
                    }
                },
                {
                    name: 'CountVoice',
                    displayName: 'Количество голосов',
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;">{{COL_FIELD}}</div></div>',
                    width: 150,
                    filter: {
                        placeholder: 'Количество голосов',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'CountVoiceFrom'
                            },
                            to: {
                                name: 'CountVoiceTo'
                            }
                        }
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    enableCellEdit: false,
                    width: 100,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Сортировка',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'SortOrderFrom'
                            },
                            to: {
                                name: 'SortOrderTo'
                            }
                        }
                    }
                },
                {
                    name: 'IsVisible',
                    displayName: 'Видимый',
                    enableCellEdit: false,
                    width: 80,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;"><label class="adv-checkbox-label">' +
                        '<input type="checkbox" ng-model="row.entity.IsVisible" class="adv-checkbox-input control-checkbox" />' +
                        '<span class="adv-checkbox-emul"></span></label></div></div>',
                    filter: {
                        placeholder: 'Видимый',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsVisible',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'DateAdded',
                    displayName: 'Дата создания',
                    width: 180,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="text-align:left;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Дата создания',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'DateAddedFrom'
                            },
                            to: {
                                name: 'DateAddedTo'
                            }
                        }
                    }
                },
                {
                    name: 'DateModify',
                    displayName: 'Дата модификации',
                    width: 180,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="text-align:left;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Дата модификации',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'DateModifyFrom'
                            },
                            to: {
                                name: 'DateModifyTo'
                            }
                        }
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditAnswersCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/voting/modal/addEditAnswers/addEditAnswers.html" ' +
                                    'data-resolve="{\'ID\': row.entity.ID, \'ThemeId\': grid.appScope.$ctrl.gridExtendCtrl.subject[\'ThemeId\']}" ' +
                                    'data-on-close="grid.appScope.$ctrl.gridExtendCtrl.gridAnswers.fetchData()"> ' +
                                    '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil">{{COL_FIELD}}</a>' +
                            '</ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="Voting/DeleteAnswers" params="{\'Ids\': row.entity.ID}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
        ];

        ctrl.gridOptionsAnswers = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsAnswers,
            uiGridCustom: {
                rowUrl: '',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'Voting/DeleteAnswers',
                        field: 'ID',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridAnswersOnInit = function (gridAnswers) {
            ctrl.gridAnswers = gridAnswers;
            ctrl.updateDiagram();
        };
    };

    VotingCtrl.$inject = ['toaster', '$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('voting', ['uiGridCustom', 'urlHelper'])
      .controller('VotingCtrl', VotingCtrl);

})(window.angular);