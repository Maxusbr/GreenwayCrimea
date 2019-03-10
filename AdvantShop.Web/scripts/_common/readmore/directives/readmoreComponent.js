; (function (ng) {
    'use strict';

    if (document.querySelector('html').classList.contains('inplace-enabled')) {
        return;
    }

    ng.module('readmore')
      .component('readmore', {
          controller: 'ReadmoreCtrl',
          template: '<div data-ng-class="{\'readmore-expanded\' : $ctrl.expanded, \'readmore-collapsed\' : !$ctrl.expanded}"><div class="readmore-content" data-ng-style="{maxHeight: $ctrl.maxHeight, transitionDuration: $ctrl.speed}"><div data-ng-transclude></div><div ng-bind-html="$ctrl.content"></div></div><div class="readmore-controls" data-ng-if="$ctrl.isActive"><a class="readmore-link" href="" data-ng-click="$ctrl.switch($ctrl.expanded)">{{$ctrl.text | translate}}</a></div></div>',
          transclude: true,
          bindings: {
              expanded: '<?',
              maxHeight: '<?',
              content: '<?',
              speed: '@',
              moreText: '@',
              lessText: '@',
          }
      });

})(window.angular);