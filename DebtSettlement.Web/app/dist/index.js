define(['./config'], function (_config) {
  'use strict';

  var exports = {};

  var _config2 = _interopRequireDefault(_config);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  requirejs(['app/extensions', 'app/components/tasks/index']);
  return exports;
});