; (function (ng) {

    'use strict';

    ng.module('modal', [])
      .constant('modalDefaultOptions', {
          id: undefined,
          isFloating: false,
          crossEnable: true,
          backgroundEnable: true,
          closeOut: true,
          isOpen: false,
          closeEsc: true,
          modalClass: undefined,
          modalOverlayClass: undefined,
          callbackInit: undefined,
          callbackOpen: undefined,
          callbackClose: undefined,
          startOpenDelay: undefined,
          zIndex: 999
      });

})(angular);