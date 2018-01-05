define(['app/utils'], function (_utils) {
    'use strict';

    var exports = {};
    Object.defineProperty(exports, "__esModule", {
        value: true
    });

    var _utils2 = _interopRequireDefault(_utils);

    function _interopRequireDefault(obj) {
        return obj && obj.__esModule ? obj : {
            default: obj
        };
    }

    var service = {};
    function baseApiUrl() {
        return _utils2.default.urlContent('/api/debtsettlements/');
    }

    service.getTask = function (id) {
        return $.when($.ajax({
            url: baseApiUrl() + id,
            type: 'GET'
        }));
    };

    service.duplicateTask = function (id) {
        return $.when($.ajax({
            url: baseApiUrl() + id + '/duplicate',
            type: 'PUT'
        }));
    };

    service.setTaskState = function (taskId, newState, comment) {
        return $.when($.ajax({
            url: baseApiUrl() + taskId + '/states',
            type: 'PUT',
            data: {
                StateName: newState,
                Comment: comment
            }
        }));
    };

    service.closeTask = function (id) {
        return $.when($.ajax({
            url: baseApiUrl() + id,
            type: 'DELETE'
        }));
    };

    service.prolongatePlannedDate = function (taskId, newDate, comment) {
        var dateStr = $.format.date(newDate, 'yyyy-MM-dd');
        return $.when($.ajax({
            url: baseApiUrl() + taskId + '/prolongate',
            type: 'POST',
            data: { Date: dateStr, Comment: comment || '' }
        }));
    };

    service.executeCommand = function (taskId, commandName, parameters) {
        return $.when($.ajax({
            url: baseApiUrl() + taskId + '/commands',
            type: 'PUT',
            contentType: "application/json",
            data: JSON.stringify({
                CommandName: commandName,
                Parameters: parameters
            })
        }));
    };

    service.getAvailableCommands = function (taskId) {
        return $.when($.ajax({
            url: baseApiUrl() + taskId + '/commands',
            type: 'GET'
        }));
    };

    service.getUserInfo = function (value) {
        return $.when($.ajax({
            url: _utils2.default.urlContent('/api/users/') + value,
            type: 'GET'
        }));
    };

    service.getActionInfo = function (type, actionId) {
        return $.when($.ajax({
            url: _utils2.default.urlContent('/api/agreements/actions/' + type + '/' + actionId),
            type: 'GET'
        }));
    };

    service.getHistoryItem = function (taskId, historyId) {
        return $.when($.ajax({
            url: baseApiUrl() + taskId + '/history/' + historyId,
            type: 'GET'
        }));
    };

    exports.default = service;
    return exports.default;
});