define(['app/components/shared/navigation', 'app/components/tasks/controllers/listController', 'app/utils', 'app/components/tasks/controllers/viewTaskController'], function (_navigation, _listController, _utils, _viewTaskController) {
    'use strict';

    var exports = {};

    var _navigation2 = _interopRequireDefault(_navigation);

    var _listController2 = _interopRequireDefault(_listController);

    var _utils2 = _interopRequireDefault(_utils);

    var _viewTaskController2 = _interopRequireDefault(_viewTaskController);

    function _interopRequireDefault(obj) {
        return obj && obj.__esModule ? obj : {
            default: obj
        };
    }

    _utils2.default.loadCss(_utils2.default.urlContent('/app/dist/components/tasks/css/tasks.css'));

    if (window.location.href.toLowerCase().indexOf('/task/item') !== -1) {
        var controller = new _viewTaskController2.default();
        var data = _utils2.default.parseUrlParams(window.location.href).id;
        controller.init($('[ng-controller=ViewTaskController]').parent(), data);
    } else {
        _navigation2.default.init(function () {
            _listController2.default.loadTasks({ hash: (this.hash || '').replace('#', '') });
        });
    }
    return exports;
});