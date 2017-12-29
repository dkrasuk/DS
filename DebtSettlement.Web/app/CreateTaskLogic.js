$(function () {

    ////// Initialization

    // Remove possibility to change the agreement 
    if ($("#isPartial").val() !== "true") {
        $(".block-agreement-change").show();
    }

    // Shift between multi and single modes
    if ($("#IsMultiTask").is(':checked')) {
        $(".agreement-info").hide();
    }
    else {
        $(".agreement-info").show();
    };


    $(".datetimepicker").datetimepicker({
        minDate: 0,
        format: 'd.m.Y',
        lang: 'uk',
        timepicker: false,
        dayOfWeekStart: 1
    }).removeAttr("data-val-date");
    
    $('[name=LegalActionTypeId]').change(function () {
        $('[name=NotLegalTypeActionId]').val(null);
    });

    $('[name=NotLegalTypeActionId]').change(function () {
        $('[name=LegalActionTypeId]').val(null);
    });

    $("#IsMultiTask").change(function () {
        if ($(this).is(':checked')) {
            $(".agreement-info").hide();

            if ($("#AssignedResponsible option:selected").text() !== 'Custom') {
                $("#Responsible").val("auto");
                $("#ResponsibleFullName").val("Automatic");
            }

            $("#ObserverFullName").attr("placeholder", "Automatic");

            $("#AgreementId").val('-1').trigger('change');
        }
        else {
            $(".agreement-info").show();

            if ($("#Responsible").val() === 'auto') {
                $("#Responsible").val("").trigger("change");
                $("#ResponsibleFullName").val("");
            }

            $("#ObserverFullName").removeAttr("placeholder");

            $("#AgreementId").val('-1').trigger('change');
        };
    });

    $("#isAutocloseAllowed").change(function () {
        if ($(this).is(':checked')) {
            $(".autoclose-panel").show();
        }
        else {
            $('[name=NotLegalTypeActionId]').val(null);
            $('[name=LegalActionTypeId]').val(null);

            $(".autoclose-panel").hide();
        };
    });
    
    $("#ResponsibleFullName").change(function () {        
        $("#AssignedResponsible").val(0);
    });

    $("#ResponsibleFullName").keyup(function (e) {
        if (isKeyPrintable(e)) {
            $("#Responsible").val("");
        }
    });

    $("#ObserverFullName").keyup(function (e) {
        if (isKeyPrintable(e)) {
            $("#Observer").val("");
        }
    });

    $("#AssignedResponsible").change(function () {
        var _agreement = $("#formCreateTask").data("agreement");

        $("#Responsible").val("");
        $("#ResponsibleFullName").val("");

        $("#Observer").val("");
        $("#ObserverFullName").val("");

        if (!$("#IsMultiTask").is(':checked')) {
            switch ($("#AssignedResponsible option:selected").text()) {
                case 'Custom':
                    break;
                case 'RS':
                    if (_agreement === null) break;
                    $("#Responsible").val(_agreement.AssignedCollector);
                    $("#ResponsibleFullName").val(_agreement.AssignedCollectorFullName);

                    if (!isEmpty($("#Responsible").val()))
                        fillObserverByResponsible();

                    break;
                case 'Field':
                    if (_agreement === null) break;
                    $("#Responsible").val(_agreement.AssignedFieldUser);
                    $("#ResponsibleFullName").val(_agreement.AssignedFieldUserFullName);

                    if (!isEmpty($("#Responsible").val()))
                        fillObserverByResponsible();

                    break;
                case 'Legal':
                    if (_agreement === null) break;
                    $("#Responsible").val(_agreement.AssignedLegalUser);
                    $("#ResponsibleFullName").val(_agreement.AssignedLegalUserFullName);

                    if (!isEmpty($("#Responsible").val()))
                        fillObserverByResponsible();

                    break;
            }
        }
        else {
            if ($("#AssignedResponsible option:selected").text() !== 'Custom') {
                $("#Responsible").val("auto");
                $("#ResponsibleFullName").val("Automatic");
            }
        }
    });

    $("#AgreementId").change(function () {
        $("#formCreateTask").data("agreement", null);
        $("#PersonFullName").val("");
        $("#ProductCode").val("");
        $("#OpenDate_CloseDate").val("");
        $("#Number").val("");

        var agreementId = $("#AgreementId").val().trim();

        var agreements = agreementId.split(";");

        if (!$("#IsMultiTask").is(':checked')) {
            agreementId = agreements[0];

            if (isNaN(agreementId) || isEmpty(agreementId) || agreementId === -1) {
                return;
            }

            $.ajax({
                type: "GET",
                url: app.utils.urlContent("/api/agreements"),
                dataType: "json",
                data: {
                    agreementId: agreementId
                },
                success: function (data) {
                    var agreement = data;

                    if (agreement !== null) {
                        $("#formCreateTask").data("agreement", agreement);

                        $("#PersonFullName").val(agreement.PersonFullName);
                        $("#ProductCode").val(agreement.ProductCode);

                        var openDate = $.format.date(new Date(agreement.OpenDate), 'dd.MM.yyyy');
                        var closeDate = $.format.date(new Date(agreement.CloseDate), 'dd.MM.yyyy');

                        $("#OpenDate_CloseDate").val(openDate + ' - ' + closeDate);
                        $("#Number").val(agreement.Number);


                        $("#AssignedResponsible").change();
                    }
                },
                error: function (result) {
                    console.log("Agreement not found");
                }
            });
        }
    });

    $("#TaskCategoryId").change(function () {
        if ($("#AssignedResponsible").val() === '0') {
            $("#Responsible").val("");
            $("#ResponsibleFullName").val("");
            $("#Observer").val("");
            $("#ObserverFullName").val("");
        }
    });

    $("#IsMultiTask").change(function () {
        if (this.checked) {
            $(".hiddenfile").empty();

            $(".hiddenfile").append("<iframe src=" + app.utils.urlContent("/Task/UploadFileWithAgreements") + " frameborder='0' style='height: 40px; width: 210px;' scrolling='no'></iframe>");
        } else {
            $(".hiddenfile").empty();
        }
    });


    function isEmpty(str) {
        return (!str || 0 === str.length);
    }

    function getForm() {
        return $('#formCreateTask').parent();
    }

    function downloadResultFile(data) {
        $("body").append("<a href='" + app.utils.urlContent("/task/GetTaskCreationResult?fileName=" + data) +
            "' class='resultRetriever' download=CreationResult_" + data +
            "></a>");
        $(".resultRetriever")[0].click();
        $(".resultRetriever").remove();
    };

    function isKeyPrintable(e) {
            var keycode = e.keyCode;

            var printable =
                ((keycode > 47 && keycode < 58) ||    // number keys
                keycode == 32 || /*keycode == 13 ||*/    // spacebar & return key(s) (if you want to allow carriage returns)
                (keycode > 64 && keycode < 91) ||    // letter keys
                (keycode > 95 && keycode < 112) ||   // numpad keys
                (keycode > 185 && keycode < 193) ||  // ;=,-./` (in order)
                    (keycode > 218 && keycode < 223)) && // [\]' (in order)
                !(e.ctrlKey || e.altKey);    // not pressed ctrl/alt combinations

            return printable;
    }

    function fillObserverByResponsible() {
        $.ajax({
            type: "GET",
            url: app.utils.urlContent("/api/users/GetBoss"),
            dataType: "json",
            data: {
                userLogin: $("#Responsible").val()
            },
            success: function (data) {
                var user = data;

                if (user !== null && $("#Responsible").val() !== "auto") {

                    if (!isEmpty(user.Login)) {
                        $("#Observer").val(user.Login);

                        if (isEmpty(user.FullName)) {
                            $("#ObserverFullName").val(user.Login);
                        }
                        else {
                            $("#ObserverFullName").val(user.Login + ' / ' + user.FullName);
                        }
                    }
                }
            },
            error: function (result) {
                console.log("Observer not found");
            }
        });
    }

    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _create: function () {
            this._super();
            this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
        },
        _renderMenu: function (ul, items) {
            var that = this,
                currentCategory = "";

            var isLegalTask = isEmpty($('#LegalActionTypeId').val());

            var itemsFiltered = !isLegalTask ? items : items.filter(function (item) { return !(item.category === "Ответственные по процессам договора") });

            $.each(itemsFiltered, function (index, item) {
                if (item.category !== currentCategory) {
                    ul.append("<li class='ui-autocomplete-category'><b>" + item.category + "</b></li>");
                    currentCategory = item.category;
                }
                var li = that._renderItemData(ul, item);
                if (item.category) {
                    li.attr("aria-label", item.category + " : " + item.label);
                }
            });
        }
    });


    $(".ui-autocomplete-input").css("width", "300px");


    $("#ResponsibleFullName").catcomplete({
        source: function (request, response) {
            $.ajax({
                url: app.utils.urlContent("/api/tasks/GetUsersWithResponsible"),
                dataType: "json",
                data: {
                    term: request.term,
                    agreementId: $("#AgreementId").val(),
                    departmentId: $("#TaskCategoryId").val()
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        open: function () {
            $("#ResponsibleFullName").catcomplete("widget").css("width", "300px");
        },
        select: function (e, ui) {
            $("#AssignedResponsible").val(0);

            var index = ui.item.value.indexOf("/");
            if (index === -1)
                return;

            $("#Responsible").val(ui.item.value.substring(index + 2));

            $("#Observer").val("");
            $("#ObserverFullName").val("");

            if (!isEmpty($("#Responsible").val())) {
                fillObserverByResponsible();
            }
        },
        delay: 300,
        minLength: 3
    });

    $("#ObserverFullName").autocomplete({
        source: app.utils.urlContent("/api/users"),
        open: function () {
            $("#ObserverFullName").autocomplete("widget").css("width", "300px");
        },
        select: function (e, ui) {
            var index = ui.item.value.indexOf("/");
            if (index === -1)
                return;
            $("#Observer").val(ui.item.value.substring(index + 2));
        },
        delay: 300,
        minLength: 3
    });


    $("#formCreateTask").submit(function (e) {

        if (!$("#formCreateTask").valid()) {
            return;
        }

        $("#btnCreateTask").attr("disabled", "disabled");

        var form = getForm();
        app.utils.ui.loader(form);

        if ($("#IsMultiTask").prop("checked")) { 

            $.ajax({
                type: "POST",
                url: app.utils.urlContent("/api/tasks/CreateMultipleTaskAndGetResult"),
                data: $("#formCreateTask").serialize(),
                success: function(data) {
                    app.utils.ui.loader(form, false);

                    downloadResultFile(data);

                    if ($("#isPartial").val() === "true") {
                        if (window.parent) {
                            window.parent.postMessage({ event: 'taskCreated', message: 'Задача создана' }, '*');
                        }

                        app.utils.ui.alert({ text: 'Задача создана' });

                        $("#btnCreateTask").removeAttr('disabled');
                    } else {
                        window.location.replace(app.utils.urlContent("/task/list"));
                    }
                },
                error: function(result) {
                    app.utils.ui.loader(form, false);

                    app.utils.ui.error({
                        text: result.responseJSON
                            ? (result.responseJSON.ExceptionMessage || result.responseJSON.Message)
                            : result.response
                    });

                    $("#btnCreateTask").removeAttr('disabled');
                }
            });
        } else {
            var data = $("#formCreateTask").serialize();
            $.ajax({
                type: "POST",
                url: app.utils.urlContent("/api/tasks"),
                data: data,
                success: function() {
                    app.utils.ui.loader(form, false);

                    if ($("#isPartial").val() === "true") {
                        if (window.parent) {
                            var parentHandlerEventName = 
                                app.utils.parseUrlParams(document.location.href).successParentHandler || 'taskCreated';                            
                            window.parent.postMessage({ event: parentHandlerEventName, message: 'Задача создана' }, '*');
                        }

                        app.utils.ui.alert({ text: 'Задача создана' });

                        $("#btnCreateTask").removeAttr('disabled');
                    } else {
                        window.location.replace(app.utils.urlContent("/task/list"));
                    }
                },
                error: function(result) {
                    app.utils.ui.loader(form, false);

                    app.utils.ui.error({
                        text: result.responseJSON
                            ? (result.responseJSON.ExceptionMessage || result.responseJSON.Message)
                            : result.response
                    });

                    $("#btnCreateTask").removeAttr('disabled');
                }
            });
        }


        e.preventDefault();
    });


    // Agreement block initialization
    $("#AgreementId").change();

});