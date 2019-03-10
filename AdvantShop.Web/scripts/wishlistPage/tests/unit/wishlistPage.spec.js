'use strict';

describe('js.tests.units: wishlistPage', function () {

    beforeEach(module('wishlistPage'));
    beforeEach(module('wishlist'));

    var $controller, $httpBackend, wishlistService;

    beforeEach(inject(function (_$controller_, _$httpBackend_, _wishlistService_) {
        $controller = _$controller_;
        $httpBackend = _$httpBackend_;
        wishlistService = _wishlistService_;
    }));

    it('update object countObj', function () {
        var $scope = {},
            ctrl = $controller('WishlistPageCtrl', { $scope: $scope }),
            countObj;

        $httpBackend.when('POST', 'wishlist/wishlistadd').respond({
                Count: 1,
                CountString: "1 товар"
        });

        wishlistService.add(0);
        
        $httpBackend.flush();

        countObj = wishlistService.getCountObj();

        expect(ctrl.countObj.count).toEqual(countObj.count);  
    });

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });
});