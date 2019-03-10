; (function (ng) {
    'use strict';

    var ProductVideosCtrl = function ($http, uiGridCustomConfig) {
        var ctrl = this;

        ctrl.$onInit = function() {
            ctrl.showGridVideos = true;
        }

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Name',
                    displayName: 'Название',
                    enableCellEdit: true,
                    enableSorting: false,
                },
                {
                    name: 'VideoSortOrder',
                    displayName: 'Порядок',
                    enableCellEdit: true,
                    enableSorting: false,
                    width: 100,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditVideoCtrl\'" controller-as="ctrl" ' +
                                            'template-url="../areas/admin/content/src/product/components/productVideos/modal/addEditVideo/addEditVideo.html" ' +
                                            'data-resolve="{\'productVideoId\': row.entity.ProductVideoId, \'productId\': row.entity.ProductId}" ' +
                                            'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="product/deleteVideo" params="{\'productVideoId\': row.entity.ProductVideoId }"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ],
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.gridVideos = grid;
            ctrl.showGridVideos = ctrl.gridVideos.gridOptions.data.length > 0;
        };
        
        ctrl.gridOnFetch = function(grid) {
            ctrl.showGridVideos = grid.gridOptions.data.length > 0;
        }
    };

    ProductVideosCtrl.$inject = ['$http', 'uiGridCustomConfig'];

    ng.module('productVideos', ['uiGridCustom'])
        .controller('ProductVideosCtrl', ProductVideosCtrl)
        .component('productVideos', {
            templateUrl: '../areas/admin/content/src/product/components/productVideos/productVideos.html',
            controller: ProductVideosCtrl,
            controllerAs: "ctrl",
            bindings: {
                productId: '=',
            }
      });

})(window.angular);