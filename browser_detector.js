/**
 * browser_dectector.js by scstauf@gmail.com
 *
 * only works for Edge, IE, Opera, Chrome, Firefox, and Safari.
 */

var BrowserDetector = {
    getBrowserName: function () {
        var ua = navigator.userAgent,
            name = 'unknown';	
        
        if (ua.indexOf('Edge/') > -1) 
            name = 'edge';
        else if (ua.indexOf('Trident/') > -1)
            name = 'ie';
        else if (ua.indexOf('OPR/') > -1)
            name = 'opera';
        else if (ua.indexOf('Chrome/') > -1)
            name = 'chrome';
        else if (ua.indexOf('Firefox/') > -1)
            name = 'moz';
        else if (ua.indexOf('Safari/') > -1)
            name = 'safari';
        else 
            console.error('could not determine browser from user agent');

        return name;
    },
    inUrl: function (test) {
        return location.search.toLowerCase().indexOf(test) > -1;
    },
    getQueryStringValue: function (qs) {
        var ret = '', search = qs.toLowerCase();

        if (this.inUrl(search)) {
            var tokens = location.search.split('&').map(function (i) { return i.toLowerCase(); });
            for (var i = 0; i < tokens.length; i++) {
                if (tokens[i].indexOf(search) > -1 && tokens[i].indexOf('=') > -1) {
                    var value = tokens[i].split('=')[1];
                    ret = value;
                    break;
                }
            }
        }

        return ret;
    }
};

// a1f6ed979ad81d7b3ed45a5d64bd2c6f
