$(function () {
    $("#agreementSearchTable").on("draw.dt", function () {
        $("#tablePanel").height($("#agreementSearchTable > tbody").height() + 150);
        $("#agreementSearchTable").height($("#agreementSearchTable > tbody").height() + 60);
    });

    $("#searchAgreementBox").keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            $("#btnStartSearch").click();
        }
    });

    function getSearchUrl() {
        return app.utils.urlContent("/api/agreements/search?searchTerm=" + $("#searchAgreementBox").val());
    };

    $("#btnStartSearch").click(function () {

        $("#tablePanel").removeClass("hidden");

        if ($.fn.DataTable.isDataTable("#agreementSearchTable")) {
            $("#agreementSearchTable").DataTable().ajax.url(getSearchUrl()).load();
            return;
        }

        $("#agreementSearchTable").dataTable({
            columns: [
                { data: "AgreementId", name: "AgreementId", title: "Id" },
                { data: "AgreementNumber", name: "AgreementNumber", title: "Номер" },
                { data: "LastName", name: "LastName", title: "Фамилия" },
                { data: "FirstName", name: "FirstName", title: "Имя" },
                { data: "Patronymic", name: "Patronymic", title: "Отчество" },
                { data: "ProductCode", name: "ProductCode", title: "Код продукта" },
                { data: "Inn", name: "Inn", title: "ИНН" },
                { data: "Dpd", name: "Dpd", title: "Дней просрочки" },
                { data: "Outstanding", name: "Outstanding", title: "Сумма просрочки" },
                { data: "Currency", name: "Currency", title: "Валюта" }
            ],
            "ajax": {
                "url": app.utils.urlContent("/api/agreements/search?searchTerm=" + $("#searchAgreementBox").val()),
                dataSrc: ""
            },
            language: {
                "processing": "Loading ...",
                "search": "Поиск:",
                "lengthMenu": "Показать _MENU_ записей",
                "info": "Записи с _START_ до _END_ из _TOTAL_ записей",
                "infoEmpty": "Записи с 0 до 0 из 0 записей",
                "infoFiltered": "(отфильтровано из _MAX_ записей)",
                "infoPostFix": "",
                "loadingRecords": "Загрузка записей...",
                "zeroRecords": "Записи отсутствуют.",
                "emptyTable": "В таблице отсутствуют данные",
                "paginate": {
                    "first": "Первая",
                    "previous": "Предыдущая",
                    "next": "Следующая",
                    "last": "Последняя"
                },
                "aria": {
                    "sortAscending": ": активировать для сортировки столбца по возрастанию",
                    "sortDescending": ": активировать для сортировки столбца по убыванию"
                }
            },
            initComplete: function() {
                
                $("#agreementSearchTable tbody").on("click", "tr", function () {
                    var table = $("#agreementSearchTable").DataTable();

                    var data = table.row($(this)).data();

                    $("#AgreementId").val(data["AgreementId"]).change();

                    $("#modalSearchAgreement").modal("toggle");
                });

            },
            "bDestroy": true
        });

    });
});