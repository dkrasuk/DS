import navigation from 'app/components/shared/navigation';
import listController from 'app/components/tasks/controllers/listController';
import utils from 'app/utils';
import ViewTaskController from 'app/components/tasks/controllers/viewTaskController';

utils.loadCss(utils.urlContent('/app/dist/components/tasks/css/tasks.css'));


if (window.location.href.toLowerCase().indexOf('/task/item') !== -1) {
    let controller = new ViewTaskController();
    let data = utils.parseUrlParams(window.location.href).id;
    controller.init($('[ng-controller=ViewTaskController]').parent(), data);
} else {
    navigation.init(function() {
        listController.loadTasks({ hash: (this.hash || '').replace('#', '') })
    });
}
