/**
 * browser_dectector.js by scstauf@gmail.com
 *
 * only works for IE, Firefox, Opera, Chrome, and Safari.
 */

var BrowserDetector = {
    getBrowserName: function() {
        var browserName = '';

        if (typeof window.clipboardData != 'undefined')
            browserName = 'ie';
        else if (typeof MozPowerManager != 'undefined')
            browserName = 'moz';
        else if (typeof opr != 'undefined')
            browserName = 'opera';
        else if (typeof chrome != 'undefined')
            browserName = 'chrome';
        else
            browserName = 'safari';

        return browserName;
    }
}

// a1f6ed979ad81d7b3ed45a5d64bd2c6f