
function getRelatedProductsInSc(className, itemIds, title, relatedType) {
    if (itemIds == null || itemIds.length == 0) {
        return;
    }

    $.ajax({
        dataType: "text",
        cache: false,
        type: "GET",
        url: "catalog/productsbyids",
        data: {
            ids: itemIds.join(','),
            type: relatedType
        },
        success: function (data) {
            if (data != null && data.length > 0) {
                var $targetDom = $(className),
                htmlToCompile = data;


                var $injector = angular.element(document).injector();

                $injector.invoke(["$compile", "$rootScope", function ($compile, $rootScope) {

                    $targetDom.html(htmlToCompile);

                    var $scope = $targetDom.scope();

                    $compile($targetDom)($scope || $rootScope);

                    $rootScope.$digest();
                }]);
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
