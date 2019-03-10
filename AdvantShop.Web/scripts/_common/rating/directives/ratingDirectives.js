; (function (ng) {
    'use strict';

    ng.module('rating')
      .directive('rating', function () {
          return {
              require: ['rating','?ngModel'],
              restrict: 'A',
              scope: true,
              controller: 'RatingCtrl',
              controllerAs: 'rating',
              bindToController: true,
              link: function (scope, element, attrs, ctrls) {

                  var rating = ctrls[0];
                  var ngModel = ctrls[1];

                  rating.max = parseInt(attrs.max);
                  rating.readonly = attrs.readonly != null ? attrs.readonly === 'true' : false;
                  rating.current = parseInt(attrs.current);
                  rating.url = attrs.url;
                  rating.objId = attrs.objId;
                  rating.rateBinding = attrs.rateBinding;

                  if (rating.readonly === false) {
                      element[0].addEventListener('mouseover', function (event) {
                          if (event.target.classList.contains('rating-item') === true) {
                              rating.change(parseInt(event.target.getAttribute('data-index')));
                              scope.$digest();
                          }
                      });

                      element[0].addEventListener('mouseleave', function (event) {
                          rating.change(-1);
                          scope.$digest();
                      });

                      element[0].addEventListener('click', function (event) {
                          if (event.target.classList.contains('rating-item') === true) {
                              if (ngModel != null) {
                                  ngModel.$setViewValue(parseInt(event.target.getAttribute('data-index')) + 1);
                              }
                              rating.select(parseInt(event.target.getAttribute('data-index')) + 1);
                              scope.$digest();
                          }
                      });
                  }
              }
          }
      });
})(window.angular);