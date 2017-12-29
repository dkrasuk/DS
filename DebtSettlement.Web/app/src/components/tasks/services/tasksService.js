import utils from 'app/utils'

    let service = {};
    function baseApiUrl() {
        return utils.urlContent('/api/tasks/');
    }

    service.getTask = function (id) {
        return $.when($.ajax({
            url: baseApiUrl() + id,
            type: 'GET'
        }));
    };

    service.duplicateTask = function (id) {
        return $.when(
            $.ajax({
                url: baseApiUrl() + id + '/duplicate',
                type: 'PUT'
            })
        );
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
        let dateStr = $.format.date(newDate, 'yyyy-MM-dd');
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
            url: utils.urlContent('/api/users/') + value,
            type: 'GET'
        }));
    };

    service.getActionInfo = function (type, actionId) {
        return $.when($.ajax({
            url: utils.urlContent('/api/agreements/actions/' + type + '/' + actionId),
            type: 'GET'
        }));
    };

    service.getHistoryItem = function(taskId, historyId) {
        return $.when($.ajax({
            url: baseApiUrl() + taskId + '/history/' + historyId,
            type: 'GET'
        }));
    };

export default service;
