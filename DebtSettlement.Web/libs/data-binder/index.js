;
// object.watch
if (!Object.prototype.watch) {
    Object.defineProperty(Object.prototype, "watch", {
        enumerable: false
        , configurable: true
        , writable: false
        , value: function (prop, handler) {
            var handlers = this._handlers || (this._handlers = {});
            var propHendlers = handlers[prop] || (handlers[prop] = []);
            propHendlers.push(handler);

            var oldval = this[prop]
                , newval = oldval
                , getter = function () {
                    return newval;
                }
                , setter = function (val) {
                    oldval = newval;
                    newval = val;
                    for (var i = 0; i < handlers[prop].length; i++) {
                        handlers[prop][i].call(this, prop, oldval, val);
                    }
                }
                ;

            if (delete this[prop]) { // can't watch constants
                Object.defineProperty(this, prop, {
                    get: getter
                    , set: setter
                    , enumerable: true
                    , configurable: true
                });
            }
        }
    });
}
// object.unwatch
if (!Object.prototype.unwatch) {
    Object.defineProperty(Object.prototype, "unwatch", {
        enumerable: false
        , configurable: true
        , writable: false
        , value: function (prop) {
            var val = this[prop];
            delete this[prop]; // remove accessors
            this[prop] = val;
        }
    });
}


var dataBinder = (function ($) {
    'use strict';
    function bind(element, data) {
        if (typeof element === 'string' || !(element instanceof jQuery)) {
            element = $(element);
        }
        var binders = element.find('[ng-bind],[data-ng-bind]');
        for (var i = 0; i < binders.length; i++) {
            var item = binders.eq(i);
            item.html(getDataFromObject(getBindValue(item), data));
            addWach(item, data);
        }

        var modelElements = element.find('[ng-model],[data-ng-model]');
        for (var i = 0; i < modelElements.length; i++) {
            var item = modelElements.eq(i);
            item.val(getDataFromObject(getModelValue(item), data));
            addWach(item, data);
        }

        var ngClick = element.find('[ng-click],[data-ng-click]');
        for (var i = 0; i < ngClick.length; i++){
            var item = ngClick.eq(i);
            item.on('click', getDataFromObject(getClickValue(item), data));
            //addWach(item, data);
        }
        var ngShow = element.find('[ng-show],[data-ng-show]');
        for (var i = 0; i < ngClick.length; i++){
            
            var item = ngShow.eq(i);
            if (evalInContext.call(data, (item.attr('ng-show') || item.attr('data-ng-show')))) {
                item.show();
            } else {
                item.hide();
            }
            //item.on('click', getDataFromObject(getClickValue(item), data));
            //addWach(item, data);
        }

        var select2 = element.find('[select2],[data-select2]')
        for (var i = 0; i < select2.length; i++){
            var item = select2.eq(i);

            (function () {
                item.select2({
                    data: getDataFromObject((item.attr('select2') || item.attr('data-select2')), data)
                });
            }).call(data)
        }


        return element;
    }

    function evalInContext(str) {
        return eval(str);
    } 


    function getDataFromObject(name, object) {
        if (name.indexOf('.') !== -1) {
            return getDataFromObject(name.substr(name.indexOf('.') + 1), object[name.substr(0, name.indexOf('.'))])
        }
        return object[name];
    }

    function setDataToObject(name, value, object) {
        object[name] = value;
    }
    function getBindValue(element) {
        return element.attr('ng-bind') || element.attr('data-ng-bind');
    }
    function getModelValue(element) {
        return element.attr('ng-model') || element.attr('data-ng-model');
    }
    function getClickValue(element) {
        return (element.attr('ng-click') || element.attr('data-ng-click')).split('(')[0];
    }

    function addWach(element, object) {
        if (element[0].nodeName === 'INPUT'
            || element[0].nodeName === 'SELECT'
            || element[0].nodeName === 'TEXTAREA') {

            var prop = getModelValue(element);
            element.on('change', function (e) {
                var elem = $(this);
                setDataToObject(prop, elem.val(), object)
            });

            object.watch(prop, function (name, oldVal, newVal) {
                if (oldVal !== newVal) {
                    
                    element.val(newVal);
                }
            });
        } else {
            object.watch(prop, function (name, oldVal, newVal) {
                if (oldVal !== newVal) {
                    element.html(newVal);
                }
            });
        }
    }

    return {
        bind: bind

    }
}(jQuery));