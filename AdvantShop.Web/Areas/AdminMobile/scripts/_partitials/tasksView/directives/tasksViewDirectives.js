; (function (ng) {
    ng.module("tasksView")

    .directive("tasksView", function () {
        return {
            restrict: "A",
            replace: true,
            transclude: true,
            controllerAs: 'tasksCtrl',
            templateUrl: "/areas/adminmobile/scripts/_partitials/tasksView/templates/task-template.html"
        };
    });
})(window.angular);
