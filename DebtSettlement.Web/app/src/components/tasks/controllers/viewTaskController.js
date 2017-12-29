import utils from 'app/utils';
import tasksService from 'app/components/tasks/services/tasksService';
import WorkflowCommandsPanelController from 'app/components/tasks/controllers/WorkflowCommandsPanelController';
import htmlHelpers from 'app/components/tasks/helpers/htmlHelpers';
import TaskHistoryController from 'app/components/tasks/controllers/taskHistoryController';

export default function ViewTaskController() {
    let vm = this;
    vm.taskId = null;
    vm.template = '[ng-controller=ViewTaskController]';
    vm.task = null;
    vm.select2AvailableStates = null;
    vm.changeWF = {
        newState: null,
        comment: ''
    };

    vm.renderUserInfo = function (value, element) {
        let result = '';
        if (value) {
            result = '<span class="glyphicon glyphicon-refresh animated rotate"></span>';
            tasksService.getUserInfo(value).then(
                function (data) {
                    if (data) {
                        element.html(data.FullName + ' \\ ' + value);
                    } else {
                        element.html(value);
                    }
                },
                function () {
                    element.html(value);
                });
        }
        return result;
    };

    vm.renderActionInfo = function (value, element) {
        let result = '';
        if (value && element.data('action-type')) {
            result = '<span class="glyphicon glyphicon-refresh animated rotate"></span>';
            tasksService.getActionInfo(element.data('action-type'), value).then(
                function (data) {
                    if (data) {
                        element.html(value + ' - ' + data.Name);
                    } else {
                        element.html(value);
                    }
                },
                function () {
                    element.html(value);
                });
        }
        return result;
    };

    vm.refreshTaskForm = function () {
        refreshTaskForm(vm.taskId);
    };

    vm.showTaskHistory = function () {
        let dialog = utils.ui.dialog({
            //url: utils.urlContent('/task/history?id=' + vm.taskId + '&isPartial=true'),
            open: function() {
                let historyController = new TaskHistoryController();
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
            utils.ui.error({ text: 'Выберите новый статус!' });
            return;
        }
        if (!vm.changeWF.comment) {
            utils.ui.error({ text: 'Введите коментарий!' });
            return;
        }
        let form = getForm();
        utils.ui.loader(form);
        tasksService.setTaskState(vm.taskId, vm.changeWF.newState, vm.changeWF.comment).then(
            function () {
                utils.ui.loader(form, false);
                utils.ui.success({
                    text: 'Статус успешно изменен'
                });
                refreshTaskForm(vm.taskId);
            },
            function () {
                showDefaultError(form);
            }
        );
    };
    vm.setTask = function (data) {
        vm.taskId = data.ID;
        vm.task = data;
        vm.task.ProcessInstance = vm.task.ProcessInstance || {};
        if (data.AvailableStates) {
            vm.select2AvailableStates = data.AvailableStates.map(function (row) {
                return { id: row.Name, text: row.VisibleName }
            });

        }
    };

    vm.duplicateTask = function () {
        utils.ui.confirm({
            text: 'Вы уверены что хотите продублировать задачу',
            func: function () {
                var form = getForm();
                utils.ui.loader(form);
                tasksService.duplicateTask(vm.taskId).then(
                    function () {
                        utils.ui.loader(form, false);
                        utils.ui.success({
                            text: 'Задача успешно продублирована'
                        });
                    },
                    function () {
                        showDefaultError(form);
                    }
                );
            }
        });
    };

    vm.generateStatusTemplate = function(status) {
        let prefix = '<span class="';
        let className = htmlHelpers.generateLabelClassByStatus(status);
        return prefix + className + '">' + status + '</span>';
    };

    function showDefaultError(form) {
        utils.ui.loader(form, false);
        utils.ui.error({
            text: 'Что-то пошло не так. Случилась ошибка при отправке запроса'
        });
    }

    function parseDate(dateStr) {
        let dateArray = dateStr.split('/');
        let date = new Date(dateArray[2], parseInt(dateArray[1]) - 1, dateArray[0]);
        return date;
    }

    vm.init = init;

    function refreshTaskForm(taskId) {
        let form = getForm();
        form.bindings('destroy');
        form.unbind();
        utils.ui.loader(form);
        form.load(utils.urlContent('/task/item/' + taskId + '?isPartial=true'), function () {
            utils.ui.loader(form, false);
            init(vm.template, taskId);
        });
    }
    function getForm() {

        let form = $(vm.template);//.find('[ng-controller=ViewTaskController]').parent();
        return form;
    }

    function getParameter(paramsArray, paramName) {
        if (paramsArray) {
            for (let i = 0; i < paramsArray.length; i++) {
                if (paramsArray[i].Name === paramName) {
                    return paramsArray[i];
                }
            }
        }
        return null;
    }   

    function bindNewParameterElement(params, parameterName) {
            
        let parameter = getParameter(params, parameterName);
        if (parameter !== null && typeof parameter !== 'undefined') {
            let form = getForm();
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
        let form = getForm();
        let template = form.find('#workflowCommandsPanel');
        if (commands && commands.length > 3){
            let lineCommands = commands.slice(0, 3);
            let lineTemplate = template.find('.inline-command-panel');
            let lineCtrl = new WorkflowCommandsPanelController(lineTemplate, lineCommands);
            lineCtrl.on('submit', function () { refreshTaskForm(vm.taskId); });
            lineCtrl.init();

            let ddlCommands = commands.slice(3);
            let ddlTemplate = template.find('.ddl-command-panel').removeClass('hidden').find('.addition-dropdown-menu');
            let ddlCtrl = new WorkflowCommandsPanelController(ddlTemplate, ddlCommands);
            ddlCtrl.on('submit', function () { refreshTaskForm(vm.taskId); });
            ddlCtrl.init();            
            
            return
        }
        let ctrl = new WorkflowCommandsPanelController(template, commands);
        ctrl.on('submit', function () { refreshTaskForm(vm.taskId); });
        ctrl.init();
    }

    function init(template, id) {
        vm.template = template;

        let form = getForm();
        id = id || form.data('task-id') || utils.parseUrlParams(window.document.location.href).id;

        utils.ui.loader(form.find('[ng-controller=ViewTaskController]'));
        tasksService.getTask(id).then(
            function (data) {
                utils.ui.loader(form.find('[ng-controller=ViewTaskController]'), false);
                data.Agreement = data.Agreement || {};
                vm.setTask(data);

                let persistanceParams = [];
                data.ProcessInstance = data.ProcessInstance || {};
                data.ProcessInstance.ProcessParameters = data.ProcessInstance.ProcessParameters || [];
                data.ProcessInstance.ProcessParameters.map(function (row) {
                    if (row.Purpose === 'Persistence') { persistanceParams.push(row); }
                });
                bindTaskParameters(persistanceParams);

                bindCommandsPanel(data.AvailableCommands);
                form.bindings('create')(vm);
            },
            function () {
                showDefaultError(form);
            }
        );
    }
};
