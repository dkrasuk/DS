var navigation = (function () {
     let init = function(callback) {
        $('#mainNavigationMenu a.tasks-list-filter').click(function() {
            callback.call(this);
        });
     }
    return {init}
}());

export default navigation;