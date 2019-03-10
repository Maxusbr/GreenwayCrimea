; (function (ng) {
    ng.module("leads")
        .directive("leads", function () {
            return {
                restrict: "A",
                replace: true,
                transclude: true,
                controllerAs: 'leadsCtrl',
                templateUrl: "/areas/adminmobile/scripts/_partitials/leads/templates/leadsTemplate.html"
            };
        });
})(window.angular);
