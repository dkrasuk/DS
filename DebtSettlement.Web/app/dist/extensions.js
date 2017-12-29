define([], function () {
    'use strict';

    var exports = {};
    Object.defineProperty(exports, "__esModule", {
        value: true
    });
    exports.default = extensions;
    ;'use strict';

    if (!String.prototype.startsWith) {
        String.prototype.startsWith = function (searchString, position) {
            return this.substr(position || 0, searchString.length) === searchString;
        };
    }

    if (window.$ && window.$.validator) {

        $.validator.addMethod("pattern", function (value, element, regexp) {
            if (!value) {
                return true;
            }
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        }, 'Значение имеет неверный формат');

        $.validator.addMethod("dateFormat", function (value, element, format) {
            if (!value) {
                return true;
            }
            return value.match(/^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.]((?:19|20)\d\d)$/);
        }, 'Пожалуйста, введите корректную дату.');
        $.validator.addMethod("minDate", function (value, element, minDate) {
            if (!value) {
                return true;
            }

            var dateArray = value.split('/');
            var d = dateArray[0];
            var m = dateArray[1];
            var y = dateArray[2];

            var dateStr = y + '-' + m + '-' + d + 'T00:00:00';
            var inputDate = new Date(dateStr);

            return inputDate >= minDate;
        }, 'Дата меньше допустимой');
        $.validator.addMethod("maxDate", function (value, element, maxDate) {
            if (!value) {
                return true;
            }

            var dateArray = value.split('/');
            var d = dateArray[0];
            var m = dateArray[1];
            var y = dateArray[2];

            var dateStr = y + '-' + m + '-' + d + 'T00:00:00';
            var inputDate = new Date(dateStr);

            return inputDate <= maxDate;
        }, 'Дата больше допустимой');
    }
    if (window.$ && $.mask) {
        $.mask.definitions['~'] = '[+-]';
    }

    if (window.$ && $.ui && $.ui.dialog && $.ui.dialog.prototype._allowInteraction) {
        var ui_dialog_interaction = $.ui.dialog.prototype._allowInteraction;
        $.ui.dialog.prototype._allowInteraction = function (e) {
            if ($(e.target).closest('.select2-dropdown').length) return true;
            return ui_dialog_interaction.apply(this, arguments);
        };
    }

    if (window.$ && $.bindings) {
        var thisEval = function thisEval(code, object) {
            return function (codeText) {
                return eval(codeText);
            }.call(object, code);
        };

        $.bindings.format = function (path, value, format, model, name) {
            var $this = $(this);
            var $elem = $this.find('[data-model="' + path + '"]');

            if (format && $elem.length === 0 && $this.data('bind-format') !== 'enabled') {
                $this.data('bind-format', 'enabled');
                if (format.indexOf('date:') === 0) {
                    if (value) {
                        return $.format.date(value, format.replace('date:', '').trim());
                    } else {
                        return null;
                    }
                }
                if (format.indexOf('func:') === 0) {
                    var func = thisEval('this.' + format.replace('func:', '').trim(), model);
                    return func.call(model, value, $this);
                }
            }
            return value;
        };

        $.bindings.custom = function (path, value, custom, model, name) {
            var $this = $(this);
            var $elem = $this.find('[data-model="' + path + '"]');
            if ($elem.length === 0 && $this.data('bind-custom') !== 'enabled') {
                $this.data('bind-custom', 'enabled');
                if (custom === 'show' && value) {
                    $this.removeClass('hidden');
                    return;
                }
                if (custom === 'hide' && value) {
                    $this.addClass('hidden');
                    return;
                }
                if (custom.indexOf('click:') === 0) {
                    var func = thisEval('this.' + custom.replace('click:', '').trim(), model);
                    $this.on('click', function () {
                        func.call(model, value, $this);
                    });
                    return;
                }
            }
            return;
        };

        //noinspection JSAnnotator
    }

    function extensions() {};
    return exports.default;
});