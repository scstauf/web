var Peeker = {
    className: 'wnd-peeker',
    show: function (config) {
        // conditionally build the kendoWindow options
        var options = {
                modal: true,
                visible: false,
                deactivate: function () {
                    // remove from DOM
                    $('.' + Peeker.className).remove();
                }
            },
            content = {
                template: ''
            };

        if (!config) {
            throw new Error('Peeker.show required a config parameter which was not supplied.');
        }

        // set the dimensions
        if (config.width) options.width = config.width;
        if (config.height) options.height = config.height;
        
        if (!config.src || typeof config.src !== 'string' || config.src.length === 0) {
            throw new Error('Peeker.show required a src parameter which was not supplied.');
        }

        // peek with an iframe
        if (config.src.indexOf('http://') > -1 || config.src.indexOf('https://') > -1) {
            content.template =
                '<div class="peeker-content">\
                    <div class="peeker-iframe-container">\
                        <iframe src= "' + config.src + '" />\
                    </div>\
                </div>';
        }
        else {
            // assume it's html
            content.template = 
                '<div class="peeker-content">\
                    <div class="peeker-html-container">' + config.src + '</div>\
                </div>';
        }

        // wrap the template with a div
        content.template = content.template;

        // set the content
        options.content = content;

        // set the optional title
        if (config.title && typeof config.title === 'string' && config.title.length > 0) {
            options.title = config.title;
        }

        // display the window
        $('<div class="' + Peeker.className + '" />')
            .appendTo('body')
            .kendoWindow(options)
            .data('kendoWindow')
            .open()
            .center();

        return false;
    },
    close: function () {
        var peeker = $('.' + Peeker.className);
        // hide
        $(peeker).fadeOut();
        // call deactivate
        $(peeker).data('kendoWindow').close();
    }
};

document.addEventListener('DOMContentLoaded', function () {
    if (typeof jQuery === 'undefined' && typeof kendo === 'undefined') {
        console.error('Peeker requires jQuery and Kendo UI');
        return;
    }
    else {
        var css = '.wnd-peeker.k-window-content.k-content{text-align:center!important;}.peeker-iframe-container iframe{position:absolute;top:0;left:0;width:100%;height:100%;}';
        $('head').append('<style type="text/css">/* Peeker.js Styling */' + css + '</style>');
    }
});
