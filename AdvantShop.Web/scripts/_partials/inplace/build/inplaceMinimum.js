!function(e){"use strict";window.angular.module("inplace",[]).constant("inplaceRichConfig",{title:!1,allowedContent:!0,autoParagraph:!1,removePlugins:"dragdrop, basket",floatSpaceDockedOffsetY:5,toolbar:[{name:"source",items:["Sourcedialog","Templates"]},{name:"elements",items:["NumberedList","BulletedList","Link","Unlink","-","Image","Flash","Table","HorizontalRule"]},{name:"styles",items:["Format","Font","FontSize","lineheight"]},"/",{name:"text",items:["Bold","Italic","Underline","Strike","Subscript","RemoveFormat"]},{name:"text",items:["TextColor","BGColor"]},{name:"align",items:["Outdent","Indent","JustifyLeft","JustifyCenter","JustifyRight","JustifyBlock"]},{name:"document",items:["PasteFromWord","Undo","Redo","Scayt"]}],extraPlugins:"scriptencode,sourcedialog,codemirror,lineheight",codemirror:{}})}(),function(e){"use strict";var t=function(e){var t=this;t.$onInit=function(){t.state=e.getProgressState()}};window.angular.module("inplace").controller("InplaceProgressCtrl",t),t.$inject=["inplaceService"]}(),function(e){"use strict";var t=function(e,t){this.change=function(n){t.setEnable(n).then(function(t){!0===t&&e.location.reload()})}};window.angular.module("inplace").controller("InplaceSwitchCtrl",t),t.$inject=["$window","inplaceService"]}(),function(e){"use strict";var t=function(t){var n=this,i={show:!1},c={rich:{},inplaceAutocomplete:{},inplaceImage:{},inplacePrice:{}};n.addRich=function(e,t){c.rich[e]=t},n.getRich=function(e){return c.rich[e]},n.addInplaceAutocomplete=function(e,t){c.inplaceAutocomplete[e]=t},n.getInplaceAutocomplete=function(e){return c.inplaceAutocomplete[e]},n.addInplaceImage=function(e,t){c.inplaceImage[e]=t},n.getInplaceImage=function(e){return c.inplaceImage[e]},n.addInplacePrice=function(e,t){c.inplacePrice[e]=t},n.getInplacePrice=function(e){return c.inplacePrice[e]},n.save=function(n,i){return t.post(n,e.extend(i,{rnd:Math.random()}))},n.setEnable=function(e){return t.post("inplaceeditor/setenable",{isEnabled:e}).then(function(e){return e.data})},n.getProgressState=function(){return i},n.startProgress=function(){i.show=!0},n.stopProgress=function(){i.show=!1}};e.module("inplace").service("inplaceService",t),t.$inject=["$http"]}(window.angular),function(e){"use strict";e.module("inplace").directive("inplaceStart",["$compile","$rootScope",function(e,t){return{restrict:"A",scope:{},link:function(n,i,c,a){var o=document.querySelectorAll("[data-inplace-rich], [data-inplace-modal], [data-inplace-image], [data-inplace-autocomplete], [data-inplace-properties-new], [data-inplace-image], [data-inplace-price], [data-inplace-price-panel], [data-inplace-switch]");null!=o&&o.length>0&&e(o)(t)}}}]),e.module("inplace").directive("inplaceSwitch",[function(e){return{restrict:"A",scope:!0,controller:"InplaceSwitchCtrl",controllerAs:"inplaceSwitch",bindToController:!0}}]),e.module("inplace").directive("inplaceProgress",function(){return{restrict:"A",scope:{},controller:"InplaceProgressCtrl",controllerAs:"inplaceProgress",bindToController:!0,replace:!0,template:'<div class="inplace-progress icon-spinner-before icon-animate-spin-before" data-ng-if="inplaceProgress.state.show === true"></div>'}})}(window.angular);