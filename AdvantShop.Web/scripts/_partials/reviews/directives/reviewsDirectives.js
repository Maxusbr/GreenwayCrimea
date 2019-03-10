; (function (ng) {
    'use strict';

    ng.module('reviews')
      .directive('reviews', function () {
          return {
              restrict: 'A',
              scope: true,
              controller: 'ReviewsCtrl',
              controllerAs: 'reviews',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  ctrl.moderate = attrs.moderate === 'true';
                  ctrl.isAdmin = attrs.isAdmin === 'true';
                  ctrl.entityId = attrs.entityId;
                  ctrl.entityType = attrs.entityType;
                  ctrl.name = attrs.name;
                  ctrl.email = attrs.email;
                  ctrl.actionUrl = attrs.actionUrl;
                  ctrl.formVisible = attrs.formVisible !== 'false';
                  ctrl.allowImageUpload = attrs.allowImageUpload === 'true';
                  ctrl.readonly = attrs.readonly === 'true';
              }
          };
      });

    ng.module('reviews')
      .directive('reviewItem', function () {
          return {
              require: '^reviews',
              restrict: 'A',
              scope: true,
              link: function (scope, element, attrs, ctrl) {
                  ctrl.addItemInStorage(attrs.reviewId, element);
              }
          };
      });

    ng.module('reviews')
      .directive('reviewsForm', function () {
          return {
              require: ['^reviewsForm', '^reviews'],
              restrict: 'A',
              scope: {
                  visibleFormCancelButton: '=',
                  reviewId: '=',
                  name: '=',
                  email: '=',
                  submitFn: '&',
                  cancelFn: '&',
                  formVisible: '=',
                  allowImageUpload: '=',
                  isShowUserAgreementText: '=',
                  userAgreementText: '@',
                  moderate: '='
              },
              controller: 'ReviewsFormCtrl',
              controllerAs: 'reviewsForm',
              bindToController: true,
              transclude: true,
              templateUrl: '/scripts/_partials/reviews/templates/reviewForm.html',
              replace: true,
              link: function (scope, element, attrs, ctrls) {
                  ctrls[1].addForm(ctrls[0], element);
              }
          };
      });

    ng.module('reviews')
      .directive('reviewReply', function () {
          return {
              require: '^reviews',
              restrict: 'A',
              replace: true,
              transclude: true,
              scope: {
                  reviewId: '@'
              },
              template: '<a href="" class="review-item-button" data-ng-transclude data-ng-click="parentScope.reply(reviewId)"></a>',
              link: function (scope, element, attrs, ctrl) {
                  scope.parentScope = ctrl;
              }
          };
      });

    ng.module('reviews')
  .directive('reviewDelete', function () {
      return {
          require: '^reviews',
          restrict: 'A',
          replace: true,
          transclude: true,
          scope: {
              reviewId: '@',
              actionUrl: '@'
          },
          template: '<a href="" class="review-item-button" data-ng-transclude data-ng-click="parentScope.delete(reviewId, actionUrl)"></a>',
          link: function (scope, element, attrs, ctrl) {
              scope.parentScope = ctrl;
          }
      };
  });

})(window.angular);