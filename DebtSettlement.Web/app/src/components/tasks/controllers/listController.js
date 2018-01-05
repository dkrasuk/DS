import tasksService from 'app/components/tasks/services/tasksService';
import htmlHelpers from 'app/components/tasks/helpers/htmlHelpers';
import utils from 'app/utils';
import WorkflowCommandsPanelController from 'app/components/tasks/controllers/WorkflowCommandsPanelController';
import ViewTaskController from 'app/components/tasks/controllers/viewTaskController';
//import navigation from 'app/components/shared/navigation';
import TaskHistoryController from 'app/components/tasks/controllers/taskHistoryController';


export default (function () {

    let defaultDateFotmat = 'dd.MM.yyyy';
    let baseApiUrl = '/api/debtsettlements/';
    let gridSelector = '#table-tasks';
    let gridFilter;
    let mainGrid;
    let $table;

    let tasksActions = {
        view: function (id) {
            let dialog = utils.ui.dialog({
                content: {
                    url: utils.urlContent('/debtsettlements/item?id=' + id + '&isPartial=true'),
                    onload: function() {
                        let ctrl = new ViewTaskController();
                        ctrl.init(dialog.find('.modal-body'), id);
                    }
                },
                close: function () {
                    refreshTasksGrid();
                },
                width: 1200, 
                height: 600,
                title: 'Просмотр задачи'
            });
            //dialog.find('.modal-body').data('task-id', id);
        },
        duplicate: function (id) {
            utils.ui.confirm({
                text: 'Вы уверены что хотите продублировать задачу?',
                func: function () {
                    utils.ui.loader($table);
                    tasksService.duplicateTask(id).then(
                        function () {
                            utils.ui.loader($table, false);
                            utils.ui.success({
                                text: 'Задача успешно продублирована.'
                            });
                            refreshTasksGrid();
                        },
                        function () {
                            showDefaultError($table);
                        }
                    );
                }
            });
        },
        showHistory: function (taskId) {
            let dialog = utils.ui.dialog({
                //url: utils.urlContent('/task/history?id=' + taskId + '&isPartial=true'),
                open: function() {
                    let historyController = new TaskHistoryController();
                    historyController.init($(this).find('.modal-body'), taskId);
                },
                width: 1100,
                height: 550,
                id: 'viewTaskHistoryDialog',
                title: 'История задачи'
            });
            dialog.find('.modal-body').data('task-id', taskId);
        },

        close: function (id) {
            utils.ui.confirm({
                text: 'Вы уверены что хотите закырть задачу?',
                func: function () {
                    utils.ui.loader($table);
                    tasksService.closeTask(id).then(
                        function () {
                            utils.ui.loader($table, false);
                            utils.ui.success({ text: 'Задача успешно закрыта' });
                            refreshTasksGrid();
                        },
                        function () {
                            showDefaultError($table);
                        }
                    );
                }
            });
        }
    };

    function showDefaultError(form) {
        utils.ui.loader(form, false);
        utils.ui.error({
            text: 'Что-то пошло не так. Случилась ошибка при отправке запроса'
        });
    }

    let tableHelpers = {
        renderActionsMenu: function (value, display, data) {
            let content = `<div class="dropdown" data-placement-range=".dataTables_scrollBody">
                      <a class="color-blue btn btn-d-v btn-circle dots-vertical dropdown-toggle"
                            data-action-menu
                            data-task-id="` + data.ID + `"
                            type="button"
                            data-toggle="dropdown"></a>`;

            content += `<ul class="dropdown-menu">
                              <li><a href="#" data-view-task="" data-task-id="` + data.ID + `">
                                      <span class="glyphicon glyphicon-search"></span> Просмотреть</a></li>
                              <li><a href="#" data-duplicate-task="" data-task-id="` + data.ID + `">
                                      <span class="glyphicon glyphicon-duplicate"></span> Копировать</a></li>
                              <li><a href="#" data-show-history="" data-task-id="` + data.ID + `">
                                      <span class="glyphicon glyphicon-list-alt"></span> История</a></li>
                              <li class="divider"></li>
                              <li><div class="addition-dropdown-menu">
                                    <span class="glyphicon glyphicon-refresh animated rotate"></span></div></li>
                        </ul>`;

            content += '</div>';
            return content;
        }
    };
    
    function getForm() {
        return $('[ng-conroller="ViewTaskListController"]');
    }

    function bindDropdownDropup(form) {
        form.on('show.bs.dropdown', '.dropdown', function () {
            let $this = $(this);
            let $listHolder = $this.find('.dropdown-menu');
            $this.data('parent', $this.parent());
            let placement, placementHeight, whereThisEnd;

            if ((placement = $this.data("placement-range"))) {
                placementHeight = $(placement).innerHeight() - 10; //10 is for scrollbar. need to replace it with some logic.
                whereThisEnd = $this.position().top + $this.height() + $listHolder.height() + 50;

                if (whereThisEnd > placementHeight)
                    $this.addClass("dropup");
                else
                    $this.removeClass("dropup");
            }

            form.append($this.css({
                position: 'absolute',
                left: 10,
                top: $this.offset().top
            }).detach());
            $listHolder.data("open", true);
        });
        form.on('hidden.bs.dropdown', '.dropdown', function () {
            let $this = $(this);
            let $listHolder = $this.find(".dropdown-menu");
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
        let form = getForm();
        form.on('click', '[data-action-menu]', function (event) {
            event.preventDefault();
            let $this = $(this);
            let template = $this.parent().find('.addition-dropdown-menu');

            tasksService.getAvailableCommands($this.data('task-id')).then(
                function (data) {
                    let ctrl = new WorkflowCommandsPanelController(template, data);
                    ctrl.on('submit', function () { refreshTasksGrid(); });
                    ctrl.init();
                },
                function () {
                    utils.ui.notify('Ошибка!', 'Не получилось загрузить список действий :(', 'error');
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
        utils.dataTable.bindGridButtons(mainGrid);

    }

    function init() {
        window.addEventListener('message', function (event) {
            if (event.data && event.data.event === 'taskCreated') {
                refreshTasksGrid();
            }
        });

        let form = getForm();

        form.tooltip({
            selector: '[data-toggle="tooltip"]'
        });

        if (window.document.location.href.indexOf('#') !== -1) {
            let filter = { hash: window.document.location.href.split('#')[1] };
            setTasksGridFilter(filter);
            processGridFilter(filter);
        }

        $table = $(gridSelector);

        $table.on('init.dt', function () { bindTableActions(); });
        $table.on('preXhr.dt', function () { utils.ui.loader(form); });
        $table.on('xhr.dt', function () {
            utils.ui.loader(form, false);
        });
        $table.on('xhrError.dt', function () {
            utils.ui.notify('Ошибочка!', ' Не удалось загрузить данные :(', 'error');
            utils.ui.loader(form, false);
        });
        let gridParameters = {
            scrollX: true,
            scrollY: "440px",
            fnServerData: fnServerOData,
            bUseODataViaJSONP: false,
            sPaginationType: "full_numbers",
            bProcessing: true,
            bServerSide: true,
            sAjaxSource: utils.urlContent(baseApiUrl),
            iODataVersion: 3,
            pagingType: 'simple',
            processing: false,
            language: {
                processing: "",
                search: "Поиск:",
                lengthMenu: "Показать _MENU_ записей",
                info: '',//"Записи с _START_ до _END_ из _TOTAL_ записей",
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
            columnDefs: [
                {
                    "targets": [3],
                    "visible": false
                },
                {
                    "targets": [4],
                    "visible": false
                },
                {
                    "targets": [5],
                    "visible": false
                },
                {
                    "targets": [6],
                    "visible": false
                },
                {
                    "targets": [12],
                    "visible": false
                }
            ],
            columns: [
                {
                    title: 'ID',
                    name: 'ID',
                    data: 'ProcessIdGuid',
                    searchable: false
                },
                {
                    title: 'Дата входа в этап',
                    name: 'StateEnterDate',
                    data: '',
                    type: 'date'
                },
                {
                    title: 'Номер договора',
                    name: 'AgreemNumber',
                    data: 'AgreemNumber',
                    type: 'string'
                },
                {
                    title: 'ФИО должника',
                    name: 'FIO',
                    data: 'FIO',
                    type: 'string'
                },
                {
                    title: 'Outstanding',
                    name: 'Outstanding',
                    data: 'Outstanding',
                    type: 'number'
                },
                {
                    title: 'Сумма погашения ',
                    name: 'RepaymentAmount ',
                    data: '',
                    type: 'number'
                },
                {
                    title: 'Регион',
                    name: 'Region',
                    data: 'Region',
                    type: 'string'
                },
                {
                    title: 'Инициатор',
                    name: 'Initiator',
                    data: '',
                    type: 'string'
                },
                {
                    title: 'Ответственный',
                    name: 'Responsible',
                    data: '',
                    type: 'string'
                },
                {
                    title: 'Ответственное подразделение',
                    name: 'ResponsibleRoles',
                    data: '',
                    type: 'string'
                },
                {
                    title: 'Этап процесса',
                    name: 'ProcessState',
                    data: 'Status',
                    type: 'string'
                },
                {
                    title: 'Время нахождения в этапе',
                    name: 'StateDaysCount',
                    data: '',
                    type: 'string'
                },
                {
                    title: 'Индикатор нахождения в этапе',
                    name: 'StateIndicator',
                    data: '',
                    type: 'string'
                },
                {
                    title: 'Ответственный сотрудник ОРК',
                    name: 'OrkResponsible',
                    data: '',
                    type: 'string'
                }
            ]
        };
  
        mainGrid = $table.dataTable(gridParameters);
    }

    function processGridFilter(filter) {
        processFilterHash(filter);
    }

    function processFilterHash(filter) {
        if (filter.hash) {
            baseApiUrl = '/api/debtsettlements/' + filter.hash
                .replace('sysdatetime', 'DateTime\'' + (new Date()).toISOString() + '\'')
                .replace('sysdate', 'DateTime\'' + (new Date()).toISOString().substr(0, 10) + '\'');
        } else {
            baseApiUrl = '/api/debtsettlements/';
        }
    }

    function parseDate(date, format) {
        if (date) {
            return $.format.date(date, (format || defaultDateFotmat));
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
        refreshTasksGrid(utils.urlContent(baseApiUrl));
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
}());

