; (function (ng) {
	'use strict';

	var FeedbackCtrl = function ($scope, $location, $anchorScroll) {

		var ctrl = this;


		ctrl.switchTheme = function(val) {
			ctrl.curTheme = val;
		};

		ctrl.isSelectedTheme = function(val) {
			return ctrl.curTheme === val;
		};

		$scope.gotoAnchor = function (anchor) {
			if ($location.hash() !== anchor) {
				$location.hash(anchor);
			} else {
				$anchorScroll();
			}
		};
	};

	FeedbackCtrl.$inject = ['$scope', '$location', '$anchorScroll'];

	ng.module('feedback')
		.controller('FeedbackCtrl', FeedbackCtrl);

})(window.angular);