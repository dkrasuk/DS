define([], function () {
    'use strict';

    var exports = {};
    Object.defineProperty(exports, "__esModule", {
        value: true
    });
    var navigation = function () {
        var init = function init(callback) {
            $('#mainNavigationMenu a.tasks-list-filter').click(function () {
                callback.call(this);
            });
        };
        return { init: init };
    }();

    exports.default = navigation;
    return exports.default;
});