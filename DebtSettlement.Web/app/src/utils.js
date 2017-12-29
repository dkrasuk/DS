; let app =  app || {};
app.utils = (function () {
    'use strict';
    let utils = {
        urlContent: function (url) {
            let baseHref = $('head base[href]');
            if (baseHref.length !== 0) {
                url = (baseHref.attr('href') === '/' ? '' : baseHref.attr('href')) + url;
            }
            return url;
        },
        parseUrlParams: function (url) {
            /// <summary>достать параметер з URL.</summary>
            if (typeof url === 'undefined' || url === null) {
                url = document.location.href;
            }
            url = url.substring(url.indexOf('?') + 1);
            let paramsArray = url.split("&");
            let result = {};
            for (let i = 0; i < paramsArray.length; i++) {
                let param = paramsArray[i].split("=");
                if (result[param[0]]) {
                    if (typeof result[param[0]] === 'string') {
                        result[param[0]] = [result[param[0]], param[1]];
                    } else {
                        result[param[0]].push(param[1]);
                    }
                } else {
                    result[param[0]] = param[1];
                }
            }
            return result;
        },
        htmlDecode: function (value) {
            return $('<div/>').html(value).text();
        },
        htmlEncode: function (value) {
            // Create a in-memory div, set its inner text (which jQuery automatically encodes)
            // Then grab the encoded contents back out. The div never exists on the page.
            return $('<div/>').text(value).html();
        },
        loadCss: function(url) {
            let link = document.createElement("link");
            link.type = "text/css";
            link.rel = "stylesheet";
            link.href = url;
            document.getElementsByTagName("head")[0].appendChild(link);
        },
        ui: {
            loader: function (element, enable, options) {
                if (typeof enable === 'undefined') {
                    enable = true;
                }
                let elem = $(element);
                let loader = elem.data('fog-loader');
                if (!loader) {
                    loader = $('<div id="" class="simple-fog-loader"></div>');
                    loader.appendTo(elem);
                }
                if (enable) {
                    options = $.extend({
                        appendTo: element,
                        modal: false,
                        message: 'Downloading',
                        width: 150,
                        animated: true,
                        style: 'progressbar',
                        progress: { value: false },
                        position: { my: "center", at: "center", of: element }
                    }, options);

                    loader.fogLoader(options);
                    elem.data('fog-loader', loader);
                } else {
                    loader = elem.data('fog-loader') || $('.simple-fog-loader');
                    try{
                        loader.fogLoader('close');
                        loader.fogLoader('destroy');
                    } catch(e) {}
                    elem.data('fog-loader', null);
                }
            }
        }
    };
    
    $.fn.loader = function (enable, options) {
        let $this = $(this);
        utils.ui.loader($(this), enable, options);
    };
    
    utils.ui.dialog = function (options) {
        let alertParams = $.extend({
            text: '',
            func: null,
            title: '',
            winType: '',
            width: '450',
            height: '150',
            modal: true,
            backdrop: 'static',
            keyboard: false
        }, options);

        let modalSizeClass = function (width) {
            if (width < 768) {
                return 'modal-sm';
            }
            if (width <= 992) {
                return 'modal-md';
            }
            if (width <= 1199) {
                return 'modal-lg';
            }
            return 'modal-xl';
        };

        let alert = $('<div class="modal fade" id="' + (alertParams.id || '') + '"  role="dialog">\
                        <div class="modal-dialog ' + modalSizeClass(alertParams.width) + '" style="' + alertParams.style + '" role="document">\
                          <div class="modal-content">\
                            <div class="modal-header">\
                              <button type="button" class="close" data-dismiss="modal">\
                                <span aria-hidden="true">&times;</span>\
                                <span class="sr-only">Close</span>\
                              </button>\
                              <h4 class="modal-title"></h4>\
                            </div>\
                            <div class="modal-body"></div>\
                          </div>\
                        </div>\
                     </div>');

        let buildModalButtons = function (buttons) {
            if (buttons) {
                let result = $('<div class="modal-footer">');
                for (let i = 0; i < buttons.length; i++) {
                    let buttonInArray = buttons[i];
                    let button = $('<button class="btn btn-default">');
                    button.html(buttonInArray.html || buttonInArray.text);
                    result.append(button);
                    if (buttonInArray['class']) {
                        button.addClass(buttonInArray['class']);
                    }
                    if (buttonInArray.click) {
                        button.data('click', buttonInArray.click);
                        button.on('click', function () { $(this).data('click').call(alert); });
                    }
                }
                return result;
            }
            return null;
        };
        
        if (alertParams.buttons) {
            alert.find('.modal-content').append(buildModalButtons(alertParams.buttons));
        }

        let bodyHtml;
        if (alertParams.content && !alertParams.content.url) {
            bodyHtml = $(alertParams.content);
        } else {

            if (alertParams.winType) {
                let img = '';
                let color = '';
                switch (alertParams.winType) {
                    case 'error':
                        img = 'remove-circle';
                        color = 'text-danger';
                        break;
                    case 'warning':
                        color = 'text-warning';
                        img = 'warning-sign';
                        break;
                    case 'confirm':
                        color = 'text-primary';
                        img = 'question-sign';
                        break;
                    case 'success':
                        color = 'text-success';
                        img = 'ok';
                        break;
                    case 'info':
                        color = 'text-primary';
                        img = 'info-sign';
                        break;
                    default:
                        break;
                }

                alertParams.text =
                '<div>\
                    <div style="position:relative;">\
                        <div style="position:absolute; top:0; left:0;" >\
                            <span style="font-size:28px;" class="' + color + ' glyphicon glyphicon-' + img + '"></span>\
                        </div>\
                        <div style="padding-left:40px;">' + alertParams.text + '\
                        </div>\
                    </div>\
                </div>';
            }

            bodyHtml = $(alertParams.text);
        }

        alert.find('.modal-title').html(alertParams.title);

        alert.find('.modal-body').html(bodyHtml);        

        alert.on('show.bs.modal', function () {
            if (alertParams.open) {
                alertParams.open.call(alert);
            }
        });
        alert.on('hidden.bs.modal', function () {
            if (alertParams.close) {
                alertParams.close.call(alert);
            }

            let t = alert.prevAll('.modal.fade.in');
            if (t.length > 0) {
                $('body').addClass('modal-open');
            }

            alert.remove();
        });

        alert.modal(alertParams);
        if (alertParams.url && alertParams.url !== '') {
            alert.find('.modal-body').load(alertParams.url, alertParams.onload);
        } else if (alertParams.content) {
            if (alertParams.content.url) {
                if (alertParams.content.isFrame){
                    let iframe = $(`<iframe id="createTaskModalFrame"
                                        src="` + alertParams.content.url + `"
                                        frameborder="0"
                                        style="width:100%; height: ` + (alertParams.content.height || '100%') + `"
                                        scrolling="yes">
                                            <p>Your browser does not support iframes.</p>
                                    </iframe>`);
                    alert.find('.modal-body').css('padding', '0').html(iframe);
                    iframe.on('load', function () {
                        //this.style.height =
                            //this.contentWindow.document.body.offsetHeight + 'px';
                        if (alertParams.content.onload || alertParams.onload) {
                            (alertParams.content.onload || alertParams.onload).call(iframe, alert);
                        }
                    });
                } else {
                    alert.find('.modal-body').load(alertParams.content.url, alertParams.content.onload || alertParams.onload);
                }
            }
        }
        alert.loader = function (trigger) {
            if (typeof trigger === 'undefined'){
                trigger = true;
            }
            utils.ui.loader(alert.find('.modal-body'), trigger, {modal: true})
        };
        return alert;



        /// quuery-ui dialog
        //'use strict';
        //var alertParams = $.extend(
        //    {
        //        autoOpen: false,
        //        id: 'utilsUiAlertDialog',
        //        text: '',
        //        actions: ["Close"],
        //        url: '',
        //        onload: null,
        //        title: '',
        //        winType: "",
        //        width: 400,
        //        height: 400,
        //        content: null,
        //        contentHtml: null,
        //        iframe: false,
        //        close: null,
        //        modal: true,
        //        //position: 'center',
        //        resizable: true,
        //        buttons: null,
        //        visible: false,
        //        pinned: false,
        //        draggable: true,
        //        appendTo: 'body'
        //    }, options);
        //var alert = $('<div id="' + alertParams.id + '"></div>');

        //if (alertParams.content) {
        //    $(alertParams.content).appendTo(alert);
        //} else {

        //    if (alertParams.winType) {
        //        var imgTemplate = '<span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>';
        //        var img = '';
        //        var color = '';
        //        switch (alertParams.winType) {
        //            case 'error':
        //                img = 'remove-circle';
        //                color = 'text-danger';
        //                break;
        //            case 'warning':
        //                color = 'text-warning';
        //                img = 'warning-sign';
        //                break;
        //            case 'confirm':
        //                color = 'text-primary';
        //                img = 'question-sign';
        //                break;
        //            case 'success':
        //                color = 'text-success';
        //                img = 'ok';
        //                break;
        //            case 'info':
        //                color = 'text-primary';
        //                img = 'info-sign';
        //                break;
        //            default:
        //                break;
        //        }

        //        alertParams.text =
        //            '<div>\
        //            <div style="position:relative;">\
        //                <div style="position:absolute; top:0; left:0;" >\
        //                    <span style="font-size:28px;" class="' + color + ' glyphicon glyphicon-' + img + '"></span>\
        //                </div>\
        //                <div style="padding-left:40px;">' + alertParams.text + '\
        //                </div>\
        //            </div>\
        //        </div>';
        //    }

        //    alert.html(alertParams.text);
        //}

        //alertParams.close = function () {
        //    if (options.close) {
        //        options.close.call(alert);
        //    }
        //    alert.remove();
        //}
        //alert.dialog(alertParams);
        //if (alertParams.url && alertParams.url !== '') {
        //    alert.load(alertParams.url, alertParams.onload);
        //}

        //alert.dialog('open');
        //return alert;
    };

    utils.ui.alert = function (options) {
        options = $.extend(
            {
                id: 'utilsUiAlertDialog',
                title: 'Сообщение!',
                winType: 'info',
                width: 400,
                height: 220,
                buttons: [
                    {
                        html: '<span class="k-icon k-i-tick"></span> Ok',
                        click: function () { $(this).modal('hide'); },
                        'class': 'btn btn-default'
                    }]
            }, options);
        return this.dialog(options);
    };
    utils.ui.error = function (options) {
        options = $.extend(
            {
                id: 'utilsUiErrorDialog',
                title: 'Ошибка!',
                winType: 'error'
            }, options);
        return this.alert(options);
    };
    utils.ui.success = function (options) {
        return this.alert(options);
    };

    utils.ui.prompt = function (options, func) {
        if (typeof options === 'string') {
            options = { text: options, func: func }
        }
        if (func === undefined) {
            func = options.func;
        }

        let input = '<div><input id="utilsUiPromptDialogInput" /></div>';
        if (options.inputType === 'date') {
            let oldOpen = options.open;
            options.open = function () {
                $(this).find('#utilsUiPromptDialogInput')
                    .datetimepicker({
                        format: 'd/m/Y',
                        lang: 'uk',
                        timepicker: false,
                        dayOfWeekStart: 1
                    })
                    .mask("99/99/9999", { placeholder: "__/__/____" })
                    .removeAttr("data-val-date");
                if (oldOpen) {
                    oldOpen.call(this);
                }
            }
        } else if (options.inputType === 'textarea') {
            input = '<div><textarea style="width:100%" id="utilsUiPromptDialogInput"></textarea></div>';
        }

        options.text = options.text += input;
        let defaultOptions = $.extend(
            {
                id: 'utilsUiPromptDialog',
                title: 'Ввведите значение',
                winType: 'confirm',
                width: 400,
                height: 250,
                buttons: [
                    {
                        text: 'Отменить',
                        click: function () { $(this).modal('hide'); },
                        'class': 'btn btn-default'
                    }, {
                        html: '<span class="text-success glyphicon glyphicon-ok"></span> Ok',
                        click: function () {
                            if (func) {
                                func.call(this, $('#utilsUiPromptDialogInput').val());
                            }
                            $(this).modal('hide');
                        },
                        'class': 'btn btn-default'
                    }
                ]
            }, options);
        return this.confirm(defaultOptions);
    };

    utils.ui.confirm = function (options, func) {
        //для того щоб можна було func передавати в options
        if (func === undefined) {
            func = options.func;
        }
        options = $.extend(
            {
                id: 'utilsUiConfirmDialog',
                title: 'Подтверждение!',
                winType: 'confirm',
                width: 400,
                height: 250,
                buttons: [
                    {
                        text: 'Отменить',
                        click: function () { $(this).modal('hide'); },
                        'class': 'btn btn-default'
                    }, {
                        html: '<span class="text-success glyphicon glyphicon-ok"></span> Ok',
                        click: function () {
                            var funcResult;
                            if (func) {
                                funcResult = func.call(this);
                            }
                            if (funcResult !== false) {
                                $(this).modal('hide');
                            }
                        },
                        'class': 'btn btn-default'
                    }
                ]
            }, options);
        return this.alert(options);
    };

    utils.ui.notify = function (title, text, type, settings) {
        let popup = $('#notifyPopup');
        if (popup.length === 0) {
            popup = $('<div />', { id: 'notifyPopup', zindex: 99999 });
            popup.append('body');
        }
        let options = {
            title: title,
            message: text
        };
        settings = $.extend({
            type: (type === 'error' ? 'danger' : type) || 'info',
            z_index: 999999
        }, settings);

        return $.notify(options, settings);
    
    };
    
    utils.dataTable = {
        bindGridButtons: function (mainGrid) {
            let input = $('<input type="text" />').keyup(function (e) {
                if (e.keyCode === 13) {
                    mainGrid.fnFilter(this.value);
                }
            });
            let html = $(`<div class="input-append" style="display: inline-block">
                        <button class="btn btn-default" type="button">
                            <span class="glyphicon glyphicon-search"></span>
                        </button>
                    </div>`);
            html.prepend(input);
            html.find('button').on('click', function () {
                mainGrid.fnFilter(input.val());
            });

            let tableWrapper = $(mainGrid[0]).parentsUntil('.dataTables_wrapper').parent();
            let oldSearch = tableWrapper.find('div.dataTables_filter input');
            input.addClass(oldSearch.attr('class'));
            oldSearch.css('display', 'none').after(html);
            $(`<button class="btn btn-default btn-sm refresh-task-table-btn" title="Refresh" >
                    <span class="glyphicon glyphicon-refresh"></span>
                </button>`)
                .on('click', function () {
                    mainGrid.fnDraw();//.ajax.reload();
                })
                .prependTo(tableWrapper.find('.dataTables_length'));

        }
    };
    
    
    return utils;
}());

let utils = app.utils;
export default utils;