/**
 * session_killer.js by scstauf@gmail.com
 *
 * The purpose of this script is to handle browser closing.  I bind to beforeunload and test if the user refreshed, clicked a link, or closed the browser.
 * This does not handle Ctrl+W/Ctrl+Shift+W
 *
 * Set $sessionKiller to some server-side action that terminates the session. 
 *
 * Known issues: if the user moves the mouse to the close button outside of the viewport, the script assumes it's a browser refresh.
 */

if (typeof jQuery != 'undefined') {
    var $sessionKiller = '/Settings/LogOut/0';
    var $cookie = 'bmsicmp';
    var $mouse = { x: 0, y: 0, _x: 0, _y: 0 };
    function setMouse(x, y, setCookie) {
        if (x > 0 && y > 0) {
            $mouse._x = $mouse.x;
            $mouse._y = $mouse.y;
            if (setCookie) document.cookie = $cookie + '=' + $mouse._x + ',' + $mouse._y;
        }
    }

    document.addEventListener('mousemove', function (e) {
        $mouse.x = e.clientX || e.pageX;
        $mouse.y = e.clientY || e.pageY;
        setMouse($mouse.x, $mouse.y, true);
    }, false);

    function getCookieByName(q) {
        var tokens = ('; ' + document.cookie).split('; ' + q + '=');
        return (tokens.length != 2) ? '' : tokens.pop().split(";").shift();
    }

    function endSession() {
        $.ajax({
            type: 'GET',
            url: $sessionKiller,
            dataType: 'json',
            data: {},
            async: false,
            success: function (data) { }
        });
    }

    $(window).on('load', function () {
        var coord = getCookieByName($cookie).split(',');
        setMouse(coord[0], coord[1], false);
    });

    $(window).bind('beforeunload', function () {
        // if you want to show the unload message, give value a non-empty value.
        var value = '';
        var target = document.activeElement;
        var unloadType = (target.tagName == 'BODY') ? (($mouse._x > $(window).width() - ($(window).width() * 0.15)) ? 2 : 1) : 0;

        switch (unloadType) {
            case 0:
                document.activeElement.blur();
                break;
            case 1:
                // page refresh
                break;
            case 2:
                endSession();
                break;
            default:
                // something else happened
                break;
        }

        return value;
    });
}

// scottyeatscode 2015