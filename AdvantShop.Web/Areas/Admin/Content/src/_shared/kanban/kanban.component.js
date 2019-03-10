; (function (ng) {
    'use strict';


    ng.module('kanban')
        .component('kanban', {
            templateUrl: '../areas/admin/content/src/_shared/kanban/template/kanban.html',
            controller: 'KanbanCtrl',
            bindings: {
                kanbanObj: '<',
                sortOptions: '<',
                update: '&',
                fetchUrl: '<?',
                fetchColumnUrl: '<?',
                kanbanOnInit: '&',
                extendCtrl: '<?',
                filterParams: '<?',
                kanbanColumnDefs: '<?',
                kanbanOnFilterInit: '&',
                kanbanParams: '<?',
                modalAddParams: '<?',
                uid: '@'
            }
        })


})(window.angular);