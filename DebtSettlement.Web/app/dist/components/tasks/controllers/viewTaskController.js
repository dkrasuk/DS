define(['app/utils', 'app/components/tasks/services/tasksService', 'app/components/tasks/controllers/WorkflowCommandsPanelController', 'app/components/tasks/helpers/htmlHelpers', 'app/components/tasks/controllers/taskHistoryController'], function (_utils, _tasksService, _WorkflowCommandsPanelController, _htmlHelpers, _taskHistoryController) {
    'use strict';

    var exports = {};
    Object.defineProperty(exports, "__esModule", {
        value: true
    });
    exports.default = ViewTaskController;

    var _utils2 = _interopRequireDefault(_utils);

    var _tasksService2 = _interopRequireDefault(_tasksService);

    var _WorkflowCommandsPanelController2 = _interopRequireDefault(_WorkflowCommandsPanelController);

    var _htmlHelpers2 = _interopRequireDefault(_htmlHelpers);

    var _taskHistoryController2 = _interopRequireDefault(_taskHistoryController);

    function _interopRequireDefault(obj) {
        return obj && obj.__esModule ? obj : {
            default: obj
        };
    }

    function ViewTaskController() {
        var vm = this;
        vm.taskId = null;
        vm.template = '[ng-controller=ViewTaskController]';
        vm.task = null;
        vm.select2AvailableStates = null;
        vm.changeWF = {
            newState: null,
            comment: ''
        };

        vm.renderUserInfo = function (value, element) {
            var result = '';
            if (value) {
                result = '<span class="glyphicon glyphicon-refresh animated rotate"></span>';
                _tasksService2.default.getUserInfo(value).then(function (data) {
                    if (data) {
                        element.html(data.FullName + ' \\ ' + value);
                    } else {
                        element.html(value);
                    }
                }, function () {
                    element.html(value);
                });
            }
            return result;
        };

        vm.renderActionInfo = function (value, element) {
            var result = '';
            if (value && element.data('action-type')) {
                result = '<span class="glyphicon glyphicon-refresh animated rotate"></span>';
                _tasksService2.default.getActionInfo(element.data('action-type'), value).then(function (data) {
                    if (data) {
                        element.html(value + ' - ' + data.Name);
                    } else {
                        element.html(value);
                    }
                }, function () {
                    element.html(value);
                });
            }
            return result;
        };

        vm.refreshTaskForm = function () {
            refreshTaskForm(vm.taskId);
        };

        vm.showTaskHistory = function () {
            var dialog = _utils2.default.ui.dialog({
                //url: utils.urlContent('/task/history?id=' + vm.taskId + '&isPartial=true'),
                open: function open() {
                    var historyController = new _taskHistoryController2.default();
                    historyController.init($(this).find('.modal-body'), vm.taskId);
                },
                width: 1100,
                height: 550,
                id: 'viewTaskHistoryDialog',
                title: 'История задачи'
            });
            //dialog.find('.modal-body').data('task-id', vm.taskId);
        };

        vm.setTaskState = function () {
            if (!vm.changeWF.newState) {
                _utils2.default.ui.error({ text: 'Выберите новый статус!' });
                return;
            }
            if (!vm.changeWF.comment) {
                _utils2.default.ui.error({ text: 'Введите коментарий!' });
                return;
            }
            var form = getForm();
            _utils2.default.ui.loader(form);
            _tasksService2.default.setTaskState(vm.taskId, vm.changeWF.newState, vm.changeWF.comment).then(function () {
                _utils2.default.ui.loader(form, false);
                _utils2.default.ui.success({
                    text: 'Статус успешно изменен'
                });
                refreshTaskForm(vm.taskId);
            }, function () {
                showDefaultError(form);
            });
        };
        vm.setTask = function (data) {
            vm.taskId = data.ID;
            vm.task = data;
            vm.task.ProcessInstance = vm.task.ProcessInstance || {};
            if (data.AvailableStates) {
                vm.select2AvailableStates = data.AvailableStates.map(function (row) {
                    return { id: row.Name, text: row.VisibleName };
                });
            }
        };

        vm.duplicateTask = function () {
            _utils2.default.ui.confirm({
                text: 'Вы уверены что хотите продублировать задачу',
                func: function func() {
                    var form = getForm();
                    _utils2.default.ui.loader(form);
                    _tasksService2.default.duplicateTask(vm.taskId).then(function () {
                        _utils2.default.ui.loader(form, false);
                        _utils2.default.ui.success({
                            text: 'Задача успешно продублирована'
                        });
                    }, function () {
                        showDefaultError(form);
                    });
                }
            });
        };

        vm.generateStatusTemplate = function (status) {
            var prefix = '<span class="';
            var className = _htmlHelpers2.default.generateLabelClassByStatus(status);
            return prefix + className + '">' + status + '</span>';
        };

        function showDefaultError(form) {
            _utils2.default.ui.loader(form, false);
            _utils2.default.ui.error({
                text: 'Что-то пошло не так. Случилась ошибка при отправке запроса'
            });
        }

        function parseDate(dateStr) {
            var dateArray = dateStr.split('/');
            var date = new Date(dateArray[2], parseInt(dateArray[1]) - 1, dateArray[0]);
            return date;
        }

        vm.init = init;

        function refreshTaskForm(taskId) {
            var form = getForm();
            form.bindings('destroy');
            form.unbind();
            _utils2.default.ui.loader(form);
            form.load(_utils2.default.urlContent('/task/item/' + taskId + '?isPartial=true'), function () {
                _utils2.default.ui.loader(form, false);
                init(vm.template, taskId);
            });
        }
        function getForm() {

            var form = $(vm.template); //.find('[ng-controller=ViewTaskController]').parent();
            return form;
        }

        function getParameter(paramsArray, paramName) {
            if (paramsArray) {
                for (var i = 0; i < paramsArray.length; i++) {
                    if (paramsArray[i].Name === paramName) {
                        return paramsArray[i];
                    }
                }
            }
            return null;
        }

        function bindNewParameterElement(params, parameterName) {

            var parameter = getParameter(params, parameterName);
            if (parameter !== null && typeof parameter !== 'undefined') {
                var form = getForm();
                form.find('.new-' + parameterName).removeClass('hidden');
                vm.task[parameterName] = parameter.Value;
            }
        }

        function bindTaskParameters(params) {
            bindNewParameterElement(params, 'NewPlannedDate');
            bindNewParameterElement(params, 'NewObserver');
            bindNewParameterElement(params, 'NewResponsible');
            bindNewParameterElement(params, 'NewStatus');
        }

        function bindCommandsPanel(commands) {
            var form = getForm();
            var template = form.find('#workflowCommandsPanel');
            if (commands && commands.length > 3) {
                var lineCommands = commands.slice(0, 3);
                var lineTemplate = template.find('.inline-command-panel');
                var lineCtrl = new _WorkflowCommandsPanelController2.default(lineTemplate, lineCommands);
                lineCtrl.on('submit', function () {
                    refreshTaskForm(vm.taskId);
                });
                lineCtrl.init();

                var ddlCommands = commands.slice(3);
                var ddlTemplate = template.find('.ddl-command-panel').removeClass('hidden').find('.addition-dropdown-menu');
                var ddlCtrl = new _WorkflowCommandsPanelController2.default(ddlTemplate, ddlCommands);
                ddlCtrl.on('submit', function () {
                    refreshTaskForm(vm.taskId);
                });
                ddlCtrl.init();

                return;
            }
            var ctrl = new _WorkflowCommandsPanelController2.default(template, commands);
            ctrl.on('submit', function () {
                refreshTaskForm(vm.taskId);
            });
            ctrl.init();
        }

        function init(template, id) {
            vm.template = template;

            var form = getForm();
            id = id || form.data('task-id') || _utils2.default.parseUrlParams(window.document.location.href).id;

            _utils2.default.ui.loader(form.find('[ng-controller=ViewTaskController]'));
            _tasksService2.default.getTask(id).then(function (data) {
                _utils2.default.ui.loader(form.find('[ng-controller=ViewTaskController]'), false);
                data.Agreement = data.Agreement || {};
                vm.setTask(data);

                var persistanceParams = [];
                data.ProcessInstance = data.ProcessInstance || {};
                data.ProcessInstance.ProcessParameters = data.ProcessInstance.ProcessParameters || [];
                data.ProcessInstance.ProcessParameters.map(function (row) {
                    if (row.Purpose === 'Persistence') {
                        persistanceParams.push(row);
                    }
                });
                bindTaskParameters(persistanceParams);

                bindCommandsPanel(data.AvailableCommands);
                form.bindings('create')(vm);
            }, function () {
                showDefaultError(form);
            });
        }
    };
    return exports.default;
});