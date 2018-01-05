define(['app/components/tasks/services/tasksService', 'app/components/tasks/helpers/htmlHelpers', 'app/utils', 'app/components/tasks/controllers/WorkflowCommandsPanelController', 'app/components/tasks/controllers/viewTaskController', 'app/components/tasks/controllers/taskHistoryController'], function (_tasksService, _htmlHelpers, _utils, _WorkflowCommandsPanelController, _viewTaskController, _taskHistoryController) {
    'use strict';

    var exports = {};
    Object.defineProperty(exports, "__esModule", {
        value: true
    });

    var _tasksService2 = _interopRequireDefault(_tasksService);

    var _htmlHelpers2 = _interopRequireDefault(_htmlHelpers);

    var _utils2 = _interopRequireDefault(_utils);

    var _WorkflowCommandsPanelController2 = _interopRequireDefault(_WorkflowCommandsPanelController);

    var _viewTaskController2 = _interopRequireDefault(_viewTaskController);

    var _taskHistoryController2 = _interopRequireDefault(_taskHistoryController);

    function _interopRequireDefault(obj) {
        return obj && obj.__esModule ? obj : {
            default: obj
        };
    }

    exports.default = function () {

        var defaultDateFotmat = 'dd.MM.yyyy';
        var baseApiUrl = '/api/debtsettlements/';
        var gridSelector = '#table-tasks';
        var gridFilter = void 0;
        var mainGrid = void 0;
        var $table = void 0;

        var tasksActions = {
            view: function view(id) {
                var dialog = _utils2.default.ui.dialog({
                    content: {
                        url: _utils2.default.urlContent('/debtsettlements/item?id=' + id + '&isPartial=true'),
                        onload: function onload() {
                            var ctrl = new _viewTaskController2.default();
                            ctrl.init(dialog.find('.modal-body'), id);
                        }
                    },
                    close: function close() {
                        refreshTasksGrid();
                    },
                    width: 1200,
                    height: 600,
                    title: 'Просмотр задачи'
                });
                //dialog.find('.modal-body').data('task-id', id);
            },
            duplicate: function duplicate(id) {
                _utils2.default.ui.confirm({
                    text: 'Вы уверены что хотите продублировать задачу?',
                    func: function func() {
                        _utils2.default.ui.loader($table);
                        _tasksService2.default.duplicateTask(id).then(function () {
                            _utils2.default.ui.loader($table, false);
                            _utils2.default.ui.success({
                                text: 'Задача успешно продублирована.'
                            });
                            refreshTasksGrid();
                        }, function () {
                            showDefaultError($table);
                        });
                    }
                });
            },
            showHistory: function showHistory(taskId) {
                var dialog = _utils2.default.ui.dialog({
                    //url: utils.urlContent('/task/history?id=' + taskId + '&isPartial=true'),
                    open: function open() {
                        var historyController = new _taskHistoryController2.default();
                        historyController.init($(this).find('.modal-body'), taskId);
                    },
                    width: 1100,
                    height: 550,
                    id: 'viewTaskHistoryDialog',
                    title: 'История задачи'
                });
                dialog.find('.modal-body').data('task-id', taskId);
            },

            close: function close(id) {
                _utils2.default.ui.confirm({
                    text: 'Вы уверены что хотите закырть задачу?',
                    func: function func() {
                        _utils2.default.ui.loader($table);
                        _tasksService2.default.closeTask(id).then(function () {
                            _utils2.default.ui.loader($table, false);
                            _utils2.default.ui.success({ text: 'Задача успешно закрыта' });
                            refreshTasksGrid();
                        }, function () {
                            showDefaultError($table);
                        });
                    }
                });
            }
        };

        function showDefaultError(form) {
            _utils2.default.ui.loader(form, false);
            _utils2.default.ui.error({
                text: 'Что-то пошло не так. Случилась ошибка при отправке запроса'
            });
        }

        var tableHelpers = {
            renderActionsMenu: function renderActionsMenu(value, display, data) {
                var content = '<div class="dropdown" data-placement-range=".dataTables_scrollBody">\n                      <a class="color-blue btn btn-d-v btn-circle dots-vertical dropdown-toggle"\n                            data-action-menu\n                            data-task-id="' + data.ID + '"\n                            type="button"\n                            data-toggle="dropdown"></a>';

                content += '<ul class="dropdown-menu">\n                              <li><a href="#" data-view-task="" data-task-id="' + data.ID + '">\n                                      <span class="glyphicon glyphicon-search"></span> \u041F\u0440\u043E\u0441\u043C\u043E\u0442\u0440\u0435\u0442\u044C</a></li>\n                              <li><a href="#" data-duplicate-task="" data-task-id="' + data.ID + '">\n                                      <span class="glyphicon glyphicon-duplicate"></span> \u041A\u043E\u043F\u0438\u0440\u043E\u0432\u0430\u0442\u044C</a></li>\n                              <li><a href="#" data-show-history="" data-task-id="' + data.ID + '">\n                                      <span class="glyphicon glyphicon-list-alt"></span> \u0418\u0441\u0442\u043E\u0440\u0438\u044F</a></li>\n                              <li class="divider"></li>\n                              <li><div class="addition-dropdown-menu">\n                                    <span class="glyphicon glyphicon-refresh animated rotate"></span></div></li>\n                        </ul>';

                content += '</div>';
                return content;
            }
        };

        function getForm() {
            return $('[ng-conroller="ViewTaskListController"]');
        }

        function bindDropdownDropup(form) {
            form.on('show.bs.dropdown', '.dropdown', function () {
                var $this = $(this);
                var $listHolder = $this.find('.dropdown-menu');
                $this.data('parent', $this.parent());
                var placement = void 0,
                    placementHeight = void 0,
                    whereThisEnd = void 0;

                if (placement = $this.data("placement-range")) {
                    placementHeight = $(placement).innerHeight() - 10; //10 is for scrollbar. need to replace it with some logic.
                    whereThisEnd = $this.position().top + $this.height() + $listHolder.height() + 50;

                    if (whereThisEnd > placementHeight) $this.addClass("dropup");else $this.removeClass("dropup");
                }

                form.append($this.css({
                    position: 'absolute',
                    left: 10,
                    top: $this.offset().top
                }).detach());
                $listHolder.data("open", true);
            });
            form.on('hidden.bs.dropdown', '.dropdown', function () {
                var $this = $(this);
                var $listHolder = $this.find(".dropdown-menu");
                $listHolder.data("open", false);

                $this.data('parent').append($this.css({
                    position: 'inherit', left: false, top: false
                }).detach());
            });
            form.find('.dataTables_scrollBody').scroll(function () {
                form.find('.dropdown.open .dropdown-toggle').dropdown('toggle');
            });
        }

        function bindTableActions() {
            var form = getForm();
            form.on('click', '[data-action-menu]', function (event) {
                event.preventDefault();
                var $this = $(this);
                var template = $this.parent().find('.addition-dropdown-menu');

                _tasksService2.default.getAvailableCommands($this.data('task-id')).then(function (data) {
                    var ctrl = new _WorkflowCommandsPanelController2.default(template, data);
                    ctrl.on('submit', function () {
                        refreshTasksGrid();
                    });
                    ctrl.init();
                }, function () {
                    _utils2.default.ui.notify('Ошибка!', 'Не получилось загрузить список действий :(', 'error');
                });
            });

            form.on('click', 'a[data-view-task]', function (event) {
                event.preventDefault();
                tasksActions.view($(this).data('task-id'));
            });
            form.on('click', 'a[data-duplicate-task]', function (event) {
                event.preventDefault();
                tasksActions.duplicate($(this).data('task-id'));
            });
            form.on('click', 'a[data-show-history]', function (event) {
                event.preventDefault();
                tasksActions.showHistory($(this).data('task-id'));
            });

            bindDropdownDropup(form);
            _utils2.default.dataTable.bindGridButtons(mainGrid);
        }

        function init() {
            window.addEventListener('message', function (event) {
                if (event.data && event.data.event === 'taskCreated') {
                    refreshTasksGrid();
                }
            });

            var form = getForm();

            form.tooltip({
                selector: '[data-toggle="tooltip"]'
            });

            if (window.document.location.href.indexOf('#') !== -1) {
                var filter = { hash: window.document.location.href.split('#')[1] };
                setTasksGridFilter(filter);
                processGridFilter(filter);
            }

            $table = $(gridSelector);

            $table.on('init.dt', function () {
                bindTableActions();
            });
            $table.on('preXhr.dt', function () {
                _utils2.default.ui.loader(form);
            });
            $table.on('xhr.dt', function () {
                _utils2.default.ui.loader(form, false);
            });
            $table.on('xhrError.dt', function () {
                _utils2.default.ui.notify('Ошибочка!', ' Не удалось загрузить данные :(', 'error');
                _utils2.default.ui.loader(form, false);
            });
            var gridParameters = {
                scrollX: true,
                scrollY: "440px",
                fnServerData: fnServerOData,
                bUseODataViaJSONP: false,
                sPaginationType: "full_numbers",
                bProcessing: true,
                bServerSide: true,
                sAjaxSource: _utils2.default.urlContent(baseApiUrl),
                iODataVersion: 3,
                pagingType: 'simple',
                processing: false,
                language: {
                    processing: "",
                    search: "Поиск:",
                    lengthMenu: "Показать _MENU_ записей",
                    info: '', //"Записи с _START_ до _END_ из _TOTAL_ записей",
                    infoEmpty: "Записи с 0 до 0 из 0 записей",
                    infoFiltered: "(отфильтровано из _MAX_ записей)",
                    infoPostFix: "",
                    loadingRecords: "Загрузка записей...",
                    zeroRecords: "Записи отсутствуют.",
                    emptyTable: "В таблице отсутствуют данные",
                    paginate: {
                        first: "Первая",
                        previous: "Предыдущая",
                        next: "Следующая",
                        last: "Последняя"
                    },
                    aria: {
                        sortAscending: ": активировать для сортировки столбца по возрастанию",
                        sortDescending: ": активировать для сортировки столбца по убыванию"
                    }
                },
                order: [[5, 'desc']],
                columnDefs: [{
                    "targets": [3],
                    "visible": false
                }, {
                    "targets": [4],
                    "visible": false
                }, {
                    "targets": [5],
                    "visible": false
                }, {
                    "targets": [6],
                    "visible": false
                }, {
                    "targets": [12],
                    "visible": false
                }],
                columns: [{
                    title: 'ID',
                    name: 'ID',
                    data: 'ID',
                    searchable: false
                }, {
                    title: 'Дата входа в этап',
                    name: 'StateEnterDate',
                    data: '',
                    type: 'date'
                }, {
                    title: 'Номер договора',
                    name: 'AgreementNumber',
                    data: '',
                    type: 'string'
                }, {
                    title: 'ФИО должника',
                    name: 'ClientFullName',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Outstanding ',
                    name: 'Outstanding ',
                    data: '',
                    type: 'number'
                }, {
                    title: 'Сумма погашения ',
                    name: 'RepaymentAmount ',
                    data: '',
                    type: 'number'
                }, {
                    title: 'Регион',
                    name: 'Region',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Инициатор',
                    name: 'Initiator',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Ответственный',
                    name: 'Responsible',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Ответственное подразделение',
                    name: 'ResponsibleRoles',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Этап процесса',
                    name: 'ProcessState',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Время нахождения в этапе',
                    name: 'StateDaysCount',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Индикатор нахождения в этапе',
                    name: 'StateIndicator',
                    data: '',
                    type: 'string'
                }, {
                    title: 'Ответственный сотрудник ОРК',
                    name: 'OrkResponsible',
                    data: '',
                    type: 'string'
                }]
            };

            mainGrid = $table.dataTable(gridParameters);
        }

        function processGridFilter(filter) {
            processFilterHash(filter);
        }

        function processFilterHash(filter) {
            if (filter.hash) {
                baseApiUrl = '/api/debtsettlements/' + filter.hash.replace('sysdatetime', 'DateTime\'' + new Date().toISOString() + '\'').replace('sysdate', 'DateTime\'' + new Date().toISOString().substr(0, 10) + '\'');
            } else {
                baseApiUrl = '/api/debtsettlements/';
            }
        }

        function parseDate(date, format) {
            if (date) {
                return $.format.date(date, format || defaultDateFotmat);
            }
            return null;
        }

        function setTasksGridFilter(filterObject) {
            gridFilter = filterObject;
        }

        function refreshTasksGrid(url) {
            if (typeof url !== 'undefined') {
                mainGrid.fnSettings().sAjaxSource = url;
                mainGrid.fnDraw();
            } else {
                mainGrid.fnDraw();
            }
        }

        function loadTasks(filter) {
            setTasksGridFilter(filter);
            processGridFilter(filter);
            refreshTasksGrid(_utils2.default.urlContent(baseApiUrl));
        }

        $(function () {
            init();
        });

        return {
            init: init,
            setTasksGridFilter: setTasksGridFilter,
            refreshTasksGrid: refreshTasksGrid,
            loadTasks: loadTasks
        };
    }();

    return exports.default;
});