'use strict';
$(function () {
    $(document).ready(function () {
        $(".datetimepicker").datetimepicker({
            format: 'd.m.Y',
            lang: 'uk',
            timepicker: false,
            dayOfWeekStart: 1
        });

        $.validator.addMethod('date',
            function (value, element) {
                if (this.optional(element)) {
                    return true;
                }

                var ok = true;
                try {
                    $.datepicker.parseDate('dd.mm.yy', value);
                }
                catch (err) {
                    ok = false;
                }
                return ok;
            });

        $.validator.addMethod("number",
            function (value, element) {
                return this.optional(element) || /^\d+(,\d+)*$/.test(value);
            });


        $("#createDS-form")
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        $.validator.unobtrusive.parse("#createDS-form");



        $(document).ready(function () {
            $('#active-table').DataTable({

                paging: false,
                orderable: false,
                searching: false,
                //  className: 'dt-body-center',
                info: false,
                columnDefs: [
                    {

                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0
                    }],
                select: {
                    style: 'milti'
                    // selector: 'td:first-child'
                },
                order: [[1, 'asc']]
            });
        });


        $(document).ready(function () {
            $('#collateral-table').DataTable({

                paging: false,
                orderable: false,
                searching: false,
                //  className: 'dt-body-center',
                info: false,
                columnDefs: [
                    {

                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0
                    }],
                select: {
                    style: 'milti'
                    // selector: 'td:first-child'
                },
                order: [[1, 'asc']]
            });
        });



        $("#create-DS").click(function (e) {
            var form = $("#createDS-form");
            var createDissableButton = document.querySelector("#create-DS");
            var redirectUrl = $("#RedirectTo").val();
            if (!form.valid())
                return;

            createDissableButton.setAttribute("disabled", "disabled");
            var data = form.serialize();

            $.ajax({
                url: "/api/debtsettlements/",
                method: 'POST',
                data: data,
                success: function (result) {
                    window.location.href = redirectUrl;
                    successHandler(result);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    errorHandler(jqXHR, textStatus, errorThrown);
                    createDissableButton.removeAttribute("disabled", "disabled");
                }
            });
        });
    });
});
//# sourceURL=ds.js