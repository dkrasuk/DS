'use strict';
$(function() {

    $(document).ready(function() {

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


        $("#ork-form")
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        $.validator.unobtrusive.parse("#ork-form");

        $('#active-table').DataTable({

            paging: false,
            orderable: false,
            searching: false,
            //  className: 'dt-body-center',
            info: false,
            columnDefs: [
                {

                    orderable: false,

                    targets: 0
                }],
            select: {
                style: 'milti'
                // selector: 'td:first-child'
            },
            order: [[1, 'asc']]
        });

        $('#collateral-table').DataTable({

            paging: false,
            orderable: false,
            searching: false,
            //  className: 'dt-body-center',
            info: false,
            columnDefs: [
                {
                    orderable: false,
                    //className: 'select-checkbox',
                    targets: 0
                }],
            select: {
                style: 'milti'
                // selector: 'td:first-child'
            },
            order: [[1, 'asc']]
        });

        $("#create-ORK").click(function(e) {
            var form = $("#ork-form");
            var createDissableButton = document.querySelector("#create-ORK");

            if (!form.valid()) {
                Notify('Не заполнены обязательные поля :("', '', 'error');
                return;
            } 
          //  createDissableButton.setAttribute("disabled", "disabled");



        });


        //Связанные списки MacroSegment => Portfolio
        $("#macroSegment-ork").change(function () {
            var macroSegment = $(this).find('option:selected').val();

            $.ajax({

                url: "/DebtSettlement/GetPortfolioType/",
                method: 'GET',

                data: { "macroSegment": macroSegment },
                success: function (data) {
                    var portfolio = JSON.parse(data);

                    $('#portfolio-ork').attr('value', portfolio['Segment']);

                }
            });
        });

        function Notify(header, message, type) {
            app.utils.ui.notify(header, message, type);
        }



    });
});