define([], function () {
    'use strict';

    var exports = {};
    Object.defineProperty(exports, "__esModule", {
        value: true
    });

    var htmlHelpers = {
        generateLabelClassByStatus: function generateLabelClassByStatus(status) {
            var prefix = 'label label-';
            var className;
            switch (status) {
                case 'Open':
                    {
                        className = 'primary';
                        break;
                    }
                case 'InProgress':
                case 'In Progress':
                    {
                        className = 'success';
                        break;
                    }
                case 'Overdue':
                    {
                        className = 'danger';
                        break;
                    }
                case 'OnApprove':
                case 'On approve':
                    {
                        className = 'warning';
                        break;
                    }
                case 'Closed':
                default:
                    {
                        className = 'default';
                    }
            }
            return prefix + className;
        } };

    exports.default = htmlHelpers;
    return exports.default;
});