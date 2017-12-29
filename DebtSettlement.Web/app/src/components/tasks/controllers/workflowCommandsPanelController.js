import utils from 'app/utils';
import tasksService from 'app/components/tasks/services/tasksService';

let commands;
let template;
let $elem;
let events = {};
let currentCommand = null;

function showDefaultError() {
    utils.ui.error({
        text: 'Что-то пошло не так. Случилась ошибка при отправке запроса'
    });
}

function createCommandButton(command) {
    let button = $('<button class="btn btn-md btn-primary" name="' + command.CommandName + '">' 
                        + command.LocalizedName 
                + '</button>');
    button.data('wf-command', command);
    button.on('click', function () {
        callEvents('click', events, command);
        if (command.Parameters) {
            displayCommandParametersDialog(command);
        } else {
            utils.ui.confirm({
                text: 'Вы уверены что хотите выполнить действие "' + command.LocalizedName + '"',
                func: function () {
                    submitCommand(command);
                }
            });
        }
    });
    return button;
}

function displayCommandParametersDialog(command, parrentDialog) {
    if (!parrentDialog){
        currentCommand = command;
    }
    let id = 'commandParametersWindow' + command.ParameterName;
    let html = $('<form id="' + id + '"></form>');
    let validateRules = {};
    for (let c = 0; c < command.Parameters.length; c++) {
        let currentParameter = command.Parameters[c];
        let commandParemeterControll = createCommandParameterControll(currentParameter, validateRules);
        if (currentParameter.GroupName){
            let panel = html.find('[data-group="' + currentParameter.GroupName + '"]');
            if (panel.length === 0){
                panel = $(`<div class="panel panel-default" data-group="`+ currentParameter.GroupName +`">
                            <div class="panel-heading">` + currentParameter.GroupName + `</div>
                            <div class="panel-body"></div>
                        </div>`);
                html.append(panel);
            }
            commandParemeterControll.appendTo(panel.find('.panel-body'));
        } else {
            commandParemeterControll.appendTo(html);            
        }        
    }
    
    let dialog = utils.ui.confirm({
        title: command.LocalizedName,
        content: html,
        width: 600,
        height: 350,
        func: function () {
            if (validateCommandParameters(html, command)) {
                if (command.SubInstanceCommands && command.SubInstanceCommands.length > 0){
                    displayCommandParametersDialog(command.SubInstanceCommands[0], dialog);
                    return false;
                } else {
                    dialog.loader(true, {modal: true});

                    submitCommand(currentCommand).then(
                        function () {
                            if (parrentDialog){
                                parrentDialog.modal('toggle');
                            }
                            dialog.loader(false);
                            dialog.modal('hide');
                            utils.ui.notify('Успешно!', 'Данные сохранены <span class="glyphicon glyphicon-thumbs-up"></span>', 'success');
                            callEvents('submit', events, command);
                        },
                        function () {
                            dialog.loader(false);
                            showDefaultError($elem);
                        }
                    );
                    return false;                  
                }
            } else {
                utils.ui.notify('Ошибка!', 'Не все параметры заполнены корректно.', 'error');
                return false;
            }
        }
    });
    
    bindLinkedParameters(html);
    
    if ($.fn.validate) {
        html.validate({
            rules: validateRules
        });
    }
}

function bindLinkedParameters(form) {
    let linkedParameters = form
        .find('[data-linked-from][data-linked-from-isrequired="true"]');
    if (linkedParameters.length > 0) {
        linkedParameters.each(function (index, domElement) {
            let $elem = $(domElement);
            let linkedElem = form.find('[name="' + $elem.data('linked-from') + '"]');
            if (linkedElem.length > 0){
                let value = linkedElem.eq(0).val();
                if (value) {
                    linkedParameters.removeAttr('disabled');
                } else {
                    linkedParameters.attr('disabled', 'disabled');
                }
            }
        });

    }
}

function createCommandParameterControll(parameter, validateRules) {
    parameter.Settings = parameter.Settings || {};

    let result = $('<div class="form-group"></div>');
    let label = $('<label>' + parameter.LocalizedName + '</label>').appendTo(result);
    let input = $('<input  class="form-control" name="' + parameter.ParameterName + '"/>');
    input.css('width', '100%');

    let rule = validateRules[parameter.ParameterName] = {};
    if (parameter.Type === 'DateTime') {
        input.wrap('<div class="input-group" style="width:220px"></div>');
        input.after('<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>');
        input.css('width', '200px');
        let datePickerParams = {
            dateFormat: 'dd/mm/yy',
            lang: 'uk',
            timepicker: false,
            dayOfWeekStart: 1,
            onSelect: function (dt, element) {
                let m = element.selectedMonth + 1;
                let d = element.selectedDay;
                parameter.Value = element.selectedYear
                    + '-' + (m < 10 ? '0' + m : m)
                    + '-' + (d < 10 ? '0' + d : d) + 'T00:00:00';
            }
        };

        if (parameter.Settings.MinValue) {
            if (parameter.Settings.MinValue.toLowerCase() === '::sysdate' ){
                datePickerParams.minDate = new Date();                
            } else {
                datePickerParams.minDate = new Date(parameter.Settings.MinValue);
            }
        }
        if (parameter.Settings.MaxValue) {
            if (parameter.Settings.MaxValue.toLowerCase() === '::sysdate') {
                datePickerParams.maxDate = new Date();
            } else {
                datePickerParams.maxDate = new Date(parameter.Settings.MaxValue);
            }
        }

        input.datepicker(datePickerParams)
        .mask('df/MF/yyyy', {
            placeholder: '__/__/____',
            'translation': {
                d: { pattern: /[0-3]/ },
                f: { pattern: /[0-9]/ },
                M: { pattern: /[0-1]/ },
                F: { pattern: /[0-9]/ },
                y: { pattern: /[0-9]/ }
            }
        });

        rule.dateFormat = true;
        if (datePickerParams.minDate) {
            rule.minDate = new Date(
                datePickerParams.minDate.getFullYear(),
                datePickerParams.minDate.getMonth(),
                datePickerParams.minDate.getDate());
        }
        if (datePickerParams.maxDate) {
            rule.maxDate = new Date(
                datePickerParams.maxDate.getFullYear(),
                datePickerParams.maxDate.getMonth(),
                datePickerParams.maxDate.getDate());
        }

        result.append(input);
    } else {
        if (parameter.Type === 'Char') {
            input.mask('с', { 'translation': { с: { pattern: /\S/ } } });
        }
        if (parameter.Type === 'Decimal' || parameter.Type.startsWith('Int')) {
            input.mask('#9', { reverse: true });
        }

        if (parameter.Settings) {
            if (parameter.Settings.DataSource) {
                input = $('<select  class="form-control" name="' + parameter.ParameterName + '"/>');
                input.css('width', '100%');
                result.append(input);
                let source = parameter.Settings.DataSource;
                let settings = {};
                
                if (source.startsWith('dictionary')) {
                    settings.ajax = {
                        url: utils.urlContent('/api/dictionary/' + source.split(':')[1].trim()),
                        cache: true,
                        dataType: 'json',
                        delay: 250,
                        data: function (params) {
                            let data = {
                                term: params.term, // search term
                                $skip: ((params.page || 1) * 100) - 100,
                                $top: 100,
                                $orderby: 'id'
                            };
                            if (parameter.LinkedParameterName) {
                                let form = input.parentsUntil('[id^="commandParametersWindow"]').parent();
                                let linkedElem = form.find('[name="' + parameter.LinkedParameterName + '"]');
                                if (linkedElem.length > 0) {
                                    let value = linkedElem.eq(0).val();
                                    if (value){
                                        let type = linkedElem.eq(0).data('wf-command-parameter').Type;
                                        data['$filter'] = (parameter.LinkedParameterAs || parameter.LinkedParameterName) 
                                            + ' eq ';
                                        if (type === 'Decimal' || type.startsWith('Int')){
                                            data['$filter'] += value;
                                        } else {
                                            data['$filter'] += '\'' + value +'\'';
                                        }
                                    }
                                }
                            }
                            return data;
                        },
                        processResults: function (data, params) {
                            params.page = params.page || 1;
                            return {
                                results: data,
                                pagination: {
                                    more: (params.page * 100) < (data.length * params.page) + 1
                                }
                            };
                        }
                    };
                } else if (source.startsWith('[') && source.endsWith(']')) {
                    settings.data = JSON.parse(source);
                } else if (source.toLowerCase().startsWith('http://') || source.toLowerCase().startsWith('https://')) {
                    settings.ajax = {
                        url: source
                    };
                } else if (source.startsWith('~/')) {
                    settings.ajax = {
                        url: utils.urlContent(source.replace('~/', ''))
                    };
                }

                input.select2(settings);
            } else {
                if (parameter.Settings.Pattern) {
                    rule.pattern = parameter.Settings.Pattern;
                }

                if (parameter.Settings.MinLength) {
                    rule.minlength = parameter.Settings.MinLength;
                }
                if (parameter.Settings.MaxLength) {
                    rule.maxlength = parameter.Settings.MaxLength;
                }

                if (parameter.Settings.MimValue) {
                    rule.min = parameter.Settings.MimValue;
                }
                if (parameter.Settings.MaxValue) {
                    rule.max = parameter.Settings.MaxValue;
                }
            }
        }
      
        result.append(input);
        input.val(parameter.DefaultValue);
        parameter.Value = parameter.DefaultValue;
    }
    if (parameter.LinkedParameterName){
        input.attr('data-linked-from', parameter.LinkedParameterName);
        if (parameter.LinkedParameterIsRequired){
            input.attr('data-linked-from-isrequired', parameter.LinkedParameterIsRequired);
        }
        if (parameter.LinkedParameterAs){
            input.attr('data-linked-as', parameter.LinkedParameterAs);            
        }
    }

    if (parameter.IsRequired) {
        label.addClass('required');
        input.addClass('required').attr('required', 'required');
        input.on('change', function () {
            let $this = $(this);
            if (!$this.val()) {
                $this.addClass('error');
            } else {
                $this.removeClass('error');
            }
        });
    }
    input.data('wf-command-parameter', parameter);
    input.on('change', function () {
        let $this = $(this);
        let value = $this.val();
        parameter.Value = value;
        
        let form = $this.parentsUntil('[id^="commandParametersWindow"]').parent();
        let linkedParameters = form
            .find('[data-linked-from="'+ parameter.ParameterName +'"][data-linked-from-isrequired="true"]');
        if (linkedParameters.length > 0) {
            if (value) {
                linkedParameters.removeAttr('disabled');
            } else {
                linkedParameters.attr('disabled', 'disabled');
                linkedParameters.val('');
                linkedParameters.trigger('change');
            }
        } 
    });

    return result;
}

function validateCommandParameters(html) {
    let result = true;
    if (!html.valid()) {
        result = false;
    }

    return result;
}

function MapSubInstanceCommandsParameters(command) {
    command.Parameters = command.Parameters || [];
    if (command.SubInstanceCommands && command.SubInstanceCommands.length > 0){
        for (let i = 0; i < command.SubInstanceCommands.length; i++){
            let subCommand = command.SubInstanceCommands[i];
            command.Parameters.push({ParameterName: subCommand.ActionName, Value: subCommand});
            MapSubInstanceCommandsParameters(subCommand);
        }
    }
}

function submitCommand(command) {
    let parameters = {};
    MapSubInstanceCommandsParameters(command);
    command.Parameters.map(function (item) {
        parameters[item.ParameterName] = item.Value;
    });
    return tasksService.executeCommand(command.ProcessId, command.CommandName, parameters);
}

function callEvents(name, eventsArray, context) {
    if (eventsArray[name]) {
        for (let e = 0; e < eventsArray[name].length; e++) {
            eventsArray[name][e].call(context);
        }
    }
}
class WorkflowCommandsPanelController {
    constructor(templateConstructor, commandsConstructor) {
        template = templateConstructor;
        $elem = $(template);
        commands = commandsConstructor;
        events = {};
    }

    init() {
        if (commands) {
            $elem.html('');
            for (let i = 0; i < commands.length; i++) {
                let button = createCommandButton(commands[i]);
                button.appendTo($elem);
            }
        }
        callEvents('init', events);
    }

    on(name, func) {
        events[name] = events[name] || [];
        events[name].push(func);
    }
}

export default WorkflowCommandsPanelController;