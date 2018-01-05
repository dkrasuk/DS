'use strict';
$(function () {
    var collateralIdAttach;
     var additionalProperty;
   
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
      



        $("#create-DS").click(function (e) {
            var form = $("#createDS-form");
            var createDissableButton = document.querySelector("#create-DS");
            var redirectUrl = $("#RedirectTo").val();
            if (!form.valid()) {
                Notify('Не заполнены обязательные поля :("', '', 'error');
                return;
            }               

            createDissableButton.setAttribute("disabled", "disabled");
            var data = form.serialize();
            app.utils.ui.loader($("#loadingBusy"), true, { modal: true });

            $.ajax({
                url: "/api/debtsettlements/",
                method: 'POST',
                data: data,
                success: function (result) {
                    app.utils.ui.loader($("#loadingBusy"), false);
                    window.location.href = redirectUrl;
                    successHandler(result);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    app.utils.ui.loader($("#loadingBusy"), false);
                    errorHandler(jqXHR, textStatus, errorThrown);
                    createDissableButton.removeAttribute("disabled", "disabled");
                }
            });
        });

     //Cheked Collaterals    
  
        $("#collateral-table :input[type=checkbox]").click(function() {
            collateralIdAttach = $('#collateral-table').find('[type="checkbox"]:checked').map(function(){
                return $(this).closest('tr').find('td:nth-child(2)').text();
            }).get();

            $("#CollateralsSelected").attr('value',collateralIdAttach);

           // console.log(collateralIdAttach);
           
        });
        

        $('#collateral-table :checkbox').change(function() {
       
            if (this.checked) {
                Notify('Прикреплен залог типа: ', ($(this).closest('tr').find('td:nth-child(3)').text()), 'success');
            } else {
               // Notify('Откреплен залог типа: ', ($(this).closest('tr').find('td:nth-child(3)').text()), 'error');
            }
        });       

    //Cheked AdditionalProperty   
  
        $('#active-table :input[type=checkbox]').click(function() {
            additionalProperty = $('#active-table').find('[type="checkbox"]:checked').map(function(){
                return $(this).closest('tr').find('td:nth-child(2)').text();
            }).get();

            $("#ActivesSelected").attr('value',additionalProperty);
           // console.log(additionalProperty);                  
        });
        

        $('#active-table :checkbox').change(function() {
       
            if (this.checked) {
                Notify('Прикреплено доп. имущество типа: ', ($(this).closest('tr').find('td:nth-child(3)').text()), 'success');
            } else {
             //   Notify('Откреплено доп. имущество типа: ', ($(this).closest('tr').find('td:nth-child(3)').text()), 'error');
            }
        });

        function Notify(header,message, type) {
            app.utils.ui.notify(header,message, type);
        }

        //Связанные списки MacroSegment => Portfolio 

        $("#macroSegment").change(function () {
            var macroSegment = $(this).find('option:selected').val();

            $.ajax({
               
                url: "/DebtSettlement/GetPortfolioType/",
                method: 'GET',
              
                data: { "macroSegment": macroSegment },
                success: function (data) {
                    var portfolio = JSON.parse(data);

                    $('#portfolio').attr('value', portfolio['Segment']);

                }
            }); 

        });
    });
});
//# sourceURL=ds.js