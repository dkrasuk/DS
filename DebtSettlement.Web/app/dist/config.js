define([], function () {
    'use strict';

    var exports = {};
    requirejs.config({
        baseUrl: document.getElementById('baseTag').href + '/libs',
        paths: {
            app: '../app/dist'
        }
    });
    return exports;
});