define([], function () {
    "use strict";

    var exports = {};
    $(function () {

        $("#btnLoadFileWithAgreements").click(function (e) {
            e.preventDefault();
            $("#file").click();
        });

        $("#file").onclick = function () {
            this.value = null;
        };

        $("#file").change(function () {
            $("#formCreateTask").submit();
        });
    });
    return exports;
});