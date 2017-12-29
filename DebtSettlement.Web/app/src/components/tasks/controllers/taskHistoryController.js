import utils from 'app/utils';
import tasksService from 'app/components/tasks/services/tasksService';
import htmlHelpers from 'app/components/tasks/helpers/htmlHelpers';

let $table;
let historyTable;
const template = '<div ng-controller="TaskHistoryController" class="container-fluid">\
                    <div class="row">\
                        <div class="col-md-12">\
                            <table id="taskHistoryTable" style="width:100%" class="table table-bordered table-striped tasks-history-table w-100"></table>\
                        </div>\
                    </div>\
                </div>';


function parseDate(date) {
    return $.format.date(date, 'dd.MM.yyyy HH:mm:ss');
}

function getStateLabel(value) {
    return '<span data-toggle="tooltip"\
                      class="' + htmlHelpers.generateLabelClassByStatus(value) + '">\
                     ' + value + '\
               </span>';
}

function showParametersWindow(taskId, historyId) {
    tasksService.getHistoryItem(taskId, historyId).then(
        function (data) {
            utils.ui.dialog({
                title: 'Параметры вызова действия',
                content: bindParametersHtml(data)
            });
        },
        function () {
            utils.ui.error({ text: 'Ошибка загрузки данных.' });
        }
    );
}

function bindParametersHtml(params) {
    let block = '<div class="transition-and-state-container-main">\
	                        <div class="state-transition-graph-spacer" style="width: 587px;">\
	                        	<div class="transition-and-state-container">\
	                        		<div class="state-container">\
	                        			<div class="state">\
                                            ' + getStateLabel(params.FromStateName) + '\
	                        		    </div>\
	                        	    </div>\
	                            </div>\
	                            <div class="transition-and-state-container">\
	                            	<div class="transition-container" style="width:300px">\
	                            		<div class="transition-arrow">\
	                            			<div class="arrow-tail"></div>\
	                            			<div class="arrow-head"></div>\
	                            			<div class="transition-reason">' + parseDate(params.TransitionTime) + '</div>\
	                            			<div class="transition-change-info">\
	                            				<div class="identity-view-control" title="' + (params.ExecutorIdentityFullName || params.ExecutorIdentityId) + '">\
	                            					<span>' + (params.ExecutorIdentityFullName || params.ExecutorIdentityId) + '</span>\
	                            				</div>\
	                            				<div class="transition-change-info-date">' + parseDate(params.TransitionTime) + '</div>\
	                            			</div>\
	                            		</div>\
	                            	</div>\
	                            	<div class="state-container">\
	                            		<div class="state">\
                                            ' + getStateLabel(params.ToStateName) + '\
	                        			</div>\
	                        		</div>\
	                        	</div>\
	                        </div>\
	                        <div class="state-transition-graph-unused-host"></div>\
                        </div>';

    let result = block + '<form class="panel panel-default" style="padding: 10px">';
    params.Parameters = params.Parameters || [];
    if (params.Parameters.length > 0) {
        for (let i = 0; i < params.Parameters.length; i++) {
            result += '<div class="form-group"><label>';
            result += params.Parameters[i].LocalizedName + '</label>';
            let value = params.Parameters[i].Value;
            if (params.Parameters[i].TypeName === 'DateTime'){
                value = parseDate(new Date(value));
            }
            result += '<span class="form-control">' + (value || '') + '</span>';
            result += '</div>';
        }
    } else {
        result += '<div class="form-group"><label>Параметры отсутствуют</label></div>';
    }
    return result + '</form>';
}

class TaskHistoryController {
    constructor() {

    }

    init(selector, taskId) {
        let form = $(selector);

        form.append(template);

        let id = taskId || form.data('task-id') || utils.parseUrlParams(window.document.location.href).id;

        $table = form.find('#taskHistoryTable');
        //$table.on('processing.dt', function () { bindTableActions(); });
        historyTable = $table.DataTable({
            ajax: {
                url: utils.urlContent('/api/tasks/' + id + '/history'),
                dataSrc: '',
                complete: function () {
                    //utils.ui.loader('#table-tasks', false);
                },
                beforeSend: function () {
                    //utils.ui.loader('#table-tasks');
                }
            },
            language: {
                "processing": "Loading ...",
                "search": "Поиск:",
                "lengthMenu": "Показать _MENU_ записей",
                "info": "Записи с _START_ до _END_ из _TOTAL_ записей",
                "infoEmpty": "Записи с 0 до 0 из 0 записей",
                "infoFiltered": "(отфильтровано из _MAX_ записей)",
                "infoPostFix": "",
                "loadingRecords": "Загрузка записей...",
                "zeroRecords": "Записи отсутствуют.",
                "emptyTable": "В таблице отсутствуют данные",
                "paginate": {
                    "first": "Первая",
                    "previous": "Предыдущая",
                    "next": "Следующая",
                    "last": "Последняя"
                },
                "aria": {
                    "sortAscending": ": активировать для сортировки столбца по возрастанию",
                    "sortDescending": ": активировать для сортировки столбца по убыванию"
                }
            },
            select: true,
            init: function () { },
            columns: [{
                title: 'Дата действия',
                name: 'TransitionTime',
                data: 'TransitionTime',
                render: parseDate
            }, {
                title: 'Исполнитель',
                name: 'ActorIdentityId',
                data: 'ActorIdentityId'
            }, {
                title: 'Прошлое состояние',
                name: 'FromStateName',
                data: 'FromStateName',
                render: function (value) {
                    return getStateLabel(value);
                }
            }, {
                title: 'Новое состояние',
                name: 'ToStateName',
                data: 'ToStateName',
                render: function (value) {
                    return getStateLabel(value);
                }
            }, {
                title: 'Параметры',
                name: 'Id',
                data: 'Id',
                render: function () {
                    return '<div style="font-size:18px; text-align:center;" title="показать параметры действия">\
                                    <span class="glyphicon glyphicon-info-sign"></span>\
                                </div>';
                }
            }],
            order: [[0, 'desc']]
        });

        $table.on('click', 'tr', function () {
            let data = historyTable.row(this).data();
            showParametersWindow(data.ProcessId, data.Id);
        });

    };
}

export default TaskHistoryController;
