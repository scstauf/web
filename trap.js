/**
 * trap.js by sstauffer@oncochart.com
 */

if (typeof jQuery != 'undefined') {
    var Mouse = {
        down: {
            x: 0, y: 0
        },
        up: {
            x: 0, y: 0
        },
        isDown: false,
        waitForSelect: false,
        reset: function () {
            this.down.x = 0;
            this.down.y = 0;
            this.up.x = 0;
            this.up.y = 0;
            this.isDown = false;
        }
    };

    $(document).ready(function () {
        $('body').mousedown(function (e) {
            if (e.which == 1 && window.getSelection().toString().length == 0) {
                if (!Mouse.isDown) {
                    Mouse.down.x = e.pageX;
                    Mouse.down.y = e.pageY;
                    Mouse.isDown = true;
                }
            }
        });

        $('body').mouseup(function (e) {
            if (e.which == 1) {
                if (Mouse.isDown) {
                    if ((Mouse.waitForSelect && window.getSelection().toString().length > 0) || !Mouse.waitForSelect) {
                        Mouse.up.x = e.pageX;
                        Mouse.up.y = e.pageY;
                        Mouse.isDown = false;
                    } else {
                        Mouse.reset();
                    }
                }
            }
        });
    });
}

// a1f6ed979ad81d7b3ed45a5d64bd2c6f