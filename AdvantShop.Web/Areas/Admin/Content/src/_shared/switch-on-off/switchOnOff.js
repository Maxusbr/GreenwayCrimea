; (function (ng) {
    'use strict';

    var idIncrement = 0

    ng.module('switchOnOff', [])
      .component('switchOnOff', {
          template: '<div class="onoffswitch"><input data-e2e="switchOnOffInput" id="{{::$ctrl.id}}" name="{{::$ctrl.id}}" type="checkbox" ng-model="$ctrl.checked" ng-change="$ctrl.onChange({checked:$ctrl.checked})" class="onoffswitch-checkbox" value="{{$ctrl.checked ? \'true\' : \'false\'}}"><label data-e2e="switchOnOffLabel" for="{{::$ctrl.id}}" class="onoffswitch-label"><span class="onoffswitch-inner"><span class="onoffswitch-inner-on">Вкл</span><span class="onoffswitch-inner-off">Выкл</span></span><span class="onoffswitch-switch"></span></label></div>',
          controller: function () {
              if (this.id == null || this.id.length === 0) {
                  this.id = 'switchOnOff_' + idIncrement;
                  idIncrement = idIncrement + 1;
              }
          },
          bindings: {
              checked: '<?',
              onChange: '&',
              id: '@'
          }
      })

})(window.angular);