; (function (ng) {
    'use strict';

    /*
     * <help-trigger class="ng-cloak" data-title="фывфывфывфыв">
            <strong>sdasfasdas</strong><br />
            <i>asdfsdasd</i>
        </help-trigger>
     */
    var increment = 1;

    ng.module('helpTrigger')
      .directive('helpTrigger', ['$sce', '$templateRequest', '$compile', '$templateCache', '$cacheFactory', function ($sce, $templateRequest, $compile, $templateCache, $cacheFactory) {
          return {
              controller: 'HelpTriggerCtrl',
              bindToController: true,
              controllerAs: '$ctrl',
              transclude: true,
              //scope: {
              //    title: '@',
              //    useTemplate: '<?'
              //},
              scope: true,
              link: function (scope, element, attrs, ctrl, transclude) {

                  ctrl.title = attrs.title;
                  ctrl.useTemplate = attrs.useTemplate ? attrs.useTemplate === 'true' : false;

                  $templateRequest('../areas/admin/content/src/_partials/help-trigger/templates/help-trigger.html')
                      .then(function (tpl) {
                          var innerEl = document.createElement('div'),
                              uiPopoverEl,
                              clone,
                              container;

                          innerEl.innerHTML = tpl;

                          uiPopoverEl = innerEl.querySelector('.help-trigger-icon');

                          clone = transclude();

                          container = document.createElement('div');

                          for (var i = 0; i < clone.length; i++) {
                              container.appendChild(clone[i]);
                          }

                          if (ctrl.useTemplate === true) {
                              uiPopoverEl.setAttribute('uib-popover-template', "'helpTrigger_" + increment + ".html'");
                              $templateCache.put('helpTrigger_' + increment + '.html', container.innerHTML);
                          } else {
                              uiPopoverEl.setAttribute('uib-popover-html', "$ctrl.content");
                              ctrl.content = $sce.trustAsHtml(ng.element('<div />').append(transclude()).html());
                          }

                          $compile(element.html(innerEl).contents())(scope);

                          increment++;
                      });  
              }
          }
      }]);



})(window.angular);