/*
* MessageBox.js by scottyeatscode
* requires jQuery + Kendo UI
*/

var MessageBox = {
    className: 'wnd-message-box',
    buttonGroupClassName: 'message-box-button-group',
    show: function (msg, title, buttons) {
        // conditionally build the kendoWindow options
        var options = {
                modal: true,
                visible: false,
                content: {
                    template: msg
                },
                deactivate: function () {
                    // remove from DOM
                    $('.' + MessageBox.className).remove();
                }
            },
            buttonsAdded = [];

        if (title && title.length > 0) {
            options.title = title;
        }

        // generate any buttons passed to this function
        if (buttons && buttons.length > 0) {
            // [{ id: 'buttonid', text: 'buttontext', click: 'event()', className: 'buttonclass' }]
            var buttonHtml = '',
                thisButton = '',
                button = {},
                noClassSet = true;

            for (var i = 0; i < buttons.length; i++) {
                button = buttons[i];
                thisButton = '<input type="button" ';
                
                for (var attr in button) {
                    var attrValue = button[attr];

                    switch (attr) {
                        case 'id':
                            thisButton += 'id="' + attrValue + '" ';
                            // push id so we can call kendoButton() on them
                            buttonsAdded.push(attrValue);
                            break;

                        case 'text':
                            thisButton += 'value="' + attrValue + '" ';
                            break;

                        case 'click':
                            thisButton += 'onclick="' + attrValue + '" ';
                            break;

                        case 'className':
                            // add k-button class if it was not supplied
                            if (attrValue.indexOf('button') < 0) {
                                attrValue += ' button';
                            }

                            thisButton += 'class="' + attrValue + '" ';
                            noClassSet = false;
                            break;

                        case 'style':
                            // inline CSS
                            thisButton += 'style="' + attrValue + '" ';
                            break;
                    }
                }

                // no class was added, set default class
                if (noClassSet) {
                    thisButton += 'class="button" ';
                }

                thisButton += '/>';
                buttonHtml += thisButton;
            }

            options.content.template += '<div style="text-align: center; margin-top: 10px;" class="' + MessageBox.buttonGroupClassName + '">' + buttonHtml + '</div>';
        }

        $('<div class="' + MessageBox.className + '" />')
            .appendTo('body')
            .kendoWindow(options)
            .data('kendoWindow')
            .open()
            .center();

        //// call kendoButton() on any buttons that were passed to this function
        //if (buttonsAdded.length > 0) {
        //    for (var i = 0; i < buttonsAdded.length; i++) {
        //        var buttonId = buttonsAdded[i];
        //        $('#' + buttonId).kendoButton();
        //    }
        //}

        return false;
    },
    close: function () {
        var messageBox = $('.' + MessageBox.className);
        // hide
        $(messageBox).fadeOut();
        // call deactivate
        $(messageBox).data('kendoWindow').close();
    }
};

document.addEventListener('DOMContentLoaded', function () {
    if (typeof jQuery === 'undefined' && typeof kendo === 'undefined') {
        console.error('MessageBox requires jQuery and Kendo UI');
        return;
    }
    else {
        var css = '.wnd-message-box.k-window-content.k-content{text-align:center!important;}';
        $('head').append('<style type="text/css">/* MessageBox.js Styling */' + css + '</style>');
    }
});
