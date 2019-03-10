; (function (ng) {
    'use strict';

    var CatalogTreeviewCtrl = function ($window, catalogService, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.url = 'catalog/categoriestree?categoryIdSelected=' + (ctrl.categoryIdSelected || '0');
        };

        ctrl.onInitTree = function (jstree) {
            ctrl.jstree = jstree;

            if(ctrl.onInit != null){
                ctrl.onInit({ jstree: jstree });
            }
        };

        ctrl.treeCallbacks = {
            select_node: function (event, data) {
                $window.location.assign('catalog?categoryid=' + data.node.id);
            },
            move_node: function (event, data) {
                var tree = ctrl.jstree, nodeId, parentId, prev, next, prevId, nextId;

                nodeId = data.node.id;
                parentId = data.parent !== "#" ? data.parent : "0";
                next = tree.get_next_dom(data.node, true);
                prev = tree.get_prev_dom(data.node, true);

                if (prev != null) {
                    prevId = tree.get_node(prev).id;
                }

                if (next != null) {
                    nextId = tree.get_node(next).id;
                }

                catalogService.changeCategorySortOrder(nodeId, prevId, nextId, parentId).then(function (response) {
                    if (response) {
                        toaster.pop('success', '', 'Изменения сохранены');
                    }
                });
            }
        };

    };


    CatalogTreeviewCtrl.$inject = ['$window', 'catalogService', 'toaster'];

    ng.module('catalog')
        .controller('CatalogTreeviewCtrl', CatalogTreeviewCtrl);

})(window.angular);
