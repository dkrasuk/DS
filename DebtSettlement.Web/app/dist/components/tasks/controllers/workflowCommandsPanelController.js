define(['app/utils', 'app/components/tasks/services/tasksService'], function (_utils, _tasksService) {
    'use strict';

    var exports = {};
    Object.defineProperty(exports, "__esModule", {
        value: true
    });

    var _utils2 = _interopRequireDefault(_utils);

    var _tasksService2 = _interopRequireDefault(_tasksService);

    function _interopRequireDefault(obj) {
        return obj && obj.__esModule ? obj : {
            default: obj
        };
    }

    function _classCallCheck(instance, Constructor) {
        if (!(instance instanceof Constructor)) {
            throw new TypeError("Cannot call a class as a function");
        }
    }

    var _createClass = function () {
        function defineProperties(target, props) {
            for (var i = 0; i < props.length; i++) {
                var descriptor = props[i];
                descriptor.enumerable = descriptor.enumerable || false;
                descriptor.configurable = true;
                if ("value" in descriptor) descriptor.writable = true;
                Object.defineProperty(target, descriptor.key, descriptor);
            }
        }

        return function (Constructor, protoProps, staticProps) {
            if (protoProps) defineProperties(Constructor.prototype, protoProps);
            if (staticProps) defineProperties(Constructor, staticProps);
            return Constructor;
        };
    }();

    var commands = void 0;
    var template = void 0;
    var $elem = void 0;
    var events = {};
    var currentCommand = null;

    function showDefaultError() {
        _utils2.default.ui.error({
            text: 'Что-то пошло не так. Случилась ошибка при отправке запроса'
        });
    }

    function createCommandButton(command) {
        var button = $('<button class="btn btn-md btn-primary" name="' + command.CommandName + '">' + command.LocalizedName + '</button>');
        button.data('wf-command', command);
        button.on('click', function () {
            callEvents('click', events, command);
            if (command.Parameters) {
                displayCommandParametersDialog(command);
            } else {
                _utils2.default.ui.confirm({
                    text: 'Вы уверены что хотите выполнить действие "' + command.LocalizedName + '"',
                    func: function func() {
                        submitCommand(command);
                    }
                });
            }
        });
        return button;
    }

    function displayCommandParametersDialog(command, parrentDialog) {
        if (!parrentDialog) {
            currentCommand = command;
        }
        var id = 'commandParametersWindow' + command.ParameterName;
        var html = $('<form id="' + id + '"></form>');
        var validateRules = {};
        for (var c = 0; c < command.Parameters.length; c++) {
            var currentParameter = command.Parameters[c];
            var commandParemeterControll = createCommandParameterControll(currentParameter, validateRules);
            if (currentParameter.GroupName) {
                var panel = html.find('[data-group="' + currentParameter.GroupName + '"]');
                if (panel.length === 0) {
                    panel = $('<div class="panel panel-default" data-group="' + currentParameter.GroupName + '">\n                            <div class="panel-heading">' + currentParameter.GroupName + '</div>\n                            <div class="panel-body"></div>\n                        </div>');
                    html.append(panel);
                }
                commandParemeterControll.appendTo(panel.find('.panel-body'));
            } else {
                commandParemeterControll.appendTo(html);
            }
        }

        var dialog = _utils2.default.ui.confirm({
            title: command.LocalizedName,
            content: html,
            width: 600,
            height: 350,
            func: function func() {
                if (validateCommandParameters(html, command)) {
                    if (command.SubInstanceCommands && command.SubInstanceCommands.length > 0) {
                        displayCommandParametersDialog(command.SubInstanceCommands[0], dialog);
                        return false;
                    } else {
                        dialog.loader(true, { modal: true });

                        submitCommand(currentCommand).then(function () {
                            if (parrentDialog) {
                                parrentDialog.modal('toggle');
                            }
                            dialog.loader(false);
                            dialog.modal('hide');
                            _utils2.default.ui.notify('Успешно!', 'Данные сохранены <span class="glyphicon glyphicon-thumbs-up"></span>', 'success');
                            callEvents('submit', events, command);
                        }, function () {
                            dialog.loader(false);
                            showDefaultError($elem);
                        });
                        return false;
                    }
                } else {
                    _utils2.default.ui.notify('Ошибка!', 'Не все параметры заполнены корректно.', 'error');
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
        var linkedParameters = form.find('[data-linked-from][data-linked-from-isrequired="true"]');
        if (linkedParameters.length > 0) {
            linkedParameters.each(function (index, domElement) {
                var $elem = $(domElement);
                var linkedElem = form.find('[name="' + $elem.data('linked-from') + '"]');
                if (linkedElem.length > 0) {
                    var value = linkedElem.eq(0).val();
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

        var result = $('<div class="form-group"></div>');
        var label = $('<label>' + parameter.LocalizedName + '</label>').appendTo(result);
        var input = $('<input  class="form-control" name="' + parameter.ParameterName + '"/>');
        input.css('width', '100%');

        var rule = validateRules[parameter.ParameterName] = {};
        if (parameter.Type === 'DateTime') {
            input.wrap('<div class="input-group" style="width:220px"></div>');
            input.after('<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>');
            input.css('width', '200px');
            var datePickerParams = {
                dateFormat: 'dd/mm/yy',
                lang: 'uk',
                timepicker: false,
                dayOfWeekStart: 1,
                onSelect: function onSelect(dt, element) {
                    var m = element.selectedMonth + 1;
                    var d = element.selectedDay;
                    parameter.Value = element.selectedYear + '-' + (m < 10 ? '0' + m : m) + '-' + (d < 10 ? '0' + d : d) + 'T00:00:00';
                }
            };

            if (parameter.Settings.MinValue) {
                if (parameter.Settings.MinValue.toLowerCase() === '::sysdate') {
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

            input.datepicker(datePickerParams).mask('df/MF/yyyy', {
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
                rule.minDate = new Date(datePickerParams.minDate.getFullYear(), datePickerParams.minDate.getMonth(), datePickerParams.minDate.getDate());
            }
            if (datePickerParams.maxDate) {
                rule.maxDate = new Date(datePickerParams.maxDate.getFullYear(), datePickerParams.maxDate.getMonth(), datePickerParams.maxDate.getDate());
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
                    var source = parameter.Settings.DataSource;
                    var settings = {};

                    if (source.startsWith('dictionary')) {
                        settings.ajax = {
                            url: _utils2.default.urlContent('/api/dictionary/' + source.split(':')[1].trim()),
                            cache: true,
                            dataType: 'json',
                            delay: 250,
                            data: function data(params) {
                                var data = {
                                    term: params.term, // search term
                                    $skip: (params.page || 1) * 100 - 100,
                                    $top: 100,
                                    $orderby: 'id'
                                };
                                if (parameter.LinkedParameterName) {
                                    var form = input.parentsUntil('[id^="commandParametersWindow"]').parent();
                                    var linkedElem = form.find('[name="' + parameter.LinkedParameterName + '"]');
                                    if (linkedElem.length > 0) {
                                        var value = linkedElem.eq(0).val();
                                        if (value) {
                                            var type = linkedElem.eq(0).data('wf-command-parameter').Type;
                                            data['$filter'] = (parameter.LinkedParameterAs || parameter.LinkedParameterName) + ' eq ';
                                            if (type === 'Decimal' || type.startsWith('Int')) {
                                                data['$filter'] += value;
                                            } else {
                                                data['$filter'] += '\'' + value + '\'';
                                            }
                                        }
                                    }
                                }
                                return data;
                            },
                            processResults: function processResults(data, params) {
                                params.page = params.page || 1;
                                return {
                                    results: data,
                                    pagination: {
                                        more: params.page * 100 < data.length * params.page + 1
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
                            url: _utils2.default.urlContent(source.replace('~/', ''))
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
        if (parameter.LinkedParameterName) {
            input.attr('data-linked-from', parameter.LinkedParameterName);
            if (parameter.LinkedParameterIsRequired) {
                input.attr('data-linked-from-isrequired', parameter.LinkedParameterIsRequired);
            }
            if (parameter.LinkedParameterAs) {
                input.attr('data-linked-as', parameter.LinkedParameterAs);
            }
        }

        if (parameter.IsRequired) {
            label.addClass('required');
            input.addClass('required').attr('required', 'required');
            input.on('change', function () {
                var $this = $(this);
                if (!$this.val()) {
                    $this.addClass('error');
                } else {
                    $this.removeClass('error');
                }
            });
        }
        input.data('wf-command-parameter', parameter);
        input.on('change', function () {
            var $this = $(this);
            var value = $this.val();
            parameter.Value = value;

            var form = $this.parentsUntil('[id^="commandParametersWindow"]').parent();
            var linkedParameters = form.find('[data-linked-from="' + parameter.ParameterName + '"][data-linked-from-isrequired="true"]');
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
        var result = true;
        if (!html.valid()) {
            result = false;
        }

        return result;
    }

    function MapSubInstanceCommandsParameters(command) {
        command.Parameters = command.Parameters || [];
        if (command.SubInstanceCommands && command.SubInstanceCommands.length > 0) {
            for (var i = 0; i < command.SubInstanceCommands.length; i++) {
                var subCommand = command.SubInstanceCommands[i];
                command.Parameters.push({ ParameterName: subCommand.ActionName, Value: subCommand });
                MapSubInstanceCommandsParameters(subCommand);
            }
        }
    }

    function submitCommand(command) {
        var parameters = {};
        MapSubInstanceCommandsParameters(command);
        command.Parameters.map(function (item) {
            parameters[item.ParameterName] = item.Value;
        });
        return _tasksService2.default.executeCommand(command.ProcessId, command.CommandName, parameters);
    }

    function callEvents(name, eventsArray, context) {
        if (eventsArray[name]) {
            for (var e = 0; e < eventsArray[name].length; e++) {
                eventsArray[name][e].call(context);
            }
        }
    }

    var WorkflowCommandsPanelController = function () {
        function WorkflowCommandsPanelController(templateConstructor, commandsConstructor) {
            _classCallCheck(this, WorkflowCommandsPanelController);

            template = templateConstructor;
            $elem = $(template);
            commands = commandsConstructor;
            events = {};
        }

        _createClass(WorkflowCommandsPanelController, [{
            key: 'init',
            value: function init() {
                if (commands) {
                    $elem.html('');
                    for (var i = 0; i < commands.length; i++) {
                        var button = createCommandButton(commands[i]);
                        button.appendTo($elem);
                    }
                }
                callEvents('init', events);
            }
        }, {
            key: 'on',
            value: function on(name, func) {
                events[name] = events[name] || [];
                events[name].push(func);
            }
        }]);

        return WorkflowCommandsPanelController;
    }();

    exports.default = WorkflowCommandsPanelController;
    return exports.default;
});