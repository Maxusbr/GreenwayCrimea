; (function (ng) {
    'use strict';

    var blocksConstructorService = function ($http, Upload) {
        var service = this;

        service.getBlocks = function (landingPageId) {
            return $http.get('landinginplace/getblocks', { params: { landingPageId: landingPageId } }).then(function (response) {
                return response.data;
            });
        };

        service.addBlock = function (lpId, name, sortOrder, productId) {
            return $http.post('landinginplace/addblock', { lpId: lpId, name: name, sortOrder: sortOrder, productId: productId }).then(function (response) {
                return response.data;
            });
        };

        service.saveBlockSortOrder = function (blockId, top) {
            return $http.post('landinginplace/saveblocksortorder', { blockId: blockId, top: top }).then(function (response) {
                return response.data;
            });
        };

        service.saveBlockSettings = function (blockId, settings) {
            return $http.post('landinginplace/saveblockSettings', { blockId: blockId, settings: settings }).then(function (response) {
                return response.data;
            });
        };

        service.removeBlock = function (blockId) {
            return $http.post('landinginplace/removeblock', { blockId: blockId }).then(function (response) {
                return response.data;
            });
        };

        service.uploadPicture = function (lpId, blockId, file) {
            return service.sendPicture('upload', lpId, blockId, file);
        };

        service.updatePicture = function (lpId, blockId, file, picture) {
            return service.sendPicture('update', lpId, blockId, file, picture);
        };

        service.sendPicture = function (action, lpId, blockId, file, picture) {

            var data = { lpId: lpId, blockId: blockId };

            if (picture != null) {
                data.picture = picture;
            }

            return Upload.upload({
                url: 'landinginplace/' + (action === 'upload' ? 'uploadpicture' : 'updatepicture'),
                data: data,
                file: file
            }).then(function (response) {
                return response.data;
            });
        };

        service.removePicture = function (lpId, blockId, picture) {
            return $http.post('landinginplace/removepicture', { lpId: lpId, blockId: blockId, picture: picture }).then(function (response) {
                return response.data;
            });
        };

        service.getBackgroundString = function (color, image) {
            var result = image != null && image.length > 0 ? ' url(' + image + ') no-repeat center ' : '';

            if (color.indexOf('linear-gradient') === -1) {
                result = color + result;
            } else {
                result = result + ',' + color;
            }

            return result;
        };
    };

    ng.module('blocksConstructor')
        .service('blocksConstructorService', blocksConstructorService);

    blocksConstructorService.$inject = ['$http', 'Upload']

})(window.angular);