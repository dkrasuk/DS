
let htmlHelpers = {
        generateLabelClassByStatus: function (status) {
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
                default: {
                    className = 'default';
                }
            }
            return prefix + className;
        }};

export default htmlHelpers;